using System;
using Smooth.Delegates;
using Smooth.Events;
using Smooth.Foundations.Algebraics;

namespace Smooth.Algebraics
{
    public struct Union<T1, T2, T3> : IEquatable<Union<T1, T2, T3>>
    {
        public T1 Case1
        {
            get
            {
                if (Case != Variant.First)
                {
                    SmoothLogger.LogError("Wrong case for union");
                }
                return _value1;
            }
        }

        public T2 Case2
        {
            get
            {
                if (Case != Variant.Second)
                {
                    SmoothLogger.LogError("Wrong case for union");
                }
                return _value2;
            }
        }

        public T3 Case3
        {
            get
            {
                if (Case != Variant.Third)
                {
                    SmoothLogger.LogError("Wrong case for union");
                }
                return _value3;
            }
        }

        public Option<T1> FirstOption => Case == Variant.First ? Option.Some(_value1) : Option<T1>.None;
        public Option<T2> SecondOption => Case == Variant.Second ? Option.Some(_value2) : Option<T2>.None;
        public Option<T3> ThirdOption => Case == Variant.Third ? Option.Some(_value3) : Option<T3>.None;

        public readonly Variant Case;

        private readonly T1 _value1;
        private readonly T2 _value2;
        private readonly T3 _value3;

        public static Union<T1, T2, T3> CreateFirst(T1 value) =>
            new Union<T1, T2, T3>(Variant.First, value, default(T2), default(T3));
        public static Union<T1, T2, T3> CreateSecond(T2 value) =>
            new Union<T1, T2, T3>(Variant.Second, default(T1), value, default(T3));
        public static Union<T1, T2, T3> CreateThird(T3 value) =>
            new Union<T1, T2, T3>(Variant.Third, default(T1), default(T2), value);

        private Union(Variant selectedCase, T1 valueFirst, T2 valueSecond, T3 valueThird)
        {
            Case = selectedCase;
            _value1 = valueFirst;
            _value2 = valueSecond;
            _value3 = valueThird;
        }

