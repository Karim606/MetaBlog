using MediatR;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Comments.Commands.DeleteComment;
using MetaBlog.Application.Features.Posts.Commands.CreatePost;
using MetaBlog.Application.Features.Posts.Commands.DeletePost;
using MetaBlog.Application.Features.Posts.Commands.EditPost;
using MetaBlog.Application.Features.Posts.Dtos.Request;
using MetaBlog.Application.Features.Posts.Dtos.Response;
using MetaBlog.Application.Features.Posts.Queries.GetPostById;
using MetaBlog.Application.Features.Posts.Queries.GetPosts;
using MetaBlog.Domain.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetaBlog.Api.Controllers
{
    [Route("api/v{version:apiVersion}/posts")]
    [ApiVersion("1.0")]

    public class PostsController(ISender sender) : ApiController
    {
        [HttpPost]
        [Authorize(Roles = "User")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)] // only returning ID
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Creates a new blog post.")]
        [EndpointDescription("Adds a new post to the system and returns its ID.")]
        [EndpointName("CreatePost")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand createPostCommand, CancellationToken ct)
        {
            var result = await sender.Send(createPostCommand, ct);

            return result.Match(
                response => CreatedAtRoute(
                    routeName: "GetPostById", // make sure this route exists
                    routeValues: new { version = "1.0", Id = response },
                    value: new { Id = response } // only return PostId
                ),
                Problem
            );
        }
        [HttpDelete("{Id:Guid}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Deletes post")]
        [EndpointDescription("Deletes the specified post  only by authorized user or admin.")]
        [EndpointName("DeletePost")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeletePost([FromRoute]Guid Id ) {
            var result = await sender.Send(new DeletePostCommand(Id));
            return result.Match(
                 Deleted => NoContent(),
                 Problem
                 );
        }

        [HttpPut("{Id:Guid}")]
        [Authorize(Roles ="User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Edits post")]
        [EndpointDescription("Edits the specified post only by authorized user .")]
        [EndpointName("EditPost")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> EditPost([FromRoute]Guid Id,[FromBody]EditPostRequest request,CancellationToken ct) {
            var command = new EditPostCommand(Id, request.Title, request.Content);  
            var result = await sender.Send(command,ct);
            return result.Match(
                Updated => NoContent(),
                Problem
                );
        }
        [HttpGet("{Id:Guid}", Name = "GetPostById")]
        [ProducesResponseType(typeof(PostDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("gets specified post")]
        [EndpointDescription("gets the specified post.")]
        [EndpointName("GetById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(Guid Id,CancellationToken ct)
        {
            var result = await sender.Send(new GetPostByIdQuery(Id),ct);
            return result.Match(
                response => Ok(response),
                Problem);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<PostDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [EndpointSummary("Get Posts paginated,filtered & sorted")]
        [EndpointDescription("supports pagination with pagesize and number, filtering with authorId & creation,sorting asc&desc ")]
        [EndpointName("GetPosts")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetPosts([FromQuery]GetPostsQuery getPostsQuery){
            var result = await sender.Send(getPostsQuery);
            return result.Match(
                posts => Ok(posts),
                Problem
                );
        }
    }
}
