using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public struct AnimatorClipInfo
	{
		private int m_ClipInstanceID;

		private float m_Weight;

		public AnimationClip clip => (m_ClipInstanceID == 0) ? null : ClipInstanceToScriptingObject(m_ClipInstanceID);

		public float weight => m_Weight;

		private static AnimationClip ClipInstanceToScriptingObject(int instanceID) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
