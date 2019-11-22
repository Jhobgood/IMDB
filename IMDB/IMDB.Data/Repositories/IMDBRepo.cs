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
                //if (OpenConnection() == true)
                //{
                    string[] content = File.ReadAllLines(filePath);

                    MySqlCommand command = connection.CreateCommand();
                    command.Parameters.AddWithValue("?nconst", "");
                    command.Parameters.AddWithValue("?primaryName", "");
                    command.Parameters.AddWithValue("?birthYear", "");
                    command.Parameters.AddWithValue("?deathYear", "");
                    command.Parameters.AddWithValue("?primaryProfession", "");
                    command.Parameters.AddWithValue("?knownForTitles", "");
                    int counter = 0;
                    command.CommandText = "Insert INTO imdb.namebasics (`nconst`, `primaryName`, `birthYear`, `deathYear` `primaryProfession`, `knownForTitles`) Values (?nconst, ?primaryName,, ?birthYear, ?deathYear ?primaryProfession, ?knownForTitles) ON DUPLICATE KEY UPDATE primaryName = VALUES(primaryName), primaryProfession = VALUES(primaryProfession), knownForTitles = VALUES(knownForTitles);";

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
                        command.Parameters["?birthYear"].Value = splitLineData[2] == @"\N" ? null : splitLineData[2];
                        command.Parameters["?deathYear"].Value = splitLineData[3] == @"\N" ? null : splitLineData[2];
                        command.Parameters["?primaryProfession"].Value = splitLineData[4];
                        command.Parameters["?knownForTitles"].Value = splitLineData[5];
                        command.ExecuteNonQuery();
                        counter++;
                    }
                //}
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
            try
            {
                if (OpenConnection() == true)
                {
                    string[] content = File.ReadAllLines(filePath);

                    MySqlCommand command = connection.CreateCommand();
                    command.Parameters.AddWithValue("?titleId", "");
                    command.Parameters.AddWithValue("?ordering", "");
                    command.Parameters.AddWithValue("?title", "");
                    command.Parameters.AddWithValue("?region", "");
                    command.Parameters.AddWithValue("?language", "");
                    command.Parameters.AddWithValue("?types", "");
                    command.Parameters.AddWithValue("?attributes", "");
                    command.Parameters.AddWithValue("?isOriginalTitle", "");
                    int counter = 0;
                    command.CommandText = "Insert INTO imdb.titleakas (`titleId`, `ordering`, `title`, `region`, `language`, `types`, `attributes`, `isOriginalTitle`) " +
                                          "Values (?titleId, ?ordering, ?title, ?region, ?language, ?types, ?attributes, ?isOriginalTitle) " +
                                          "ON DUPLICATE KEY UPDATE ordering = VALUES(ordering), title = VALUES(title), region = VALUES(region), language = VALUES(language), types = VALUES(types), attributes = VALUES(attributes), isOriginalTitle = VALUES(isOriginalTitle);";

                    foreach (var i in content)
                    {
                        if (counter == 0)
                        {
                            counter++;
                            continue;
                        }
                        string[] splitLineData = i.Split('\t').ToArray();
                        command.Parameters["?titleId"].Value = splitLineData[0];
                        command.Parameters["?ordering"].Value = splitLineData[1];
                        command.Parameters["?title"].Value = splitLineData[2];
                        command.Parameters["?region"].Value = splitLineData[3];
                        command.Parameters["?language"].Value = splitLineData[4];
                        command.Parameters["?types"].Value = splitLineData[5];
                        command.Parameters["?attributes"].Value = splitLineData[6];
                        command.Parameters["?isOriginalTitle"].Value = splitLineData[7];
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

        public void PersistTitleBasics(string filePath)
        {
            try
            {
                if (OpenConnection() == true)
                {
                    string[] content = File.ReadAllLines(filePath);

                    MySqlCommand command = connection.CreateCommand();
                    command.Parameters.AddWithValue("?tconst", "");
                    command.Parameters.AddWithValue("?titleType", "");
                    command.Parameters.AddWithValue("?primaryTitle", "");
                    command.Parameters.AddWithValue("?originalTitle", "");
                    command.Parameters.AddWithValue("?isAdult", "");
                    command.Parameters.AddWithValue("?startYear", "");
                    command.Parameters.AddWithValue("?endYear", "");
                    command.Parameters.AddWithValue("?runtimeMinutes", "");
                    command.Parameters.AddWithValue("?genres", "");
                    int counter = 0;
                    command.CommandText = "Insert INTO imdb.titlebasics (`tconst`, `titleType`, `primaryTitle`, `originalTitle`, `isAdult`, `startYear`, `endYear`, `runtimeMinutes`, `genres`) " +
                                          "Values (?tconst, ?titleType, ?primaryTitle, ?originalTitle, ?isAdult, ?startYear, ?endYear, ?runtimeMinutes, ?genres) " +
                                          "ON DUPLICATE KEY UPDATE titleType = VALUES(titleType), primaryTitle = VALUES(primaryTitle), originalTitle = VALUES(originalTitle), isAdult = VALUES(isAdult), startYear = VALUES(startYear), endYear = VALUES(endYear), runtimeMinutes = VALUES(runtimeMinutes), genres = VALUES(genres);";

                    foreach (var i in content)
                    {
                        if (counter == 0)
                        {
                            counter++;
                            continue;
                        }
                        string[] splitLineData = i.Split('\t').ToArray();
                        command.Parameters["?tconst"].Value = splitLineData[0];
                        command.Parameters["?titleType"].Value = splitLineData[1];
                        command.Parameters["?primaryTitle"].Value = splitLineData[2];
                        command.Parameters["?originalTitle"].Value = splitLineData[3];
                        command.Parameters["?isAdult"].Value = splitLineData[4];
                        command.Parameters["?startYear"].Value = splitLineData[5];
                        command.Parameters["?endYear"].Value = splitLineData[6];
                        command.Parameters["?runtimeMinutes"].Value = splitLineData[7];
                        command.Parameters["?genres"].Value = splitLineData[8];
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

        public void PersistTitleCrew(string filePath)
        {
            try
            {
                if (OpenConnection() == true)
                {
                    string[] content = File.ReadAllLines(filePath);

                    MySqlCommand command = connection.CreateCommand();
                    command.Parameters.AddWithValue("?tconst", "");
                    command.Parameters.AddWithValue("?titleType", "");
                    command.Parameters.AddWithValue("?primaryTitle", "");
                    int counter = 0;
                    command.CommandText = "Insert INTO imdb.titlecrew (`tconst`, `directors`, `writers`) " +
                                          "Values (?tconst, ?directors, ?writers) " +
                                          "ON DUPLICATE KEY UPDATE directors = VALUES(directors), writers = VALUES(writers);";

                    foreach (var i in content)
                    {
                        if (counter == 0)
                        {
                            counter++;
                            continue;
                        }
                        string[] splitLineData = i.Split('\t').ToArray();
                        command.Parameters["?tconst"].Value = splitLineData[0];
                        command.Parameters["?titleType"].Value = splitLineData[1];
                        command.Parameters["?primaryTitle"].Value = splitLineData[2];
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

        public void PersistTitleEpisode(string filePath)
        {
            try
            {
                if (OpenConnection() == true)
                {
                    string[] content = File.ReadAllLines(filePath);

                    MySqlCommand command = connection.CreateCommand();
                    command.Parameters.AddWithValue("?tconst", "");
                    command.Parameters.AddWithValue("?parentTconst", "");
                    command.Parameters.AddWithValue("?seasonNumber", "");
                    command.Parameters.AddWithValue("?episodeNumber", "");
                    int counter = 0;
                    command.CommandText = "Insert INTO imdb.titleepisode (`tconst`, `parentTconst`, `seasonNumber`, `episodeNumber`) " +
                                          "Values (?tconst, ?parentTconst, ?seasonNumber, ?episodeNumber) " +
                                          "ON DUPLICATE KEY UPDATE parentTconst = VALUES(parentTconst), seasonNumber = VALUES(seasonNumber), episodeNumber = VALUES(episodeNumber);";

                    foreach (var i in content)
                    {
                        if (counter == 0)
                        {
                            counter++;
                            continue;
                        }
                        string[] splitLineData = i.Split('\t').ToArray();
                        command.Parameters["?tconst"].Value = splitLineData[0];
                        command.Parameters["?parentTconst"].Value = splitLineData[1];
                        command.Parameters["?seasonNumber"].Value = splitLineData[2];
                        command.Parameters["?episodeNumber"].Value = splitLineData[2];
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

        public void PersistTitlePrincipals(string filePath)
        {
            try
            {
                if (OpenConnection() == true)
                {
                    string[] content = File.ReadAllLines(filePath);

                    MySqlCommand command = connection.CreateCommand();
                    command.Parameters.AddWithValue("?tconst", "");
                    command.Parameters.AddWithValue("?ordering", "");
                    command.Parameters.AddWithValue("?nconst", "");
                    command.Parameters.AddWithValue("?category", "");
                    command.Parameters.AddWithValue("?job", "");
                    command.Parameters.AddWithValue("?characters", "");
                    int counter = 0;
                    command.CommandText = "Insert INTO imdb.titleprincipals (`tconst`, `ordering`, `nconst`, `category`, `job`, `characters`) " +
                                          "Values (?tconst, ?ordering, ?nconst, ?category, ?job, ?characters) " +
                                          "ON DUPLICATE KEY UPDATE ordering = VALUES(ordering), nconst = VALUES(nconst), category = VALUES(category), job = VALUES(job), characters = VALUES(characters);";

                    foreach (var i in content)
                    {
                        if (counter == 0)
                        {
                            counter++;
                            continue;
                        }
                        string[] splitLineData = i.Split('\t').ToArray();
                        command.Parameters["?tconst"].Value = splitLineData[0];
                        command.Parameters["?ordering"].Value = splitLineData[1];
                        command.Parameters["?nconst"].Value = splitLineData[2];
                        command.Parameters["?category"].Value = splitLineData[2];
                        command.Parameters["?job"].Value = splitLineData[2];
                        command.Parameters["?characters"].Value = splitLineData[2];
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

        public void PersistTitleRating(string filePath)
        {
            try
            {
                if (OpenConnection() == true)
                {
                    string[] content = File.ReadAllLines(filePath);

                    MySqlCommand command = connection.CreateCommand();
                    command.Parameters.AddWithValue("?tconst", "");
                    command.Parameters.AddWithValue("?directors", "");
                    command.Parameters.AddWithValue("?writers", "");
                    int counter = 0;
                    command.CommandText = "Insert INTO imdb.titlerating (`tconst`, `averageRating`, `numVotes`) " +
                                          "Values (?tconst, ?averageRating, ?numVotes) " +
                                          "ON DUPLICATE KEY UPDATE averageRating = VALUES(averageRating), numVotes = VALUES(numVotes);";

                    foreach (var i in content)
                    {
                        if (counter == 0)
                        {
                            counter++;
                            continue;
                        }
                        string[] splitLineData = i.Split('\t').ToArray();
                        command.Parameters["?tconst"].Value = splitLineData[0];
                        command.Parameters["?averageRating"].Value = splitLineData[1];
                        command.Parameters["?numVotes"].Value = splitLineData[2];
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
    }
}
