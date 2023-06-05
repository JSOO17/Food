using System;
using System.Collections.Generic;

namespace Foods.Infrastructure.Data.Models
{
    public partial class Order
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public DateTime Date { get; set; }
        public string State { get; set; } = null!;
        public string? Code { get; set; }
        public long ChefId { get; set; }
        public long RestaurantId { get; set; }

        public virtual Restaurantemployee Chef { get; set; } = null!;
        public virtual Restaurant Restaurant { get; set; } = null!;
    }
}
