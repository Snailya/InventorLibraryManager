using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "Cell")]
    public class InventorCell
    {
        [XmlAttribute(AttributeName = "ColumnId")]
        public string ColumnId;

        [XmlText] public string Text;
    }
}