        public TResult Cata<TResult>(DelegateFunc<T1, TResult> first, 
                                     DelegateFunc<T2, TResult> second,
                                     DelegateFunc<T3, TResult> third)
        {
            switch (Case)
            {
                case Variant.First:
                    return first(_value1);
                case Variant.Second:
                    return second(_value2);
                case Variant.Third:
                    return third(_value3);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TResult Cata<TResult, P1>(DelegateFunc<T1, P1, TResult> first, P1 param1,
                                         DelegateFunc<T2, TResult> second,
                                         DelegateFunc<T3, TResult> third)
        {
            switch (Case)
            {
                case Variant.First:
                    return first(_value1, param1);
                case Variant.Second:
                    return second(_value2);
                case Variant.Third:
                    return third(_value3);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TResult Cata<TResult, P2>(DelegateFunc<T1, TResult> first,
                                         DelegateFunc<T2, P2, TResult> second, P2 param2,
                                         DelegateFunc<T3, TResult> third)
        {
            switch (Case)
            {
                case Variant.First:
                    return first(_value1);
                case Variant.Second:
                    return second(_value2, param2);
                case Variant.Third:
                    return third(_value3);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TResult Cata<TResult, P3>(DelegateFunc<T1, TResult> first,
                                         DelegateFunc<T2, TResult> second,
                                         DelegateFunc<T3, P3, TResult> third, P3 param3)
        {
            switch (Case)
            {
                case Variant.First:
                    return first(_value1);
                case Variant.Second:
                    return second(_value2);
                case Variant.Third:
                    return third(_value3, param3);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TResult Cata<TResult, P1, P2>(DelegateFunc<T1, P1, TResult> first, P1 param1,
                                             DelegateFunc<T2, P2, TResult> second, P2 param2,
                                             DelegateFunc<T3, TResult> third)
        {
            switch (Case)
            {
                case Variant.First:
                    return first(_value1, param1);
                case Variant.Second:
                    return second(_value2, param2);
                case Variant.Third:
                    return third(_value3);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TResult Cata<TResult, P2, P3>(DelegateFunc<T1, TResult> first,
                                             DelegateFunc<T2, P2, TResult> second, P2 param2,
                                             DelegateFunc<T3, P3, TResult> third, P3 param3)
        {
            switch (Case)
            {
                case Variant.First:
                    return first(_value1);
                case Variant.Second:
                    return second(_value2, param2);
                case Variant.Third:
                    return third(_value3, param3);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TResult Cata<TResult, P1, P3>(DelegateFunc<T1, P1, TResult> first, P1 param1,
                                             DelegateFunc<T2, TResult> second,
                                             DelegateFunc<T3, P3, TResult> third, P3 param3)
        {
            switch (Case)
            {
                case Variant.First:
                    return first(_value1, param1);
                case Variant.Second:
                    return second(_value2);
                case Variant.Third:
                    return third(_value3, param3);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TResult Cata<TResult, P1, P2, P3>(DelegateFunc<T1, P1, TResult> first, P1 param1,
                                                 DelegateFunc<T2, P2, TResult> second, P2 param2,
                                                 DelegateFunc<T3, P3, TResult> third, P3 param3)
        {
            switch (Case)
            {
                case Variant.First:
                    return first(_value1, param1);
                case Variant.Second:
                    return second(_value2, param2);
                case Variant.Third:
                    return third(_value3, param3);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ForEach(DelegateAction<T1> first,
                            DelegateAction<T2> second,
                            DelegateAction<T3> third)
        {
            switch (Case)
            {
                case Variant.First:
                    first(_value1);
                    break;
                case Variant.Second:
                    second(_value2);
                    break;
                case Variant.Third:
                    third(_value3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ForEach<P1>(DelegateAction<T1, P1> first, P1 param1,
                                DelegateAction<T2> second,
                                DelegateAction<T3> third)
        {
            switch (Case)
            {
                case Variant.First:
                    first(_value1, param1);
                    break;
                case Variant.Second:
                    second(_value2);
                    break;
                case Variant.Third:
                    third(_value3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ForEach<P2>(DelegateAction<T1> first,
                                DelegateAction<T2, P2> second, P2 param2,
                                DelegateAction<T3> third)
        {
            switch (Case)
            {
                case Variant.First:
                    first(_value1);
                    break;
                case Variant.Second:
                    second(_value2, param2);
                    break;
                case Variant.Third:
                    third(_value3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ForEach<P3>(DelegateAction<T1> first,
                                DelegateAction<T2> second,
                                DelegateAction<T3, P3> third, P3 param3)
        {
            switch (Case)
            {
                case Variant.First:
                    first(_value1);
                    break;
                case Variant.Second:
                    second(_value2);
                    break;
                case Variant.Third:
                    third(_value3, param3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ForEach<P1, P2>(DelegateAction<T1, P1> first, P1 param1,
                                    DelegateAction<T2, P2> second, P2 param2,
                                    DelegateAction<T3> third)
        {
            switch (Case)
            {
                case Variant.First:
                    first(_value1, param1);
                    break;
                case Variant.Second:
                    second(_value2, param2);
                    break;
                case Variant.Third:
                    third(_value3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ForEach<P2, P3>(DelegateAction<T1> first,
                                    DelegateAction<T2, P2> second, P2 param2,
                                    DelegateAction<T3, P3> third, P3 param3)
        {
            switch (Case)
            {
                case Variant.First:
                    first(_value1);
                    break;
                case Variant.Second:
                    second(_value2, param2);
                    break;
                case Variant.Third:
                    third(_value3, param3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ForEach<P1, P3>(DelegateAction<T1, P1> first, P1 param1,
                                    DelegateAction<T2> second,
                                    DelegateAction<T3, P3> third, P3 param3)
        {
            switch (Case)
            {
                case Variant.First:
                    first(_value1, param1);
                    break;
                case Variant.Second:
                    second(_value2);
                    break;
                case Variant.Third:
                    third(_value3, param3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ForEach<P1, P2, P3>(DelegateAction<T1, P1> first, P1 param1,
                                        DelegateAction<T2, P2> second, P2 param2,
                                        DelegateAction<T3, P3> third, P3 param3)
        {
            switch (Case)
            {
                case Variant.First:
                    first(_value1, param1);
                    break;
                case Variant.Second:
                    second(_value2, param2);
                    break;
                case Variant.Third:
                    third(_value3, param3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool Equals(Union<T1, T2, T3> other)
        {
            if (Case != other.Case)
                return false;
            switch (Case)
            {
                case Variant.First:
                    return Collections.EqualityComparer<T1>.Default.Equals(_value1, other.Case1);
                case Variant.Second:
                    return Collections.EqualityComparer<T2>.Default.Equals(_value2, other.Case2);
                case Variant.Third:
                    return Collections.EqualityComparer<T3>.Default.Equals(_value3, other.Case3);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Union<T1, T2, T3> && Equals((Union<T1, T2, T3>)obj);
        }

        public override int GetHashCode()
        {
            switch (Case)
            {
                case Variant.First:
                    return Collections.EqualityComparer<T1>.Default.GetHashCode(_value1);
                case Variant.Second:
                    return Collections.EqualityComparer<T2>.Default.GetHashCode(_value2);
                case Variant.Third:
                    return Collections.EqualityComparer<T3>.Default.GetHashCode(_value3);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string ToString()
        {
            switch (Case)
            {
                case Variant.First:
                    return $"[First: {_value1}]";
                case Variant.Second:
                    return $"[Second: {_value2}]";
                case Variant.Third:
                    return $"[Third: {_value3}]";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool operator ==(Union<T1, T2, T3> lhs, Union<T1, T2, T3> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Union<T1, T2, T3> lhs, Union<T1, T2, T3> rhs)
        {
            return !(lhs == rhs);
        }
    }
}