using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AnimationClip : Motion
	{
		public float length
		{
			get;
		}

		internal float startTime
		{
			get;
		}

		internal float stopTime
		{
			get;
		}

		public float frameRate
		{
			get;
			set;
		}

		public WrapMode wrapMode
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

		public new bool legacy
		{
			get;
			set;
		}

		public bool humanMotion
		{
			get;
		}

		public AnimationEvent[] events
		{
			get
			{
				return (AnimationEvent[])GetEventsInternal();
			}
			set
			{
				SetEventsInternal(value);
			}
		}

		public AnimationClip()
		{
			Internal_CreateAnimationClip(this);
		}

		public void SampleAnimation(GameObject go, float time) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private static void Internal_CreateAnimationClip([Writable] AnimationClip self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetCurve(string relativePath, Type type, string propertyName, AnimationCurve curve) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void EnsureQuaternionContinuity()
		{
			INTERNAL_CALL_EnsureQuaternionContinuity(this);
		}

		private static void INTERNAL_CALL_EnsureQuaternionContinuity(AnimationClip self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ClearCurves()
		{
			INTERNAL_CALL_ClearCurves(this);
		}

		private static void INTERNAL_CALL_ClearCurves(AnimationClip self) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_get_localBounds(out Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void INTERNAL_set_localBounds(ref Bounds value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void AddEvent(AnimationEvent evt)
		{
			AddEventInternal(evt);
		}

		internal void AddEventInternal(object evt) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal void SetEventsInternal(Array value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal Array GetEventsInternal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
