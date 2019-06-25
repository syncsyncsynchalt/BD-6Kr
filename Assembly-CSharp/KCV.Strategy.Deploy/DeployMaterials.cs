using Common.Enum;
using local.managers;
using local.utils;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Deploy
{
	public class DeployMaterials : MonoBehaviour
	{
		[SerializeField]
		private UILabel[] MaterialNums;

		public void setMaterials(int[] Nums)
		{
			for (int i = 0; i < MaterialNums.Length; i++)
			{
				MaterialNums[i].text = Nums[i].ToString();
			}
		}

		public void updateMaterials(int areaID, int tankerCount, EscortDeckManager manager)
		{
			Dictionary<enumMaterialCategory, int> areaResource = local.utils.Utils.GetAreaResource(areaID, tankerCount, manager);
			MaterialNums[0].text = areaResource[enumMaterialCategory.Fuel].ToString();
			MaterialNums[1].text = areaResource[enumMaterialCategory.Steel].ToString();
			MaterialNums[2].text = areaResource[enumMaterialCategory.Bull].ToString();
			MaterialNums[3].text = areaResource[enumMaterialCategory.Bauxite].ToString();
		}
	}
}
