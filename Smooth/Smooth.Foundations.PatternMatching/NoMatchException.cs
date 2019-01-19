using System;

namespace Smooth.Foundations.PatternMatching
{
    public sealed class NoMatchException : Exception
    {
        public NoMatchException(string message) : base(message) { }
    }
}