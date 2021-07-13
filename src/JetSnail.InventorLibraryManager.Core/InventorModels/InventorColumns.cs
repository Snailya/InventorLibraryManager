using System.Collections.Generic;
using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "Columns")]
    public class InventorColumns
    {
        [XmlElement(ElementName = "Column")] public List<InventorColumn> Columns;
    }
}