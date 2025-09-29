using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Favorites.Dtos.Response;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Favorites.Queries.GetFavorites
{
    public class GetFavoritesQueryHandler(ICurrentUserService currentUserService,IFavoriteQueryService favoriteQueryService) : IRequestHandler<GetFavoritesQuery, Result<PaginatedList<FavoriteDto>>>
    {
        public async Task<Result<PaginatedList<FavoriteDto>>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(currentUserService.GetId());
            if (userId == null)
            {
                return Error.Unauthorized();
            }

            var favorites = await favoriteQueryService.GetFavorites(request.page, request.pageSize, userId, cancellationToken);
            if (favorites == null) { 
            return Error.NotFound();
            }
            return favorites;
        }
    }
}
