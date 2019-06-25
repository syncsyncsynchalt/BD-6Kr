using UnityEngine;

namespace Librarys.InspectorExtension
{
	[RequireComponent(typeof(MeshFilter))]
	[AddComponentMenu("GameObject/3DObject")]
	[RequireComponent(typeof(MeshRenderer))]
	public class Object3D : MonoBehaviour
	{
		[SerializeField]
		private Texture _texture;

		[SerializeField]
		private Material _material;

		[SerializeField]
		private Mesh _mesh;

		[SerializeField]
		private Color _color = Color.white;

		[SerializeField]
		private Vector2 _localSize = Vector2.one;

		[SerializeField]
		private int _nRenderQueue;

		[SerializeField]
		[Button("MakePixelPerfect", "MakePixelPerfect.", new object[]
		{

		})]
		private int _isMakePixelPerfect;

		private MeshFilter _mfFiler;

		private MeshRenderer _mrRenderer;

		public virtual Texture mainTexture
		{
			get
			{
				if (_texture != null)
				{
					return _texture;
				}
				return null;
			}
			set
			{
				if (_texture != value)
				{
					_texture = value;
					MarkAsChanged();
				}
			}
		}

		public virtual Material material
		{
			get
			{
				if (_material == null)
				{
					return meshRenderer.material;
				}
				return _material;
			}
			set
			{
				if (_material != value)
				{
					_material = value;
					MarkAsChanged();
				}
			}
		}

		public Color color
		{
			get
			{
				return _color;
			}
			set
			{
				if (_color != value)
				{
					_color = value;
					MarkAsChanged();
				}
			}
		}

		public Mesh mesh
		{
			get
			{
				return _mesh;
			}
			set
			{
				if (_mesh != value)
				{
					_mesh = value;
					MarkAsChanged();
				}
			}
		}

		public MeshFilter meshFilter
		{
			get
			{
				if (_mfFiler == null)
				{
					_mfFiler = this.SafeGetComponent<MeshFilter>();
				}
				return _mfFiler;
			}
			set
			{
				if (_mfFiler != value)
				{
					_mfFiler = value;
				}
			}
		}

		public MeshRenderer meshRenderer
		{
			get
			{
				if ((UnityEngine.Object)_mrRenderer == null)
				{
					_mrRenderer = ((Component)this).SafeGetComponent<MeshRenderer>();
				}
				return _mrRenderer;
			}
			set
			{
				if ((UnityEngine.Object)_mrRenderer != (UnityEngine.Object)value)
				{
					_mrRenderer = value;
				}
			}
		}

		public Vector2 localSize
		{
			get
			{
				return _localSize;
			}
			set
			{
				if (value != _localSize)
				{
					_localSize = value;
				}
			}
		}

		public int renderQueue
		{
			get
			{
				if ((UnityEngine.Object)meshRenderer == null)
				{
					return -1;
				}
				if (meshRenderer.sharedMaterial == null)
				{
					return -1;
				}
				return meshRenderer.sharedMaterial.renderQueue;
			}
			set
			{
				if (!((UnityEngine.Object)meshRenderer == null) && meshRenderer.sharedMaterial != null)
				{
					_nRenderQueue = value;
					meshRenderer.sharedMaterial.renderQueue = _nRenderQueue;
				}
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref _texture);
			Mem.Del(ref _material);
			Mem.Del(ref _mesh);
			Mem.Del(ref _color);
			Mem.Del(ref _nRenderQueue);
			Mem.Del(ref _mfFiler);
			Mem.Del(ref _mrRenderer);
			Transform p = base.transform;
			Mem.DelMeshSafe(ref p);
		}

		public virtual void Release()
		{
			if (_texture != null)
			{
				Resources.UnloadAsset(_texture);
			}
		}

		public virtual void MarkAsChanged()
		{
			if (material != null && meshRenderer.sharedMaterial != material)
			{
				meshRenderer.sharedMaterial = material;
			}
			if (!(mesh != null) || !(meshFilter != null))
			{
				return;
			}
			meshFilter.mesh = mesh;
			if (meshRenderer.material.mainTexture != mainTexture)
			{
				meshRenderer.material.mainTexture = mainTexture;
			}
			if (meshRenderer.material.HasProperty("_Color"))
			{
				if (meshRenderer.material.color != color)
				{
					meshRenderer.material.color = color;
				}
			}
			else if (meshRenderer.material.HasProperty("_TintColor") && meshRenderer.material.GetColor("_TintColor") != color)
			{
				meshRenderer.material.SetColor("_TintColor", color);
			}
		}

		public virtual void MakePixelPerfect()
		{
			if (!(meshRenderer.sharedMaterial.mainTexture == null))
			{
				base.transform.localScale = new Vector3(meshRenderer.sharedMaterial.mainTexture.width, meshRenderer.sharedMaterial.mainTexture.height, 0f);
				localSize = base.transform.localScale;
			}
		}
	}
}
