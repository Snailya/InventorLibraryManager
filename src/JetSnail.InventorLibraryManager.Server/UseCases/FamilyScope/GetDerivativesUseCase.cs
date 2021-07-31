using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;

namespace JetSnail.InventorLibraryManager.Server.UseCases.FamilyScope
{
    public class GetDerivativesUseCase : IGetDerivativesUseCase
    {
        private readonly IGetPrototypeByIdUseCase _getPrototypeByIdUseCase;

        public GetDerivativesUseCase(IGetPrototypeByIdUseCase getPrototypeByIdUseCase)
        {
            _getPrototypeByIdUseCase = getPrototypeByIdUseCase;
        }

        public async Task<DerivativeFamilyDto[]> Execute(int prototypeId)
        {
            return (await _getPrototypeByIdUseCase.Execute(prototypeId)).Derivatives;
        }
    }
}