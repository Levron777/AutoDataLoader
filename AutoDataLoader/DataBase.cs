using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDataLoader
{
    class DataBase
    {
        SqlConnection dbConnection = new SqlConnection(@"Data Source=DESKTOP-A14PILH\DEV;Initial Catalog=saleCarsDB;Integrated Security=True");

        public async void OpenConnection()
        {
            if (dbConnection.State != ConnectionState.Open)
                await dbConnection.OpenAsync();
        }

        public void CloseConnection()
        {
            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
                dbConnection.Close();
        }

        public SqlConnection GetConnection()
        {
            return dbConnection;
        }
    }
}
