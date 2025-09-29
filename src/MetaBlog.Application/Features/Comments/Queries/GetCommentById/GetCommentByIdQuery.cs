using MediatR;
using MetaBlog.Application.Features.Comments.Dtos;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Comments.Queries.GetCommentById
{
    public record GetCommentByIdQuery(Guid id):IRequest<Result<CommentDto>>;
    
}
