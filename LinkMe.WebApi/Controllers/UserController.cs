﻿using Azure.Core;
using LinkMe.Application.Logic.User;
using LinkMe.Infrastructure.Auth;
using LinkMe.WebApi.Application.Auth;
using LinkMe.WebApi.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LinkMe.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly CookieSettings? _cookieSettings;
        private readonly JwtManager _jwtManager;

        public UserController(
            ILogger<UserController> logger,
            IOptions<CookieSettings> cookieSettings,
            JwtManager jwtManager,
            IMediator mediator) : base(logger, mediator)
        {
            _cookieSettings = cookieSettings != null ? cookieSettings.Value : null;
            _jwtManager = jwtManager;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserWithAccount([FromBody] CreateUserWithAccountCommand.Request model)
        {
            var createAccountResult = await _mediator.Send(model);
            var token = _jwtManager.GenerateUserToken(createAccountResult.UserId);
            SetTokenCookie(token);
            return Ok(new JwtToken() { AccessToken = token });
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginCommand.Request model)
        {
            var loginResult = await _mediator.Send(model);
            var token = _jwtManager.GenerateUserToken(loginResult.UserId);
            SetTokenCookie(token);
            return Ok(new JwtToken() { AccessToken = token });
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            var logoutResult = await _mediator.Send(new LogoutCommand.Request());
            DeleteTokenCookie();
            return Ok(logoutResult);
        }

        private void SetTokenCookie(string token)
        {
            var cookieOption = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddDays(30),
                SameSite = SameSiteMode.Lax,
            };

            if (_cookieSettings != null)
            {
                cookieOption = new CookieOptions()
                {
                    HttpOnly = cookieOption.HttpOnly,
                    Expires = cookieOption.Expires,
                    Secure = _cookieSettings.Secure,
                    SameSite = _cookieSettings.SameSite,
                };
            }

            Response.Cookies.Append(CookieSettings.CookieName, token, cookieOption);
        }

        public void DeleteTokenCookie()
        {
            Response.Cookies.Delete(CookieSettings.CookieName, new CookieOptions()
            {
                HttpOnly = true,
            });
        }
    }
}
