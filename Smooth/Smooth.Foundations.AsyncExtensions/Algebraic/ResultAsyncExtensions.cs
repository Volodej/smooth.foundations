using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Smooth.Algebraics;
using Smooth.Algebraics.Results;
using Smooth.Algebraics.Results.Exceptions;

namespace Smooth.Foundations.AsyncExtensions.Algebraic
{
    // TODO: create correct solution for .ConfigureAwait(false) -- Partially implemented but not tested
    // TODO: split methods to few classes
    // TODO: create tests for async extensions
    public static class ResultAsyncExtensions
    {
        public static async Task<Result<T>> OrAsync<T>(this Task<Result<T>> resultTask, Result<T> elseResult)
        {
            var result = await resultTask;
            return result.IsError ? elseResult : result;
        }

        public static async Task<Result<T>> OrAsync<T>(this Task<Result<T>> resultTask, Task<Result<T>> elseResult)
        {
            var result = await resultTask;
            return result.IsError ? await elseResult : result;
        }

        public static async Task<T> ValueOrAsync<T>(this Task<Result<T>> resultTask, T elseValue)
        {
            var result = await resultTask;
            return result.IsError ? elseValue : result.Value;
        }

        public static async Task<T> ValueOrAsync<T>(this Task<Result<T>> resultTask, Func<T> elseValue)
        {
            var result = await resultTask;
            return result.IsError ? elseValue() : result.Value;
        }

        #region ThenAsync

        public static Task<Result<TResult>> ThenAsync<TValue, TResult>(this Result<TValue> result, Func<TValue, Task<TResult>> func)
        {
            return result.IsError
                ? Task.FromResult(Result<TResult>.FromError(result.Error))
                : func(result.Value).ContinueWith(task => Result<TResult>.FromValue(task.Result));
        }

        public static Task<Result<TResult>> ThenAsync<TValue, TResult>(this Result<TValue> result, Func<TValue, Task<Result<TResult>>> func)
        {
            return result.IsError
                ? Task.FromResult(Result<TResult>.FromError(result.Error))
                : func(result.Value);
        }

        public static Task<Result<TResult>> ThenAsync<TValue, TResult>(this Task<Result<TValue>> resultTask, Func<TValue, TResult> func)
        {
            return resultTask.ContinueWith(task => task.Result.Then(func));
        }

        public static Task<Result<TResult>> ThenAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, Result<TResult>> func)
        {
            return resultTask.ContinueWith(task => task.Result.Then(func));
        }

