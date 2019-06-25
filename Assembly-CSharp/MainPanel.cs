using KCV;
using KCV.Remodel;
using local.models;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
	[SerializeField]
	public UIRemodelParameter[] mUIRemodelParameters;

	[SerializeField]
	private UILabel mLabel_ShipName;

	[SerializeField]
	private UILabel mLabel_Level;

	[SerializeField]
	private UIRemodelLevel mUIRemodelLevel_RemodelLevel;

	[SerializeField]
	private CommonShipBanner mCommonShipBanner_ShipBanner;

	private ShipModel mShipModel;

	public void Initialize(ShipModel shipModel)
	{
		mShipModel = shipModel;
		mLabel_Level.text = shipModel.Level.ToString();
		mLabel_ShipName.text = shipModel.Name;
		mCommonShipBanner_ShipBanner.SetShipData(shipModel);
	}
}
