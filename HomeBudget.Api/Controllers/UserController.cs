﻿using HomeBudget.Api.Data;
using HomeBudget.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeBudget.Common.EntityDTOs.User;
using HomeBudget.Api.Services.Interfaces;

namespace HomeBudget.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public UserController(IUserService userService, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<UserGetResponseModel>> GetUser()
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(authorization.User!.Id);
                return user == null ? NotFound() : Ok(user);
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

        [HttpPut, Authorize]
        public async Task<ActionResult<UserUpdateResponseModel>> UpdateUser([FromBody] UserUpdateRequestModel requestModel)
        {
            var authorization = await IsAuthorizedUser();

            if (!authorization.Result)
            {
                return Forbid();
            }

            try
            {
                var response = await _userService.UpdateUserAsync(authorization.User!.Id, requestModel);
                return response == null ? NotFound() : CreatedAtAction(nameof(UpdateUser), new { authorization.User!.Id }, response);
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
