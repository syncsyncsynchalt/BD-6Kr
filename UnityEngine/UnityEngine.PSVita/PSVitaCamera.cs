using System.Runtime.CompilerServices;

namespace UnityEngine.PSVita;

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
		plus1_5 = 15,
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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern Saturation GetSaturation(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetSaturation(Device devnum, Saturation value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetBrightness(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetBrightness(Device devnum, int value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetContrast(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetContrast(Device devnum, int value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern Sharpness GetSharpness(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetSharpness(Device devnum, Sharpness value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern Reverse GetReverse(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetReverse(Device devnum, Reverse value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern Effect GetEffect(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetEffect(Device devnum, Effect value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern EV GetEV(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetEV(Device devnum, EV value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int GetZoom(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetZoom(Device devnum, int value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern AntiFlicker GetAntiFlicker(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetAntiFlicker(Device devnum, AntiFlicker value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern ISO GetISO(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetISO(Device devnum, ISO value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern WhiteBalance GetWhiteBalance(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetWhiteBalance(Device devnum, WhiteBalance value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool GetBacklight(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetBacklight(Device devnum, bool value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern Nightmode GetNightmode(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetNightmode(Device devnum, Nightmode value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool GetAutoControlHold(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetAutoControlHold(Device devnum, bool value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern ExposureCeiling GetExposureCeiling(Device devnum);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetExposureCeiling(Device devnum, ExposureCeiling value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void SetFormat(CameraFormat value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void DoShutterSound(ShutterSound value);
}
