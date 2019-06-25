using Common.Struct;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyMonthWeekBonus : MonoBehaviour
	{
		[SerializeField]
		private UILabel Title;

		[SerializeField]
		private UILabel MonthName;

		[SerializeField]
		private UILabel[] MaterialNums;

		[SerializeField]
		private UISprite MaterialIcon;

		public void SetLabels(string monthName, MaterialInfo materialInfo)
		{
			Title.text = ((monthName.Length != 2) ? "新しい月、\u3000\u3000\u3000\u3000となりました！" : "新しい月、\u3000\u3000\u3000となりました！");
			MonthName.text = monthName;
			UILabel[] materialNums = MaterialNums;
			foreach (UILabel uILabel in materialNums)
			{
				uILabel.transform.parent.SetActive(isActive: true);
			}
			MaterialNums[0].text = " × " + materialInfo.Fuel.ToString();
			MaterialNums[1].text = " × " + materialInfo.Ammo.ToString();
			MaterialNums[2].text = " × " + materialInfo.Steel.ToString();
			MaterialNums[3].text = " × " + materialInfo.Baux.ToString();
			MaterialNums[4].text = " × " + materialInfo.Devkit.ToString();
		}

		public void SetLabelsWeek(MaterialInfo materialInfo)
		{
			int num = 0;
			int num2 = 0;
			if (materialInfo.Fuel > 0)
			{
				num = 1;
				num2 = materialInfo.Fuel;
			}
			else if (materialInfo.Ammo > 0)
			{
				num = 2;
				num2 = materialInfo.Ammo;
			}
			else if (materialInfo.Steel > 0)
			{
				num = 3;
				num2 = materialInfo.Steel;
			}
			MaterialNums[0].text = " × " + num2.ToString();
			MaterialIcon.spriteName = "icon2_m" + num;
		}
	}
}
