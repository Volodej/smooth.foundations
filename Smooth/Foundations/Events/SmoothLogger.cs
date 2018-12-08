using System;

namespace Smooth.Events
{
    public static class SmoothLogger
    {
        private static ILoggingProvider _loggingProvider;
        public static void RegisterProvider(ILoggingProvider provider) => _loggingProvider = provider;
        public static void Log(string message) => _loggingProvider?.Log(message);
        public static void LogWarning(string message) => _loggingProvider?.LogWarning(message);
        public static void LogError(string message) => _loggingProvider?.LogError(message);
        public static void LogError(Exception e) => _loggingProvider?.LogError(e);
    }
}