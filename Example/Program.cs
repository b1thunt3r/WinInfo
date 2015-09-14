using Bit0.Utils.Windows.Info;
using System;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var wmi = new WmiWrapper();

            var a = wmi.GetQueryResult("Win32_NetworkAdapter")
                .Where(p => p.Properties["PhysicalAdapter"].Value.ToString() == "True");

            foreach (var i in a)
            {
                Console.WriteLine(i.Properties["Name"].Value);
                Console.WriteLine(i.Properties["MACAddress"].Value);
            }

            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
