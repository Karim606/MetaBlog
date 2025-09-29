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

namespace MetaBlog.Application.Features.Likes.Commands.RemoveLike
{
    public class RemoveLikeCommandHandler(IPostRepository postRepository, ICommentRepository commentRepository, ICurrentUserService currentUserService, ILikeRepository likeRepository) : IRequestHandler<RemoveLikeCommand, Result<Deleted>>
    {
        public async Task<Result<Deleted>> Handle(RemoveLikeCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(currentUserService.GetId());
            if (userId == Guid.Empty)
            {
                return Error.Unauthorized();
            }

            ILikable entity;
            if (request.TargetType == LikeTargetType.Post) { entity = await postRepository.GetPostByIDAsync(request.targetId, cancellationToken); }
            else { entity = await commentRepository.GetCommentByIdAsync(request.targetId, cancellationToken); }

            if (entity == null) { return Error.NotFound(); }
            await likeRepository.RemoveLikeAsync(entity, entity.Id, request.TargetType, userId);
            return Result.Deleted;
        }
    }
}
