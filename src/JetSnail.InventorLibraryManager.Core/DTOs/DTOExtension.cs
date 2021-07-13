using JetSnail.InventorLibraryManager.Core.DbModels;

namespace JetSnail.InventorLibraryManager.Core.DTOs
{
    public static class DTOExtension
    {
        public static GroupDto ToDto(this DatabaseGroup entity)
        {
            return new GroupDto { Id = entity.Id, DisplayName = entity.DisplayName };
        }

        public static PartDto ToDto(this DatabasePart entity)
        {
            return new PartDto
                { Id = entity.Id, PartInternalName = entity.InternalName, InventorPartNumber = entity.PartNumber };
        }
    }
}