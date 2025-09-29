using MediatR;
using MetaBlog.Domain.RepositoriesInterfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaBlog.Domain.Posts;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Application.Common.Interfaces;
namespace MetaBlog.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommandHandler(ICurrentUserService currentUserService,IPostRepository postRepository,ILogger<CreatePostCommandHandler>logger) : IRequestHandler<CreatePostCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.GetId();
            if(userId == null || userId == string.Empty)
            {
                logger.LogWarning("Unauthorized attempt to create a post.");
                return Error.Unauthorized("Unauthorized attempt to create a post.");
            }
            var post = Post.Create(new Guid(), request.Title, request.Content, Guid.Parse(userId));
           await  postRepository.AddPostAsync(post, cancellationToken);
            return post.Id;
        }
    }
}
