using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "Column")]
    public class InventorColumn
    {
        [XmlAttribute(AttributeName = "ColumnId")]
        public string ColumnId;

        [XmlAttribute(AttributeName = "ColumnwideValue")]
        public string ColumnwideValue;

        [XmlAttribute(AttributeName = "Custom")]
        public bool Custom;

        [XmlAttribute(AttributeName = "Hidden")]
        public bool Hidden;

        [XmlAttribute(AttributeName = "KeyWeight")]
        public int KeyWeight;

        [XmlAttribute(AttributeName = "Title")]
        public string Title;

        [XmlAttribute(AttributeName = "UnitType")]
        public string UnitType;

        [XmlAttribute(AttributeName = "ValueType")]
        public string ValueType;
    }
}