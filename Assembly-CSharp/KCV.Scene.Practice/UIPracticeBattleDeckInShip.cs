using local.models;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIWidget))]
	public class UIPracticeBattleDeckInShip : MonoBehaviour
	{
		private UIWidget mWidgetThis;

		private ShipModel mShipModel;

		[SerializeField]
		private UILabel mLabel_Type;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UILabel mLabel_Level;

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
			mWidgetThis.alpha = 0f;
		}

		public void Initialize(ShipModel shipModel)
		{
			mShipModel = shipModel;
			mLabel_Type.text = shipModel.ShipTypeName;
			mLabel_Name.text = shipModel.Name;
			mLabel_Level.text = $"Lv{shipModel.Level}";
			mWidgetThis.alpha = 1f;
		}

		private void OnDestroy()
		{
			mWidgetThis = null;
			mShipModel = null;
			mLabel_Type = null;
			mLabel_Name = null;
			mLabel_Level = null;
		}
	}
}
