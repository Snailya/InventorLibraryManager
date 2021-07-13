using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JetSnail.InventorLibraryManager.UseCase.UseCases;
using JetSnail.InventorLibraryManager.Web.ViewModels;

namespace JetSnail.InventorLibraryManager.Web.Validators
{
    public class GroupValidator : AbstractValidator<GroupLineItemViewModel>
    {
        private readonly IGetGroupsUseCase _getGroupsUseCase;

        public GroupValidator(IGetGroupsUseCase getGroupsUseCase)
        {
            _getGroupsUseCase = getGroupsUseCase;

            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("{PropertyName}不能为空")
                .MustAsync(BeUniqueDisplayName).WithMessage("{PropertyValue}已存在")
                .WithName("组名");
            RuleFor(x => x.ShortName)
                .NotEmpty().WithMessage("{PropertyName}不能为空")
                .Must(UseUseThreeUpperCaseAlphabets).WithMessage("{PropertyName}必须使用3位大写字母")
                .MustAsync(BeUniqueShortName).WithMessage("{PropertyValue}已存在")
                .WithName("编码");
        }


        private async Task<bool> BeUniqueDisplayName(GroupLineItemViewModel vm, string arg2, CancellationToken arg3)
        {
            var groups = await _getGroupsUseCase.Execute();
            var invalids = vm.Id == null
                ? groups.ToList()
                : groups.Where(x => x.Id != vm.Id).ToList();

            return invalids.All(x => x.DisplayName != arg2);
        }


        private async Task<bool> BeUniqueShortName(GroupLineItemViewModel vm, string arg2, CancellationToken arg3)
        {
            var groups = await _getGroupsUseCase.Execute();
            var invalids = vm.Id == null
                ? groups.ToList()
                : groups.Where(x => x.Id != vm.Id).ToList();

            return invalids.All(x => x.ShortName != arg2);
        }

        private static bool UseUseThreeUpperCaseAlphabets(string arg)
        {
            return arg is { Length: 3 } && Regex.IsMatch(arg, "[A-Z]");
        }
    }
}