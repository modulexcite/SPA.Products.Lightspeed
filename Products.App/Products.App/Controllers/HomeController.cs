using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Products.Entities.Models;
using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Web.Mvc;

namespace Products.App.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository _repo;

        public HomeController(IProductRepository repo)
        {
            _repo = repo;
            //not used anyway - we AJAX
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
