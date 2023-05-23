using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foods.Domain.Models
{
    public class RestaurantModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int OwnerId { get; set; }
        public string Cellphone { get; set; }
        public string UrlLogo { get; set; }
        public int Nit { get; set; }
    }
}
