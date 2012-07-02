using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Products.Entities.Models;
using Products.App.Infrastructure;
using Products.Entities.DTO;
using Generic.Util.Logging;

namespace Products.App.Controllers.MVC3API
{
    public class ColorsController : Controller
    {
        private IProductRepository _repo;
        private ILogger _logger;

        public ColorsController(IProductRepository repo, ILogger logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Colors()
        {
            _logger.Info("Retrieving colors");
            var colors = _repo.Colors.Select(i => new ColorDTO(i)).ToList();
            return new JsonNetResult() { Data = colors };
        }

        [HttpGet]
        public ActionResult Color(string guid)
        {
            _logger.Info("Retrieving color by ID - " + guid);
            var color = _repo.GetEntity<Color>(guid);
            return new JsonNetResult() { Data = new ColorDTO(color) };
        }

        [HttpPost]
        public ActionResult AddColor(ColorDTO color)
        {
            if (ModelState.IsValid)
            {
                _logger.Info("Adding color, Name - " + color.Name);
                var guid = _repo.AddColor(color);
                if (guid != null)
                {
                    _logger.Info("Color added, ID - " + guid);
                    return new JsonNetResult() { Data = new { success = true, ID = guid } };
                }
            }

            _logger.Info("Color could not be added");
            return new JsonNetResult() { Data = new { success = false } };
        }

        [HttpPost]
        public ActionResult DeleteColor(string guid)
        {
            if (ModelState.IsValid)
            {
                var success = _repo.DeleteEntity<Color>(guid);

                if (success)
                    return new JsonNetResult() { Data = new { success = true } };
            }

            return new JsonNetResult() { Data = new { success = false } };
        }
    }
}
