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
            FormSubmit t = new FormSubmit();
            return View(t);
        }

        [HttpPost]
        public void Save(FormSubmit i)
        {
            
        }
    }
}
