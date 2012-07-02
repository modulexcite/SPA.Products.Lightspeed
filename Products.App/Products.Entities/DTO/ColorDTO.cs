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
    public class ColorDTO
    {
        [DataMember]
        public Guid ID { get; set; }

        [DataMember]
        [StringLength(40,MinimumLength=3)]
        public string Name { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public DateTime LastUpdated { get; set; }

        public ColorDTO() { }

        public ColorDTO(Color color) {
            ID = color.Id;
            Name = color.Name;
            Created = color.CreatedOn;
            LastUpdated = color.UpdatedOn;
        }
    }
}
