using MediatR;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Follow.Commands.FollowUser;
using MetaBlog.Application.Features.Follow.Commands.UnFollowUser;
using MetaBlog.Application.Features.Follow.Dtos.response;
using MetaBlog.Application.Features.Follow.Queries.GetFollowers;
using MetaBlog.Application.Features.Follow.Queries.GetFollowing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetaBlog.Api.Controllers
{
    [Route("api/users")]
    public class FollowController(ISender sender) : ApiController
    {
        [Authorize]
        [HttpPost("{id}/follow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("follow specific user")]
        [EndpointDescription("follow specific user by it's id ")]
        [EndpointName("follow")]
        public async Task<IActionResult> FollowUser(Guid Id)
        {
            var result = await sender.Send(new FollowCommand(Id));
            return result.Match(
                 Followed => Ok(),
                 Problem
                 );
        }

        [Authorize]
        [HttpDelete("{id}/follow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("unfollow specific user")]
        [EndpointDescription("unfollow specific user by it's id ")]
        [EndpointName("unfollow")]
        public async Task<IActionResult> UnfollowUser(Guid Id)
        {
            var result = await sender.Send(new UnFollowCommand(Id));
            return result.Match(
                 Followed => Ok(),
                 Problem
                 );
        }

        [Authorize]
        [HttpGet("{id}/followers")]
        [ProducesResponseType(typeof(PaginatedList<Followers_FollowedDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Get Followers of user")]
        [EndpointDescription("supports pagination with pagesize and number,searching,sorting asc&desc ")]
        [EndpointName("GetFollowers")]
        public async Task<IActionResult> GetFollowers(Guid Id,[FromQuery]GetFollowersQuery getFollowersQuery)
        {
            var result = await sender.Send( getFollowersQuery with {userId = Id});
            return result.Match(
                 Followed => Ok(),
                 Problem
                 );
        }

        [Authorize]
        [HttpGet("{id}/following")]
        [ProducesResponseType(typeof(PaginatedList<Followers_FollowedDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Get Followed people by user")]
        [EndpointDescription("supports pagination with pagesize and number,searching,sorting asc&desc ")]
        [EndpointName("GetFollowed")]
        public async Task<IActionResult> GetFollowing(Guid Id,[FromQuery]GetFollowedQuery getFollowedQuery)
        {
            var result = await sender.Send(getFollowedQuery with { userId = Id });
            return result.Match(
                 Followed => Ok(Followed),
                 Problem
                 );
        }


    }
}
