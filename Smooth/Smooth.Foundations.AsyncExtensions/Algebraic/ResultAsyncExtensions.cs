using System;
using System.Threading;
using System.Threading.Tasks;
using Smooth.Algebraics;
using Smooth.Algebraics.Results;
using Smooth.Algebraics.Results.Exceptions;
using Smooth.Slinq;

namespace Smooth.Foundations.AsyncExtensions.Algebraic
{
    public static class ResultAsyncExtensions
    {
        public static Task<Result<T>> OrAsync<T>(this Task<Result<T>> resultTask, Result<T> elseResult)
        {
            return resultTask.ContinueWith(task =>
            {
                var result = task.Result;
                return result.IsError ? elseResult : result;
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public static Task<Result<T>> OrAsync<T>(this Task<Result<T>> resultTask, Func<Result<T>> elseResultFunc,
            TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    return result.IsError ? ((Func<Result<T>>) f)() : result;
                }, elseResultFunc, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task<T> ValueOrAsync<T>(this Task<Result<T>> resultTask, T elseValue)
        {
            return resultTask.ContinueWith(task =>
            {
                var result = task.Result;
                return result.IsError ? elseValue : result.Value;
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public static Task<T> ValueOrAsync<T>(this Task<Result<T>> resultTask, Func<T> elseValueFunc, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    return result.IsError ? ((Func<T>) f)() : result.Value;
                }, elseValueFunc, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        #region ThenAsync

        public static Task<Result<TResult>> ThenAsync<TValue, TResult>(this Result<TValue> result, Func<TValue, Task<TResult>> func)
        {
            return result.IsError
                ? Task.FromResult(Result<TResult>.FromError(result.Error))
                : func(result.Value).ContinueWith(task => Result<TResult>.FromValue(task.Result), CancellationToken.None,
                    TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        public static Task<Result<TResult>> ThenAsync<TValue, TResult>(this Result<TValue> result, Func<TValue, Task<Result<TResult>>> func)
        {
            return result.IsError
                ? Task.FromResult(Result<TResult>.FromError(result.Error))
                : func(result.Value);
        }

        public static Task<Result<TResult>> ThenAsync<TValue, TResult>(this Task<Result<TValue>> resultTask, Func<TValue, TResult> func,
            TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) => task.Result.Then((Func<TValue, TResult>) f), func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default);
        }

        public static Task<Result<TResult>> ThenAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, Result<TResult>> func, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) => task.Result.Then((Func<TValue, Result<TResult>>) f), func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default);
        }

        public static Task<Result<TResult>> ThenAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, Task<TResult>> func, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    return task.Result.IsError
                        ? Task.FromResult(Result<TResult>.FromError(task.Result.Error))
                        : ((Func<TValue, Task<TResult>>) f)(task.Result.Value)
                        .ContinueWith(t => Result.FromValue(t.Result), TaskContinuationOptions.ExecuteSynchronously);
                }, func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default).Unwrap();
        }

