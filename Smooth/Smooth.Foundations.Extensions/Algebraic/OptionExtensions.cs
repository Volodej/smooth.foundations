using System;
using System.Globalization;
using Smooth.Algebraics;
using Smooth.Algebraics.Results;

namespace Smooth.Extensions.Algebraic
{
    public static class OptionExtensions
    {
        public static Option<int> TryParseInt(this string number)
        {
            return number.TryParseIntInternal(NumberStyles.Integer);
        }

        public static Option<int> TryParseHex(this string number)
        {
            return number.TryParseIntInternal(NumberStyles.HexNumber);
        }

        public static Option<float> TryParseFloat(this string number)
        {
            return number.TryParseFloatInternal(NumberStyles.Any);
        }

        public static Option<double> TryParseDouble(this string number)
        {
            return number.TryParseDoubleInternal(NumberStyles.Any);
        }

        public static Option<Action> SelectDelegate<T>(this Option<T> option, Action<T> action)
        {
            if (option.isNone)
                return Option<Action>.None;

            void GetResult() => action(option.value);
            return Option.Create((Action) GetResult);
        }

        public static Option<T> ToOption<T>(this T? value) where T : struct
        {
            return value?.ToSome() ?? Option<T>.None;
        }

        private static Option<int> TryParseIntInternal(this string number, NumberStyles numberStyle)
        {
            return int.TryParse(number, numberStyle, CultureInfo.InvariantCulture, out var value)
                ? value.ToSome()
                : Option<int>.None;
        }

        private static Option<float> TryParseFloatInternal(this string number, NumberStyles numberStyle)
        {
            return float.TryParse(number, numberStyle, CultureInfo.InvariantCulture, out var value)
                ? value.ToSome()
                : Option<float>.None;
        }

        private static Option<double> TryParseDoubleInternal(this string number, NumberStyles numberStyle)
        {
            return double.TryParse(number, numberStyle, CultureInfo.InvariantCulture, out var value)
                ? value.ToSome()
                : Option<double>.None;
        }

        public static Result<Option<T>> SwapWithResult<T>(this Option<Result<T>> option)
        {
            return option.isNone ? Result<Option<T>>.FromValue(Option<T>.None) : option.value.Then(o => o.ToSome());
        }
    }
}