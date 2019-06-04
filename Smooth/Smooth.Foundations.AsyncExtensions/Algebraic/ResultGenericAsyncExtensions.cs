using System;
using System.Threading;
using System.Threading.Tasks;
using Smooth.Algebraics;
using Smooth.Algebraics.Results;
using Smooth.Algebraics.Results.Exceptions;

namespace Smooth.Foundations.AsyncExtensions.Algebraic
{
    public static class ResultGenericAsyncExtensions
    {
        public static Task<ResultGeneric<TValue, TError>> OrAsync<TValue, TError>(this Task<ResultGeneric<TValue, TError>> resultTask, ResultGeneric<TValue, TError> elseResult)
        {
            return resultTask.ContinueWith(task =>
            {
                var result = task.Result;
                return result.IsError ? elseResult : result;
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public static Task<ResultGeneric<TValue, TError>> OrAsync<TValue, TError>(this Task<ResultGeneric<TValue, TError>> resultTask, Func<ResultGeneric<TValue, TError>> elseResultFunc,
            TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    return result.IsError ? ((Func<ResultGeneric<TValue, TError>>) f)() : result;
                }, elseResultFunc, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task<TValue> ValueOrAsync<TValue, TError>(this Task<ResultGeneric<TValue, TError>> resultTask, TValue elseValue)
        {
            return resultTask.ContinueWith(task =>
            {
                var result = task.Result;
                return result.IsError ? elseValue : result.Value;
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public static Task<TValue> ValueOrAsync<TValue, TError>(this Task<ResultGeneric<TValue, TError>> resultTask, Func<TValue> elseValueFunc, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    return result.IsError ? ((Func<TValue>) f)() : result.Value;
                }, elseValueFunc, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        #region ThenAsync

        public static Task<ResultGeneric<TResult, TError>> ThenAsync<TValue, TError, TResult>(this ResultGeneric<TValue, TError> result, Func<TValue, Task<TResult>> func)
        {
            return result.IsError
                ? Task.FromResult(ResultGeneric<TResult, TError>.FromError(result.Error))
                : func(result.Value).ContinueWith(task => ResultGeneric<TResult, TError>.FromValue(task.Result), CancellationToken.None,
                    TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        public static Task<ResultGeneric<TResult, TError>> ThenAsync<TValue, TError, TResult>(this ResultGeneric<TValue, TError> result, Func<TValue, Task<ResultGeneric<TResult, TError>>> func)
        {
            return result.IsError
                ? Task.FromResult(ResultGeneric<TResult, TError>.FromError(result.Error))
                : func(result.Value);
        }

        public static Task<ResultGeneric<TResult, TError>> ThenAsync<TValue, TError, TResult>(this Task<ResultGeneric<TValue, TError>> resultTask, Func<TValue, TResult> func,
            TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) => task.Result.Then((Func<TValue, TResult>) f), func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default);
        }

        public static Task<ResultGeneric<TResult, TError>> ThenAsync<TValue, TError, TResult>(this Task<ResultGeneric<TValue, TError>> resultTask,
            Func<TValue, ResultGeneric<TResult, TError>> func, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) => task.Result.Then((Func<TValue, ResultGeneric<TResult, TError>>) f), func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default);
        }

        public static Task<ResultGeneric<TResult, TError>> ThenAsync<TValue, TError, TResult>(this Task<ResultGeneric<TValue, TError>> resultTask,
            Func<TValue, Task<TResult>> func, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    return task.Result.IsError
                        ? Task.FromResult(ResultGeneric<TResult, TError>.FromError(task.Result.Error))
                        : ((Func<TValue, Task<TResult>>) f)(task.Result.Value)
                        .ContinueWith(t => ResultGeneric<TResult, TError>.FromValue(t.Result), TaskContinuationOptions.ExecuteSynchronously);
                }, func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default).Unwrap();
        }

        public static Task<ResultGeneric<TResult, TError>> ThenAsync<TValue, TError, TResult>(this Task<ResultGeneric<TValue, TError>> resultTask,
            Func<TValue, Task<ResultGeneric<TResult, TError>>> func, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    return task.Result.IsError
                        ? Task.FromResult(ResultGeneric<TResult, TError>.FromError(task.Result.Error))
                        : ((Func<TValue, Task<ResultGeneric<TResult, TError>>>) f)(task.Result.Value)
                        .ContinueWith(t => t.Result, TaskContinuationOptions.ExecuteSynchronously);
                }, func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default).Unwrap();
        }

        #endregion

