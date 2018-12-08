using System;
using System.Collections.Generic;
using Smooth.Algebraics.Extensions;
using Smooth.Delegates;
using Smooth.Slinq;

namespace Smooth.Foundations.PatternMatching
{
    public sealed class WithForFuncHandler<TMatcher, T1, TResult>
    {
        private readonly List<T1> _values;
        private readonly Action<DelegateFunc<T1, bool>, DelegateFunc<T1, TResult>> _recorder;
        private readonly TMatcher _matcher;

        internal WithForFuncHandler(T1 value,
                                    Action<DelegateFunc<T1, bool>, DelegateFunc<T1, TResult>> recorder,
                                    TMatcher matcher)
        {
            _values = new List<T1> { value };
            _recorder = recorder;
            _matcher = matcher;
        }

        public WithForFuncHandler<TMatcher, T1, TResult> Or(T1 value)
        {
            _values.Add(value);
            return this;
        }

        [Obsolete("Please use return")]
        public TMatcher Do(TResult result) => Return(result);

        [Obsolete("Please use return")]
        public TMatcher Do(DelegateFunc<T1, TResult> func) => Return(func);

        public TMatcher Return(DelegateFunc<T1, TResult> action)
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

    public sealed class WithForFuncHandler<TMatcher, T1, T2, TResult>
    {
        private readonly List<ValueTuple<T1, T2>> _values;
        private readonly Action<DelegateFunc<T1, T2, bool>, DelegateFunc<T1, T2, TResult>> _recorder;
        private readonly TMatcher _matcher;

        internal WithForFuncHandler(ValueTuple<T1, T2> value,
                                    Action<DelegateFunc<T1, T2, bool>, DelegateFunc<T1, T2, TResult>> recorder,
                                    TMatcher matcher)
        {
            _values = new List<ValueTuple<T1, T2>>
            {
                value
            };
            _recorder = recorder;
            _matcher = matcher;
        }

        public WithForFuncHandler<TMatcher, T1, T2, TResult> Or(T1 value1, T2 value2)
        {
            _values.Add((value1, value2));
            return this;
        }

        [Obsolete("Please use return")]
        public TMatcher Do(TResult result) => Return(result);

        [Obsolete("Please use return")]
        public TMatcher Do(DelegateFunc<T1, T2, TResult> func) => Return(func);

        public TMatcher Return(DelegateFunc<T1, T2, TResult> action)
        {
            _recorder((x, y) =>
                _values
                .Slinq()
                .Any((t1, t2) => t1.ItemsEquals(t2), (x, y)), action);
            return _matcher;
        }

        public TMatcher Return(TResult value)
        {
            _recorder((x, y) =>
                _values
                .Slinq()
                .Any((t1, t2) => t1.ItemsEquals(t2), (x, y)), (_, __) => value);
            return _matcher;
        }
    }

    public sealed class WithForFuncHandler<TMatcher, T1, T2, T3, TResult>
    {
        private readonly List<ValueTuple<T1, T2, T3>> _values;
        private readonly Action<DelegateFunc<T1, T2, T3, bool>, DelegateFunc<T1, T2, T3, TResult>> _recorder;
        private readonly TMatcher _matcher;

        internal WithForFuncHandler(ValueTuple<T1, T2, T3> value,
                                    Action<DelegateFunc<T1, T2, T3, bool>, DelegateFunc<T1, T2, T3, TResult>> recorder,
                                    TMatcher matcher)
        {
            _values = new List<ValueTuple<T1, T2, T3>>
            {
                value
            };
            _recorder = recorder;
            _matcher = matcher;
        }

        public WithForFuncHandler<TMatcher, T1, T2, T3, TResult> Or(T1 value1, T2 value2, T3 value3)
        {
            _values.Add((value1, value2, value3));
            return this;
        }

        [Obsolete("Please use return")]
        public TMatcher Do(TResult result) => Return(result);

        [Obsolete("Please use return")]
        public TMatcher Do(DelegateFunc<T1, T2, T3, TResult> func) => Return(func);

        public TMatcher Return(DelegateFunc<T1, T2, T3, TResult> action)
        {
            _recorder((x, y, z) =>
                _values
                .Slinq()
                .Any((t1, t2) => t1.ItemsEquals(t2), (x, y, z)), action);
            return _matcher;
        }

        public TMatcher Return(TResult value)
        {
            _recorder((x, y, z) =>
                _values
                .Slinq()
                .Any((t1, t2) => t1.ItemsEquals(t2), (x, y, z)), (_, __, ___) => value);
            return _matcher;
        }
    }

    public sealed class WithForFuncHandler<TMatcher, T1, T2, T3, T4, TResult>
    {
        private readonly List<ValueTuple<T1, T2, T3, T4>> _values;
        private readonly Action<DelegateFunc<T1, T2, T3, T4, bool>, DelegateFunc<T1, T2, T3, T4, TResult>> _recorder;
        private readonly TMatcher _matcher;

        internal WithForFuncHandler(ValueTuple<T1, T2, T3, T4> value,
                                    Action<DelegateFunc<T1, T2, T3, T4, bool>, DelegateFunc<T1, T2, T3, T4, TResult>> recorder,
                                    TMatcher matcher)
        {
            _values = new List<ValueTuple<T1, T2, T3, T4>>
            {
                value
            };
            _recorder = recorder;
            _matcher = matcher;
        }

        public WithForFuncHandler<TMatcher, T1, T2, T3, T4, TResult> Or(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            _values.Add((value1, value2, value3, value4));
            return this;
        }

        [Obsolete("Please use return")]
        public TMatcher Do(TResult result) => Return(result);

        [Obsolete("Please use return")]
        public TMatcher Do(DelegateFunc<T1, T2, T3, T4, TResult> func) => Return(func);

        public TMatcher Return(DelegateFunc<T1, T2, T3, T4, TResult> action)
        {
            _recorder((w, x, y, z) =>
                _values
                .Slinq()
                .Any((t1, t2) => t1.ItemsEquals(t2), (w, x, y, z)), action);
            return _matcher;
        }

        public TMatcher Return(TResult value)
        {
            _recorder((w, x, y, z) =>
                _values
                .Slinq()
                .Any((t1, t2) => t1.ItemsEquals(t2), (w, x, y, z)), (p1, p2, p3, p4) => value);
            return _matcher;
        }
    }

}