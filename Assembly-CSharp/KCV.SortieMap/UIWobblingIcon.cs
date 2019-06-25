using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.SortieMap
{
	public class UIWobblingIcon : MonoBehaviour
	{
		private const int ORIGIN_NUM = 11;

		[SerializeField]
		private int _nMstID = 500;

		[SerializeField]
		private Material _material;

		[SerializeField]
		private MeshFilter _mfFilter;

		[SerializeField]
		private MeshRenderer _mrRenderer;

		[Button("SetShipTexture", "画像設定", new object[]
		{

		})]
		[SerializeField]
		private int _nSetShipTexture;

		private int _nRenderQueue = 4;

		private List<Vector3> _listCenterOrigin;

		private List<Vector3> _listTempVertex;

		private List<Vector3> _listVertex;

		private List<Vector2> _listUVs;

		private List<Vector3> _listNormals;

		private bool _isWobblling;

		public bool isWobbling
		{
			get
			{
				return _isWobblling;
			}
			set
			{
				_isWobblling = value;
				_mrRenderer.enabled = value;
			}
		}

		private void Awake()
		{
			_listCenterOrigin = new List<Vector3>(11);
			for (int i = 0; i < 11; i++)
			{
				_listCenterOrigin.Add(Vector3.zero);
			}
			_mfFilter.sharedMesh.MarkDynamic();
			_listVertex = new List<Vector3>(_mfFilter.sharedMesh.vertices);
			_listUVs = new List<Vector2>(_mfFilter.mesh.uv);
			_listNormals = new List<Vector3>(_mfFilter.mesh.normals);
			_listTempVertex = new List<Vector3>(_mfFilter.mesh.vertices);
			isWobbling = false;
		}

		private void OnDestroy()
		{
			Transform p = ((Component)_mrRenderer).transform;
			Mem.DelMeshSafe(ref p);
			Mem.Del(ref _material);
			Mem.Del(ref _mfFilter);
			Mem.Del(ref _mrRenderer);
			Mem.DelListSafe(ref _listCenterOrigin);
			Mem.DelListSafe(ref _listTempVertex);
			Mem.DelListSafe(ref _listVertex);
			Mem.DelListSafe(ref _listUVs);
			Mem.DelListSafe(ref _listNormals);
			Mem.Del(ref p);
		}

		public bool FixedRun()
		{
			if (!isWobbling)
			{
				return true;
			}
			for (int i = 0; i < _listCenterOrigin.Count; i++)
			{
				_listCenterOrigin[i] = Mathf.Sin(2f * (Time.time - 0.2f * (float)i)) * Vector3.right;
				for (int j = 0; j < _listCenterOrigin.Count; j++)
				{
					List<Vector3> listTempVertex = _listTempVertex;
					int index = _listCenterOrigin.Count * i + j;
					Vector3 vector = _listCenterOrigin[i];
					float x = 0.6f + vector.x + 5f - (float)j;
					Vector3 vector2 = _listTempVertex[_listCenterOrigin.Count * i + j];
					listTempVertex[index] = new Vector3(x, 0f, vector2.z);
				}
			}
			_mfFilter.mesh.vertices = _listTempVertex.ToArray();
			_mfFilter.mesh.uv = _listUVs.ToArray();
			_mfFilter.mesh.normals = _listNormals.ToArray();
			_mfFilter.mesh.Optimize();
			_mfFilter.mesh.RecalculateBounds();
			return true;
		}

		public LTDescr Show()
		{
			SetShipTexture();
			isWobbling = true;
			Color white = Color.white;
			white.a = 0f;
			_mrRenderer.sharedMaterial.color = white;
			return ((Component)_mrRenderer).transform.LTValue(0f, 1f, 0.3f).setOnUpdate(delegate(float x)
			{
				Color color = _mrRenderer.sharedMaterial.color;
				color.a = x;
				_mrRenderer.sharedMaterial.color = color;
			});
		}

		public LTDescr Hide()
		{
			return ((Component)_mrRenderer).transform.LTValue(1f, 0f, 0.3f).setOnUpdate(delegate(float x)
			{
				Color color = _mrRenderer.sharedMaterial.color;
				color.a = x;
				_mrRenderer.sharedMaterial.color = color;
			}).setOnComplete((Action)delegate
			{
				isWobbling = false;
			});
		}

		private void SetShipTexture()
		{
			Texture2D texture2D = Resources.Load<Texture2D>($"Textures/Ships/{_nMstID}/{9}");
			_mrRenderer.material = new Material(_material);
			_mrRenderer.sharedMaterial.color = Color.clear;
			_mrRenderer.sharedMaterial.mainTexture = texture2D;
			if (texture2D != null)
			{
				((Component)_mrRenderer).transform.localScale = new Vector3(texture2D.width, 0f, texture2D.height);
			}
			_mrRenderer.sharedMaterial.renderQueue = 3000 + _nRenderQueue;
		}
	}
}
