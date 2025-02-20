﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityPilot.BLL.Areas.Identity.DTO;
using UniversityPilot.BLL.Areas.Identity.Interfaces;

namespace UniversityPilot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(2)
            };

            Response.Cookies.Append("session_token", token, cookieOptions);

            return Ok(new { message = "Zalogowano pomyślnie" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("session_token");
            return Ok(new { message = "Wylogowano pomyślnie" });
        }

        [HttpGet]
        [Route("Details")]
        [Authorize]
        public IActionResult Details(int id)
        {
            var userDetails = _accountService.GetUserDetails(id);
            return Ok(userDetails);
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize]
        public IActionResult Edit(int id, [FromBody] UserDetailsDto userDetails)
        {
            _accountService.Edit(id, userDetails);
            return Ok(userDetails);
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize]
        public IActionResult ChangePassword(int id, [FromBody] ChangePasswordDto changePassword)
        {
            _accountService.ChangePassword(id, changePassword);
            return Ok();
        }
    }
}