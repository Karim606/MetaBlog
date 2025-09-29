using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Application.Features.Posts.Dtos.Response;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.Posts;
using MetaBlog.Domain.RepositoriesInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Posts.Queries.GetPostById
{
    public class GetPostByIdQueryHandler(IPostQueryService postQueryService,ILogger<GetPostByIdQueryHandler>logger)
        :IRequestHandler<GetPostByIdQuery,Result<PostDto>>
    {
        public async Task<Result<PostDto>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var postDto = await postQueryService.GetPostByIdAsync(request.Id, cancellationToken);
            if (postDto == null)
            {
                return Error.NotFound("Post not Found.");
            }
          

            return postDto;

        }
    }
    
}
