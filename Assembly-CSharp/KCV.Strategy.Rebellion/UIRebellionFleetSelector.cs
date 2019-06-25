using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRebellionFleetSelector : MonoBehaviour, RouletteSelectorHandler
	{
		[Serializable]
		private class FleetInfos
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UITexture _uiBackground;

			[SerializeField]
			private UILabel _uiFleetName;

			[SerializeField]
			private UITexture _uiFleetNum;

			[SerializeField]
			private List<UIButton> _listBtns;

			public Transform transform => _tra;

			public List<UIButton> buttons => _listBtns;

			public bool Init(MonoBehaviour mono, string methodName)
			{
				int cnt = 0;
				_listBtns.ForEach(delegate(UIButton x)
				{
					x.onClick = Util.CreateEventDelegateList(mono, methodName, (ArrowType)cnt);
					cnt++;
				});
				return true;
			}

			public bool UnInit()
			{
				Mem.Del(ref _tra);
				Mem.Del(ref _uiBackground);
				Mem.Del(ref _uiFleetName);
				Mem.Del(ref _uiFleetNum);
				Mem.DelListSafe(ref _listBtns);
				return true;
			}

			public void SetFleetInfos(string fleetName, int fleetID)
			{
				_uiFleetName.text = fleetName;
				_uiFleetName.supportEncoding = false;
				_uiFleetNum.mainTexture = Resources.Load<Texture2D>($"Textures/Common/DeckFlag/icon_deck{fleetID}");
				_uiFleetNum.MakePixelPerfect();
			}
		}

		public enum ArrowType
		{
			Left,
			Right
		}

		[SerializeField]
		private Transform _prefabSelectorShip;

		[SerializeField]
		private RouletteSelector _clsRouletteSelector;

		[SerializeField]
		private Vector3 _vOriginPos = new Vector3(240f, -160f, 0f);

		[SerializeField]
		private FleetInfos _clsFleetInfos;

		[SerializeField]
		private List<Vector3> _listFleetInfosPos = new List<Vector3>
		{
			new Vector3(3f, -11f, 0f),
			new Vector3(-494f, -22f, 0f)
		};

		[SerializeField]
		private List<Vector3> _listSelectorPos = new List<Vector3>
		{
			new Vector3(0f, 231f, 0f),
			new Vector3(-525f, 242f, 0f)
		};

		private int _nSelectedIndex;

		private List<DeckModel> _listDeckModels;

		private List<UIRebellionSelectorShip> _listSelectorShips;

		private List<BoxCollider2D> _listCollider;

		private DelDecideRebellionOrganizeFleetSelector _delDecideRebellionOrganizeFleetSelector;

		public int selectedIndex => _nSelectedIndex;

		public int fleetCnt
		{
			get
			{
				if (_listDeckModels == null)
				{
					return -1;
				}
				return _listDeckModels.Count;
			}
		}

		public DeckModel nowSelectedDeck => _listDeckModels[_nSelectedIndex];

		public bool isColliderEnabled
		{
			get
			{
				if (_listCollider == null)
				{
					_listCollider = new List<BoxCollider2D>();
					_listCollider.AddRange(GetComponentsInChildren<BoxCollider2D>());
				}
				return _listCollider[0].enabled;
			}
			set
			{
				if (isColliderEnabled != value)
				{
					_listCollider.ForEach(delegate(BoxCollider2D x)
					{
						x.enabled = value;
					});
				}
			}
		}

		public RouletteSelector rouletteSelector => _clsRouletteSelector;

		public UIPanel panel => GetComponent<UIPanel>();

		bool RouletteSelectorHandler.IsSelectable(int index)
		{
			DebugUtils.Log("UIRebellionFleetSelector", "index:" + index);
			return true;
		}

		void RouletteSelectorHandler.OnUpdateIndex(int index, Transform transform)
		{
			DebugUtils.Log("UIRebellionFleetSelector", $"index:{index} transform:{transform.name}");
			ChangeFleet(index);
		}

		void RouletteSelectorHandler.OnSelect(int index, Transform transform)
		{
			DebugUtils.Log("UIRebellionFleetSelector", $"index:{index} transform:{transform.name}");
			_nSelectedIndex = index;
			DebugUtils.Log($"[{nowSelectedDeck.Id}({nowSelectedDeck.Name})]{((nowSelectedDeck.GetFlagShip() == null) ? string.Empty : nowSelectedDeck.GetFlagShip().Name)}");
			if (_delDecideRebellionOrganizeFleetSelector != null)
			{
				_delDecideRebellionOrganizeFleetSelector(nowSelectedDeck);
			}
		}

		public static UIRebellionFleetSelector Instantiate(UIRebellionFleetSelector prefab, Transform parent)
		{
			UIRebellionFleetSelector uIRebellionFleetSelector = UnityEngine.Object.Instantiate(prefab);
			uIRebellionFleetSelector.transform.parent = parent;
			uIRebellionFleetSelector.transform.localScaleOne();
			uIRebellionFleetSelector.transform.localPosition = uIRebellionFleetSelector._vOriginPos;
			return uIRebellionFleetSelector;
		}

		private void Awake()
		{
			if (_clsRouletteSelector == null)
			{
				Util.FindParentToChild(ref _clsRouletteSelector, base.transform, "ShipRoletteSelector");
			}
			_nSelectedIndex = 0;
			_listDeckModels = new List<DeckModel>();
			_listSelectorShips = new List<UIRebellionSelectorShip>();
			_clsFleetInfos.Init(this, "DecideSelectorArrow");
			_clsFleetInfos.transform.localPosition = _listFleetInfosPos[0];
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabSelectorShip);
			Mem.Del(ref _clsRouletteSelector);
			Mem.DelListSafe(ref _listSelectorPos);
			Mem.Del(ref _delDecideRebellionOrganizeFleetSelector);
			Mem.Del(ref _nSelectedIndex);
			Mem.DelListSafe(ref _listDeckModels);
			Mem.DelListSafe(ref _listSelectorShips);
			Mem.DelListSafe(ref _listCollider);
		}

		public bool Init(List<DeckModel> models, int initIndex, DelDecideRebellionOrganizeFleetSelector decideDelegate)
		{
			DebugUtils.Log("UIRebellionFleetSelector", string.Empty);
			_listDeckModels = models;
			_nSelectedIndex = initIndex;
			_delDecideRebellionOrganizeFleetSelector = decideDelegate;
			SetFleetInfos(initIndex);
			foreach (Transform item in _clsRouletteSelector.GetContainer().transform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			int num = 0;
			foreach (DeckModel model in models)
			{
				_listSelectorShips.Add(UIRebellionSelectorShip.Instantiate(((Component)_prefabSelectorShip).GetComponent<UIRebellionSelectorShip>(), _clsRouletteSelector.transform, Vector3.zero, model.GetFlagShip()));
				_listSelectorShips[num].transform.name = "SelectorShips" + num;
				num++;
			}
			_clsRouletteSelector.Init(this);
			_clsRouletteSelector.SetKeyController(StrategyTaskManager.GetStrategyRebellion().keycontrol);
			_clsRouletteSelector.ScaleForce(0.3f, 1f);
			return true;
		}

		public void ReqMode(CtrlRebellionOrganize.RebellionOrganizeMode iMode, float time, LeanTweenType easeType)
		{
			switch (iMode)
			{
			case CtrlRebellionOrganize.RebellionOrganizeMode.Main:
				_clsFleetInfos.buttons.ForEach(delegate(UIButton x)
				{
					x.SetActive(isActive: true);
				});
				_clsFleetInfos.transform.LTMoveLocal(_listFleetInfosPos[(int)iMode], time).setEase(easeType);
				_listSelectorShips.ForEach(delegate(UIRebellionSelectorShip x)
				{
					if (x.shipModel != nowSelectedDeck.GetFlagShip())
					{
						x.transform.LTValue(0f, 1f, time).setEase(easeType).setOnUpdate(delegate(float y)
						{
							x.textureAlpha = y;
						});
					}
				});
				_clsRouletteSelector.transform.LTMoveLocal(_listSelectorPos[0], time).setEase(easeType);
				break;
			case CtrlRebellionOrganize.RebellionOrganizeMode.Detail:
				_clsFleetInfos.buttons.ForEach(delegate(UIButton x)
				{
					x.SetActive(isActive: false);
				});
				_clsFleetInfos.transform.LTMoveLocal(_listFleetInfosPos[(int)iMode], time).setEase(easeType);
				_listSelectorShips.ForEach(delegate(UIRebellionSelectorShip x)
				{
					if (x.shipModel != nowSelectedDeck.GetFlagShip())
					{
						x.transform.LTValue(1f, 0f, time).setEase(easeType).setOnUpdate(delegate(float y)
						{
							x.textureAlpha = y;
						});
					}
				});
				_clsRouletteSelector.transform.LTMoveLocal(_listSelectorPos[1], time).setEase(easeType);
				break;
			}
		}

		private void ChangeFleet(int nIndex)
		{
			_nSelectedIndex = nIndex;
			SetFleetInfos(nIndex);
			ShipUtils.PlayShipVoice(_listDeckModels[nIndex].GetFlagShip(), 13);
		}

		private void SetFleetInfos(int nFleetNum)
		{
			if (nFleetNum < fleetCnt)
			{
				_clsFleetInfos.SetFleetInfos(_listDeckModels[nFleetNum].Name, _listDeckModels[nFleetNum].Id);
			}
		}

		private void DecideSelectorArrow(ArrowType iType)
		{
			_clsFleetInfos.buttons[(int)iType].state = UIButtonColor.State.Normal;
			switch (iType)
			{
			case ArrowType.Left:
				_clsRouletteSelector.MovePrev();
				break;
			case ArrowType.Right:
				_clsRouletteSelector.MoveNext();
				break;
			}
		}
	}
}
