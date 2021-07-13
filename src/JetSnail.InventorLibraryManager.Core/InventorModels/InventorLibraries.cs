using System.Collections.Generic;
using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "Libraries")]
    public class InventorLibraries
    {
        [XmlElement(ElementName = "Library")] public List<InventorLibrary> Libraries;
    }
}