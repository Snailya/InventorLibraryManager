using System.Collections.Generic;
using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "CategoryParameter")]
    public class InventorCategoryParameter
    {
        [XmlAttribute(AttributeName = "AppliesToMember")]
        public string AppliesToMember;

        [XmlElement(ElementName = "CategoryParameter")]
        public List<InventorCategoryParameter> CategoryParameters;

        [XmlAttribute(AttributeName = "DefinitionOnly")]
        public bool DefinitionOnly;

        [XmlAttribute(AttributeName = "DisplayName")]
        public string DisplayName;

        [XmlAttribute(AttributeName = "IndexedSearch")]
        public bool IndexedSearch;

        [XmlAttribute(AttributeName = "InternalName")]
        public string InternalName;

        [XmlAttribute(AttributeName = "Mnemonic")]
        public string Mnemonic;

        [XmlAttribute(AttributeName = "Optional")]
        public bool Optional;

        [XmlAttribute(AttributeName = "Repeatable")]
        public bool Repeatable;

        [XmlAttribute(AttributeName = "UnitType")]
        public string UnitType;

        [XmlAttribute(AttributeName = "UsesDefinition")]
        public string UsesDefinition;

        [XmlAttribute(AttributeName = "ValueLocalized")]
        public bool ValueLocalized;

        [XmlAttribute(AttributeName = "ValueType")]
        public string ValueType;
    }
}