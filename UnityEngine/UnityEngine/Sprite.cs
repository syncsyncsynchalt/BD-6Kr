using System;

using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Sprite : Object
	{
		public Bounds bounds
		{
			get
			{
				INTERNAL_get_bounds(out Bounds value);
				return value;
			}
		}

		public Rect rect
		{
			get
			{
				INTERNAL_get_rect(out Rect value);
				return value;
			}
		}

		public float pixelsPerUnit
		{
			get;
		}

		public Texture2D texture
		{
			get;
		}

		public Texture2D associatedAlphaSplitTexture
		{
			get;
		}

		public Rect textureRect
		{
			get
			{
				INTERNAL_get_textureRect(out Rect value);
				return value;
			}
		}

		public Vector2 textureRectOffset
		{
			get
			{
				Internal_GetTextureRectOffset(this, out Vector2 output);
				return output;
			}
		}

		public bool packed
		{
			get;
		}

		public SpritePackingMode packingMode
		{
			get;
		}

		public SpritePackingRotation packingRotation
		{
			get;
		}

		public Vector2 pivot
		{
			get
			{
				Internal_GetPivot(this, out Vector2 output);
				return output;
			}
		}

		public Vector4 border
		{
			get
			{
				INTERNAL_get_border(out Vector4 value);
				return value;
			}
		}

		public Vector2[] vertices
		{
			get;
		}

		public ushort[] triangles
		{
			get;
		}

		public Vector2[] uv
		{
			get;
		}

		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, [DefaultValue("100.0f")] float pixelsPerUnit, [DefaultValue("0")] uint extrude, [DefaultValue("SpriteMeshType.Tight")] SpriteMeshType meshType, [DefaultValue("Vector4.zero")] Vector4 border)
		{
			return INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref border);
		}

		[ExcludeFromDocs]
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType)
		{
			Vector4 border = Vector4.zero;
			return INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref border);
		}

		[ExcludeFromDocs]
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude)
		{
			Vector4 border = Vector4.zero;
			SpriteMeshType meshType = SpriteMeshType.Tight;
			return INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref border);
		}

		[ExcludeFromDocs]
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit)
		{
			Vector4 border = Vector4.zero;
			SpriteMeshType meshType = SpriteMeshType.Tight;
			uint extrude = 0u;
			return INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref border);
		}

		[ExcludeFromDocs]
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot)
		{
			Vector4 border = Vector4.zero;
			SpriteMeshType meshType = SpriteMeshType.Tight;
			uint extrude = 0u;
			float pixelsPerUnit = 100f;
			return INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref border);
		}

		private static Sprite INTERNAL_CALL_Create(Texture2D texture, ref Rect rect, ref Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType, ref Vector4 border) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_bounds(out Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_rect(out Rect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_textureRect(out Rect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_GetTextureRectOffset(Sprite sprite, out Vector2 output) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_GetPivot(Sprite sprite, out Vector2 output) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_border(out Vector4 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void OverrideGeometry(Vector2[] vertices, ushort[] triangles) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
