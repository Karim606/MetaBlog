using MediatR;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Posts.Commands.EditPost
{
    public record EditPostCommand(Guid PostId, string Title, string Content) : IRequest<Result<Updated>>;

}
