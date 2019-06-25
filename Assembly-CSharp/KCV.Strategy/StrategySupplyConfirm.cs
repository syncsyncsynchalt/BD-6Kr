using local.managers;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategySupplyConfirm : MonoBehaviour
	{
		[SerializeField]
		private UILabel AmmoNum;

		[SerializeField]
		private UILabel FuelNum;

		[SerializeField]
		private YesNoButton YesNoBtn;

		private TweenScale ts;

		private void Awake()
		{
			base.transform.localScaleZero();
			ts = GetComponent<TweenScale>();
		}

		private void OnDestroy()
		{
			AmmoNum = null;
			FuelNum = null;
			YesNoBtn = null;
		}

		public void SetModel(SupplyManager manager)
		{
			AmmoNum.textInt = manager.AmmoForSupply;
			FuelNum.textInt = manager.FuelForSupply;
			AmmoNum.color = ((manager.AmmoForSupply <= manager.Material.Ammo) ? Color.black : Color.red);
			FuelNum.color = ((manager.FuelForSupply <= manager.Material.Fuel) ? Color.black : Color.red);
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
