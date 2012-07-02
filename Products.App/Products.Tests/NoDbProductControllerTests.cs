using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Products.Tests.Common;
using Products.App.Controllers.MVC3API;
using Products.App.Infrastructure;
using Newtonsoft.Json;
using Products.Entities.Models;
using Products.Entities.DTO;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Products.Tests
{
    [TestFixture]
    public class NoDbProductControllerTests
    {
        [Test]
        public void Controller_NotNull() {

            //arrange
            var repo = Setup.SetupMockRepository();

            var api = new ProductsController(repo.Object, new NLogger(this.GetType().Name));

            //assert
            Assert.That(api, Is.Not.Null);
        }

        [Test]
        public void GetProducts_NotNull()
        {
            //arrange
            var repo = Setup.SetupMockRepository();

            var api = new ProductsController(repo.Object, new NLogger(this.GetType().Name));
            var products = ((JsonNetResult)api.Products()).Data;

            //assert
            Assert.That(products, Is.Not.Null);
        }

        [Test]
        public void GetProducts_CastedAsProductDTONotNull()
        {
            //arrange
            var repo = Setup.SetupMockRepository();

            var api = new ProductsController(repo.Object, new NLogger(this.GetType().Name));
            var products = ((JsonNetResult)api.Products()).Data as List<ProductDTO>;

            //assert
            Assert.That(products, Is.Not.Null);
        }

        [Test]
        public void GetProducts_ProductCountEqualsProductDTOCount()
        {
            //arrange
            var repo = Setup.SetupMockRepository();

            var api = new ProductsController(repo.Object, new NLogger(this.GetType().Name));
            var products = ((JsonNetResult)api.Products()).Data as List<ProductDTO>;

            //assert
            Assert.That(repo.Object.Products.Count(), Is.EqualTo(products.Count()));
        }

        [Test]
        public void AddProduct()
        {
            //arrange
            var repo = Setup.SetupMockRepository();

            var api = new ProductsController(repo.Object, new NLogger(this.GetType().Name));
            var count = repo.Object.Products.Count();

            var productDTO = new ProductDTO()
            {
                Name = "test",
                Price = 50,
                Quantity = 30,
                    Colors = new List<ColorDTO>() {
                            new ColorDTO() {
                                Name = "#00dd00"
                            }
                     }
            };

            JObject result = JObject.FromObject(((JsonNetResult)api.AddProduct(productDTO)).Data);
            var success = (bool)result.Property("success");

            //assert
            Assert.That(count+1, Is.EqualTo(repo.Object.Products.Count()));
            Assert.That(success, Is.True);
        }

        [Test]
        public void DeleteProduct()
        {
            //arrange
            var repo = Setup.SetupMockRepository();

            var api = new ProductsController(repo.Object, new NLogger(this.GetType().Name));
            var count = repo.Object.Products.Count();

            JObject result = JObject.FromObject(((JsonNetResult)api.DeleteProduct("56c9f99d-1b21-4dbf-a151-c2c78298580e")).Data);
            var success = (bool)result.Property("success");

            //assert
            Assert.That(count - 1, Is.EqualTo(repo.Object.Products.Count()));
            Assert.That(success, Is.True);
        }
    }
}
