using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smooth.Algebraics;
using Smooth.Foundations.Algebraics;
using Smooth.Slinq;
// ReSharper disable UnusedMember.Global

namespace Smooth.Extensions.Algebraic
{
    public static class ValueOrErrorExtensions
    {
        public static ValueOrError<T> Or<T>(this ValueOrError<T> voe1, Func<ValueOrError<T>> voe2)
            => voe1.IsError ? voe2() : voe1;

        public static async Task<ValueOrError<T>> OrAsync<T>(this Task<ValueOrError<T>> voeTask, ValueOrError<T> elseVoe)
        {
            var voe = await voeTask;
            return voe.IsError ? elseVoe : voe;
        }

        public static async Task<ValueOrError<T>> OrAsync<T>(this Task<ValueOrError<T>> voeTask, Task<ValueOrError<T>> elseVoe)
        {
            var voe = await voeTask;
            return voe.IsError ? await elseVoe : voe;
        }

        public static T ValueOr<T>(this ValueOrError<T> voe, T elseValue)
        {
            return voe.IsError ? elseValue : voe.Value;
        }

        public static T ValueOr<T>(this ValueOrError<T> voe, Func<T> elseValue)
        {
            return voe.IsError ? elseValue() : voe.Value;
        }

        public static T ValueOr<T, TP>(this ValueOrError<T> voe, Func<TP, T> elseValue, TP param)
        {
            return voe.IsError ? elseValue(param) : voe.Value;
        }

        public static async Task<T> ValueOrAsync<T>(this Task<ValueOrError<T>> voeTask, T elseValue)
        {
            var voe = await voeTask;
            return voe.IsError ? elseValue : voe.Value;
        }

        public static async Task<T> ValueOrAsync<T>(this Task<ValueOrError<T>> voeTask, Func<T> elseValue)
        {
            var voe = await voeTask;
            return voe.IsError ? elseValue() : voe.Value;
        }

        public static async Task<ValueOrError<TResult>> ContinueWithAsync<T, TResult>(this ValueOrError<T> valueOrError,
            Func<T, Task<ValueOrError<TResult>>> func, string errorMessage = null)
        {
            if (valueOrError.IsError)
                return ValueOrError<TResult>.FromError(errorMessage == null ? valueOrError.Error : $"{errorMessage}: {valueOrError.Error}");

            return await func(valueOrError.Value);
        }

        public static async Task<ValueOrError<TResult>> ContinueWithAsync<T, TResult>(this ValueOrError<T> valueOrError,
            Func<T, Task<TResult>> func, string errorMessage = null)
        {
            if (valueOrError.IsError)
                return ValueOrError<TResult>.FromError(errorMessage == null ? valueOrError.Error : $"{errorMessage}: {valueOrError.Error}");

            try
            {
                var funcResult = await func(valueOrError.Value);
                return funcResult.ToValue();
            }
            catch (Exception ex)
            {
                var additionalMessage = errorMessage == null ? "" : $"{errorMessage}: ";
                return ValueOrError<TResult>.FromError($"{additionalMessage}Exception: {ex.Message}; StackTrace:\n{ex.StackTrace}\n");
            }
        }

        public static async Task<ValueOrError<TResult>> ContinueWithAsync<T, TP, TResult>(this ValueOrError<T> valueOrError,
            Func<T, TP, Task<ValueOrError<TResult>>> func, TP param, string errorMessage = null)
        {
            if (valueOrError.IsError)
                return ValueOrError<TResult>.FromError(errorMessage == null ? valueOrError.Error : $"{errorMessage}: {valueOrError.Error}");

            return await func(valueOrError.Value, param);
        }

        public static Option<T> ToOption<T>(ValueOrError<T> voe)
        {
            return !voe.IsError ? voe.Value.ToSome() : Option<T>.None;
        }
        
        public static ValueOrError<T> ToValueOrError<T>(this T o)
            => o != null
                ? ValueOrError<T>.FromValue(o)
                : ValueOrError<T>.FromError("Value was null");

