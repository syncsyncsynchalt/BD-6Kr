using local.models;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_Status : MonoBehaviour
	{
		[SerializeField]
		private UILabel ShipNameLabel;

		[SerializeField]
		private UILabel LevelLabel;

		[SerializeField]
		private UILabel HPLabel;

		[SerializeField]
		private UISprite HPGauge;

		[SerializeField]
		private UISprite EXPGauge;

		[SerializeField]
		private Transform StarParent;

		private UISprite[] _uiStar;

		public void SetStatus(ShipModel ship)
		{
			ShipNameLabel.text = ship.Name;
			LevelLabel.textInt = ship.Level;
			HPLabel.text = (ship.NowHp + "/" + ship.MaxHp).ToString();
			SetHPGauge(ship);
			EXPGauge.fillAmount = (float)ship.Exp_Percentage / 100f;
			SetStar(ship.Srate);
		}

		private void SetHPGauge(ShipModel ship)
		{
			float fillAmount = (float)ship.NowHp / (float)ship.MaxHp;
			HPGauge.fillAmount = fillAmount;
			HPGauge.color = Util.HpGaugeColor2(ship.MaxHp, ship.NowHp);
		}

		public void SetStar(int StarNum)
		{
			_uiStar = new UISprite[5];
			for (int i = 0; i < 5; i++)
			{
				_uiStar[i] = ((Component)StarParent.transform.FindChild("Star" + (i + 1))).GetComponent<UISprite>();
				if (i <= StarNum)
				{
					_uiStar[i].spriteName = "star_on";
				}
				else
				{
					_uiStar[i].spriteName = "star";
				}
			}
		}

		private void OnDestroy()
		{
			ShipNameLabel = null;
			LevelLabel = null;
			HPLabel = null;
			HPGauge = null;
			EXPGauge = null;
			StarParent = null;
			_uiStar = null;
		}
	}
}
