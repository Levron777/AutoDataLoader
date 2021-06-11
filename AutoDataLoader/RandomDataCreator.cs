using AutoDataLoader.Controllers;
using AutoDataLoader.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AutoDataLoader
{
    class RandomDataCreator : Form
    {
        private int CountOfNewSales { get; set; }
        public Dictionary<string, string> NewRandomProductsArray { get; set; }
        public Dictionary<string, string> NewRandomOutletsArray { get; set; }
        public List<string> NewRandomSalesArray { get; set; }
        public RandomDataCreator()
        {
            NewRandomProductsArray = new Dictionary<string, string>(5);
            NewRandomOutletsArray = new Dictionary<string, string>(3);
            NewRandomSalesArray = new List<string>();
            CountOfNewSales = Convert.ToInt32(AutoDataLoader.CountOfSales);
            string str = @"Data Source=DESKTOP-A14PILH\DEV;Initial Catalog=saleCarsDB;MultipleActiveResultSets=True;"
                + "Integrated Security=SSPI";
            GetRandomData(str);
        }
        private void GetRandomData(string connectionString)
        {
            RussianOutlets russianOutlets = new RussianOutlets();
            russianOutlets.GetReadData();
            Sales sale = new Sales();
            sale.GetReadData();
            Cars car = new Cars();
            car.GetReadData();
            Motorcycles moto = new Motorcycles();
            moto.GetReadData();
            Controller cont = new Controller(car, moto, russianOutlets, sale);
            cont.GetRandomData();
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
            cont.GetRandomSales(outletsId, productsId, CountOfNewSales);
            NewRandomProductsArray = cont.NewRandomProductsArray;
            NewRandomOutletsArray = cont.NewRandomOutletsArray;
            NewRandomSalesArray = cont.NewRandomSalesArray;
        }
    }
}
