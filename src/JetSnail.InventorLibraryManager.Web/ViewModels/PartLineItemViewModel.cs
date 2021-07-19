using System.ComponentModel;

namespace JetSnail.InventorLibraryManager.Web.ViewModels
{
    public class PartLineItemViewModel
    {
        /// <summary>
        ///     Part Id in database
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        ///     Part RowId in inventor
        /// </summary>
        public string PartInternalName { get; set; }

        /// <summary>
        ///     Part FILENAME in inventor
        /// </summary>
        [DisplayName("文件名")]
        public string Name { get; set; }

        /// <summary>
        ///     Part PARTNUMBER in inventor
        /// </summary>
        [DisplayName("Inventor")]
        public string InventorPartNumber { get; set; }

        /// <summary>
        ///     Part PARTNUMBER in Database
        /// </summary>
        [DisplayName("数据库")]
        public string DatabasePartNumber { get; set; }

        /// <summary>
        ///     Indicates the PartNumber property need update.
        /// </summary>
        public bool NeedUpdate =>
            string.IsNullOrEmpty(InventorPartNumber) || string.IsNullOrEmpty(DatabasePartNumber) ||
            InventorPartNumber != DatabasePartNumber;
    }
}