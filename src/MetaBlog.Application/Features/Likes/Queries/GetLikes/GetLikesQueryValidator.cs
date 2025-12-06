using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Likes.Queries.GetLikes
{
    public class GetLikesQueryValidator:AbstractValidator<GetLikesQuery>
    {
        public GetLikesQueryValidator() {

            RuleFor(q => q.pageSize)
            .InclusiveBetween(1, 20)
            .WithMessage("pageSize must be between 1 and 20.");

            RuleFor(q => q.offset)
                .GreaterThanOrEqualTo(0)
                .WithMessage("offset must be 0 or greater.");

            RuleFor(q => q.type)
                .IsInEnum()
                .WithMessage("Invalid like target type.");
        }
    }
}
