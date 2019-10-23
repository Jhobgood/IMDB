using System;
using MySql.Data.MySqlClient;

namespace IMDB.Data.Repositories
{
    public class IMDBRepo : Base
    {
        public IMDBRepo(string connString) : base(connString)
        {

        }

        public void TestInsert()
        {
            string cmdText = @"INSERT INTO new_table (testInsert)
                              VALUES (?values)";


            //var i = Select<int>(cmdText, new MySqlParameter("?id", 1));
            Insert(cmdText, new MySqlParameter("?values", "asdlfjalskdj"));
        }
    }
}
