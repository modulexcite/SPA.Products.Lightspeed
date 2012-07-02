// -----------------------------------------------------------------------
// <copyright file="InMemoryWebApiTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Products.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net.Http;
    using Products.Tests.Common;
    using Products.App.Controllers.WebAPI;
    using Products.Entities.Models;
    using Products.App.Infrastructure;
    using System.Net.Http.Headers;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using Products.Entities.DTO;

    [TestFixture]
    public class InMemoryWebApiTests
    {
        private string _url = "http://tempuri/";

        [Test]
        public void ProductsFromWebAPIReceivedAndCorrect()
        {
            var repo = Setup.SetupMockRepository();

            var controller = new ValuesController(repo.Object, new NLogger(this.GetType().Name));
            var products = controller.Products();
            var expected = repo.Object.Products.Select(i => new ProductDTO(i));
            
            Assert.NotNull(products);
            Assert.AreEqual(expected.Count(), products.Count());
        }

        [Test]
        public void ColorsFromWebAPIReceivedAndCorrect()
        {
            var repo = Setup.SetupMockRepository();

            var controller = new ValuesController(repo.Object, new NLogger(this.GetType().Name));
            var colors = controller.Colors();
            var expected = repo.Object.Colors.Select(i => new ColorDTO(i));

            Assert.NotNull(colors);
            Assert.AreEqual(expected.Count(), colors.Count());
        }

        [Test]
        public void ProductsFromWebAPIviaHTTP()
        {
            var repo = Setup.SetupMockRepository();
            var client = new HttpClient(Setup.SetupInMemoryWebServer());
            var request = createRequest("api/values/products", "application/json", HttpMethod.Get);
            var expectedJson = JsonConvert.SerializeObject(Setup.Products.Select(i => new ProductDTO(i)));

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                Assert.NotNull(response);
                Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.AreEqual(JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(expectedJson).Count(), Setup.Products.Count());
            }

            request.Dispose();
        }

        [Test]
        public void ColorsFromWebAPIviaHTTP()
        {
            var repo = Setup.SetupMockRepository();
            var client = new HttpClient(Setup.SetupInMemoryWebServer());
            var request = createRequest("api/values/colors", "application/json", HttpMethod.Get);
            var expectedJson = JsonConvert.SerializeObject(Setup.Colors.Select(i => new ColorDTO(i)));

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                Assert.NotNull(response);
                Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.AreEqual(JsonConvert.DeserializeObject<IEnumerable<ColorDTO>>(expectedJson).Count(), Setup.Colors.Count());
            }

            request.Dispose();
        }

        private HttpRequestMessage createRequest(string url, string mthv, HttpMethod method)
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(_url + url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mthv));
            request.Method = method;

            return request;
        }
    }
}
