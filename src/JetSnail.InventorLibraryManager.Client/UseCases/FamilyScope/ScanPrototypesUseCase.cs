using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using JetSnail.InventorLibraryManager.Client.Options;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope;
using JetSnail.InventorLibraryManager.UseCase.FamilyScope.DTOs;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace JetSnail.InventorLibraryManager.Client.UseCases.FamilyScope
{
    public class ScanPrototypesUseCase : IScanPrototypesUseCase
    {
        private readonly HubConnection _connection;

        public ScanPrototypesUseCase(IOptions<ServerOptions> options, HubConnectionBuilder hubConnectionBuilder)
        {
            _connection = hubConnectionBuilder.WithUrl($"{options.Value.BaseUrl}hubs/dashboard").Build();
        }

        public async IAsyncEnumerable<ProgressDto> Execute(
            [EnumeratorCancellation] CancellationToken token)
        {
            await _connection.StartAsync(token);
            await foreach (var dto in _connection.StreamAsync<ProgressDto>(
                "scan-prototypes", token))
                yield return dto;
            await _connection.StopAsync(token);
        }
    }
}