using System.ComponentModel;

namespace JetSnail.InventorLibraryManager.Web.ViewModels
{
    public class FamilyLineItemViewModel
    {
        public int? DatabaseId { get; set; }
        public string Id { get; set; }
        [DisplayName("名称")] public string Name { get; set; }
        [DisplayName("分组")] public GroupLineItemViewModel Group { get; set; }
        [DisplayName("库")] public LibraryOptionViewModel Library { get; set; }
    }
}