using MediatR;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Favorites.Dtos.Response;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Favorites.Queries.GetFavorites
{
    public record GetFavoritesQuery(int page,int pageSize):IRequest<Result<PaginatedList<FavoriteDto>>>;
   
}
