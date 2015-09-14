using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Bit0.Utils.Windows.Info
{
    public class WmiWrapper
    {
        private readonly ManagementScope _scope;

        public WmiWrapper(ManagementPath path)
        {
            _scope = new ManagementScope(path)
            {
                Options = { Impersonation = ImpersonationLevel.Impersonate }
            };
        }

        public WmiWrapper(string ns = @"root\CIMV2", string server = ".")
            : this(new ManagementPath() { NamespacePath = ns, Server = server }) { }

        public IEnumerable<ManagementObject> GetQueryResult(string @class, string col = "*", string where = "")
        {
            var qStr = string.Format("Select {1} From {0} ", @class, col);
            if (!string.IsNullOrEmpty(where))
                qStr += where;

            var query = new SelectQuery(qStr);
            var searcher = new ManagementObjectSearcher(_scope, query);

            return searcher.Get().Cast<ManagementObject>();
        }

        public IEnumerable<ManagementObject> GetResult(string @class, string[] col, string where = "")
        {
            return GetQueryResult(@class, string.Join(", ", col), where);
        }
    }
}
