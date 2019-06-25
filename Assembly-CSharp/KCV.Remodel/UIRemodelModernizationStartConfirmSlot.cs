using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UIWidget))]
	public class UIRemodelModernizationStartConfirmSlot : MonoBehaviour
	{
		[SerializeField]
		private CommonShipBanner mCommonShipBanner;

		[SerializeField]
		private UILabel mLabel_Level;

		private UIWidget mWidgetThis;

		private ShipModel mBaitShipModel;

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
			mWidgetThis.alpha = 0.01f;
		}

		public void Initialize(ShipModel baitShipModel)
		{
			UITexture uITexture = mCommonShipBanner.GetUITexture();
			Texture releaseRequestTexture = uITexture.mainTexture;
			uITexture.mainTexture = null;
			UserInterfaceRemodelManager.instance.ReleaseRequestBanner(ref releaseRequestTexture);
			mBaitShipModel = baitShipModel;
			if (baitShipModel == null)
			{
				mWidgetThis.alpha = 0.01f;
				return;
			}
			mCommonShipBanner.SetShipData(baitShipModel);
			mLabel_Level.text = baitShipModel.Level.ToString();
			mWidgetThis.alpha = 1f;
		}

		public CommonShipBanner GetShipBanner()
		{
			return mCommonShipBanner;
		}

		public void StopKira()
		{
			mCommonShipBanner.StopParticle();
		}

		internal void Release()
		{
			mCommonShipBanner = null;
			if (mLabel_Level != null)
			{
				mLabel_Level.RemoveFromPanel();
			}
			mLabel_Level = null;
			if (mWidgetThis != null)
			{
				mWidgetThis.RemoveFromPanel();
			}
			mWidgetThis = null;
			mBaitShipModel = null;
		}
	}
}
