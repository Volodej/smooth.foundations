using System;
using Smooth.Algebraics;

namespace Utils.Algebraic
{
    public static class EitherExtensions
    {
        // TODO: to Either
        public static Either<T3, T4> Select<T1, T2, T3, T4>(this Either<T1, T2> either, Func<T1, T3> firstSelector, Func<T2, T4> secondSelector)
        {
            return either.isLeft 
                ? Either<T3, T4>.Left(firstSelector(either.leftValue)) 
                : Either<T3, T4>.Right(secondSelector(either.rightValue));
        }
    }
}