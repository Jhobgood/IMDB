using IMDB.Data.Objects;
using IMDB.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDownloaderAndExtractor
{
    public class InsertFilesetIntoDatabase
    {
        private readonly IMDBRepo _IMDBRepo;
        public FileSet CurrentFileSet { get; private set; }

        public InsertFilesetIntoDatabase()
        {
            string connstring = "Server=localhost;Database=imdb;Password=Samsung55;UserID=root";
            _IMDBRepo = new IMDBRepo(connstring);
            CurrentFileSet = new FileSet(@"C:\QuaverLogs");
        }

        public void ReadInFile()
        {
            Parallel.ForEach(CurrentFileSet.Files, new ParallelOptions { MaxDegreeOfParallelism = Math.Min(Environment.ProcessorCount, 4) }, file =>
            {
                switch (file.Name)
                {
                    case FileSet.NAME_BASICS_FILE_NAME:
                        {
                            _IMDBRepo.PersistNameBasics(file.FullName);
                            break;
                        }
                    case FileSet.TITLE_AKAS_FILE_NAME:
                        {
                            _IMDBRepo.PersistTitleAkas(file.FullName);
                            break;
                        }
                    case FileSet.TITLE_BASICS_FILE_NAME:
                        {
                            _IMDBRepo.PersistTitleBasics(file.FullName);
                            break;
                        }
                    case FileSet.TITLE_CREW_FILE_NAME:
                        {
                            _IMDBRepo.PersistTitleCrew(file.FullName);
                            break;
                        }
                    case FileSet.TITLE_EPISODE_FILE_NAME:
                        {
                            _IMDBRepo.PersistTitleEpisode(file.FullName);
                            break;
                        }
                    case FileSet.TITLE_PRINCIPALS_FILE_NAME:
                        {
                            _IMDBRepo.PersistTitlePrincipals(file.FullName);
                            break;
                        }
                    case FileSet.TITLE_RATING_FILE_NAME:
                        {
                            _IMDBRepo.PersistTitleRating(file.FullName);
                            break;
                        }
                }
            });
        }
    }
}
