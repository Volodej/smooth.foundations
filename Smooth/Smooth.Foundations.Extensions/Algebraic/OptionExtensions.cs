using System;
using System.Globalization;
using System.Threading.Tasks;
using Smooth.Algebraics;
using Smooth.Foundations.Algebraics;

namespace Smooth.Extensions.Algebraic
{
    public static class OptionExtensions
    {
        public static Option<int> TryParseInt(this string number)
            => number.TryParseIntInternal(NumberStyles.Integer);

        public static Option<int> TryParseHex(this string number)
            => number.TryParseIntInternal(NumberStyles.HexNumber);

        public static Option<float> TryParseFloat(this string number)
            => number.TryParseFloatInternal(NumberStyles.Any);

        public static Option<double> TryParseDouble(this string number)
            => number.TryParseDoubleInternal(NumberStyles.Any);

        public static Option<Action> SelectDelegate<T>(this Option<T> option, Action<T> action)
        {
            if(option.isNone)
                return Option<Action>.None;

            Action result = () => action(option.value);
            return Option.Create(result);
        }

        public static async Task<Option<T>> ToAsync<T>(this Option<Task<T>> option)
        {
            if (option.isNone)
                return Option<T>.None;

            var res = await option.value;
            return res.ToOption();
        }

        public static Option<T> ToOption<T>(this T? value) where T : struct
            => value?.ToSome() ?? Option<T>.None;

        private static Option<int> TryParseIntInternal(this string number, NumberStyles numberStyle)
        {
            int value;
            return int.TryParse(number, numberStyle, CultureInfo.InvariantCulture, out value)
                ? value.ToSome()
                : Option<int>.None;
        }

        private static Option<float> TryParseFloatInternal(this string number, NumberStyles numberStyle)
        {
            float value;
            return float.TryParse(number, numberStyle, CultureInfo.InvariantCulture, out value)
                ? value.ToSome()
                : Option<float>.None;
        }

        private static Option<double> TryParseDoubleInternal(this string number, NumberStyles numberStyle)
        {
            double value;
            return double.TryParse(number, numberStyle, CultureInfo.InvariantCulture, out value)
                ? value.ToSome()
                : Option<double>.None;
        }

        public static ValueOrError<Option<T>> SwapWithValueOrError<T>(this Option<ValueOrError<T>> option)
        {
            return option.isNone ? ValueOrError<Option<T>>.FromValue(Option<T>.None) : option.value.ContinueWith(o => o.ToSome());
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
            if(option.isNone)
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
    }
}
