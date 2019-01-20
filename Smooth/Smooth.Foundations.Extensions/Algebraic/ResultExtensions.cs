using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Algebraics.Results;
using Smooth.Extensions.Collections;
using Smooth.Slinq;

namespace Smooth.Extensions.Algebraic
{
    public static class ResultExtensions
    {
        public static Result<T> ToResult<T>(this Option<T> o)
        {
            return o.isSome
                ? Result<T>.FromValue(o.value)
                : Result<T>.FromError("Value was missing");
        }

        public static Result<T> ToResult<T>(this Option<T> o, string errorMessage)
        {
            return o.isSome
                ? Result<T>.FromValue(o.value)
                : Result<T>.FromError(errorMessage);
        }

        public static Result<T> ToResult<T>(this Option<T> o, Func<string> errorFunc)
        {
            return o.Cata((v, _) => v.ToValue(), Unit.Default, f => Result<T>.FromError(f()), errorFunc);
        }

        public static Result<T> ToResult<T, TP>(this Option<T> o, Func<TP, string> errorFunc, TP errorParam)
        {
            return o.Cata((v, _) => v.ToValue(), Unit.Default,
                t => Result<T>.FromError(t.func(t.param)), (func: errorFunc, param: errorParam));
        }

        public static Result<T> ToValue<T>(this T v)
        {
            return Result<T>.FromValue(v);
        }

        public static Result<T> ToError<T>(this T _, string error)
        {
            return string.IsNullOrEmpty(error)
                ? Result<T>.FromError("Generic error")
                : Result<T>.FromError(error);
        }

        public static Result<T> Where<T>(this Result<T> result, Func<T, bool> predicate, string errorMessage)
        {
            if (result.IsError) return result;

            return predicate(result.Value)
                ? Result<T>.FromValue(result.Value)
                : Result<T>.FromError($"Value didn't satisfy condition: {errorMessage}");
        }

        public static Result<T> Where<T, TP>(this Result<T> result, Func<T, TP, bool> predicate, TP param,
            string errorMessage)
        {
            if (result.IsError) return Result<T>.FromError(result.Error);

            return predicate(result.Value, param)
                ? Result<T>.FromValue(result.Value)
                : Result<T>.FromError($"Value didn't satisfy condition: {errorMessage}");
        }

        public static Result<T> Where<T>(this Result<T> result, Func<T, bool> predicate,
            Func<T, string> errorMessageFunc)
        {
            return result.IsError || predicate(result.Value)
                ? result
                : Result<T>.FromError(errorMessageFunc(result.Value));
        }

        public static void ForEach<T>(this Result<T> result, Action<T> action)
        {
            if (!result.IsError)
                action(result.Value);
        }

        public static void ForEach<T, TP>(this Result<T> result, Action<T, TP> action, TP param)
        {
            if (!result.IsError)
                action(result.Value, param);
        }

        public static Result<List<T>> All<T>(IEnumerable<Result<T>> results)
        {
            return results.Slinq().AggregateWhile(Result<List<T>>.FromValue(new List<T>()),
                (list, result) => !list.IsError
                    ? result.Then((v, l) => Result<List<T>>.FromValue(l.Value.WithItem(v)), list)
                        .SpecifyError("One of items has an error")
                        .ToSome()
                    : Option<Result<List<T>>>.None);
        }
    }
}