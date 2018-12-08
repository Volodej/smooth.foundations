using Smooth.Compare;
using Smooth.Events;
using UnityEngine;

namespace Smooth.Unity
{
    public static class ComparerRegister
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
	        SmoothLogger.RegisterProvider(new UnityLoggingProvider());
	        
            #region Common structs without type specific equality and/or hashcode methods
			
            Finder.Register<Color32>((a, b) => Color32ToInt(a) == Color32ToInt(b), Color32ToInt);
			
            #endregion
            				
            #region UnityEngine structs
				
            //
            // Note: UnityEngine structs do not adhere to the contract of equality.
            //
            // Thus they should not be used as Dictionary keys or in other use cases that rely on a correct equality implementation.
            //
				
            Finder.Register<Color>((a, b) => a == b);
				
            Finder.Register<Vector2>((a, b) => a == b);
            Finder.Register<Vector3>((a, b) => a == b);
            Finder.Register<Vector4>((a, b) => a == b);
				
            Finder.Register<Quaternion>((a, b) => a == b);
				
            #endregion
				
            #region UnityEngine enums
				
            Finder.RegisterEnum<AudioSpeakerMode>();
            Finder.RegisterEnum<EventModifiers>();
            Finder.RegisterEnum<UnityEngine.EventType>();
            Finder.RegisterEnum<KeyCode>();
            Finder.RegisterEnum<PrimitiveType>();
            Finder.RegisterEnum<RuntimePlatform>();

            #endregion
        }
        
        /// <summary>
        /// Converts a 32-bit color to a 32-bit integer without loss of information
        /// </summary>
        public static int Color32ToInt(Color32 c) {
            return (c.r << 24) | (c.g << 16) | (c.b << 8) | c.a;
        }
    }
}