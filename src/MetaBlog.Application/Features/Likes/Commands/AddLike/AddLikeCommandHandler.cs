using MediatR;
using MetaBlog.Application.Common.Interfaces;
using MetaBlog.Domain.Common.Interfaces;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.Likes;
using MetaBlog.Domain.RepositoriesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Likes.Commands.AddLike
{
    public class AddLikeCommandHandler(IPostRepository postRepository,ICommentRepository commentRepository,ICurrentUserService currentUserService,ILikeRepository likeRepository)
        : IRequestHandler<AddLikeCommand, Result<Success>>
    {
        public async Task<Result<Success>> Handle(AddLikeCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse( currentUserService.GetId());
            if (userId==Guid.Empty)
            {
                return Error.Unauthorized();
            }

            ILikable entity;
            if (request.TargetType == LikeTargetType.Post) { entity = await postRepository.GetPostByIDAsync(request.targetId,cancellationToken); }
            else {entity = await commentRepository.GetCommentByIdAsync(request.targetId,cancellationToken); }

            if (entity == null) { return Error.NotFound(); }
            await likeRepository.AddLikeAsync(entity, entity.Id,request.TargetType, userId);
            return Result.Success;

        }
    }
}
