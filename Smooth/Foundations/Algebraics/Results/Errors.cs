using System;

namespace Smooth.Algebraics.Results
{
    public struct Error
    {
        public readonly string ErrorValue;

        public Error(string errorValue)
        {
            ErrorValue = errorValue;
        }
    }

    public struct ErrorEx
    {
        public readonly Exception ErrorValue;

        public ErrorEx(Exception errorValue)
        {
            ErrorValue = errorValue;
        }
    }

    public struct ErrorGeneric<T>
    {
        public readonly T ErrorValue;

        public ErrorGeneric(T errorValue)
        {
            ErrorValue = errorValue;
        }
    }
}