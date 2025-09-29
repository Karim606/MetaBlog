using MediatR;
using MetaBlog.Application.Features.Likes.Commands.AddLike;
using MetaBlog.Application.Features.Likes.Commands.RemoveLike;
using MetaBlog.Domain.Likes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetaBlog.Api.Controllers
{
    [Route("api/v{version:ApiVersion}/likes")]
    [ApiVersion("1.0")]
    public class LikesController(ISender sender) : ApiController
    {
        [HttpPost("{targetType:LikeTargetType}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Add Like to posts or comments")]
        [EndpointDescription("Adding like to post or comments by demonstrating target-id and target-type.  only allowed for authorized users.")]
        [EndpointName("AddLike")]
        public async Task<IActionResult> AddLike([FromBody]Guid targetId,[FromRoute]LikeTargetType targetType,CancellationToken ct)
        {
            var result = await sender.Send(new AddLikeCommand(targetId,targetType),ct);
            return result.Match(
                Success => Ok(),
                Problem
                );
        }

        [HttpDelete("{targetType:LikeTargetType}/{targetId:Guid}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Remove like from specific post")]
        [EndpointDescription("Removing like from post or comments by demonstrating target-id and target-type.  only allowed for authorized users.")]
        [EndpointName("RemoveLike")]
        public async Task<IActionResult> RemoveLike([FromRoute]Guid targetId,[FromRoute]LikeTargetType targetType,CancellationToken ct)
        {
            var result = await sender.Send(new RemoveLikeCommand(targetId,targetType),ct);
            return result.Match(
                Deleted => NoContent(),
                Problem
                );
        }

       
    }
}
