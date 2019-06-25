using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class MeshCollider : Collider
	{
		public Mesh sharedMesh
		{
			get;
			set;
		}

		public bool convex
		{
			get;
			set;
		}

		[Obsolete("Configuring smooth sphere collisions is no longer needed. PhysX3 has a better behaviour in place.")]
		public bool smoothSphereCollisions
		{
			get
			{
				return true;
			}
			set
			{
			}
		}
	}
}
