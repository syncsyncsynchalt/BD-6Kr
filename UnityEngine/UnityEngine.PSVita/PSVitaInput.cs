using System.Runtime.CompilerServices;

namespace UnityEngine.PSVita;

public sealed class PSVitaInput
{
	public enum CompassStability
	{
		CompassUnstable,
		CompassStable,
		CompassVeryStable
	}

	public static extern bool secondaryTouchIsScreenSpace
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public static Touch[] touchesSecondary
	{
		get
		{
			int num = touchCountSecondary;
			Touch[] array = new Touch[num];
			for (int i = 0; i < num; i++)
			{
				ref Touch reference = ref array[i];
				reference = GetSecondaryTouch(i);
			}
			return array;
		}
	}

	public static extern int touchCountSecondary
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern bool secondaryTouchEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern int secondaryTouchWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern int secondaryTouchHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern CompassStability compassFieldStability
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public static extern bool gyroDeadbandFilterEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public static extern bool gyroTiltCorrectionEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public static extern bool fingerIdEqSceTouchId
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	private PSVitaInput()
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern Touch GetSecondaryTouch(int index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern void ResetMotionSensors();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern bool WirelesslyControlled();
}
