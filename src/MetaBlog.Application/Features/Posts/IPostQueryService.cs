using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Posts.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Posts
{
    public interface IPostQueryService
    {
        Task<PaginatedList<PostDto>> GetPostsAsync(int pageNumber, int pageSize, string? searchTerm, Guid? authorId, DateTime? createdAfter, string? sortBy, bool? sortDescending, CancellationToken ct);
        Task<PostDto> GetPostByIdAsync(Guid Id,CancellationToken ct);
    }
}
