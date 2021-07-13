using System.IO;
using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    public static class XmlExtension
    {
        public static T ToObject<T>(this string xmlString) where T : class
        {
            var serial = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xmlString);
            return (T)serial.Deserialize(reader);
        }

        public static string ToXmlString<T>(this T obj) where T : class
        {
            using var writer = new StringWriter();
            var serial = new XmlSerializer(typeof(T));
            serial.Serialize(writer, obj);
            return writer.ToString();
        }
    }
}