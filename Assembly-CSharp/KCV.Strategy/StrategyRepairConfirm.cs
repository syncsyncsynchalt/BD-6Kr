using Common.Struct;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyRepairConfirm : MonoBehaviour
	{
		[SerializeField]
		private UILabel ShipName;

		[SerializeField]
		private UILabel ShipLV;

		[SerializeField]
		private UILabel SteelNum;

		[SerializeField]
		private UILabel FuelNum;

		[SerializeField]
		private UILabel NeedDay;

		[SerializeField]
		private UILabel UseKit;

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
			SteelNum = null;
			FuelNum = null;
			NeedDay = null;
			UseKit = null;
			YesNoBtn = null;
		}

		public void SetModel(ShipModel ship)
		{
			ShipName.text = ship.Name;
			ShipLV.textInt = ship.Level;
			UILabel steelNum = SteelNum;
			MaterialInfo resourcesForRepair = ship.GetResourcesForRepair();
			steelNum.textInt = resourcesForRepair.Steel;
			UILabel fuelNum = FuelNum;
			MaterialInfo resourcesForRepair2 = ship.GetResourcesForRepair();
			fuelNum.textInt = resourcesForRepair2.Fuel;
			NeedDay.textInt = ship.RepairTime;
			UseKit.text = "使用しない";
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
                throw new NotImplementedException("なにこれ");
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
