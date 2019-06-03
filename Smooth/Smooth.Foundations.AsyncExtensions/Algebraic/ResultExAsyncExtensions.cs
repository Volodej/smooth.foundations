using System;
using System.Threading;
using System.Threading.Tasks;
using Smooth.Algebraics;
using Smooth.Algebraics.Results;

namespace Smooth.Foundations.AsyncExtensions.Algebraic
{
    public static class ResultExAsyncExtensions
    {
        public static Task<ResultEx<T>> OrAsync<T>(this Task<ResultEx<T>> resultTask, ResultEx<T> elseResult)
        {
            return resultTask.ContinueWith(task =>
            {
                var result = task.Result;
                return result.IsError ? elseResult : result;
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public static Task<ResultEx<T>> OrAsync<T>(this Task<ResultEx<T>> resultTask, Func<ResultEx<T>> elseResultFunc,
            TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    return result.IsError ? ((Func<ResultEx<T>>) f)() : result;
                }, elseResultFunc, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task<T> ValueOrAsync<T>(this Task<ResultEx<T>> resultTask, T elseValue)
        {
            return resultTask.ContinueWith(task =>
            {
                var result = task.Result;
                return result.IsError ? elseValue : result.Value;
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public static Task<T> ValueOrAsync<T>(this Task<ResultEx<T>> resultTask, Func<T> elseValueFunc, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    return result.IsError ? ((Func<T>) f)() : result.Value;
                }, elseValueFunc, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        #region ThenAsync

        public static Task<ResultEx<TResult>> ThenAsync<TValue, TResult>(this ResultEx<TValue> result, Func<TValue, Task<TResult>> func)
        {
            return result.IsError
                ? Task.FromResult(ResultEx<TResult>.FromError(result.Error))
                : func(result.Value).ContinueWith(task => ResultEx<TResult>.FromValue(task.Result), CancellationToken.None,
                    TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        public static Task<ResultEx<TResult>> ThenAsync<TValue, TResult>(this ResultEx<TValue> result, Func<TValue, Task<ResultEx<TResult>>> func)
        {
            return result.IsError
                ? Task.FromResult(ResultEx<TResult>.FromError(result.Error))
                : func(result.Value);
        }

        public static Task<ResultEx<TResult>> ThenAsync<TValue, TResult>(this Task<ResultEx<TValue>> resultTask, Func<TValue, TResult> func,
            TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) => task.Result.Then((Func<TValue, TResult>) f), func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default);
        }

        public static Task<ResultEx<TResult>> ThenAsync<TValue, TResult>(this Task<ResultEx<TValue>> resultTask,
            Func<TValue, ResultEx<TResult>> func, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) => task.Result.Then((Func<TValue, ResultEx<TResult>>) f), func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default);
        }

        public static Task<ResultEx<TResult>> ThenAsync<TValue, TResult>(this Task<ResultEx<TValue>> resultTask,
            Func<TValue, Task<TResult>> func, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    return task.Result.IsError
                        ? Task.FromResult(ResultEx<TResult>.FromError(task.Result.Error))
                        : ((Func<TValue, Task<TResult>>) f)(task.Result.Value)
                        .ContinueWith(t => ResultEx.FromValue(t.Result), TaskContinuationOptions.ExecuteSynchronously);
                }, func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default).Unwrap();
        }

        public static Task<ResultEx<TResult>> ThenAsync<TValue, TResult>(this Task<ResultEx<TValue>> resultTask,
            Func<TValue, Task<ResultEx<TResult>>> func, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    return task.Result.IsError
                        ? Task.FromResult(ResultEx<TResult>.FromError(task.Result.Error))
                        : ((Func<TValue, Task<ResultEx<TResult>>>) f)(task.Result.Value)
                        .ContinueWith(t => t.Result, TaskContinuationOptions.ExecuteSynchronously);
                }, func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default).Unwrap();
        }

        #endregion

