using System.Management;

namespace Bit0.Utils.Window.Info
{
    public class Common
    {
        private static Common m_Instance;
        public static Common Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new Common();

                return m_Instance;
            }
        }

        public ManagementObjectCollection GetQueryResult(string @class, string col = "*", string where = "")
        {
            var q = new SelectQuery("Select " + col + " From" + @class + " " + where);
            ManagementObjectSearcher s = new ManagementObjectSearcher(q);

            return s.Get();
        }

        public ManagementObjectCollection GetResult(string @class, string[] col, string where = "")
        {
            return GetQueryResult(@class, string.Join(", ", col), where);
        }
    }
}
