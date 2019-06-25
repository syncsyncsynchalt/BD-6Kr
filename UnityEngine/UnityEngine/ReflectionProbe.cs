using System;

using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Rendering;

namespace UnityEngine
{
	public sealed class ReflectionProbe : Behaviour
	{
		public ReflectionProbeType type
		{
			get;
			set;
		}

		public bool hdr
		{
			get;
			set;
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

		public Vector3 center
		{
			get
			{
				INTERNAL_get_center(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_center(ref value);
			}
		}

		public float nearClipPlane
		{
			get;
			set;
		}

		public float farClipPlane
		{
			get;
			set;
		}

		public float shadowDistance
		{
			get;
			set;
		}

		public int resolution
		{
			get;
			set;
		}

		public int cullingMask
		{
			get;
			set;
		}

		public ReflectionProbeClearFlags clearFlags
		{
			get;
			set;
		}

		public Color backgroundColor
		{
			get
			{
				INTERNAL_get_backgroundColor(out Color value);
				return value;
			}
			set
			{
				INTERNAL_set_backgroundColor(ref value);
			}
		}

		public float intensity
		{
			get;
			set;
		}

		public float blendDistance
		{
			get;
			set;
		}

		public bool boxProjection
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

		public ReflectionProbeMode mode
		{
			get;
			set;
		}

		public int importance
		{
			get;
			set;
		}

		public ReflectionProbeRefreshMode refreshMode
		{
			get;
			set;
		}

		public ReflectionProbeTimeSlicingMode timeSlicingMode
		{
			get;
			set;
		}

		public Texture bakedTexture
		{
			get;
			set;
		}

		public Texture customBakedTexture
		{
			get;
			set;
		}

		public Texture texture
		{
			get;
		}

		private void INTERNAL_get_size(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_size(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_center(out Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_center(ref Vector3 value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_backgroundColor(out Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_backgroundColor(ref Color value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_bounds(out Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int RenderProbe([DefaultValue("null")] RenderTexture targetTexture) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public int RenderProbe()
		{
			RenderTexture targetTexture = null;
			return RenderProbe(targetTexture);
		}

		public bool IsFinishedRendering(int renderId) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool BlendCubemap(Texture src, Texture dst, float blend, RenderTexture target) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
