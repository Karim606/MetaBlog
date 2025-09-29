using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Posts.Queries.GetPosts
{
    public class GetPostsQueryValidator:AbstractValidator<GetPostsQuery>
    {
        public GetPostsQueryValidator()
        {
            RuleFor(q => q.pageNumber).Must(p => p > 0).WithMessage("PageNumber has to be bigger than zero. ");
            RuleFor(q => q.pageSize).Must(p => p > 0).WithMessage("PageSize has to bigger than zero.");

        }
    }
}
