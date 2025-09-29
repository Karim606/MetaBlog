using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaBlog.Domain.Common.Results.Interface
{
    public interface IResult
    {
        List<Error>? Errors { get; }
        bool IsSuccess { get; }
    }

    public interface IResult<T> : IResult
    {
        T Value { get; }
    }
}
