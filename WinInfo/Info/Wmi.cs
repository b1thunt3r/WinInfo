
using System.Management;

namespace Bit0.Utils.Windows.Info
{
    public class Wmi
    {
        public static ManagementScope GetScope(string ns = @"root\cimv2", string server = ".")
        {
            return new ManagementScope()
            {
                Options = { Impersonation = ImpersonationLevel.Impersonate },
                Path = new ManagementPath()
                {
                    NamespacePath = ns,
                    Server = server
                }
            };
        }
    }
}
