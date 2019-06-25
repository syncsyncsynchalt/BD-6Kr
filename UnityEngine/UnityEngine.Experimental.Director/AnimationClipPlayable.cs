using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.Director
{
	public sealed class AnimationClipPlayable : AnimationPlayable
	{
		public AnimationClip clip
		{
			get;
		}

		public float speed
		{
			get;
			set;
		}

		public AnimationClipPlayable(AnimationClip clip)
			: base(final: false)
		{
			m_Ptr = IntPtr.Zero;
			InstantiateEnginePlayable(clip);
		}

		private void InstantiateEnginePlayable(AnimationClip clip) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public override int AddInput(AnimationPlayable source)
		{
			Debug.LogError("AnimationClipPlayable doesn't support adding inputs");
			return -1;
		}

		public override bool SetInput(AnimationPlayable source, int index)
		{
			Debug.LogError("AnimationClipPlayable doesn't support setting inputs");
			return false;
		}

		public override bool SetInputs(IEnumerable<AnimationPlayable> sources)
		{
			Debug.LogError("AnimationClipPlayable doesn't support setting inputs");
			return false;
		}

		public override bool RemoveInput(int index)
		{
			Debug.LogError("AnimationClipPlayable doesn't support removing inputs");
			return false;
		}

		public override bool RemoveInput(AnimationPlayable playable)
		{
			Debug.LogError("AnimationClipPlayable doesn't support removing inputs");
			return false;
		}

		public override bool RemoveAllInputs()
		{
			Debug.LogError("AnimationClipPlayable doesn't support removing inputs");
			return false;
		}
	}
}
