using Common.Enum;
using local.models;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeShipBanner : BaseShipBanner
	{
		public UISprite UiConditionIcon;

		public UISprite UiConditionMask;

		public ParticleSystem KiraPar;

		public int sizeX;

		private void OnValidate()
		{
		}

		public new virtual void SetShipData(ShipModel model)
		{
			UiConditionIcon = ((Component)base.transform.FindChild("ConditionIcon")).GetComponent<UISprite>();
			UiConditionMask = ((Component)base.transform.FindChild("ConditionMask")).GetComponent<UISprite>();
			UIPanel component = ((Component)base.transform.parent.parent.transform.FindChild("Panel")).GetComponent<UIPanel>();
			KiraPar = ((Component)component.transform.FindChild("Light")).GetComponent<ParticleSystem>();
			KiraPar.Stop();
			((Component)KiraPar).SetActive(isActive: false);
			if (model != null)
			{
				_clsShipModel = model;
				int texNum = (!model.IsDamaged()) ? 1 : 2;
				_uiShipTex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, texNum);
				UpdateDamage(model.DamageStatus);
				UpdateCondition(model.ConditionState);
			}
		}

		private void UpdateCondition(FatigueState state)
		{
			KiraPar.Stop();
			((Component)KiraPar).SetActive(isActive: false);
			switch (state)
			{
			case FatigueState.Normal:
				UiConditionMask.alpha = 0f;
				UiConditionIcon.alpha = 0f;
				break;
			case FatigueState.Light:
				UiConditionMask.alpha = 1f;
				UiConditionIcon.alpha = 1f;
				UiConditionMask.spriteName = "card-ss_fatigue_1";
				UiConditionIcon.spriteName = "icon_fatigue_1";
				break;
			case FatigueState.Distress:
				UiConditionMask.alpha = 1f;
				UiConditionIcon.alpha = 1f;
				UiConditionMask.spriteName = "card-ss_fatigue_2";
				UiConditionIcon.spriteName = "icon_fatigue_2";
				break;
			case FatigueState.Exaltation:
				UiConditionMask.alpha = 0f;
				UiConditionIcon.alpha = 0f;
				((Component)KiraPar).SetActive(isActive: true);
				KiraPar.Play();
				break;
			}
		}
	}
}
