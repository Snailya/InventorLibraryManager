using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "Library")]
    public class InventorLibrary
    {
        [XmlAttribute(AttributeName = "AttachName")]
        public string AttachName;

        [XmlAttribute(AttributeName = "DefaultLanguage")]
        public string DefaultLanguage;

        [XmlAttribute(AttributeName = "DisplayName")]
        public string DisplayName;

        [XmlAttribute(AttributeName = "Icon")] public string Icon;

        [XmlAttribute(AttributeName = "InternalName")]
        public string InternalName;

        [XmlAttribute(AttributeName = "ReadOnly")]
        public bool ReadOnly;

        [XmlAttribute(AttributeName = "RevisionId")]
        public string RevisionId;
    }
}