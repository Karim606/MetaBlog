using MediatR;
using MetaBlog.Application.Features.Identity.Dtos.Responses;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.RefreshToken
{
    public record RefreshTokenCommand(string unHashedRefreshToken):IRequest<Result<RefreshTokenResponseDto>>;
    
}
