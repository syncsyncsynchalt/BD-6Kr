using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public struct SortingLayer
	{
		private int m_Id;

		public int id => m_Id;

		public string name => IDToName(m_Id);

		public int value => GetLayerValueFromID(m_Id);

		public static SortingLayer[] layers
		{
			get
			{
				int[] sortingLayerIDsInternal = GetSortingLayerIDsInternal();
				SortingLayer[] array = new SortingLayer[sortingLayerIDsInternal.Length];
				for (int i = 0; i < sortingLayerIDsInternal.Length; i++)
				{
					array[i].m_Id = sortingLayerIDsInternal[i];
				}
				return array;
			}
		}

		private static int[] GetSortingLayerIDsInternal() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int GetLayerValueFromID(int id) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int GetLayerValueFromName(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static int NameToID(string name) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static string IDToName(int id) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public static bool IsValid(int id) { throw new NotImplementedException("‚È‚É‚±‚ê"); }
	}
}
