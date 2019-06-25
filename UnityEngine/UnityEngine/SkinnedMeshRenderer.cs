using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class SkinnedMeshRenderer : Renderer
	{
		public Transform[] bones
		{
			get;
			set;
		}

		public Transform rootBone
		{
			get;
			set;
		}

		public SkinQuality quality
		{
			get;
			set;
		}

		public Mesh sharedMesh
		{
			get;
			set;
		}

		public bool updateWhenOffscreen
		{
			get;
			set;
		}

		public Bounds localBounds
		{
			get
			{
				INTERNAL_get_localBounds(out Bounds value);
				return value;
			}
			set
			{
				INTERNAL_set_localBounds(ref value);
			}
		}

		private void INTERNAL_get_localBounds(out Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_localBounds(ref Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void BakeMesh(Mesh mesh) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public float GetBlendShapeWeight(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetBlendShapeWeight(int index, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
