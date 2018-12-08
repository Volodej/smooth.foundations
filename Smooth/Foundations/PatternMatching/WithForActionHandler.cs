using System;
using System.Collections.Generic;
using Smooth.Algebraics.Extensions;
using Smooth.Delegates;
using Smooth.Slinq;

namespace Smooth.Foundations.PatternMatching
{
    public sealed class WithForActionHandler<TMatcher, T1>
    {
        private readonly List<T1> _values;
        private readonly Action<Func<T1, bool>, Action<T1>> _recorder;
        private readonly TMatcher _matcher;

        internal WithForActionHandler(T1 value, Action<Func<T1, bool>, Action<T1>> recorder,
                                      TMatcher matcher)
        {
            _values = new List<T1> { value };
            _recorder = recorder;
            _matcher = matcher;
        }

        public WithForActionHandler<TMatcher, T1> Or(T1 value)
        {
            _values.Add(value);
            return this;
        }

        public TMatcher Do(Action<T1> action)
        {
            _recorder(x => _values
                .Slinq()
                .Any((y, xp) => Collections.EqualityComparer<T1>.Default.Equals(xp, y), x), action);
            return _matcher;
        }
    }

    public sealed class WithForActionHandler<TMatcher, T1, T2>
    {
        private readonly List<ValueTuple<T1, T2>> _values;
        private readonly Action<Func<T1, T2, bool>, Action<T1, T2>> _recorder;
        private readonly TMatcher _matcher;

        internal WithForActionHandler(ValueTuple<T1, T2> value, Action<Func<T1, T2, bool>, Action<T1, T2>> recorder,
                                      TMatcher matcher)
        {
            _values = new List<ValueTuple<T1, T2>> { value };
            _recorder = recorder;
            _matcher = matcher;
        }

        public WithForActionHandler<TMatcher, T1, T2> Or(T1 value1, T2 value2)
        {
            _values.Add((value1, value2));
            return this;
        }

        public TMatcher Do(Action<T1, T2> action)
        {
            _recorder((x, y) => _values
                .Slinq()
                .Any((t1, t2) => t1.ItemsEquals(t2), (x, y)), action);
            return _matcher;
        }
    }

    public sealed class WithForActionHandler<TMatcher, T1, T2, T3>
    {
        private readonly List<ValueTuple<T1, T2, T3>> _values;
        private readonly Action<Func<T1, T2, T3, bool>, Action<T1, T2, T3>> _recorder;
        private readonly TMatcher _matcher;

        internal WithForActionHandler(ValueTuple<T1, T2, T3> value, Action<Func<T1, T2, T3, bool>, Action<T1, T2, T3>> recorder,
                                      TMatcher matcher)
        {
            _values = new List<ValueTuple<T1, T2, T3>> { value };
            _recorder = recorder;
            _matcher = matcher;
        }

        public WithForActionHandler<TMatcher, T1, T2, T3> Or(T1 value1, T2 value2, T3 value3)
        {
            _values.Add((value1, value2, value3));
            return this;
        }

        public TMatcher Do(Action<T1, T2, T3> action)
        {
            _recorder((x, y, z) => _values
                .Slinq()
                .Any((t1, t2) => t1.ItemsEquals(t2), (x, y, z)), action);
            return _matcher;
        }
    }

    public sealed class WithForActionHandler<TMatcher, T1, T2, T3, T4>
    {
        private readonly List<ValueTuple<T1, T2, T3, T4>> _values;
        private readonly Action<Func<T1, T2, T3, T4, bool>, Action<T1, T2, T3, T4>> _recorder;
        private readonly TMatcher _matcher;

        internal WithForActionHandler(ValueTuple<T1, T2, T3, T4> value, Action<Func<T1, T2, T3, T4, bool>, Action<T1, T2, T3, T4>> recorder,
                                      TMatcher matcher)
        {
            _values = new List<ValueTuple<T1, T2, T3, T4>> { value };
            _recorder = recorder;
            _matcher = matcher;
        }

        public WithForActionHandler<TMatcher, T1, T2, T3, T4> Or(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            _values.Add((value1, value2, value3, value4));
            return this;
        }

        public TMatcher Do(Action<T1, T2, T3, T4> action)
        {

            _recorder((w, x, y, z) =>
                _values
                .Slinq()
                .Any((t1, t2) => t1.ItemsEquals(t2), (w, x, y, z)), action);
            return _matcher;
        }
    }

}