using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDataLoader.Models
{
    class RussianOutlets : Outlets
    {
        public RussianOutlets()
        {
            FileName = (typeof(RussianOutlets).Name).ToLower();
        }
    }
}
