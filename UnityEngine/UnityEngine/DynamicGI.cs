using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class DynamicGI
	{
		public static float indirectScale
		{
			get;
			set;
		}

		public static float updateThreshold
		{
			get;
			set;
		}

		public static bool synchronousMode
		{
			get;
			set;
		}

		public static void SetEmissive(Renderer renderer, Color color)
		{
			INTERNAL_CALL_SetEmissive(renderer, ref color);
		}

		private static void INTERNAL_CALL_SetEmissive(Renderer renderer, ref Color color) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void UpdateMaterials(Renderer renderer)
		{
			UpdateMaterialsForRenderer(renderer);
		}

		internal static void UpdateMaterialsForRenderer(Renderer renderer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void UpdateMaterials(Terrain terrain)
		{
			if (terrain == null)
			{
				throw new ArgumentNullException("terrain");
			}
			if (terrain.terrainData == null)
			{
				throw new ArgumentException("Invalid terrainData.");
			}
			UpdateMaterialsForTerrain(terrain, new Rect(0f, 0f, 1f, 1f));
		}

		public static void UpdateMaterials(Terrain terrain, int x, int y, int width, int height)
		{
			if (terrain == null)
			{
				throw new ArgumentNullException("terrain");
			}
			if (terrain.terrainData == null)
			{
				throw new ArgumentException("Invalid terrainData.");
			}
			float num = terrain.terrainData.alphamapWidth;
			float num2 = terrain.terrainData.alphamapHeight;
			UpdateMaterialsForTerrain(terrain, new Rect((float)x / num, (float)y / num2, (float)width / num, (float)height / num2));
		}

		internal static void UpdateMaterialsForTerrain(Terrain terrain, Rect uvBounds)
		{
			INTERNAL_CALL_UpdateMaterialsForTerrain(terrain, ref uvBounds);
		}

		private static void INTERNAL_CALL_UpdateMaterialsForTerrain(Terrain terrain, ref Rect uvBounds) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void UpdateEnvironment() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
