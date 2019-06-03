using System;
using System.Threading;
using System.Threading.Tasks;
using Smooth.Algebraics.Results;
using Smooth.Foundations.AsyncExtensions.Algebraic;
using Xunit;

namespace Smooth.Foundations.AsyncExtensions.Tests
{
    public class ResultThenAsyncTests
    {
        private const int VALUE = 10;

        [Fact]
        public async void TestThenAsync_Result_TaskValue()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            const string initialErrorMessage = "Initial result error";

            var errorValueTask = CreateInitialResultError(initialErrorMessage).ThenAsync(v => CreateTaskValue(v, VALUE));
            var valueValueTask = CreateInitialResultValue(VALUE).ThenAsync(v => CreateTaskValue(v, VALUE));

            var assertThreadTask1 = AssertThreadEquals(errorValueTask, threadId); // Task for error is already completed
            var assertThreadTask2 = AssertThreadNotEquals(valueValueTask, threadId);

            await Task.WhenAll(assertThreadTask1, assertThreadTask2);

            await AssertError(errorValueTask, initialErrorMessage);
            await AssertValue(valueValueTask, true);
        }

        [Fact]
        public async void TestThenAsync_Result_TaskResult()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            const string initialErrorMessage = "Initial result error";
            const string createdErrorMessage = "Created task result error";

            var errorErrorTask = CreateInitialResultError(initialErrorMessage).ThenAsync(v => CreateErrorTaskResult(createdErrorMessage));
            var errorValueTask = CreateInitialResultError(initialErrorMessage).ThenAsync(v => CreateValueTaskResult(v, VALUE));
            var valueErrorTask = CreateInitialResultValue(VALUE).ThenAsync(v => CreateErrorTaskResult(createdErrorMessage));
            var valueValueTask = CreateInitialResultValue(VALUE).ThenAsync(v => CreateValueTaskResult(v, VALUE));

            var assertThreadTask1 = AssertThreadEquals(errorErrorTask, threadId); // Task for error is already completed
            var assertThreadTask2 = AssertThreadEquals(errorValueTask, threadId); // Task for error is already completed
            var assertThreadTask3 = AssertThreadNotEquals(valueErrorTask, threadId);
            var assertThreadTask4 = AssertThreadNotEquals(valueValueTask, threadId);

            await Task.WhenAll(assertThreadTask1, assertThreadTask2, assertThreadTask3, assertThreadTask4);

