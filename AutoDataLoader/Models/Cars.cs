using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDataLoader.Models
{
    class Cars : MainDataClass
    {
        public Cars()
        {
            FileName = (typeof(Cars).Name).ToLower();
        }
    }
}
