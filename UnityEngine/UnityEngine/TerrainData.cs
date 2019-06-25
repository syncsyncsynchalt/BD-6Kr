using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class TerrainData : Object
	{
		public int heightmapWidth
		{
			get;
		}

		public int heightmapHeight
		{
			get;
		}

		public int heightmapResolution
		{
			get;
			set;
		}

		public Vector3 heightmapScale
		{
			get
			{
				INTERNAL_get_heightmapScale(out Vector3 value);
				return value;
			}
		}

		public Vector3 size
		{
			get
			{
				INTERNAL_get_size(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_size(ref value);
			}
		}

		public float thickness
		{
			get;
			set;
		}

		public float wavingGrassStrength
		{
			get;
			set;
		}

		public float wavingGrassAmount
		{
			get;
			set;
		}

		public float wavingGrassSpeed
		{
			get;
			set;
		}

		public Color wavingGrassTint
		{
			get
			{
				INTERNAL_get_wavingGrassTint(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_wavingGrassTint(ref value);
			}
		}

		public int detailWidth
		{
			get;
		}

		public int detailHeight
		{
			get;
		}

		public int detailResolution
		{
			get;
		}

		internal int detailResolutionPerPatch
		{
			get;
		}

		public DetailPrototype[] detailPrototypes
		{
			get;
			set;
		}

		public TreeInstance[] treeInstances
		{
			get;
			set;
		}

		public int treeInstanceCount
		{
			get;
		}

		public TreePrototype[] treePrototypes
		{
			get;
			set;
		}

		public int alphamapLayers
		{
			get;
		}

		public int alphamapResolution
		{
			get;
			set;
		}

		public int alphamapWidth
		{
			get;
		}

		public int alphamapHeight
		{
			get;
		}

		public int baseMapResolution
		{
			get;
			set;
		}

		private int alphamapTextureCount
		{
			get;
		}

		public Texture2D[] alphamapTextures
		{
			get
			{
				Texture2D[] array = new Texture2D[alphamapTextureCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = GetAlphamapTexture(i);
				}
				return array;
			}
		}

		public SplatPrototype[] splatPrototypes
		{
			get;
			set;
		}

		public TerrainData()
		{
			Internal_Create(this);
		}

		internal void Internal_Create([Writable] TerrainData terrainData) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal bool HasUser(GameObject user) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void AddUser(GameObject user) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void RemoveUser(GameObject user) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_heightmapScale(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_size(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_size(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetHeight(int x, int y) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetInterpolatedHeight(float x, float y) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float[,] GetHeights(int xBase, int yBase, int width, int height) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetHeights(int xBase, int yBase, float[,] heights)
		{
			if (heights == null)
			{
				throw new NullReferenceException();
			}
			if (xBase + heights.GetLength(1) > heightmapWidth || xBase + heights.GetLength(1) < 0 || yBase + heights.GetLength(0) < 0 || xBase < 0 || yBase < 0 || yBase + heights.GetLength(0) > heightmapHeight)
			{
				throw new ArgumentException(UnityString.Format("X or Y base out of bounds. Setting up to {0}x{1} while map size is {2}x{3}", xBase + heights.GetLength(1), yBase + heights.GetLength(0), heightmapWidth, heightmapHeight));
			}
			Internal_SetHeights(xBase, yBase, heights.GetLength(1), heights.GetLength(0), heights);
		}

		private void Internal_SetHeights(int xBase, int yBase, int width, int height, float[,] heights) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void Internal_SetHeightsDelayLOD(int xBase, int yBase, int width, int height, float[,] heights) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetHeightsDelayLOD(int xBase, int yBase, float[,] heights)
		{
			if (heights == null)
			{
				throw new ArgumentNullException("heights");
			}
			int length = heights.GetLength(0);
			int length2 = heights.GetLength(1);
			if (xBase < 0 || xBase + length2 < 0 || xBase + length2 > heightmapWidth)
			{
				throw new ArgumentException(UnityString.Format("X out of bounds - trying to set {0}-{1} but the terrain ranges from 0-{2}", xBase, xBase + length2, heightmapWidth));
			}
			if (yBase < 0 || yBase + length < 0 || yBase + length > heightmapHeight)
			{
				throw new ArgumentException(UnityString.Format("Y out of bounds - trying to set {0}-{1} but the terrain ranges from 0-{2}", yBase, yBase + length, heightmapHeight));
			}
			Internal_SetHeightsDelayLOD(xBase, yBase, length2, length, heights);
		}

		public float GetSteepness(float x, float y) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public Vector3 GetInterpolatedNormal(float x, float y) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal int GetAdjustedSize(int size) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_wavingGrassTint(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_wavingGrassTint(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetDetailResolution(int detailResolution, int resolutionPerPatch) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void ResetDirtyDetails() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RefreshPrototypes() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int[] GetSupportedLayers(int xBase, int yBase, int totalWidth, int totalHeight) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int[,] GetDetailLayer(int xBase, int yBase, int width, int height, int layer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetDetailLayer(int xBase, int yBase, int layer, int[,] details)
		{
			Internal_SetDetailLayer(xBase, yBase, details.GetLength(1), details.GetLength(0), layer, details);
		}

		private void Internal_SetDetailLayer(int xBase, int yBase, int totalWidth, int totalHeight, int detailIndex, int[,] data) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public TreeInstance GetTreeInstance(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetTreeInstance(int index, TreeInstance instance)
		{
			INTERNAL_CALL_SetTreeInstance(this, index, ref instance);
		}

		private static void INTERNAL_CALL_SetTreeInstance(TerrainData self, int index, ref TreeInstance instance) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void RemoveTreePrototype(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void RecalculateTreePositions() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void RemoveDetailPrototype(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal bool NeedUpgradeScaledTreePrototypes() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void UpgradeScaledTreePrototype() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float[,,] GetAlphamaps(int x, int y, int width, int height) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetAlphamaps(int x, int y, float[,,] map)
		{
			if (map.GetLength(2) != alphamapLayers)
			{
				throw new Exception(UnityString.Format("Float array size wrong (layers should be {0})", alphamapLayers));
			}
			Internal_SetAlphamaps(x, y, map.GetLength(1), map.GetLength(0), map);
		}

		private void Internal_SetAlphamaps(int x, int y, int width, int height, float[,,] map) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void RecalculateBasemapIfDirty() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void SetBasemapDirty(bool dirty) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private Texture2D GetAlphamapTexture(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void AddTree(out TreeInstance tree) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal int RemoveTrees(Vector2 position, float radius, int prototypeIndex)
		{
			return INTERNAL_CALL_RemoveTrees(this, ref position, radius, prototypeIndex);
		}

		private static int INTERNAL_CALL_RemoveTrees(TerrainData self, ref Vector2 position, float radius, int prototypeIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
