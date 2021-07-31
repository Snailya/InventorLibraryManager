using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;

namespace JetSnail.InventorLibraryManager.Server.UseCases.GroupScope
{
    public class GetGroupsUseCase : IGetGroupsUseCase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GetGroupsUseCase(IMapper mapper, IGroupRepository groupRepository)
        {
            _mapper = mapper;
            _groupRepository = groupRepository;
        }

        public async Task<GroupDto[]> Execute()
        {
            return (await _groupRepository.GetAllAsync()).Select(x => _mapper.Map<GroupDto>(x)).ToArray();
        }
    }
}