        public static ValueOrError<T> ToValueOrError<T>(this Option<T> o)
        {
            return o.isSome
                ? ValueOrError<T>.FromValue(o.value)
                : ValueOrError<T>.FromError("Value was missing");
        }

        public static ValueOrError<T> ToValueOrError<T>(this Option<T> o, string errorMessage)
        {
            return o.isSome
                ? ValueOrError<T>.FromValue(o.value)
                : ValueOrError<T>.FromError(errorMessage);
        }

        public static ValueOrError<T> ToValueOrError<T>(this Option<T> o, Func<string> errorFunc)
        {
            return o.Cata((v, _) => v.ToValue(), Unit.Default, f => ValueOrError<T>.FromError(f()), errorFunc);
        }

        public static ValueOrError<T> ToValueOrError<T, TP>(this Option<T> o, Func<TP, string> errorFunc, TP errorParam)
        {
            return o.Cata((v, _) => v.ToValue(), Unit.Default,
                t => ValueOrError<T>.FromError(t.func(t.param)), (func: errorFunc, param: errorParam));
        }

        public static async Task<ValueOrError<T>> ToValueOrErrorAsync<T>(this Task<Option<T>> optionTask)
        {
            var option = await optionTask;
            return option.isSome
                ? ValueOrError<T>.FromValue(option.value)
                : ValueOrError<T>.FromError("Value was missing");
        }

        public static async Task<ValueOrError<T>> ToValueOrErrorAsync<T>(this Task<Option<T>> optionTask, string errorMessage)
        {
            var option = await optionTask;
            return option.isSome
                ? ValueOrError<T>.FromValue(option.value)
                : ValueOrError<T>.FromError(errorMessage);
        }

        public static ValueOrError<T> ToValue<T>(this T v)
        {
            return ValueOrError<T>.FromValue(v);
        }

        public static ValueOrError<T> ToError<T>(this T _, string error)
        {
            return string.IsNullOrEmpty(error)
                ? ValueOrError<T>.FromError("Generic error")
                : ValueOrError<T>.FromError(error);
        }

        public static ValueOrError<T> IfError<T>(this ValueOrError<T> voe, Action<string> func)
        {
            if (!voe.IsError) return voe;

            try
            {
                func(voe.Error);
            }
            catch (Exception e)
            {
                return ValueOrError<T>.FromError(e.Message);
            }

            return voe;
        }

        public static async Task<ValueOrError<T>> IfErrorAsync<T>(this Task<ValueOrError<T>> voe, Action<string> func)
        {
            var result = await voe;

            if (!result.IsError) return result;

            try
            {
                func(result.Error);
            }
            catch (Exception e)
            {
                return ValueOrError<T>.FromError(e.Message);
            }

            return result;
        }

        public static ValueOrError<T> IfError<T, TP>(this ValueOrError<T> voe, Action<string, TP> func, TP param)
        {
            if (!voe.IsError) return voe;
            try
            {
                func(voe.Error, param);
            }
            catch (Exception e)
            {
                return ValueOrError<T>.FromError(e.Message);
            }

            return voe;
        }

        public static async Task<ValueOrError<TResult>> ContinueWithAsync<T, TResult>(this Task<ValueOrError<T>> valueOrErrorTask,
            Func<T, Task<ValueOrError<TResult>>> func, string errorMessage = null)
        {
            var valueOrError = await valueOrErrorTask;
            if (valueOrError.IsError)
                return ValueOrError<TResult>.FromError(errorMessage == null ? valueOrError.Error : $"{errorMessage}: {valueOrError.Error}");

            return await func(valueOrError.Value);
        }

        public static async Task<ValueOrError<TResult>> ContinueWithAsync<T, TResult>(this Task<ValueOrError<T>> valueOrErrorTask,
            Func<T, ValueOrError<TResult>> func, string errorMessage = null)
        {
            var valueOrError = await valueOrErrorTask;
            return valueOrError.ContinueWith((vor, f) => f(vor), func, errorMessage);
        }

