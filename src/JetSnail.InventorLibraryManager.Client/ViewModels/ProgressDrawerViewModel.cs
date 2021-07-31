using System.Collections.Generic;
using System.Threading;

namespace JetSnail.InventorLibraryManager.Client.ViewModels
{
    public class ProgressDrawerViewModel
    {
        public List<string> Logs { get; set; } = new();
        public double Percent { get; set; }
        public CancellationTokenSource Source { get; set; } = new();
    }
}