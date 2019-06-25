using KCV.Utils;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode]
	public class FlareBulletSceneController : MonoBehaviour
	{
		[Serializable]
		public struct FlareBulletFire
		{
			public bool isActive;

			public float z_progress;

			public float xy_progress;

			public bool isSpinMode;

			public float spinmove_z_progress;

			public float spinrotation_progress;
		}

		private Vector3 _friendFrontDirection = Vector3.back;

		public Transform _referenceCameraTransform;

		public Vector3 _flareBulletCameraStartPivot;

		public Vector3 _flareBulletFirePivot;

		public Vector3 _flareBulletEnemyCameraPivot;

		public float _focusDistance = 5f;

		public FlareBulletFire _flareBulletFire;

		public FlareBullet _flareBullet1;

		public FlareBullet _flareBullet2;

		private Animation _animation;

		private bool _isAnimating;

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

		public Vector3 FlareBulletCameraStartPivot
		{
			get
			{
				return _flareBulletCameraStartPivot;
			}
			set
			{
				_flareBulletCameraStartPivot = value;
			}
		}

		public Vector3 FlareBulletFirePivot
		{
			get
			{
				return _flareBulletFirePivot;
			}
			set
			{
				_flareBulletFirePivot = value;
			}
		}

		public Vector3 FlareBulletEnemyCameraPivot
		{
			get
			{
				return _flareBulletEnemyCameraPivot;
			}
			set
			{
				_flareBulletEnemyCameraPivot = value;
			}
		}

		private Vector3 FlareBulletFocusCameraPoint => _flareBulletFirePivot + _friendFrontDirection * _focusDistance;

		private void Start()
		{
			_animation = GetComponent<Animation>();
			_flareBulletFire = default(FlareBulletFire);
		}

		private void OnDestroy()
		{
			Mem.Del(ref _friendFrontDirection);
			Mem.Del(ref _referenceCameraTransform);
			Mem.Del(ref _flareBulletCameraStartPivot);
			Mem.Del(ref _flareBulletFirePivot);
			Mem.Del(ref _flareBulletEnemyCameraPivot);
			Mem.Del(ref _focusDistance);
			Mem.Del(ref _flareBulletFire);
			Mem.Del(ref _flareBullet1);
			Mem.Del(ref _flareBullet2);
			Mem.Del(ref _animation);
			Mem.Del(ref _isAnimating);
		}

		private void LateUpdate()
		{
			if (_flareBulletFire.isActive)
			{
				if (!_flareBulletFire.isSpinMode)
				{
					Vector3 flareBulletCameraStartPivot = _flareBulletCameraStartPivot;
					Vector3 flareBulletFocusCameraPoint = FlareBulletFocusCameraPoint;
					Vector2 vector = Vector2.Lerp(new Vector2(flareBulletCameraStartPivot.x, flareBulletCameraStartPivot.y), new Vector2(flareBulletFocusCameraPoint.x, flareBulletFocusCameraPoint.y), _flareBulletFire.xy_progress);
					float z = Mathf.Lerp(flareBulletCameraStartPivot.z, flareBulletFocusCameraPoint.z, _flareBulletFire.z_progress);
					_referenceCameraTransform.position = new Vector3(vector.x, vector.y, z);
				}
				else
				{
					Vector3 flareBulletFocusCameraPoint2 = FlareBulletFocusCameraPoint;
					Vector3 flareBulletEnemyCameraPivot = _flareBulletEnemyCameraPivot;
					Vector3 position = Vector3.Lerp(flareBulletFocusCameraPoint2, flareBulletEnemyCameraPivot, _flareBulletFire.spinmove_z_progress);
					_referenceCameraTransform.position = position;
				}
				float y = Mathf.Lerp(0f, 180f, _flareBulletFire.spinrotation_progress);
				_referenceCameraTransform.eulerAngles = new Vector3(0f, y, 0f);
			}
		}

		private void FireFlareBullet()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_055);
			_flareBullet1.PlayAnimation().Subscribe(delegate
			{
			});
			_flareBullet2.PlayAnimation().Subscribe(delegate
			{
			});
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
			_flareBulletFire = default(FlareBulletFire);
			_flareBulletFire.isActive = true;
			_referenceCameraTransform.transform.eulerAngles = Vector3.zero;
			string anim = "FlareBulletFire";
			_animation.Play(anim);

            throw new NotImplementedException("‚È‚É‚±‚ê");
            // yield return new WaitForSeconds(_animation.get_Item(anim).length);
            yield return new WaitForSeconds(0);

            _flareBulletFire.isActive = false;
			_isAnimating = false;
			observer.OnNext(0);
			observer.OnCompleted();
		}

		public UniRx.IObservable<int> PlayAnimation()
		{
			return Observable.FromCoroutine((UniRx.IObserver<int> observer) => AnimationCoroutine(observer));
		}
	}
}
