using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class AnimationCurve
	{
		internal IntPtr m_Ptr;

		public Keyframe[] keys
		{
			get
			{
				return GetKeys();
			}
			set
			{
				SetKeys(value);
			}
		}

		public Keyframe this[int index] => GetKey_Internal(index);

		public int length
		{
			get;
		}

		public WrapMode preWrapMode
		{
			get;
			set;
		}

		public WrapMode postWrapMode
		{
			get;
			set;
		}

		public AnimationCurve(params Keyframe[] keys)
		{
			Init(keys);
		}

		public AnimationCurve()
		{
			Init(null);
		}

		private void Cleanup() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~AnimationCurve()
		{
			Cleanup();
		}

		public float Evaluate(float time) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int AddKey(float time, float value) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int AddKey(Keyframe key)
		{
			return AddKey_Internal(key);
		}

		private int AddKey_Internal(Keyframe key)
		{
			return INTERNAL_CALL_AddKey_Internal(this, ref key);
		}

		private static int INTERNAL_CALL_AddKey_Internal(AnimationCurve self, ref Keyframe key) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int MoveKey(int index, Keyframe key)
		{
			return INTERNAL_CALL_MoveKey(this, index, ref key);
		}

		private static int INTERNAL_CALL_MoveKey(AnimationCurve self, int index, ref Keyframe key) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void RemoveKey(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void SetKeys(Keyframe[] keys) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private Keyframe GetKey_Internal(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private Keyframe[] GetKeys() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SmoothTangents(int index, float weight) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static AnimationCurve Linear(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			float num = (valueEnd - valueStart) / (timeEnd - timeStart);
			Keyframe[] keys = new Keyframe[2]
			{
				new Keyframe(timeStart, valueStart, 0f, num),
				new Keyframe(timeEnd, valueEnd, num, 0f)
			};
			return new AnimationCurve(keys);
		}

		public static AnimationCurve EaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			Keyframe[] keys = new Keyframe[2]
			{
				new Keyframe(timeStart, valueStart, 0f, 0f),
				new Keyframe(timeEnd, valueEnd, 0f, 0f)
			};
			return new AnimationCurve(keys);
		}

		private void Init(Keyframe[] keys) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
