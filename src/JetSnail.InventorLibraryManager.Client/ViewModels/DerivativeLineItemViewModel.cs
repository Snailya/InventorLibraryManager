using System;
using System.ComponentModel;

namespace JetSnail.InventorLibraryManager.Client.ViewModels
{
    public sealed class DerivativeLineItemViewModel
    {
        [DisplayName("名称")] public string Name { get; set; }
        [DisplayName("描述")] public string Description { get; set; }

        /// <summary>
        /// This id is used to filter library options when open create derivative select
        /// </summary>
        public string LibraryId { get; set; }
        [DisplayName("库")] public string LibraryName { get; set; }
        
        /// <summary>
        /// This date is used to decide whether this derivative is untracked as if it is not tracked then the created date is null
        /// </summary>
        public DateTime? CreatedAt { get; set; }
        
        /// <summary>
        ///     This datetime is used to decide whether derivatives need update by comparing whether prototype's modified date is
        ///     later than derivatives' synchronized date
        /// </summary>
        public DateTime? SynchronizedAt { get; set; }
    }

    public enum DerivativeStatus
    {
        Normal,
        UnTracked
    }
}