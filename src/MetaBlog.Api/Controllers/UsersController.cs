using MediatR;
using MetaBlog.Application.Features.Users.Dtos;
using MetaBlog.Application.Features.Users.Queries.GetUserProfile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetaBlog.Api.Controllers
{
    [Route("api/users")]
    public class UsersController(ISender sender) : ApiController
    {
        [ProducesResponseType(typeof(UserProfileDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [HttpGet("{id:guid}/profile")]
        public async Task<IActionResult> GetUserProfile(Guid id)
        {
            var result = await sender.Send(new GetUserProfileQuery { UserId = id });

            return result.Match(
                userProfile => Ok(userProfile),
                Problem
                );
        }
    }
}
