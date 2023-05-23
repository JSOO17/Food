using FluentValidation;
using Foods.Application.DTO.Request;
using Foods.Application.Services.Interfaces;
using Foods.Domain.HttpClients.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foods.Infrastructure.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : BaseController
    {
        private readonly IRestaurantServices _restaurantServices;
        private readonly ILogger<RestaurantController> _logger;
        private readonly IUserMicroHttpClient _userHttpClient;

        public RestaurantController(
                IRestaurantServices restaurantServices,
                ILogger<RestaurantController> logger,
                IUserMicroHttpClient userMicroHttpClient
            ) : base(userMicroHttpClient)
        {
            _restaurantServices = restaurantServices;
            _logger = logger;
            _userHttpClient = userMicroHttpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetRestaurants([FromQuery] int page, [FromQuery] int count)
        {
            try
            {
                return Ok(await _restaurantServices.GetRestaurants(page, count));
            }
            catch(Exception ex)
            {
                _logger.LogError("Something was wrong", ex);

                return StatusCode(500, new ApiResult
                {
                    StatusCode = 500,
                    Message = "Something was wrong"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] RestaurantRequestDTO restaurant)
        {
            try
            {
                await ValidateToken();

                var payload = GetPayload();

                if (payload.RoleId != 1)
                {
                    return StatusCode(403, new ApiResult
                    {
                        StatusCode = 403,
                        Message = $"You dont have permission"
                    });
                }

                var restaurantNew = await _restaurantServices.CreateRestaurant(restaurant);

                return Ok(restaurantNew);
            }
            catch (ValidationException ex)
            {
                _logger.LogDebug("bad request. Errors: {0}", ex.Message);

                return BadRequest(new ApiResult
                {
                    StatusCode = 400,
                    Message = $"Errors: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Something was wrong", ex);

                return StatusCode(500, new ApiResult
                {
                    StatusCode = 500,
                    Message = "Something was wrong"
                });
            }
        }

        [Route("Employee")]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] RestaurantEmployeesRequestDTO restaurantEmployees)
        {
            try
            {
                await ValidateToken();

                var payload = GetPayload();

                var restaurantEmployee = await _restaurantServices.CreateEmployeeRestaurant(restaurantEmployees, payload.UserId);

                return Ok(restaurantEmployee);
            }
            catch (ValidationException ex)
            {
                _logger.LogDebug("bad request. Errors: {0}", ex.Message);

                return BadRequest(new ApiResult
                {
                    StatusCode = 400,
                    Message = $"Errors: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Something was wrong", ex);

                return StatusCode(500, new ApiResult
                {
                    StatusCode = 500,
                    Message = "Something was wrong"
                });
            }
        }
    }

    public class ApiResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
