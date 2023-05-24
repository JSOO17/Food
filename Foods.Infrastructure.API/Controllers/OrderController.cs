using FluentValidation;
using Foods.Application.DTO.Request;
using Foods.Application.Services.Interfaces;
using Foods.Domain.Exceptions;
using Foods.Domain.HttpClients.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Foods.Infrastructure.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderServices _orderServices;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
                IOrderServices orderServices,
                ILogger<OrderController> logger,
                IUserMicroHttpClient userMicroHttpClient
            ) : base(userMicroHttpClient)
        {
            _orderServices = orderServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromBody] OrderFiltersRequest filters,
                                                   [FromQuery] int page,
                                                   [FromQuery] int count)
        {
            try
            {
                await ValidateToken();

                var payload = GetPayload();

                var orderResponse = await _orderServices.GetOrders(filters, page, count, payload.UserId);

                return Ok(orderResponse);

            }
            catch (TokenIsNotValidException ex)
            {
                _logger.LogDebug(MessageUnauthorized, ex.Message);

                return StatusCode(StatusCodes.Status401Unauthorized, new ApiResult
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = $"Errors: {ex.Message}"
                });
            }
            catch (UserIsNotAEmployeeException ex)
            {
                _logger.LogDebug(MessageForbbiden, ex.Message);

                return StatusCode(StatusCodes.Status403Forbidden, new ApiResult
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Message = $"Errors: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something was wrong");

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Something was wrong"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDTO orderRequest)
        {
            try
            {
                await ValidateToken();

                var orderResponse = await _orderServices.CreateOrder(orderRequest);

                return Ok(orderResponse);

            }
            catch (TokenIsNotValidException ex)
            {
                _logger.LogDebug(MessageUnauthorized, ex.Message);

                return StatusCode(StatusCodes.Status401Unauthorized, new ApiResult
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = $"Errors: {ex.Message}"
                });
            }
            catch (RoleHasNotPermissionException ex)
            {
                _logger.LogDebug(MessageForbbiden, ex.Message);

                return StatusCode(StatusCodes.Status403Forbidden, new ApiResult
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Message = $"Errors: {ex.Message}"
                });
            }
            catch (ClientAlreadyHasOrderException ex)
            {
                _logger.LogDebug(MessageBadRequest, ex.Message);

                return BadRequest(new ApiResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Errors: {ex.Message}"
                });
            }
            catch (ValidationException ex)
            {
                _logger.LogDebug(MessageBadRequest, ex.Message);

                return BadRequest(new ApiResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Errors: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something was wrong");

                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Something was wrong"
                });
            }
        }
    }
}
