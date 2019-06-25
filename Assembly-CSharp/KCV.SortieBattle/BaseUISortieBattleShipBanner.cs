using Common.Enum;
using local.models;
using UnityEngine;

namespace KCV.SortieBattle
{
	[RequireComponent(typeof(UIWidget))]
	public class BaseUISortieBattleShipBanner<ShipModelType> : MonoBehaviour where ShipModelType : ShipModel_Battle
	{
		[SerializeField]
		protected UITexture _uiShipTexture;

		[SerializeField]
		protected UISprite _uiDamageIcon;

		[SerializeField]
		protected UISprite _uiDamageMask;

		[SerializeField]
		protected UILabel _uiShipName;

		[SerializeField]
		protected UISlider _uiHPSlider;

		[SerializeField]
		protected int _nBannerSizeX = 256;

		private UIWidget _uiWidget;

		private ShipModelType _clsShipModel;

		public UIWidget widget
		{
			get
			{
				return this.GetComponentThis(ref _uiWidget);
			}
			protected set
			{
				_uiWidget = value;
			}
		}

		public ShipModelType shipModel
		{
			get
			{
				return _clsShipModel;
			}
			protected set
			{
				_clsShipModel = value;
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiShipTexture);
			Mem.Del(ref _uiDamageIcon);
			Mem.Del(ref _uiDamageMask);
			Mem.Del(ref _uiShipName);
			Mem.Del(ref _uiHPSlider);
			OnUnInit();
		}

		protected virtual void VirtualCtor(ShipModelType model)
		{
			_clsShipModel = model;
		}

		protected virtual void OnUnInit()
		{
		}

		protected virtual void SetShipInfos(ShipModelType model)
		{
		}

		protected virtual void SetShipName(string strName)
		{
			_uiShipName.text = strName;
		}

		protected virtual void SetShipBannerTexture(Texture2D texture, bool isDamaged, DamageState_Battle iState, bool isEscape)
		{
			_uiShipTexture.mainTexture = texture;
			_uiShipTexture.localSize = ResourceManager.SHIP_TEXTURE_SIZE[(!isDamaged) ? 1 : 2];
			_uiShipTexture.shader = GetBannerShader(iState, isEscape);
		}

		protected virtual void SetHPSliderValue(int nNowHP, int nMaxHP)
		{
			_uiHPSlider.value = Mathe.Rate(0f, nMaxHP, nNowHP);
			_uiHPSlider.foregroundWidget.color = Util.HpGaugeColor2(nMaxHP, nNowHP);
		}

		protected virtual void SetUISettings(bool isFriend)
		{
		}

		protected void SetShipIcons(DamageState_Battle iState, bool isGroundFacility, bool isEscape)
		{
			switch (iState)
			{
			case DamageState_Battle.Normal:
			{
				UISprite uiDamageMask = _uiDamageMask;
				float alpha = 0f;
				_uiDamageIcon.alpha = alpha;
				uiDamageMask.alpha = alpha;
				break;
			}
			case DamageState_Battle.Shouha:
			{
				UISprite uiDamageMask2 = _uiDamageMask;
				float alpha = 1f;
				_uiDamageIcon.alpha = alpha;
				uiDamageMask2.alpha = alpha;
				_uiDamageMask.spriteName = "icon-ss_burned_shoha";
				_uiDamageIcon.spriteName = ((!isGroundFacility) ? "icon-ss_shoha" : "icon-ss_konran");
				break;
			}
			case DamageState_Battle.Tyuuha:
				_uiDamageMask.alpha = 1f;
				_uiDamageIcon.alpha = 1f;
				_uiDamageMask.spriteName = "icon-ss_burned_chuha";
				_uiDamageIcon.spriteName = ((!isGroundFacility) ? "icon-ss_chuha" : "icon-ss_songai");
				break;
			case DamageState_Battle.Taiha:
				_uiDamageMask.alpha = 1f;
				_uiDamageIcon.alpha = 1f;
				_uiDamageMask.spriteName = "icon-ss_burned_taiha";
				_uiDamageIcon.spriteName = ((!isGroundFacility) ? "icon-ss_taiha" : "icon-ss_sonsyo");
				break;
			case DamageState_Battle.Gekichin:
				_uiDamageMask.alpha = 1f;
				_uiDamageIcon.alpha = 1f;
				_uiDamageMask.spriteName = "icon-ss_burned_taiha";
				_uiDamageIcon.spriteName = ((!isGroundFacility) ? "icon-ss_gekichin" : "icon-ss_hakai");
				break;
			}
			if (isEscape)
			{
				_uiDamageIcon.spriteName = "icon-ss_taihi";
				_uiDamageIcon.alpha = 1f;
			}
		}

		protected Shader GetBannerShader(DamageState_Battle iState, bool isEscape)
		{
			return (iState != DamageState_Battle.Gekichin && !isEscape) ? SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList[9] : SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList[0];
		}
	}
}
