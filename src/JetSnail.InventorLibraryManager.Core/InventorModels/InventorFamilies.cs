using System.Collections.Generic;
using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "Families")]
    public class InventorFamilies
    {
        [XmlElement(ElementName = "Family")] public List<InventorFamily> Families;
    }
}