            await AssertError(errorErrorTask, initialErrorMessage);
            await AssertError(errorValueTask, initialErrorMessage);
            await AssertError(valueErrorTask, createdErrorMessage);
            await AssertValue(valueValueTask, true);
        }

        // Can't check for Current Context, because running thread from XUnit doesn't have a context with threads synchronization.
        // Thant's why TaskScheduler.FromCurrentSynchronizationContext() couldn't be used in this test.
        // But it could be used in Unity from main thread or from UI thread.
        [Fact]
        public async void TestThenAsync_TaskResult_Value_DefaultContext()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            const string initialErrorMessage = "Initial result error";

            var errorValueTask = CreateErrorTaskInitial(initialErrorMessage).ThenAsync(v => v == VALUE);
            var valueValueTask = CreateValueTaskInitial(VALUE).ThenAsync(v => v == VALUE);

            var assertThreadTask1 = AssertThreadNotEquals(errorValueTask, threadId);
            var assertThreadTask2 = AssertThreadNotEquals(valueValueTask, threadId);

            await Task.WhenAll(assertThreadTask1, assertThreadTask2);

            await AssertError(errorValueTask, initialErrorMessage);
            await AssertValue(valueValueTask, true);
        }

        [Fact]
        public async void TestThenAsync_TaskResult_Result_DefaultContext()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            const string initialErrorMessage = "Initial task result error";
            const string createdErrorMessage = "Created result error";

            var errorErrorTask = CreateErrorTaskInitial(initialErrorMessage).ThenAsync(v => Result<bool>.FromError(createdErrorMessage));
            var errorValueTask = CreateErrorTaskInitial(initialErrorMessage).ThenAsync(v => Result.FromValue(v == VALUE));
            var valueErrorTask = CreateValueTaskInitial(VALUE).ThenAsync(v => Result<bool>.FromError(createdErrorMessage));
            var valueValueTask = CreateValueTaskInitial(VALUE).ThenAsync(v => Result.FromValue(v == VALUE));

            var assertThreadTask1 = AssertThreadNotEquals(errorErrorTask, threadId);
            var assertThreadTask2 = AssertThreadNotEquals(errorValueTask, threadId);
            var assertThreadTask3 = AssertThreadNotEquals(valueErrorTask, threadId);
            var assertThreadTask4 = AssertThreadNotEquals(valueValueTask, threadId);

            await Task.WhenAll(assertThreadTask1, assertThreadTask2, assertThreadTask3, assertThreadTask4);

            await AssertError(errorErrorTask, initialErrorMessage);
            await AssertError(errorValueTask, initialErrorMessage);
            await AssertError(valueErrorTask, createdErrorMessage);
            await AssertValue(valueValueTask, true);
        }

        [Fact]
        public async void TestThenAsync_TaskResult_TaskValue_DefaultContext()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            const string initialErrorMessage = "Initial result error";

            var errorValueTask = CreateErrorTaskInitial(initialErrorMessage)
                .ThenAsync(v => CreateTaskValue(v, VALUE));
            var valueValueTask = CreateValueTaskInitial(VALUE)
                .ThenAsync(v => CreateTaskValue(v, VALUE));

            var assertThreadTask1 = AssertThreadNotEquals(errorValueTask, threadId);
            var assertThreadTask2 = AssertThreadNotEquals(valueValueTask, threadId);

            await Task.WhenAll(assertThreadTask1, assertThreadTask2);

            await AssertError(errorValueTask, initialErrorMessage);
            await AssertValue(valueValueTask, true);
        }

        [Fact]
        public async void TestThenAsync_TaskResult_TaskResult_DefaultContext()
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            const string initialErrorMessage = "Initial result error";
            const string createdErrorMessage = "Created task result error";

            var errorErrorTask = CreateErrorTaskInitial(initialErrorMessage).ThenAsync(v => CreateErrorTaskResult(createdErrorMessage));
            var errorValueTask = CreateErrorTaskInitial(initialErrorMessage).ThenAsync(v => CreateValueTaskResult(v, VALUE));
            var valueErrorTask = CreateValueTaskInitial(VALUE).ThenAsync(v => CreateErrorTaskResult(createdErrorMessage));
            var valueValueTask = CreateValueTaskInitial(VALUE).ThenAsync(v => CreateValueTaskResult(v, VALUE));

            var assertThreadTask1 = AssertThreadNotEquals(errorErrorTask, threadId);
            var assertThreadTask2 = AssertThreadNotEquals(errorValueTask, threadId);
            var assertThreadTask3 = AssertThreadNotEquals(valueErrorTask, threadId);
            var assertThreadTask4 = AssertThreadNotEquals(valueValueTask, threadId);

            await Task.WhenAll(assertThreadTask1, assertThreadTask2, assertThreadTask3, assertThreadTask4);

            await AssertError(errorErrorTask, initialErrorMessage);
            await AssertError(errorValueTask, initialErrorMessage);
            await AssertError(valueErrorTask, createdErrorMessage);
            await AssertValue(valueValueTask, true);
        }

        [Fact]
        public async void TestThenAsync_HandleException()
        {
            Func<Result<bool>> throwFunc = () => throw new InvalidOperationException();
            Func<int, Result<bool>> throwFuncWithInt = _ => throw new InvalidOperationException();

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                CreateInitialResultValue(VALUE).ThenAsync(v => Task.Run(throwFunc)));
            
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                CreateValueTaskInitial(VALUE).ThenAsync(throwFuncWithInt));
        }

        [Fact]
        public async void TestThenTryAsync_Result_ExceptionFunction()
        {
            var resultEx = await CreateInitialResultValue(VALUE).ThenTryAsync(v =>
            {
                throw new InvalidOperationException();
                return Task.FromResult(true);
            });
            
            Assert.True(resultEx.IsError);
            Assert.IsType<InvalidOperationException>(resultEx.Error);
        }

        [Fact]
        public async void TestThenTryAsync_Result_ExceptionInFunction()
        {
            var resultEx = await CreateInitialResultValue(VALUE).ThenTryAsync(v => Task.Run(() =>
            {
                Thread.Sleep(10);
                throw new InvalidOperationException();
                return Task.FromResult(true);
            }));
            
            Assert.True(resultEx.IsError);
            Assert.IsType<InvalidOperationException>(resultEx.Error);
        }

        #region Helpers

        private Task AssertThreadEquals<T>(Task<T> task, int threadId)
        {
            return task.ContinueWith(t =>
            {
                Assert.Equal(threadId, Thread.CurrentThread.ManagedThreadId);
                return t.Result;
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        private Task AssertThreadNotEquals<T>(Task<T> task, int threadId)
        {
            Assert.False(task.IsCompleted);
            return task.ContinueWith(t =>
            {
                Assert.NotEqual(threadId, Thread.CurrentThread.ManagedThreadId);
                return t.Result;
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        private async Task AssertError<T>(Task<Result<T>> task, string expectedMessage)
        {
            var result = await task;
            Assert.True(result.IsError);
            Assert.Equal(expectedMessage, result.Error);
        }

        private async Task AssertValue<T>(Task<Result<T>> task, T expectedResult)
        {
            var result = await task;
            Assert.True(!result.IsError);
            Assert.Equal(expectedResult, result.Value);
        }

        private Result<int> CreateInitialResultValue(int value) => Result.FromValue(value);
        private Result<int> CreateInitialResultError(string errorMessage) => Result<int>.FromError(errorMessage);

        private Task<bool> CreateTaskValue(int value, int compareWith) => Task.Run(() =>
        {
            Thread.Sleep(10);
            return value == compareWith;
        });

        private Task<Result<int>> CreateErrorTaskInitial(string errorMessage) => Task.Run(() =>
        {
            Thread.Sleep(10);
            return Result<int>.FromError(errorMessage);
        });

        private Task<Result<int>> CreateValueTaskInitial(int value) => Task.Run(() =>
        {
            Thread.Sleep(10);
            return Result.FromValue(value);
        });

        private Task<Result<bool>> CreateErrorTaskResult(string errorMessage) => Task.Run(() =>
        {
            Thread.Sleep(10);
            return Result<bool>.FromError(errorMessage);
        });

        private Task<Result<bool>> CreateValueTaskResult(int value, int compareWith) => Task.Run(() =>
        {
            Thread.Sleep(10);
            return Result.FromValue(value == compareWith);
        });

        #endregion
    }
}