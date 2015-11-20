using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arhivach_Image_Downloader
{
    public static class ProcessHelper
    {
        /// <summary>
        /// Kill processes if they meet the parameter values of process name, owner name, expired started times.
        /// </summary>
        /// <param name="ProcessName">Process Name, case sensitive, for emample "EXCEL" could not be "excel"</param>
        /// <param name="ProcessUserName">Owner name or user name of the process, casse sensitive</param>
        /// <param name="HasStartedForHours">if process has started for more than n (parameter input) hours. 0 means regardless how many hours ago</param>
        public static bool KillProcessByName(string ProcessName)
        {
            try
            {
                Process[] foundProcesses = Process.GetProcessesByName(ProcessName);
                foreach (Process p in foundProcesses)
                {
                    p.Kill();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
