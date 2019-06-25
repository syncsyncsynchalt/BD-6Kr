using Common.Enum;
using KCV.Utils;
using System.Collections;
using UnityEngine;

namespace KCV.Supply
{
	public class SupplyRightPane : MonoBehaviour
	{
		[SerializeField]
		private UILabel _fuelLabel;

		[SerializeField]
		private UILabel _ammoLabel;

		[SerializeField]
		private UIButton _fuelBtn;

		[SerializeField]
		private UIButton _ammoBtn;

		[SerializeField]
		private UIButton _allBtn;

		[SerializeField]
		private UITexture _window;

		private UIButton _currentBtn;

		[SerializeField]
		private UISupplyFuelIconManager _fuelSupplyIconManager;

		[SerializeField]
		private UISupplyAmmoIconManager _ammoSupplyIconManager;

		[SerializeField]
		private ButtonLightTexture[] btnLight = new ButtonLightTexture[3];

		public void Init()
		{
			_fuelBtn.enabled = false;
			_ammoBtn.enabled = false;
			_allBtn.enabled = false;
			_currentBtn = null;
			UpdateBtns();
		}

		public void SelectButtonLengthwise(bool isUp)
		{
			if (isUp)
			{
				if (_fuelBtn.enabled && _currentBtn == _allBtn)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					SetCurrentBtn(_fuelBtn);
				}
				else if (_ammoBtn.enabled && _currentBtn == _allBtn)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					SetCurrentBtn(_ammoBtn);
				}
			}
			else if (_currentBtn != _allBtn)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				if (_allBtn.enabled)
				{
					SetCurrentBtn(_allBtn);
				}
			}
		}

		public bool SelectButtonHorizontal(bool isLeft)
		{
			if (isLeft)
			{
				if (_currentBtn == _ammoBtn && _fuelBtn.enabled)
				{
					SetCurrentBtn(_fuelBtn);
				}
				else
				{
					SupplyMainManager.Instance.change_2_SHIP_SELECT(defaultFocus: true);
				}
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			else
			{
				if (!(_currentBtn == _fuelBtn) || !_ammoBtn.enabled)
				{
					return true;
				}
				SetCurrentBtn(_ammoBtn);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			return false;
		}

		public void DisideButton()
		{
			if (_currentBtn == _fuelBtn)
			{
				DoSupplyFuel();
			}
			else if (_currentBtn == _ammoBtn)
			{
				DoSupplyAmmo();
			}
			else if (_currentBtn == _allBtn)
			{
				DoSupplyAll();
			}
		}

		public void Refresh()
		{
			bool flag = SupplyMainManager.Instance.SupplyManager.IsValidSupply(SupplyType.Fuel);
			bool flag2 = SupplyMainManager.Instance.SupplyManager.IsValidSupply(SupplyType.Ammo);
			_fuelBtn.enabled = flag;
			_ammoBtn.enabled = flag2;
			_allBtn.enabled = SupplyMainManager.Instance.SupplyManager.IsValidSupply(SupplyType.All);
			UpdateBtns();
			int fuelForSupply = SupplyMainManager.Instance.SupplyManager.FuelForSupply;
			int ammoForSupply = SupplyMainManager.Instance.SupplyManager.AmmoForSupply;
			_fuelLabel.textInt = fuelForSupply;
			_ammoLabel.textInt = ammoForSupply;
			Color color = new Color(1f, 1f, 1f);
			Color color2 = new Color(1f, 0f, 0f);
			_fuelLabel.color = ((fuelForSupply != 0 && !flag) ? color2 : color);
			_ammoLabel.color = ((ammoForSupply != 0 && !flag2) ? color2 : color);
			_fuelSupplyIconManager.init(fuelForSupply);
			_ammoSupplyIconManager.init(ammoForSupply);
		}

		public void OnFuelBtnClick()
		{
			UpdateBtns();
			SetCurrentBtn(_fuelBtn);
			DoSupplyFuel();
		}

		public void OnAmmoBtnClick()
		{
			UpdateBtns();
			SetCurrentBtn(_ammoBtn);
			DoSupplyAmmo();
		}

		public void OnAllBtnClick()
		{
			UpdateBtns();
			SetCurrentBtn(_allBtn);
			DoSupplyAll();
		}

		private void DoSupplyFuel()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			_ammoLabel.text = "0";
			SupplyMainManager.Instance.change_2_SUPPLY_PROCESS(SupplyType.Fuel);
		}

		private void DoSupplyAmmo()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			_fuelLabel.text = "0";
			SupplyMainManager.Instance.change_2_SUPPLY_PROCESS(SupplyType.Ammo);
		}

		private void DoSupplyAll()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			Debug.Log("まとめて補給ボタン押下");
			SupplyMainManager.Instance.change_2_SUPPLY_PROCESS(SupplyType.All);
		}

		public void setFocus()
		{
			if (_allBtn.enabled)
			{
				SetCurrentBtn(_allBtn);
			}
			else if (_fuelBtn.enabled)
			{
				SetCurrentBtn(_fuelBtn);
			}
			else if (_ammoBtn.enabled)
			{
				SetCurrentBtn(_ammoBtn);
			}
		}

		public void LostFocus()
		{
			SupplyMainManager.Instance.SetControllDone(enabled: false);
			_currentBtn = null;
			UpdateBtns();
		}

		public void SetEnabled(bool enabled)
		{
			if (enabled)
			{
				_fuelBtn.enabled = SupplyMainManager.Instance.SupplyManager.IsValidSupply(SupplyType.Fuel);
				_ammoBtn.enabled = SupplyMainManager.Instance.SupplyManager.IsValidSupply(SupplyType.Ammo);
				_allBtn.enabled = SupplyMainManager.Instance.SupplyManager.IsValidSupply(SupplyType.All);
			}
			else
			{
				_fuelBtn.enabled = false;
				_ammoBtn.enabled = false;
				_allBtn.enabled = false;
			}
			UpdateBtns();
		}

		public bool IsFocusable()
		{
			return _allBtn.enabled || _fuelBtn.enabled || _ammoBtn.enabled;
		}

		private void SetCurrentBtn(UIButton btn)
		{
			UpdateBtns();
			_currentBtn = btn;
			_currentBtn.SetState(UIButtonColor.State.Hover, immediate: true);
		}

		private void UpdateBtns()
		{
			_allBtn.SetState((!_allBtn.enabled) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, immediate: true);
			_fuelBtn.SetState((!_fuelBtn.enabled) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, immediate: true);
			_ammoBtn.SetState((!_ammoBtn.enabled) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, immediate: true);
			if (_allBtn.enabled)
			{
				btnLight[0].PlayAnim();
			}
			else
			{
				btnLight[0].StopAnim();
			}
			if (_fuelBtn.enabled)
			{
				btnLight[1].PlayAnim();
			}
			else
			{
				btnLight[1].StopAnim();
			}
			if (_ammoBtn.enabled)
			{
				btnLight[2].PlayAnim();
			}
			else
			{
				btnLight[2].StopAnim();
			}
		}

		public void DoWindowOpenAnimation(SupplyType supplyType)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.3f);
			hashtable.Add("y", 199f);
			hashtable.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable.Add("isLocal", true);
			hashtable.Add("oncomplete", "OnCompleteOfWindowOpenAnimation");
			hashtable.Add("oncompletetarget", base.gameObject);
			hashtable.Add("oncompleteparams", supplyType);
			_window.transform.MoveTo(hashtable);
		}

		private void OnCompleteOfWindowOpenAnimation(SupplyType supplyType)
		{
			switch (supplyType)
			{
			case SupplyType.Fuel:
				_fuelSupplyIconManager.ProcessConsumingAnimation();
				_ammoSupplyIconManager.ProcessCancelAnimation();
				break;
			case SupplyType.Ammo:
				_fuelSupplyIconManager.ProcessCancelAnimation();
				_ammoSupplyIconManager.ProcessConsumingAnimation();
				break;
			case SupplyType.All:
				_ammoSupplyIconManager.ProcessConsumingAnimation();
				_fuelSupplyIconManager.ProcessConsumingAnimation();
				break;
			}
		}

		public void DoWindowCloseAnimation()
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.6f);
			hashtable.Add("y", 0f);
			hashtable.Add("easeType", iTween.EaseType.easeInOutSine);
			hashtable.Add("isLocal", true);
			hashtable.Add("oncomplete", "OnCompleteOfRecoveryAnimation");
			hashtable.Add("oncompletetarget", base.gameObject);
			_window.transform.MoveTo(hashtable);
		}

		public void OnCompleteOfRecoveryAnimation()
		{
			SupplyMainManager.Instance.ProcessSupplyFinished();
		}
	}
}
