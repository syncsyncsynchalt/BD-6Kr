using System;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Director;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Animator : DirectorPlayer, IAnimatorControllerPlayable
	{
		public bool isOptimizable
		{
			get;
		}

		public bool isHuman
		{
			get;
		}

		public bool hasRootMotion
		{
			get;
		}

		internal bool isRootPositionOrRotationControlledByCurves
		{
			get;
		}

		public float humanScale
		{
			get;
		}

		public bool isInitialized
		{
			get;
		}

		public Vector3 deltaPosition
		{
			get
			{
				INTERNAL_get_deltaPosition(out Vector3 value);
				return value;
			}
		}

		public Quaternion deltaRotation
		{
			get
			{
				INTERNAL_get_deltaRotation(out Quaternion value);
				return value;
			}
		}

		public Vector3 velocity
		{
			get
			{
				INTERNAL_get_velocity(out Vector3 value);
				return value;
			}
		}

		public Vector3 angularVelocity
		{
			get
			{
				INTERNAL_get_angularVelocity(out Vector3 value);
				return value;
			}
		}

		public Vector3 rootPosition
		{
			get
			{
				INTERNAL_get_rootPosition(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_rootPosition(ref value);
			}
		}

		public Quaternion rootRotation
		{
			get
			{
				INTERNAL_get_rootRotation(out Quaternion value);
				return value;
			}
			set
			{
				INTERNAL_set_rootRotation(ref value);
			}
		}

		public bool applyRootMotion
		{
			get;
			set;
		}

		public bool linearVelocityBlending
		{
			get;
			set;
		}

		[Obsolete("Use Animator.updateMode instead")]
		public bool animatePhysics
		{
			get
			{
				return updateMode == AnimatorUpdateMode.AnimatePhysics;
			}
			set
			{
				updateMode = (value ? AnimatorUpdateMode.AnimatePhysics : AnimatorUpdateMode.Normal);
			}
		}

		public AnimatorUpdateMode updateMode
		{
			get;
			set;
		}

		public bool hasTransformHierarchy
		{
			get;
		}

		internal bool allowConstantClipSamplingOptimization
		{
			get;
			set;
		}

		public float gravityWeight
		{
			get;
		}

		public Vector3 bodyPosition
		{
			get
			{
				INTERNAL_get_bodyPosition(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_bodyPosition(ref value);
			}
		}

		public Quaternion bodyRotation
		{
			get
			{
				INTERNAL_get_bodyRotation(out Quaternion value);
				return value;
			}
			set
			{
				INTERNAL_set_bodyRotation(ref value);
			}
		}

		public bool stabilizeFeet
		{
			get;
			set;
		}

		public int layerCount
		{
			get;
		}

		public AnimatorControllerParameter[] parameters
		{
			get;
		}

		public int parameterCount
		{
			get;
		}

		public float feetPivotActive
		{
			get;
			set;
		}

		public float pivotWeight
		{
			get;
		}

		public Vector3 pivotPosition
		{
			get
			{
				INTERNAL_get_pivotPosition(out Vector3 value);
				return value;
			}
		}

		public bool isMatchingTarget
		{
			get;
		}

		public float speed
		{
			get;
			set;
		}

		public Vector3 targetPosition
		{
			get
			{
				INTERNAL_get_targetPosition(out Vector3 value);
				return value;
			}
		}

		public Quaternion targetRotation
		{
			get
			{
				INTERNAL_get_targetRotation(out Quaternion value);
				return value;
			}
		}

		internal Transform avatarRoot
		{
			get;
		}

		public AnimatorCullingMode cullingMode
		{
			get;
			set;
		}

		public float playbackTime
		{
			get;
			set;
		}

		public float recorderStartTime
		{
			get;
			set;
		}

		public float recorderStopTime
		{
			get;
			set;
		}

		public AnimatorRecorderMode recorderMode
		{
			get;
		}

		public RuntimeAnimatorController runtimeAnimatorController
		{
			get;
			set;
		}

		public Avatar avatar
		{
			get;
			set;
		}

		public bool layersAffectMassCenter
		{
			get;
			set;
		}

		public float leftFeetBottomHeight
		{
			get;
		}

		public float rightFeetBottomHeight
		{
			get;
		}

		public bool logWarnings
		{
			get;
			set;
		}

		public bool fireEvents
		{
			get;
			set;
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

		public void SetFloat(string name, float value, float dampTime, float deltaTime)
		{
			SetFloatStringDamp(name, value, dampTime, deltaTime);
		}

		public void SetFloat(int id, float value)
		{
			SetFloatID(id, value);
		}

		public void SetFloat(int id, float value, float dampTime, float deltaTime)
		{
			SetFloatIDDamp(id, value, dampTime, deltaTime);
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

		private void INTERNAL_get_deltaPosition(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_deltaRotation(out Quaternion value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_velocity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_angularVelocity(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_rootPosition(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_rootPosition(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_rootRotation(out Quaternion value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_rootRotation(ref Quaternion value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_bodyPosition(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_bodyPosition(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_bodyRotation(out Quaternion value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_bodyRotation(ref Quaternion value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 GetIKPosition(AvatarIKGoal goal)
		{
			CheckIfInIKPass();
			return GetIKPositionInternal(goal);
		}

		internal Vector3 GetIKPositionInternal(AvatarIKGoal goal) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetIKPosition(AvatarIKGoal goal, Vector3 goalPosition)
		{
			CheckIfInIKPass();
			SetIKPositionInternal(goal, goalPosition);
		}

		internal void SetIKPositionInternal(AvatarIKGoal goal, Vector3 goalPosition)
		{
			INTERNAL_CALL_SetIKPositionInternal(this, goal, ref goalPosition);
		}

		private static void INTERNAL_CALL_SetIKPositionInternal(Animator self, AvatarIKGoal goal, ref Vector3 goalPosition) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Quaternion GetIKRotation(AvatarIKGoal goal)
		{
			CheckIfInIKPass();
			return GetIKRotationInternal(goal);
		}

		internal Quaternion GetIKRotationInternal(AvatarIKGoal goal) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetIKRotation(AvatarIKGoal goal, Quaternion goalRotation)
		{
			CheckIfInIKPass();
			SetIKRotationInternal(goal, goalRotation);
		}

		internal void SetIKRotationInternal(AvatarIKGoal goal, Quaternion goalRotation)
		{
			INTERNAL_CALL_SetIKRotationInternal(this, goal, ref goalRotation);
		}

		private static void INTERNAL_CALL_SetIKRotationInternal(Animator self, AvatarIKGoal goal, ref Quaternion goalRotation) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetIKPositionWeight(AvatarIKGoal goal)
		{
			CheckIfInIKPass();
			return GetIKPositionWeightInternal(goal);
		}

		internal float GetIKPositionWeightInternal(AvatarIKGoal goal) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetIKPositionWeight(AvatarIKGoal goal, float value)
		{
			CheckIfInIKPass();
			SetIKPositionWeightInternal(goal, value);
		}

		internal void SetIKPositionWeightInternal(AvatarIKGoal goal, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetIKRotationWeight(AvatarIKGoal goal)
		{
			CheckIfInIKPass();
			return GetIKRotationWeightInternal(goal);
		}

		internal float GetIKRotationWeightInternal(AvatarIKGoal goal) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetIKRotationWeight(AvatarIKGoal goal, float value)
		{
			CheckIfInIKPass();
			SetIKRotationWeightInternal(goal, value);
		}

		internal void SetIKRotationWeightInternal(AvatarIKGoal goal, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 GetIKHintPosition(AvatarIKHint hint)
		{
			CheckIfInIKPass();
			return GetIKHintPositionInternal(hint);
		}

		internal Vector3 GetIKHintPositionInternal(AvatarIKHint hint) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetIKHintPosition(AvatarIKHint hint, Vector3 hintPosition)
		{
			CheckIfInIKPass();
			SetIKHintPositionInternal(hint, hintPosition);
		}

		internal void SetIKHintPositionInternal(AvatarIKHint hint, Vector3 hintPosition)
		{
			INTERNAL_CALL_SetIKHintPositionInternal(this, hint, ref hintPosition);
		}

		private static void INTERNAL_CALL_SetIKHintPositionInternal(Animator self, AvatarIKHint hint, ref Vector3 hintPosition) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetIKHintPositionWeight(AvatarIKHint hint)
		{
			CheckIfInIKPass();
			return GetHintWeightPositionInternal(hint);
		}

		internal float GetHintWeightPositionInternal(AvatarIKHint hint) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetIKHintPositionWeight(AvatarIKHint hint, float value)
		{
			CheckIfInIKPass();
			SetIKHintPositionWeightInternal(hint, value);
		}

		internal void SetIKHintPositionWeightInternal(AvatarIKHint hint, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetLookAtPosition(Vector3 lookAtPosition)
		{
			CheckIfInIKPass();
			SetLookAtPositionInternal(lookAtPosition);
		}

		internal void SetLookAtPositionInternal(Vector3 lookAtPosition)
		{
			INTERNAL_CALL_SetLookAtPositionInternal(this, ref lookAtPosition);
		}

		private static void INTERNAL_CALL_SetLookAtPositionInternal(Animator self, ref Vector3 lookAtPosition) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void SetLookAtWeight(float weight, float bodyWeight, float headWeight, float eyesWeight)
		{
			float clampWeight = 0.5f;
			SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		public void SetLookAtWeight(float weight, float bodyWeight, float headWeight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		public void SetLookAtWeight(float weight, float bodyWeight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			float headWeight = 1f;
			SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		public void SetLookAtWeight(float weight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			float headWeight = 1f;
			float bodyWeight = 0f;
			SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		public void SetLookAtWeight(float weight, [DefaultValue("0.00f")] float bodyWeight, [DefaultValue("1.00f")] float headWeight, [DefaultValue("0.00f")] float eyesWeight, [DefaultValue("0.50f")] float clampWeight)
		{
			CheckIfInIKPass();
			SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		internal void SetLookAtWeightInternal(float weight, [DefaultValue("0.00f")] float bodyWeight, [DefaultValue("1.00f")] float headWeight, [DefaultValue("0.00f")] float eyesWeight, [DefaultValue("0.50f")] float clampWeight) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		internal void SetLookAtWeightInternal(float weight, float bodyWeight, float headWeight, float eyesWeight)
		{
			float clampWeight = 0.5f;
			SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		internal void SetLookAtWeightInternal(float weight, float bodyWeight, float headWeight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		internal void SetLookAtWeightInternal(float weight, float bodyWeight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			float headWeight = 1f;
			SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		[ExcludeFromDocs]
		internal void SetLookAtWeightInternal(float weight)
		{
			float clampWeight = 0.5f;
			float eyesWeight = 0f;
			float headWeight = 1f;
			float bodyWeight = 0f;
			SetLookAtWeightInternal(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
		}

		public void SetBoneLocalRotation(HumanBodyBones humanBoneId, Quaternion rotation)
		{
			CheckIfInIKPass();
			SetBoneLocalRotationInternal(humanBoneId, rotation);
		}

		internal void SetBoneLocalRotationInternal(HumanBodyBones humanBoneId, Quaternion rotation)
		{
			INTERNAL_CALL_SetBoneLocalRotationInternal(this, humanBoneId, ref rotation);
		}

		private static void INTERNAL_CALL_SetBoneLocalRotationInternal(Animator self, HumanBodyBones humanBoneId, ref Quaternion rotation) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal ScriptableObject GetBehaviour(Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public T GetBehaviour<T>() where T : StateMachineBehaviour
		{
			return GetBehaviour(typeof(T)) as T;
		}

		internal ScriptableObject[] GetBehaviours(Type type) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal static T[] ConvertStateMachineBehaviour<T>(ScriptableObject[] rawObjects) where T : StateMachineBehaviour
		{
			if (rawObjects == null)
			{
				return null;
			}
			T[] array = new T[rawObjects.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (T)rawObjects[i];
			}
			return array;
		}

		public T[] GetBehaviours<T>() where T : StateMachineBehaviour
		{
			return ConvertStateMachineBehaviour<T>(GetBehaviours(typeof(T)));
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

		private void INTERNAL_get_pivotPosition(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void MatchTarget(Vector3 matchPosition, Quaternion matchRotation, AvatarTarget targetBodyPart, MatchTargetWeightMask weightMask, float startNormalizedTime, [DefaultValue("1")] float targetNormalizedTime)
		{
			INTERNAL_CALL_MatchTarget(this, ref matchPosition, ref matchRotation, targetBodyPart, ref weightMask, startNormalizedTime, targetNormalizedTime);
		}

		[ExcludeFromDocs]
		public void MatchTarget(Vector3 matchPosition, Quaternion matchRotation, AvatarTarget targetBodyPart, MatchTargetWeightMask weightMask, float startNormalizedTime)
		{
			float targetNormalizedTime = 1f;
			INTERNAL_CALL_MatchTarget(this, ref matchPosition, ref matchRotation, targetBodyPart, ref weightMask, startNormalizedTime, targetNormalizedTime);
		}

		private static void INTERNAL_CALL_MatchTarget(Animator self, ref Vector3 matchPosition, ref Quaternion matchRotation, AvatarTarget targetBodyPart, ref MatchTargetWeightMask weightMask, float startNormalizedTime, float targetNormalizedTime) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InterruptMatchTarget([DefaultValue("true")] bool completeMatch) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void InterruptMatchTarget()
		{
			bool completeMatch = true;
			InterruptMatchTarget(completeMatch);
		}

		[Obsolete("ForceStateNormalizedTime is deprecated. Please use Play or CrossFade instead.")]
		public void ForceStateNormalizedTime(float normalizedTime)
		{
			Play(0, 0, normalizedTime);
		}

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

		public void SetTarget(AvatarTarget targetIndex, float targetNormalizedTime) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_targetPosition(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_targetRotation(out Quaternion value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("use mask and layers to control subset of transfroms in a skeleton", true)]
		public bool IsControlled(Transform transform) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal bool IsBoneTransform(Transform transform) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Transform GetBoneTransform(HumanBodyBones humanBoneId) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void StartPlayback() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void StopPlayback() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void StartRecording(int frameCount) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void StopRecording() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool HasState(int layerIndex, int stateID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int StringToHash(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal string GetStats() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void CheckIfInIKPass()
		{
			if (logWarnings && !CheckIfInIKPassInternal())
			{
				Debug.LogWarning("Setting and getting IK Goals, Lookat and BoneLocalRotation should only be done in OnAnimatorIK or OnStateIK");
			}
		}

		private bool CheckIfInIKPassInternal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

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

		private void SetFloatStringDamp(string name, float value, float dampTime, float deltaTime) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetFloatIDDamp(int id, float value, float dampTime, float deltaTime) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Update(float deltaTime) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Rebind() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ApplyBuiltinRootMotion() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal string ResolveHash(int hash) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("GetVector is deprecated.")]
		public Vector3 GetVector(string name)
		{
			return Vector3.zero;
		}

		[Obsolete("GetVector is deprecated.")]
		public Vector3 GetVector(int id)
		{
			return Vector3.zero;
		}

		[Obsolete("SetVector is deprecated.")]
		public void SetVector(string name, Vector3 value)
		{
		}

		[Obsolete("SetVector is deprecated.")]
		public void SetVector(int id, Vector3 value)
		{
		}

		[Obsolete("GetQuaternion is deprecated.")]
		public Quaternion GetQuaternion(string name)
		{
			return Quaternion.identity;
		}

		[Obsolete("GetQuaternion is deprecated.")]
		public Quaternion GetQuaternion(int id)
		{
			return Quaternion.identity;
		}

		[Obsolete("SetQuaternion is deprecated.")]
		public void SetQuaternion(string name, Quaternion value)
		{
		}

		[Obsolete("SetQuaternion is deprecated.")]
		public void SetQuaternion(int id, Quaternion value)
		{
		}
	}
}
