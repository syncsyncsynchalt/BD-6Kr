using KCV.Scene.Port;
using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelLeftShipStatus : MonoBehaviour, UIRemodelView
	{
		private const float ANIMATION_DURATION = 0.2f;

		[SerializeField]
		private UITexture shipTypeMarkIcon;

		[SerializeField]
		private UILabel labelName;

		[SerializeField]
		private UILabel labelLevel;

		[SerializeField]
		private UiStarManager stars;

		[SerializeField]
		private UISprite background;

		[SerializeField]
		private UITable paramTable;

		[SerializeField]
		private UILabel labelKaryoku;

		[SerializeField]
		private UILabel labelSoukou;

		[SerializeField]
		private UILabel labelRaiso;

		[SerializeField]
		private UILabel labelTaiku;

		private ShipModel ship;

		private Vector3 showPos = new Vector3(-210f, -170f);

		private Vector3 showPos4Expand = new Vector3(-210f, -90f);

		private Vector3 hidePos = new Vector3(-840f, -170f);

		private Vector3 hidePos4Expand = new Vector3(-840f, -90f);

		private int EXPANDED_HEIGHT = 180;

		private int NORMAL_HEIGHT = 100;

		private bool expand;

		private void Awake()
		{
			base.transform.localPosition = hidePos;
		}

		public void Start()
		{
			base.transform.localPosition = hidePos;
		}

		public void Init(ShipModel ship)
		{
			this.ship = ship;
			labelName.text = ship.Name;
			labelLevel.text = ship.Level.ToString();
			stars.init(ship.Srate);
			shipTypeMarkIcon.mainTexture = ResourceManager.LoadShipTypeIcon(ship);
			labelKaryoku.text = ship.Karyoku.ToString();
			labelSoukou.text = ship.Soukou.ToString();
			labelRaiso.text = ship.Raisou.ToString();
			labelTaiku.text = ship.Taiku.ToString();
		}

		public void SetExpand(bool expand)
		{
			this.expand = expand;
			if (expand)
			{
				paramTable.GetComponent<UIWidget>().alpha = 1f;
			}
			else
			{
				paramTable.GetComponent<UIWidget>().alpha = 0.001f;
			}
			background.height = ((!expand) ? NORMAL_HEIGHT : EXPANDED_HEIGHT);
		}

		public void Show()
		{
			Show(animation: true);
		}

		public void Show(bool animation)
		{
			base.gameObject.SetActive(true);
			Vector3 vector = (!expand) ? showPos : showPos4Expand;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, vector, 0.2f);
			}
			else
			{
				base.transform.localPosition = vector;
			}
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			Vector3 vector = (!expand) ? hidePos : hidePos4Expand;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, vector, 0.2f, delegate
				{
					base.gameObject.SetActive(false);
				});
				return;
			}
			base.transform.localPosition = vector;
			base.gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref shipTypeMarkIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref labelName);
			UserInterfacePortManager.ReleaseUtils.Release(ref background);
			UserInterfacePortManager.ReleaseUtils.Release(ref labelKaryoku);
			UserInterfacePortManager.ReleaseUtils.Release(ref labelSoukou);
			UserInterfacePortManager.ReleaseUtils.Release(ref labelRaiso);
			UserInterfacePortManager.ReleaseUtils.Release(ref labelTaiku);
			UserInterfacePortManager.ReleaseUtils.Release(ref shipTypeMarkIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref shipTypeMarkIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref shipTypeMarkIcon);
			stars = null;
			paramTable = null;
			ship = null;
		}
	}
}
