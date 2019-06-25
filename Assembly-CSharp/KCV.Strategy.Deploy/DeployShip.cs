using local.models;
using UnityEngine;

namespace KCV.Strategy.Deploy
{
	[RequireComponent(typeof(UIWidget))]
	public class DeployShip : MonoBehaviour
	{
		private UIWidget mWidgetThis;

		[SerializeField]
		private UISprite mSprite_ShipTypeIcon;

		private Vector3 mDefaultLocalPosition;

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
			mDefaultLocalPosition = mSprite_ShipTypeIcon.transform.localPosition;
		}

		public void Initialize(ShipModel shipModel)
		{
			int shipType = shipModel.ShipType;
			SetShipTypeIcon(shipType);
			TweenPosition tweenPosition = TweenPosition.Begin(mSprite_ShipTypeIcon.gameObject, 2f, mDefaultLocalPosition + new Vector3(0f, 3f, 0f));
			tweenPosition.from = mDefaultLocalPosition;
			tweenPosition.style = UITweener.Style.PingPong;
		}

		public void InitializeDefailt()
		{
			mSprite_ShipTypeIcon.spriteName = string.Empty;
			mWidgetThis.alpha = 1E-05f;
		}

		private void SetShipTypeIcon(int shipTypeId)
		{
			mSprite_ShipTypeIcon.spriteName = $"shipicon_{shipTypeId}";
			mWidgetThis.alpha = 1f;
		}

		private void SetShipTypeIconColor(Color color)
		{
			mSprite_ShipTypeIcon.color = color;
		}
	}
}
