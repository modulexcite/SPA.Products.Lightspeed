using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mindscape.LightSpeed.Web.Mvc;
using Mindscape.LightSpeed;
using Products.Entities.Models;
using Newtonsoft.Json;
using Products.Entities;
using Newtonsoft.Json.Linq;
using Products.Entities.DTO;
using Products.App.Infrastructure;

namespace Products.App.Controllers.MVC3API
{
    public class NinjectJsonController : Controller
    {
        private IProductRepository _repo;
        private ILogger _logger;

        public NinjectJsonController(IProductRepository repo, ILogger logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Products()
        {
            _logger.Info("Retrieving products");
            var products = _repo.Products.Select(i => new ProductDTO(i)).ToList();
            return new JsonNetResult() { Data = products };
        }

        [HttpGet]
        public ActionResult ProductsFilter(int take, int offset, string order)
        {
            _logger.Info("Retrieving products - take " + take + ", offset " + offset);
            
            IEnumerable<ProductDTO> products;
            switch (order)
            {
                case "created":
                    products = _repo.Products.Select(i => new ProductDTO(i)).OrderBy(i => i.Created).Reverse().ToList().Skip(offset).Take(take);
                    break;
                case "updated":
                    products = _repo.Products.Select(i => new ProductDTO(i)).OrderBy(i => i.LastUpdated).Reverse().ToList().Skip(offset).Take(take);
                    break;
                default:
                    products = _repo.Products.Select(i => new ProductDTO(i)).OrderBy(i => i.Name).ToList().Skip(offset).Take(take);
                    break;
            }
            return new JsonNetResult() { Data = new { result = products, count = _repo.Products.ToList().Count } };
        }

        [HttpGet]
        public ActionResult Colors()
        {
            _logger.Info("Retrieving colors");
            var colors = _repo.Colors.Select(i => new ColorDTO(i)).ToList();
            return new JsonNetResult() { Data = colors };
        }

        [HttpGet]
        public ActionResult Product(string guid)
        {
            _logger.Info("Retrieving product by ID - " + guid);
            var product = _repo.GetEntity<Product>(guid);
            return new JsonNetResult() { Data = new ProductDTO(product) };
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
        public ActionResult AddProduct(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                _logger.Info("Adding product, Name - " + product.Name);
                var guid = _repo.AddProduct(product);

                if (guid != null)
                {
                    _logger.Info("Product added, ID - " + guid);
                    return new JsonNetResult() { Data = new { success = true, ID = guid } };
                }
            }

            _logger.Info("Product could not be added");
            return new JsonNetResult() { Data = new { success = false } };
        }

        [HttpPost]
        public ActionResult DeleteProduct(string guid)
        {
            if (ModelState.IsValid)
            {
                var success = _repo.DeleteEntity<Product>(guid);
                
                if (success)
                    return new JsonNetResult() { Data = new { success = true } };
            }

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

        [HttpPost]
        public ActionResult UpdateProduct(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                var success = _repo.UpdateProduct(product);

                if (success)
                    return new JsonNetResult() { Data = new { success = true } };
            }

            return new JsonNetResult() { Data = new { success = false } };
        }
    }
}
