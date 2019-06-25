using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Input
	{
		private static Gyroscope m_MainGyro;

		private static LocationService locationServiceInstance;

		private static Compass compassInstance;

		public static bool compensateSensors
		{
			get;
			set;
		}

		[Obsolete("isGyroAvailable property is deprecated. Please use SystemInfo.supportsGyroscope instead.")]
		public static bool isGyroAvailable
		{
			get;
		}

		public static Gyroscope gyro
		{
			get
			{
				if (m_MainGyro == null)
				{
					m_MainGyro = new Gyroscope(mainGyroIndex_Internal());
				}
				return m_MainGyro;
			}
		}

		public static Vector3 mousePosition
		{
			get
			{
				INTERNAL_get_mousePosition(out Vector3 value);
				return value;
			}
		}

		public static Vector2 mouseScrollDelta
		{
			get
			{
				INTERNAL_get_mouseScrollDelta(out Vector2 value);
				return value;
			}
		}

		public static bool mousePresent
		{
			get;
		}

		public static bool simulateMouseWithTouches
		{
			get;
			set;
		}

		public static bool anyKey
		{
			get;
		}

		public static bool anyKeyDown
		{
			get;
		}

		public static string inputString
		{
			get;
		}

		public static Vector3 acceleration
		{
			get
			{
				INTERNAL_get_acceleration(out Vector3 value);
				return value;
			}
		}

		public static AccelerationEvent[] accelerationEvents
		{
			get
			{
				int accelerationEventCount = Input.accelerationEventCount;
				AccelerationEvent[] array = new AccelerationEvent[accelerationEventCount];
				for (int i = 0; i < accelerationEventCount; i++)
				{
					array[i] = GetAccelerationEvent(i);
				}
				return array;
			}
		}

		public static int accelerationEventCount
		{
			get;
		}

		public static Touch[] touches
		{
			get
			{
				int touchCount = Input.touchCount;
				Touch[] array = new Touch[touchCount];
				for (int i = 0; i < touchCount; i++)
				{
					array[i] = GetTouch(i);
				}
				return array;
			}
		}

		public static int touchCount
		{
			get;
		}

		[Obsolete("eatKeyPressOnTextFieldFocus property is deprecated, and only provided to support legacy behavior.")]
		public static bool eatKeyPressOnTextFieldFocus
		{
			get;
			set;
		}

		public static bool touchPressureSupported
		{
			get;
		}

		public static bool touchSupported => true;

		public static bool multiTouchEnabled
		{
			get;
			set;
		}

		public static LocationService location
		{
			get
			{
				if (locationServiceInstance == null)
				{
					locationServiceInstance = new LocationService();
				}
				return locationServiceInstance;
			}
		}

		public static Compass compass
		{
			get
			{
				if (compassInstance == null)
				{
					compassInstance = new Compass();
				}
				return compassInstance;
			}
		}

		public static DeviceOrientation deviceOrientation
		{
			get;
		}

		public static IMECompositionMode imeCompositionMode
		{
			get;
			set;
		}

		public static string compositionString
		{
			get;
		}

		public static bool imeIsSelected
		{
			get;
		}

		public static Vector2 compositionCursorPos
		{
			get
			{
				INTERNAL_get_compositionCursorPos(out Vector2 value);
				return value;
			}
			set
			{
				INTERNAL_set_compositionCursorPos(ref value);
			}
		}

		private static int mainGyroIndex_Internal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static bool GetKeyInt(int key) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static bool GetKeyString(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static bool GetKeyUpInt(int key) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static bool GetKeyUpString(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static bool GetKeyDownInt(int key) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static bool GetKeyDownString(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static float GetAxis(string axisName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static float GetAxisRaw(string axisName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool GetButton(string buttonName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool GetButtonDown(string buttonName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool GetButtonUp(string buttonName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool GetKey(string name)
		{
			return GetKeyString(name);
		}

		public static bool GetKey(KeyCode key)
		{
			return GetKeyInt((int)key);
		}

		public static bool GetKeyDown(string name)
		{
			return GetKeyDownString(name);
		}

		public static bool GetKeyDown(KeyCode key)
		{
			return GetKeyDownInt((int)key);
		}

		public static bool GetKeyUp(string name)
		{
			return GetKeyUpString(name);
		}

		public static bool GetKeyUp(KeyCode key)
		{
			return GetKeyUpInt((int)key);
		}

		public static string[] GetJoystickNames() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool GetMouseButton(int button) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool GetMouseButtonDown(int button) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool GetMouseButtonUp(int button) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void ResetInputAxes() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_mousePosition(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_mouseScrollDelta(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_get_acceleration(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static AccelerationEvent GetAccelerationEvent(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Touch GetTouch(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Use ps3 move API instead", true)]
		public static Quaternion GetRotation(int deviceID)
		{
			return Quaternion.identity;
		}

		[Obsolete("Use ps3 move API instead", true)]
		public static Vector3 GetPosition(int deviceID)
		{
			return Vector3.zero;
		}

		private static void INTERNAL_get_compositionCursorPos(out Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void INTERNAL_set_compositionCursorPos(ref Vector2 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
