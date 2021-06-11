using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDataLoader.Models
{
    class MainDataClass
    {
        public List<string> BrandsArray { get; set; }
        public List<string> ProductsNameArray { get; set; }
        public List<string> SegmentsArray { get; set; }
        public List<string> IncomingPriceArray { get; set; }
        public List<string> PartysArray { get; set; }
        public string Path { get; set; }
        public List<string>[] AllFieldsName { get; set; }
        public string FileName { get; set; }
        public MainDataClass()
        {
            FileName = (typeof(object).Name).ToLower();
            AllFieldsName = new[] { BrandsArray, ProductsNameArray, SegmentsArray, IncomingPriceArray, PartysArray };
        }

        public virtual void GetReadData()
        {
            Path = Directory.GetCurrentDirectory() + @"\" + FileName + ".txt";

            if (File.Exists(Path))
            {
                try
                {
                    string[] textLines = File.ReadAllLines(Path);
                    for (var i = 0; i < textLines.Length; i++)
                    {
                        AllFieldsName[i] = new List<string>();
                        var splitLineData = textLines[i].Split(new char[] { ',' });
                        foreach (var item in splitLineData)
                        {
                            AllFieldsName[i].Add(item.Trim(new Char[] { ' ', '*', '.', '"' }));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
