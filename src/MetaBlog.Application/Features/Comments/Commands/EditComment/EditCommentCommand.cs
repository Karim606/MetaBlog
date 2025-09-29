using MediatR;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Comments.Commands.EditComment
{
    public record EditCommentCommand(string content, Guid id) : IRequest<Result<Updated>>;
    
}
