using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Posts.Commands.EditPost
{
    public class EditPostCommandValidator:AbstractValidator<EditPostCommand>
    {
        public EditPostCommandValidator()
        {
            RuleFor(p => p.PostId).NotEmpty().WithMessage("PostId is required");
            RuleFor(p => p.Title).NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");
            RuleFor(p => p.Content).NotEmpty().WithMessage("Content is required");
        }
    }
}
