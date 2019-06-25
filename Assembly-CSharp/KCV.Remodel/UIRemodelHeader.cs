using DG.Tweening;
using KCV.Scene.Port;
using local.managers;
using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelHeader : MonoBehaviour
	{
		[SerializeField]
		private UILabel titleLabel;

		[SerializeField]
		private UILabel ammoLabel;

		[SerializeField]
		private UILabel steelLabel;

		[SerializeField]
		private UILabel bauxLabel;

		[SerializeField]
		private Transform mTransform_TurnEndStamp;

		public void RefreshMaterial(ManagerBase manager)
		{
			int materialMaxNum = manager.UserInfo.GetMaterialMaxNum();
			if (materialMaxNum <= manager.Material.Ammo)
			{
				ammoLabel.color = Color.yellow;
			}
			else
			{
				ammoLabel.color = Color.white;
			}
			ammoLabel.text = manager.Material.Ammo.ToString();
			if (materialMaxNum <= manager.Material.Steel)
			{
				steelLabel.color = Color.yellow;
			}
			else
			{
				steelLabel.color = Color.white;
			}
			steelLabel.text = manager.Material.Steel.ToString();
			if (materialMaxNum <= manager.Material.Baux)
			{
				bauxLabel.color = Color.yellow;
			}
			else
			{
				bauxLabel.color = Color.white;
			}
			bauxLabel.text = manager.Material.Baux.ToString();
		}

		public void RefreshTitle(ScreenStatus status, DeckModel deck)
		{
			string text = string.Empty;
			switch (status)
			{
			case ScreenStatus.SELECT_DECK_SHIP:
				text = ((!(deck.Name == string.Empty)) ? ("艦娘選択 -" + deck.Name + "-") : ("艦娘選択 - 第" + deck.Id + "艦隊 -"));
				break;
			case ScreenStatus.SELECT_OTHER_SHIP:
				text = "艦娘選択 - その他 -";
				break;
			case ScreenStatus.SELECT_SETTING_MODE:
				text = "メニュ\u30fc選択";
				break;
			case ScreenStatus.MODE_SOUBI_HENKOU:
			case ScreenStatus.MODE_SOUBI_HENKOU_TYPE_SELECT:
			case ScreenStatus.MODE_SOUBI_HENKOU_ITEM_SELECT:
			case ScreenStatus.MODE_SOUBI_HENKOU_PREVIEW:
				text = "装備変更";
				break;
			case ScreenStatus.MODE_KINDAIKA_KAISHU:
			case ScreenStatus.MODE_KINDAIKA_KAISHU_SOZAI_SENTAKU:
			case ScreenStatus.MODE_KINDAIKA_KAISHU_KAKUNIN:
			case ScreenStatus.MODE_KINDAIKA_KAISHU_ANIMATION:
			case ScreenStatus.MODE_KINDAIKA_KAISHU_END_ANIMATION:
				text = "近代化改修";
				break;
			case ScreenStatus.MODE_KAIZO:
			case ScreenStatus.MODE_KAIZO_ANIMATION:
			case ScreenStatus.MODE_KAIZO_END_ANIMATION:
				text = "改造";
				break;
			}
			if (deck != null && deck.IsActionEnd())
			{
				mTransform_TurnEndStamp.SetActive(isActive: true);
				mTransform_TurnEndStamp.DOKill();
				mTransform_TurnEndStamp.DOLocalRotate(new Vector3(0f, 0f, 300f), 0f, RotateMode.FastBeyond360);
				mTransform_TurnEndStamp.DOLocalRotate(new Vector3(0f, 0f, 360f), 0.8f, RotateMode.FastBeyond360).SetEase(Ease.OutBounce);
			}
			else
			{
				mTransform_TurnEndStamp.SetActive(isActive: false);
			}
			titleLabel.text = text;
			titleLabel.supportEncoding = false;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref titleLabel);
			UserInterfacePortManager.ReleaseUtils.Release(ref ammoLabel);
			UserInterfacePortManager.ReleaseUtils.Release(ref steelLabel);
			UserInterfacePortManager.ReleaseUtils.Release(ref bauxLabel);
			mTransform_TurnEndStamp = null;
		}
	}
}
