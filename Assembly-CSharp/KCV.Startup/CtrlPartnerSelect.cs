using KCV.Display;
using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlPartnerSelect : MonoBehaviour
	{
		public enum ButtonIndex
		{
			L,
			R,
			Back,
			Deside
		}

		public enum ShipPartsIndex
		{
			Girl,
			Background,
			Info
		}

		private Dictionary<int, Vector2> _shipLocate = new Dictionary<int, Vector2>
		{
			{
				54,
				new Vector2(247f, -190f)
			},
			{
				55,
				new Vector2(284f, -185f)
			},
			{
				56,
				new Vector2(314f, -202f)
			},
			{
				9,
				new Vector2(250f, -177f)
			},
			{
				33,
				new Vector2(180f, -135f)
			},
			{
				37,
				new Vector2(243f, -134f)
			},
			{
				46,
				new Vector2(248f, -228f)
			},
			{
				94,
				new Vector2(324f, -190f)
			},
			{
				1,
				new Vector2(312f, -160f)
			},
			{
				43,
				new Vector2(327f, -160f)
			},
			{
				96,
				new Vector2(280f, -206f)
			}
		};

		private UIPanel _uiPanel;

		private List<List<ShipModelMst>> _listStarterShips;

		private Dictionary<ButtonIndex, UIButton> _dicButtons;

		private Dictionary<ShipPartsIndex, UITexture> _dicShipParts;

		private CtrlStarterSelect.StarterType _iStarterType;

		private int _nSelectedId;

		private Action _actOnCancel;

		private Action<ShipModelMst> _actOnDecidePartnerShip;

		private bool _isMove;

		private bool _isDecide;

		private UIDisplaySwipeEventRegion _clsSwipeEvent;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public Transform partnerShip => _dicShipParts[ShipPartsIndex.Girl].transform;

		public static CtrlPartnerSelect Instantiate(CtrlPartnerSelect prefab, Transform parent)
		{
			return UnityEngine.Object.Instantiate(prefab);
		}

		private void Awake()
		{
			_isMove = false;
			_dicButtons = new Dictionary<ButtonIndex, UIButton>();
			foreach (int value in Enum.GetValues(typeof(ButtonIndex)))
			{
				_dicButtons.Add((ButtonIndex)value, ((Component)base.transform.FindChild("Button_" + ((ButtonIndex)value).ToString())).GetComponent<UIButton>());
				_dicButtons[(ButtonIndex)value].onClick = Util.CreateEventDelegateList(this, "press_Button", (ButtonIndex)value);
			}
			_dicShipParts = new Dictionary<ShipPartsIndex, UITexture>();
			foreach (int value2 in Enum.GetValues(typeof(ShipPartsIndex)))
			{
				_dicShipParts.Add((ShipPartsIndex)value2, ((Component)base.transform.FindChild("Ship_" + ((ShipPartsIndex)value2).ToString())).GetComponent<UITexture>());
			}
			_iStarterType = CtrlStarterSelect.StarterType.Ex;
			_nSelectedId = 0;
			_clsSwipeEvent = GameObject.Find("EventArea").GetComponent<UIDisplaySwipeEventRegion>();
			_clsSwipeEvent.SetOnSwipeActionJudgeCallBack(OnSwipe);
			_clsSwipeEvent.SetEventCatchCamera(StartupTaskManager.GetPSVitaMovie().GetComponent<Camera>());
			_isDecide = false;
			panel.widgetsAreStatic = true;
		}

		private void Start()
		{
			_listStarterShips = new List<List<ShipModelMst>>();
			foreach (List<int> item in Defines.STARTER_PARTNER_SHIPS_ID)
			{
				List<ShipModelMst> starterShips = new List<ShipModelMst>();
				item.ForEach(delegate(int x)
				{
					starterShips.Add(new ShipModelMst(x));
				});
				_listStarterShips.Add(starterShips);
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiPanel);
			Mem.DelListSafe(ref _listStarterShips);
			Mem.DelDictionarySafe(ref _dicButtons);
			Mem.DelDictionarySafe(ref _dicShipParts);
			Mem.Del(ref _iStarterType);
			Mem.Del(ref _nSelectedId);
			Mem.Del(ref _actOnCancel);
			Mem.Del(ref _actOnDecidePartnerShip);
			Mem.Del(ref _clsSwipeEvent);
			base.transform.GetComponentsInChildren<UIWidget>().ForEach(delegate(UIWidget x)
			{
				if (x is UISprite)
				{
					((UISprite)x).Clear();
				}
				Mem.Del(ref x);
			});
		}

		public bool Init(Action<ShipModelMst> onDecidePartnerShip, Action onCancel)
		{
			_actOnCancel = onCancel;
			_actOnDecidePartnerShip = onDecidePartnerShip;
			panel.widgetsAreStatic = false;
			base.transform.localScaleOne();
			ChangePartnerShip(0);
			_isDecide = false;
			foreach (Transform item in base.transform)
			{
				if (!item.gameObject.activeInHierarchy)
				{
					item.SetActive(isActive: true);
				}
			}
			return true;
		}

		public void SetStarter(CtrlStarterSelect.StarterType iType)
		{
			_iStarterType = iType;
			_nSelectedId = 0;
		}

		public ShipModelMst getSelectedShip()
		{
			return _listStarterShips[(int)_iStarterType][_nSelectedId];
		}

		public void PreparaNext(bool isFoward)
		{
			if (!_isMove)
			{
				_nSelectedId = Mathe.NextElementRev(_nSelectedId, 0, _listStarterShips[(int)_iStarterType].Count - 1, isFoward);
				ChangePartnerShip(_nSelectedId);
			}
		}

		public void press_Button(ButtonIndex iIndex)
		{
			if (!_isMove)
			{
				switch (iIndex)
				{
				case ButtonIndex.Back:
					break;
				case ButtonIndex.L:
					PreparaNext(isFoward: false);
					break;
				case ButtonIndex.R:
					PreparaNext(isFoward: true);
					break;
				case ButtonIndex.Deside:
					OnDecidePartnerShip();
					break;
				}
			}
		}

		public void Show()
		{
			panel.widgetsAreStatic = false;
			base.transform.localScaleOne();
		}

		public void Hide()
		{
			foreach (Transform item in base.transform)
			{
				if (!(item == partnerShip.transform))
				{
					item.SetActive(isActive: false);
				}
			}
		}

		public bool OnCancel()
		{
			if (_isMove)
			{
				return false;
			}
			Dlg.Call(ref _actOnCancel);
			return true;
		}

		public bool OnDecidePartnerShip()
		{
			if (_isMove)
			{
				return false;
			}
			if (_isDecide)
			{
				return false;
			}
			_isDecide = true;
			_dicShipParts[ShipPartsIndex.Girl].transform.LTCancel();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			Dlg.Call(ref _actOnDecidePartnerShip, getSelectedShip());
			return true;
		}

		private void OnSwipe(UIDisplaySwipeEventRegion.ActionType iType, float dX, float dY, float mpX, float mpY, float et)
		{
			if (!_isMove && iType == UIDisplaySwipeEventRegion.ActionType.Moving)
			{
				if (mpX >= 0.15f)
				{
					PreparaNext(isFoward: true);
				}
				else if (mpX <= -0.15f)
				{
					PreparaNext(isFoward: false);
				}
			}
		}

		private void ChangePartnerShip(int selectedShipCursor)
		{
			string str = $"{_listStarterShips[(int)_iStarterType][selectedShipCursor].GetGraphicsMstId():000}";
			_dicShipParts[ShipPartsIndex.Background].mainTexture = (Resources.Load("Textures/Startup/PartnerShip/startup_c" + str + "_txtArea") as Texture);
			_dicShipParts[ShipPartsIndex.Background].localSize = new Vector2(740f, 382f);
			Transform transform = _dicShipParts[ShipPartsIndex.Girl].transform;
			Vector2 vector = _shipLocate[_listStarterShips[(int)_iStarterType][selectedShipCursor].MstId];
			transform.localPositionX(vector.x);
			Transform transform2 = _dicShipParts[ShipPartsIndex.Girl].transform;
			Vector2 vector2 = _shipLocate[_listStarterShips[(int)_iStarterType][selectedShipCursor].MstId];
			transform2.localPositionY(vector2.y);
			_dicShipParts[ShipPartsIndex.Girl].transform.LTCancel();
			_dicShipParts[ShipPartsIndex.Girl].transform.localScaleOne();
			_dicShipParts[ShipPartsIndex.Girl].transform.LTScale(Vector3.one * 1.05f, 5f).setEase(LeanTweenType.easeOutSine).setLoopPingPong();
			Vector3 localPosition = _dicShipParts[ShipPartsIndex.Girl].transform.localPosition;
			_dicShipParts[ShipPartsIndex.Girl].transform.localPositionX(1000f);
			_dicShipParts[ShipPartsIndex.Girl].transform.LTMoveLocal(localPosition, 0.2f).setEase(LeanTweenType.easeOutSine).setOnComplete((Action)delegate
			{
				_isMove = false;
			});
			_dicShipParts[ShipPartsIndex.Girl].mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(_listStarterShips[(int)_iStarterType][selectedShipCursor].GetGraphicsMstId(), 9);
			_dicShipParts[ShipPartsIndex.Girl].MakePixelPerfect();
			_dicShipParts[ShipPartsIndex.Girl].transform.localScale = Vector3.one;
			_dicShipParts[ShipPartsIndex.Info].transform.localPositionY(155f);
			Vector3 localPosition2 = _dicShipParts[ShipPartsIndex.Info].transform.localPosition;
			_dicShipParts[ShipPartsIndex.Info].transform.localPositionY(135f);
			_dicShipParts[ShipPartsIndex.Info].transform.LTMoveLocal(localPosition2, 0.2f).setEase(LeanTweenType.easeOutSine);
			_dicShipParts[ShipPartsIndex.Info].mainTexture = (Resources.Load("Textures/Startup/PartnerShip/startup_c" + str + "_txt") as Texture);
			_dicShipParts[ShipPartsIndex.Info].localSize = Defines.STARTER_PARTNER_TEXT_SIZE[(int)_iStarterType][selectedShipCursor];
			_dicShipParts[ShipPartsIndex.Info].transform.localScale = Vector3.one * 0.8f;
			ShipUtils.PlayShipVoice(_listStarterShips[(int)_iStarterType][selectedShipCursor], 26);
			_isMove = true;
		}

		public void cachePreLoad()
		{
		}
	}
}
