using System;
using UnityEngine;

namespace Librarys.Cameras
{
	[RequireComponent(typeof(Camera))]
	public class CameraActor : MonoBehaviour
	{
		[Flags]
		public enum Axis
		{
			None = 0x1,
			XAxis = 0x2,
			YAxis = 0x4,
			ZAxis = 0x8
		}

		public enum ViewMode
		{
			NotViewModeCtrl = -1,
			Fix,
			FixChasing,
			FixChasingRot,
			ZoomChasing,
			ZoomChasingUp,
			SmoothMove,
			SmoothMoveKI2ndEdition,
			FixedPositionChasing,
			RotateAroundObject,
			Rotation,
			Bezier
		}

		protected Camera _cam;

		[SerializeField]
		protected ViewMode _iViewMode;

		[SerializeField]
		protected Vector3 _vPointOfGaze;

		[SerializeField]
		protected float _fRotateSpeed = 10f;

		[SerializeField]
		protected float _fRotateDistance = 10f;

		protected Axis _iAxis;

		protected Vector3 _vSrcPos;

		protected Quaternion _quaSrcRot;

		protected Vector3 _vDestPos;

		protected Quaternion _quaDestRot;

		protected Quaternion _quaTempRot;

		protected Vector3 _vLeaveRotEuler = Vector3.zero;

		protected Vector3 _vLeavePosEuler = Vector3.zero;

		protected float _fLeavePosDistance;

		[SerializeField]
		protected float _fSmoothTime = 0.3f;

		[SerializeField]
		protected float _fSmoothDistance = 10f;

		[SerializeField]
		protected float _fSmoothRotDamping = 2f;

		[SerializeField]
		protected float _fSmoothCorrectionY;

		protected Vector3 _vSmoothVelocity;

		protected bool _isRotSmoothX = true;

		protected bool _isRotSmoothY = true;

		protected bool _isRotSmoothZ = true;

		[SerializeField]
		protected LayerMask _lmNearCollisionLayer;

		[SerializeField]
		protected Transform _traNearLeftUp;

		[SerializeField]
		protected Transform _traNearLeftDown;

		[SerializeField]
		protected Transform _traNearRightUp;

		[SerializeField]
		protected Transform _traNearRightDown;

		private Bezier _clsBezier;

		[SerializeField]
		protected float _fBezierTime;

		private bool _isAdjust;

		private float _fAdjustY;

		[SerializeField]
		protected float _fYAxisLimit;

		[SerializeField]
		protected bool _isThroughMaxY;

		[SerializeField]
		protected float _fSpecificOrbitLimitY;

		private float _fCorrectAccel = 0.005f;

		private float _fBackPosY;

		private float hosei;

		public new Camera camera
		{
			get
			{
				if (_cam == null)
				{
					_cam = GetComponent<Camera>();
				}
				return _cam;
			}
		}

		public Vector3 eyePosition
		{
			get
			{
				return base.transform.position;
			}
			set
			{
				base.transform.position = value;
			}
		}

		public Quaternion eyeRotation
		{
			get
			{
				return base.transform.rotation;
			}
			set
			{
				base.transform.rotation = value;
			}
		}

		public virtual int cullingMask
		{
			get
			{
				return camera.cullingMask;
			}
			set
			{
				camera.cullingMask = value;
			}
		}

		public virtual float fieldOfView
		{
			get
			{
				return camera.fieldOfView;
			}
			set
			{
				if (camera.fieldOfView != value)
				{
					camera.fieldOfView = value;
				}
			}
		}

		public virtual CameraClearFlags clearFlags
		{
			get
			{
				return camera.clearFlags;
			}
			set
			{
				camera.clearFlags = value;
			}
		}

		public float nearClip
		{
			get
			{
				return camera.nearClipPlane;
			}
			set
			{
				camera.nearClipPlane = value;
			}
		}

		public float farClip
		{
			get
			{
				return camera.farClipPlane;
			}
			set
			{
				camera.farClipPlane = value;
			}
		}

