using MediatR;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Follow.Commands.UnFollowUser
{
    public record UnFollowCommand(Guid followedId):IRequest<Result<Success>>;
   
}
