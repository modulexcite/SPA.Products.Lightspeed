// -----------------------------------------------------------------------
// <copyright file="ProductDTO.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Products.Entities.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;
    using Products.Entities.Models;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [DataContract]
    public class ProductDTO
    {
        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; }

        [DataMember]
        [RegularExpression("([0-9]+)")]
        public double Price { get; set; }

        [DataMember]
        [RegularExpression("([0-9]+)")]
        public Int32 Quantity { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public DateTime LastUpdated { get; set; }

        [DataMember]
        public IEnumerable<ColorDTO> Colors { get; set; }

        public ProductDTO() { }

        public ProductDTO(Product prod)
        {
              ID = prod.Id;
              Name = prod.Name;
              Price = prod.Price;
              Quantity = prod.Quantity;
              Created = prod.CreatedOn;
              LastUpdated = prod.UpdatedOn;
              Colors = prod.Colors.Select(i => new ColorDTO(i));
        }
    }
}
