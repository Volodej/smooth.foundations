using System;
using System.Collections.Generic;
using Smooth.Foundations.Algebraics;
using Smooth.Slinq;

namespace Smooth.Foundations.PatternMatching.ValueOrError.Function
{
    public class WithForValueOrErrorHandler<TMatcher, T1, TResult>
    {
        private readonly List<T1> _values;
        private readonly Action<Func<T1, bool>, Func<T1, TResult>> _recorder;
        private readonly TMatcher _matcher;
        private readonly ValueOrError<T1> _item;

        internal WithForValueOrErrorHandler(T1 value, Action<Func<T1, bool>, Func<T1, TResult>> recorder,
            TMatcher matcher)
        {
            _values = new List<T1> {value};
            _recorder = recorder;
            _matcher = matcher;
        }

        public WithForValueOrErrorHandler<TMatcher, T1, TResult> Or(T1 value)
        {
            if (!_item.IsError)
            {
                _values.Add(value);
            }
            return this;
        }

        [Obsolete("Please use Return")]
        public TMatcher Do(Func<T1, TResult> action) => Return(action);

        [Obsolete("Please use Return")]
        public TMatcher Do(TResult value) => Return(value);

        public TMatcher Return(Func<T1, TResult> action)
        {
            _recorder(x => _values
                .Slinq()
                .Any((y, xp) =>
                    Collections.EqualityComparer<T1>.Default.Equals(y, xp), x), action);
            return _matcher;
        }

        public TMatcher Return(TResult value)
        {
            _recorder(x => _values
                .Slinq()
                .Any((y, xp) =>
                    Collections.EqualityComparer<T1>.Default.Equals(xp, y), x), v => value);
            return _matcher;
        }
    }
}