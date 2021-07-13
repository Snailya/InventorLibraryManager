using System.Collections.Generic;

namespace JetSnail.InventorLibraryManager.Web.ViewModels
{
    public class GroupPageViewModel
    {
        public AntDesign.Form<GroupLineItemViewModel> Form { get; set; }
        public List<GroupLineItemViewModel> Groups { get; set; }
        public GroupLineItemViewModel GroupToAdd { get; set; } = new();
    }
}