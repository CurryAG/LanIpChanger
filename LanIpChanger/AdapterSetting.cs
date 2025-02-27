using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LanIpChanger
{
    public class AdapterSetting
    {
        public string Name { get; set; }
        public string AdapterId { get; }
        public string? MainIP { get; set; }
        public string? MaskIP { get; set; }
        public string? GateAwayIP { get; set; }
        public string? FirstDNS { get; set; }
        public string? SecondDNS { get; set; }
        public bool IsDefault { get; }
        public AdapterSetting(string name, string adapterId, bool isdefault = false)
        {
            Name = name;
            AdapterId = adapterId;
            IsDefault = isdefault;
        }
    }
}