        public static async Task<ValueOrError<TResult>> ContinueWithAsync<T, TResult, TP>(this Task<ValueOrError<T>> valueOrErrorTask,
            Func<T, TP, ValueOrError<TResult>> func, TP param, string errorMessage = null)
        {
            var valueOrError = await valueOrErrorTask;
            if (valueOrError.IsError)
            {
                return ValueOrError<TResult>.FromError(errorMessage == null ? valueOrError.Error : $"{errorMessage}: {valueOrError.Error}");
            }

            try
            {
                return func(valueOrError.Value, param);
            }
            catch (Exception ex)
            {
                var additionalMessage = errorMessage == null ? "" : $"{errorMessage}: ";
                return ValueOrError<TResult>.FromError($"{additionalMessage}Exception: {ex.Message}; StackTrace:\n{ex.StackTrace}\n");
            }
        }

        public static async Task<ValueOrError<TResult>> ContinueWithAsync<T, TResult>(this Task<ValueOrError<T>> valueOrErrorTask,
            Func<T, TResult> func, string errorMessage = null)
        {
            var taskResult = await valueOrErrorTask;
            return taskResult.ContinueWith((v, f) => f(v), func, errorMessage);
        }

        public static async Task<ValueOrError<TResult>> ContinueWithAsync<T, TResult>(this Task<ValueOrError<T>> valueOrErrorTask,
            Func<T, Task<TResult>> func, string errorMessage = null)
        {
            var valueOrError = await valueOrErrorTask;
            if (valueOrError.IsError)
                return ValueOrError<TResult>.FromError(errorMessage == null ? valueOrError.Error : $"{errorMessage}: {valueOrError.Error}");
            try
            {
                var result = await func(valueOrError.Value);
                return ValueOrError<TResult>.FromValue(result);
            }
            catch (Exception ex)
            {
                var additionalMessage = errorMessage == null ? "" : $"{errorMessage}: ";
                return ValueOrError<TResult>.FromError($"{additionalMessage}Exception: {ex.Message}; StackTrace:\n{ex.StackTrace}\n");
            }
        }

        public static async Task<ValueOrError<TResult>> ContinueWithAsync<T, TResult, TP>(this Task<ValueOrError<T>> voeTask,
            Func<T, TP, TResult> func, TP param, string errorMessage = null)
        {
            var valueOrError = await voeTask;
            if (valueOrError.IsError)
            {
                return ValueOrError<TResult>.FromError(errorMessage == null ? valueOrError.Error : $"{errorMessage}: {valueOrError.Error}");
            }

            try
            {
                var res = func(valueOrError.Value, param);
                return ValueOrError.FromValue(res);
            }
            catch (Exception ex)
            {
                var additionalMessage = errorMessage == null ? "" : $"{errorMessage}: ";
                return ValueOrError<TResult>.FromError($"{additionalMessage}Exception: {ex.Message}; StackTrace:\n{ex.StackTrace}\n");
            }
        }

        public static async Task<ValueOrError<TResult>> ContinueWithAsync<T, TResult, TP>(this Task<ValueOrError<T>> voeTask,
            Func<T, TP, Task<TResult>> func, TP param, string errorMessage = null)
        {
            var valueOrError = await voeTask;
            if (valueOrError.IsError)
            {
                return ValueOrError<TResult>.FromError(errorMessage == null ? valueOrError.Error : $"{errorMessage}: {valueOrError.Error}");
            }

            try
            {
                var res = await func(valueOrError.Value, param);
                return ValueOrError.FromValue(res);
            }
            catch (Exception ex)
            {
                var additionalMessage = errorMessage == null ? "" : $"{errorMessage}: ";
                return ValueOrError<TResult>.FromError($"{additionalMessage}Exception: {ex.Message}; StackTrace:\n{ex.StackTrace}\n");
            }
        }

