using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.models;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIWidget))]
	public class UITacticalSituationShipBanner : BaseUISortieBattleShipBanner<ShipModel_BattleAll>
	{
		[SerializeField]
		private UILabel _uiShipHp;

		public static UITacticalSituationShipBanner Instantiate(UITacticalSituationShipBanner prefab, Transform parent, ShipModel_BattleAll model)
		{
			if (model == null)
			{
				return null;
			}
			UITacticalSituationShipBanner uITacticalSituationShipBanner = Object.Instantiate(prefab);
			uITacticalSituationShipBanner.transform.parent = parent;
			uITacticalSituationShipBanner.transform.localScaleOne();
			uITacticalSituationShipBanner.transform.localPositionZero();
			uITacticalSituationShipBanner.VirtualCtor(model);
			return uITacticalSituationShipBanner;
		}

		protected override void VirtualCtor(ShipModel_BattleAll model)
		{
			base.VirtualCtor(model);
			SetShipInfos(model);
			SetUISettings(model.IsFriend());
		}

		protected override void OnUnInit()
		{
			Mem.Del(ref _uiShipHp);
		}

		protected override void SetShipInfos(ShipModel_BattleAll model)
		{
			SetShipName(model.Name);
			SetShipBannerTexture(ShipUtils.LoadBannerTextureInTacticalSituation(model), model.DamagedFlgEnd, model.DmgStateEnd, model.IsEscape());
			SetShipIcons(model.DmgStateEnd, model.IsGroundFacility(), model.IsEscape());
			SetHPSliderValue(model.HpEnd, model.MaxHp);
		}

		protected override void SetHPSliderValue(int nNowHP, int nMaxHP)
		{
			base.SetHPSliderValue(nNowHP, nMaxHP);
			_uiShipHp.text = $"{nNowHP}/{nMaxHP}";
		}

		protected override void SetUISettings(bool isFriend)
		{
			_uiShipHp.pivot = ((!isFriend) ? UIWidget.Pivot.Left : UIWidget.Pivot.Right);
			_uiShipHp.transform.localPosition = ((!isFriend) ? new Vector3(-428f, -20f, 0f) : new Vector3(428f, -20f, 0f));
			_uiHPSlider.fillDirection = UIProgressBar.FillDirection.LeftToRight;
			_uiHPSlider.transform.localPosition = ((!isFriend) ? new Vector3(-175f, -35f, 0f) : new Vector3(175f, -35f, 0f));
			_uiHPSlider.transform.localRotation = ((!isFriend) ? Quaternion.Euler(Vector3.up * 180f) : Quaternion.Euler(Vector3.zero));
			_uiShipName.pivot = ((!isFriend) ? UIWidget.Pivot.Right : UIWidget.Pivot.Left);
			_uiShipName.transform.localPosition = ((!isFriend) ? new Vector3(-175f, -15f, 0f) : new Vector3(175f, -15f, 0f));
			_uiShipTexture.pivot = ((!isFriend) ? UIWidget.Pivot.TopRight : UIWidget.Pivot.TopLeft);
			_uiShipTexture.transform.localPositionZero();
			_uiDamageIcon.pivot = ((!isFriend) ? UIWidget.Pivot.TopRight : UIWidget.Pivot.TopLeft);
			_uiDamageIcon.transform.localPositionZero();
			_uiDamageMask.pivot = ((!isFriend) ? UIWidget.Pivot.TopRight : UIWidget.Pivot.TopLeft);
			_uiDamageMask.transform.localPositionZero();
		}
	}
}
