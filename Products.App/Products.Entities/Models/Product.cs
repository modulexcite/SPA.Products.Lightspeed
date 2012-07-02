using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mindscape.LightSpeed;

namespace Products.Entities.Models
{
    public partial class Product
    {
        public EntityCollection<Color> Colors
        {
            get { return Get(ProductColor.Colors); }
        }
    }
}
