using MediatR;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Likes.Commands.AddLike;
using MetaBlog.Application.Features.Likes.Commands.RemoveLike;
using MetaBlog.Application.Features.Likes.Dtos;
using MetaBlog.Application.Features.Likes.Queries.GetLikes;
using MetaBlog.Domain.Likes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetaBlog.Api.Controllers
{
    [Route("api/v{version:ApiVersion}/{targetType:LikeTargetType}/{targetId:Guid}/likes")]
    [ApiVersion("1.0")]
    public class LikesController(ISender sender) : ApiController
    {
        [HttpPost]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Add Like to posts or comments")]
        [EndpointDescription("Adding like to post or comments by demonstrating target-id and target-type.  only allowed for authorized users.")]
        [EndpointName("AddLike")]
        public async Task<IActionResult> AddLike([FromRoute]Guid targetId,[FromRoute]LikeTargetType targetType,CancellationToken ct)
        {
            var result = await sender.Send(new AddLikeCommand(targetId,targetType),ct);
            return result.Match(
                Success => Ok(),
                Problem
                );
        }

        [HttpDelete]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Remove like from  target")]
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

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PaginatedList<LikeDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Get likes like for specific post or comment")]
        [EndpointDescription(" Get likes for post or comments by demonstrating target-id and target-type.  only allowed for authorized users.")]
        [EndpointName("GetLikes")]
        public async Task<IActionResult> GetLikes([FromRoute] Guid targetId, [FromRoute] LikeTargetType targetType,[FromQuery]int pageSize, [FromQuery] int offset)
        {
            var result = await sender.Send(new GetLikesQuery(targetId, targetType, pageSize, offset));
            return result.Match(
                list => Ok(list),
                Problem
                );
        }

    }
}
