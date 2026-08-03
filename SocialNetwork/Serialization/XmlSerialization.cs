using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SocialNetwork.Serialization
{
    public class XmlSerialization
    {

        public static string SerializeToXML<T>(T obj, XmlRootAttribute root)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false

            }; XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer serializer = new XmlSerializer(typeof(T), root);

            using StringWriter writer = new StringWriter();
            using XmlWriter xmlWritter = XmlWriter.Create(writer, settings);

            serializer.Serialize(xmlWritter, obj, namespaces);

            return writer.ToString();


        }
        public static T DeserializeToObject<T>(string xml, XmlRootAttribute root)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(T), root);

            using StringReader reader = new StringReader(xml);

            return (T)serializer.Deserialize(reader)!;




        }
    }
}
