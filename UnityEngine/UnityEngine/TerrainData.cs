using System;
using System.Runtime.CompilerServices;

namespace UnityEngine;

public sealed class TerrainData : Object
{
	public extern int heightmapWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int heightmapHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int heightmapResolution
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public Vector3 heightmapScale
	{
		get
		{
			INTERNAL_get_heightmapScale(out var value);
			return value;
		}
	}

	public Vector3 size
	{
		get
		{
			INTERNAL_get_size(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_size(ref value);
		}
	}

	public extern float thickness
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float wavingGrassStrength
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float wavingGrassAmount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern float wavingGrassSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public Color wavingGrassTint
	{
		get
		{
			INTERNAL_get_wavingGrassTint(out var value);
			return value;
		}
		set
		{
			INTERNAL_set_wavingGrassTint(ref value);
		}
	}

	public extern int detailWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int detailHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int detailResolution
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	internal extern int detailResolutionPerPatch
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern DetailPrototype[] detailPrototypes
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern TreeInstance[] treeInstances
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int treeInstanceCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern TreePrototype[] treePrototypes
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int alphamapLayers
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int alphamapResolution
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public extern int alphamapWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int alphamapHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
	}

	public extern int baseMapResolution
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	private extern int alphamapTextureCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
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

	public extern SplatPrototype[] splatPrototypes
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[WrapperlessIcall]
		set;
	}

	public TerrainData()
	{
		Internal_Create(this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void Internal_Create([Writable] TerrainData terrainData);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern bool HasUser(GameObject user);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void AddUser(GameObject user);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void RemoveUser(GameObject user);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_heightmapScale(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_size(out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_size(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern float GetHeight(int x, int y);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern float GetInterpolatedHeight(float x, float y);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern float[,] GetHeights(int xBase, int yBase, int width, int height);

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void Internal_SetHeights(int xBase, int yBase, int width, int height, float[,] heights);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void Internal_SetHeightsDelayLOD(int xBase, int yBase, int width, int height, float[,] heights);

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

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern float GetSteepness(float x, float y);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern Vector3 GetInterpolatedNormal(float x, float y);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern int GetAdjustedSize(int size);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_get_wavingGrassTint(out Color value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void INTERNAL_set_wavingGrassTint(ref Color value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void SetDetailResolution(int detailResolution, int resolutionPerPatch);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void ResetDirtyDetails();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern void RefreshPrototypes();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern int[] GetSupportedLayers(int xBase, int yBase, int totalWidth, int totalHeight);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern int[,] GetDetailLayer(int xBase, int yBase, int width, int height, int layer);

	public void SetDetailLayer(int xBase, int yBase, int layer, int[,] details)
	{
		Internal_SetDetailLayer(xBase, yBase, details.GetLength(1), details.GetLength(0), layer, details);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void Internal_SetDetailLayer(int xBase, int yBase, int totalWidth, int totalHeight, int detailIndex, int[,] data);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern TreeInstance GetTreeInstance(int index);

	public void SetTreeInstance(int index, TreeInstance instance)
	{
		INTERNAL_CALL_SetTreeInstance(this, index, ref instance);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern void INTERNAL_CALL_SetTreeInstance(TerrainData self, int index, ref TreeInstance instance);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void RemoveTreePrototype(int index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void RecalculateTreePositions();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void RemoveDetailPrototype(int index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern bool NeedUpgradeScaledTreePrototypes();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void UpgradeScaledTreePrototype();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	public extern float[,,] GetAlphamaps(int x, int y, int width, int height);

	public void SetAlphamaps(int x, int y, float[,,] map)
	{
		if (map.GetLength(2) != alphamapLayers)
		{
			throw new Exception(UnityString.Format("Float array size wrong (layers should be {0})", alphamapLayers));
		}
		Internal_SetAlphamaps(x, y, map.GetLength(1), map.GetLength(0), map);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern void Internal_SetAlphamaps(int x, int y, int width, int height, float[,,] map);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void RecalculateBasemapIfDirty();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void SetBasemapDirty(bool dirty);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private extern Texture2D GetAlphamapTexture(int index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	internal extern void AddTree(out TreeInstance tree);

	internal int RemoveTrees(Vector2 position, float radius, int prototypeIndex)
	{
		return INTERNAL_CALL_RemoveTrees(this, ref position, radius, prototypeIndex);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[WrapperlessIcall]
	private static extern int INTERNAL_CALL_RemoveTrees(TerrainData self, ref Vector2 position, float radius, int prototypeIndex);
}
