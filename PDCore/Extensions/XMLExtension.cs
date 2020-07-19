using Newtonsoft.Json;
using PDCore.Writers;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
            xmlDocument.DeserializeFromXML(out T o);

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

        /// <summary>
        /// Utworzenie ExpandoObject dla zadanego dokumentu XML
        /// </summary>
        /// <param name="document">Dokument XML, na podstawie którego zostanie utworzony ExpandoObject</param>
        /// <returns>ExpandoObject utworzony na podstawie zadanego dokumentu XML</returns>
        public static dynamic AsExpando(this XDocument document)
        {
            return CreateExpando(document.Root); //Utworzenie ExpandoObject dla elementu najwyższego w hierarchi w zadanym dokumencie XML
        }

        /// <summary>
        /// Utworzenie ExpandoObject dla zadanego elementu XML
        /// </summary>
        /// <param name="element">Element XML, na podstawie którego zostanie utworzony ExpandoObject</param>
        /// <returns>ExpandoObject utworzony na podstawie zadanego elementu XML</returns>
        private static dynamic CreateExpando(XElement element)
        {
            //Utworzenie instancji klasy ExpandoObject i zrzutowanie jej na interfejs IDictionary w którym kluczem jest string, a wartością object
            var result = new ExpandoObject() as IDictionary<string, object>;

            //Sprawdzenie czy jakikolwiek z elementów podrzędnych przekazanego elementu posiada podelementy - czy jakikolwiek element (węzeł) nie jest liściem
            if (element.Elements().Any(e => e.HasElements))
            {
                var list = new List<ExpandoObject>(); //Utworzenie listy obiektów ExpandoObject

                result.Add(element.Name.ToString(), list); //Dodanie do słownika nazwy przekazanego elementu i listy ExpandoObject

                foreach (var childElement in element.Elements()) //Przejście po każdym dziecku przekazanego elementu
                {
                    //Dodanie do listy obiektu ExpandoObject utworzonego na podstawie danego podelementu przekazanego elementu
                    list.Add(CreateExpando(childElement));
                }
            }
            else //Wszystkie elementy podrzędne przekazanego elementu są liśćmi - nie posiadają podelementów
            {
                foreach (var leafElement in element.Elements()) //Przejście po każdym podelemencie (każdy jest liściem) przekazanego elementu
                {
                    result.Add(leafElement.Name.ToString(), leafElement.Value); //Dodanie do słownika nazwy elementu i jego wartości
                }
            }

            return result; //Zwrócenie utworzonego ExpandoObject (słownika)
        }

        public static dynamic ToExpando(this XDocument xDocument)
        {
            string jsonText = JsonConvert.SerializeXNode(xDocument);

            dynamic dyn = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);

            return dyn;
        }
    }
}
