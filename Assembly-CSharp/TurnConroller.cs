using System;
using System.Collections;
using UniRx;
using UnityEngine;

public class TurnConroller : MonoBehaviour
{
	[Serializable]
	public struct Anim
	{
		public bool isEval;

		public float z_progress;

		public float rotation_progress;
	}

	public Transform _referenceCameraTransform;

	public Vector3 _cameraBeginPivot;

	public Vector3 _cameraEndPivot;

	public Anim _anim;

	public Animation _animation;

	private bool _isAnimating;

	private Quaternion? _beginRotation;

	public bool _isCCW;

	public bool _launchLoop;

	private Subject<int> _explosionTargetSubject = new Subject<int>();

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

	public Vector3 CameraBeginPivot
	{
		get
		{
			return _cameraBeginPivot;
		}
		set
		{
			_cameraBeginPivot = value;
		}
	}

	public Vector3 CameraEndPivot
	{
		get
		{
			return _cameraEndPivot;
		}
		set
		{
			_cameraEndPivot = value;
		}
	}

	public bool IsCCW
	{
		get
		{
			return _isCCW;
		}
		set
		{
			_isCCW = value;
		}
	}

	public UniRx.IObservable<int> ExplosionTargetObservable => _explosionTargetSubject;

	private Vector3 DirectionXZ
	{
		get
		{
			Vector3 vector = CameraEndPivot - CameraBeginPivot;
			return new Vector3(vector.x, 0f, vector.z).normalized;
		}
	}

	private void Start()
	{
		if (_launchLoop)
		{
			PlayAnimation().Delay(TimeSpan.FromSeconds(0.5)).Repeat().Subscribe(delegate
			{
			});
		}
	}

	private void OnDestroy()
	{
		_explosionTargetSubject.OnCompleted();
	}

	private void LateUpdate()
	{
		if (_anim.isEval && _referenceCameraTransform != null)
		{
			ApplyTransform(out Vector3 position, out Quaternion rotation);
			_referenceCameraTransform.position = position;
			_referenceCameraTransform.rotation = rotation;
		}
	}

	private void ApplyTransform(out Vector3 position, out Quaternion rotation)
	{
		Vector3 cameraBeginPivot = CameraBeginPivot;
		Vector3 cameraEndPivot = CameraEndPivot;
		Vector3 vector = position = Vector3.Lerp(cameraBeginPivot, cameraEndPivot, _anim.z_progress);
		float num = 180f;
		float angle = Mathf.Lerp(0f, (!_isCCW) ? num : (0f - num), _anim.rotation_progress);
		rotation = ((!_beginRotation.HasValue) ? Quaternion.identity : _beginRotation.Value) * Quaternion.AngleAxis(angle, Vector3.up);
	}

	private void ExplosionTargetEvent()
	{
		_explosionTargetSubject.OnNext(0);
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
		_anim = default(Anim);
		_anim.isEval = true;
		_beginRotation = Quaternion.LookRotation(-DirectionXZ);
		string anim = "TurnAnimation";
		_animation.Play(anim);

        throw new NotImplementedException("‚È‚É‚±‚ê");
        //yield return new WaitForSeconds(_animation.get_Item(anim).length);
        yield return new WaitForSeconds(0);

        _anim.isEval = false;
		_isAnimating = false;
		observer.OnNext(0);
		observer.OnCompleted();
		_beginRotation = null;
	}

	public UniRx.IObservable<int> PlayAnimation()
	{
		return Observable.FromCoroutine((UniRx.IObserver<int> observer) => AnimationCoroutine(observer));
	}
}
