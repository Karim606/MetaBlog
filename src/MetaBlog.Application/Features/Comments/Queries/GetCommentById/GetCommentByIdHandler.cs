using MediatR;
using MetaBlog.Application.Features.Comments.Dtos;
using MetaBlog.Application.Features.Comments.Dtos.Request;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Comments.Queries.GetCommentById
{
    public class GetCommentByIdHandler(ICommentQueryService commentQueryService) : IRequestHandler<GetCommentByIdQuery, Result<CommentDto>>
    {
        public async Task<Result<CommentDto>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await commentQueryService.GetCommentByIdAsync(request.id, cancellationToken);
            if (comment == null)
               return Error.NotFound();

            return comment;
         }
     }
}
