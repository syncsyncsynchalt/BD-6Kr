using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(UICamera))]
public class BaseCamera : SingletonMonoBehaviour<BaseCamera>
{
	protected Camera _camCamera;

	protected UICamera _camUICamera;

	public new Camera camera
	{
		get
		{
			if (_camCamera == null)
			{
				_camCamera = GetComponent<Camera>();
			}
			return _camCamera;
		}
	}

	public UICamera uiCamera
	{
		get
		{
			if (_camUICamera == null)
			{
				_camUICamera = GetComponent<UICamera>();
			}
			return _camUICamera;
		}
	}

	public virtual bool isCulling
	{
		get
		{
			return _camCamera.enabled && _camUICamera.enabled;
		}
		set
		{
			Camera camera = this.camera;
			bool flag2 = base.enabled = value;
			flag2 = flag2;
			uiCamera.enabled = flag2;
			camera.enabled = flag2;
		}
	}

	public virtual Generics.Layers cullingMask
	{
		get
		{
			if (_camCamera == null)
			{
				return Generics.Layers.Nothing;
			}
			return (Generics.Layers)_camCamera.cullingMask;
		}
		set
		{
			if (_camCamera == null)
			{
				_camCamera = this.SafeGetComponent<Camera>();
			}
			_camCamera.cullingMask = (int)value;
		}
	}

	public virtual Generics.Layers eventMask
	{
		get
		{
			if (_camUICamera == null)
			{
				return Generics.Layers.Nothing;
			}
			return (Generics.Layers)_camUICamera.eventReceiverMask.value;
		}
		set
		{
			if (_camUICamera == null)
			{
				_camUICamera = this.SafeGetComponent<UICamera>();
			}
			_camUICamera.eventReceiverMask = (int)value;
		}
	}

	public virtual Generics.Layers sameMask
	{
		set
		{
			_camCamera.cullingMask = (_camUICamera.eventReceiverMask = (int)value);
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

	public virtual Color backgroundColor
	{
		get
		{
			return camera.backgroundColor;
		}
		set
		{
			camera.backgroundColor = value;
		}
	}

	public virtual float nearClip
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

	public virtual float farClip
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

	public virtual Rect viewportRect
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

	public virtual float depth
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

	public virtual RenderingPath renderingPath
	{
		get
		{
			return camera.renderingPath;
		}
		set
		{
			camera.renderingPath = value;
		}
	}

	public virtual RenderTexture targetTexture
	{
		get
		{
			return camera.targetTexture;
		}
		set
		{
			camera.targetTexture = value;
		}
	}

	public virtual bool isOcclisionCulling
	{
		get
		{
			return camera.useOcclusionCulling;
		}
		set
		{
			camera.useOcclusionCulling = value;
		}
	}

	public virtual bool isHDR
	{
		get
		{
			return camera.hdr;
		}
		set
		{
			camera.hdr = value;
		}
	}

	protected override void Awake()
	{
		_camCamera = GetComponent<Camera>();
		_camUICamera = this.SafeGetComponent<UICamera>();
	}

	private void OnDestroy()
	{
		Mem.Del(ref _camCamera);
		Mem.Del(ref _camUICamera);
		OnUnInit();
	}

	protected virtual void OnUnInit()
	{
	}

	[Obsolete]
	public virtual void EventMask(Generics.Layers layer)
	{
		_camUICamera.eventReceiverMask = (int)layer;
	}

	[Obsolete]
	public virtual void CullingMask(Generics.Layers layer)
	{
		_camCamera.cullingMask = (int)layer;
	}

	[Obsolete]
	public virtual void SameMask(Generics.Layers layer)
	{
		_camCamera.cullingMask = (_camUICamera.eventReceiverMask = (int)layer);
	}
}
