using System;
using System.Threading.Tasks;
using Smooth.Algebraics;
using Smooth.Algebraics.Results;

namespace Smooth.Foundations.AsyncExtensions.Algebraic
{
    // TODO: create correct solution for .ConfigureAwait(false)
    public static class OptionAsyncExtensions
    {
        public static async Task<Option<T>> ToAsync<T>(this Option<Task<T>> option)
        {
            if (option.isNone)
                return Option<T>.None;

            var res = await option.value;
            return res.ToOption();
        }

        public static async Task<Option<TResult>> SelectAsync<T, TResult>(this Task<Option<T>> optionTask,
            Func<T, Task<TResult>> func)
        {
            var option = await optionTask;
            if (option.isNone)
                return Option<TResult>.None;
            var res = await func(option.value);
            return res.ToSome();
        }

        public static async Task<Option<TResult>> SelectAsync<T, TResult>(this Task<Option<T>> optionTask,
            Func<T, Task<Option<TResult>>> func)
        {
            var option = await optionTask;
            if (option.isNone)
                return Option<TResult>.None;
            return await func(option.value);
        }

        public static async Task<Option<TResult>> SelectAsync<T, TResult>(this Task<Option<T>> optionTask,
            Func<T, TResult> func)
        {
            var option = await optionTask;
            if (option.isNone)
                return Option<TResult>.None;

            var res = func(option.value);
            return res.ToSome();
        }

        public static async Task<Option<T>> OrAsync<T>(this Task<Option<T>> optionTask, Func<Option<T>> noneFunc)
        {
            var option = await optionTask;
            if (option.isNone)
                return noneFunc();

            return option;
        }

        public static async Task<Option<T>> OrAsync<T>(this Task<Option<T>> optionTask, Option<T> noneValue)
        {
            var option = await optionTask;
            return option.isNone ? noneValue : option;
        }

        public static async Task<Option<T>> OrAsyncWithState<T, P>(this Task<Option<T>> optionTask, Func<P, Option<T>> noneFunc, P param)
        {
            var option = await optionTask;
            if (option.isNone)
                return noneFunc(param);

            return option;
        }

        public static async Task<Option<T>> OrAsync<T>(this Task<Option<T>> optionTask, Func<Task<Option<T>>> noneFunc)
        {
            var option = await optionTask;
            if (option.isNone)
                return await noneFunc();

            return option;
        }

        public static async Task<Option<T>> OrAsync<T, P>(this Task<Option<T>> optionTask, Func<P, Task<Option<T>>> noneFunc, P param)
        {
            var option = await optionTask;
            if (option.isNone)
                return await noneFunc(param);

            return option;
        }

        public static async Task<T> ValueOrAsync<T>(this Task<Option<T>> optionTask, T noneValue)
        {
            var option = await optionTask;
            if (option.isNone)
                return noneValue;

            return option.value;
        }

        public static async Task<T> ValueOrAsync<T>(this Task<Option<T>> optionTask, Func<T> noneFunc)
        {
            var option = await optionTask;
            if (option.isNone)
                return noneFunc();

            return option.value;
        }

        public static async Task<T> ValueOrAsync<T, P>(this Task<Option<T>> optionTask, Func<P, T> noneFunc, P param)
        {
            var option = await optionTask;
            if (option.isNone)
                return noneFunc(param);

            return option.value;
        }

        public static async Task<T> ValueOrAsync<T>(this Task<Option<T>> optionTask, Func<Task<T>> noneFunc)
        {
            var option = await optionTask;
            if (option.isNone)
                return await noneFunc();
            return option.value;
        }

        public static async Task<Option<TResult>> SelectManyAsync<T, TResult>(this Task<Option<T>> optionTask,
            Func<T, Option<TResult>> func)
        {
            var option = await optionTask;
            return option.isNone ? Option<TResult>.None : func(option.value);
        }
        
        public static async Task<Result<T>> ToResultAsync<T>(this Task<Option<T>> optionTask)
        {
            var option = await optionTask;
            return option.isSome
                ? Result<T>.FromValue(option.value)
                : Result<T>.FromError("Value was missing");
        }

        public static async Task<Result<T>> ToResultAsync<T>(this Task<Option<T>> optionTask, string errorMessage)
        {
            var option = await optionTask;
            return option.isSome
                ? Result<T>.FromValue(option.value)
                : Result<T>.FromError(errorMessage);
        }
    }
}