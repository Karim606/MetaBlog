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

namespace MetaBlog.Application.Features.Favorites.Commands.AddToFavorites
{
    public class AddToFavoritesCommandHandler(IFavoriteRepository favoriteRepository,ICurrentUserService currentUserService) : IRequestHandler<AddToFavoritesCommand, Result<Created>>
    {
        public async Task<Result<Created>> Handle(AddToFavoritesCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.GetId();
            if(userId == null) { return Error.Unauthorized(); }

            if(await favoriteRepository.FavoriteExist(request.postId, Guid.Parse(userId)) ) {
                return Error.Conflict();
            }
            var favorite = Favorite.Create(new Guid(), request.postId, Guid.Parse(userId));
            await favoriteRepository.AddFavorite(favorite);
            return  Result.Created;

        }
    }
}
