using local.models;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_Paramerter : MonoBehaviour
	{
		[SerializeField]
		private UILabel[] ParamLabels;

		public void SetParams(ShipModel ship)
		{
			ParamLabels[0].textInt = ship.MaxHp;
			ParamLabels[1].textInt = ship.Karyoku;
			ParamLabels[2].textInt = ship.Soukou;
			ParamLabels[3].textInt = ship.Raisou;
			ParamLabels[4].textInt = ship.Kaihi;
			ParamLabels[5].textInt = ship.Taiku;
			ParamLabels[6].textInt = ship.TousaiMaxAll;
			ParamLabels[7].textInt = ship.Taisen;
			ParamLabels[8].text = GetSokuText(ship.Soku);
			ParamLabels[9].textInt = ship.Sakuteki;
			ParamLabels[10].text = GetLengText(ship.Leng);
			ParamLabels[11].textInt = ship.Lucky;
		}

		private string GetSokuText(int value)
		{
			if (value == 10)
			{
				return "高速";
			}
			return "低速";
		}

		private string GetLengText(int value)
		{
			switch (value)
			{
			case 0:
				return "無";
			case 1:
				return "短";
			case 2:
				return "中";
			case 3:
				return "長";
			case 4:
				return "超長";
			default:
				return string.Empty;
			}
		}

		private void OnDestroy()
		{
			ParamLabels = null;
		}
	}
}
