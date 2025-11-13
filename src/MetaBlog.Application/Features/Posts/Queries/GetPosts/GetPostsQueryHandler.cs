using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Common.Models;
using MetaBlog.Application.Features.Posts.Dtos.Response;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Posts.Queries.GetPosts
{
    public class GetPostsQueryHandler(IPostQueryService postQueryService,ICurrentUserService currentUserService) : IRequestHandler<GetPostsQuery, Result<PaginatedList<PostDto>>>
    {
        public async Task<Result<PaginatedList<PostDto>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
             var result = Guid.TryParse(currentUserService.GetId(), out var userId);

            var posts = await postQueryService.GetPostsAsync(request.pageNumber,
                request.pageSize,
                request.searchTerm,
                request.authorId,
                request.createdAfter,
                request.sortBy,
                request.sortDescending,
                currentUserId:result ? userId : null,
                cancellationToken);

            return posts;
        
        }
    }
}
