using MediatR;
using MetaBlog.Api.OpenApi.Transformers;
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
using MetaBlog.Domain.RefreshTokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MetaBlog.Api.Controllers
{
    [Route("api/auth")]
    
    public class AuthController(ISender sender,IConfiguration configuration) : ApiController
    {
        private readonly ISender _sender = sender;

        [HttpPost("register")]
        [ProducesResponseType(typeof(AccessToken), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RefreshTokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EndpointSummary("Registers New User.")]
        [EndpointDescription("Register New user to System. it returns refreshToken in response body for mobile while webBrowser gets it in cookie.")]
        [EndpointName("Register")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Register( [FromForm]RegisterUserDto request)
        {
            var command = new RegisterCommand(request.firstName, request.lastName, request.Email, request.Password,request.Bio,request.ProfileImage,request.Dob);
            
            var result = await _sender.Send(command);

            var origin = Request.Headers["Origin"].ToString();

            bool isBrowser = false;
            if (!string.IsNullOrEmpty(origin))
            {
                isBrowser = true;
            }

            if (result.IsSuccess && isBrowser)
                SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiry);

            return result.Match(
                value => {
                    if (isBrowser)
                        return Ok(new AccessToken(value.AccessToken));
                    else
                        return Ok(value);
                },
                Problem
                );

        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AccessToken), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RefreshTokenResponseDto),StatusCodes.Status200OK)]
        [ReturnsCookie("refreshToken",200,bodyType: typeof(AccessToken))]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [EndpointSummary("Login.")]
        [EndpointDescription("Login into system if user is already registered. it returns refreshToken in response body for mobile while webBrowser gets it in cookie.")]
        [EndpointName("Login")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto request)
        {

            var origin = Request.Headers["Origin"].ToString();
            bool isBrowser = false;
            if (!string.IsNullOrEmpty(origin))
            {
                isBrowser = true;
            }

            var command = new LoginCommand(request.Email,request.Password);
            var result = await _sender.Send(command);
            if(result.IsSuccess&&isBrowser)
            SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiry);
            
            return result.Match(
                value => {
                    if (isBrowser)
                        return Ok(new AccessToken(value.AccessToken));
                    else
                        return Ok(value);
                    },
                Problem
                );


        }


        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AccessToken),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RefreshTokenResponseDto), StatusCodes.Status200OK)]
        [ReturnsCookie("refreshToken",200,bodyType: typeof(AccessToken))]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [EndpointSummary("Refresh your old token.")]
        [EndpointDescription("When refreshing a token, the old token is replaced with a new one. For web users, the new refresh token " +
            "is stored securely in an HttpOnly cookie, while mobile users receive it in the JSON response body to store in secure storage.")]
        [EndpointName("Refresh Token")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest? request)
        {
            string refreshToken = request?.RefreshToken;
            bool isBrowser = false;

            var origin = Request.Headers["Origin"].ToString();
            bool hasCookie = Request.Cookies.ContainsKey("refreshToken");

            if (hasCookie || !string.IsNullOrEmpty(origin))
            {
                refreshToken = Request.Cookies["refreshToken"];
                isBrowser = true;
            }

            var result = await _sender.Send(new RefreshTokenCommand(refreshToken));

            if (result.IsSuccess && isBrowser)
                SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiry);
            
            return result.Match(
                value => { 
                    if(isBrowser)
                     return  Ok(new AccessToken(value.AccessToken));
                    else
                     return Ok(value);
                },
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
                Path ="/",
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
            SetRefreshTokenCookie("",DateTime.UtcNow.AddDays(-1));
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
