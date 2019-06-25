using System;
using UniRx;
using UnityEngine;

namespace Librarys.Cameras
{
	[RequireComponent(typeof(Camera))]
	public class CameraShake : MonoBehaviour
	{
		private bool _isShake;

		private bool _isShakePos;

		private bool _isShakeRot;

		private float _fShakeIntensity;

		private float _fDelayTimer;

		private Vector3 _vOriginPosition;

		private Quaternion _quaOriginRotation;

		private Action _actCallback;

		[SerializeField]
		private float _fShakeDecay = 0.002f;

		[SerializeField]
		private float _fCoefShakeIntensity = 0.1f;

		public bool isShake => _isShake;

		public float shakeDecay
		{
			get
			{
				return _fShakeDecay;
			}
			set
			{
				if (value != _fShakeDecay)
				{
					_fShakeDecay = value;
				}
			}
		}

		public float coefShakeIntensity
		{
			get
			{
				return _fCoefShakeIntensity;
			}
			set
			{
				if (value != _fCoefShakeIntensity)
				{
					_fCoefShakeIntensity = value;
				}
			}
		}

		public Vector3 originPosition => _vOriginPosition;

		private Quaternion originRotation => _quaOriginRotation;

		private void FixedUpdate()
		{
			if (!_isShake)
			{
				return;
			}
			if (_fShakeIntensity > 0f)
			{
				if (_isShakePos)
				{
					base.transform.position = _vOriginPosition + XorRandom.GetInsideUnitSphere(_fShakeIntensity);
				}
				if (_isShakeRot)
				{
					base.transform.rotation = new Quaternion(_quaOriginRotation.x + XorRandom.GetFLim(0f - _fShakeIntensity, _fShakeIntensity) * 2f, _quaOriginRotation.y + XorRandom.GetFLim(0f - _fShakeIntensity, _fShakeIntensity) * 2f, _quaOriginRotation.z + XorRandom.GetFLim(0f - _fShakeIntensity, _fShakeIntensity) * 2f, _quaOriginRotation.w + XorRandom.GetFLim(0f - _fShakeIntensity, _fShakeIntensity) * 2f);
				}
				_fShakeIntensity -= _fShakeDecay;
			}
			else
			{
				Observable.TimerFrame(2, FrameCountType.EndOfFrame).Subscribe(delegate
				{
					_isShake = false;
					if (_actCallback != null)
					{
						_actCallback();
					}
				});
			}
		}

		public void Shake()
		{
			Shake(null);
		}

		public void Shake(Action callback)
		{
			if (!_isShake)
			{
				_isShakePos = true;
				_isShakeRot = true;
				_vOriginPosition = base.transform.position;
				_quaOriginRotation = base.transform.rotation;
				_fShakeIntensity = _fCoefShakeIntensity;
				_fDelayTimer = 0f;
				_isShake = true;
				_actCallback = callback;
			}
		}

		public void ShakePos(Action callback)
		{
			if (!_isShake)
			{
				_isShakePos = true;
				_isShakeRot = false;
				_vOriginPosition = base.transform.position;
				_quaOriginRotation = base.transform.rotation;
				_fShakeIntensity = _fCoefShakeIntensity;
				_fDelayTimer = 0f;
				_isShake = true;
				_actCallback = callback;
			}
		}

		public void ShakeRot(Action callback)
		{
			if (!_isShake)
			{
				_isShakePos = false;
				_isShakeRot = true;
				_vOriginPosition = base.transform.position;
				_quaOriginRotation = base.transform.rotation;
				_fShakeIntensity = _fCoefShakeIntensity;
				_fDelayTimer = 0f;
				_isShake = true;
				_actCallback = callback;
			}
		}
	}
}
