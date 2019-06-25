using KCV;
using KCV.Scene.Port;
using local.models;
using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class UIMissionShipBanner : MonoBehaviour
{
	[SerializeField]
	private CommonShipBanner mCommonShipBanner;

	[SerializeField]
	private CommonShipSupplyState mCommonShipSupplyState;

	[SerializeField]
	private UILabel mLabel_ShipPosition;

	private UIPanel mPanelThis;

	private void Awake()
	{
		mPanelThis = GetComponent<UIPanel>();
		mPanelThis.alpha = 0.01f;
	}

	public void Initialize(int position, ShipModel shipModel)
	{
		mLabel_ShipPosition.text = position.ToString();
		mCommonShipBanner.SetShipData(shipModel);
		if (mCommonShipSupplyState != null)
		{
			mCommonShipSupplyState.setSupplyState(shipModel);
		}
	}

	public void Show()
	{
		mPanelThis.alpha = 1f;
	}

	private void OnDestroy()
	{
		mCommonShipBanner = null;
		mCommonShipSupplyState = null;
		UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_ShipPosition);
		UserInterfacePortManager.ReleaseUtils.Release(ref mPanelThis);
	}
}
