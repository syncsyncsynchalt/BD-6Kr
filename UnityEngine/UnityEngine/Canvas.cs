using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Canvas : Behaviour
	{
		public delegate void WillRenderCanvases();

		public RenderMode renderMode
		{
			get;
			set;
		}

		public bool isRootCanvas
		{
			get;
		}

		public Camera worldCamera
		{
			get;
			set;
		}

		public Rect pixelRect
		{
			get
			{
				INTERNAL_get_pixelRect(out Rect value);
				return value;
			}
		}

		public float scaleFactor
		{
			get;
			set;
		}

		public float referencePixelsPerUnit
		{
			get;
			set;
		}

		public bool overridePixelPerfect
		{
			get;
			set;
		}

		public bool pixelPerfect
		{
			get;
			set;
		}

		public float planeDistance
		{
			get;
			set;
		}

		public int renderOrder
		{
			get;
		}

		public bool overrideSorting
		{
			get;
			set;
		}

		public int sortingOrder
		{
			get;
			set;
		}

		public int sortingLayerID
		{
			get;
			set;
		}

		public int cachedSortingLayerValue
		{
			get;
		}

		public string sortingLayerName
		{
			get;
			set;
		}

		public static event WillRenderCanvases willRenderCanvases;

		private void INTERNAL_get_pixelRect(out Rect value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static Material GetDefaultCanvasMaterial() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("Shared default material now used for text and general UI elements, call Canvas.GetDefaultCanvasMaterial()")]
		public static Material GetDefaultCanvasTextMaterial() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void SendWillRenderCanvases()
		{
			if (Canvas.willRenderCanvases != null)
			{
				Canvas.willRenderCanvases();
			}
		}

		public static void ForceUpdateCanvases()
		{
			SendWillRenderCanvases();
		}
	}
}
