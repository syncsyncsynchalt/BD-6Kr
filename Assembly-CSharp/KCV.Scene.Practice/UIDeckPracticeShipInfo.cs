using DG.Tweening;
using local.models;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIDeckPracticeShipInfo : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_ShipName;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private UILabel mLabel_ShipType;

		[SerializeField]
		private UISlider mSlider_Exp;

		private ShipModel mShipModel;

		private ShipExpModel mShipExpModel;

		private Vector3 mVector3_DefaultLocalPosition;

		private void Awake()
		{
			mVector3_DefaultLocalPosition = base.transform.localPosition;
		}

		public void Initialize(ShipModel shipModel, ShipExpModel exp)
		{
			mShipModel = shipModel;
			mShipExpModel = exp;
			mLabel_Level.text = exp.LevelBefore.ToString();
			mLabel_ShipName.text = shipModel.Name;
			mLabel_ShipType.text = shipModel.ShipTypeName;
			mSlider_Exp.value = ((exp.ExpRateBefore != 0) ? ((float)exp.ExpRateBefore / 100f) : 0f);
		}

		public Tween GenerateTweenExpAndLevel()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			int expRateAfterAll = (from num in mShipExpModel.ExpRateAfter
				where true
				select num).Sum();
			return DOVirtual.Float(0f, 1f, 1.3f, delegate(float percentage)
			{
				float num2 = mShipExpModel.ExpRateBefore + (int)((float)(expRateAfterAll - mShipExpModel.ExpRateBefore) * percentage);
				float value = (num2 != 0f) ? (num2 / 100f % 1f) : 0f;
				int num3 = (int)num2 / 100;
				mSlider_Exp.value = value;
				if (mLabel_Level.text != (mShipExpModel.LevelBefore + num3).ToString())
				{
					mLabel_Level.text = (mShipExpModel.LevelBefore + num3).ToString();
				}
			}).SetId(this);
		}

		public void Reposition()
		{
			base.transform.localPosition = mVector3_DefaultLocalPosition;
		}

		private void OnDestroy()
		{
			mLabel_ShipName = null;
			mLabel_Level = null;
			mLabel_ShipType = null;
			mSlider_Exp = null;
			mShipModel = null;
			mShipExpModel = null;
		}
	}
}
