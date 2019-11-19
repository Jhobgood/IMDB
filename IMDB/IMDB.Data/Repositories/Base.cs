using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMDB.Data.Repositories
{
    public class sqlBase
    {
        public MySqlConnection connection { get; set; }
        private string server;
        private string database;
        private string uid;
        private string password;

        public void Initialize(string connString)
        {
            connection = new MySqlConnection(connString);
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        // Cannot connect to server.  Contact administrator
                        break;

                    case 1045:
                        // Invalid username/password, please try again
                        break;
                }
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }

        public void AddParam(ref List<MySqlParameter> list, string name, object value)
        {
            MySqlParameter p = new MySqlParameter(name, value ?? DBNull.Value);
            list.Add(p);
        }
    }
}
