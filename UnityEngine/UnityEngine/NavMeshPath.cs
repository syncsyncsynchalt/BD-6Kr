using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class NavMeshPath
	{
		internal IntPtr m_Ptr;

		internal Vector3[] m_corners;

		public Vector3[] corners
		{
			get
			{
				CalculateCorners();
				return m_corners;
			}
		}

		public NavMeshPathStatus status
		{
			get;
		}

		public NavMeshPath() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void DestroyNavMeshPath() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		~NavMeshPath()
		{
			DestroyNavMeshPath();
			m_Ptr = IntPtr.Zero;
		}

		public int GetCornersNonAlloc(Vector3[] results) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private Vector3[] CalculateCornersInternal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void ClearCornersInternal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void ClearCorners()
		{
			ClearCornersInternal();
			m_corners = null;
		}

		private void CalculateCorners()
		{
			if (m_corners == null)
			{
				m_corners = CalculateCornersInternal();
			}
		}
	}
}
