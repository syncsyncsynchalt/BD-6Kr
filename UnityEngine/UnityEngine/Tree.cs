using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Tree : Component
	{
		public ScriptableObject data
		{
			get;
			set;
		}

		public bool hasSpeedTreeWind
		{
			get;
		}
	}
}
