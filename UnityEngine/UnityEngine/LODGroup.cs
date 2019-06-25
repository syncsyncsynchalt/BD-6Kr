using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class LODGroup : Component
	{
		public Vector3 localReferencePoint
		{
			get
			{
				INTERNAL_get_localReferencePoint(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_localReferencePoint(ref value);
			}
		}

		public float size
		{
			get;
			set;
		}

		public int lodCount
		{
			get;
		}

		public LODFadeMode fadeMode
		{
			get;
			set;
		}

		public bool animateCrossFading
		{
			get;
			set;
		}

		public bool enabled
		{
			get;
			set;
		}

		public static float crossFadeAnimationDuration
		{
			get;
			set;
		}

		private void INTERNAL_get_localReferencePoint(out Vector3 value) { throw new NotImplementedException("�Ȃɂ���"); }

		private void INTERNAL_set_localReferencePoint(ref Vector3 value) { throw new NotImplementedException("�Ȃɂ���"); }

		public void RecalculateBounds() { throw new NotImplementedException("�Ȃɂ���"); }

		public LOD[] GetLODs() { throw new NotImplementedException("�Ȃɂ���"); }

		[Obsolete("Use SetLODs instead.")]
		public void SetLODS(LOD[] lods)
		{
			SetLODs(lods);
		}

		public void SetLODs(LOD[] lods) { throw new NotImplementedException("�Ȃɂ���"); }

		public void ForceLOD(int index) { throw new NotImplementedException("�Ȃɂ���"); }
	}
}
