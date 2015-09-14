﻿using System;
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

            var a = Common.Instance.GetQueryResult(path, "Win32_NetworkAdapter")
                .Where(p => p.Properties["PhysicalAdapter"].Value.ToString() == "True");

            foreach (ManagementObject i in a)
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
