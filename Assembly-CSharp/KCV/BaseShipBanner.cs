using Common.Enum;
using KCV.Utils;
using local.models;
using UnityEngine;

namespace KCV
{
	public class BaseShipBanner : MonoBehaviour
	{
		protected ShipModel _clsShipModel;

		protected ShipModel_Battle _clsShipModelBattle;

		protected IShipModel _clsIShipModel;

		[SerializeField]
		protected UITexture _uiShipTex;

		[SerializeField]
		protected UISprite _uiDamageIcon;

		[SerializeField]
		protected UISprite _uiDamageMask;

		public ShipModel ShipModel => _clsShipModel;

		public ShipModel_Battle ShipModeBattle => _clsShipModelBattle;

		public IShipModel IShipModel => _clsIShipModel;

		protected virtual void Awake()
		{
			_uiDamageMask.alpha = 0f;
			_uiDamageIcon.alpha = 0f;
		}

		protected virtual void OnDestroy()
		{
			_uiShipTex = null;
			_uiDamageIcon = null;
			_uiDamageMask = null;
		}

		public virtual void SetShipData(IShipModel model)
		{
			if (model != null)
			{
				_clsIShipModel = model;
			}
		}

		public virtual void SetShipData(ShipModel model)
		{
			if (model != null)
			{
				_clsShipModel = model;
				int texNum = (!model.IsDamaged()) ? 1 : 2;
				_Load(model.MstId, texNum);
			}
		}

		public virtual void SetShipData(ShipModel_BattleAll model)
		{
			_clsIShipModel = model;
			if (model != null)
			{
				_uiShipTex.mainTexture = ShipUtils.LoadBannerTexture(model);
				_uiShipTex.localSize = ResourceManager.SHIP_TEXTURE_SIZE[1];
				_uiShipTex.shader = ((model.DmgStateEnd != DamageState_Battle.Gekichin && !model.IsEscape()) ? SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList[1] : SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList[0]);
				UpdateDamage(model.DmgStateEnd, model.IsEscape());
				_uiShipTex.MakePixelPerfect();
			}
		}

		public virtual void SetShipData(ShipModel_BattleResult model)
		{
			_clsIShipModel = model;
			if (model != null)
			{
				_uiShipTex.mainTexture = ShipUtils.LoadBannerTexture(model);
				_uiShipTex.localSize = ResourceManager.SHIP_TEXTURE_SIZE[(!model.IsDamaged()) ? 1 : 2];
				_uiShipTex.shader = ((model.DmgStateEnd != DamageState_Battle.Gekichin && !model.IsEscape()) ? SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList[1] : SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList[0]);
				UpdateDamage(model.DmgStateEnd, model.IsEscape());
			}
		}

		protected virtual void _Load(int shipID, int texNum)
		{
			_uiShipTex.mainTexture = ShipUtils.LoadTexture(shipID, texNum);
			_uiShipTex.MakePixelPerfect();
		}

		public virtual void UpdateDamage(DamageState state)
		{
			switch (state)
			{
			case DamageState.Normal:
				_uiDamageMask.alpha = 0f;
				_uiDamageIcon.alpha = 0f;
				break;
			case DamageState.Shouha:
				_uiDamageMask.alpha = 1f;
				_uiDamageIcon.alpha = 1f;
				_uiDamageMask.spriteName = "icon-ss_burned_shoha";
				_uiDamageIcon.spriteName = "icon-ss_shoha";
				break;
			case DamageState.Tyuuha:
				_uiDamageMask.alpha = 1f;
				_uiDamageIcon.alpha = 1f;
				_uiDamageMask.spriteName = "icon-ss_burned_chuha";
				_uiDamageIcon.spriteName = "icon-ss_chuha";
				break;
			case DamageState.Taiha:
				_uiDamageMask.alpha = 1f;
				_uiDamageIcon.alpha = 1f;
				_uiDamageMask.spriteName = "icon-ss_burned_taiha";
				_uiDamageIcon.spriteName = "icon-ss_taiha";
				break;
			}
		}

		public virtual void UpdateDamage(DamageState_Battle state, bool isEscape)
		{
			switch (state)
			{
			case DamageState_Battle.Normal:
				_uiDamageMask.alpha = 0f;
				_uiDamageIcon.alpha = 0f;
				break;
			case DamageState_Battle.Shouha:
				_uiDamageMask.alpha = 1f;
				_uiDamageIcon.alpha = 1f;
				_uiDamageMask.spriteName = "icon-ss_burned_shoha";
				_uiDamageIcon.spriteName = "icon-ss_shoha";
				break;
			case DamageState_Battle.Tyuuha:
				_uiDamageMask.alpha = 1f;
				_uiDamageIcon.alpha = 1f;
				_uiDamageMask.spriteName = "icon-ss_burned_chuha";
				_uiDamageIcon.spriteName = "icon-ss_chuha";
				break;
			case DamageState_Battle.Taiha:
				_uiDamageMask.alpha = 1f;
				_uiDamageIcon.alpha = 1f;
				_uiDamageMask.spriteName = "icon-ss_burned_taiha";
				_uiDamageIcon.spriteName = "icon-ss_taiha";
				break;
			case DamageState_Battle.Gekichin:
				_uiDamageMask.alpha = 1f;
				_uiDamageIcon.alpha = 1f;
				_uiDamageMask.spriteName = "icon-ss_burned_taiha";
				_uiDamageIcon.spriteName = "icon-ss_gekichin";
				break;
			}
			if (isEscape)
			{
				_uiDamageIcon.spriteName = "icon-ss_taihi";
				_uiDamageIcon.alpha = 1f;
			}
		}
	}
}
