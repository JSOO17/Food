﻿using FluentValidation;
using Foods.Application.DTO.Request;
using Foods.Application.Services;
using Foods.Application.Services.Interfaces;
using Foods.Domain.Exceptions;
using Foods.Domain.HttpClients.Interfaces;
using Foods.Domain.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Foods.Infrastructure.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : BaseController
    {
        private const string MessageForbbiden = "forbbiden. Errors: {0}";

        private readonly IDishServices _dishServices;
        private readonly ILogger<DishController> _logger;

        public DishController(
                IDishServices dishServices,
                ILogger<DishController> logger,
                IUserMicroHttpClient userMicroHttpClient
            ) : base(userMicroHttpClient)
        {
            _dishServices = dishServices;
            _logger = logger;
        }

        [Route("restaurant/{restaurantId}")]
        [HttpGet]
        public async Task<IActionResult> GetDishes([FromRoute]long restaurantId, [FromQuery] int page, [FromQuery] int count)
        {
            try
            {
                return Ok(await _dishServices.GetDishes(page, count, restaurantId));
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

        // POST api/<DishController>
        [HttpPost]
        public async Task<IActionResult> CreateDish([FromBody] DishRequestDTO dishRequest)
        {
            try
            {
                await ValidateToken();

                var payload = GetPayload();

                IsOwner(payload.RoleId);

                var dishResponse = await _dishServices.CreateDish(dishRequest);

                return Ok(dishResponse);

            }
            catch (RoleHasNotPermissionException ex)
            {
                _logger.LogDebug(MessageForbbiden, ex.Message);

                return StatusCode(403, ex.Message);
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

        // PUT api/<DishController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDish(long id, [FromBody] DishRequestDTO dishRequest)
        {
            try
            {
                await ValidateToken();

                var payload = GetPayload();

                IsOwner(payload.RoleId);

                await _dishServices.UpdateDish(id, dishRequest, payload.UserId);

                return Ok(new ApiResult
                {
                    StatusCode = 200,
                    Message = "Dish updated successfull"
                });

            }
            catch (RoleHasNotPermissionException ex)
            {
                _logger.LogDebug(MessageForbbiden, ex.Message);

                return StatusCode(403, ex.Message);
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
}
