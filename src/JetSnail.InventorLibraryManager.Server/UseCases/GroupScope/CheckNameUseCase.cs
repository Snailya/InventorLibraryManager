using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.GroupScope;
using JetSnail.InventorLibraryManager.UseCase.GroupScope.DTOs;
using JetSnail.InventorLibraryManager.UseCase.Plugins.DataStores;

namespace JetSnail.InventorLibraryManager.Server.UseCases.GroupScope
{
    public class CheckNameUseCase : ICheckNameUseCase
    {
        private readonly IGroupRepository _groupRepository;

        public CheckNameUseCase(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public Task<(bool, string)> Execute(string newValue, string oldValue)
        {
            if (string.IsNullOrEmpty(newValue))
                return Task.FromResult((false, $"{nameof(GroupDto.ShortName)}不能为空。"));

            if (!Regex.IsMatch(newValue, "^[A-Z]{3}$"))
                return Task.FromResult((false, $"{nameof(GroupDto.ShortName)}必须使用3位大写字母。"));

            if (!string.IsNullOrEmpty(oldValue) && newValue == oldValue)
                return Task.FromResult((true, $"{newValue}可以使用。"));

            return _groupRepository.GetAll().Any(e => e.ShortName == newValue)
                ? Task.FromResult((false, $"{newValue}已存在。"))
                : Task.FromResult((true, $"{newValue}可以使用。"));
        }
    }
}