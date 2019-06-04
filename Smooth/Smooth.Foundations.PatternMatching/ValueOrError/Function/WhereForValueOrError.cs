using System;

namespace Smooth.Foundations.PatternMatching.ValueOrError.Function
{
    public class WhereForValueOrError<TMatcher, T1, TResult>
    {
        private readonly Func<T1, bool> _expression;
        private readonly Action<Func<T1, bool>, Func<T1, TResult>> _recorder;
        private readonly TMatcher _matcher;

        internal WhereForValueOrError(Func<T1, bool> expression, Action<Func<T1, bool>, Func<T1, TResult>> recorder,
            TMatcher matcher)
        {
            _expression = expression;
            _recorder = recorder;
            _matcher = matcher;
        }

        public TMatcher Return(Func<T1, TResult> action)
        {
            _recorder(_expression, action);
            return _matcher;
        }

        public TMatcher Return(TResult value)
        {
            _recorder(_expression, x => value);
            return _matcher;
        }
    }
}