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

            var processor = Processor.GetInstances(scope, enumOptions).Cast<Processor>().FirstOrDefault();
            if (processor != null)
            {
                sb.AppendLine(string.Format("{0,-22} {1}", "Name:", processor.Name));
                sb.AppendLine(string.Format("{0,-22} {1} ({2}-bit)", "Architecture:",
                    processor.Architecture.GetDescription(), processor.DataWidth));
                sb.AppendLine(string.Format("{0,-22} {1}", "Processor ID:", processor.ProcessorId));
            }
            sb.AppendLine();

            var os = new OperatingSystem0(scope);
            {
                sb.AppendLine(string.Format("{0,-22} {1} [{2}] ({3})", "Operating System:", os.Caption, os.Version, os.OSArchitecture));
                sb.AppendLine(string.Format("{0,-22} {1:yyyy-MM-dd HH:mm:ss} ({2,3:dd} days {2:hh}:{2:mm}:{2:ss})",
                    "Install Date", os.InstallDate, (DateTime.Now - os.InstallDate)));
                sb.AppendLine(string.Format("{0,-22} {1:yyyy-MM-dd HH:mm:ss} ({2,3:dd} days {2:hh}:{2:mm}:{2:ss})",
                    "Last boot", os.LastBootUpTime, (DateTime.Now - os.LastBootUpTime)));
            }

            sb.AppendLine();

            Console.WriteLine(sb.ToString());
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
