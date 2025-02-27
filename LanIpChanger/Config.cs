using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanIpChanger
{
    public class Config
    {
        public List<AdapterData> Adapters { get; set; }
        public string? SelectedAdapterId { get; set; }
        public string? SelectedAdapterSettingsName { get; set; }
        public List<AdapterSetting> AdapterSettings { get; set; }

        public Config()
        {
            Adapters = new List<AdapterData>();
            AdapterSettings = new List<AdapterSetting>();
        }
    }
}
