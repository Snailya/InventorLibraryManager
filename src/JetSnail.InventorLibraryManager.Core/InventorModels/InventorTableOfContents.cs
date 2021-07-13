using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "TableOfContents")]
    public class InventorTableOfContents
    {
        [XmlElement(ElementName = "Libraries")]
        public InventorLibraries Libraries;

        [XmlElement(ElementName = "Families")] public InventorFamilies Families;

        [XmlElement(ElementName = "Categories")]
        public InventorCategories Categories;


        [XmlAttribute(AttributeName = "SchemaVersion")]
        public string SchemaVersion;
    }
}