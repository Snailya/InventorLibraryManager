namespace JetSnail.InventorLibraryManager.Core.Domains
{
	public class ModelBase<TInventorModel, TDatabaseModel>
	{
		public TInventorModel InventorModel { get; set; }
		public TDatabaseModel DatabaseModel { get; set; }
	}
}