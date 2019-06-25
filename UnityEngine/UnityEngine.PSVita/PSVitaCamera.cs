using System;

using System.Runtime.CompilerServices;

namespace UnityEngine.PSVita
{
	public sealed class PSVitaCamera
	{
		public enum Device
		{
			Front,
			Back
		}

		public enum Effect
		{
			Normal,
			Nega,
			Bw,
			Sepia,
			Bluish,
			Reddish,
			Greenish
		}

		public enum Saturation
		{
			sat0 = 0,
			sat0_5 = 5,
			sat1 = 10,
			sat2 = 20,
			sat3 = 30,
			sat4 = 40
		}

		public enum Sharpness
		{
			percent100 = 1,
			percent200,
			percent300,
			percent400
		}

		public enum Reverse
		{
			off,
			mirror,
			flip,
			mirror_flip
		}

		public enum EV
		{
			plus2 = 20,
			plus1_7 = 17,
			plus1_5 = 0xF,
			plus1_3 = 13,
			plus1_0 = 10,
			plus0_7 = 7,
			plus0_5 = 5,
			plus0_3 = 3,
			plus0 = 0,
			minus0_3 = -3,
			minus0_5 = -5,
			minus0_7 = -7,
			minus1_0 = -10,
			minus1_3 = -13,
			minus1_5 = -15,
			minus1_7 = -17,
			minus2_0 = -20
		}

		public enum AntiFlicker
		{
			auto,
			hz50,
			hz60
		}

		public enum ISO
		{
			auto = 1,
			iso100 = 100,
			iso200 = 200,
			iso400 = 400
		}

		public enum WhiteBalance
		{
			auto,
			day,
			cwf,
			a
		}

		public enum Nightmode
		{
			off,
			less10,
			less100,
			over100
		}

		public enum ExposureCeiling
		{
			normal,
			half
		}

		public enum CameraFormat
		{
			YUV422_PLANE = 1,
			YUV422_PACKED,
			YUV420_PLANE,
			YUV422_TO_ARGB,
			YUV422_TO_ABGR,
			RAW8
		}

		public enum ShutterSound
		{
			IMAGE,
			VIDEO_START,
			VIDEO_STOP
		}

		public static Saturation GetSaturation(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetSaturation(Device devnum, Saturation value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int GetBrightness(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetBrightness(Device devnum, int value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int GetContrast(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetContrast(Device devnum, int value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Sharpness GetSharpness(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetSharpness(Device devnum, Sharpness value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Reverse GetReverse(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetReverse(Device devnum, Reverse value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Effect GetEffect(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetEffect(Device devnum, Effect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static EV GetEV(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetEV(Device devnum, EV value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int GetZoom(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetZoom(Device devnum, int value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static AntiFlicker GetAntiFlicker(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetAntiFlicker(Device devnum, AntiFlicker value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static ISO GetISO(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetISO(Device devnum, ISO value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static WhiteBalance GetWhiteBalance(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetWhiteBalance(Device devnum, WhiteBalance value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool GetBacklight(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetBacklight(Device devnum, bool value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Nightmode GetNightmode(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetNightmode(Device devnum, Nightmode value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool GetAutoControlHold(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetAutoControlHold(Device devnum, bool value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static ExposureCeiling GetExposureCeiling(Device devnum) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetExposureCeiling(Device devnum, ExposureCeiling value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void SetFormat(CameraFormat value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void DoShutterSound(ShutterSound value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
