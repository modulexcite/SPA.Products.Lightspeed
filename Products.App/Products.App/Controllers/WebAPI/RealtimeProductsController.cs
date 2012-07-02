using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Products.Entities.Models;
using Products.App.Infrastructure;
using Products.Entities.DTO;
using Products.App.Hubs;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using Generic.Util.Logging;

namespace Products.App.Controllers.WebAPI
{
    public class RealTimeProductsController : RealTimeBaseController<ProductsHub>
    {
        private static IProductRepository _repo;
        private static ILogger _logger;

        public RealTimeProductsController(IProductRepository repo, ILogger logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public IEnumerable<ProductDTO> GetProducts()
        {
            lock (_repo)
            {
                lock (_logger) { _logger.Info("Retrieving products - real time");}
                var products = _repo.Products.Select(i => new ProductDTO(i)).ToList();
                return products;
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage PostProduct(ProductDTO prod)
        {
            lock (_repo)
            {
                if (ModelState.IsValid)
                {
                    lock (_logger) { _logger.Info("Adding product - real time - " + prod.Name); }
                    var newprod = _repo.AddProduct(prod, true);

                    if (newprod != null)
                    {
                        var dto = new ProductDTO(newprod);
                        Hub.Clients.addP(dto);
                        lock (_logger) { _logger.Info("Added product - notifying clients, real time - " + prod.Name); }
                        return Request.CreateResponse(HttpStatusCode.Created, dto);
                    }
                }
                else
                {
                    lock (_logger) { _logger.Warn("Model state was invalid - " + prod.Name); }
                    return  Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }

            lock (_logger) { _logger.Error("Model was not bound when trying to add product"); }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage RemoveProduct([FromBody]string guid)
        {
            lock (_repo)
            {
                if (ModelState.IsValid)
                {
                    lock (_logger) { _logger.Info("Deleting product - real time - " + guid); }
                    var success = _repo.DeleteEntity<Product>(guid);

                    if (success)
                    {
                        Hub.Clients.deleteP(guid);
                        lock (_logger) { _logger.Info("Deleted product - notifying clients, real time - " + guid); }
                        return Request.CreateResponse(HttpStatusCode.Created, guid);
                    }
                }
                else
                {
                    lock (_logger) { _logger.Warn("Model state was invalid - " + guid); }
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }

            lock (_logger) { _logger.Error("Model was not bound when trying to remove product"); }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

    }
}
