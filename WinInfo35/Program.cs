using System;
using System.Linq;
using System.Management;

namespace Bit0.Utils.Window.Info
{
    class Program
    {
        static void Main(string[] args)
        {
            ManagementPath path = new ManagementPath()
            {
                NamespacePath = @"root\CIMV2",
                Server = "."
            };

            ManagementScope scope = new ManagementScope(path);
            scope.Options.Impersonation = ImpersonationLevel.Impersonate;

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter");

            ManagementObjectSearcher s = new ManagementObjectSearcher(scope, query);

            //foreach (ManagementObject service in s.Get())
            //{
            //    Console.WriteLine(service.Properties["Name"].Value);
            //}

            //Console.WriteLine();

            var a = s.Get().Cast<ManagementObject>().ToList().Where(x => x.Properties["PhysicalAdapter"].Value.ToString() == "True");

            foreach (ManagementObject i in a)
            {
                Console.WriteLine(i.Properties["Name"].Value);
            }

            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
