using MediatR;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Posts.Commands.DeletePost
{
    public record DeletePostCommand(Guid Id):IRequest<Result<Deleted>>;
}
