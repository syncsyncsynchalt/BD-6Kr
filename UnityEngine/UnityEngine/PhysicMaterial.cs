using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class PhysicMaterial : Object
	{
		public float dynamicFriction
		{
			get;
			set;
		}

		public float staticFriction
		{
			get;
			set;
		}

		public float bounciness
		{
			get;
			set;
		}

		[Obsolete("Use PhysicMaterial.bounciness instead", true)]
		public float bouncyness
		{
			get
			{
				return bounciness;
			}
			set
			{
				bounciness = value;
			}
		}

		[Obsolete("Anisotropic friction is no longer supported since Unity 5.0.", true)]
		public Vector3 frictionDirection2
		{
			get
			{
				return Vector3.zero;
			}
			set
			{
			}
		}

		[Obsolete("Anisotropic friction is no longer supported since Unity 5.0.", true)]
		public float dynamicFriction2
		{
			get;
			set;
		}

		[Obsolete("Anisotropic friction is no longer supported since Unity 5.0.", true)]
		public float staticFriction2
		{
			get;
			set;
		}

		public PhysicMaterialCombine frictionCombine
		{
			get;
			set;
		}

		public PhysicMaterialCombine bounceCombine
		{
			get;
			set;
		}

		[Obsolete("Anisotropic friction is no longer supported since Unity 5.0.", true)]
		public Vector3 frictionDirection
		{
			get
			{
				return Vector3.zero;
			}
			set
			{
			}
		}

		public PhysicMaterial()
		{
			Internal_CreateDynamicsMaterial(this, null);
		}

		public PhysicMaterial(string name)
		{
			Internal_CreateDynamicsMaterial(this, name);
		}

		private static void Internal_CreateDynamicsMaterial([Writable] PhysicMaterial mat, string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