        #region ThenTryAsync

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this ResultEx<TValue> result, Func<TValue, Task<TResult>> func)
        {
            if (result.IsError)
                return Task.FromResult(ResultEx<TResult>.FromError(result.Error));

            try
            {
                return func(result.Value)
                    .ContinueWith(task => task.IsFaulted
                            ? ResultEx<TResult>.FromError(task.Exception.TryFlattenAggregateException())
                            : ResultEx<TResult>.FromValue(
                                task.Result), CancellationToken.None,
                        TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }
            catch (Exception e)
            {
                var exception = (e as AggregateException)?.GetBaseException() ?? e;
                return Task.FromResult(ResultEx<TResult>.FromError(exception));
            }
        }

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this ResultEx<TValue> result,
            Func<TValue, Task<ResultEx<TResult>>> func)
        {
            if (result.IsError)
                return Task.FromResult(ResultEx<TResult>.FromError(result.Error));

            try
            {
                return func(result.Value).ContinueWith(task => task.Result,
                    TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously);
            }
            catch (Exception e)
            {
                return Task.FromResult(ResultEx<TResult>.FromError(e));
            }
        }

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this Task<ResultEx<TValue>> resultTask,
            Func<TValue, TResult> func,
            TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) => task.Result.ThenTry((Func<TValue, TResult>) f), func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default);
        }

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this Task<ResultEx<TValue>> resultTask,
            Func<TValue, Task<TResult>> func, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    if (result.IsError)
                        return Task.FromResult(ResultEx<TResult>.FromError(result.Error));

                    try
                    {
                        return ((Func<TValue, Task<TResult>>) f)(task.Result.Value)
                            .ContinueWith(t => ResultEx.FromValue(t.Result), TaskContinuationOptions.ExecuteSynchronously);
                    }
                    catch (Exception e)
                    {
                        return Task.FromResult(ResultEx<TResult>.FromError(e));
                    }
                }, func, CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default).Unwrap();
        }

        #endregion

        public static Task<ResultEx<TValue>> SelectIfErrorAsync<TValue>(this Task<ResultEx<TValue>> resultTask,
            Func<string, string> errorSelector, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, selector) => task.Result.SelectIfError((Func<Exception, Exception>) selector),
                errorSelector, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task<Option<T>> ToOptionAsync<T>(this Task<ResultEx<T>> resultTask)
        {
            return resultTask.ContinueWith(task =>
            {
                var result = task.Result;
                return !result.IsError ? result.Value.ToSome() : Option<T>.None;
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public static Task<ResultEx<T>> Where<T>(this Task<ResultEx<T>> resultTask, Func<T, bool> predicate,
            string errorMessage, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    return result.IsError ? ResultEx<T>.FromError(result.Error) : result.Where((Func<T, bool>) f, errorMessage);
                }, predicate, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task<ResultEx<T>> Where<T>(this Task<ResultEx<T>> resultTask, Func<T, bool> predicate,
            Func<T, string> errorMessageFunc, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    return result.IsError ? ResultEx<T>.FromError(result.Error) : result.Where((Func<T, bool>) f, errorMessageFunc);
                }, predicate, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task IfValue<T>(this Task<ResultEx<T>> resultTask, Action<T> action, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, a) => task.Result.IfValue((Action<T>) a),
                action, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task IfError<T>(this Task<ResultEx<T>> resultTask, Action<Exception> action, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, a) => task.Result.IfError((Action<Exception>) a),
                action, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task<Result<T>> ToResultAsync<T>(this Task<ResultEx<T>> resultTask, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task) => task.Result.ToResult(), scheduler ?? TaskScheduler.Default);
        }

        public static Task<Result<T>> ToResultAsync<T>(this Task<ResultEx<T>> resultTask, Func<Exception, string> errorSelector,
            TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) => task.Result.ToResult((Func<Exception, string>) f), errorSelector,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }
    }
}