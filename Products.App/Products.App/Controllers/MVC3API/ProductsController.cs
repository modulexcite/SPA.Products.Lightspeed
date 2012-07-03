using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Products.Entities.DTO;
using Products.App.Infrastructure;
using Products.Entities.Models;
using Generic.Util.Logging;

namespace Products.App.Controllers.MVC3API
{
    public class ProductsController : Controller
    {
        private IProductRepository _repo;
        private ILogger _logger;

        public ProductsController(IProductRepository repo, ILogger logger)
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
        public ActionResult ProductsFilter(int? take, int? offset, string order)
        {
            _logger.Info(string.Format("Retrieving products - take {0}, offset {1}, order {2}",take, offset, order));

            IEnumerable<ProductDTO> products = _repo.Products.Select(i => new ProductDTO(i));
            var result = products.Count();

            int _take = take ?? result;
            int _offset = offset ?? 0;
            string _order = order ?? "Name";

            if (_offset >= result)
            {
                _offset = 0;
            }

            if (_take > result)
            {
                _take = result;
            }
                
            switch (_order.ToLowerInvariant())
            {
                case "created":
                    products = products.OrderBy(i => i.Created).Reverse().ToList().Skip(_offset).Take(_take);
                    break;
                case "updated":
                    products = products.OrderBy(i => i.LastUpdated).Reverse().ToList().Skip(_offset).Take(_take);
                    break;
                default:
                    products = products.OrderBy(i => i.Name).ToList().Skip(_offset).Take(_take);
                    break;
            }
            return new JsonNetResult() { Data = new { result = products, count = _repo.Products.ToList().Count } };
        }

        [HttpGet]
        public ActionResult Product(string guid)
        {
            _logger.Info("Retrieving product by ID - " + guid);
            var product = _repo.GetEntity<Product>(guid);
            return new JsonNetResult() { Data = new ProductDTO(product) };
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