        public static async Task<ValueOrError<TResult>> ContinueWithAsync<T, TResult, TP>(this Task<ValueOrError<T>> voeTask,
            Func<T, TP, Task<ValueOrError<TResult>>> func, TP param, string errorMessage = null)
        {
            var valueOrError = await voeTask;
            if (valueOrError.IsError)
                return ValueOrError<TResult>.FromError(errorMessage == null ? valueOrError.Error : $"{errorMessage}: {valueOrError.Error}");

            return await func(valueOrError.Value, param);
        }

        public static ValueOrError<TResult> ContinueWith<T, TResult, TP>(this ValueOrError<T> valueOrError,
            Func<T, TP, ValueOrError<TResult>> func, TP arg, string errorMessage = null)
        {
            if (valueOrError.IsError)
            {
                return ValueOrError<TResult>.FromError(errorMessage == null ? valueOrError.Error : $"{errorMessage}: {valueOrError.Error}");
            }

            try
            {
                return func(valueOrError.Value, arg);
            }
            catch (Exception ex)
            {
                var additionalMessage = errorMessage == null ? "" : $"{errorMessage}: ";
                return ValueOrError<TResult>.FromError($"{additionalMessage}Exception: {ex.Message}; StackTrace:\n{ex.StackTrace}\n");
            }
        }

        public static ValueOrError<TResult> ContinueWith<T, TResult, TP>(this ValueOrError<T> valueOrError,
            Func<T, TP, TResult> func, TP arg, string errorMessage = null)
        {
            if (valueOrError.IsError)
                return ValueOrError<TResult>.FromError(errorMessage == null ? valueOrError.Error : $"{errorMessage}: {valueOrError.Error}");
            try
            {
                return ValueOrError<TResult>.FromValue(func(valueOrError.Value, arg));
            }
            catch (Exception ex)
            {
                var additionalMessage = errorMessage == null ? "" : $"{errorMessage}: ";
                return ValueOrError<TResult>.FromError($"{additionalMessage}Exception: {ex.Message}; StackTrace:\n{ex.StackTrace}\n");
            }
        }

        public static async Task<Option<T>> ToOptionAsync<T>(this Task<ValueOrError<T>> voeTask)
        {
            var result = await voeTask;
            return !result.IsError ? result.Value.ToOption() : Option<T>.None;
        }

        public static ValueOrError<T> Where<T>(this ValueOrError<T> voe, Func<T, bool> predicate, string errorMessage)
        {
            if (voe.IsError)
            {
                return voe;
            }

            return predicate(voe.Value)
                ? ValueOrError<T>.FromValue(voe.Value)
                : ValueOrError<T>.FromError($"Value didn't satisfy condition: {errorMessage}");
        }

        public static ValueOrError<T> Where<T, TP>(this ValueOrError<T> voe, Func<T, TP, bool> predicate, TP param,
            string errorMessage)
        {
            if (voe.IsError)
            {
                return ValueOrError<T>.FromError(voe.Error);
            }

            return predicate(voe.Value, param)
                ? ValueOrError<T>.FromValue(voe.Value)
                : ValueOrError<T>.FromError($"Value didn't satisfy condition: {errorMessage}");
        }

        public static ValueOrError<T> Where<T>(this ValueOrError<T> voe, Func<T, bool> predicate,
            Func<T, string> errorMessageFunc)
        {
            return voe.IsError || predicate(voe.Value)
                ? voe
                : ValueOrError<T>.FromError(errorMessageFunc(voe.Value));
        }

        public static async Task<ValueOrError<T>> Where<T>(this Task<ValueOrError<T>> voeTask, Func<T, bool> predicate,
            string errorMessage)
        {
            var voe = await voeTask;
            if (voe.IsError)
            {
                return ValueOrError<T>.FromError(voe.Error);
            }

            return predicate(voe.Value)
                ? ValueOrError<T>.FromValue(voe.Value)
                : ValueOrError<T>.FromError($"Value didn't satisfy condition: {errorMessage}");
        }

