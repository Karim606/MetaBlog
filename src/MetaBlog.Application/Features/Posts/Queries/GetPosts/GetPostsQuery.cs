using MediatR;
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
    public record GetPostsQuery(int pageNumber,int pageSize,string? searchTerm,Guid? authorId,DateTime?createdAfter,string? sortBy,bool? sortDescending)
        :IRequest<Result<PaginatedList<PostDto>>>;
    
}
