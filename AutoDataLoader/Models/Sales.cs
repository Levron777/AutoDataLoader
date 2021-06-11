using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDataLoader.Models
{
    class Sales : MainDataClass
    {
        public List<string> SalePriceList { get; set; }
        public List<string> SalesDataList { get; set; }

        public Sales()
        {
            FileName = (typeof(Sales).Name).ToLower();
            AllFieldsName = new[] { SalePriceList, SalesDataList };
        }
    }
}
