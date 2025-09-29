using FluentValidation;
using MediatR;
using MetaBlog.Domain.Common.Results;
using MetaBlog.Domain.Common.Results.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest,TResponse>(IValidator<TRequest>? validator=null):IPipelineBehavior<TRequest,TResponse>
         where TRequest: IRequest<TResponse>
         where TResponse:IResult
    {
        private readonly IValidator<TRequest>? _validator = validator;

        public async Task<TResponse> Handle(TRequest request,RequestHandlerDelegate<TResponse>next,CancellationToken ct)
        {
            if(_validator is null) 
                return await next(ct);
            var validationResult= await _validator.ValidateAsync(request,ct);
            if(validationResult.IsValid) 
                return await next();
            var errors = validationResult.Errors.ConvertAll(e => Error.Validation(e.PropertyName, e.ErrorMessage));
            return (dynamic)errors;
        }
    }
}
