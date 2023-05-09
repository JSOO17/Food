using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foods.Domain.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Direccion { get; set; }
        public int OwnerId { get; set; }
        public string Phone { get; set; }
        public string UrlLogo { get; set; }
        public string Nit { get; set; }
    }
}
