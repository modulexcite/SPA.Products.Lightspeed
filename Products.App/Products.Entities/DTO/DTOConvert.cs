// -----------------------------------------------------------------------
// <copyright file="DTOConvert.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Products.Entities.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Products.Entities.Models;
    using Products.Entities.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class DTOConvert
    {
        public static Converter<Product, ProductDTO> ConvertProduct =
      (prod) =>
      {
          return new ProductDTO()
          {
              ID = prod.Id,
              Name = prod.Name,
              Price = prod.Price,
              Quantity = prod.Quantity,
              Created = prod.CreatedOn,
              LastUpdated = prod.UpdatedOn,
              Colors = prod.Colors.Select(i => new ColorDTO(i))
          };
      };

        public static Converter<ProductDTO, Product> ConvertProductDTO =
(prod) =>
{
    var product = new Product()
    {
        Id = prod.ID == null ? new Guid() : prod.ID,
        Name = prod.Name,
        Price = prod.Price,
        Quantity = prod.Quantity
    };

    if (prod.Colors != null)
    {
        foreach (var color in prod.Colors)
        {
            product.Colors.Add(DTOConvert.ConvertColorDTO(color));
        }
    }

    return product;
};

        public static Converter<ColorDTO, Color> ConvertColorDTO =
(color) =>
{
    return new Color()
    {
        Id = color.ID,
        Name = color.Name
    };
};

        public static Converter<Color, ColorDTO> ConvertColor =
(color) =>
{
    return new ColorDTO()
    {
        ID = color.Id,
        Name = color.Name
    };
};
    }
}