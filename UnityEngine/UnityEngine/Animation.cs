using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class Animation : Behaviour, IEnumerable
	{
		private sealed class Enumerator : IEnumerator
		{
			private Animation m_Outer;

			private int m_CurrentIndex = -1;

			public object Current => m_Outer.GetStateAtIndex(m_CurrentIndex);

			internal Enumerator(Animation outer)
			{
				m_Outer = outer;
			}

			public bool MoveNext()
			{
				int stateCount = m_Outer.GetStateCount();
				m_CurrentIndex++;
				return m_CurrentIndex < stateCount;
			}

			public void Reset()
			{
				m_CurrentIndex = -1;
			}
		}

		public AnimationClip clip
		{
			get;
			set;
		}

		public bool playAutomatically
		{
			get;
			set;
		}

		public WrapMode wrapMode
		{
			get;
			set;
		}

		public bool isPlaying
		{
			get;
		}

		public AnimationState this[string name] => GetState(name);

		public bool animatePhysics
		{
			get;
			set;
		}

		[Obsolete("Use cullingType instead")]
		public bool animateOnlyIfVisible
		{
			get;
			set;
		}

		public AnimationCullingType cullingType
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

		public void Stop()
		{
			INTERNAL_CALL_Stop(this);
		}

		private static void INTERNAL_CALL_Stop(Animation self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Stop(string name)
		{
			Internal_StopByName(name);
		}

		private void Internal_StopByName(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Rewind(string name)
		{
			Internal_RewindByName(name);
		}

		private void Internal_RewindByName(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Rewind()
		{
			INTERNAL_CALL_Rewind(this);
		}

		private static void INTERNAL_CALL_Rewind(Animation self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void Sample()
		{
			INTERNAL_CALL_Sample(this);
		}

		private static void INTERNAL_CALL_Sample(Animation self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsPlaying(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public bool Play()
		{
			PlayMode mode = PlayMode.StopSameLayer;
			return Play(mode);
		}

		public bool Play([DefaultValue("PlayMode.StopSameLayer")] PlayMode mode)
		{
			return PlayDefaultAnimation(mode);
		}

		public bool Play(string animation, [DefaultValue("PlayMode.StopSameLayer")] PlayMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public bool Play(string animation)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			return Play(animation, mode);
		}

		public void CrossFade(string animation, [DefaultValue("0.3F")] float fadeLength, [DefaultValue("PlayMode.StopSameLayer")] PlayMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void CrossFade(string animation, float fadeLength)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			CrossFade(animation, fadeLength, mode);
		}

		[ExcludeFromDocs]
		public void CrossFade(string animation)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			float fadeLength = 0.3f;
			CrossFade(animation, fadeLength, mode);
		}

		public void Blend(string animation, [DefaultValue("1.0F")] float targetWeight, [DefaultValue("0.3F")] float fadeLength) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void Blend(string animation, float targetWeight)
		{
			float fadeLength = 0.3f;
			Blend(animation, targetWeight, fadeLength);
		}

		[ExcludeFromDocs]
		public void Blend(string animation)
		{
			float fadeLength = 0.3f;
			float targetWeight = 1f;
			Blend(animation, targetWeight, fadeLength);
		}

		public AnimationState CrossFadeQueued(string animation, [DefaultValue("0.3F")] float fadeLength, [DefaultValue("QueueMode.CompleteOthers")] QueueMode queue, [DefaultValue("PlayMode.StopSameLayer")] PlayMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public AnimationState CrossFadeQueued(string animation, float fadeLength, QueueMode queue)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			return CrossFadeQueued(animation, fadeLength, queue, mode);
		}

		[ExcludeFromDocs]
		public AnimationState CrossFadeQueued(string animation, float fadeLength)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			QueueMode queue = QueueMode.CompleteOthers;
			return CrossFadeQueued(animation, fadeLength, queue, mode);
		}

		[ExcludeFromDocs]
		public AnimationState CrossFadeQueued(string animation)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			QueueMode queue = QueueMode.CompleteOthers;
			float fadeLength = 0.3f;
			return CrossFadeQueued(animation, fadeLength, queue, mode);
		}

		public AnimationState PlayQueued(string animation, [DefaultValue("QueueMode.CompleteOthers")] QueueMode queue, [DefaultValue("PlayMode.StopSameLayer")] PlayMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public AnimationState PlayQueued(string animation, QueueMode queue)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			return PlayQueued(animation, queue, mode);
		}

		[ExcludeFromDocs]
		public AnimationState PlayQueued(string animation)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			QueueMode queue = QueueMode.CompleteOthers;
			return PlayQueued(animation, queue, mode);
		}

		public void AddClip(AnimationClip clip, string newName)
		{
			AddClip(clip, newName, int.MinValue, int.MaxValue);
		}

		public void AddClip(AnimationClip clip, string newName, int firstFrame, int lastFrame, [DefaultValue("false")] bool addLoopFrame) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public void AddClip(AnimationClip clip, string newName, int firstFrame, int lastFrame)
		{
			bool addLoopFrame = false;
			AddClip(clip, newName, firstFrame, lastFrame, addLoopFrame);
		}

		public void RemoveClip(AnimationClip clip) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RemoveClip(string clipName)
		{
			RemoveClip2(clipName);
		}

		public int GetClipCount() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void RemoveClip2(string clipName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private bool PlayDefaultAnimation(PlayMode mode) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("use PlayMode instead of AnimationPlayMode.")]
		public bool Play(AnimationPlayMode mode)
		{
			return PlayDefaultAnimation((PlayMode)mode);
		}

		[Obsolete("use PlayMode instead of AnimationPlayMode.")]
		public bool Play(string animation, AnimationPlayMode mode)
		{
			return Play(animation, (PlayMode)mode);
		}

		public void SyncLayer(int layer)
		{
			INTERNAL_CALL_SyncLayer(this, layer);
		}

		private static void INTERNAL_CALL_SyncLayer(Animation self, int layer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public IEnumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		internal AnimationState GetState(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal AnimationState GetStateAtIndex(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal int GetStateCount() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public AnimationClip GetClip(string name)
		{
			AnimationState state = GetState(name);
			if ((bool)state)
			{
				return state.clip;
			}
			return null;
		}

		private void INTERNAL_get_localBounds(out Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_localBounds(ref Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
