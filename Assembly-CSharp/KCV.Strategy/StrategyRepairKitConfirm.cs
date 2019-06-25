using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyRepairKitConfirm : MonoBehaviour
	{
		[SerializeField]
		private UILabel ShipName;

		[SerializeField]
		private UILabel ShipLV;

		[SerializeField]
		private UILabel NeedDay;

		[SerializeField]
		private UILabel KitNumBefore;

		[SerializeField]
		private UILabel KitNumAfter;

		[SerializeField]
		private YesNoButton YesNoBtn;

		[SerializeField]
		private CommonShipBanner shipBanner;

		private TweenScale ts;

		private void Awake()
		{
			base.transform.localScaleZero();
			ts = GetComponent<TweenScale>();
		}

		private void OnDestroy()
		{
			ShipName = null;
			ShipLV = null;
			NeedDay = null;
			KitNumBefore = null;
			KitNumAfter = null;
			YesNoBtn = null;
		}

		public void SetModel(RepairDockModel dock, int RepairKitNum)
		{
			ShipModel ship = dock.GetShip();
			ShipName.text = ship.Name;
			ShipLV.textInt = ship.Level;
			NeedDay.textInt = dock.RemainingTurns;
			KitNumBefore.textInt = RepairKitNum;
			KitNumAfter.textInt = RepairKitNum - 1;
			shipBanner.SetShipData(ship);
		}

		public void Open()
		{
			ts.onFinished.Clear();
			ts.PlayForward();
			YesNoBtn.SetKeyController(new KeyControl());
		}

		public IEnumerator Close()
		{
			TweenAlpha ta = TweenAlpha.Begin(base.gameObject, 0.4f, 0f);
			ta.onFinished.Clear();
			ta.SetOnFinished(delegate
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // base._003CisFinished_003E__0 = true;

				UnityEngine.Object.Destroy(this.gameObject);
			});
			ta.PlayForward();
			yield return null;
		}

		public void SetOnSelectPositive(Action act)
		{
			YesNoBtn.SetOnSelectPositiveListener(act);
		}

		public void SetOnSelectNeagtive(Action act)
		{
			YesNoBtn.SetOnSelectNegativeListener(act);
		}
	}
}
