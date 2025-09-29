using MediatR;
using MetaBlog.Application.Features.Favorites.Commands.AddToFavorites;
using MetaBlog.Application.Features.Favorites.Commands.RemoveFromFavorites;
using MetaBlog.Application.Features.Favorites.Queries.GetFavorites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetaBlog.Api.Controllers
{
    [Route("api/v{version:ApiVersion}/favorites")]
    [ApiVersion("1.0")]
    
    public class FavoritesController(ISender sender) : ApiController
    {
        [HttpPost]
        [Authorize(Roles ="User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Add post to favorite")]
        [EndpointDescription("Adding post to favorites by post id only allowed for authorized users.")]
        [EndpointName("AddToFavorites")]
        public async Task<IActionResult> AddFavorite(Guid postId)
        {
            var result = await sender.Send(new AddToFavoritesCommand(postId));
           return result.Match(
                Created => Ok(),
                Problem
                );
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Remove post from favorite")]
        [EndpointDescription("Remove post from favorites by id only allowed for authorized users.")]
        [EndpointName("RemoveFromFavorites")]
        public async Task<IActionResult>RemoveFromFavorites([FromRoute]Guid id)
        {
            var result = await sender.Send(new RemoveFromFavoritesCommand(id));
            return result.Match(
                Deleted => NoContent(),
                Problem
                );
        }
        [HttpGet]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Get favorite posts")]
        [EndpointDescription("Getting favorite post from favorites only allowed for authorized users.")]
        [EndpointName("GetFavoritePosts")]
        public async Task<IActionResult> GetFavorites([FromQuery]GetFavoritesQuery getFavoritesQuery)
        {
            var result = await sender.Send(getFavoritesQuery);
            return result.Match(
                Favorites => Ok(Favorites),
                Problem
                );
        }
    }
}
