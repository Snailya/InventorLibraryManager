using System.Collections.Generic;
using System.Xml.Serialization;

namespace JetSnail.InventorLibraryManager.Core.InventorModels
{
    [XmlRoot(ElementName = "Categories")]
    public class InventorCategories
    {
        [XmlElement(ElementName = "Category")] public List<InventorCategory> Categories;
    }
}