using local.models;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIWidget))]
	public class UIPracticeBattleConfirmShipSlot : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_DeckInNumber;

		[SerializeField]
		private UILabel mLabel_ShipName;

		[SerializeField]
		private UISprite[] mSprites_RemodelLevel;

		[SerializeField]
		private UILabel mLabel_ShipLevel;

		[SerializeField]
		private CommonShipBanner mCommonShipBanner_Ship;

		private ShipModel mShipModel;

		private int mDeckInNumber;

		private UIWidget mWidgetThis;

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
			mWidgetThis.alpha = 0f;
		}

		public void Initialize(int deckInNumber, ShipModel shipModel)
		{
			mDeckInNumber = deckInNumber;
			mShipModel = shipModel;
			mLabel_DeckInNumber.text = deckInNumber.ToString();
			mCommonShipBanner_Ship.SetShipData(shipModel);
			mLabel_ShipName.text = shipModel.Name;
			mLabel_ShipLevel.text = mShipModel.Level.ToString();
			for (int i = 0; i < mSprites_RemodelLevel.Length; i++)
			{
				if (i <= mShipModel.Srate)
				{
					mSprites_RemodelLevel[i].spriteName = "star_on";
				}
				else
				{
					mSprites_RemodelLevel[i].spriteName = "star";
				}
			}
			mWidgetThis.alpha = 1f;
		}

		public void InitializeDefault()
		{
			mWidgetThis.alpha = 0f;
		}

		private void OnDestroy()
		{
			mLabel_DeckInNumber = null;
			mLabel_ShipName = null;
			mSprites_RemodelLevel = null;
			mLabel_ShipLevel = null;
			mCommonShipBanner_Ship = null;
			mShipModel = null;
			mWidgetThis = null;
		}
	}
}