        public static async Task<ValueOrError<T>> Where<T>(this Task<ValueOrError<T>> voeTask, Func<T, bool> predicate,
            Func<T, string> errorMessageFunc)
        {
            var voe = await voeTask;
            if (voe.IsError)
            {
                return ValueOrError<T>.FromError(voe.Error);
            }

            return predicate(voe.Value)
                ? ValueOrError<T>.FromValue(voe.Value)
                : ValueOrError<T>.FromError($"Value didn't satisfy condition: {errorMessageFunc(voe.Value)}");
        }

        public static async Task<ValueOrError<T>> Where<T, TP>(this Task<ValueOrError<T>> voeTask, Func<T, TP, bool> predicate,
            TP param, string errorMessage)
        {
            var voe = await voeTask;
            if (voe.IsError)
            {
                return ValueOrError<T>.FromError(voe.Error);
            }

            return predicate(voe.Value, param)
                ? ValueOrError<T>.FromValue(voe.Value)
                : ValueOrError<T>.FromError($"Value didn't satisfy condition: {errorMessage}");
        }

        public static async Task<ValueOrError<T>> WhereKeepError<T>(this Task<ValueOrError<T>> voeTask, Func<T, bool> predicate,
            Func<T, string> errorMessageFunc)
        {
            var voe = await voeTask;
            return voe.IsError || predicate(voe.Value)
                ? voe
                : ValueOrError<T>.FromError(errorMessageFunc(voe.Value));
        }

        public static void ForEach<T>(this ValueOrError<T> voe, Action<T> action)
        {
            if (!voe.IsError)
                action(voe.Value);
        }

        public static void ForEach<T, TP>(this ValueOrError<T> voe, Action<T, TP> action, TP param)
        {
            if (!voe.IsError)
                action(voe.Value, param);
        }

        public static async Task ForEach<T>(this Task<ValueOrError<T>> voeTask, Action<T> action)
        {
            var voe = await voeTask;
            if (!voe.IsError)
                action(voe.Value);
        }

        public static async Task ForEach<T, TP>(this Task<ValueOrError<T>> voeTask, Action<T, TP> action, TP param)
        {
            var voe = await voeTask;
            if (!voe.IsError)
                action(voe.Value, param);
        }

        public static async Task ForEachOr<T, TP>(this Task<ValueOrError<T>> voeTask, Action<T> valueAction,
            Action<string, TP> errorAction, TP errorParam)
        {
            var voe = await voeTask;
            if (!voe.IsError)
                valueAction(voe.Value);
            else
                errorAction(voe.Error, errorParam);
        }

        public static async Task ForEachOr<T, TP>(this Task<ValueOrError<T>> voeTask, Action<T, TP> valueAction, TP param,
            Action<string> errorAction)
        {
            var voe = await voeTask;
            if (!voe.IsError)
                valueAction(voe.Value, param);
            else
                errorAction(voe.Error);
        }

        public static async Task ForEachOr<T, TP1, TP2>(this Task<ValueOrError<T>> voeTask, Action<T, TP1> valueAction, TP1 valueParam,
            Action<string, TP2> errorAction, TP2 errorParam)
        {
            var voe = await voeTask;
            if (!voe.IsError)
                valueAction(voe.Value, valueParam);
            else
                errorAction(voe.Error, errorParam);
        }

        public static ValueOrError<TResult> All<T, TResult>(ValueOrError<T>[] vs, Func<IEnumerable<T>,
            TResult> func)
        {
            if (vs.Slinq().Any(v => v.IsError))
            {
                return ValueOrError<TResult>.FromError(vs.Slinq().First(v => v.IsError).Error);
            }

            return ValueOrError<TResult>.FromValue(func(vs.Select(v => v.Value)));
        }
    }
}