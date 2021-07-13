using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "FamilyTable")]
    public class InventorFamilyTable
    {
        [XmlElement(ElementName = "Columns")] public InventorColumns Columns;

        [XmlElement(ElementName = "Rows")] public InventorRows Rows;
    }
}