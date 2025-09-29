using MediatR;
using MetaBlog.Application.Features.Identity.Dto.Responses;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.Login
{
    public record LoginCommand(string Email,string Password):IRequest<Result<Token>>;
    
}
