using System;
using MySql.Data.MySqlClient;

namespace IMDB.Data.Repositories
{
    public class IMDBRepo : sqlBase
    {
        public IMDBRepo(string connString)
        {
            Initialize(connString);
        }


        public void TestInsert()
        {
            string cmdText = @"INSERT INTO new_table (testInsert)
                              VALUES (?values)";
            //open connection
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(cmdText, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }

        }
    }
}
