using HomeBudget.Api.Entities;
using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Common.EntityDTOs.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudget.Api.Controllers
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly UserManager<User> _userManager;

        public AddressController(IAddressService addressService, UserManager<User> userManager)
        {
            _addressService = addressService;
            _userManager = userManager;
        }

        [HttpGet("{addressId}"), Authorize]
        public async Task<ActionResult<AddressGetResponseModel>> GetAddressById(string addressId)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var address = await _addressService.GetAddressByIdsAsync(authorization.User!.Id, addressId);
                return address == null ? NotFound() : Ok(address);
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                {
                    return Unauthorized(ex.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("{addressId}"), Authorize]
        public async Task<ActionResult<AddressUpdateResponseModel>> UpdateAddress(string addressId, [FromBody] AddressUpdateRequestModel requestModel)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _addressService.UpdateAddressAsync(authorization.User!.Id, addressId, requestModel);
                return response == null ? NotFound() : CreatedAtAction(nameof(UpdateAddress), new { authorization.User!.Id, addressId}, response);
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                {
                    return Unauthorized(ex.Message);
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        private async Task<(bool Result, User? User)> IsAuthorizedUser()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            return (loggedInUser != null, loggedInUser);
        }
    }
}
