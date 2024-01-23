using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten_Threads
{
    internal class Beverage
    {
        public string Type { get; set; }
        public int TypeId { get; set; }
        public Beverage(string type, int typeId)
        {
            this.Type = type;
            TypeId = typeId;
        }
    }
}
