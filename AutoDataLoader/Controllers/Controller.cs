using AutoDataLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDataLoader.Controllers
{
    class Controller
    {
        public Dictionary<string, string> NewRandomProductsArray { get; }
        public Dictionary<string, string> NewRandomOutletsArray { get; }
        public List<string> NewRandomSalesArray { get; }
        private Cars Cars { get; set; }
        private RussianOutlets RussianOutlets { get; }
        private Motorcycles Motorcycles { get; set; }
        private List<string>[] AllProducts { get; set; }
        private Sales Sales { get; }
        private Dictionary<int, int> Rnd { get; set; }
        private Random Random { get; set; }
        public Controller()
        {
            NewRandomProductsArray = new Dictionary<string, string>(5);
            NewRandomOutletsArray = new Dictionary<string, string>(3);
            NewRandomSalesArray = new List<string>();
            Rnd = new Dictionary<int, int>();
            Random = new Random();
        }
        public Controller(Cars cars, Motorcycles moto, RussianOutlets russianOutlets, Sales sales) : this()
        {
            Cars = cars ?? throw new ArgumentNullException("Данные об автомобилях не обнаружены!", nameof(cars));
            Motorcycles = moto ?? throw new ArgumentNullException("Данные о мотоциклах не обнаружены!", nameof(moto));
            RussianOutlets = russianOutlets ?? throw new ArgumentNullException("Данные об автомобилях не обнаружены!", nameof(russianOutlets));
            Sales = sales ?? throw new ArgumentNullException("Данные о мотоциклах не обнаружены!", nameof(sales));

            var allCars = Cars.AllFieldsName;
            var allMoto = Motorcycles.AllFieldsName;
            AllProducts = allCars;
            for (var i = 0; i < allCars.Length; i++)
            {
                AllProducts[i] = allCars[i].Concat(allMoto[i]).ToList();
            }
        }
        public Controller(Cars cars, RussianOutlets russianOutlets, Sales sales) : this()
        {
            Cars = cars ?? throw new ArgumentNullException("Данные об автомобилях не обнаружены!", nameof(cars));
            RussianOutlets = russianOutlets ?? throw new ArgumentNullException("Данные об автомобилях не обнаружены!", nameof(russianOutlets));
            Sales = sales ?? throw new ArgumentNullException("Данные о мотоциклах не обнаружены!", nameof(sales));

            var allCars = Cars.AllFieldsName;
            AllProducts = allCars;
        }
        public void GetRandomData()
        {
            GetRandomInt(AllProducts);
            NewRandomProductsArray.Add("brand", AllProducts[0][Rnd[0]]);
            NewRandomProductsArray.Add("productname", AllProducts[1][Rnd[1]]);
            NewRandomProductsArray.Add("segments", AllProducts[2][Rnd[2]]);
            NewRandomProductsArray.Add("incomingprice", AllProducts[3][Rnd[3]]);
            NewRandomProductsArray.Add("party", AllProducts[4][Rnd[4]]);

            var outlets = RussianOutlets.AllFieldsName;
            GetRandomInt(outlets);
            NewRandomOutletsArray.Add("name", outlets[0][Rnd[0]]);
            NewRandomOutletsArray.Add("city", outlets[1][Rnd[1]]);
            NewRandomOutletsArray.Add("adress", outlets[2][Rnd[2]]);
        }
        public void GetRandomSales(List<int> outletsId, List<int> productsId, int countOfNewSales)
        {
            if (outletsId.Count > 3 && productsId.Count > 3)
            {
                for (int i = 0; i < countOfNewSales; i++)
                {
                    int rnd = Random.Next(productsId.Count);
                    int rnd1 = Random.Next(outletsId.Count);
                    var sales = Sales.AllFieldsName;
                    GetRandomInt(sales);
                    NewRandomSalesArray.Add(Convert.ToString(productsId[rnd]));
                    NewRandomSalesArray.Add(Convert.ToString(outletsId[rnd1]));
                    NewRandomSalesArray.Add(sales[0][Rnd[0]]);
                    NewRandomSalesArray.Add(sales[1][Rnd[1]]);
                }
            }
            else
            {
                for (int i = 0; i < countOfNewSales; i++)
                {
                    NewRandomSalesArray.Add(null);
                }
            }
        }
        private void GetRandomInt(params List<string>[] list)
        {
            Rnd.Clear();
            for (int i = 0; i < list.Length; i++)
            {
                Rnd.Add(i, Random.Next(list[i].Count));
            }
        }
    }
}
