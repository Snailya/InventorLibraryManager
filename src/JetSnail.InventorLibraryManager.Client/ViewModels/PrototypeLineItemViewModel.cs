using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace JetSnail.InventorLibraryManager.Client.ViewModels
{
    public sealed class
        PrototypeLineItemViewModel
    {
        /// <summary>
        ///     This identifier is used to as parameter to pass in all prototype related api
        /// </summary>
        public int Id { get; set; }

        [DisplayName("名称")] public string Name { get; set; }
        [DisplayName("描述")] public string Description { get; set; }
        [DisplayName("库")] public string LibraryName { get; set; }
        
        /// <summary>
        ///     This datetime is used to decide whether derivatives need update by comparing whether prototype's modified date is
        ///     later than derivatives' synchronized date
        /// </summary>
        [DisplayName("修改于")]
        public DateTime? GroupModifiedAt { get; set; }
        [DisplayName("分组")] public string GroupName { get; set; }
        
        public IList<DerivativeLineItemViewModel> Derivatives { get; set; }
    }
}