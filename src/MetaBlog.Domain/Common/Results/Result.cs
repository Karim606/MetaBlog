using MetaBlog.Domain.Common.Results.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MetaBlog.Domain.Common.Results
{
    public static class Result
    {
        public static Success Success => default;
        public static Created Created => default;
        public static Updated Updated => default;
        public static Deleted Deleted => default;

    }

    public sealed class Result<T> : IResult<T>
    {
        private readonly T? _value = default;
        private readonly List<Error>? _errors = null;
        public bool IsSuccess { get; }
        public bool IsError => !IsSuccess;

        private Result(Error error)
        {
            _errors = [error];
        }

        private Result(List<Error> errors)
        {
            if (errors == null || !errors.Any())
                throw new ArgumentNullException(nameof(errors));
        }

        private Result(T value)
        {   if (value == null)
                throw new ArgumentNullException(nameof(value));
            _value = value;
            IsSuccess = true;
        }

        public List<Error>? Errors => IsError ? _errors!:[];
        public T Value => IsSuccess ? _value! : default;
        public Error TopError => IsError ? _errors!.First() : default;

        public static implicit operator Result<T>(T value) => new Result<T>(value);
        public static implicit operator Result<T>(Error error) => new Result<T>(error);
        public static implicit operator Result<T>(List<Error> errors) => new Result<T>(errors);

        [JsonConstructor]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("For serializer only.",true)]
        public Result(T? value,List<Error>? errors,bool isSuccess)
        {
            if (isSuccess)
            {
                _value = value ?? throw new ArgumentNullException(nameof(value));
                IsSuccess = true;
                _errors = [];
            }
            else
            {
                if(errors == null || !errors.Any())
                    throw new ArgumentNullException(nameof(errors));
                _errors = errors;
                _value = default;
                IsSuccess = false;
            }
        }
        public TNextValue Match<TNextValue>(Func<T, TNextValue> onSuccess, Func<List<Error>, TNextValue> onError)
        {
            if (IsSuccess)
                return onSuccess(_value!);
            else
                return onError(_errors!);
        }
    } 
}


public readonly record struct Success;
public readonly record struct Created;
public readonly record struct Updated;
public readonly record struct Deleted;
