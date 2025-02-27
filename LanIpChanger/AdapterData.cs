using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanIpChanger
{
    public class AdapterData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AdapterData(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
