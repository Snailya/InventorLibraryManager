using System.Collections.Generic;
using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "Row")]
    public class InventorRow
    {
        [XmlElement(ElementName = "Cell")] public List<InventorCell> Cells;

        [XmlAttribute(AttributeName = "RowId")]
        public string InternalName;

        [XmlText] public string Text;
    }
}