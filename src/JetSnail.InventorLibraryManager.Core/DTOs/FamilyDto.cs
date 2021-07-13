using System.Text.Json.Serialization;

namespace JetSnail.InventorLibraryManager.Core.DTOs
{
	/// <summary>
	///     族的简要信息，用于在资源中心页面显示Family
	/// </summary>
	public class FamilyDto
    {
		/// <summary>
		///     Family Id in database.
		/// </summary>
		[JsonPropertyName("family_database_id")]
		public int? Id { get; set; }

		/// <summary>
		///     Family name.
		/// </summary>
		[JsonPropertyName("name")]
		public string DisplayName { get; set; }

		/// <summary>
		///     Family Id in Inventor.
		/// </summary>
		[JsonPropertyName("family_id")]
		public string FamilyInternalName { get; set; }

		/// <summary>
		///     Library Id in Inventor.
		/// </summary>
	[JsonPropertyName("library_id")]
		public string LibraryInternalName { get; set; }

		// /// <summary>
		// ///     The Library that the family belongs to.
		// /// </summary>
		// public string LibraryDisplayName { get; set; }

		/// <summary>
		///     Group Info in database
		/// </summary>
		[JsonPropertyName("group")]
		public GroupDto Group { get; set; }
    }
}