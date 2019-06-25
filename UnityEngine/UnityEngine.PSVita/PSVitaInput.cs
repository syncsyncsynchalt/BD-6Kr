using System;

using System.Runtime.CompilerServices;

namespace UnityEngine.PSVita
{
	public sealed class PSVitaInput
	{
		public enum CompassStability
		{
			CompassUnstable,
			CompassStable,
			CompassVeryStable
		}

		public static bool secondaryTouchIsScreenSpace
		{
			get;
			set;
		}

		public static Touch[] touchesSecondary
		{
			get
			{
				int touchCountSecondary = PSVitaInput.touchCountSecondary;
				Touch[] array = new Touch[touchCountSecondary];
				for (int i = 0; i < touchCountSecondary; i++)
				{
					array[i] = GetSecondaryTouch(i);
				}
				return array;
			}
		}

		public static int touchCountSecondary
		{
			get;
		}

		public static bool secondaryTouchEnabled
		{
			get;
		}

		public static int secondaryTouchWidth
		{
			get;
		}

		public static int secondaryTouchHeight
		{
			get;
		}

		public static CompassStability compassFieldStability
		{
			get;
		}

		public static bool gyroDeadbandFilterEnabled
		{
			get;
			set;
		}

		public static bool gyroTiltCorrectionEnabled
		{
			get;
			set;
		}

		public static bool fingerIdEqSceTouchId
		{
			get;
			set;
		}

		private PSVitaInput()
		{
		}

		public static Touch GetSecondaryTouch(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void ResetMotionSensors() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool WirelesslyControlled() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
