using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	public sealed class Terrain : Behaviour
	{
		public enum MaterialType
		{
			BuiltInStandard,
			BuiltInLegacyDiffuse,
			BuiltInLegacySpecular,
			Custom
		}

		public TerrainRenderFlags editorRenderFlags
		{
			get;
			set;
		}

		public TerrainData terrainData
		{
			get;
			set;
		}

		public float treeDistance
		{
			get;
			set;
		}

		public float treeBillboardDistance
		{
			get;
			set;
		}

		public float treeCrossFadeLength
		{
			get;
			set;
		}

		public int treeMaximumFullLODCount
		{
			get;
			set;
		}

		public float detailObjectDistance
		{
			get;
			set;
		}

		public float detailObjectDensity
		{
			get;
			set;
		}

		public bool collectDetailPatches
		{
			get;
			set;
		}

		public float heightmapPixelError
		{
			get;
			set;
		}

		public int heightmapMaximumLOD
		{
			get;
			set;
		}

		public float basemapDistance
		{
			get;
			set;
		}

		[Obsolete("use basemapDistance", true)]
		public float splatmapDistance
		{
			get
			{
				return basemapDistance;
			}
			set
			{
				basemapDistance = value;
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

		public bool castShadows
		{
			get;
			set;
		}

		public ReflectionProbeUsage reflectionProbeUsage
		{
			get;
			set;
		}

		public MaterialType materialType
		{
			get;
			set;
		}

		public Material materialTemplate
		{
			get;
			set;
		}

		public Color legacySpecular
		{
			get
			{
				INTERNAL_get_legacySpecular(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_legacySpecular(ref value);
			}
		}

		public float legacyShininess
		{
			get;
			set;
		}

		public bool drawHeightmap
		{
			get;
			set;
		}

		public bool drawTreesAndFoliage
		{
			get;
			set;
		}

		public static Terrain activeTerrain
		{
			get;
		}

		public static Terrain[] activeTerrains
		{
			get;
		}

		private void INTERNAL_get_lightmapScaleOffset(out Vector4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_lightmapScaleOffset(ref Vector4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_realtimeLightmapScaleOffset(out Vector4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_realtimeLightmapScaleOffset(ref Vector4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void GetClosestReflectionProbesInternal(object result) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void GetClosestReflectionProbes(List<ReflectionProbeBlendInfo> result)
		{
			GetClosestReflectionProbesInternal(result);
		}

		private void INTERNAL_get_legacySpecular(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_legacySpecular(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float SampleHeight(Vector3 worldPosition)
		{
			return INTERNAL_CALL_SampleHeight(this, ref worldPosition);
		}

		private static float INTERNAL_CALL_SampleHeight(Terrain self, ref Vector3 worldPosition) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ApplyDelayedHeightmapModification() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddTreeInstance(TreeInstance instance)
		{
			INTERNAL_CALL_AddTreeInstance(this, ref instance);
		}

		private static void INTERNAL_CALL_AddTreeInstance(Terrain self, ref TreeInstance instance) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetNeighbors(Terrain left, Terrain top, Terrain right, Terrain bottom) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 GetPosition() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Flush() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void RemoveTrees(Vector2 position, float radius, int prototypeIndex)
		{
			INTERNAL_CALL_RemoveTrees(this, ref position, radius, prototypeIndex);
		}

		private static void INTERNAL_CALL_RemoveTrees(Terrain self, ref Vector2 position, float radius, int prototypeIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static GameObject CreateTerrainGameObject(TerrainData assignTerrain) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
