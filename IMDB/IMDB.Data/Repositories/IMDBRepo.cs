using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace IMDB.Data.Repositories
{
    public class IMDBRepo : sqlBase
    {
        private const int ROWS_TO_PROCESS = 1000;

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

        public void PersistNameBasics(string filePath)
        {
            StringBuilder sb1 = new StringBuilder();
            StringBuilder valuesText = new StringBuilder();

            String[] content = File.ReadAllLines(filePath);
            sb1.Append("Insert INTO NameBasics ('nconst', 'primaryName', 'brithYear', 'deathYear', 'primaryProfession', 'knownForTitles') Values");
            string onDuplicateText = @" ON DUPLICATE KEY UPDATE 
                                                        primaryName = VALUES(primaryName),
                                                        brithYear = VALUES(brithYear),
                                                        deathYear = VALUES(deathYear),
                                                        primaryProfession = VALUES(primaryProfession),
                                                        knownForTitles = VALUES(knownForTitles);";
            int counter = 0;
            var sqlParams = new List<MySqlParameter>();
            MySqlCommand command = connection.CreateCommand();
            foreach (var i in content)
            {
                if (counter == 0)
                {
                    counter++;
                    continue;
                }
                valuesText.Append("?nconst" + counter.ToString() + "," +
                                   "?primaryName" + counter.ToString() + "," +
                                   "?brithYear" + counter.ToString() + "," +
                                   "?deathYear" + counter.ToString() + "," +
                                   "?primaryProfession" + counter.ToString() + "," +
                                   "?knownForTitles" + counter.ToString() + ",");

                command.Parameters.AddWithValue("?nconst" + counter.ToString(), i[0]);
                command.Parameters.AddWithValue("?primaryName" + counter.ToString(), i[1]);
                command.Parameters.AddWithValue("?brithYear" + counter.ToString(), i[2]);
                command.Parameters.AddWithValue("?deathYear" + counter.ToString(), i[3]);
                command.Parameters.AddWithValue("?primaryProfession" + counter.ToString(), i[4]);
                command.Parameters.AddWithValue("?knownForTitles" + counter.ToString(), i[5]);
                counter++;
                if (counter % ROWS_TO_PROCESS == 0)
                {
                    string testcomd = sb1.ToString() + valuesText.ToString().TrimEnd(',') + onDuplicateText;
                    if (OpenConnection() == true)
                    {
                        command.CommandText = sb1.ToString() + valuesText.ToString().TrimEnd(',') + onDuplicateText;

                        //Execute command
                        command.ExecuteNonQuery();
                        sqlParams.Clear();
                        valuesText.Clear();
                    }
                }
            }

            if (!string.IsNullOrEmpty(valuesText.ToString()))
            {
                if (OpenConnection() == true)
                {
                    command.CommandText = sb1.ToString() + valuesText.ToString().TrimEnd(',') + onDuplicateText;

                    //Execute command
                    command.ExecuteNonQuery();
                }
            }
        }

        public void PersistTitleAkas(string filePath)
        {
        }

        public void PersistTitleBasics(string filePath)
        {

        }

        public void PersistTitleCrew(string filePath)
        {

        }

        public void PersistTitleEpisode(string filePath)
        {

        }

        public void PersistTitlePrincipals(string filePath)
        {

        }

        public void PersistTitleRating(string filePath)
        {

        }
    }
}
