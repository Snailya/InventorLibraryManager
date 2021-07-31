using System;
using System.Linq;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Server.Exceptions;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;

namespace JetSnail.InventorLibraryManager.Server.UseCases.GroupScope
{
    public class DeleteGroupIfNoDerivedUseCase : IDeleteGroupIfNoDerivedUseCase
    {
        private readonly IGroupRepository _groupRepository;

        public DeleteGroupIfNoDerivedUseCase(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ResourceNotFoundException">分组不存在</exception>
        /// <exception cref="InvalidOperationException">分组关联了族</exception>
        public async Task Execute(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null) throw new ResourceNotFoundException();
            if (group.Prototypes.SelectMany(e => e.Derivatives).Any())
                throw new InvalidOperationException("无法删除关联其他族的分组。");

            await _groupRepository.DeleteAsync(id);
        }
    }
}