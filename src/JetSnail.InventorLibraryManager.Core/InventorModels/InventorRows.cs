using System.Collections.Generic;
using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "Rows")]
    public class InventorRows
    {
        [XmlElement(ElementName = "Row")] public List<InventorRow> Rows;
    }
}