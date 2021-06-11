using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDataLoader.Models
{
    class Outlets : MainDataClass
    {
        public List<string> OutletsNameArray { get; set; }
        public List<string> CitiesArray { get; set; }
        public List<string> AdressArray { get; set; }
        public Outlets()
        {
            AllFieldsName = new[] { OutletsNameArray, CitiesArray, AdressArray };
        }
    }
}
