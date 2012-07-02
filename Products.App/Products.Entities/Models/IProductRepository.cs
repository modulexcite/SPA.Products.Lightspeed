namespace Products.Entities.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mindscape.LightSpeed;
    using Products.Entities.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IProductRepository : IRepository
    {
        IEnumerable<Product> Products { get; }
        IEnumerable<Color> Colors { get; }

        T GetEntity<T>(string guid) where T : Entity;
        bool DeleteEntity<T>(string guid) where T : Entity;

        Guid? AddProduct(Product product);
        Guid? AddProduct(ProductDTO product);
        Product AddProduct(ProductDTO product, bool full);

        bool UpdateProduct(Product product);
        bool UpdateProduct(ProductDTO product);

        Guid? AddColor(Color color);
        Guid? AddColor(ColorDTO color);
    }
}
