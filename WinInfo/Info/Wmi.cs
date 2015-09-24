
using System.Management;

namespace Bit0.Utils.Windows.Info
{
    public class Wmi
    {
        public static ManagementScope GetScope(string ns = @"root\cimv2", string server = ".", string username = null, string password = null)
        {
            var scope = new ManagementScope()
            {
                Options =
                {
                    Impersonation = ImpersonationLevel.Impersonate,
                    Authentication = AuthenticationLevel.PacketPrivacy,
                    EnablePrivileges = true
                },
                Path = new ManagementPath()
                {
                    NamespacePath = ns,
                    Server = server
                }
            };

            if (!string.IsNullOrWhiteSpace(username))
                scope.Options.Username = username;

            if (!string.IsNullOrWhiteSpace(password))
                scope.Options.Password = password;

            return scope;
        }
    }
}
