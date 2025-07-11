using System;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Director;
using UnityEngine.Internal;

namespace UnityEngine;

public sealed class Animator : DirectorPlayer, IAnimatorControllerPlayable
{
	public extern bool isOptimizable
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool isHuman
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool hasRootMotion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	internal extern bool isRootPositionOrRotationControlledByCurves
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern float humanScale
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool isInitialized
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public Vector3 deltaPosition
	{
		get
		{
			INTERNAL_get_deltaPosition(out var value);
			return value;
		}
	}

	public Quaternion deltaRotation
	{
		get
		{
			INTERNAL_get_deltaRotation(out var value);
			return value;
		}
	}

	public Vector3 velocity
	{
		get
		{
			INTERNAL_get_velocity(out var value);
			return value;
		}
	}

	public Vector3 angularVelocity
	{
		get
		{
			INTERNAL_get_angularVelocity(out var value);
			return value;
		}
	}

	public Vector3 rootPosition
	{
		get
		{
			INTERNAL_get_rootPosition(out var value);
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
			INTERNAL_get_rootRotation(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_rootRotation(ref value);
		}
	}

	public extern bool applyRootMotion
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool linearVelocityBlending
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
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

	public extern AnimatorUpdateMode updateMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool hasTransformHierarchy
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	internal extern bool allowConstantClipSamplingOptimization
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float gravityWeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public Vector3 bodyPosition
	{
		get
		{
			INTERNAL_get_bodyPosition(out var value);
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
			INTERNAL_get_bodyRotation(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_bodyRotation(ref value);
		}
	}

	public extern bool stabilizeFeet
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int layerCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern AnimatorControllerParameter[] parameters
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

	public extern float feetPivotActive
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float pivotWeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public Vector3 pivotPosition
	{
		get
		{
			INTERNAL_get_pivotPosition(out var value);
			return value;
		}
	}

	public extern bool isMatchingTarget
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern float speed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public Vector3 targetPosition
	{
		get
		{
			INTERNAL_get_targetPosition(out var value);
			return value;
		}
	}

	public Quaternion targetRotation
	{
		get
		{
			INTERNAL_get_targetRotation(out var value);
			return value;
		}
	}

	internal extern Transform avatarRoot
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern AnimatorCullingMode cullingMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float playbackTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float recorderStartTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float recorderStopTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern AnimatorRecorderMode recorderMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern RuntimeAnimatorController runtimeAnimatorController
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern Avatar avatar
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool layersAffectMassCenter
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float leftFeetBottomHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern float rightFeetBottomHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern bool logWarnings
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern bool fireEvents
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_deltaPosition(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_deltaRotation(out Quaternion value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_velocity(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_angularVelocity(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_rootPosition(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_rootPosition(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_rootRotation(out Quaternion value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_rootRotation(ref Quaternion value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_bodyPosition(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_bodyPosition(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_bodyRotation(out Quaternion value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_bodyRotation(ref Quaternion value);

	public Vector3 GetIKPosition(AvatarIKGoal goal)
	{
		CheckIfInIKPass();
		return GetIKPositionInternal(goal);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern Vector3 GetIKPositionInternal(AvatarIKGoal goal);

	public void SetIKPosition(AvatarIKGoal goal, Vector3 goalPosition)
	{
		CheckIfInIKPass();
		SetIKPositionInternal(goal, goalPosition);
	}

	internal void SetIKPositionInternal(AvatarIKGoal goal, Vector3 goalPosition)
	{
		INTERNAL_CALL_SetIKPositionInternal(this, goal, ref goalPosition);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetIKPositionInternal(Animator self, AvatarIKGoal goal, ref Vector3 goalPosition);

	public Quaternion GetIKRotation(AvatarIKGoal goal)
	{
		CheckIfInIKPass();
		return GetIKRotationInternal(goal);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern Quaternion GetIKRotationInternal(AvatarIKGoal goal);

	public void SetIKRotation(AvatarIKGoal goal, Quaternion goalRotation)
	{
		CheckIfInIKPass();
		SetIKRotationInternal(goal, goalRotation);
	}

	internal void SetIKRotationInternal(AvatarIKGoal goal, Quaternion goalRotation)
	{
		INTERNAL_CALL_SetIKRotationInternal(this, goal, ref goalRotation);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetIKRotationInternal(Animator self, AvatarIKGoal goal, ref Quaternion goalRotation);

	public float GetIKPositionWeight(AvatarIKGoal goal)
	{
		CheckIfInIKPass();
		return GetIKPositionWeightInternal(goal);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern float GetIKPositionWeightInternal(AvatarIKGoal goal);

	public void SetIKPositionWeight(AvatarIKGoal goal, float value)
	{
		CheckIfInIKPass();
		SetIKPositionWeightInternal(goal, value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void SetIKPositionWeightInternal(AvatarIKGoal goal, float value);

	public float GetIKRotationWeight(AvatarIKGoal goal)
	{
		CheckIfInIKPass();
		return GetIKRotationWeightInternal(goal);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern float GetIKRotationWeightInternal(AvatarIKGoal goal);

	public void SetIKRotationWeight(AvatarIKGoal goal, float value)
	{
		CheckIfInIKPass();
		SetIKRotationWeightInternal(goal, value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void SetIKRotationWeightInternal(AvatarIKGoal goal, float value);

	public Vector3 GetIKHintPosition(AvatarIKHint hint)
	{
		CheckIfInIKPass();
		return GetIKHintPositionInternal(hint);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern Vector3 GetIKHintPositionInternal(AvatarIKHint hint);

	public void SetIKHintPosition(AvatarIKHint hint, Vector3 hintPosition)
	{
		CheckIfInIKPass();
		SetIKHintPositionInternal(hint, hintPosition);
	}

	internal void SetIKHintPositionInternal(AvatarIKHint hint, Vector3 hintPosition)
	{
		INTERNAL_CALL_SetIKHintPositionInternal(this, hint, ref hintPosition);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetIKHintPositionInternal(Animator self, AvatarIKHint hint, ref Vector3 hintPosition);

	public float GetIKHintPositionWeight(AvatarIKHint hint)
	{
		CheckIfInIKPass();
		return GetHintWeightPositionInternal(hint);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern float GetHintWeightPositionInternal(AvatarIKHint hint);

	public void SetIKHintPositionWeight(AvatarIKHint hint, float value)
	{
		CheckIfInIKPass();
		SetIKHintPositionWeightInternal(hint, value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void SetIKHintPositionWeightInternal(AvatarIKHint hint, float value);

	public void SetLookAtPosition(Vector3 lookAtPosition)
	{
		CheckIfInIKPass();
		SetLookAtPositionInternal(lookAtPosition);
	}

	internal void SetLookAtPositionInternal(Vector3 lookAtPosition)
	{
		INTERNAL_CALL_SetLookAtPositionInternal(this, ref lookAtPosition);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetLookAtPositionInternal(Animator self, ref Vector3 lookAtPosition);

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void SetLookAtWeightInternal(float weight, [DefaultValue("0.00f")] float bodyWeight, [DefaultValue("1.00f")] float headWeight, [DefaultValue("0.00f")] float eyesWeight, [DefaultValue("0.50f")] float clampWeight);

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetBoneLocalRotationInternal(Animator self, HumanBodyBones humanBoneId, ref Quaternion rotation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern ScriptableObject GetBehaviour(Type type);

	public T GetBehaviour<T>() where T : StateMachineBehaviour
	{
		return GetBehaviour(typeof(T)) as T;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern ScriptableObject[] GetBehaviours(Type type);

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
	private extern void INTERNAL_get_pivotPosition(out Vector3 value);

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_MatchTarget(Animator self, ref Vector3 matchPosition, ref Quaternion matchRotation, AvatarTarget targetBodyPart, ref MatchTargetWeightMask weightMask, float startNormalizedTime, float targetNormalizedTime);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void InterruptMatchTarget([DefaultValue("true")] bool completeMatch);

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
	public extern void SetTarget(AvatarTarget targetIndex, float targetNormalizedTime);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_targetPosition(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_targetRotation(out Quaternion value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	[Obsolete("use mask and layers to control subset of transfroms in a skeleton", true)]
	public extern bool IsControlled(Transform transform);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern bool IsBoneTransform(Transform transform);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Transform GetBoneTransform(HumanBodyBones humanBoneId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void StartPlayback();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void StopPlayback();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void StartRecording(int frameCount);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void StopRecording();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern bool HasState(int layerIndex, int stateID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public static extern int StringToHash(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern string GetStats();

	private void CheckIfInIKPass()
	{
		if (logWarnings && !CheckIfInIKPassInternal())
		{
			Debug.LogWarning("Setting and getting IK Goals, Lookat and BoneLocalRotation should only be done in OnAnimatorIK or OnStateIK");
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern bool CheckIfInIKPassInternal();

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetFloatStringDamp(string name, float value, float dampTime, float deltaTime);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void SetFloatIDDamp(int id, float value, float dampTime, float deltaTime);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void Update(float deltaTime);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void Rebind();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void ApplyBuiltinRootMotion();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern string ResolveHash(int hash);

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
