
using Bit0.Utils.Windows.Extensions;
using ROOT.CIMV2.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace Bit0.Utils.Windows.Info.Ui
{
    public class Tui
    {
        private readonly ComputerSystem _system;
        private readonly Processor _processor;
        private readonly IEnumerable<NetworkAdapter> _networkAdapters;
        private readonly OperatingSystem0 _operatingSystem;

        public Tui()
        {
            var scope = Wmi.GetScope();
            var enumOptions = new EnumerationOptions { EnsureLocatable = true };

            _system = ComputerSystem.GetInstances(scope, enumOptions).Cast<ComputerSystem>().FirstOrDefault();
            _processor = Processor.GetInstances(scope, enumOptions).Cast<Processor>().FirstOrDefault();
            _networkAdapters = NetworkAdapter.GetInstances(scope, enumOptions)
                .Cast<NetworkAdapter>()
                .Where(n => n.PhysicalAdapter);
            _operatingSystem = new OperatingSystem0(scope);
        }

        public string SystemName
        {
            get
            {
                if (_system != null)
                    return _system.Name;
                return "Unknown";
            }
        }

        private string[] _levels = new[]
        {
            "B", "KB", "MB", "GB", "TB" 
        };
        private string Round(float input, int level = 0)
        {
            var temp = input / 1024f;
            if (temp < 1024f)
                return input + " " + _levels[level];

            level++;
            return Round(temp, level);
        }

        private string SystemInfomation
        {
            get
            {
                var sb = new StringBuilder();
                if (_system == null) return sb.ToString();

                sb.AppendLine(string.Format("{0,-22} {1}", "System Name:", _system.Name));
                sb.AppendLine(string.Format("{0,-22} {1}", "Domain:", _system.Domain));
                sb.AppendLine(string.Format("{0,-22} {1}", "Workgroup:", _system.Workgroup));
                sb.AppendLine(string.Format("{0,-22} {1}", "Username:", _system.UserName));
                sb.AppendLine();
                sb.AppendLine(string.Format("{0,-22} {1}", "Manufacturer:", _system.Manufacturer));
                sb.AppendLine(string.Format("{0,-22} {1}", "Model:", _system.Model));
                sb.AppendLine(string.Format("{0,-22} {1}", "Type:", _system.PCSystemType));
                sb.AppendLine();
                sb.AppendLine(string.Format("{0,-22} {1}", "Total Physical Memory:", Round(_system.TotalPhysicalMemory)));
                sb.AppendLine(string.Format("{0,-22} {1} ({2} logical cores)", "Number of Processors:", _system.NumberOfProcessors, _system.NumberOfLogicalProcessors));

                return sb.ToString();
            }
        }

        private string NetworkInfomation
        {
            get
            {
                var sb = new StringBuilder();
                if (_networkAdapters == null || !_networkAdapters.Any()) return sb.ToString();

                foreach (var net in _networkAdapters)
                {
                    sb.AppendLine(string.Format("{0,-22} {1}", "NIC Name:", net.Name));
                    sb.AppendLine(string.Format("{0,-22} {1}", "MAC Address:", net.MACAddress));
                }

                return sb.ToString();
            }
        }

        private string ProcessorInfomation
        {
            get
            {
                var sb = new StringBuilder();
                if (_processor == null) return sb.ToString();

                sb.AppendLine(string.Format("{0,-22} {1}", "Name:", _processor.Name));
                sb.AppendLine(string.Format("{0,-22} {1} ({2}-bit)", "Architecture:",
                    _processor.Architecture.GetDescription(), _processor.DataWidth));
                sb.AppendLine(string.Format("{0,-22} {1}", "Processor ID:", _processor.ProcessorId));

                return sb.ToString();
            }
        }

        private string OperatingSystemInfomation
        {
            get
            {
                var sb = new StringBuilder();
                if (_operatingSystem == null) return sb.ToString();

                sb.AppendLine(string.Format("{0,-22} {1}", "Operating System:", _operatingSystem.Caption));
                sb.AppendLine(string.Format("{0,-22} {1}", "Version:", _operatingSystem.Version));
                sb.AppendLine(string.Format("{0,-22} {1}", "Architecture:", _operatingSystem.OSArchitecture));
                sb.AppendLine();
                sb.AppendLine(string.Format("{0,-22} {1:yyyy-MM-dd HH:mm:ss} ({2,4:dd} days ago)",
                    "Install Date:", _operatingSystem.InstallDate, (DateTime.Now - _operatingSystem.InstallDate)));
                sb.AppendLine(string.Format("{0,-22} {1:yyyy-MM-dd HH:mm:ss} ({2,4:dd} days ago)",
                    "Last boot:", _operatingSystem.LastBootUpTime, (DateTime.Now - _operatingSystem.LastBootUpTime)));

                return sb.ToString();
            }
        }

        public string GetInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("System");
            sb.AppendLine("======");
            sb.AppendLine(SystemInfomation);

            sb.AppendLine("Processor");
            sb.AppendLine("=========");
            sb.AppendLine(ProcessorInfomation);

            sb.AppendLine("Network");
            sb.AppendLine("=======");
            sb.AppendLine(NetworkInfomation);

            sb.AppendLine("Operating System");
            sb.AppendLine("================");
            sb.AppendLine(OperatingSystemInfomation);

            return sb.ToString();
        }
    }
}
