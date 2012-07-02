// -----------------------------------------------------------------------
// <copyright file="Setup.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Products.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Products.Entities.Models;
    using Mindscape.LightSpeed;
    using Moq;
    using System.Threading.Tasks;
    using Products.Entities.DTO;
    using Products.Entities;
    using System.Web.Http;
    using Ninject;
    using Generic.Util.Logging;
    using Products.App.Infrastructure;

    public class Setup
    {
        private static List<Product> _products = new List<Product>() {
                new Product {
                   Id = Guid.NewGuid(), 
                   Name = "test product1", 
                   Price = 10, 
                   Quantity = 1
                },
                new Product {
                   Id = Guid.NewGuid(), 
                   Name = "test product2", 
                   Price = 20, 
                   Quantity = 2, 
                },
                new Product {
                   Id = new Guid("56c9f99d-1b21-4dbf-a151-c2c78298580e"), 
                   Name = "test product3", 
                   Price = 30, 
                   Quantity = 3
                }
            };

        private static List<Color> _colors = new List<Color>()
        {
              new Color() {Id = new Guid("ad9adb06-ec7a-4b93-b952-dee2e409ca2e"), Name = "#f2f2f2"},
              new Color() {Name = "#f3f3f3"},
              new Color() {Name = "#f4f4f4"}
        };

        public static List<Product> Products
        {
            get
            {
                return _products;
            }

            set
            {
                _products = value;
            }
        }

        public static List<Color> Colors
        {
            get
            {
                return _colors;
            }

            set
            {
                _colors = value;
            }
        }

        public static Mock<IProductRepository> SetupMockRepository()
        {
            var repo = new Mock<IProductRepository>();

            Products[0].Colors.Add(Colors[0]);
            Products[0].Colors.Add(Colors[1]);
            Products[1].Colors.Add(Colors[2]);

            repo.Setup(i => i.Products).Returns(Products.AsEnumerable<Product>);
            repo.Setup(i => i.Colors).Returns(Colors.AsEnumerable<Color>);

            Guid? guid = null;
            repo.Setup(i => i.AddProduct(It.IsAny<ProductDTO>())).Callback((ProductDTO t) =>
                {
                    var prod = DTOConvert.ConvertProductDTO(t);
                    prod.Id = Guid.NewGuid();
                    Products.Add(prod);

                    guid = prod.Id;
                }).Returns(() => guid);

            bool success = false;
            repo.Setup(i => i.DeleteEntity<Product>(It.IsAny<string>())).Callback((string t) =>
                {
                    var prod = Products.Find(i => i.Id == new Guid(t));
                    Products.Remove(prod);
                    success = true;
                }).Returns(() => success);

            Guid? colorguid = null;
            repo.Setup(i => i.AddColor(It.IsAny<ColorDTO>())).Callback((ColorDTO c) =>
                {
                    var color = DTOConvert.ConvertColorDTO(c);
                    color.Id = Guid.NewGuid();
                    Colors.Add(color);

                    colorguid = color.Id;
                }).Returns(() => colorguid);

            bool colorsuccess = false;
            repo.Setup(i => i.DeleteEntity<Color>(It.IsAny<string>())).Callback((string c) =>
                {
                    var col = Colors.Find(i => i.Id == new Guid(c));
                    Colors.Remove(col);
                    colorsuccess = true;
                }).Returns(() => colorsuccess);

            return repo;
        }

        public static HttpServer SetupInMemoryWebServer()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(name: "Default", routeTemplate: "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            var kernel = new StandardKernel();
            //kernel.Bind<IProductRepository>().To<ProductRepository>();
            kernel.Bind<IProductRepository>().ToMethod(x => SetupMockRepository().Object);
            kernel.Bind<ILogger>().To<NLogger>().WithConstructorArgument("currentClass", x => x.Request.ParentContext.Request.Service.FullName);
            config.DependencyResolver = new NinjectResolver(kernel);

            return new HttpServer(config);
        }
    } 
}
