using PDCore.Writers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PDCore.Extensions
{
    public static class XMLExtension
    {
        public static bool IsSerializable(Type t)
        {
            return t.IsSerializable && !(typeof(ISerializable).IsAssignableFrom(t));
        }

        public static string SerializeToXml<T>(this T o, bool deleteNamespaces = true) where T : class, new()
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));

            string xml = null;

            using (var sww = new Utf8StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    if (deleteNamespaces)
                    {
                        var ns = new XmlSerializerNamespaces();
                        ns.Add("", "");

                        xsSubmit.Serialize(writer, o, ns);
                    }
                    else
                    {
                        xsSubmit.Serialize(writer, o);
                    }

                    xml = sww.ToString();
                }
            }

            return xml;
        }

        public static void DeserializeFromXML<T>(this string xmlDocument, out T o) where T : class, new()
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(xmlDocument))
            {
                using (XmlReader reader = XmlReader.Create(sr))
                {
                    o = (T)xs.Deserialize(reader);
                }
            }
        }

        public static T DeserializeFromXML<T>(this string xmlDocument) where T : class, new()
        {
            T o = null;

            xmlDocument.DeserializeFromXML(out o);

            return o;
        }

        public static T DeserializeFromXML<T>(this XElement xElement) where T : class, new()
        {
            return DeserializeFromXML<T>(xElement.ToString());
        }

        public static string ToXml(this XDocument xDoc, bool deleteWhitespaces = true)
        {
            StringBuilder builder = new StringBuilder();

            using (TextWriter writer = new Utf8StringWriter(builder))
            {
                xDoc.Save(writer, deleteWhitespaces ? SaveOptions.DisableFormatting : SaveOptions.None);

                return builder.ToString();
            }
        }

        public static string ToXml(this XmlDocument xmlDocument)
        {
            StringBuilder builder = new StringBuilder();

            using (TextWriter writer = new Utf8StringWriter(builder))
            {
                xmlDocument.Save(writer);

                return builder.ToString();
            }
        }

        public static XElement GetFirstDescendantElement(this XElement xElement, string attributeName, string attributeValue)
        {
            return xElement.Descendants().First(x => x.HasAttributes && x.FirstAttribute.Name == attributeName && x.FirstAttribute.Value == attributeValue);
        }

        public static string GetAttributeValue(this XElement xElement, string name, bool required = true)
        {
            XAttribute xAttribute = xElement.Attribute(name);

            if (xAttribute == null)
            {
                if (required)
                {
                    throw new Exception(string.Format("Nie znaleziono atrybutu o nazwie \"{0}\" dla elementu: {1}{1}{2}", name, Environment.NewLine, xElement.ToString()));
                }

                return null;
            }
            else
            {
                return xAttribute.Value;
            }
        }
    }
}
