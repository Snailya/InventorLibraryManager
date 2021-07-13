using System.Collections.Generic;
using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "Category")]
    public class InventorCategory
    {
        [XmlElement(ElementName = "CategoryParameter")]
        public List<InventorCategoryParameter> CategoryParameters;

        [XmlElement(ElementName = "FamilyReference")]
        public List<InventorFamilyReference> FamilyReferences;

        [XmlElement(ElementName = "Category")] public List<InventorCategory> Categories;


        [XmlAttribute(AttributeName = "DisplayName")]
        public string DisplayName;

        [XmlAttribute(AttributeName = "Domain")]
        public string Domain;


        [XmlAttribute(AttributeName = "Hidden")]
        public bool Hidden;

        [XmlAttribute(AttributeName = "InternalName")]
        public string InternalName;

        [XmlAttribute(AttributeName = "Library")]
        public string Library;

        [XmlAttribute(AttributeName = "Mnemonic")]
        public string Mnemonic;

        [XmlAttribute(AttributeName = "RevisionId")]
        public string RevisionId;

        [XmlAttribute(AttributeName = "SpecialAuthoring")]
        public string SpecialAuthoring;

        [XmlAttribute(AttributeName = "VersionTime")]
        public string VersionTime;
    }
}