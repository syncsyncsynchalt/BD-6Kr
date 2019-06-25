using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode]
	public class TorpedoStraightController : MonoBehaviour
	{
		[Serializable]
		public class Params
		{
			public Vector3 beginPivot;

			public Vector3 targetPivot;

			public float gazeHeight = 5f;

			public float targetCameraBackLength = 10f;

			public float quakePower = 1f;

			public float quakeSpeed = 10f;
		}

		[Serializable]
		public struct Anim
		{
			public bool isEval;

			public float z_progress;

			public float y;

			public float pitch_progress;

			public float quake;
		}

		private static readonly string AnimationClipName = "TorpedoStraight";

		public bool _isAnimating;

		private float _tilt;

		private float _animationBegan;

		private Subject<int> _flyingFinishSubject2F = new Subject<int>();

		public Anim _anim;

		public Animation _animation;

		public Params _params = new Params();

		public Transform _referenceCameraTransform;

		public Transform ReferenceCameraTransform
		{
			get
			{
				return _referenceCameraTransform;
			}
			set
			{
				_referenceCameraTransform = value;
			}
		}

		public Vector3 BeginPivot
		{
			get
			{
				return _params.beginPivot;
			}
			set
			{
				_params.beginPivot = value;
				_params.beginPivot.y = 0f;
			}
		}

		public Vector3 TargetPivot
		{
			get
			{
				return _params.targetPivot;
			}
			set
			{
				_params.targetPivot = value;
				_params.targetPivot.y = 0f;
			}
		}

		public UniRx.IObservable<int> FlyingFinish2F => _flyingFinishSubject2F;

		public Vector3 CameraEndPosition
		{
			get
			{
				Vector3 normalized = (BeginPivot - TargetPivot).normalized;
				return TargetPivot + normalized * _params.targetCameraBackLength;
			}
		}

		public Vector3 LookDir => (TargetPivot - BeginPivot).normalized;

		public Vector3 RightDir => Vector3.Cross(Vector3.up, LookDir);

		private void OnValidate()
		{
			_params.beginPivot.y = 0f;
			_params.targetPivot.y = 0f;
		}

		private void MakeCameraTransform(out Vector3 position, out Quaternion rotation)
		{
			Vector3 a = Vector3.Lerp(BeginPivot, CameraEndPosition, _anim.z_progress);
			position = a + Vector3.up * _params.gazeHeight * _anim.y;
			Vector3 vector = Vector3.Slerp(Vector3.down, LookDir, _anim.pitch_progress);
			Vector3 upwards = Vector3.Cross(vector, RightDir);
			rotation = Quaternion.LookRotation(vector, upwards);
		}

		private void LateUpdate()
		{
			if (_isAnimating && _anim.isEval && _referenceCameraTransform != null)
			{
				MakeCameraTransform(out Vector3 position, out Quaternion rotation);
				_referenceCameraTransform.position = position;
				_referenceCameraTransform.rotation = rotation;
				_referenceCameraTransform.Rotate(_referenceCameraTransform.right, 1f);
				_referenceCameraTransform.Rotate(_referenceCameraTransform.forward, _tilt);
				float time = Time.time;
				float num = time - _animationBegan;
				float d = (Mathf.PerlinNoise(num * _params.quakeSpeed, 0f) * 2f - 1f) * _params.quakePower * _anim.quake;
				float d2 = (Mathf.PerlinNoise(num * _params.quakeSpeed, 10f) * 2f - 1f) * _params.quakePower * _anim.quake;
				_referenceCameraTransform.transform.position += _referenceCameraTransform.transform.right * d + _referenceCameraTransform.transform.up * d2;
			}
		}

		private void Start()
		{
			_tilt = ((!(UnityEngine.Random.value > 0.5f)) ? (-2f) : 2f);
		}

		private void OnDestroy()
		{
			_flyingFinishSubject2F.OnCompleted();
		}

		private IEnumerator AnimationCoroutine(UniRx.IObserver<int> observer)
		{
			if (_isAnimating)
			{
				observer.OnNext(0);
				observer.OnCompleted();
				yield break;
			}
			_isAnimating = true;
			_anim.isEval = true;
			_animationBegan = Time.time;
			_animation.Play(AnimationClipName);

            throw new NotImplementedException("‚È‚É‚±‚ê");
            //yield return new WaitForSeconds(_animation.get_Item(AnimationClipName).length);
            yield return new WaitForSeconds(0);


            _anim.isEval = false;
			_isAnimating = false;
			observer.OnNext(0);
			observer.OnCompleted();
		}

		public UniRx.IObservable<int> PlayAnimation()
		{
			return Observable.FromCoroutine((UniRx.IObserver<int> observer) => AnimationCoroutine(observer));
		}

		private void FlyingFinishEvent()
		{
			_flyingFinishSubject2F.OnNext(0);
		}
	}
}
