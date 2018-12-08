using System;

namespace Smooth.Events
{
    public interface ILoggingProvider
    {
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(Exception e);
    }
}