        #region ThenTryAsync

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TError, TResult>(this ResultGeneric<TValue, TError> result, Func<TValue, Task<TResult>> func)
        {
            if (result.IsError)
                return Task.FromResult(ResultEx<TResult>.FromError(ResultErrorException.FromError(result.Error)));

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

        public static Task<ResultGeneric<TResult, TError>> ThenTryAsync<TValue, TError, TResult>(this ResultGeneric<TValue, TError> result, Func<TValue, Task<TResult>> func,
            Func<Exception, TError> catchFunc)
        {
            if (result.IsError)
                return Task.FromResult(ResultGeneric<TResult, TError>.FromError(result.Error));

            try
            {
                return func(result.Value).ContinueWith(task => task.IsFaulted
                        ? ResultGeneric<TResult, TError>.FromError(catchFunc(task.Exception.TryFlattenAggregateException()))
                        : ResultGeneric<TResult, TError>.FromValue(task.Result), CancellationToken.None,
                    TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }
            catch (Exception e)
            {
                return Task.FromResult(ResultGeneric<TResult, TError>.FromError(catchFunc(e)));
            }
        }

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TError, TResult>(this ResultGeneric<TValue, TError> result,
            Func<TValue, Task<ResultGeneric<TResult, TError>>> func)
        {
            if (result.IsError)
                return Task.FromResult(ResultEx<TResult>.FromError(ResultErrorException.FromError(result.Error)));

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

        public static Task<ResultGeneric<TResult, TError>> ThenTryAsync<TValue, TError, TResult>(this ResultGeneric<TValue, TError> result,
            Func<TValue, Task<ResultGeneric<TResult, TError>>> func,
            Func<Exception, TError> catchFunc)
        {
            if (result.IsError)
                return Task.FromResult(ResultGeneric<TResult, TError>.FromError(result.Error));

            try
            {
                return func(result.Value);
            }
            catch (Exception e)
            {
                return Task.FromResult(ResultGeneric<TResult, TError>.FromError(catchFunc(e)));
            }
        }

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TError, TResult>(this Task<ResultGeneric<TValue, TError>> resultTask,
            Func<TValue, TResult> func,
            TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) => task.Result.ThenTry((Func<TValue, TResult>) f), func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default);
        }

        public static Task<ResultGeneric<TResult, TError>> ThenTryAsync<TValue, TError, TResult>(this Task<ResultGeneric<TValue, TError>> resultTask, Func<TValue, TResult> func,
            Func<Exception, TError> catchFunc, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) => task.Result.ThenTry((Func<TValue, TResult>) f, catchFunc), func,
                CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default);
        }

        public static Task<ResultEx<TResult>> ThenTryAsync<TValue, TError, TResult>(this Task<ResultGeneric<TValue, TError>> resultTask,
            Func<TValue, Task<TResult>> func, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    if (result.IsError)
                        return Task.FromResult(ResultEx<TResult>.FromError(ResultErrorException.FromError(result.Error)));

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

        public static Task<ResultGeneric<TResult, TError>> ThenTryAsync<TValue, TError, TResult>(this Task<ResultGeneric<TValue, TError>> resultTask,
            Func<TValue, Task<TResult>> func, Func<Exception, TError> catchFunc, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    if (task.Result.IsError)
                        return Task.FromResult(ResultGeneric<TResult, TError>.FromError(task.Result.Error));

                    try
                    {
                        return ((Func<TValue, Task<TResult>>) f)(task.Result.Value)
                            .ContinueWith(t => ResultGeneric<TResult, TError>.FromValue(t.Result), TaskContinuationOptions.ExecuteSynchronously);
                    }
                    catch (Exception e)
                    {
                        return Task.FromResult(ResultGeneric<TResult, TError>.FromError(catchFunc(e)));
                    }
                }, func, CancellationToken.None, TaskContinuationOptions.DenyChildAttach,
                scheduler ?? TaskScheduler.Default).Unwrap();
        }

        #endregion

        public static Task<Option<TValue>> ToOptionAsync<TValue, TError>(this Task<ResultGeneric<TValue, TError>> resultTask)
        {
            return resultTask.ContinueWith(task =>
            {
                var result = task.Result;
                return !result.IsError ? result.Value.ToSome() : Option<TValue>.None;
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        public static Task<ResultGeneric<TValue, TError>> Where<TValue, TError>(this Task<ResultGeneric<TValue, TError>> resultTask, Func<TValue, bool> predicate,
            TError errorValue, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    return result.IsError ? ResultGeneric<TValue, TError>.FromError(result.Error) : result.Where((Func<TValue, bool>) f, errorValue);
                }, predicate, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task<ResultGeneric<TValue, TError>> Where<TValue, TError>(this Task<ResultGeneric<TValue, TError>> resultTask, Func<TValue, bool> predicate,
            Func<TValue, TError> errorValueFunc, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, f) =>
                {
                    var result = task.Result;
                    return result.IsError ? ResultGeneric<TValue, TError>.FromError(result.Error) : result.Where((Func<TValue, bool>) f, errorValueFunc);
                }, predicate, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task IfValue<TValue, TError>(this Task<ResultGeneric<TValue, TError>> resultTask, Action<TValue> action, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, a) => task.Result.IfValue((Action<TValue>) a),
                action, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }

        public static Task IfError<TValue, TError>(this Task<ResultGeneric<TValue, TError>> resultTask, Action<TError> action, TaskScheduler scheduler = null)
        {
            return resultTask.ContinueWith((task, a) => task.Result.IfError((Action<TError>) a),
                action, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, scheduler ?? TaskScheduler.Default);
        }
    }
}