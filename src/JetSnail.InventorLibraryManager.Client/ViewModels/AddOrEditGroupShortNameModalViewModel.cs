using System.ComponentModel;
using JetSnail.InventorLibraryManager.Client.Validators;

namespace JetSnail.InventorLibraryManager.Client.ViewModels
{
    public sealed class AddOrEditGroupShortNameModalViewModel : IGroupShortName
    {
        [DisplayName("Id")] public int Id { get; set; }

        [DisplayName("名称")] public string Name { get; set; }

        [DisplayName("编码")] public string ShortName { get; set; }
    }
}