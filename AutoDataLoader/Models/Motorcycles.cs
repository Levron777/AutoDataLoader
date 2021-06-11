using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDataLoader.Models
{
    class Motorcycles : MainDataClass
    {
        public Motorcycles()
        {
            FileName = (typeof(Motorcycles).Name).ToLower();
        }
    }
}
