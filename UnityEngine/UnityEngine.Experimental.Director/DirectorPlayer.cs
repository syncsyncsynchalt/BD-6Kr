using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.Director;

public class DirectorPlayer : Behaviour
{
	public void Play(Playable playable, object customData)
	{
		PlayInternal(playable, customData);
	}

	public void Play(Playable playable)
	{
		PlayInternal(playable, null);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void PlayInternal(Playable playable, object customData);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void Stop();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetTime(double time);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern double GetTime();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetTimeUpdateMode(DirectorUpdateMode mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern DirectorUpdateMode GetTimeUpdateMode();
}
