using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class CullingGroup : IDisposable
	{
		public delegate void StateChanged(CullingGroupEvent sphere);

		internal IntPtr m_Ptr;

		private StateChanged m_OnStateChanged;

		public StateChanged onStateChanged
		{
			get
			{
				return m_OnStateChanged;
			}
			set
			{
				m_OnStateChanged = value;
			}
		}

		public bool enabled
		{
			get;
			set;
		}

		public Camera targetCamera
		{
			get;
			set;
		}

		public CullingGroup()
		{
			Init();
		}

		~CullingGroup()
		{
			if (m_Ptr != IntPtr.Zero)
			{
				FinalizerFailure();
			}
		}

		public void Dispose() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetBoundingSpheres(BoundingSphere[] array) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetBoundingSphereCount(int count) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void EraseSwapBack(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static void EraseSwapBack<T>(int index, T[] myArray, ref int size)
		{
			size--;
			myArray[index] = myArray[size];
		}

		public int QueryIndices(bool visible, int[] result, int firstIndex)
		{
			return QueryIndices(visible, -1, CullingQueryOptions.IgnoreDistance, result, firstIndex);
		}

		public int QueryIndices(int distanceIndex, int[] result, int firstIndex)
		{
			return QueryIndices(visible: false, distanceIndex, CullingQueryOptions.IgnoreVisibility, result, firstIndex);
		}

		public int QueryIndices(bool visible, int distanceIndex, int[] result, int firstIndex)
		{
			return QueryIndices(visible, distanceIndex, CullingQueryOptions.Normal, result, firstIndex);
		}

		private int QueryIndices(bool visible, int distanceIndex, CullingQueryOptions options, int[] result, int firstIndex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public bool IsVisible(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public int GetDistance(int index) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetBoundingDistances(float[] distances) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetDistanceReferencePoint(Vector3 point)
		{
			INTERNAL_CALL_SetDistanceReferencePoint(this, ref point);
		}

		private static void INTERNAL_CALL_SetDistanceReferencePoint(CullingGroup self, ref Vector3 point) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void SetDistanceReferencePoint(Transform transform) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[SecuritySafeCritical]
		private unsafe static void SendEvents(CullingGroup cullingGroup, IntPtr eventsPtr, int count)
		{
			CullingGroupEvent* ptr = (CullingGroupEvent*)eventsPtr.ToPointer();
			if (cullingGroup.m_OnStateChanged != null)
			{
				for (int i = 0; i < count; i++)
				{
					cullingGroup.m_OnStateChanged(*(CullingGroupEvent*)((byte*)ptr + i * sizeof(CullingGroupEvent)));
				}
			}
		}

		private void Init() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void FinalizerFailure() { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
