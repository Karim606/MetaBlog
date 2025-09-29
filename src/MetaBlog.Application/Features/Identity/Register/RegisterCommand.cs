using MediatR;
using MetaBlog.Application.Common.Attributes;
using MetaBlog.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Features.Identity.Register
{
    public record RegisterCommand(string firstName,string lastName, string Email,[property:SensitiveData] string Password,DateOnly Dob)
        :IRequest<Result<Created>>; 

}
