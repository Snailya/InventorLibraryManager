using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JetSnail.InventorLibraryManager.Web.ViewModels
{
    public class GroupLineItemViewModel
    {
        public int? Id { get; set; }

        [Required] [DisplayName("名称")] public string DisplayName { get; set; }

        [Required] [DisplayName("编码")] public string ShortName { get; set; }
    }
}