		public float rotateSpeed
		{
			get
			{
				return _fRotateSpeed;
			}
			set
			{
				_fRotateSpeed = value;
			}
		}

		public float rotateDistance
		{
			get
			{
				return _fRotateDistance;
			}
			set
			{
				_fRotateDistance = value;
			}
		}

		public bool isCulling
		{
			get
			{
				return camera.enabled;
			}
			set
			{
				camera.enabled = value;
			}
		}

		public Rect viewportRect
		{
			get
			{
				return camera.rect;
			}
			set
			{
				camera.rect = value;
			}
		}

		public float depth
		{
			get
			{
				return camera.depth;
			}
			set
			{
				camera.depth = value;
			}
		}

		public virtual ViewMode viewMode
		{
			get
			{
				return _iViewMode;
			}
			set
			{
				_iViewMode = value;
			}
		}

		public Vector3 pointOfGaze
		{
			get
			{
				return _vPointOfGaze;
			}
			set
			{
				_vPointOfGaze = value;
			}
		}

		public Vector3 leaveRotateEuler
		{
			get
			{
				return _vLeaveRotEuler;
			}
			set
			{
				_vLeaveRotEuler = value;
			}
		}

		public Vector3 leavePositionEuler
		{
			get
			{
				return _vLeavePosEuler;
			}
			set
			{
				_vLeavePosEuler = value;
			}
		}

		public float leavePositionDistance
		{
			get
			{
				return _fLeavePosDistance;
			}
			set
			{
				_fLeavePosDistance = value;
			}
		}

		public float smoothTime
		{
			get
			{
				return _fSmoothTime;
			}
			set
			{
				_fSmoothTime = value;
			}
		}

		public float smoothDistance
		{
			get
			{
				return _fSmoothDistance;
			}
			set
			{
				_fSmoothDistance = value;
			}
		}

		public float smoothRotationDamping
		{
			get
			{
				return _fSmoothRotDamping;
			}
			set
			{
				_fSmoothRotDamping = value;
			}
		}

		public float smoothCorrectionY
		{
			get
			{
				return _fSmoothCorrectionY;
			}
			set
			{
				_fSmoothCorrectionY = value;
			}
		}

		public bool isRotationSmoothX
		{
			get
			{
				return _isRotSmoothX;
			}
			set
			{
				_isRotSmoothX = value;
			}
		}

		public bool isRotationSmoothY
		{
			get
			{
				return _isRotSmoothY;
			}
			set
			{
				_isRotSmoothY = value;
			}
		}

		public bool isRotationSmoothZ
		{
			get
			{
				return _isRotSmoothZ;
			}
			set
			{
				_isRotSmoothZ = value;
			}
		}

		public LayerMask nearCollitionLayer
		{
			get
			{
				return _lmNearCollisionLayer;
			}
			set
			{
				_lmNearCollisionLayer = value;
			}
		}

		public Transform nearClipCollitionLeftUp
		{
			get
			{
				return _traNearLeftUp;
			}
			set
			{
				_traNearLeftUp = value;
			}
		}

		public Transform nearClipCollitionLeftDown
		{
			get
			{
				return _traNearLeftDown;
			}
			set
			{
				_traNearLeftDown = value;
			}
		}

		public Transform nearClipCollitionRightUp
		{
			get
			{
				return _traNearRightUp;
			}
			set
			{
				_traNearRightUp = value;
			}
		}

		public Transform nearClipCollitionRightDown
		{
			get
			{
				return _traNearRightDown;
			}
			set
			{
				_traNearRightDown = value;
			}
		}

		public float YAxisLimit
		{
			get
			{
				return _fYAxisLimit;
			}
			set
			{
				_fYAxisLimit = value;
			}
		}

		public bool isThroughMaxY
		{
			get
			{
				return _isThroughMaxY;
			}
			set
			{
				_isThroughMaxY = value;
			}
		}

		public float specificOrbitLimitY
		{
			get
			{
				return _fSpecificOrbitLimitY;
			}
			set
			{
				_fSpecificOrbitLimitY = value;
			}
		}

