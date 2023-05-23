using System;
using System.Collections.Generic;

namespace Foods.Infrastructure.Data.Models
{
    public partial class Dish
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string UrlImagen { get; set; } = null!;
        public bool IsActive { get; set; }
        public long CategoryId { get; set; }
        public long RestaurantId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Restaurant Restaurant { get; set; } = null!;
    }
}
