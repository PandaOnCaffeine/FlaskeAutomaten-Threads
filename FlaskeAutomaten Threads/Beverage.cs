using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace FlaskeAutomaten_Threads
{
    internal class Beverage
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Key { get; set; }

        public Beverage(string name, int id, int key)
        {
            Name = name;
            Id = id;
            Key = key;
        }
    }
}
