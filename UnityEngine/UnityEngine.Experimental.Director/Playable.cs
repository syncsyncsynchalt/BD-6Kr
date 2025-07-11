using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.Director;

public class Playable : IDisposable
{
	internal IntPtr m_Ptr;

	internal int m_UniqueId;

	public extern double time
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern PlayState state
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int inputCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int outputCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public Playable()
	{
		m_Ptr = IntPtr.Zero;
		m_UniqueId = GenerateUniqueId();
		InstantiateEnginePlayable();
	}

	internal Playable(bool callCPPConstructor)
	{
		m_Ptr = IntPtr.Zero;
		m_UniqueId = GenerateUniqueId();
		if (callCPPConstructor)
		{
			InstantiateEnginePlayable();
		}
	}

	private void Dispose(bool disposing)
	{
		ReleaseEnginePlayable();
		m_Ptr = IntPtr.Zero;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern int GetUniqueIDInternal();

	public static bool Connect(Playable source, Playable target)
	{
		return Connect(source, target, -1, -1);
	}

	public static bool Connect(Playable source, Playable target, int sourceOutputPort, int targetInputPort)
	{
		if (!CheckPlayableValidity(source, "source") && !CheckPlayableValidity(target, "target"))
		{
			return false;
		}
		if (source != null && !source.CheckInputBounds(sourceOutputPort, acceptAny: true))
		{
			return false;
		}
		if (!target.CheckInputBounds(targetInputPort, acceptAny: true))
		{
			return false;
		}
		return ConnectInternal(source, target, sourceOutputPort, targetInputPort);
	}

	public static void Disconnect(Playable target, int inputPort)
	{
		if (CheckPlayableValidity(target, "target") && target.CheckInputBounds(inputPort))
		{
			DisconnectInternal(target, inputPort);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void ReleaseEnginePlayable();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void InstantiateEnginePlayable();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern int GenerateUniqueId();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern bool SetInputWeightInternal(int inputIndex, float weight);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern float GetInputWeightInternal(int inputIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern bool ConnectInternal(Playable source, Playable target, int sourceOutputPort, int targetInputPort);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal static extern void DisconnectInternal(Playable target, int inputPort);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Playable GetInput(int inputPort);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Playable[] GetInputs();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void ClearInputs();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Playable GetOutput(int outputPort);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Playable[] GetOutputs();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void GetInputsInternal(object list);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void GetOutputsInternal(object list);

	~Playable()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public override bool Equals(object p)
	{
		return CompareIntPtr(this, p as Playable);
	}

	public override int GetHashCode()
	{
		return m_UniqueId;
	}

	internal static bool CompareIntPtr(Playable lhs, Playable rhs)
	{
		bool flag = (object)lhs == null || !IsNativePlayableAlive(lhs);
		bool flag2 = (object)rhs == null || !IsNativePlayableAlive(rhs);
		if (flag2 && flag)
		{
			return true;
		}
		if (flag2)
		{
			return !IsNativePlayableAlive(lhs);
		}
		if (flag)
		{
			return !IsNativePlayableAlive(rhs);
		}
		return lhs.GetUniqueIDInternal() == rhs.GetUniqueIDInternal();
	}

	internal static bool IsNativePlayableAlive(Playable p)
	{
		return p.m_Ptr != IntPtr.Zero;
	}

	internal static bool CheckPlayableValidity(Playable playable, string name)
	{
		if (playable == null)
		{
			throw new NullReferenceException("Playable " + name + "is null");
		}
		return true;
	}

	internal bool CheckInputBounds(int inputIndex)
	{
		return CheckInputBounds(inputIndex, acceptAny: false);
	}

	internal bool CheckInputBounds(int inputIndex, bool acceptAny)
	{
		if (inputIndex == -1 && acceptAny)
		{
			return true;
		}
		if (inputIndex < 0)
		{
			throw new IndexOutOfRangeException("Index must be greater than 0");
		}
		Playable[] inputs = GetInputs();
		if (inputs.Length <= inputIndex)
		{
			throw new IndexOutOfRangeException("inputIndex " + inputIndex + " is greater than the number of available inputs (" + inputs.Length + ").");
		}
		return true;
	}

	public float GetInputWeight(int inputIndex)
	{
		if (CheckInputBounds(inputIndex))
		{
			return GetInputWeightInternal(inputIndex);
		}
		return -1f;
	}

	public bool SetInputWeight(int inputIndex, float weight)
	{
		if (CheckInputBounds(inputIndex))
		{
			return SetInputWeightInternal(inputIndex, weight);
		}
		return false;
	}

	public void GetInputs(List<Playable> inputList)
	{
		inputList.Clear();
		GetInputsInternal(inputList);
	}

	public void GetOutputs(List<Playable> outputList)
	{
		outputList.Clear();
		GetOutputsInternal(outputList);
	}

	public virtual void PrepareFrame(FrameData info)
	{
	}

	public virtual void ProcessFrame(FrameData info, object playerData)
	{
	}

	public virtual void OnSetTime(float localTime)
	{
	}

	public virtual void OnSetPlayState(PlayState newState)
	{
	}

	public static bool operator ==(Playable x, Playable y)
	{
		return CompareIntPtr(x, y);
	}

	public static bool operator !=(Playable x, Playable y)
	{
		return !CompareIntPtr(x, y);
	}

	public static implicit operator bool(Playable exists)
	{
		return !CompareIntPtr(exists, null);
	}
}
