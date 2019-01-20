using System;
using Smooth.Algebraics;

namespace Smooth.Slinq.Context
{
    public struct FuncContext<T>
    {
        #region Slinqs

        public static Slinq<T, FuncContext<T>> Sequence(T seed, Func<T, T> selector)
        {
            return new Slinq<T, FuncContext<T>>(
                skip,
                remove,
                dispose,
                new FuncContext<T>(seed, selector));
        }

        #endregion

        #region Context

        private bool needsMove;
        private T acc;
        private readonly Func<T, T> selector;

#pragma warning disable 0414
        private BacktrackDetector bd;
#pragma warning restore 0414

        private FuncContext(T seed, Func<T, T> selector)
        {
            needsMove = false;
            acc = seed;
            this.selector = selector;

            bd = BacktrackDetector.Borrow();
        }

        #endregion

        #region Delegates

        private static readonly Mutator<T, FuncContext<T>> skip = Skip;
        private static readonly Mutator<T, FuncContext<T>> remove = skip;
        private static readonly Mutator<T, FuncContext<T>> dispose = Dispose;

        private static void Skip(ref FuncContext<T> context, out Option<T> next)
        {
            context.bd.DetectBacktrack();

            if (context.needsMove)
                context.acc = context.selector(context.acc);
            else
                context.needsMove = true;
            next = new Option<T>(context.acc);
        }

        private static void Remove(ref FuncContext<T> context, out Option<T> next)
        {
            throw new NotSupportedException();
        }

        private static void Dispose(ref FuncContext<T> context, out Option<T> next)
        {
            next = new Option<T>();
            context.bd.Release();
        }

        #endregion
    }

    public struct FuncContext<T, P>
    {
        #region Slinqs

        public static Slinq<T, FuncContext<T, P>> Sequence(T seed, Func<T, P, T> selector, P parameter)
        {
            return new Slinq<T, FuncContext<T, P>>(
                skip,
                remove,
                dispose,
                new FuncContext<T, P>(seed, selector, parameter));
        }

        #endregion

        #region Context

        private bool needsMove;
        private T acc;
        private readonly Func<T, P, T> selector;
        private readonly P parameter;

#pragma warning disable 0414
        private BacktrackDetector bd;
#pragma warning restore 0414

        private FuncContext(T seed, Func<T, P, T> selector, P parameter)
        {
            needsMove = false;
            acc = seed;
            this.selector = selector;
            this.parameter = parameter;

            bd = BacktrackDetector.Borrow();
        }

        #endregion

        #region Delegates

        private static readonly Mutator<T, FuncContext<T, P>> skip = Skip;
        private static readonly Mutator<T, FuncContext<T, P>> remove = skip;
        private static readonly Mutator<T, FuncContext<T, P>> dispose = Dispose;

        private static void Skip(ref FuncContext<T, P> context, out Option<T> next)
        {
            context.bd.DetectBacktrack();

            if (context.needsMove)
                context.acc = context.selector(context.acc, context.parameter);
            else
                context.needsMove = true;
            next = new Option<T>(context.acc);
        }

        private static void Remove(ref FuncContext<T, P> context, out Option<T> next)
        {
            throw new NotSupportedException();
        }

        private static void Dispose(ref FuncContext<T, P> context, out Option<T> next)
        {
            next = new Option<T>();
            context.bd.Release();
        }

        #endregion
    }
}