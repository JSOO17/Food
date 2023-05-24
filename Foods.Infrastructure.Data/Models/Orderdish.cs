using System;
using System.Collections.Generic;

namespace Foods.Infrastructure.Data.Models
{
    public partial class Orderdish
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long DishId { get; set; }
        public int Cuantity { get; set; }
    }
}
