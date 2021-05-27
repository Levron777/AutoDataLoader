using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDataLoader
{
    class RandomDataCreator : Form
    {
        // add here your data arrays
        private readonly List<string> brandsArray = new List<string>() { "Honda", "Toyota", "Suzuki", "Volvo", "Audi", "BMW", "Mercedes", "Lexus", "Lada", "Nissan" };
        private readonly List<string> outletsNameArray = new List<string>() { "Priority auto", "Auto one", "Auto trade", "Skiff", "Mauto", "Emotors", "Auto prime", "Nsk motors", "Vip auto", "Consul" };
        private readonly List<string> productsNameArray = new List<string>() { "RAV4", "CR-V", "A4", "XC60", "Camry", "X6", "X3", "Civic", "Focus", "Rio" };
        private readonly List<string> segmentsArray = new List<string>() { "SUV", "Passenger", "Moto", "Mini" };
        private readonly List<string> citiesArray = new List<string>() { "Novosibirsk", "Novokuzneck", "Omsk", "Kemerovo", "Barnaul", "Krasnoyarsk", "Tomsk" };
        private readonly List<string> adressArray = new List<string>() { "Frunze, 5", "Lenina, 21", "Ordzhonikidze, 12", "Lenina, 28", "Oktyabrskaya, 125", "Severnaya, 163" };
        private readonly List<string> partysArray = new List<string>() { "3254464", "86846", "3423425", "1223414", "1225451", "5637347", "78449084", "4606804", "5464646", "5324514" };
        private readonly List<string> incomingPriceArray = new List<string>() { "2345000", "1345000", "2656300", "2355000", "2745450", "2255000", "940000", "12152133", "3504500" };
        private readonly List<string> salePriceArray = new List<string>() { "3345000", "2345000", "3656300", "2955000", "3745450", "3255000", "1540000", "22152133", "4504500" };
        private readonly List<string> dateOfSaleArray = new List<string>() { "2020 - 10 - 07", "2020 - 10 - 05", "2020 - 11 - 05", "2020 - 05 - 22", "2020 - 03 - 05", "2020 - 01 - 25", "2020 - 05 - 12" };
        private readonly int countOfNewSales = Convert.ToInt32(AutoDataLoader.CountOfSales);

        // public arrays with a new random data
        //public string[] newRandomProductsArray = new string[5];
        //public string[] newRandomOutletsArray = new string[3];
        public Dictionary<string, string> newRandomProductsArray = new Dictionary<string, string>(5);
        public Dictionary<string, string> newRandomOutletsArray = new Dictionary<string, string>(3);
        public List<string> newRandomSalesArray = new List<string>();
        readonly Random random = new Random();     

        public RandomDataCreator()
        {
            string str = @"Data Source=DESKTOP-A14PILH\DEV;Initial Catalog=saleCarsDB;MultipleActiveResultSets=True;"
                + "Integrated Security=SSPI";
            GetRandomSales(str);
            GetRandomProductsArray();
            GetRandomOutletsArray();
        }

        private void GetRandomProductsArray()
        {
            int rnd, rnd2, rnd3, rnd4, rnd5;
            rnd = random.Next(brandsArray.Count);
            rnd2 = random.Next(productsNameArray.Count);
            rnd3 = random.Next(segmentsArray.Count);
            rnd4 = random.Next(incomingPriceArray.Count);
            rnd5 = random.Next(partysArray.Count);
            newRandomProductsArray["brand"] = brandsArray[rnd];
            newRandomProductsArray["productname"] = productsNameArray[rnd2];
            newRandomProductsArray["segments"] = segmentsArray[rnd3];
            newRandomProductsArray["incomingprice"] = incomingPriceArray[rnd4];
            newRandomProductsArray["party"] = partysArray[rnd5];
        }

        private void GetRandomOutletsArray()
        {
            int rnd, rnd2, rnd3;
            rnd = random.Next(outletsNameArray.Count);
            rnd2 = random.Next(citiesArray.Count);
            rnd3 = random.Next(adressArray.Count);
            newRandomOutletsArray["name"] = outletsNameArray[rnd];
            newRandomOutletsArray["city"] = citiesArray[rnd2];
            newRandomOutletsArray["adress"] = adressArray[rnd3];
        }

        private void GetRandomSales(string connectionString)
        {
            SqlConnection dbConnection;       
            dbConnection = new SqlConnection(connectionString);
            SqlDataReader dbReaderOutlets = null;
            SqlDataReader dbReaderProducts = null;

            List<int> outletsId = new List<int>();
            List<int> productsId = new List<int>();

            try
            {
                SqlCommand commandOutlets = new SqlCommand("SELECT * FROM [outlets]", dbConnection);
                SqlCommand commandProducts = new SqlCommand("SELECT * FROM [products]", dbConnection);
                dbConnection.Open();
                dbReaderOutlets =  commandOutlets.ExecuteReader();
                dbReaderProducts =  commandProducts.ExecuteReader();

                while (dbReaderProducts.Read() &&  dbReaderOutlets.Read())
                {
                    outletsId.Add(dbReaderOutlets.GetInt32(0));
                    productsId.Add(dbReaderProducts.GetInt32(0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (dbReaderProducts != null && dbReaderOutlets != null)
                {
                    dbReaderOutlets.Close();
                    dbReaderProducts.Close();
                }

                if (dbConnection != null)
                {
                    dbConnection.Dispose();
                }
            }

            if (outletsId.Count > 3 && productsId.Count > 3)
            {
                for (int i = 0; i < countOfNewSales; i++)
                {
                    int rnd, rnd2, rnd3, rnd4;
                    rnd = random.Next(productsId.Count);
                    rnd2 = random.Next(outletsId.Count);
                    rnd3 = random.Next(salePriceArray.Count);
                    rnd4 = random.Next(dateOfSaleArray.Count);
                    newRandomSalesArray.Add(Convert.ToString(productsId[rnd]));
                    newRandomSalesArray.Add(Convert.ToString(outletsId[rnd2]));
                    newRandomSalesArray.Add(salePriceArray[rnd3]);
                    newRandomSalesArray.Add(dateOfSaleArray[rnd4]);
                }
            } else
            {
                for (int i = 0; i < countOfNewSales; i++)
                {
                    newRandomSalesArray.Add(null);
                }
            }
        }
    }
}
