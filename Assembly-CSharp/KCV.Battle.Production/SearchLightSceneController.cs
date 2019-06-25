using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode]
	public class SearchLightSceneController : MonoBehaviour
	{
		[Serializable]
		public struct SearchLightFocus
		{
			public bool isEval;

			public float z_progress;

			public float xy_progress;

			public float backprogress;
		}

		private Vector3 _friendFrontDirection = Vector3.back;

		public Vector3 _searchLightCameraStartPivot;

		public Vector3 _searchLightPivot;

		public Transform _referenceCameraTransform;

		public float _focusDistance = 5f;

		public float _backLength = 10f;

		private bool _isAnimating;

		public SearchLightFocus _searchLightFocus;

		public Transform _searchLight;

		private Animation _animation;

		public bool _launchLoop;

		public Vector3 SearchLightCameraStartPivot
		{
			get
			{
				return _searchLightCameraStartPivot;
			}
			set
			{
				_searchLightCameraStartPivot = value;
			}
		}

		public Vector3 SearchLightPivot
		{
			get
			{
				return _searchLightPivot;
			}
			set
			{
				_searchLightPivot = value;
			}
		}

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

		private Vector3 SearchLightFocusCameraPoint => _searchLightPivot + _friendFrontDirection * _focusDistance;

		private void OnDestroy()
		{
			Mem.Del(ref _friendFrontDirection);
			Mem.Del(ref _searchLightCameraStartPivot);
			Mem.Del(ref _searchLightPivot);
			Mem.Del(ref _referenceCameraTransform);
			Mem.Del(ref _focusDistance);
			Mem.Del(ref _backLength);
			Mem.Del(ref _isAnimating);
			Mem.Del(ref _searchLightFocus);
			Mem.Del(ref _searchLight);
			Mem.Del(ref _animation);
			Mem.Del(ref _launchLoop);
		}

		private void Start()
		{
			_animation = GetComponent<Animation>();
			_searchLightFocus = default(SearchLightFocus);
			if (_launchLoop)
			{
				PlayAnimation().DelayFrame(10).Repeat().Subscribe(delegate
				{
				});
			}
		}

		private void LateUpdate()
		{
			if (_searchLightFocus.isEval && _referenceCameraTransform != null)
			{
				Vector3 searchLightCameraStartPivot = _searchLightCameraStartPivot;
				Vector3 searchLightFocusCameraPoint = SearchLightFocusCameraPoint;
				Vector2 vector = Vector2.Lerp(new Vector2(searchLightCameraStartPivot.x, searchLightCameraStartPivot.y), new Vector2(searchLightFocusCameraPoint.x, searchLightFocusCameraPoint.y), _searchLightFocus.xy_progress);
				float z = Mathf.Lerp(searchLightCameraStartPivot.z, searchLightFocusCameraPoint.z, _searchLightFocus.z_progress);
				Vector3 a = new Vector3(vector.x, vector.y, z);
				_referenceCameraTransform.position = a + _friendFrontDirection * _backLength * _searchLightFocus.backprogress;
			}
		}

		private void SearchLightOn()
		{
			_searchLight.transform.position = _searchLightPivot;
			SearchLightController component = ((Component)_searchLight).GetComponent<SearchLightController>();
			component.PlayAnimation().Subscribe(delegate
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
			_searchLightFocus = default(SearchLightFocus);
			_searchLightFocus.isEval = true;
			_referenceCameraTransform.transform.eulerAngles = Vector3.zero;
			string anim = "SearchLightFocusAndBack";
			_animation.Play(anim);

            throw new NotImplementedException("‚È‚É‚±‚ê");
            // yield return new WaitForSeconds(_animation.get_Item(anim).length);
            yield return new WaitForSeconds(0);

            _searchLightFocus.isEval = false;
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
