using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using Products.Entities.Models;
using Products.Entities.DTO;
using Mindscape.LightSpeed;
using Generic.Util.Logging;

namespace Products.App.Controllers.WebAPI
{
    public class ValuesController : ApiController
    {
        private static IProductRepository _repo;
        private static ILogger _logger;

        public ValuesController(IProductRepository repo, ILogger logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [System.Web.Http.HttpGet]
        public IEnumerable<ProductDTO> Products()
        {
            lock (_repo)
            {
                var products = _repo.Products.Select(i => new ProductDTO(i)).ToList();
                return products;
            }
        }

        [System.Web.Http.HttpGet]
        public IEnumerable<ColorDTO> Colors()
        {
            lock (_repo)
            {
                var colors = _repo.Colors.Select(i => new ColorDTO(i)).ToList();
                return colors;
            }
        }

    }
}
