using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.Favorites;
using MetaBlog.Domain.RepositoriesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Favorites.Commands.RemoveFromFavorites
{
    public class RemoveFromFavoritesCommandHandler(IFavoriteRepository favoriteRepository, ICurrentUserService currentUserService) : IRequestHandler<RemoveFromFavoritesCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(RemoveFromFavoritesCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.GetId();
            if (userId == null) { return Error.Unauthorized(); }
            var favorite = await favoriteRepository.GetFavorite(request.id);
            if (favorite == null||favorite.userId != Guid.Parse(userId))
            {
                return Error.NotFound();
            }
            await favoriteRepository.RemoveFavorite(favorite);
            await favoriteRepository.SaveChangesAsync();
            return Result.Deleted;
        }
    }
}
