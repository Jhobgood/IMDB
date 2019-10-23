using System;
using IMDB.Data.Repositories;
namespace IMDB.Services
{
    public class IMDBService
    {
        private readonly IMDBRepo _IMDBRepo;

        public IMDBService(IMDBRepo imdbRepo)
        {
            _IMDBRepo = imdbRepo;
        }

        public void FirstSQLStatment()
        {
            _IMDBRepo.TestInsert();
        }
    }
}
