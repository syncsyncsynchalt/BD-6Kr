using LT.Tweening;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(Skybox))]
	[RequireComponent(typeof(Camera))]
	public class BattleFieldDimCamera : BaseCamera
	{
		[SerializeField]
		private Transform _traSyncTarget;

		[SerializeField]
		private MeshRenderer _mrRender;

		[Button("syncTransform", "sync to transform.", new object[]
		{

		})]
		[SerializeField]
		private int syncTrans;

		[SerializeField]
		[Button("SyncCameraProperty", "sync to camera properties", new object[]
		{

		})]
		private int syncCameraProperty;

		[SerializeField]
		[Button("SetMaskPlane", "set to mask plane.", new object[]
		{

		})]
		private int setMaskPlane;

		private bool _isSync;

		private Color _cColor = Color.clear;

		private Skybox _skybox;

		public Transform syncTarget
		{
			get
			{
				if (_traSyncTarget == null)
				{
					return null;
				}
				return _traSyncTarget;
			}
			set
			{
				if (_traSyncTarget != value)
				{
					_traSyncTarget = value;
				}
			}
		}

		public bool isSync
		{
			get
			{
				return _isSync;
			}
			set
			{
				if (_isSync != value)
				{
					_isSync = value;
				}
			}
		}

		public Color maskColor
		{
			get
			{
				if (_cColor == Color.clear)
				{
					_mrRender.material.GetColor("_Color");
				}
				return _cColor;
			}
			set
			{
				if (value != maskColor)
				{
					_cColor = value;
					_mrRender.material.SetColor("_Color", _cColor);
				}
			}
		}

		public float maskAlpha
		{
			get
			{
				Color maskColor = this.maskColor;
				return maskColor.a;
			}
			set
			{
				Color maskColor = this.maskColor;
				if (value != maskColor.a)
				{
					_cColor.a = value;
					Material material = _mrRender.material;
					Color maskColor2 = this.maskColor;
					float r = maskColor2.r;
					Color maskColor3 = this.maskColor;
					float g = maskColor3.g;
					Color maskColor4 = this.maskColor;
					material.SetColor("_Color", new Color(r, g, maskColor4.b, value));
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

		public static BattleFieldDimCamera Instantiate(BattleFieldDimCamera prefab, Transform parent)
		{
			BattleFieldDimCamera battleFieldDimCamera = Object.Instantiate(prefab);
			battleFieldDimCamera.transform.parent = parent;
			battleFieldDimCamera.transform.position = Vector3.zero;
			battleFieldDimCamera.transform.localScale = Vector3.one;
			battleFieldDimCamera.name = "BattleFieldDimCamera";
			return battleFieldDimCamera;
		}

		protected override void Awake()
		{
			base.Awake();
			isCulling = false;
			isSync = SyncCameraProperty();
		}

		protected override void OnUnInit()
		{
			Transform p = ((Component)_mrRender).transform;
			Mem.DelMeshSafe(ref p);
			Mem.Del(ref _traSyncTarget);
			Mem.Del(ref _mrRender);
			Mem.Del(ref _isSync);
			Mem.Del(ref _cColor);
			Mem.DelSkyboxSafe(ref _skybox);
			Mem.Del(ref p);
			base.OnUnInit();
		}

		private void FixedUpdate()
		{
			if (isCulling && isSync)
			{
				SyncTransform();
			}
		}

		public void SyncTransform()
		{
			if (syncTarget != null)
			{
				if (syncTarget.position != base.transform.position)
				{
					base.transform.position = syncTarget.position;
				}
				if (syncTarget.rotation != base.transform.rotation)
				{
					base.transform.rotation = syncTarget.rotation;
				}
			}
		}

		public bool SyncCameraProperty()
		{
			if (syncTarget != null && (bool)((Component)syncTarget).GetComponent<Camera>())
			{
				Camera component = ((Component)syncTarget).GetComponent<Camera>();
				clearFlags = component.clearFlags;
				backgroundColor = component.backgroundColor;
				cullingMask = (Generics.Layers)component.cullingMask;
				nearClip = component.nearClipPlane;
				farClip = component.farClipPlane;
				viewportRect = component.rect;
				isOcclisionCulling = component.useOcclusionCulling;
				isHDR = component.hdr;
				return true;
			}
			return false;
		}

		public void SetMaskPlane()
		{
			if (!((Object)_mrRender == null))
			{
				((Component)_mrRender).transform.localPositionZ(nearClip + 0.1f);
				((Component)_mrRender).transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
			}
		}

		public void SetMaskPlaneAlpha(float from, float to, float time)
		{
			base.transform.LTValue(from, to, time).setOnUpdate(delegate(float x)
			{
				maskAlpha = x;
			});
		}

		public void SetMaskPlaneAlpha(float val)
		{
			maskAlpha = val;
		}
	}
}
