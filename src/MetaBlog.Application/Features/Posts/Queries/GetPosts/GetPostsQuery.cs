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
    public record GetPostsQuery(int pageNumber=1,int pageSize=10,string? searchTerm=null,Guid? authorId=null,DateTime?createdAfter=null,string? sortBy = null,bool? sortDescending = null)
        :IRequest<Result<PaginatedList<PostDto>>>;
    
}
