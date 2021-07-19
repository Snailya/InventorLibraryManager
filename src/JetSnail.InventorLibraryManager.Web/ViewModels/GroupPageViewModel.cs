using System.Collections.Generic;
using AntDesign;

namespace JetSnail.InventorLibraryManager.Web.ViewModels
{
    public class GroupPageViewModel
    {
        public Form<GroupLineItemViewModel> Form { get; set; }
        public List<GroupLineItemViewModel> Groups { get; set; }
        public GroupLineItemViewModel GroupToAdd { get; set; } = new();
    }
}