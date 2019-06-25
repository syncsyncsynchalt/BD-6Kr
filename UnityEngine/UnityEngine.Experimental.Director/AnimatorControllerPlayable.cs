using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine.Experimental.Director
{
	public sealed class AnimatorControllerPlayable : AnimationPlayable, IAnimatorControllerPlayable
	{
		public RuntimeAnimatorController animatorController
		{
			get;
		}

		public int layerCount
		{
			get;
		}

		public int parameterCount
		{
			get;
		}

		private AnimatorControllerParameter[] parameters
		{
			get;
		}

		public AnimatorControllerPlayable(RuntimeAnimatorController controller)
			: base(final: false)
		{
			m_Ptr = IntPtr.Zero;
			InstantiateEnginePlayable(controller);
		}

		private void InstantiateEnginePlayable(RuntimeAnimatorController controller) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		public string GetLayerName(int layerIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int GetLayerIndex(string layerName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetLayerWeight(int layerIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetLayerWeight(int layerIndex, float weight) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal string ResolveHash(int hash) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsInTransition(int layerIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public AnimatorControllerParameter GetParameter(int index)
		{
			AnimatorControllerParameter[] parameters = this.parameters;
			if (index < 0 && index >= this.parameters.Length)
			{
				throw new IndexOutOfRangeException("index");
			}
			return parameters[index];
		}

		private static int StringToHash(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		public void CrossFade(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		public void PlayInFixedTime(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		public void Play(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		public bool HasState(int layerIndex, int stateID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetFloatString(string name, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetFloatID(int id, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private float GetFloatString(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private float GetFloatID(int id) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetBoolString(string name, bool value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetBoolID(int id, bool value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private bool GetBoolString(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private bool GetBoolID(int id) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetIntegerString(string name, int value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetIntegerID(int id, int value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private int GetIntegerString(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private int GetIntegerID(int id) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetTriggerString(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetTriggerID(int id) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void ResetTriggerString(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void ResetTriggerID(int id) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private bool IsParameterControlledByCurveString(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private bool IsParameterControlledByCurveID(int id) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
