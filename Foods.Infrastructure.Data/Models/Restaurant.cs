using System;
using System.Collections.Generic;

namespace Foods.Infrastructure.Data.Models
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            Dishes = new HashSet<Dish>();
            Orders = new HashSet<Order>();
            Restaurantemployees = new HashSet<Restaurantemployee>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Cellphone { get; set; } = null!;
        public string UrlLogo { get; set; } = null!;
        public int Nit { get; set; }
        public long OwnerId { get; set; }

        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Restaurantemployee> Restaurantemployees { get; set; }
    }
}
