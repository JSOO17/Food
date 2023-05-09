using Foods.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foods.Domain.Interfaces
{
    public interface IRestaurantServicesPort
    {
        Task CreateRestaurant(Restaurant restaurant);

        Task CreateDish(Dish dish);
    }
}
