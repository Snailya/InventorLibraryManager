using System.Collections.Generic;

namespace JetSnail.InventorLibraryManager.Web.ViewModels
{
    public class FamilyPageViewModel
    {
        public List<LibraryOptionViewModel> Libraries { get; set; } 
        public List<FamilyLineItemViewModel> Families { get; set; }
        public LibraryOptionViewModel CurrentLibrary { get; set; }
        public IEnumerable<FamilyLineItemViewModel> FilteredFamilies { get; set; }
    }
}