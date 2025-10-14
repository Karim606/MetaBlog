using MediatR;
using MetaBlog.Application.Features.Identity.Dto.Requests;
using MetaBlog.Application.Features.Identity.Dto.Responses;
using MetaBlog.Application.Features.Identity.Dtos.Requests;
using MetaBlog.Application.Features.Identity.Dtos.Responses;
using MetaBlog.Application.Features.Identity.Login;
using MetaBlog.Application.Features.Identity.RefreshToken;
using MetaBlog.Application.Features.Identity.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetaBlog.Api.Controllers
{
    [Route("api/[controller]")]
    
    public class AuthController(ISender sender) : ApiController
    {
        private readonly ISender _sender = sender;

        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [EndpointSummary("Registers New User.")]
        [EndpointDescription("Register New user to System.")]
        [EndpointName("Register")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            var command = new RegisterCommand(request.firstName, request.lastName, request.Email, request.Password,request.Dob);
            
            var result = await _sender.Send(command);
            return result.Match(
                Created => Ok(result),
                Problem
            );

        }
        [HttpPost("login")]
        [ProducesResponseType(typeof(Token), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [EndpointSummary("Login.")]
        [EndpointDescription("Login into system if user is already registered.")]
        [EndpointName("Login")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto request)
        {
            // Dummy authentication logic for demonstration purposes
            var command = new LoginCommand(request.Email,request.Password);
            var result = await _sender.Send(command);
            SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiry);

            return result.Match(
                Success => Ok(result.Value.AccessToken),
                Problem
            );
        }


        [HttpPost("refresh")]
        [ProducesResponseType(typeof(RefreshTokenResponseDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [EndpointSummary("Refresh your old token.")]
        [EndpointDescription("Refresh your old token with new one.")]
        [EndpointName("Refresh Token")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Refresh()
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var incomingValue))
                return Unauthorized();
            var result = await _sender.Send(new RefreshTokenCommand(incomingValue));
            SetRefreshTokenCookie(result.Value.refreshToken, result.Value.expiresAt);

            return result.Match(
                Success => Ok(result.Value.accessToken),
                Problem
                );

        }



        private void SetRefreshTokenCookie(string refreshToken,DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = expires,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path ="api/auth/refresh"
            };
            Response.Cookies.Append("refreshToken",refreshToken,cookieOptions);
        }

        
        //[HttpPost]
        //public IActionResult Logout()
        //{
        //    // Dummy logout logic for demonstration purposes
        //    return Ok(new { Message = "User logged out successfully." });
        //}
        //[HttpPost]
        //public IActionResult ForgotPassword() { return Ok(new { Message = "Password reset link sent." }); }

    }
}
