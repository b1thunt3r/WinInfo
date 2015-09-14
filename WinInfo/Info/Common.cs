using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Bit0.Utils.Windows.Info
{
    public class Common
    {
        private static Common _instance;
        public static Common Instance
        {
            get { return _instance ?? (_instance = new Common()); }
        }

        public IEnumerable<ManagementObject> GetQueryResult(ManagementPath path, string @class, string col = "*", string where = "")
        {
            var scope = new ManagementScope(path)
            {
                Options = { Impersonation = ImpersonationLevel.Impersonate }
            };

            var qStr = string.Format("Select {1} From {0} ", @class, col);
            if (!string.IsNullOrEmpty(where))
                qStr += where;

            var query = new SelectQuery(qStr);
            var searcher = new ManagementObjectSearcher(scope, query);

            return searcher.Get().Cast<ManagementObject>();
        }

        //public ManagementObjectCollection GetResult(string @class, string[] col, string where = "")
        //{
        //    return GetQueryResult(@class, string.Join(", ", col), where);
        //}
    }
}
