using System.Collections.Generic;

namespace JetSnail.InventorLibraryManager.Web.ViewModels
{
    public class FamilyPageViewModel
    {
        public List<LibraryOptionViewModel> Libraries { get; set; } = new()
        {
            new LibraryOptionViewModel
            {
                Id = string.Empty,
                Name = "合并视图",
                ReadOnly = false
            }
        };

        public List<FamilyLineItemViewModel> Families { get; set; }
    }
}