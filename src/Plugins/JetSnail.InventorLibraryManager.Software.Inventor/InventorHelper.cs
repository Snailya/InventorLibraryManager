using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using Inventor;

namespace JetSnail.InventorLibraryManager.Software.Inventor
{
    public static class InventorHelper
    {
        private static Guid _clsid = Guid.Parse("B6B5DC40-96E3-11d2-B774-0060B0F159EF");

        public static Application GetAppReference()
        {
            GetActiveObject(ref _clsid, IntPtr.Zero, out var obj);
            if (obj == null)
            {
                var inventorType = Type.GetTypeFromCLSID(_clsid, true);
                Activator.CreateInstance(inventorType);
            }

            GetActiveObject(ref _clsid, IntPtr.Zero, out obj);

            return (Application) obj;
        }


        [DllImport("oleaut32.dll", PreserveSig = false)]
        [ResourceExposure(ResourceScope.None)]
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical] // auto-generated
        private static extern void GetActiveObject(ref Guid rclsid, IntPtr reserved,
            [MarshalAs(UnmanagedType.Interface)] out object ppunk);
    }
}