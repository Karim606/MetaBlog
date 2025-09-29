using MediatR;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Comments.Commands.AddComment;
using MetaBlog.Application.Features.Comments.Commands.DeleteComment;
using MetaBlog.Application.Features.Comments.Commands.EditComment;
using MetaBlog.Application.Features.Comments.Dtos;
using MetaBlog.Application.Features.Comments.Dtos.Request;
using MetaBlog.Application.Features.Comments.Queries.GetCommentById;
using MetaBlog.Application.Features.Comments.Queries.LoadComments;
using MetaBlog.Application.Features.Likes.Commands.AddLike;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MetaBlog.Api.Controllers
{
    [Route("api/v{version:ApiVersion}/posts/{postid:Guid}/comments")]
    [ApiVersion("1.0")]
    
    public class CommentsController(ISender sender) : ApiController
    {

        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status401Unauthorized)]
        [EndpointSummary("Creates a comment.")]
        [EndpointDescription("Add comment to a specific post only allowed by authorized users.")]
        [EndpointName("AddComment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddComment([FromRoute]Guid postid, [FromBody]CommentRequestDto requestDto)
        {
            var result = await sender.Send(new AddCommentCommand(requestDto.content,postid,requestDto.parentCommentId));
            return result.Match(
                Created => CreatedAtRoute(
                    "GetCommentById",new{version="1.0",postid=postid,id= result},new {commentid=result}),
                Problem
                );
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Deletes comment.")]
        [EndpointDescription("Delete comment published on a specific post only allowed by authorized users.")]
        [EndpointName("DeleteComment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var result = await sender.Send(new DeleteCommentCommand(id));
            return result.Match(
                Deleted => NoContent(),
                Problem
                );
        }


        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Edit comment.")]
        [EndpointDescription("Edit comment published on a specific post only allowed by authorized users.")]
        [EndpointName("EditComment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> EditComment([FromRoute] Guid id,[FromBody]string content)
        {
            var result = await sender.Send(new EditCommentCommand(content,id));
            return result.Match(
                Updated => Ok(Updated),
                Problem
                );
        }



        [HttpGet]
        [ProducesResponseType(typeof(PaginatedListCursorBased<CommentDto,DateTime>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("  Loads comments.")]
        [EndpointDescription("Loads comments and replies of a specific post with cursorbased pagination according to last comment in list")]
        [EndpointName("loadcomments")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> LoadComments([FromRoute]Guid postid,[FromQuery]Guid? parentCommentId, [FromQuery] DateTime? lastCommentTime, [FromQuery] int pageSize,CancellationToken ct)
        {
            var result = await sender.Send(new LoadCommentsQuery(postid,parentCommentId,lastCommentTime,pageSize),ct);
            return result.Match(
                comments => Ok(comments),
                Problem
                );
        }

        [HttpGet("{id:Guid}",Name ="GetCommentById")]
        [ProducesResponseType(typeof(CommentDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [EndpointSummary("Get a comment")]
        [EndpointDescription("Get a comment published on a specific post.")]
        [EndpointName("GetComment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var result = await sender.Send(new GetCommentByIdQuery(id));
            return result.Match(
                response => Ok(response),
                Problem
                );
        }
    }
}
