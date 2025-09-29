using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Comments.Commands.EditComment
{
    public class EditCommentCommandValidator:AbstractValidator<EditCommentCommand>
    {
        public EditCommentCommandValidator()
        {
            RuleFor(c => c.content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(1000).WithMessage("Content must not exceed 1000 characters.");
            RuleFor(c => c.id)
                .NotEmpty().WithMessage("Comment ID is required.");
        }
    }
}
