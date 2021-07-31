using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JetSnail.InventorLibraryManager.Client.ViewModels
{
    public class GroupLineItemViewModel
    {
        [DisplayName("Id")] public int Id { get; set; }

        [Required] [DisplayName("名称")] public string Name { get; set; }

        [Required] [DisplayName("编码")] public string ShortName { get; set; }

        public List<PrototypeLineItemViewModel> Prototypes { get; set; }
    }
}