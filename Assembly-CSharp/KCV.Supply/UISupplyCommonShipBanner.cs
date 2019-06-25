using Common.Enum;
using local.models;
using UnityEngine;

namespace KCV.Supply
{
	public abstract class UISupplyCommonShipBanner : MonoBehaviour
	{
		[SerializeField]
		private UILabel _shipName;

		[SerializeField]
		private UILabel _shipLevel;

		[SerializeField]
		private UILabel _shipTaikyu;

		[SerializeField]
		private UISprite _shipGauge;

		[SerializeField]
		private UISprite _shipGaugeFrame;

		[SerializeField]
		private UISprite _checkBox;

		[SerializeField]
		private UISprite _check;

		[SerializeField]
		private UITexture _backGround;

		[SerializeField]
		private SupplyStorage _ammoStorage;

		[SerializeField]
		private SupplyStorage _fuelStorage;

		[SerializeField]
		private UITexture _waveAnime;

		public int idx;

		public bool selected;

		public ShipModel Ship;

		[Button("ProcessRecoveryAnimation", "ProcessRecoveryAnimation", new object[]
		{

		})]
		public int button1;

		public iTween.EaseType type;

		public bool Init()
		{
			selected = false;
			return true;
		}

		public virtual void SetBanner(ShipModel ship, int idx)
		{
			SetEnabled((ship != null) ? true : false);
			Ship = ship;
			this.idx = idx;
			selected = false;
			if (base.enabled)
			{
				UpdateCheckBoxBackground();
				UpdateCheckBox();
				_checkBox.SetActive(isActive: true);
				_check.SetActive(isActive: false);
				_shipName.text = Ship.Name;
				_shipLevel.text = "Lv" + Ship.Level;
				_shipTaikyu.text = Ship.NowHp + "/" + Ship.MaxHp;
				float num = (float)Ship.NowHp / (float)Ship.MaxHp;
				float num2 = (float)_shipGaugeFrame.width * num;
				_shipGauge.width = (int)num2;
				_shipGauge.alpha = 1f;
				_shipGaugeFrame.alpha = 1f;
				_shipGauge.color = Util.HpGaugeColor2(ship.MaxHp, ship.NowHp);
				int num3 = 0;
				for (int i = 0; i < 10 && ship.Ammo > ship.AmmoMax / 5 * i; i++)
				{
					num3++;
				}
				_ammoStorage.init(num3, 0.2f);
				int num4 = 0;
				for (int j = 0; j < 5 && ship.Fuel > ship.FuelMax / 5 * j; j++)
				{
					num4++;
				}
				_fuelStorage.init(num4, 0.2f);
				UpdateCheckBoxBackground();
			}
		}

		public void BannerAnimation(bool isShow)
		{
			Animation component = GetComponent<Animation>();
			component.Stop();
			if (isShow)
			{
                component["SupplyBanner"].time = 0f;
				component.Play("SupplyBanner");
			}
			else
			{
				component["SupplyShutterIn"].time = 0f;
				component.Play("SupplyShutterIn");
			}
		}

		public virtual void SetEnabled(bool enabled)
		{
			base.enabled = enabled;
			_checkBox.SetActive(enabled);
			_check.SetActive(enabled);
			_shipName.SetActive(enabled);
			_shipLevel.SetActive(enabled);
			_shipTaikyu.SetActive(enabled);
			_shipGauge.SetActive(enabled);
			_shipGaugeFrame.SetActive(enabled);
			_fuelStorage.SetActive(enabled);
			_ammoStorage.SetActive(enabled);
			if (!enabled)
			{
				_fuelStorage.init(0, 0.5f);
				_ammoStorage.init(0, 0.5f);
			}
		}

		public void Hover(bool enabled)
		{
			UISelectedObject.SelectedOneObjectBlink(_backGround.gameObject, enabled);
		}

		private void UpdateCheckBoxBackground()
		{
			_checkBox.spriteName = ((!IsSelectable()) ? "check_bg" : "check_bg_on");
		}

		private void UpdateCheckBox()
		{
			_check.transform.SetActive(selected);
		}

		public void Select(bool selected)
		{
			this.selected = selected;
			UpdateCheckBox();
		}

		public bool SwitchSelected()
		{
			selected = !selected;
			UpdateCheckBox();
			SupplyMainManager.Instance.SupplyManager.ClickCheckBox(Ship.MemId);
			return selected;
		}

		public void ProcessRecoveryAnimation()
		{
			_waveAnime.transform.localPosition = new Vector3(-360f, 4f, 0f);
			_waveAnime.alpha = 1f;
			iTween.MoveTo(_waveAnime.gameObject, iTween.Hash("position", new Vector3(250f, 0f, 0f), "islocal", true, "time", 0.39f, "easetype", type, "looptype", iTween.LoopType.none, "oncomplete", "OnCompleteOfRecoveryAnimation", "oncompletetarget", base.gameObject));
			TweenAlpha.Begin(_waveAnime.gameObject, 0.39f, 0.5f);
		}

		public void OnCompleteOfRecoveryAnimation()
		{
			iTween.Stop(_waveAnime.gameObject);
			_waveAnime.alpha = 0f;
		}

		public bool IsSelectable()
		{
			if (SupplyMainManager.Instance.SupplyManager.CheckBoxStates == null)
			{
				DebugUtils.SLog("SupplyManager.CheckBoxStates == null occured!!!!");
				return false;
			}
			return base.enabled && SupplyMainManager.Instance.SupplyManager.CheckBoxStates[idx] != CheckBoxStatus.DISABLE;
		}

		private bool IsAmmoNotFull()
		{
			return Ship.Ammo != Ship.AmmoMax;
		}

		private bool IsFuelNotFull()
		{
			return Ship.Fuel != Ship.FuelMax;
		}
	}
}
