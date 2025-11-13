using MediatR;
using MetaBlog.Application.Features.Users.Dtos;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Users.Queries.GetUserProfile
{
    public record GetUserProfileQuery: IRequest<Result<UserProfileDto>>
    {
        public Guid UserId { get; init; }
    }
}
