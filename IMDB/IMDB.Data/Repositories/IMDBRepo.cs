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

                //close connection
                CloseConnection();
            }

        }

        public void PersistNameBasics(string filePath)
        {
            try
            {
                if (OpenConnection() == true)
                {
                    string[] content = File.ReadAllLines(filePath);

                    MySqlCommand command = connection.CreateCommand();
                    command.Parameters.AddWithValue("?nconst", "");
                    command.Parameters.AddWithValue("?primaryName", "");
                    command.Parameters.AddWithValue("?primaryProfession", "");
                    command.Parameters.AddWithValue("?knownForTitles", "");
                    int counter = 0;
                    command.CommandText = "Insert INTO imdb.namebasics (`nconst`, `primaryName`, `primaryProfession`, `knownForTitles`) Values (?nconst, ?primaryName, ?primaryProfession, ?knownForTitles) ON DUPLICATE KEY UPDATE primaryName = VALUES(primaryName), primaryProfession = VALUES(primaryProfession), knownForTitles = VALUES(knownForTitles);";

                    foreach (var i in content)
                    {
                        if (counter == 0)
                        {
                            counter++;
                            continue;
                        }
                        string[] splitLineData = i.Split('\t').ToArray();
                        command.Parameters["?nconst"].Value = splitLineData[0];
                        command.Parameters["?primaryName"].Value = splitLineData[1];
                        command.Parameters["?primaryProfession"].Value = splitLineData[4];
                        command.Parameters["?knownForTitles"].Value = splitLineData[5];
                        command.ExecuteNonQuery();
                        counter++;
                    }
                }
            }
            catch
            {

            }
            finally
            {
                CloseConnection();
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
