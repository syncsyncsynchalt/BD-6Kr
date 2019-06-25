using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public struct LayerMask
	{
		private int m_Mask;

		public int value
		{
			get
			{
				return m_Mask;
			}
			set
			{
				m_Mask = value;
			}
		}

		public static string LayerToName(int layer) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int NameToLayer(string layerName) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int GetMask(params string[] layerNames)
		{
			int num = 0;
			foreach (string layerName in layerNames)
			{
				int num2 = NameToLayer(layerName);
				if (num2 != 0)
				{
					num |= 1 << (num2 & 0x1F);
				}
			}
			return num;
		}

		public static implicit operator int(LayerMask mask)
		{
			return mask.m_Mask;
		}

		public static implicit operator LayerMask(int intVal)
		{
			LayerMask result = default(LayerMask);
			result.m_Mask = intVal;
			return result;
		}
	}
}
