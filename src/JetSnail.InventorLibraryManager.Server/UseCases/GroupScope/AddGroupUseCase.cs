using System;
using System.Threading.Tasks;
using AutoMapper;
using JetSnail.InventorLibraryManager.Core.Entities;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;

namespace JetSnail.InventorLibraryManager.Server.UseCases.GroupScope
{
    public class AddGroupUseCase : IAddGroupUseCase
    {
        private readonly ICheckNameUseCase _checkNameUseCase;
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public AddGroupUseCase(IMapper mapper, ICheckNameUseCase checkNameUseCase, IGroupRepository groupRepository)
        {
            _mapper = mapper;
            _checkNameUseCase = checkNameUseCase;
            _groupRepository = groupRepository;
        }

        public async Task<GroupDto> Execute(string name, string shortName)
        {
            // valid name
            var (result, message) = await _checkNameUseCase.Execute(shortName);
            if (!result) throw new InvalidOperationException(message);

            var group = await _groupRepository.InsertAsync(new GroupEntity
            {
                Name = name, ShortName = shortName
            });

            return _mapper.Map<GroupDto>(group);
        }
    }
}