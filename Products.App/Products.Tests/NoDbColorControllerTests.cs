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
    public class NoDbColorControllerTests
    {
        [Test]
        public void ColorController_NotNull() {

            //arrange
            var repo = Setup.SetupMockRepository();

            var api = new ColorsController(repo.Object, new NLogger(this.GetType().Name));

            //assert
            Assert.That(api, Is.Not.Null);
        }

        [Test]
        public void GetColors_NotNull()
        {
            //arrange
            var repo = Setup.SetupMockRepository();

            var api = new ColorsController(repo.Object, new NLogger(this.GetType().Name));
            var colors = ((JsonNetResult)api.Colors()).Data;

            //assert
            Assert.That(colors, Is.Not.Null);
        }

        [Test]
        public void GetColors_CastedAsColorDTONotNull()
        {
            //arrange
            var repo = Setup.SetupMockRepository();

            var api = new ColorsController(repo.Object, new NLogger(this.GetType().Name));
            var colors = ((JsonNetResult)api.Colors()).Data as List<ColorDTO>;

            //assert
            Assert.That(colors, Is.Not.Null);
        }

        [Test]
        public void GetColors_ColorCountEqualsColorDTOCount()
        {
            //arrange
            var repo = Setup.SetupMockRepository();

            var api = new ColorsController(repo.Object, new NLogger(this.GetType().Name));
            var colors = ((JsonNetResult)api.Colors()).Data as List<ColorDTO>;

            //assert
            Assert.That(repo.Object.Colors.Count(), Is.EqualTo(colors.Count()));
        }

        [Test]
        public void AddColor()
        {
            //arrange
            var repo = Setup.SetupMockRepository();

            var api = new ColorsController(repo.Object, new NLogger(this.GetType().Name));
            var count = repo.Object.Colors.Count();

            var colorDTO = new ColorDTO()
            {
                Name = "test"
            };

            JObject result = JObject.FromObject(((JsonNetResult)api.AddColor(colorDTO)).Data);
            var success = (bool)result.Property("success");

            //assert
            Assert.That(count+1, Is.EqualTo(repo.Object.Colors.Count()));
            Assert.That(success, Is.True);
        }

        [Test]
        public void DeleteColor()
        {
            //arrange
            var repo = Setup.SetupMockRepository();

            var api = new ColorsController(repo.Object, new NLogger(this.GetType().Name));
            var count = repo.Object.Colors.Count();

            JObject result = JObject.FromObject(((JsonNetResult)api.DeleteColor("ad9adb06-ec7a-4b93-b952-dee2e409ca2e")).Data);
            var success = (bool)result.Property("success");

            //assert
            Assert.That(count - 1, Is.EqualTo(repo.Object.Colors.Count()));
            Assert.That(success, Is.True);
        }
    }
}
