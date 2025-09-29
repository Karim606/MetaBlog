using MediatR;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.Likes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Likes.Commands.AddLike
{
    public record  AddLikeCommand(Guid targetId,LikeTargetType TargetType):IRequest<Result<Success>>;
    
}
