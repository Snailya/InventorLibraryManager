using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetSnail.InventorLibraryManager.Core.Domains;
using JetSnail.InventorLibraryManager.Core.InventorModels;
using JetSnail.InventorLibraryManager.UseCase.DataStores;
using Microsoft.Extensions.Logging;

namespace JetSnail.InventorLibraryManager.Service.Services
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly ILogger<LibraryRepository> _logger;

        public LibraryRepository(ILogger<LibraryRepository> logger)
        {
            _logger = logger;
        }

        public Task<IEnumerable<Library>> GetAllAsync()
        {
            // update inventor content
            var app = InventorHelper.GetAppReference();
            app.ContentCenter.ForceRefreshCache();

            // get libraries through xml
            var xmlLibraries = app.ContentCenter.LibraryManager.GetServerLibraries().ToObject<InventorLibraries>();

            return Task.FromResult(xmlLibraries.Libraries.Select(x => new Library { InventorModel = x }));
        }

        public Task<Library> GetByIdAsync(string id)
        {
            // update inventor content
            var app = InventorHelper.GetAppReference();
            app.ContentCenter.ForceRefreshCache();
            var xmlLibraries = app.ContentCenter.LibraryManager.GetServerLibraries().ToObject<InventorLibraries>();

            var xmlLibrary = xmlLibraries.Libraries.SingleOrDefault(x => x.InternalName == id);
            return xmlLibrary == null
                ? Task.FromResult<Library>(null)
                : Task.FromResult(new Library { InventorModel = xmlLibrary });
        }
    }
}