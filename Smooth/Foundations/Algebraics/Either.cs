using System;
using Smooth.Collections;

namespace Smooth.Algebraics
{
	/// <summary>
	///     Struct representing a value that can be an instance of either the L (left) or the R (right) type.
	/// </summary>
	[Serializable]
    public struct Either<L, R> : IComparable<Either<L, R>>, IEquatable<Either<L, R>>
    {
	    /// <summary>
	    ///     True if the either contains an L value; otherwise, false;
	    /// </summary>
	    public readonly bool isLeft;

	    /// <summary>
	    ///     If the either isLeft, the value contained by the either; otherwise, default(L).
	    /// </summary>
	    public readonly L leftValue;

	    /// <summary>
	    ///     If the either isRight, the value contained by the either; otherwise, default(R).
	    /// </summary>
	    public readonly R rightValue;

	    /// <summary>
	    ///     True if the either contains an R value; otherwise, false;
	    /// </summary>
	    public bool isRight => !isLeft;

	    /// <summary>
	    ///     If the either isLeft, the an option containing the either's value; otherwise, an empty option.
	    /// </summary>
	    public Option<L> leftOption => isLeft ? new Option<L>(leftValue) : new Option<L>();

	    /// <summary>
	    ///     If the either isRight, the an option containing the either's value; otherwise, an empty option.
	    /// </summary>
	    public Option<R> rightOption => isLeft ? new Option<R>() : new Option<R>(rightValue);

	    /// <summary>
	    ///     Returns a left either containing the specified value.
	    /// </summary>
	    /// <param name="value">Value.</param>
	    public static Either<L, R> Left(L value)
        {
            return new Either<L, R>(true, value, default);
        }

	    /// <summary>
	    ///     Returns a right either containing the specified value.
	    /// </summary>
	    /// <param name="value">Value.</param>
	    public static Either<L, R> Right(R value)
        {
            return new Either<L, R>(false, default, value);
        }

        private Either(bool isLeft, L leftValue, R rightValue)
        {
            this.isLeft = isLeft;
            this.leftValue = leftValue;
            this.rightValue = rightValue;
        }

        /// <summary>
        ///     If the either isLeft, returns leftFunc applied to the either's value; otherwise, returns rightFunc applied to the either's value.
        /// </summary>
        public V Cata<V>(Func<L, V> leftFunc, Func<R, V> rightFunc)
        {
            return isLeft ? leftFunc(leftValue) : rightFunc(rightValue);
        }

        /// <summary>
        ///     If the either isLeft, returns leftFunc applied to the either's value and p; otherwise, returns rightFunc applied to the either's value.
        /// </summary>
        public V Cata<V, P>(Func<L, P, V> leftFunc, P p, Func<R, V> rightFunc)
        {
            return isLeft ? leftFunc(leftValue, p) : rightFunc(rightValue);
        }

        /// <summary>
        ///     If the either isLeft, returns leftFunc applied to the either's value; otherwise, returns rightFunc applied to the either's value and
        ///     p2.
        /// </summary>
        public V Cata<V, P2>(Func<L, V> leftFunc, Func<R, P2, V> rightFunc, P2 p2)
        {
            return isLeft ? leftFunc(leftValue) : rightFunc(rightValue, p2);
        }

        /// <summary>
        ///     If the either isLeft, returns leftFunc applied to the either's value and p; otherwise, returns rightFunc applied to the either's value
        ///     and p2.
        /// </summary>
        public V Cata<V, P, P2>(Func<L, P, V> leftFunc, P p, Func<R, P2, V> rightFunc, P2 p2)
        {
            return isLeft ? leftFunc(leftValue, p) : rightFunc(rightValue, p2);
        }

        /// <summary>
        ///     If the either isLeft, applies leftAction to the either's value; otherwise, applies rightAction to the either's value.
        /// </summary>
        public void ForEach(Action<L> leftAction, Action<R> rightAction)
        {
            if (isLeft) leftAction(leftValue);
            else rightAction(rightValue);
        }

        /// <summary>
        ///     If the either isLeft, applies leftAction to the either's value and p; otherwise, applies rightAction to the either's value.
        /// </summary>
        public void ForEach<P>(Action<L, P> leftAction, P p, Action<R> rightAction)
        {
            if (isLeft) leftAction(leftValue, p);
            else rightAction(rightValue);
        }

        /// <summary>
        ///     If the either isLeft, applies leftAction to the either's value; otherwise, applies rightAction to the either's value and p2.
        /// </summary>
        public void ForEach<P2>(Action<L> leftAction, Action<R, P2> rightAction, P2 p2)
        {
            if (isLeft) leftAction(leftValue);
            else rightAction(rightValue, p2);
        }

        /// <summary>
        ///     If the either isLeft, applies leftAction to the either's value and p; otherwise, applies rightAction to the either's value and p2.
        /// </summary>
        public void ForEach<P, P2>(Action<L, P> leftAction, P p, Action<R, P2> rightAction, P2 p2)
        {
            if (isLeft) leftAction(leftValue, p);
            else rightAction(rightValue, p2);
        }
        
        /// <summary>
        ///     Returns new Either with applied selector function if value exist.
        /// </summary>
        public Either<LR, RR> Select<LR, RR>(Func<L, LR> firstSelector, Func<R, RR> secondSelector)
        {
            return isLeft 
                ? Either<LR, RR>.Left(firstSelector(leftValue)) 
                : Either<LR, RR>.Right(secondSelector(rightValue));
        }

        public override bool Equals(object o)
        {
            return o is Either<L, R> && Equals((Either<L, R>) o);
        }

        public bool Equals(Either<L, R> other)
        {
            return isLeft
                ? other.isLeft && EqualityComparer<L>.Default.Equals(leftValue, other.leftValue)
                : !other.isLeft && EqualityComparer<R>.Default.Equals(rightValue, other.rightValue);
        }

        public override int GetHashCode()
        {
            return isLeft ? EqualityComparer<L>.Default.GetHashCode(leftValue) : EqualityComparer<R>.Default.GetHashCode(rightValue);
        }

        public int CompareTo(Either<L, R> other)
        {
            return isLeft ? other.isLeft ? Comparer<L>.Default.Compare(leftValue, other.leftValue) : -1 :
                other.isLeft ? 1 : Comparer<R>.Default.Compare(rightValue, other.rightValue);
        }

        public static bool operator ==(Either<L, R> lhs, Either<L, R> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Either<L, R> lhs, Either<L, R> rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static bool operator >(Either<L, R> lhs, Either<L, R> rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public static bool operator <(Either<L, R> lhs, Either<L, R> rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator >=(Either<L, R> lhs, Either<L, R> rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        public static bool operator <=(Either<L, R> lhs, Either<L, R> rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }

        public override string ToString()
        {
            return isLeft ? "[Left: " + leftValue + " ]" : "[Right: " + rightValue + " ]";
        }
    }
}