        public static Task<Result<TResult>> ThenAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, Task<Result<TResult>>> func, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    return task.Result.IsError
                        ? Task.FromResult(Result<TResult>.FromError(task.Result.Error))
                        : ((Func<TValue, Task<Result<TResult>>>) f)(task.Result.Value)
                        .ContinueWith(t => t.Result, TaskContinuationOptions.ExecuteSynchronously);
                }, func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default).Unwrap();
        }

        #endregion

        #region ThenTryAsync

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this Result<TValue> result, Func<TValue, Task<TResult>> func)
        {
            if (result.IsError)
                return Task.FromResult(ResultEx<TResult>.FromError(new ResultErrorException(result.Error)));

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

        public static Task<Result<TResult>> ThenTryAsync<TValue, TResult>(this Result<TValue> result, Func<TValue, Task<TResult>> func,
            Func<Exception, string> catchFunc)
        {
            if (result.IsError)
                return Task.FromResult(Result<TResult>.FromError(result.Error));

            try
            {
                return func(result.Value).ContinueWith(task => task.IsFaulted
                        ? Result<TResult>.FromError(catchFunc(task.Exception.TryFlattenAggregateException()))
                        : Result.FromValue(task.Result), CancellationToken.None,
                    TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }
            catch (Exception e)
            {
                return Task.FromResult(Result<TResult>.FromError(catchFunc(e)));
            }
        }

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this Result<TValue> result,
            Func<TValue, Task<Result<TResult>>> func)
        {
            if (result.IsError)
                return Task.FromResult(ResultEx<TResult>.FromError(new ResultErrorException(result.Error)));

            try
            {
                return func(result.Value).ContinueWith(task => task.Result.ToResultEx(),
                    TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously);
            }
            catch (Exception e)
            {
                return Task.FromResult(ResultEx<TResult>.FromError(e));
            }
        }

        public static Task<Result<TResult>> ThenTryAsync<TValue, TResult>(this Result<TValue> result,
            Func<TValue, Task<Result<TResult>>> func,
            Func<Exception, string> catchFunc)
        {
            if (result.IsError)
                return Task.FromResult(Result<TResult>.FromError(result.Error));

            try
            {
                return func(result.Value);
            }
            catch (Exception e)
            {
                return Task.FromResult(Result<TResult>.FromError(catchFunc(e)));
            }
        }

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, TResult> func,
            TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) => task.Result.ThenTry((Func<TValue, TResult>) f), func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default);
        }

        public static Task<Result<TResult>> ThenTryAsync<TValue, TResult>(this Task<Result<TValue>> resultTask, Func<TValue, TResult> func,
            Func<Exception, string> catchFunc, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) => task.Result.ThenTry((Func<TValue, TResult>) f, catchFunc), func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default);
        }

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, Task<TResult>> func, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    if (result.IsError)
                        return Task.FromResult(ResultEx<TResult>.FromError(new ResultErrorException(result.Error)));

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

        public static Task<Result<TResult>> ThenTryAsync<TValue, TResult>(this Task<Result<TValue>> resultTask,
            Func<TValue, Task<TResult>> func, Func<Exception, string> catchFunc, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    if (task.Result.IsError)
                        return Task.FromResult(Result<TResult>.FromError(task.Result.Error));

                    try
                    {
                        return ((Func<TValue, Task<TResult>>) f)(task.Result.Value)
                            .ContinueWith(t => Result.FromValue(t.Result), TaskContinuationOptions.ExecuteSynchronously);
                    }
                    catch (Exception e)
                    {
                        return Task.FromResult(Result<TResult>.FromError(catchFunc(e)));
                    }
                }, func, CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default).Unwrap();
        }

        #endregion

        public static Task<Result<TValue>> SpecifyErrorAsync<TValue>(this Task<Result<TValue>> resultTask, string error)
        {
            return resultTask.ContinueWith((task, e) => task.Result.SpecifyError((string) e), error,
                TaskContinuationOptions.ExecuteSynchronously);
        }

        public static Task<Result<TValue>> SelectIfErrorAsync<TValue>(this Task<Result<TValue>> resultTask,
            Func<string, string> errorSelector, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, selector) => task.Result.SelectIfError((Func<string, string>) selector),
                errorSelector, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task<Option<T>> ToOptionAsync<T>(this Task<Result<T>> resultTask)
        {
            return resultTask.ContinueWith(task =>
            {
                var result = task.Result;
                return !result.IsError ? result.Value.ToSome() : Option<T>.None;
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public static Task<Result<T>> Where<T>(this Task<Result<T>> resultTask, Func<T, bool> predicate,
            string errorMessage, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    return result.IsError ? Result<T>.FromError(result.Error) : result.Where((Func<T, bool>) f, errorMessage);
                }, predicate, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task<Result<T>> Where<T>(this Task<Result<T>> resultTask, Func<T, bool> predicate,
            Func<T, string> errorMessageFunc, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    return result.IsError ? Result<T>.FromError(result.Error) : result.Where((Func<T, bool>) f, errorMessageFunc);
                }, predicate, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task IfValue<T>(this Task<Result<T>> resultTask, Action<T> action, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, a) => task.Result.IfValue((Action<T>) a),
                action, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task IfError<T>(this Task<Result<T>> resultTask, Action<string> action, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, a) => task.Result.IfError((Action<string>) a),
                action, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        internal static Exception TryFlattenAggregateException(this Exception exception)
        {
            return (exception as AggregateException).ToOption().Select(aggregateException =>
                    aggregateException.InnerExceptions.Slinq()
                        .SingleOrNone()
                        .ValueOr(aggregateException))
                .ValueOr(exception);
        }
    }
}