		public Bezier bezier
		{
			get
			{
				return _clsBezier;
			}
			set
			{
				_clsBezier = value;
			}
		}

		public float bezierTime
		{
			get
			{
				return _fBezierTime;
			}
			set
			{
				_fBezierTime = Mathe.MinMax2(value, 0f, 1f);
			}
		}

		protected virtual void Awake()
		{
			_iViewMode = ViewMode.Fix;
			_vSmoothVelocity = Vector3.zero;
			_iAxis = Axis.None;
		}

		protected virtual void Start()
		{
			if (_traNearLeftUp != null)
			{
				_traNearLeftUp.position = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0f, 1f, GetComponent<Camera>().nearClipPlane));
			}
			if (_traNearLeftDown != null)
			{
				_traNearLeftDown.position = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0f, 0f, GetComponent<Camera>().nearClipPlane));
			}
			if (_traNearRightUp != null)
			{
				_traNearRightUp.position = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1f, 1f, GetComponent<Camera>().nearClipPlane));
			}
			if (_traNearRightDown != null)
			{
				_traNearRightDown.position = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1f, 0f, GetComponent<Camera>().nearClipPlane));
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref _cam);
			Mem.Del(ref _iViewMode);
			Mem.Del(ref _vPointOfGaze);
			Mem.Del(ref _fRotateSpeed);
			Mem.Del(ref _fRotateDistance);
			Mem.Del(ref _iAxis);
			Mem.Del(ref _vSrcPos);
			Mem.Del(ref _quaSrcRot);
			Mem.Del(ref _vDestPos);
			Mem.Del(ref _quaDestRot);
			Mem.Del(ref _quaTempRot);
			Mem.Del(ref _vLeavePosEuler);
			Mem.Del(ref _vLeaveRotEuler);
			Mem.Del(ref _fLeavePosDistance);
			Mem.Del(ref _fSmoothTime);
			Mem.Del(ref _fSmoothDistance);
			Mem.Del(ref _fSmoothRotDamping);
			Mem.Del(ref _fSmoothCorrectionY);
			Mem.Del(ref _vSmoothVelocity);
			Mem.Del(ref _isRotSmoothX);
			Mem.Del(ref _isRotSmoothY);
			Mem.Del(ref _isRotSmoothZ);
			Mem.Del(ref _lmNearCollisionLayer);
			Mem.Del(ref _traNearLeftUp);
			Mem.Del(ref _traNearLeftDown);
			Mem.Del(ref _traNearRightUp);
			Mem.Del(ref _traNearRightDown);
			Mem.Del(ref _clsBezier);
			Mem.Del(ref _fBezierTime);
			Mem.Del(ref _isAdjust);
			Mem.Del(ref _fAdjustY);
			Mem.Del(ref _fYAxisLimit);
			Mem.Del(ref _isThroughMaxY);
			Mem.Del(ref _fSpecificOrbitLimitY);
			Mem.Del(ref _fCorrectAccel);
			Mem.Del(ref _fBackPosY);
			OnUnInit();
		}

		protected virtual void OnUnInit()
		{
		}

		public virtual void ReqViewMode(ViewMode iView)
		{
			_iViewMode = iView;
		}

		public virtual void VelocityReset()
		{
			_vSmoothVelocity = Vector3.zero;
		}

		public virtual void SetPosition(Vector3 src, Vector3 dst)
		{
			_vSrcPos = src;
			_vDestPos = dst;
			_fBezierTime = 0f;
			_initCameraState();
		}

		public virtual void SetPosition(Vector3 src, Quaternion rot, Quaternion dest)
		{
			_vSrcPos = src;
			_quaSrcRot = rot;
			_quaDestRot = dest;
			_initCameraState();
		}

		public virtual void SetPosition(Vector3 pos, Quaternion rot)
		{
			Vector3 a = rot * Quaternion.Euler(_vLeavePosEuler) * Vector3.back;
			_vDestPos = pos + a * _fLeavePosDistance;
			_quaDestRot = rot * Quaternion.Euler(_vLeaveRotEuler);
			_vSrcPos = base.transform.position;
			_quaSrcRot = base.transform.rotation;
			_initCameraState();
		}

		public virtual void SetRawPosition(Vector3 pos)
		{
			base.transform.position = pos;
		}

		public virtual void SetRawRotate(Vector3 rot)
		{
			base.transform.rotation = Quaternion.Euler(rot);
		}

		public virtual void SetFixCamera(Vector3 srcPos, Quaternion srcRot)
		{
			_vSrcPos = srcPos;
			_quaSrcRot = srcRot;
			_initCameraState();
		}

		public virtual void SetFixChasingCamera(Vector3 srcPos)
		{
			_vSrcPos = (_vDestPos = srcPos);
			_initCameraState();
		}

		public virtual void SetFixChasingRotCamera(Axis iAxis, Vector3 srcPos, float rotSpeed)
		{
			_vSrcPos = srcPos;
			_iAxis = iAxis;
			_fRotateSpeed = rotSpeed;
			_initCameraState();
		}

		public virtual void SetSmoothMoveCamera(Vector3 targetPos, Quaternion targetRot, Vector3 srcPos, Quaternion srcRot)
		{
			Vector3 a = targetRot * Quaternion.Euler(_vLeavePosEuler) * Vector3.back;
			_vDestPos = targetPos + a * _fLeavePosDistance;
			_quaDestRot = targetRot * Quaternion.Euler(_vLeaveRotEuler);
			_vSrcPos = srcPos;
			_quaSrcRot = srcRot;
			_initCameraState();
		}

		public virtual void SetSmoothMoveCamera(Vector3 targetPos, Quaternion targetRot, Vector3 srcPos, Quaternion srcRot, float smoothDst)
		{
			_vDestPos = targetPos * _fLeavePosDistance;
			_fSmoothDistance = smoothDistance;
			_vSrcPos = srcPos;
			_quaSrcRot = srcRot;
			_initCameraState();
		}

		public virtual void SetRotateAroundObjectCamera(Vector3 pog, float dir, float rotSpeed)
		{
			_vPointOfGaze = pog;
			_fRotateDistance = dir;
			Vector3 a = Mathe.NormalizeDirection(_vPointOfGaze, base.transform.position);
			Vector3 vector = _vDestPos = pog + a * _fRotateDistance;
			_quaDestRot = Quaternion.Euler(a * -1f);
			_fRotateSpeed = rotSpeed;
			_initCameraState();
		}

		public virtual void SetRotateAroundObjectCamera(Vector3 pog, Vector3 srcPos, float rotSpeed)
		{
			_vPointOfGaze = pog;
			_fRotateDistance = Vector3.Distance(srcPos, _vPointOfGaze);
			Vector3 a = Mathe.NormalizeDirection(_vPointOfGaze, srcPos);
			_vDestPos = srcPos;
			_quaDestRot = Quaternion.Euler(a * -1f);
			_fRotateSpeed = rotSpeed;
			_initCameraState();
		}

		public virtual void SetBezierCamera(Bezier bezier, Vector3 srcPos, Quaternion srcRot)
		{
			_clsBezier = bezier;
			_vSrcPos = srcPos;
			_quaSrcRot = srcRot;
			_vDestPos = _clsBezier.Interpolate(1f);
			_initCameraState();
		}

		public virtual void SetBezierCamera(Vector3 pog, Bezier bezier, Vector3 srcPos, Quaternion srcRot)
		{
			_vPointOfGaze = pog;
			SetBezierCamera(bezier, srcPos, srcRot);
		}

		public virtual void SetRotationCamera(Vector3 srcPos, Quaternion srcRot, float rotSpeed)
		{
			_vSrcPos = srcPos;
			_quaSrcRot = srcRot;
			_fRotateSpeed = rotSpeed;
			_initCameraState();
		}

		protected virtual void _initCameraState()
		{
			_fYAxisLimit = 0f;
			_fSmoothCorrectionY = 0f;
			_isThroughMaxY = false;
			switch (_iViewMode)
			{
			case ViewMode.NotViewModeCtrl:
				break;
			case ViewMode.Fix:
				base.transform.position = _vSrcPos;
				base.transform.rotation = _quaSrcRot;
				_vSmoothVelocity = Vector3.zero;
				break;
			case ViewMode.FixChasing:
				base.transform.position = _vDestPos;
				base.transform.LookAt(_vPointOfGaze);
				break;
			case ViewMode.FixChasingRot:
				base.transform.position = _vSrcPos;
				_quaTempRot = base.transform.rotation;
				break;
			case ViewMode.ZoomChasing:
			{
				float d2 = _fSmoothDistance / Vector3.Distance(_vPointOfGaze, base.transform.position);
				_vDestPos = _vPointOfGaze + (base.transform.position - _vPointOfGaze) * d2;
				_vSmoothVelocity = Vector3.zero;
				_fSmoothTime = 0.2f;
				_isAdjust = false;
				base.transform.LookAt(_vPointOfGaze);
				break;
			}
			case ViewMode.ZoomChasingUp:
			{
				float d = _fSmoothDistance / Vector3.Distance(_vPointOfGaze, base.transform.position);
				_vDestPos = _vPointOfGaze + (base.transform.position - _vPointOfGaze) * d;
				_vSmoothVelocity = Vector3.zero;
				_fSmoothTime = 0.2f;
				_isAdjust = false;
				Vector3 position = base.transform.position;
				_fAdjustY = position.y + 10f;
				base.transform.LookAt(_vPointOfGaze);
				break;
			}
			case ViewMode.SmoothMove:
			case ViewMode.SmoothMoveKI2ndEdition:
				base.transform.position = _vSrcPos;
				base.transform.rotation = _quaSrcRot;
				break;
			case ViewMode.FixedPositionChasing:
				base.transform.position = _vPointOfGaze + base.transform.TransformDirection(Vector3.back * _fSmoothDistance);
				base.transform.rotation = _quaDestRot;
				_vSmoothVelocity = Vector3.zero;
				_fSmoothTime = 0.1f;
				_fSmoothCorrectionY = 6f;
				break;
			case ViewMode.RotateAroundObject:
			{
				Vector3 point = Mathe.Direction(_vPointOfGaze, _vDestPos);
				Vector3 b = _quaDestRot * point;
				base.transform.position = _vPointOfGaze + b;
				base.transform.LookAt(_vPointOfGaze);
				_vSmoothVelocity = Vector3.zero;
				break;
			}
			case ViewMode.Rotation:
				base.transform.position = _vSrcPos;
				base.transform.rotation = _quaSrcRot;
				break;
			case ViewMode.Bezier:
				eyePosition = _vSrcPos;
				eyeRotation = _quaSrcRot;
				_fBezierTime = 0f;
				break;
			}
		}

		protected virtual void FixedUpdate()
		{
			switch (_iViewMode)
			{
			case ViewMode.NotViewModeCtrl:
				break;
			case ViewMode.Fix:
				break;
			case ViewMode.FixChasing:
				base.transform.LookAt(_vPointOfGaze);
				break;
			case ViewMode.FixChasingRot:
				_fixChasingRot();
				break;
			case ViewMode.ZoomChasing:
				_zoomChasing();
				break;
			case ViewMode.ZoomChasingUp:
				_zoomChasingUp();
				break;
			case ViewMode.SmoothMove:
				_smoothMove();
				break;
			case ViewMode.SmoothMoveKI2ndEdition:
				_smoothMoveKI2ndEdition();
				break;
			case ViewMode.FixedPositionChasing:
				_fixedPositionChasing();
				break;
			case ViewMode.RotateAroundObject:
				_rotateAroundObject();
				break;
			case ViewMode.Rotation:
				_rotation();
				break;
			case ViewMode.Bezier:
				_bezier();
				break;
			}
		}

		protected virtual void _fixChasingRot()
		{
			_quaTempRot = base.transform.rotation;
			base.transform.LookAt(_vPointOfGaze);
			Quaternion identity = Quaternion.identity;
			if (_iAxis.HasFlag(Axis.XAxis))
			{
				Quaternion quaternion = Quaternion.Euler(_fRotateSpeed * Time.deltaTime, 0f, 0f);
				_quaTempRot *= quaternion;
				Transform transform = base.transform;
				Vector3 eulerAngles = _quaTempRot.eulerAngles;
				float x = eulerAngles.x;
				Vector3 eulerAngles2 = base.transform.rotation.eulerAngles;
				float y = eulerAngles2.y;
				Vector3 eulerAngles3 = base.transform.rotation.eulerAngles;
				transform.rotation = Quaternion.Euler(x, y, eulerAngles3.z);
			}
			else if (_iAxis.HasFlag(Axis.YAxis))
			{
				Quaternion quaternion = Quaternion.Euler(0f, _fRotateSpeed * Time.deltaTime, 0f);
				_quaTempRot *= quaternion;
				Transform transform2 = base.transform;
				Vector3 eulerAngles4 = base.transform.rotation.eulerAngles;
				float x2 = eulerAngles4.x;
				Vector3 eulerAngles5 = _quaTempRot.eulerAngles;
				float y2 = eulerAngles5.y;
				Vector3 eulerAngles6 = base.transform.rotation.eulerAngles;
				transform2.rotation = Quaternion.Euler(x2, y2, eulerAngles6.z);
			}
			else if (_iAxis.HasFlag(Axis.ZAxis))
			{
				Quaternion quaternion = Quaternion.Euler(0f, 0f, _fRotateSpeed * Time.deltaTime);
				_quaTempRot *= quaternion;
				Transform transform3 = base.transform;
				Vector3 eulerAngles7 = base.transform.rotation.eulerAngles;
				float x3 = eulerAngles7.x;
				Vector3 eulerAngles8 = base.transform.rotation.eulerAngles;
				float y3 = eulerAngles8.y;
				Vector3 eulerAngles9 = _quaTempRot.eulerAngles;
				transform3.rotation = Quaternion.Euler(x3, y3, eulerAngles9.z);
			}
		}

		protected virtual void _zoomChasing()
		{
			Vector3 vector = _vDestPos;
			vector = Vector3.SmoothDamp(base.transform.position, vector, ref _vSmoothVelocity, _fSmoothTime);
			if (_isAdjust)
			{
				Vector3 position = base.transform.position;
				vector.y = Mathf.Lerp(position.y, _fAdjustY, 0.05f);
				base.transform.position = vector;
				base.transform.LookAt(_vPointOfGaze, Vector3.up);
				return;
			}
			base.transform.position = vector;
			if (PositionGroundAdjust(vector) != 0)
			{
				base.transform.LookAt(_vPointOfGaze, Vector3.up);
				_fAdjustY = vector.y + 10f;
				_isAdjust = true;
			}
			else
			{
				base.transform.LookAt(_vPointOfGaze);
			}
		}

		protected virtual void _zoomChasingUp()
		{
			Vector3 vector = _vDestPos;
			vector = Vector3.SmoothDamp(base.transform.position, vector, ref _vSmoothVelocity, _fSmoothTime);
			Vector3 position = base.transform.position;
			vector.y = Mathf.Lerp(position.y, _fAdjustY, 0.05f);
			base.transform.position = vector;
			base.transform.LookAt(_vPointOfGaze, Vector3.up);
		}

		protected virtual void _smoothMove()
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			Vector3 eulerAngles2 = _quaDestRot.eulerAngles;
			Vector3 vector = eulerAngles2;
			if (_isRotSmoothX)
			{
				vector.x = eulerAngles.x;
			}
			if (_isRotSmoothY)
			{
				vector.y = eulerAngles.y;
			}
			if (_isRotSmoothZ)
			{
				vector.z = eulerAngles.z;
			}
			base.transform.rotation = Quaternion.Euler(vector);
			eulerAngles = base.transform.eulerAngles;
			eulerAngles.x = Mathf.LerpAngle(eulerAngles.x, eulerAngles2.x, _fSmoothRotDamping * Time.deltaTime);
			eulerAngles.y = Mathf.LerpAngle(eulerAngles.y, eulerAngles2.y, _fSmoothRotDamping * Time.deltaTime);
			eulerAngles.z = Mathf.LerpAngle(eulerAngles.z, eulerAngles2.z, _fSmoothRotDamping * Time.deltaTime);
			Quaternion rotation = Quaternion.Euler(eulerAngles);
			base.transform.rotation = rotation;
			vector = _vDestPos;
			vector += base.transform.TransformDirection(Vector3.back * _fSmoothDistance);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, vector, ref _vSmoothVelocity, _fSmoothTime);
			PositionGroundAdjust(eyePosition);
		}

		protected virtual void _smoothMoveKI2ndEdition()
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			Vector3 eulerAngles2 = _quaDestRot.eulerAngles;
			Vector3 vector = eulerAngles2;
			if (_isRotSmoothX)
			{
				vector.x = eulerAngles.x;
			}
			if (_isRotSmoothY)
			{
				vector.y = eulerAngles.y;
			}
			if (_isRotSmoothZ)
			{
				vector.z = eulerAngles.z;
			}
			base.transform.rotation = Quaternion.Euler(vector);
			eulerAngles = base.transform.eulerAngles;
			eulerAngles.x = Mathf.LerpAngle(eulerAngles.x, eulerAngles2.x, _fSmoothRotDamping * Time.deltaTime);
			eulerAngles.y = Mathf.LerpAngle(eulerAngles.y, eulerAngles2.y, _fSmoothRotDamping * Time.deltaTime);
			eulerAngles.z = Mathf.LerpAngle(eulerAngles.z, eulerAngles2.z, _fSmoothRotDamping * Time.deltaTime);
			Quaternion rotation = Quaternion.Euler(eulerAngles);
			base.transform.rotation = rotation;
			vector = _vDestPos;
			vector += Mathe.NormalizeDirection(eyePosition, _vDestPos) * _fSmoothDistance;
			base.transform.position = Vector3.SmoothDamp(base.transform.position, vector, ref _vSmoothVelocity, _fSmoothTime);
		}

		protected virtual void _fixedPositionChasing()
		{
			Vector3 a = default(Vector3);
			a.x = _vPointOfGaze.x;
			a.y = _vPointOfGaze.y + 1f;
			a.z = _vPointOfGaze.z;
			Vector3 a2 = _quaDestRot * Quaternion.Euler(_vLeavePosEuler) * Vector3.back;
			_vDestPos = a + a2 * _fLeavePosDistance;
			if (_isAdjust)
			{
				_fSmoothCorrectionY = Mathf.Lerp(_fSmoothCorrectionY, _fYAxisLimit, 0.05f);
				if (_fSmoothCorrectionY >= _fYAxisLimit)
				{
					_isAdjust = false;
				}
			}
			Vector3 vector = default(Vector3);
			vector.x = _vDestPos.x;
			vector.y = _vDestPos.y + _fSmoothCorrectionY;
			vector.z = _vDestPos.z;
			vector += base.transform.TransformDirection(Quaternion.Euler(0f, 0f, 0f) * Vector3.back * _fSmoothDistance);
			Vector3 vector2 = Vector3.SmoothDamp(base.transform.position, vector, ref _vSmoothVelocity, _fSmoothTime);
			RaycastHit raycastHit = default(RaycastHit);

            throw new NotImplementedException("なにこれ");
            // if (Physics.Linecast(vector2 + new Vector3(0f, 4000f, 0f), vector2, ref raycastHit, _lmNearCollisionLayer.value))
			//{
			//	Debug.Log($"高さ補正:{vector2}|{raycastHit.point}");
			//	vector2 = raycastHit.point + new Vector3(0f, 1f, 0f);
			//}

			base.transform.position = vector2;
			if (PositionGroundAdjust(vector2) != 0)
			{
				_isAdjust = true;
				_fYAxisLimit = _fSmoothCorrectionY + 3f;
			}
			if (_vPointOfGaze.y > _fSpecificOrbitLimitY * 0.9f && _vPointOfGaze.y > _fBackPosY)
			{
				_fSmoothCorrectionY = Mathf.Max(2f, _fSmoothCorrectionY - Mathf.Pow(_fCorrectAccel, 2f) * 1.3E-05f);
				_isThroughMaxY = true;
				_fCorrectAccel += 1f;
			}
			else if (_isThroughMaxY && _vPointOfGaze.y < _fSpecificOrbitLimitY * 0.95f)
			{
				_fSmoothCorrectionY = Mathf.Lerp(_fSmoothCorrectionY, 0f, 0.1f);
			}
			_fBackPosY = _vPointOfGaze.y;
		}

		protected virtual void _rotateAroundObject()
		{
			Vector3 a = Mathe.NormalizeDirection(_vPointOfGaze, base.transform.position);
			Quaternion rotation = Quaternion.Euler(0f, _fRotateSpeed * Time.deltaTime, 0f);
			Vector3 b = rotation * (a * _fRotateDistance);
			base.transform.position = _vPointOfGaze + b;
			base.transform.LookAt(_vPointOfGaze);
		}

		protected virtual void _rotation()
		{
			Quaternion quaternion = Quaternion.Euler(0f, _fRotateSpeed * Time.deltaTime, 0f);
			base.transform.rotation *= quaternion;
		}

		protected virtual void _bezier()
		{
			base.transform.LookAt(pointOfGaze);
			Vector3 position = _clsBezier.Interpolate(_fBezierTime);
			base.transform.position = position;
		}

		public int CheckNearCollision(out Vector3 lhit, out Vector3 rhit)
		{
			int num = 0;
			if (_traNearLeftDown == null || _traNearLeftUp == null || _traNearRightDown == null || _traNearRightUp == null)
			{
				lhit = Vector3.zero;
				rhit = Vector3.zero;
				return 0;
			}
			RaycastHit raycastHit = default(RaycastHit);

            throw new NotImplementedException("なにこれ");
            // if (Physics.Linecast(_traNearLeftUp.position, _traNearLeftDown.position, ref raycastHit, _lmNearCollisionLayer.value))
			// {
			// 	lhit = raycastHit.point;
			// 	num++;
			// }
			// else
			// {
			//	lhit = Vector3.zero;
			// }

            throw new NotImplementedException("なにこれ");
            // if (Physics.Linecast(_traNearRightUp.position, _traNearRightDown.position, ref raycastHit, _lmNearCollisionLayer.value))
			// {
			//	rhit = raycastHit.point;
			//	num += 2;
			// }
			// else
			// {
			//	rhit = Vector3.zero;
			// }

			return num;
		}

		public int PositionGroundAdjust(Vector3 newpos)
		{
			Vector3 lhit;
			Vector3 rhit;
			int num = CheckNearCollision(out lhit, out rhit);
			if (num == 3)
			{
				num = ((lhit.y > rhit.y) ? 1 : 2);
			}
			switch (num)
			{
			case 1:
			{
				Vector3 a2 = newpos;
				float y2 = lhit.y;
				Vector3 position2 = _traNearLeftDown.position;
				newpos = a2 + new Vector3(0f, y2 - position2.y + 0.2f, 0f);
				break;
			}
			case 2:
			{
				Vector3 a = newpos;
				float y = rhit.y;
				Vector3 position = _traNearRightDown.position;
				newpos = a + new Vector3(0f, y - position.y + 0.2f, 0f);
				break;
			}
			}
			base.transform.position = newpos;
			return num;
		}

		public virtual void LookAt(Vector3 target)
		{
			_vPointOfGaze = target;
			base.transform.LookAt(target);
		}

		public virtual void LookTo(Vector3 target, float time)
		{
			iTween.LookTo(base.gameObject, target, time);
		}
	}
}
