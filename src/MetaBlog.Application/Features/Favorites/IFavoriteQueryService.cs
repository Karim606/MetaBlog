using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Favorites.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Favorites
{
    public interface IFavoriteQueryService
    {
        public Task<PaginatedList<FavoriteDto>> GetFavorites(int page,int pageSize,Guid userId,CancellationToken ct);
    }
}
