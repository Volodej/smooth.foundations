using Smooth.Dispose;
using Smooth.Events;
using UnityEngine;

public class SmoothDisposer : MonoBehaviour
{
    private static SmoothDisposer _instance;

    private void Awake()
    {
        if (_instance)
        {
            SmoothLogger.LogWarning("Only one " + GetType().Name + " should exist at a time, instantiated by the " +
                                    typeof(DisposalQueue).Name + " class.");
            Destroy(this);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void LateUpdate()
    {
        DisposalQueue.Pulse();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        new GameObject(typeof(SmoothDisposer).Name).AddComponent<SmoothDisposer>();
    }
}