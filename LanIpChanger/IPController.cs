using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanIpChanger
{
    internal class IPController
    {
        public static void SetIP(string AdapterName, string MainIP, string MaskIP, string GateawayIP)
        {
            CmdExecutor.ExecuteCmd($"netsh interface ip set address \"{AdapterName}\" static {MainIP} {MaskIP} {GateawayIP}");
        }
        public static void SetDns(string AdapterName, string FirstDNS, string SecondDNS)
        {
            CmdExecutor.ExecuteCmd($"netsh interface ipv4 set dns \"{AdapterName}\" static {FirstDNS}");
            CmdExecutor.ExecuteCmd($"netsh interface ipv4 add dns \"{AdapterName}\" {SecondDNS} index=2");
        }
        public static void Reset(string AdapterName)
        {
            CmdExecutor.ExecuteCmd($"netsh interface ip set address name=\"{AdapterName}\" source=dhcp");
            CmdExecutor.ExecuteCmd($"netsh interface ip set dns name=\"{AdapterName}\" source=dhcp");
        }
    }
}
