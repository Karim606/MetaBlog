using MediatR;
using MetaBlog.Application.Features.Identity.Dto.Requests;
using MetaBlog.Application.Features.Identity.Dto.Responses;
using MetaBlog.Application.Features.Identity.Dtos.Requests;
using MetaBlog.Application.Features.Identity.Dtos.Responses;
using MetaBlog.Application.Features.Identity.ForgotPassword;
using MetaBlog.Application.Features.Identity.Login;
using MetaBlog.Application.Features.Identity.Logout;
using MetaBlog.Application.Features.Identity.RefreshToken;
using MetaBlog.Application.Features.Identity.Register;
using MetaBlog.Application.Features.Identity.ResetPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MetaBlog.Api.Controllers
{
    [Route("api/auth")]
    
    public class AuthController(ISender sender) : ApiController
    {
        private readonly ISender _sender = sender;

        [HttpPost("register")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EndpointSummary("Registers New User.")]
        [EndpointDescription("Register New user to System.")]
        [EndpointName("Register")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            var command = new RegisterCommand(request.firstName, request.lastName, request.Email, request.Password,request.confirmPassword,request.Dob);
            
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
            if(result.IsSuccess)
            SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiry);

            return result.Match(
                Success => Ok(new { accessToken = result.Value.AccessToken}),
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
            if (!Request.Cookies.TryGetValue("refreshToken", out var incomingValue))
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

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
        [EndpointSummary("logout")]
        [EndpointName("logout")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Logout()
        {
            Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
            var command = new LogOutCommand(refreshToken);
            SetRefreshTokenCookie("",DateTime.MinValue);
            var result = await _sender.Send(command);
            return result.Match(
                Success => NoContent(),
                Problem
                );
        }
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Forgot Password")]
        [EndpointDescription("send request to reset password by email.")]
        [EndpointName("Forgot-Password")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordDto model) {

            var result = await _sender.Send(new ForgotPasswordCommand(model.Email));
            return result.Match(
                Success => Ok(),
                Problem);
             }

        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("reset your password")]
        [EndpointDescription("reset your password by sending new one with token ")]
        [EndpointName("reset-password")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var result = await _sender.Send(new ResetPasswordCommand(model));

           return result.Match(
                Success => Ok(),
                Problem
                );
        }

    }
}
