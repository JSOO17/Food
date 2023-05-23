using System;
using System.Collections.Generic;

namespace Foods.Infrastructure.Data.Models
{
    public partial class Orderdish
    {
        public int OrderId { get; set; }
        public long DishId { get; set; }
        public long Cuantity { get; set; }
    }
}
