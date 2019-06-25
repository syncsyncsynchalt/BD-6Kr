using Common.Enum;
using local.models;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class DeckSortieInfoManager : MonoBehaviour
	{
		[SerializeField]
		private DeckSortieInfo[] deckSortieInfos;

		public int Init(List<DeckModel> areaDecks)
		{
			List<DeckModel> list = new List<DeckModel>();
			List<IsGoCondition> list2 = new List<IsGoCondition>();
			for (int i = 0; i < areaDecks.Count; i++)
			{
				List<IsGoCondition> list3 = areaDecks[i].IsValidSortie();
				if (0 < list3.Count)
				{
					list.Add(areaDecks[i]);
					list2.Add(list3[0]);
				}
			}
			deckSortieInfos = new DeckSortieInfo[9];
			for (int j = 1; j < deckSortieInfos.Length; j++)
			{
				deckSortieInfos[j] = ((Component)base.transform.FindChild("DeckSortieInfo" + j)).GetComponent<DeckSortieInfo>();
			}
			for (int k = 1; k < deckSortieInfos.Length; k++)
			{
				if (k < list.Count + 1)
				{
					deckSortieInfos[k].SetActive(isActive: true);
					deckSortieInfos[k].SetDeckInfo(list[k - 1], list2[k - 1]);
				}
				else
				{
					deckSortieInfos[k].SetActive(isActive: false);
				}
			}
			return list.Count;
		}

		public List<DeckModel> GetSortieEnableDeck(List<DeckModel> areaDecks)
		{
			List<DeckModel> list = new List<DeckModel>();
			for (int i = 0; i < areaDecks.Count; i++)
			{
				List<IsGoCondition> list2 = areaDecks[i].IsValidSortie();
				if (list2.Count == 0)
				{
					list.Add(areaDecks[i]);
				}
			}
			return list;
		}
	}
}
