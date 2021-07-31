using System.Threading.Tasks;
using AutoMapper;
using JetSnail.InventorLibraryManager.Server.Exceptions;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;

namespace JetSnail.InventorLibraryManager.Server.UseCases.GroupScope
{
    public class GetGroupByIdUseCase : IGetGroupByIdUseCase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GetGroupByIdUseCase(IMapper mapper, IGroupRepository groupRepository)
        {
            _mapper = mapper;
            _groupRepository = groupRepository;
        }

        public async Task<GroupDto> Execute(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null) throw new ResourceNotFoundException();

            return _mapper.Map<GroupDto>(group);
        }
    }
}