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


        public void TestInsert(string userName)
        {
            //open connection
            if (OpenConnection() == true)
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO users (username)
                                         VALUES(?TestName)";
                command.Parameters.AddWithValue("?TestName", userName);

                //Execute command
                command.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }

        }
    }
}
