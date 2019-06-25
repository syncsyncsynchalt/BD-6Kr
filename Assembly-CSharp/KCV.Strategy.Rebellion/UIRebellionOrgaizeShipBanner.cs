using local.models;
using System;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class UIRebellionOrgaizeShipBanner : CommonShipBanner
	{
		[SerializeField]
		private UISprite _uiBackground;

		[SerializeField]
		private UILabel _uiIndex;

		[SerializeField]
		private UISlider _uiHpSlider;

		[SerializeField]
		private UILabel _uiLv;

		[SerializeField]
		private UILabel _uiName;

		[SerializeField]
		private CommonShipSupplyState _uiSupplyState;

		[SerializeField]
		private UiStarManager _uiStarManager;

		[SerializeField]
		private BannerShutter _bunnerShutter;

		[SerializeField]
		private GameObject _shipState;

		private int _nIndex;

		private Action<int> OnClickDelegate;

		public int index => _nIndex;

		public static UIRebellionOrgaizeShipBanner Instantiate(UIRebellionOrgaizeShipBanner prefab, Transform parent, int nIndex)
		{
			UIRebellionOrgaizeShipBanner uIRebellionOrgaizeShipBanner = UnityEngine.Object.Instantiate(prefab);
			uIRebellionOrgaizeShipBanner.transform.parent = parent;
			uIRebellionOrgaizeShipBanner.transform.localScaleOne();
			uIRebellionOrgaizeShipBanner.transform.localPositionZero();
			uIRebellionOrgaizeShipBanner.Setup(nIndex);
			return uIRebellionOrgaizeShipBanner;
		}

		private bool Setup(int nIndex)
		{
			Awake();
			if (_uiBackground == null)
			{
				Util.FindParentToChild(ref _uiBackground, base.transform, "Background");
			}
			if (_uiIndex == null)
			{
				Util.FindParentToChild(ref _uiIndex, base.transform, "Index");
			}
			if (_uiHpSlider == null)
			{
				Util.FindParentToChild(ref _uiHpSlider, base.transform, "HPSlider");
			}
			if (_uiLv == null)
			{
				Util.FindParentToChild(ref _uiLv, base.transform, "Lv");
			}
			if (_uiName == null)
			{
				Util.FindParentToChild(ref _uiName, base.transform, "Name");
			}
			if (_uiSupplyState == null)
			{
				Util.FindParentToChild(ref _uiSupplyState, base.transform, "Materials");
			}
			if (_bunnerShutter == null)
			{
				Transform transform = base.transform.FindChild("BannerShutter");
				if (transform != null)
				{
					_bunnerShutter = ((Component)transform).GetComponent<BannerShutter>();
				}
			}
			_uiStarManager.init(0);
			_nIndex = nIndex;
			return true;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiIndex);
			Mem.Del(ref _uiHpSlider);
			Mem.Del(ref _uiLv);
			Mem.Del(ref _uiName);
			Mem.Del(ref _uiSupplyState);
			Mem.Del(ref _uiStarManager);
			Mem.Del(ref _nIndex);
			Mem.Del(ref _bunnerShutter);
			Mem.Del(ref _shipState);
		}

		public void SetShipData(ShipModel model, int nIndex)
		{
			base.SetShipData(model);
			if (model == null)
			{
				_uiStarManager.SetStar(0);
				if (_bunnerShutter != null)
				{
					_bunnerShutter.SetActive(isActive: true);
					if (_shipState != null)
					{
						UISelectedObject.SelectedOneObjectBlink(_uiBackground.gameObject, value: false);
						_shipState.SetActive(false);
					}
				}
				else
				{
					base.transform.localScaleZero();
				}
				return;
			}
			if (_shipState != null)
			{
				_shipState.SetActive(true);
			}
			base.transform.localScaleOne();
			if (_bunnerShutter != null)
			{
				_bunnerShutter.SetFocusLight(isEnable: false);
				_bunnerShutter.SetActive(isActive: false);
			}
			_uiIndex.textInt = nIndex;
			_uiHpSlider.value = Mathe.Rate(0f, model.MaxHp, model.NowHp);
			_uiHpSlider.foregroundWidget.color = Util.HpLabelColor(model.MaxHp, model.NowHp);
			_uiLv.textInt = model.Level;
			_uiName.text = model.Name;
			_uiSupplyState.setSupplyState(model);
			_uiSupplyState.SetActive(_uiSupplyState.isEitherSupplyNeeds);
			_uiStarManager.SetStar(model.Srate);
		}

		public void SetShipIndex(int index)
		{
			_nIndex = index;
		}

		public void SetFocus(bool isEnable)
		{
			if (_bunnerShutter != null && _clsShipModel == null)
			{
				_bunnerShutter.SetFocusLight(isEnable);
			}
			else
			{
				UISelectedObject.SelectedOneObjectBlink(_uiBackground.gameObject, isEnable);
			}
		}

		public void UnsetFocus()
		{
			if (_bunnerShutter != null)
			{
				_bunnerShutter.SetFocusLight(isEnable: false);
			}
			UISelectedObject.SelectedOneObjectBlink(_uiBackground.gameObject, value: false);
		}

		public void SetOnClick(Action<int> dele)
		{
			OnClickDelegate = dele;
		}

		private void OnClick()
		{
			OnClickDelegate(_nIndex);
		}
	}
}
