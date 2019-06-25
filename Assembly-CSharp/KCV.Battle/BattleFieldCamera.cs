using Librarys.Cameras;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(FlareLayer))]
	[RequireComponent(typeof(Camera))]
	[RequireComponent(typeof(MotionBlur))]
	[RequireComponent(typeof(CameraShake))]
	[RequireComponent(typeof(Skybox))]
	[RequireComponent(typeof(Vignetting))]
	[RequireComponent(typeof(GlowEffect))]
	public class BattleFieldCamera : CameraActor
	{
		private bool _isMove;

		private Skybox _skybox;

		private MotionBlur _clsMotionBlur;

		private CameraShake _clsCameraShake;

		private FlareLayer _clsFlareLayer;

		private Vignetting _clsVignetting;

		private GlowEffect _clsGlowEffect;

		private float _fMotionBlurTime;

		private float _fMotionBlurDecay;

		public bool isMove
		{
			get
			{
				return _isMove;
			}
			set
			{
				if (value != _isMove)
				{
					_isMove = value;
				}
			}
		}

		public Skybox skybox
		{
			get
			{
				if (_skybox == null)
				{
					_skybox = GetComponent<Skybox>();
				}
				return _skybox;
			}
			set
			{
				if (value != _skybox)
				{
					_skybox = value;
				}
			}
		}

		public MotionBlur motionBlur
		{
			get
			{
				if (_clsMotionBlur == null)
				{
					_clsMotionBlur = this.AddComponent<MotionBlur>();
				}
				return _clsMotionBlur;
			}
		}

		public FlareLayer flareLayer
		{
			get
			{
				if (_clsFlareLayer == null)
				{
					_clsFlareLayer = GetComponent<FlareLayer>();
				}
				return _clsFlareLayer;
			}
		}

		public float motionBlurTime
		{
			get
			{
				return _fMotionBlurTime;
			}
			set
			{
				if (value != _fMotionBlurTime)
				{
					_fMotionBlurTime = value;
				}
			}
		}

		public float motionBlurDecay
		{
			get
			{
				return _fMotionBlurDecay;
			}
			set
			{
				if (value != _fMotionBlurDecay)
				{
					_fMotionBlurDecay = value;
				}
			}
		}

		public CameraShake cameraShake
		{
			get
			{
				if (_clsCameraShake == null)
				{
					_clsCameraShake = this.SafeGetComponent<CameraShake>();
				}
				return _clsCameraShake;
			}
		}

		public new Generics.Layers cullingMask
		{
			get
			{
				return (Generics.Layers)base.cullingMask;
			}
			set
			{
				base.cullingMask = (int)value;
			}
		}

		public Vignetting vignetting => this.GetComponentThis(ref _clsVignetting);

		public GlowEffect glowEffect => this.GetComponentThis(ref _clsGlowEffect);

		protected override void Awake()
		{
			base.Awake();
			_skybox = GetComponent<Skybox>();
			_isMove = false;
			_clsMotionBlur = this.SafeGetComponent<MotionBlur>();
			_clsMotionBlur.blurAmount = 0.8f;
			_clsMotionBlur.enabled = false;
			if (_clsMotionBlur.shader == null)
			{
				_clsMotionBlur.shader = Shader.Find("Hidden/MotionBlur");
			}
			_clsCameraShake = this.SafeGetComponent<CameraShake>();
			flareLayer.enabled = false;
			vignetting.enabled = false;
			glowEffect.enabled = false;
		}

		private void Reset()
		{
			cullingMask = (Generics.Layers.TransparentFX | Generics.Layers.Water | Generics.Layers.Background | Generics.Layers.ShipGirl | Generics.Layers.Effects);
		}

		protected override void OnUnInit()
		{
			Mem.Del(ref _isMove);
			Mem.DelSkyboxSafe(ref _skybox);
			Mem.Del(ref _clsMotionBlur);
			Mem.Del(ref _clsCameraShake);
			Mem.Del(ref _clsFlareLayer);
			Mem.Del(ref _clsVignetting);
			Mem.Del(ref _clsGlowEffect);
			Mem.Del(ref _fMotionBlurTime);
			Mem.Del(ref _fMotionBlurDecay);
			base.OnUnInit();
		}

		public void SetRotationCamera(Vector3 srcPos, Quaternion srcRot, Quaternion destRot, float rotSpeed)
		{
			ReqViewMode(ViewMode.Rotation);
			_vSrcPos = srcPos;
			_quaSrcRot = srcRot;
			_quaDestRot = destRot;
			_fRotateSpeed = rotSpeed;
			_initCameraState();
		}

		public void SetZoomCamera(Vector3 srcPos, Quaternion srcRot, Vector3 targetPos, Quaternion targetRot, float speed)
		{
			ReqViewMode(ViewMode.SmoothMoveKI2ndEdition);
			_vSrcPos = srcPos;
			_quaSrcRot = srcRot;
			_fSmoothTime = speed;
			SetSmoothMoveCamera(targetPos, targetRot, srcPos, srcRot);
		}

		protected override void FixedUpdate()
		{
			switch (_iViewMode)
			{
			case ViewMode.SmoothMoveKI2ndEdition:
			{
				Vector3 eyePosition = base.eyePosition;
				Vector3 vDestPos = _vDestPos;
				if ((double)Vector3.Distance(eyePosition, vDestPos) <= Math.Floor(1.0))
				{
					_isMove = true;
					ReqViewMode(ViewMode.NotViewModeCtrl);
				}
				break;
			}
			case ViewMode.Rotation:
				if (Mathf.Sign(_fRotateSpeed) == 1f)
				{
					if ((double)Vector3.Distance(base.eyeRotation.eulerAngles, _quaDestRot.eulerAngles) <= Math.Floor(1.0) || (double)Vector3.Distance(base.eyeRotation.eulerAngles, Vector3.up * 360f) <= Math.Floor(1.0))
					{
						_isMove = true;
						ReqViewMode(ViewMode.NotViewModeCtrl);
					}
					break;
				}
				if (Mathf.Sign(_fRotateSpeed) == -1f)
				{
					if ((double)Vector3.Distance(base.eyeRotation.eulerAngles, _quaDestRot.eulerAngles) <= Math.Floor(-1.0))
					{
						_isMove = true;
						ReqViewMode(ViewMode.NotViewModeCtrl);
					}
					break;
				}
				return;
			}
			base.FixedUpdate();
		}

		public void SetMotionBlur(float blurAmount, float blurTime, float blurDecay)
		{
			motionBlur.blurAmount = blurAmount;
			_fMotionBlurTime = blurTime;
			_fMotionBlurDecay = blurDecay;
			motionBlur.enabled = true;
		}

		public void ResetMotionBlur()
		{
			motionBlur.extraBlur = false;
			motionBlur.blurAmount = 0.8f;
			motionBlur.enabled = false;
		}
	}
}
