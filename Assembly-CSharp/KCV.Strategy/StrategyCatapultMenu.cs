using Common.Enum;
using local.managers;
using local.models;
using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyCatapultMenu : MonoBehaviour
	{
		private const int NormalWidth = 321;

		private const int ShowCatapultWidth = 750;

		private const int KaisyuModeLinePosX = 424;

		private const int ItemModeLinePosX = 560;

		[SerializeField]
		private UISprite Catapult;

		[SerializeField]
		private UISprite MenuBG;

		[SerializeField]
		private UISprite MenuLine;

		[SerializeField]
		private UILabel SPointNumLabel;

		[SerializeField]
		private UILabel NejiNumLabel;

		[SerializeField]
		private UIButton ItemShopBtn;

		[SerializeField]
		private UIButton ItemHouseBtn;

		[SerializeField]
		private UIButton ArsenalBtn;

		[SerializeField]
		private Transform ItemModeLabel;

		[SerializeField]
		private Transform KaisyuModeLabel;

		[SerializeField]
		private BoxCollider2D CancelTouch;

		[SerializeField]
		private UIButtonManager ButtonManager;

		private TweenPosition TweenPos;

		private Transform MenuBox;

		private bool isItemMode;

		private bool isShow;

		private KeyControl key;

		[SerializeField]
		private new Camera camera;

		[Button("Initialize", "Initialize", new object[]
		{

		})]
		public int Button00;

		[Button("Show", "Show", new object[]
		{

		})]
		public int Button01;

		[Button("Hide", "Hide", new object[]
		{

		})]
		public int Button02;

		[Button("setItemMode", "setItemMode", new object[]
		{

		})]
		public int Button03;

		[Button("setKaisyuMode", "setKaisyuMode", new object[]
		{

		})]
		public int Button04;

		private void Awake()
		{
			TweenPos = GetComponent<TweenPosition>();
			MenuBox = base.transform.FindChild("MenuBox");
			isShow = false;
			CancelTouch.enabled = false;
			key = new KeyControl();
		}

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			MenuBox.SetActive(isActive: false);
		}

		private void Update()
		{
			if (App.OnlyController == key)
			{
				key.Update();
				if (key.IsAnyKey)
				{
					key.ClearKeyAll();
					Hide();
				}
			}
		}

		public void OnTouch()
		{
			if (!StrategyTopTaskManager.GetSailSelect().isRun)
			{
				return;
			}
			ShipModel flagShip = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip();
			if (flagShip == null)
			{
				CommonPopupDialog.Instance.StartPopup("艦隊が編成されていません");
				return;
			}
			Initialize(flagShip);
			if (isShow)
			{
				Hide();
			}
			else
			{
				Show();
			}
		}

		public void Initialize(ShipModel FlagShip)
		{
			StrategyMapManager logicManager = StrategyTopTaskManager.GetLogicManager();
			SPointNumLabel.textInt = logicManager.UserInfo.SPoint;
			NejiNumLabel.textInt = logicManager.Material.Revkit;
			if (FlagShip.ShipType == 19)
			{
				setKaisyuMode();
			}
			else
			{
				setItemMode();
			}
		}

		public void Show()
		{
			isShow = true;
			App.OnlyController = key;
			CancelTouch.enabled = true;
			camera.SetActive(isActive: true);
			MenuBox.SetActive(isActive: true);
			TweenPos.onFinished.Clear();
			TweenPos.PlayForward();
		}

		public void Hide()
		{
			isShow = false;
			App.OnlyController = null;
			CancelTouch.enabled = false;
			TweenPos.onFinished.Clear();
			TweenPos.SetOnFinished(OnHideFinished);
			TweenPos.PlayReverse();
		}

		private void OnHideFinished()
		{
			MenuBox.SetActive(isActive: false);
			camera.SetActive(isActive: false);
		}

		private void setItemMode()
		{
			isItemMode = true;
			ArsenalBtn.SetActive(isActive: false);
			KaisyuModeLabel.SetActive(isActive: false);
			ItemModeLabel.SetActive(isActive: true);
			MenuLine.transform.localPositionX(560f);
		}

		private void setKaisyuMode()
		{
			isItemMode = false;
			ArsenalBtn.SetActive(isActive: true);
			KaisyuModeLabel.SetActive(isActive: true);
			ItemModeLabel.SetActive(isActive: false);
			MenuLine.transform.localPositionX(424f);
		}

		public void OnClickStoreButton()
		{
			if (isMoveable())
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add(UserInterfaceItemManager.SHARE_DATA_START_AT_KEY, UserInterfaceItemManager.SHARE_DATA_START_AT_VALUE_ITEMSTORE);
				RetentionData.SetData(hashtable);
				SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.Item);
			}
		}

		public void OnClickHouseButton()
		{
			if (isMoveable())
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add(UserInterfaceItemManager.SHARE_DATA_START_AT_KEY, UserInterfaceItemManager.SHARE_DATA_START_AT_VALUE_ITEMLIST);
				RetentionData.SetData(hashtable);
				SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.Item);
			}
		}

		public void OnClickKaisyuButton()
		{
			if (isMoveable())
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.SceneLoad(Generics.Scene.ImprovementArsenal);
			}
		}

		private bool isMoveable()
		{
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.HasBling())
			{
				CommonPopupDialog.Instance.StartPopup("撤退中の艦を含んでいます");
				return false;
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.MissionState != 0)
			{
				CommonPopupDialog.Instance.StartPopup(Util.getCancelReason(IsGoCondition.Mission));
				return false;
			}
			if (SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Count == 0)
			{
				CommonPopupDialog.Instance.StartPopup("艦隊が編成されていません");
				return false;
			}
			return true;
		}

		private void OnDestroy()
		{
			Catapult = null;
			MenuBG = null;
			MenuLine = null;
			SPointNumLabel = null;
			NejiNumLabel = null;
			ItemShopBtn = null;
			ItemHouseBtn = null;
			ArsenalBtn = null;
			ItemModeLabel = null;
			KaisyuModeLabel = null;
			CancelTouch = null;
			ButtonManager = null;
			TweenPos = null;
			MenuBox = null;
			key = null;
			camera = null;
		}
	}
}
