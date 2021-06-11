using AutoDataLoader.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDataLoader
{
    public partial class AutoDataLoader : Form
    {
        private SqlConnection dbConnection;
        readonly string connectionString = @"Data Source=DESKTOP-A14PILH\DEV;Initial Catalog=saleCarsDB;Integrated Security=True;MultipleActiveResultSets=True";
        public static string CountOfSales;
        public AutoDataLoader()
        {
            InitializeComponent();
        }
        private void AutoDataLoader_Load(object sender, EventArgs e)
        {
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.View = View.Details;
            listView1.Columns.Add("Id");
            listView1.Columns.Add("Name");
            listView1.Columns.Add("City");
            listView1.Columns.Add("Adress");
            listView2.GridLines = true;
            listView2.FullRowSelect = true;
            listView2.View = View.Details;
            listView2.Columns.Add("Id");
            listView2.Columns.Add("Product name");
            listView2.Columns.Add("Segments");
            listView2.Columns.Add("Brand");
            listView2.Columns.Add("Incoming price");
            listView2.Columns.Add("Party");
            listView3.GridLines = true;
            listView3.FullRowSelect = true;
            listView3.View = View.Details;
            listView3.Columns.Add("Id");
            listView3.Columns.Add("Product Id");
            listView3.Columns.Add("Outlet Id");
            listView3.Columns.Add("Sale price");
            listView3.Columns.Add("Date of sale");
            CountOfSales = InputNumber.Text;
            LoadDataFromDB();
        }
        private void LoadDataFromDB()
        {
            dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
            SqlDataReader dbReaderOutlets = null;
            SqlDataReader dbReaderProducts = null;
            SqlDataReader dbReaderSales = null;
            SqlCommand getOutletsSql = new SqlCommand("SELECT * FROM [outlets]", dbConnection);
            SqlCommand getProductsSql = new SqlCommand("SELECT * FROM [products]", dbConnection);
            SqlCommand getSalesSql = new SqlCommand("SELECT * FROM [sales]", dbConnection);
            try
            {
                dbReaderOutlets = getOutletsSql.ExecuteReader();
                dbReaderProducts = getProductsSql.ExecuteReader();
                dbReaderSales = getSalesSql.ExecuteReader();
                while (dbReaderOutlets.Read())
                {
                    ListViewItem itemOutlets = new ListViewItem(new string[]
                     {
                        Convert.ToString(dbReaderOutlets["id"]),
                        Convert.ToString(dbReaderOutlets["name"]),
                        Convert.ToString(dbReaderOutlets["city"]),
                        Convert.ToString(dbReaderOutlets["adress"])
                     });
                    listView1.Items.Add(itemOutlets);
                }
                while (dbReaderProducts.Read())
                {
                    ListViewItem itemProducts = new ListViewItem(new string[]
                     {
                        Convert.ToString(dbReaderProducts["id"]),
                        Convert.ToString(dbReaderProducts["product name"]),
                        Convert.ToString(dbReaderProducts["segments"]),
                        Convert.ToString(dbReaderProducts["brand"]),
                        Convert.ToString(dbReaderProducts["incoming price"]),
                        Convert.ToString(dbReaderProducts["party"])
                     });
                    listView2.Items.Add(itemProducts);
                }
                while (dbReaderSales.Read())
                {
                    ListViewItem itemSales = new ListViewItem(new string[]
                     {
                        Convert.ToString(dbReaderSales["id"]),
                        Convert.ToString(dbReaderSales["product_id"]),
                        Convert.ToString(dbReaderSales["outlet_id"]),
                        Convert.ToString(dbReaderSales["sale price"]),
                        Convert.ToString(dbReaderSales["date of sale"])
                     });
                    listView3.Items.Add(itemSales);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (dbReaderProducts != null && dbReaderOutlets != null && dbReaderSales != null)
                {
                    dbReaderOutlets.Close();
                    dbReaderProducts.Close();
                    dbReaderSales.Close();
                }

                if (dbConnection != null)
                {
                    dbConnection.Dispose();
                }
            }
        }
        private void SubmitButton_MouseHover(object sender, EventArgs e)
        {
            SubmitButton.BackColor = Color.LawnGreen;
        }
        private void SubmitButton_MouseLeave(object sender, EventArgs e)
        {
            SubmitButton.BackColor = Color.White;
        }
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            RandomDataCreator dataCreator = new RandomDataCreator();
            dbConnection = new SqlConnection(connectionString);
            var newRandomProductsData = dataCreator.NewRandomProductsArray;
            var newRandomOutletsData = dataCreator.NewRandomOutletsArray;
            var newRandomSalesData = dataCreator.NewRandomSalesArray;
            dbConnection.Open();
            try
            {
                SqlCommand commandOutlets = new SqlCommand("INSERT INTO [outlets] (name, city, adress) VALUES (@name, @city, @adress)", dbConnection);
                SqlCommand commandProducts = new SqlCommand("INSERT INTO [products] ([product name], segments, brand, [incoming price], party) VALUES (@productname, @segments, @brand, @incomingprice, @party)", dbConnection);
                commandOutlets.Parameters.AddWithValue("@name", newRandomOutletsData["name"]);
                commandOutlets.Parameters.AddWithValue("@city", newRandomOutletsData["city"]);
                commandOutlets.Parameters.AddWithValue("@adress", newRandomOutletsData["adress"]);
                commandProducts.Parameters.AddWithValue("@productname", newRandomProductsData["productname"]);
                commandProducts.Parameters.AddWithValue("@segments", newRandomProductsData["segments"]);
                commandProducts.Parameters.AddWithValue("@brand", newRandomProductsData["brand"]);
                commandProducts.Parameters.AddWithValue("@incomingprice", newRandomProductsData["incomingprice"]);
                commandProducts.Parameters.AddWithValue("@party", newRandomProductsData["party"]);
                if (newRandomSalesData[0] != null)
                {
                    int i = 0;
                    for (; i < newRandomSalesData.Count; )
                    {
                        SqlCommand commandSales = new SqlCommand("INSERT INTO [sales] (product_id, outlet_id, [sale price], [date of sale]) VALUES (@product_id, @outlet_id, @saleprice, @dateofsale)", dbConnection);
                        commandSales.Parameters.AddWithValue("@product_id", newRandomSalesData[i++]);
                        commandSales.Parameters.AddWithValue("@outlet_id", newRandomSalesData[i++]);
                        commandSales.Parameters.AddWithValue("@saleprice", newRandomSalesData[i++]);
                        commandSales.Parameters.AddWithValue("@dateofsale", newRandomSalesData[i++]);
                        commandSales.ExecuteNonQuery();
                    }
                }
                commandOutlets.ExecuteNonQuery();
                commandProducts.ExecuteNonQuery();
                ListViewItem itemOutlets = new ListViewItem(new string[]
                    {
                        Convert.ToString(" "),
                        Convert.ToString(newRandomOutletsData["name"]),
                        Convert.ToString(newRandomOutletsData["city"]),
                        Convert.ToString(newRandomOutletsData["adress"])
                    });
                listView1.Items.Add(itemOutlets);
                ListViewItem itemProducts = new ListViewItem(new string[]
                    {
                        Convert.ToString(" "),
                        Convert.ToString(newRandomProductsData["productname"]),
                        Convert.ToString(newRandomProductsData["segments"]),
                        Convert.ToString(newRandomProductsData["brand"]),
                        Convert.ToString(newRandomProductsData["incomingprice"]),
                        Convert.ToString(newRandomProductsData["party"])
                    });
                listView2.Items.Add(itemProducts);
                if (newRandomSalesData[0] != null)
                {
                
                    ListViewItem itemSales;
                    int i = 0;
                    for (; i < newRandomSalesData.Count; )
                    {
                        itemSales = new ListViewItem(new string[]
                         {
                        " ",
                        newRandomSalesData[i++],
                        newRandomSalesData[i++],
                        newRandomSalesData[i++],
                        newRandomSalesData[i++]
                         });
                        listView3.Items.Add(itemSales);
                    }
                }
                if (listView1.Items.Count > 0)
                {
                    informationField.Visible = true;
                    informationField.Text = "Данные успешно добавлены в базу!";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (dbConnection != null)
                {
                    dbConnection.Dispose();
                }
            }
        }
        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
                dbConnection.Close();
        }
        private void AutoDataLoader_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
                dbConnection.Close();
        }
        private void ОбновитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
            SqlDataReader dbReaderOutlets = null;
            SqlDataReader dbReaderProducts = null;
            SqlDataReader dbReaderSales = null;
            SqlCommand getOutletsSql = new SqlCommand("SELECT * FROM [outlets]", dbConnection);
            SqlCommand getProductsSql = new SqlCommand("SELECT * FROM [products]", dbConnection);
            SqlCommand getSalesSql = new SqlCommand("SELECT * FROM [sales]", dbConnection);
            try
            {
                dbReaderOutlets = getOutletsSql.ExecuteReader();
                dbReaderProducts = getProductsSql.ExecuteReader();
                dbReaderSales = getSalesSql.ExecuteReader();

                while (dbReaderOutlets.Read())
                {
                    ListViewItem itemOutlets = new ListViewItem(new string[]
                     {
                        Convert.ToString(dbReaderOutlets["id"]),
                        Convert.ToString(dbReaderOutlets["name"]),
                        Convert.ToString(dbReaderOutlets["city"]),
                        Convert.ToString(dbReaderOutlets["adress"])
                     });
                    listView1.Items.Add(itemOutlets);
                }
                while (dbReaderProducts.Read())
                {
                    ListViewItem itemProducts = new ListViewItem(new string[]
                     {
                        Convert.ToString(dbReaderProducts["id"]),
                        Convert.ToString(dbReaderProducts["product name"]),
                        Convert.ToString(dbReaderProducts["segments"]),
                        Convert.ToString(dbReaderProducts["brand"]),
                        Convert.ToString(dbReaderProducts["incoming price"]),
                        Convert.ToString(dbReaderProducts["party"])
                     });
                    listView2.Items.Add(itemProducts);
                }
                while (dbReaderSales.Read())
                {
                    ListViewItem itemSales = new ListViewItem(new string[]
                     {
                        Convert.ToString(dbReaderSales["id"]),
                        Convert.ToString(dbReaderSales["product_id"]),
                        Convert.ToString(dbReaderSales["outlet_id"]),
                        Convert.ToString(dbReaderSales["sale price"]),
                        Convert.ToString(dbReaderSales["date of sale"])
                     });
                    listView3.Items.Add(itemSales);
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
        }
        private void InputNumber_TextChanged(object sender, EventArgs e)
        {
            CountOfSales = InputNumber.Text;
        }
    }
}
