using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IMDB.Data.Objects;
using IMDB.Data.Repositories;
namespace IMDB.Services
{
    public class IMDBService
    {
        private readonly IMDBRepo _IMDBRepo;
        public FileSet CurrentFileSet { get; private set; }

        public IMDBService(IMDBRepo imdbRepo)
        {
            _IMDBRepo = imdbRepo;
            CurrentFileSet = new FileSet(@"C:\QuaverLogs");
        }

        public void FirstSQLStatment()
        {
            _IMDBRepo.TestInsert("JJ");
        }

        public void ReadInFile()
        {
            foreach (var file in CurrentFileSet.Files)
            {
                switch (file.Name)
                {
                    case FileSet.NAME_BASICS_FILE_NAME:
                        {
                            _IMDBRepo.PersistNameBasics(file.FullName);
                            break;
                        }
                        //case FileSet.TITLE_AKAS_FILE_NAME:
                        //    {
                        //        _IMDBRepo.PersistTitleAkas(file.FullName);
                        //        break;
                        //    }
                        //case FileSet.TITLE_BASICS_FILE_NAME:
                        //    {
                        //        _IMDBRepo.PersistTitleBasics(file.FullName);
                        //        break;
                        //    }
                        //case FileSet.TITLE_CREW_FILE_NAME:
                        //    {
                        //        _IMDBRepo.PersistTitleCrew(file.FullName);
                        //        break;
                        //    }
                        //case FileSet.TITLE_EPISODE_FILE_NAME:
                        //    {
                        //        _IMDBRepo.PersistTitleEpisode(file.FullName);
                        //        break;
                        //    }
                        //case FileSet.TITLE_PRINCIPALS_FILE_NAME:
                        //    {
                        //        _IMDBRepo.PersistTitlePrincipals(file.FullName);
                        //        break;
                        //    }
                        //case FileSet.TITLE_RATING_FILE_NAME:
                        //    {
                        //        _IMDBRepo.PersistTitleRating(file.FullName);
                        //        break;
                        //    }
                }
            }


            //Parallel.ForEach(CurrentFileSet.Files, new ParallelOptions { MaxDegreeOfParallelism = Math.Min(Environment.ProcessorCount, 4) }, file =>
            //{
            //    switch (file.Name)
            //    {
            //        case FileSet.NAME_BASICS_FILE_NAME:
            //            {
            //                _IMDBRepo.PersistNameBasics(file.FullName);
            //                break;
            //            }
            //        case FileSet.TITLE_AKAS_FILE_NAME:
            //            {
            //                _IMDBRepo.PersistTitleAkas(file.FullName);
            //                break;
            //            }
            //        case FileSet.TITLE_BASICS_FILE_NAME:
            //            {
            //                _IMDBRepo.PersistTitleBasics(file.FullName);
            //                break;
            //            }
            //        case FileSet.TITLE_CREW_FILE_NAME:
            //            {
            //                _IMDBRepo.PersistTitleCrew(file.FullName);
            //                break;
            //            }
            //        case FileSet.TITLE_EPISODE_FILE_NAME:
            //            {
            //                _IMDBRepo.PersistTitleEpisode(file.FullName);
            //                break;
            //            }
            //        case FileSet.TITLE_PRINCIPALS_FILE_NAME:
            //            {
            //                _IMDBRepo.PersistTitlePrincipals(file.FullName);
            //                break;
            //            }
            //        case FileSet.TITLE_RATING_FILE_NAME:
            //            {
            //                _IMDBRepo.PersistTitleRating(file.FullName);
            //                break;
            //            }
            //    }
            //});
        }
    }
}
