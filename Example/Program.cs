using Bit0.Utils.Windows.Extensions;
using Bit0.Utils.Windows.Info;
using ROOT.CIMV2.Win32;
using System;
using System.Linq;
using System.Management;
using System.Text;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var sb = new StringBuilder();
            var scope = Wmi.GetScope();
            var enumOptions = new EnumerationOptions { EnsureLocatable = true };

            var system = ComputerSystem.GetInstances(scope, enumOptions).Cast<ComputerSystem>().FirstOrDefault();
            if (system != null)
            {
                sb.AppendLine("{0,-22} {1}", "System Name:", system.Name);
                sb.AppendLine("{0,-22} {1}", "Domain:", system.Domain);
                sb.AppendLine("{0,-22} {1}", "Workgroup:", system.Workgroup);
                sb.AppendLine("{0,-22} {1}", "Username:", system.UserName);
                sb.AppendLine();
                sb.AppendLine("{0,-22} {1}", "Manufacturer:", system.Manufacturer);
                sb.AppendLine("{0,-22} {1}", "Model:", system.Model);
                sb.AppendLine("{0,-22} {1}", "Type:", system.PCSystemType);
                sb.AppendLine();
                sb.AppendLine("{0,-22} {1}", "Total Physical Memory:", system.TotalPhysicalMemory);
                sb.AppendLine("{0,-22} {1} ({2} logical cores)", "Number of Processors:", system.NumberOfProcessors, system.NumberOfLogicalProcessors);
            }

            var processor = Processor.GetInstances(scope, enumOptions).Cast<Processor>().FirstOrDefault();
            if (processor != null)
            {
                sb.AppendLine("{0,-22} {1}", "Name:", processor.Name);
                sb.AppendLine("{0,-22} {1} ({2}-bit)", "Architecture:",
                    processor.Architecture.GetDescription(), processor.DataWidth);
                sb.AppendLine("{0,-22} {1}", "Processor ID:", processor.ProcessorId);
            }
            sb.AppendLine();

            var nets = NetworkAdapter.GetInstances(scope, enumOptions)
                .Cast<NetworkAdapter>()
                .Where(n => n.PhysicalAdapter);
            foreach (var net in nets)
            {
                sb.AppendLine("{0,-22} {1}", "NIC Name:", net.Name);
                sb.AppendLine("{0,-22} {1}", "MAC Address:", net.MACAddress);
            }
            sb.AppendLine();

            var os = new OperatingSystem0(scope);
            {
                sb.AppendLine("{0,-22} {1} [{2}] ({3})", "Operating System:", os.Caption, os.Version, os.OSArchitecture);
                sb.AppendLine("{0,-22} {1:yyyy-MM-dd HH:mm:ss} ({2,3:dd} days {2:hh}:{2:mm}:{2:ss})",
                    "Install Date", os.InstallDate, (DateTime.Now - os.InstallDate));
                sb.AppendLine("{0,-22} {1:yyyy-MM-dd HH:mm:ss} ({2,3:dd} days {2:hh}:{2:mm}:{2:ss})",
                    "Last boot", os.LastBootUpTime, (DateTime.Now - os.LastBootUpTime));
            }

            sb.AppendLine();
            Console.WriteLine(sb.ToString());
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
