using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class PhysicsMaterial2D : Object
	{
		public float bounciness
		{
			get;
			set;
		}

		public float friction
		{
			get;
			set;
		}

		public PhysicsMaterial2D()
		{
			Internal_Create(this, null);
		}

		public PhysicsMaterial2D(string name)
		{
			Internal_Create(this, name);
		}

		private static void Internal_Create([Writable] PhysicsMaterial2D mat, string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
