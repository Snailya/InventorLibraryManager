using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "FamilyReference")]
    public class InventorFamilyReference
    {
        [XmlAttribute(AttributeName = "DataTypeCategory")]
        public string DataTypeCategory;

        [XmlAttribute(AttributeName = "InternalName")]
        public string InternalName;
    }
}