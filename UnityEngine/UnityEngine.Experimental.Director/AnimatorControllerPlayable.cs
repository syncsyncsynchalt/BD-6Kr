using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine.Experimental.Director;

public sealed class AnimatorControllerPlayable : AnimationPlayable, IAnimatorControllerPlayable
{
	public extern RuntimeAnimatorController animatorController
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int layerCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int parameterCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	private extern AnimatorControllerParameter[] parameters
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public AnimatorControllerPlayable(RuntimeAnimatorController controller)
		: base(final: false)
	{
		m_Ptr = IntPtr.Zero;
		InstantiateEnginePlayable(controller);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void InstantiateEnginePlayable(RuntimeAnimatorController controller);

	public override int AddInput(AnimationPlayable source)
	{
		Debug.LogError("AnimationClipPlayable doesn't support adding inputs");
		return -1;
	}

	public override bool SetInput(AnimationPlayable source, int index)
	{
		Debug.LogError("AnimationClipPlayable doesn't support setting inputs");
		return false;
	}

	public override bool SetInputs(IEnumerable<AnimationPlayable> sources)
	{
		Debug.LogError("AnimationClipPlayable doesn't support setting inputs");
		return false;
	}

	public override bool RemoveInput(int index)
	{
		Debug.LogError("AnimationClipPlayable doesn't support removing inputs");
		return false;
	}

	public override bool RemoveInput(AnimationPlayable playable)
	{
		Debug.LogError("AnimationClipPlayable doesn't support removing inputs");
		return false;
	}

	public override bool RemoveAllInputs()
	{
		Debug.LogError("AnimationClipPlayable doesn't support removing inputs");
		return false;
	}

	public float GetFloat(string name)
	{
		return GetFloatString(name);
	}

	public float GetFloat(int id)
	{
		return GetFloatID(id);
	}

	public void SetFloat(string name, float value)
	{
		SetFloatString(name, value);
	}

	public void SetFloat(int id, float value)
	{
		SetFloatID(id, value);
	}

	public bool GetBool(string name)
	{
		return GetBoolString(name);
	}

	public bool GetBool(int id)
	{
		return GetBoolID(id);
	}

	public void SetBool(string name, bool value)
	{
		SetBoolString(name, value);
	}

	public void SetBool(int id, bool value)
	{
		SetBoolID(id, value);
	}

	public int GetInteger(string name)
	{
		return GetIntegerString(name);
	}

	public int GetInteger(int id)
	{
		return GetIntegerID(id);
	}

	public void SetInteger(string name, int value)
	{
		SetIntegerString(name, value);
	}

	public void SetInteger(int id, int value)
	{
		SetIntegerID(id, value);
	}

	public void SetTrigger(string name)
	{
		SetTriggerString(name);
	}

	public void SetTrigger(int id)
	{
		SetTriggerID(id);
	}

	public void ResetTrigger(string name)
	{
		ResetTriggerString(name);
	}

	public void ResetTrigger(int id)
	{
		ResetTriggerID(id);
	}

	public bool IsParameterControlledByCurve(string name)
	{
		return IsParameterControlledByCurveString(name);
	}

	public bool IsParameterControlledByCurve(int id)
	{
		return IsParameterControlledByCurveID(id);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern string GetLayerName(int layerIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern int GetLayerIndex(string layerName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern float GetLayerWeight(int layerIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetLayerWeight(int layerIndex, float weight);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern string ResolveHash(int hash);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern bool IsInTransition(int layerIndex);

	public AnimatorControllerParameter GetParameter(int index)
	{
		AnimatorControllerParameter[] array = parameters;
		if (index < 0 && index >= parameters.Length)
		{
			throw new IndexOutOfRangeException("index");
		}
		return array[index];
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int StringToHash(string name);

	[ExcludeFromDocs]
	public void CrossFadeInFixedTime(string stateName, float transitionDuration, int layer)
	{
		float fixedTime = 0f;
		CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime);
	}

	[ExcludeFromDocs]
	public void CrossFadeInFixedTime(string stateName, float transitionDuration)
	{
		float fixedTime = 0f;
		int layer = -1;
		CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime);
	}

	public void CrossFadeInFixedTime(string stateName, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime)
	{
		CrossFadeInFixedTime(StringToHash(stateName), transitionDuration, layer, fixedTime);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime);

	[ExcludeFromDocs]
	public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, int layer)
	{
		float fixedTime = 0f;
		CrossFadeInFixedTime(stateNameHash, transitionDuration, layer, fixedTime);
	}

	[ExcludeFromDocs]
	public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration)
	{
		float fixedTime = 0f;
		int layer = -1;
		CrossFadeInFixedTime(stateNameHash, transitionDuration, layer, fixedTime);
	}

	[ExcludeFromDocs]
	public void CrossFade(string stateName, float transitionDuration, int layer)
	{
		float normalizedTime = float.NegativeInfinity;
		CrossFade(stateName, transitionDuration, layer, normalizedTime);
	}

	[ExcludeFromDocs]
	public void CrossFade(string stateName, float transitionDuration)
	{
		float normalizedTime = float.NegativeInfinity;
		int layer = -1;
		CrossFade(stateName, transitionDuration, layer, normalizedTime);
	}

	public void CrossFade(string stateName, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
	{
		CrossFade(StringToHash(stateName), transitionDuration, layer, normalizedTime);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void CrossFade(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime);

	[ExcludeFromDocs]
	public void CrossFade(int stateNameHash, float transitionDuration, int layer)
	{
		float normalizedTime = float.NegativeInfinity;
		CrossFade(stateNameHash, transitionDuration, layer, normalizedTime);
	}

	[ExcludeFromDocs]
	public void CrossFade(int stateNameHash, float transitionDuration)
	{
		float normalizedTime = float.NegativeInfinity;
		int layer = -1;
		CrossFade(stateNameHash, transitionDuration, layer, normalizedTime);
	}

	[ExcludeFromDocs]
	public void PlayInFixedTime(string stateName, int layer)
	{
		float fixedTime = float.NegativeInfinity;
		PlayInFixedTime(stateName, layer, fixedTime);
	}

	[ExcludeFromDocs]
	public void PlayInFixedTime(string stateName)
	{
		float fixedTime = float.NegativeInfinity;
		int layer = -1;
		PlayInFixedTime(stateName, layer, fixedTime);
	}

	public void PlayInFixedTime(string stateName, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime)
	{
		PlayInFixedTime(StringToHash(stateName), layer, fixedTime);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void PlayInFixedTime(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime);

	[ExcludeFromDocs]
	public void PlayInFixedTime(int stateNameHash, int layer)
	{
		float fixedTime = float.NegativeInfinity;
		PlayInFixedTime(stateNameHash, layer, fixedTime);
	}

	[ExcludeFromDocs]
	public void PlayInFixedTime(int stateNameHash)
	{
		float fixedTime = float.NegativeInfinity;
		int layer = -1;
		PlayInFixedTime(stateNameHash, layer, fixedTime);
	}

	[ExcludeFromDocs]
	public void Play(string stateName, int layer)
	{
		float normalizedTime = float.NegativeInfinity;
		Play(stateName, layer, normalizedTime);
	}

	[ExcludeFromDocs]
	public void Play(string stateName)
	{
		float normalizedTime = float.NegativeInfinity;
		int layer = -1;
		Play(stateName, layer, normalizedTime);
	}

	public void Play(string stateName, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
	{
		Play(StringToHash(stateName), layer, normalizedTime);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void Play(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime);

	[ExcludeFromDocs]
	public void Play(int stateNameHash, int layer)
	{
		float normalizedTime = float.NegativeInfinity;
		Play(stateNameHash, layer, normalizedTime);
	}

	[ExcludeFromDocs]
	public void Play(int stateNameHash)
	{
		float normalizedTime = float.NegativeInfinity;
		int layer = -1;
		Play(stateNameHash, layer, normalizedTime);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern bool HasState(int layerIndex, int stateID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetFloatString(string name, float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetFloatID(int id, float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern float GetFloatString(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern float GetFloatID(int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetBoolString(string name, bool value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetBoolID(int id, bool value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern bool GetBoolString(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern bool GetBoolID(int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetIntegerString(string name, int value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetIntegerID(int id, int value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern int GetIntegerString(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern int GetIntegerID(int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetTriggerString(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetTriggerID(int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void ResetTriggerString(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void ResetTriggerID(int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern bool IsParameterControlledByCurveString(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern bool IsParameterControlledByCurveID(int id);
}
