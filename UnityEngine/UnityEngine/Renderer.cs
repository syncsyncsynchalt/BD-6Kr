using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	public class Renderer : Component
	{
		internal Transform staticBatchRootTransform
		{
			get;
			set;
		}

		internal int staticBatchIndex
		{
			get;
		}

		public bool isPartOfStaticBatch
		{
			get;
		}

		public Matrix4x4 worldToLocalMatrix
		{
			get
			{
				INTERNAL_get_worldToLocalMatrix(out Matrix4x4 value);
				return value;
			}
		}

		public Matrix4x4 localToWorldMatrix
		{
			get
			{
				INTERNAL_get_localToWorldMatrix(out Matrix4x4 value);
				return value;
			}
		}

		public bool enabled
		{
			get;
			set;
		}

		public ShadowCastingMode shadowCastingMode
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Property castShadows has been deprecated. Use shadowCastingMode instead.")]
		public bool castShadows
		{
			get;
			set;
		}

		public bool receiveShadows
		{
			get;
			set;
		}

		public Material material
		{
			get;
			set;
		}

		public Material sharedMaterial
		{
			get;
			set;
		}

		public Material[] materials
		{
			get;
			set;
		}

		public Material[] sharedMaterials
		{
			get;
			set;
		}

		public Bounds bounds
		{
			get
			{
				INTERNAL_get_bounds(out Bounds value);
				return value;
			}
		}

		public int lightmapIndex
		{
			get;
			set;
		}

		public int realtimeLightmapIndex
		{
			get;
			set;
		}

		public Vector4 lightmapScaleOffset
		{
			get
			{
				INTERNAL_get_lightmapScaleOffset(out Vector4 value);
				return value;
			}
			set
			{
				INTERNAL_set_lightmapScaleOffset(ref value);
			}
		}

		public Vector4 realtimeLightmapScaleOffset
		{
			get
			{
				INTERNAL_get_realtimeLightmapScaleOffset(out Vector4 value);
				return value;
			}
			set
			{
				INTERNAL_set_realtimeLightmapScaleOffset(ref value);
			}
		}

		public bool isVisible
		{
			get;
		}

		public bool useLightProbes
		{
			get;
			set;
		}

		public Transform probeAnchor
		{
			get;
			set;
		}

		public ReflectionProbeUsage reflectionProbeUsage
		{
			get;
			set;
		}

		public string sortingLayerName
		{
			get;
			set;
		}

		public int sortingLayerID
		{
			get;
			set;
		}

		public int sortingOrder
		{
			get;
			set;
		}

		internal void SetSubsetIndex(int index, int subSetIndexForMaterial) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_worldToLocalMatrix(out Matrix4x4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_localToWorldMatrix(out Matrix4x4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_bounds(out Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_lightmapScaleOffset(out Vector4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_lightmapScaleOffset(ref Vector4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_realtimeLightmapScaleOffset(out Vector4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_realtimeLightmapScaleOffset(ref Vector4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetPropertyBlock(MaterialPropertyBlock properties) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void GetPropertyBlock(MaterialPropertyBlock dest) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void GetClosestReflectionProbesInternal(object result) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void GetClosestReflectionProbes(List<ReflectionProbeBlendInfo> result)
		{
			GetClosestReflectionProbesInternal(result);
		}
	}
}
