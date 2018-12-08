using System;
using Smooth.Events;
using UnityEngine;

namespace Smooth.Unity
{
    public class UnityLoggingProvider : ILoggingProvider
    {
        public void Log(string message) => Debug.Log(message);
        public void LogWarning(string message)=> Debug.LogWarning(message);
        public void LogError(string message)=> Debug.LogError(message);
        public void LogError(Exception e)=> Debug.LogError(e);
    }
}