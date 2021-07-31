using System.Collections.Generic;
using System.Linq;
using JetSnail.InventorLibraryManager.Core.Entities;
using JetSnail.InventorLibraryManager.Core.InventorModels;

namespace JetSnail.InventorLibraryManager.Server.UseCases
{
    public static class UseCaseExtensions
    {
        public static PrototypeFamilyEntity WithInventorView(this PrototypeFamilyEntity prototype,
            IEnumerable<InventorFamily> loadedFamilies)
        {
            var derivatives = new List<DerivativeEntity>();

            foreach (var family in loadedFamilies.Where(x => x.Library != prototype.LibraryId))
                // if already exist add to 
                if (prototype.Derivatives.SingleOrDefault(x => x.LibraryId == family.Library) is { } derivative)
                    derivatives.Add(derivative);
                // if not create new
                else
                    derivatives.Add(new DerivativeEntity
                        {FamilyId = family.InternalName, LibraryId = family.Library});

            prototype.Derivatives = derivatives;
            return prototype;
        }
    }
}