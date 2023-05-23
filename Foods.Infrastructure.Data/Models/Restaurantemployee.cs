using System;
using System.Collections.Generic;

namespace Foods.Infrastructure.Data.Models
{
    public partial class Restaurantemployee
    {
        public Restaurantemployee()
        {
            Orders = new HashSet<Order>();
        }

        public long Id { get; set; }
        public long RestaurantId { get; set; }
        public long EmployeeId { get; set; }

        public virtual Restaurant Restaurant { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
