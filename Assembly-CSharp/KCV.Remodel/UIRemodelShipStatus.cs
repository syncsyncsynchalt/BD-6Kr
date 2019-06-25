using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelShipStatus : MonoBehaviour, UIRemodelView
	{
		[SerializeField]
		private UITexture shipTexture;

		private ShipModel ship;

		private Vector3 hidePos = new Vector3(-600f, 125f);

		private Vector3 showPos = new Vector3(-462f, 31f);

		private Vector3 showPos2 = new Vector3(-462f, 31f);

		public void Init(ShipModel ship)
		{
			this.ship = ship;
			if (shipTexture.mainTexture != null)
			{
				Resources.UnloadAsset(shipTexture.mainTexture);
				shipTexture.mainTexture = null;
				UIDrawCall.ReleaseInactive();
			}
			shipTexture.mainTexture = null;
			shipTexture.mainTexture = ShipUtils.LoadTexture(ship);
			shipTexture.MakePixelPerfect();
			shipTexture.ResizeCollider();
			shipTexture.transform.localPosition = Util.Poi2Vec(new ShipOffset(ship.GetGraphicsMstId()).GetSlotItemCategory(ship.IsDamaged())) + Vector3.down * 20f;
			shipTexture.transform.localScale = Vector3.one * 1.1f;
			Show();
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
			if (UserInterfaceRemodelManager.instance.focusedDeckModel == null)
			{
				base.transform.localPosition = showPos2;
			}
			else
			{
				base.transform.localPosition = showPos;
			}
		}

		public void ShowMove()
		{
			base.transform.localPosition = showPos;
		}

		public void Hide()
		{
			base.transform.localPosition = hidePos;
			base.gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref shipTexture);
			ship = null;
		}
	}
}
