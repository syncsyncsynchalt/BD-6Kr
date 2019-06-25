using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[Serializable]
	public struct SortieDebugProperties
	{
		[Header("[Priority Properties]")]
		public Generics.BattleRootType rootType;

		public bool isCutInChk;

		public bool isSkipBattle;

		[Range(1f, 18f)]
		[Header("[SortieMap(Normal) Properties]")]
		public int sortieAreaID;

		[Range(1f, 7f)]
		public int sortieMapID;

		[Range(1f, 8f)]
		public int sortieDeckID;

		[Range(1f, 18f)]
		[Header("[SortieMap(Rebellion) Properties]")]
		public int rebellionAreaID;

		[Range(1f, 8f)]
		public int rebellionSubDeckID;

		[Range(1f, 8f)]
		public int rebellionMainDeckID;

		[Range(-1f, 8f)]
		public int rebellionSubSupportDeckID;

		[Range(-1f, 8f)]
		public int rebellionMainSupportDeckID;
	}
}
