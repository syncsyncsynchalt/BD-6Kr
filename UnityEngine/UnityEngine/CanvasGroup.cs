using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class CanvasGroup : Component, ICanvasRaycastFilter
	{
		public float alpha
		{
			get;
			set;
		}

		public bool interactable
		{
			get;
			set;
		}

		public bool blocksRaycasts
		{
			get;
			set;
		}

		public bool ignoreParentGroups
		{
			get;
			set;
		}

		public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			return blocksRaycasts;
		}
	}
}
