using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDB.Models;
using IMDB.Services;
using Microsoft.AspNetCore.Mvc;


namespace IMDB.Controllers
{
    [Route("[controller]/[action]")]
    public class IMDBController : Controller
    {
        private readonly IMDBService _imdbService;
        public IMDBController(IMDBService service)
        {
            _imdbService = service;
        }

        public IActionResult IMDB()
        {
            IMDBTest t = new IMDBTest();
            return View(t);
        }

        [HttpPost]
        public void Save(IMDBTest i)
        {
            _imdbService.ReadInFile();
        }
    }
}
