using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class GUILayer : Behaviour
	{
		public GUIElement HitTest(Vector3 screenPosition)
		{
			return INTERNAL_CALL_HitTest(this, ref screenPosition);
		}

		private static GUIElement INTERNAL_CALL_HitTest(GUILayer self, ref Vector3 screenPosition) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
