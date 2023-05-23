using System;
using System.Collections.Generic;

namespace Foods.Infrastructure.Data.Models
{
    public partial class Category
    {
        public Category()
        {
            Dishes = new HashSet<Dish>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Dish> Dishes { get; set; }
    }
}