        public static Task<Result<TResult>> ThenAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, Task<TResult>> func)
        {
            return resultTask.ContinueWith(task => task.Result.ThenAsync(func)).Unwrap();
        }

        public static Task<Result<TResult>> ThenAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, Task<Result<TResult>>> func)
        {
            return resultTask.ContinueWith(task => task.Result.ThenAsync(func)).Unwrap();
        }

        #endregion

        #region ThenAsync with ConfiguredTaskAwaitable

        public static async Task<Result<TResult>> ThenAsync<TValue, TResult>(this Result<TValue> result,
            Func<TValue, ConfiguredTaskAwaitable<TResult>> func)
        {
            return result.IsError
                ? Result<TResult>.FromError(result.Error)
                : Result<TResult>.FromValue(await func(result.Value));
        }

        public static async Task<Result<TResult>> ThenAsync<TValue, TResult>(this Result<TValue> result,
            Func<TValue, ConfiguredTaskAwaitable<Result<TResult>>> func)
        {
            return result.IsError
                ? Result<TResult>.FromError(result.Error)
                : await func(result.Value);
        }

        public static async Task<Result<TResult>> ThenAsync<TValue, TResult>(this ConfiguredTaskAwaitable<Result<TValue>> resultTask,
            Func<TValue, TResult> func)
        {
            var result = await resultTask;
            return result.Then(func);
        }

        public static async Task<Result<TResult>> ThenAsync<TValue, TResult>(this ConfiguredTaskAwaitable<Result<TValue>> resultTask,
            Func<TValue, Result<TResult>> func)
        {
            var result = await resultTask;
            return result.Then(func);
        }

        public static async Task<Result<TResult>> ThenAsync<TValue, TResult>(this ConfiguredTaskAwaitable<Result<TValue>> resultTask,
            Func<TValue, Task<TResult>> func)
        {
            var result = await resultTask;
            return await result.ThenAsync(func).ConfigureAwait(false);
        }

        public static async Task<Result<TResult>> ThenAsync<TValue, TResult>(this ConfiguredTaskAwaitable<Result<TValue>> resultTask,
            Func<TValue, Task<Result<TResult>>> func)
        {
            var result = await resultTask;
            return await result.ThenAsync(func).ConfigureAwait(false);
        }

        public static async Task<Result<TResult>> ThenAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, ConfiguredTaskAwaitable<TResult>> func)
        {
            var result = await resultTask;
            return await result.ThenAsync(func).ConfigureAwait(false);
        }

        public static async Task<Result<TResult>> ThenAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, ConfiguredTaskAwaitable<Result<TResult>>> func)
        {
            var result = await resultTask;
            return await result.ThenAsync(func).ConfigureAwait(false);
        }

        public static async Task<Result<TResult>> ThenAsync<TValue, TResult>(this ConfiguredTaskAwaitable<Result<TValue>> resultTask,
            Func<TValue, ConfiguredTaskAwaitable<TResult>> func)
        {
            var result = await resultTask;
            return await result.ThenAsync(func).ConfigureAwait(false);
        }

        public static async Task<Result<TResult>> ThenAsync<TValue, TResult>(this ConfiguredTaskAwaitable<Result<TValue>> resultTask,
            Func<TValue, ConfiguredTaskAwaitable<Result<TResult>>> func)
        {
            var result = await resultTask;
            return await result.ThenAsync(func).ConfigureAwait(false);
        }

        #endregion

        #region ThenTryAsync

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this Result<TValue> result, Func<TValue, Task<TResult>> func)
        {
            if (result.IsError)
                return Task.FromResult(ResultEx<TResult>.FromError(new ResultErrorException(result.Error)));

            try
            {
                return func(result.Value).ContinueWith(task => ResultEx<TResult>.FromValue(task.Result));
            }
            catch (Exception e)
            {
                return Task.FromResult(ResultEx<TResult>.FromError(e));
            }
        }

        public static Task<Result<TResult>> ThenTryAsync<TValue, TResult>(this Result<TValue> result, Func<TValue, Task<TResult>> func,
            Func<Exception, string> catchFunc)
        {
            if (result.IsError)
                return Task.FromResult(Result<TResult>.FromError(result.Error));

            try
            {
                return func(result.Value).ContinueWith(task => Result<TResult>.FromValue(task.Result));
            }
            catch (Exception e)
            {
                return Task.FromResult(Result<TResult>.FromError(catchFunc(e)));
            }
        }

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, TResult> func)
        {
            return resultTask.ContinueWith(task => task.Result.ThenTry(func));
        }

        public static Task<Result<TResult>> ThenTryAsync<TValue, TResult>(this Task<Result<TValue>> resultTask, Func<TValue, TResult> func,
            Func<Exception, string> catchFunc)
        {
            return resultTask.ContinueWith(task => task.Result.ThenTry(func, catchFunc));
        }

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, Task<TResult>> func)
        {
            return resultTask.ContinueWith(task => task.Result.ThenTryAsync(func)).Unwrap();
        }

        public static Task<Result<TResult>> ThenTryAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, Task<TResult>> func,
            Func<Exception, string> catchFunc)
        {
            return resultTask.ContinueWith(task => task.Result.ThenTryAsync(func, catchFunc)).Unwrap();
        }

        #endregion

        #region ThenTryAsync with ConfiguredTaskAwaitable

        public static async Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this Result<TValue> result,
            Func<TValue, ConfiguredTaskAwaitable<TResult>> func)
        {
            if (result.IsError)
                return ResultEx<TResult>.FromError(new ResultErrorException(result.Error));

            try
            {
                return ResultEx<TResult>.FromValue(await func(result.Value));
            }
            catch (Exception e)
            {
                return ResultEx<TResult>.FromError(e);
            }
        }

        public static async Task<Result<TResult>> ThenTryAsync<TValue, TResult>(this Result<TValue> result,
            Func<TValue, ConfiguredTaskAwaitable<TResult>> func,
            Func<Exception, string> catchFunc)
        {
            if (result.IsError)
                return Result<TResult>.FromError(result.Error);

            try
            {
                return Result<TResult>.FromValue(await func(result.Value));
            }
            catch (Exception e)
            {
                return Result<TResult>.FromError(catchFunc(e));
            }
        }

        public static async Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this ConfiguredTaskAwaitable<Result<TValue>> resultTask,
            Func<TValue, TResult> func)
        {
            var result = await resultTask;
            return result.ThenTry(func);
        }

        public static async Task<Result<TResult>> ThenTryAsync<TValue, TResult>(this ConfiguredTaskAwaitable<Result<TValue>> resultTask,
            Func<TValue, TResult> func,
            Func<Exception, string> catchFunc)
        {
            var result = await resultTask;
            return result.ThenTry(func, catchFunc);
        }

        public static async Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this ConfiguredTaskAwaitable<Result<TValue>> resultTask,
            Func<TValue, Task<TResult>> func)
        {
            var result = await resultTask;
            return await result.ThenTryAsync(func).ConfigureAwait(false);
        }

        public static async Task<Result<TResult>> ThenTryAsync<TValue, TResult>(this ConfiguredTaskAwaitable<Result<TValue>> resultTask,
            Func<TValue, Task<TResult>> func,
            Func<Exception, string> catchFunc)
        {
            var result = await resultTask;
            return await result.ThenTryAsync(func, catchFunc).ConfigureAwait(false);
        }

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, ConfiguredTaskAwaitable<TResult>> func)
        {
            return resultTask.ContinueWith(task => task.Result.ThenTryAsync(func)).Unwrap();
        }

        public static Task<Result<TResult>> ThenTryAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, ConfiguredTaskAwaitable<TResult>> func,
            Func<Exception, string> catchFunc)
        {
            return resultTask.ContinueWith(task => task.Result.ThenTryAsync(func, catchFunc)).Unwrap();
        }

        public static async Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this ConfiguredTaskAwaitable<Result<TValue>> resultTask,
            Func<TValue, ConfiguredTaskAwaitable<TResult>> func)
        {
            var result = await resultTask;
            if (result.IsError)
                return ResultEx<TResult>.FromError(new ResultErrorException(result.Error));

            try
            {
                return ResultEx<TResult>.FromValue(await func(result.Value));
            }
            catch (Exception e)
            {
                return ResultEx<TResult>.FromError(e);
            }
        }

        public static async Task<Result<TResult>> ThenTryAsync<TValue, TResult>(this ConfiguredTaskAwaitable<Result<TValue>> resultTask,
            Func<TValue, ConfiguredTaskAwaitable<TResult>> func,
            Func<Exception, string> catchFunc)
        {
            var result = await resultTask;
            if (result.IsError)
                return Result<TResult>.FromError(result.Error);

            try
            {
                return Result<TResult>.FromValue(await func(result.Value));
            }
            catch (Exception e)
            {
                return Result<TResult>.FromError(catchFunc(e));
            }
        }

        #endregion

        public static Task<Result<TValue>> SpecifyErrorAsync<TValue>(this Task<Result<TValue>> resultTask, string error)
        {
            return resultTask.ContinueWith(task => task.Result.SpecifyError(error));
        }

        public static Task<Result<TValue>> SelectIfErrorAsync<TValue>(this Task<Result<TValue>> resultTask,
            Func<string, string> errorSelector)
        {
            return resultTask.ContinueWith(task => task.Result.SelectIfError(errorSelector));
        }

        public static async Task<Result<TResult>> ThenAsync<T, TResult>(this Result<T> valueOrError,
            Func<T, Task<Result<TResult>>> func, string errorMessage = null)
        {
            if (valueOrError.IsError)
                return Result<TResult>.FromError(errorMessage == null ? valueOrError.Error : $"{errorMessage}: {valueOrError.Error}");

            return await func(valueOrError.Value);
        }

        public static async Task<Option<T>> ToOptionAsync<T>(this Task<Result<T>> resultTask)
        {
            var result = await resultTask;
            return !result.IsError ? result.Value.ToOption() : Option<T>.None;
        }

        public static async Task<Result<T>> Where<T>(this Task<Result<T>> resultTask, Func<T, bool> predicate,
            string errorMessage)
        {
            var result = await resultTask;
            if (result.IsError) return Result<T>.FromError(result.Error);

            return predicate(result.Value)
                ? Result<T>.FromValue(result.Value)
                : Result<T>.FromError($"Value didn't satisfy condition: {errorMessage}");
        }

        public static async Task<Result<T>> Where<T>(this Task<Result<T>> resultTask, Func<T, bool> predicate,
            Func<T, string> errorMessageFunc)
        {
            var result = await resultTask;
            if (result.IsError) return Result<T>.FromError(result.Error);

            return predicate(result.Value)
                ? Result<T>.FromValue(result.Value)
                : Result<T>.FromError($"Value didn't satisfy condition: {errorMessageFunc(result.Value)}");
        }

        public static async Task<Result<T>> Where<T, TP>(this Task<Result<T>> resultTask, Func<T, TP, bool> predicate,
            TP param, string errorMessage)
        {
            var result = await resultTask;
            if (result.IsError) return Result<T>.FromError(result.Error);

            return predicate(result.Value, param)
                ? Result<T>.FromValue(result.Value)
                : Result<T>.FromError($"Value didn't satisfy condition: {errorMessage}");
        }

        public static async Task<Result<T>> WhereKeepError<T>(this Task<Result<T>> resultTask, Func<T, bool> predicate,
            Func<T, string> errorMessageFunc)
        {
            var result = await resultTask;
            return result.IsError || predicate(result.Value)
                ? result
                : Result<T>.FromError(errorMessageFunc(result.Value));
        }


        public static async Task ForEach<T>(this Task<Result<T>> resultTask, Action<T> action)
        {
            var result = await resultTask;
            if (!result.IsError)
                action(result.Value);
        }

        public static async Task ForEach<T, TP>(this Task<Result<T>> resultTask, Action<T, TP> action, TP param)
        {
            var result = await resultTask;
            if (!result.IsError)
                action(result.Value, param);
        }

        public static async Task ForEachOr<T, TP>(this Task<Result<T>> resultTask, Action<T> valueAction,
            Action<string, TP> errorAction, TP errorParam)
        {
            var result = await resultTask;
            if (!result.IsError)
                valueAction(result.Value);
            else
                errorAction(result.Error, errorParam);
        }

        public static async Task ForEachOr<T, TP>(this Task<Result<T>> resultTask, Action<T, TP> valueAction, TP param,
            Action<string> errorAction)
        {
            var result = await resultTask;
            if (!result.IsError)
                valueAction(result.Value, param);
            else
                errorAction(result.Error);
        }

        public static async Task ForEachOr<T, TP1, TP2>(this Task<Result<T>> resultTask, Action<T, TP1> valueAction, TP1 valueParam,
            Action<string, TP2> errorAction, TP2 errorParam)
        {
            var result = await resultTask;
            if (!result.IsError)
                valueAction(result.Value, valueParam);
            else
                errorAction(result.Error, errorParam);
        }
    }
}