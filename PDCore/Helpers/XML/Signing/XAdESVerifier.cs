
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace PDCore.Helpers.XML.Signing
{
    public class XAdESVerifier
    {
        private static readonly Regex pattern = new Regex("_(.+?)_", RegexOptions.Compiled);

        private X509Certificate2 _cert;
        private SignedXml _xml;

        public byte[] Content
        {
            get
            {
                if (_xml == null)
                {
                    throw new NullReferenceException("Xml not loaded");
                }

                var id = _xml.Signature.Id;
                var match = pattern.Match(id);
                if (!match.Success)
                {
                    throw new FormatException("Signature id");
                }

                var objId = match.Groups[1].Value;

                foreach (DataObject dataObject in _xml.Signature.ObjectList)
                {
                    if (dataObject.Id == null ||
                        !dataObject.Id.Contains(objId))
                    {
                        continue;
                    }

                    return Convert.FromBase64String(dataObject.Data.Item(0).InnerText);
                }

                throw new FormatException("No objects embedded");
            }
        }

        public X509Certificate2 Certificate
        {
            get { return _cert; }
        }

        public void Load(string filePath)
        {
            var doc = new XmlDocument { PreserveWhitespace = true };
            doc.Load(filePath);

            Load(doc);
        }

        public void LoadXml(string xml)
        {
            var doc = new XmlDocument { PreserveWhitespace = true };
            doc.LoadXml(xml);

            Load(doc);
        }

        private void Load(XmlDocument doc)
        {
            if (doc.DocumentElement == null)
            {
                throw new NullReferenceException("Document root");
            }

            _cert = ExtractCertificate(doc);

            _xml = new SignedXml(doc);
            _xml.SigningKey = new RSACryptoServiceProvider(1024);
            _xml.LoadXml(doc.DocumentElement);
        }

        private X509Certificate2 ExtractCertificate(XmlDocument doc)
        {
            var namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            var node = doc.SelectSingleNode("/ds:Signature/ds:KeyInfo/ds:X509Data/ds:X509Certificate", namespaceManager);
            if (node == null)
            {
                throw new FormatException("No certificate");
            }

            return new X509Certificate2(Convert.FromBase64String(node.InnerText));
        }

        public bool Verify(bool verifySIgnatureOnly = false)
        {
            return _xml.CheckSignature(_cert, verifySIgnatureOnly);
        }

        public void CheckRoot(string thumbprint)
        {
            var chain = new X509Chain();
            var result = chain.Build(Certificate);
            if (!result)
            {
                throw new Exception("Unable to build certificate chain");
            }

            if (chain.ChainElements.Count == 0)
            {
                throw new Exception("Certificate chain length is 0");
            }

            if (
                StringComparer.OrdinalIgnoreCase.Compare(chain.ChainElements[chain.ChainElements.Count - 1].Certificate.Thumbprint,
                                                         thumbprint) != 0)
            {
                throw new Exception("Root certificate thumbprint mismatch");
            }
        }
    }
}
