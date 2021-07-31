using System;

namespace JetSnail.InventorLibraryManager.Server.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message)
        {
        }

        public ResourceNotFoundException() : base("未找到。")
        {
        }
    }
}