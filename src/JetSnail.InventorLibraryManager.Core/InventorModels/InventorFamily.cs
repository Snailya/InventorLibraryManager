using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "Family")]
    public class InventorFamily
    {
        [XmlAttribute(AttributeName = "DataType")]
        public string DataType;

        [XmlAttribute(AttributeName = "Description")]
        public string Description;

        [XmlAttribute(AttributeName = "DisplayName")]
        public string DisplayName;

        [XmlAttribute(AttributeName = "FromSource")]
        public string FromSource;

        [XmlAttribute(AttributeName = "HealthStatus")]
        public int HealthStatus;

        [XmlAttribute(AttributeName = "InternalName")]
        public string InternalName;

        [XmlAttribute(AttributeName = "LastModifiedTime")]
        public string LastModifiedTime;

        [XmlAttribute(AttributeName = "Library")]
        public string Library;

        [XmlAttribute(AttributeName = "Manufacturer")]
        public string Manufacturer;

        [XmlAttribute(AttributeName = "RevisionId")]
        public string RevisionId;

        [XmlAttribute(AttributeName = "SourceMoniker")]
        public string SourceMoniker;

        [XmlAttribute(AttributeName = "SourceRevisionId")]
        public string SourceRevisionId;

        [XmlAttribute(AttributeName = "SrcActive")]
        public string SrcActive;

        [XmlAttribute(AttributeName = "StandardsOrg")]
        public string StandardsOrg;
    }
}