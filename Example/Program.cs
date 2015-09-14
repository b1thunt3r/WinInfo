using ROOT.CIMV2.Win32;
using System;
using System.Linq;
using System.Management;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var scope = new ManagementScope(new ManagementPath()
            {
                NamespacePath = @"root\cimv2",
                Server = "."
            });

            var nets = NetworkAdapter.GetInstances(scope, "")
                .Cast<NetworkAdapter>().Where(n => n.PhysicalAdapter);

            foreach (NetworkAdapter net in nets)
            {
                Console.WriteLine(net.Name);
                Console.WriteLine("\t" + net.MACAddress);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
