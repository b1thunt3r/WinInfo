using Bit0.Utils.Windows.Info;
using ROOT.CIMV2.Win32;
using System;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = ".";
            if (args != null && args.Any())
                server = args[0];

            var nets = NetworkAdapter.GetInstances(Wmi.GetScope(server: server), "")
                .Cast<NetworkAdapter>().Where(n => n.PhysicalAdapter);

            foreach (var net in nets)
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
