using FirmaXadesNet;
using FirmaXadesNet.Crypto;
using FirmaXadesNet.Signature.Parameters;
using Microsoft.Xades;
using PDCore.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Xsl;

namespace PDCore.Utils
{
    public static class XMLUtils
    {
        public static string Transform(Stream stylesheet, string xmlDocument, XsltArgumentList xsltArgumentList = null)
        {
            XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();

            XsltSettings xsltSettings = new XsltSettings(true, false);


            using (XmlReader xmlReader = XmlReader.Create(stylesheet))
            {
                xslCompiledTransform.Load(xmlReader, xsltSettings, new XmlUrlResolver());
            }


            StringBuilder stringBuilder = new StringBuilder();


            using (StringReader stringReader = new StringReader(xmlDocument))
            {
                using (XmlReader xmlReader = XmlReader.Create(stringReader))
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xslCompiledTransform.OutputSettings))
                    {
                        xslCompiledTransform.Transform(xmlReader, xsltArgumentList, xmlWriter);
                    }
                }
            }

            return stringBuilder.ToString();
        }

        public static XmlDocument FirmarXML(string xmlDoc, X509Certificate2 myCert)
        {
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.LoadXml(xmlDoc);


            return FirmarXML(xmlDocument, myCert);
        }

        public static void Validate(string xmlDoc, string schemaNamespace, string schemaUrl)
        {
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.LoadXml(xmlDoc);


            var schemaSet = new XmlSchemaSet();

            schemaSet.Add(schemaNamespace, schemaUrl);


            xmlDocument.Schemas = schemaSet;


            ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

            // the following call to Validate succeeds.
            xmlDocument.Validate(eventHandler);
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Trace.TraceInformation("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Trace.TraceInformation("Warning {0}", e.Message);
                    break;
            }
        }

        public static string XadesSign(string xml, X509Certificate2 x509Certificate)
        {
            XadesService xadesService = new XadesService();

            SignatureParameters parametros = new SignatureParameters
            {
                SignaturePolicyInfo = new SignaturePolicyInfo(),
                SignaturePackaging = SignaturePackaging.ENVELOPED,
                InputMimeType = "text/xml"
            };


            using (parametros.Signer = new Signer(x509Certificate))
            {
                using (MemoryStream fs = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    var docFirmado = xadesService.Sign(fs, parametros);

                    return docFirmado.Document.ToXml();
                }
            }
        }

        public static XmlDocument FirmarXML(XmlDocument xmlDoc, X509Certificate2 myCert)
        {
            try
            {
                if (myCert != null)
                {
                    RSA rsaKey = ((RSA)myCert.PrivateKey);

                    // Sign the XML document. 
                    SignXml(xmlDoc, rsaKey, myCert);
                }

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            return xmlDoc;
        }


        // Sign an XML file. 
        // This document cannot be verified unless the verifying 
        // code has the key with which it was signed.
        public static void SignXml(XmlDocument xmlDoc, RSA Key, X509Certificate2 myCert)
        {
            // Check arguments.
            if (xmlDoc == null)
                throw new ArgumentException("xmlDoc");

            if (Key == null)
                throw new ArgumentException("Key");

            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(xmlDoc)
            {

                // Add the key to the SignedXml document.
                SigningKey = Key
            };

            KeyInfo keyInfo = new KeyInfo();

            KeyInfoX509Data keyInfoData = new KeyInfoX509Data(myCert);

            keyInfo.AddClause(keyInfoData);

            signedXml.KeyInfo = keyInfo;

            // Create a reference to be signed.
            Reference reference = new Reference
            {
                Uri = ""
            };

            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();

            reference.AddTransform(env);


            // Add the reference to the SignedXml object.
            signedXml.AddReference(reference);

            // Compute the signature.
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            // Append the element to the XML document.
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
        }

        public static string Sign(string xml, X509Certificate2 x509)
        {
            // Wczytaj.
            XmlDocument doc = new XmlDocument
            {
                PreserveWhitespace = true
            };
            doc.LoadXml(xml);

            // SignedXml object
            XadesSignedXml signedXml = new XadesSignedXml(doc);

            signedXml.Signature.Id = "ID-1234";
            signedXml.SigningKey = x509.PrivateKey;
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigCanonicalizationUrl;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

            // dodaj referencję na dokument
            Reference reference = new Reference("#Dokument");
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            signedXml.AddReference(reference);

            // dodaj KeyInfo
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(x509)); // ??? WholeChain ???
            signedXml.KeyInfo = keyInfo;

            //
            XadesObject xo = new XadesObject();
            {
                Cert cert = new Cert();

                cert.IssuerSerial.X509IssuerName = x509.IssuerName.Name;
                cert.IssuerSerial.X509SerialNumber = x509.SerialNumber;

                {
                    SHA1 cryptoServiceProvider = new SHA1CryptoServiceProvider();
                    cert.CertDigest.DigestValue = cryptoServiceProvider.ComputeHash(x509.RawData);
                    cert.CertDigest.DigestMethod.Algorithm = SignedXml.XmlDsigSHA1Url;
                }

                xo.QualifyingProperties.Target = "#" + signedXml.Signature.Id;
                xo.QualifyingProperties.SignedProperties.SignedSignatureProperties.SigningTime = DateTime.Now;
                xo.QualifyingProperties.SignedProperties.SignedSignatureProperties.SignaturePolicyIdentifier.SignaturePolicyImplied = true;

                xo.QualifyingProperties.SignedProperties.SignedSignatureProperties.SigningCertificate.CertCollection.Add(cert);

                DataObjectFormat dof = new DataObjectFormat
                {
                    ObjectReferenceAttribute = "#Dokument",
                    Description = "Dokument w formacie xml [XML]",
                    Encoding = SignedXml.XmlDsigBase64TransformUrl, // ...xmldsig/#base64
                    MimeType = "text/plain"
                };
                xo.QualifyingProperties.SignedProperties.SignedDataObjectProperties.DataObjectFormatCollection.Add(dof);
            }
            signedXml.AddXadesObject(xo);

            //// W dokumentacji 2.9.9.a, Id dla <ds:Object> ma mieć wartość "Dokument", ale nie ma tego w przykładach
            var data = new DataObject("Dokument", "text/xml", "", doc.DocumentElement);
            signedXml.AddObject(data);

            // Podpisz
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();


            return xmlDigitalSignature.OuterXml;
        }

        public static void SaveCertificateBySoapMessage(string filePath, string saveFilePath)
        {
            X509Certificate2 x509Certificate = ExtractCertificate(filePath);

            byte[] certificateBuffer = x509Certificate.Export(X509ContentType.Cert);


            File.WriteAllBytes(saveFilePath, certificateBuffer);
        }

        public static XmlDocument LoadXmlDocument(string filePath)
        {
            var doc = new XmlDocument { PreserveWhitespace = true };

            doc.Load(filePath);


            return doc;
        }

        public static X509Certificate2 ExtractCertificate(string filePath)
        {
            var xmlDocument = LoadXmlDocument(filePath);

            return ExtractCertificate(xmlDocument);
        }

        public static X509Certificate2 ExtractCertificate(XmlDocument doc)
        {
            var namespaceManager = new XmlNamespaceManager(doc.NameTable);

            namespaceManager.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            namespaceManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");

            var node = doc.SelectSingleNode("soap:Envelope/soap:Header/wsse:Security/wsse:BinarySecurityToken", namespaceManager);

            if (node == null)
            {
                throw new FormatException("No certificate");
            }

            return new X509Certificate2(Convert.FromBase64String(node.InnerText));
        }

        public static XElement ParseToXElement(string xml, XNamespace xNamespace)
        {
            XElement xElement = XElement.Parse(xml);

            xElement.Name = xNamespace + xElement.Name.LocalName;


            foreach(XElement element in xElement.Descendants())
            {
                element.Name = xNamespace + element.Name.LocalName;
            }

            return xElement;
        }
    }
}
