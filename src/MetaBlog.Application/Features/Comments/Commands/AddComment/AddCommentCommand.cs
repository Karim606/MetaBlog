using FluentValidation;
using MediatR;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Comments.Commands.AddComment
{
    public record AddCommentCommand(string content,Guid postId,Guid? parentCommentId):IRequest<Result<Guid>>;
    
}
