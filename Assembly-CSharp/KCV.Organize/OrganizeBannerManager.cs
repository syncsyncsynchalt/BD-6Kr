using Common.Enum;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeBannerManager : UIDragDropItem
	{
		private struct DragDropParams
		{
			public Vector3 defaultPos;

			public int defaultShipFrameDepth;

			public int defaultShutterPanelDepth;

			public DragDropParams(Vector3 defaultPos, int defaultFrameDepth, int defaultShutterDepth)
			{
				this.defaultPos = defaultPos;
				defaultShipFrameDepth = defaultFrameDepth;
				defaultShutterPanelDepth = defaultShutterDepth;
			}
		}

		private const float SHUTTER_OPEN_DURATION_SEC = 0.3f;

		private const float SHUTTER_OPEN_DELAY_SEC = 0.2f;

		private const float SHUTTER_CLOSE_DURATION_SEC = 0.3f;

		private const float SCALE_ANIMATION_DURATION_SEC = 0.15f;

		private static readonly Vector2 SHUTTER_OPEN_POS_L = new Vector2(-290f, 3f);

		private static readonly Vector2 SHUTTER_OPEN_POS_R = new Vector2(290f, 3f);

		private static readonly Vector2 SHUTTER_CLOSE_POS_L = new Vector2(-83f, 3f);

		private static readonly Vector2 SHUTTER_CLOSE_POS_R = new Vector2(78f, 3f);

		[SerializeField]
		private GameObject _shipFrame;

		[SerializeField]
		private UIPanel _shutterPanel;

		[SerializeField]
		private UIPanel _bannerPanel;

		[SerializeField]
		private UISprite _gauge;

		[SerializeField]
		private UILabel _labelNumber;

		[SerializeField]
		private UILabel _labelLevel;

		[SerializeField]
		private UILabel _labelName;

		[SerializeField]
		private UILabel _labelTaikyu;

		[SerializeField]
		private GameObject _starManager;

		[SerializeField]
		private GameObject _shutterObj;

		[SerializeField]
		private UISprite _shutterL;

		[SerializeField]
		private UISprite _shutterR;

		[SerializeField]
		private UISprite _shutterLShadow;

		[SerializeField]
		private UISprite _shutterRShadow;

		[SerializeField]
		private Animation _animation;

		[SerializeField]
		public CommonShipBanner shipBanner;

		[SerializeField]
		public UISprite _frame;

		private int gaugeMaxWidth;

		private bool isShow;

		private Action _callback;

		private Action _actOnDragDropEnd;

		private ShipModel _ship;

		private DragDropParams _strDragDropParams;

		private Predicate<OrganizeBannerManager> _preOnCheckDragDropTarget;

		private Action<OrganizeBannerManager> _actOnDragDropStart;

		private Predicate<OrganizeBannerManager> _preOnDragDropRelease;

		public bool IsSet;

		public bool _isDeckChange;

		public UiStarManager StarManager;

		private bool isCloseing;

		public int number
		{
			get;
			private set;
		}

		public ShipModel ship => _ship;

		public ShipBannerDragDrop bannerDragDrop
		{
			get;
			private set;
		}

		private void Awake()
		{
			if (_shipFrame == null)
			{
				_shipFrame = base.transform.FindChild("ShipFrame").gameObject;
			}
			Util.FindParentToChild(ref shipBanner, _shipFrame.transform, "CommonShipBanner2");
			Util.FindParentToChild(ref _bannerPanel, base.transform, "ShipFrame");
			Util.FindParentToChild(ref _shutterPanel, base.transform, "ShutterPanel");
			Util.FindParentToChild(ref _frame, _shipFrame.transform, "Frame");
			Util.FindParentToChild(ref _gauge, _shipFrame.transform, "Gauge");
			Util.FindParentToChild(ref _labelNumber, _shipFrame.transform, "LabelNumber");
			Util.FindParentToChild(ref _labelLevel, _shipFrame.transform, "LabelLevel");
			Util.FindParentToChild(ref _labelName, _shipFrame.transform, "LabelName");
			Util.FindParentToChild(ref _labelTaikyu, _shipFrame.transform, "LabelTaikyu");
			if (_shutterObj == null)
			{
				_shutterObj = _shutterPanel.transform.FindChild("ShutterObj").gameObject;
			}
			Util.FindParentToChild(ref _shutterL, _shutterPanel.transform, "ShutterObj/ShutterL");
			Util.FindParentToChild(ref _shutterR, _shutterPanel.transform, "ShutterObj/ShutterR");
			Util.FindParentToChild(ref _shutterLShadow, _shutterL.transform, "ShutterL_Shadow");
			Util.FindParentToChild(ref _shutterRShadow, _shutterR.transform, "ShutterR_Shadow");
			Util.FindParentToChild(ref StarManager, _shipFrame.transform, "StarManager");
			bannerDragDrop = GetComponent<ShipBannerDragDrop>();
			_strDragDropParams = new DragDropParams(Vector3.zero, _bannerPanel.depth, _shutterPanel.depth);
		}

		private void OnDestroy()
		{
			Mem.Del(ref _shipFrame);
			Mem.Del(ref _shutterPanel);
			Mem.Del(ref _bannerPanel);
			Mem.Del(ref shipBanner);
			Mem.Del(ref _frame);
			Mem.Del(ref _gauge);
			Mem.Del(ref _labelNumber);
			Mem.Del(ref _labelLevel);
			Mem.Del(ref _labelName);
			Mem.Del(ref _labelTaikyu);
			Mem.Del(ref _starManager);
			Mem.Del(ref _shutterObj);
			Mem.Del(ref _shutterL);
			Mem.Del(ref _shutterR);
			Mem.Del(ref _shutterLShadow);
			Mem.Del(ref _shutterRShadow);
			Mem.Del(ref _animation);
			Mem.Del(ref _callback);
			Mem.Del(ref _ship);
			Mem.Del(ref StarManager);
			Mem.Del(ref _preOnCheckDragDropTarget);
			Mem.Del(ref _actOnDragDropStart);
			Mem.Del(ref _actOnDragDropEnd);
		}

		public void init(int number, Predicate<OrganizeBannerManager> onCheckDragDropTarget, Action<OrganizeBannerManager> onDragDropStart, Predicate<OrganizeBannerManager> onDragDropRelease, Action onDragDropEnd, bool isInitPos = true)
		{
			_preOnCheckDragDropTarget = onCheckDragDropTarget;
			_actOnDragDropStart = onDragDropStart;
			_preOnDragDropRelease = onDragDropRelease;
			_actOnDragDropEnd = onDragDropEnd;
			IsSet = false;
			this.number = number;
			_isDeckChange = false;
			_callback = null;
			if (isInitPos)
			{
				int num = 0;
				if (this.number == 1 || this.number == 2)
				{
					num = 0;
				}
				else if (this.number == 3 || this.number == 4)
				{
					num = 1;
				}
				else if (this.number == 5 || this.number == 6)
				{
					num = 2;
				}
				base.gameObject.transform.localPosition = ((this.number % 2 != 1) ? new Vector3(680f, 66f - (float)(124 * num)) : new Vector3(-670f, 66f - (float)(124 * num)));
			}
			if ((UnityEngine.Object)_animation == null)
			{
				_animation = base.gameObject.GetComponent<Animation>();
			}
			_animation.Stop();
			UIButtonMessage component = GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "DetailEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			InitBanner(closeAnimation: false);
			_labelNumber.textInt = number;
			UISprite component2 = ((Component)_shipFrame.transform.FindChild("GaugeFrame")).GetComponent<UISprite>();
			gaugeMaxWidth = component2.width;
		}

		public void InitBanner(bool closeAnimation)
		{
			if (IsSetShip())
			{
				shipBanner.StopParticle();
			}
			IsSet = false;
			_callback = null;
			shipBanner.transform.gameObject.SetActive(false);
			_labelNumber.alpha = 0f;
			_gauge.alpha = 0f;
			_labelName.alpha = 0f;
			_labelLevel.alpha = 0f;
			_labelTaikyu.alpha = 0f;
			_shutterL.alpha = 1f;
			_shutterR.alpha = 1f;
			CloseBanner(closeAnimation);
			if (!closeAnimation)
			{
				SetShipFrameActive(active: false);
			}
		}

		public void InitChangeBanner(bool closeAnimation)
		{
			if (IsSetShip())
			{
				shipBanner.StopParticle();
			}
			IsSet = false;
			shipBanner.transform.gameObject.SetActive(false);
			_labelNumber.alpha = 0f;
			_gauge.alpha = 0f;
			_labelName.alpha = 0f;
			_labelLevel.alpha = 0f;
			_labelTaikyu.alpha = 0f;
			_shutterL.alpha = 1f;
			_shutterR.alpha = 1f;
			if (!closeAnimation)
			{
				CloseBanner(closeAnimation);
				SetShipFrameActive(active: false);
			}
		}

		public void SetShipFrameActive(bool active)
		{
			_shipFrame.SetActive(active);
			_shutterLShadow.SetActive(active);
			_shutterRShadow.SetActive(active);
		}

		public void CloseBanner(bool animation)
		{
			if (!isCloseing)
			{
				if (animation)
				{
					isCloseing = true;
					_shutterPanel.alpha = 1f;
					MoveTo(_shutterL.gameObject, SHUTTER_CLOSE_POS_L, 0.3f, 0f, "compCloseAnimation");
					MoveTo(_shutterR.gameObject, SHUTTER_CLOSE_POS_R, 0.3f);
				}
				else
				{
					_shutterPanel.alpha = 1f;
					_shutterL.transform.localPosition = SHUTTER_CLOSE_POS_L;
					_shutterR.transform.localPosition = SHUTTER_CLOSE_POS_R;
				}
			}
		}

		public void OpenBanner(bool animation)
		{
			isCloseing = false;
			if (animation)
			{
				shipBanner.StopParticle();
				MoveTo(_shutterL.gameObject, SHUTTER_OPEN_POS_L, 0.3f, 0.2f, "compOpenAnimation");
				MoveTo(_shutterR.gameObject, SHUTTER_OPEN_POS_R, 0.3f, 0.2f, string.Empty);
			}
			else
			{
				_shutterL.transform.localPosition = SHUTTER_OPEN_POS_L;
				_shutterR.transform.localPosition = SHUTTER_OPEN_POS_R;
				_shutterPanel.alpha = 0f;
			}
		}

		public void setBanner(ShipModel ship, bool openAnimation, Action callback, bool isShutterHide = false)
		{
			IsSet = true;
			_ship = ship;
			_callback = callback;
			updateBannerWhenShipExist(openAnimation, isShutterHide);
		}

		public void ChangeBanner(ShipModel ship)
		{
			IsSet = true;
			_ship = ship;
			updateBannerWhenShipExist(openAnimation: false);
			OpenBanner(animation: false);
		}

		public void setShip(ShipModel ship)
		{
			_ship = ship;
		}

		public void updateBannerWhenShipExist(bool openAnimation, bool isShutterHide = false)
		{
			if (!IsSet)
			{
				_shipFrame.SetActive(false);
				return;
			}
			SetShipFrameActive(active: true);
			StarManager.init(_ship.Srate);
			shipBanner.transform.gameObject.SetActive(true);
			shipBanner.SetShipData(_ship);
			_labelNumber.alpha = 1f;
			_gauge.alpha = 1f;
			SetHpGauge();
			_labelLevel.alpha = 1f;
			_labelLevel.textInt = _ship.Level;
			_labelName.alpha = 1f;
			_labelName.text = _ship.Name;
			_labelName.color = ((!_ship.IsMarriage()) ? new Color(0f, 0f, 0f) : new Color(1f, 0.7f, 0f));
			_labelName.effectColor = ((!_ship.IsMarriage()) ? new Color(1f, 1f, 1f) : new Color(0f, 0f, 0f));
			_labelTaikyu.alpha = 1f;
			_labelTaikyu.text = _ship.NowHp + "/" + _ship.MaxHp;
			if (openAnimation)
			{
				CloseBanner(animation: false);
				OpenBanner(!isShutterHide);
			}
		}

		public bool IsSetShip()
		{
			return IsSet;
		}

		private void SetHpGauge()
		{
			float num = (float)_ship.NowHp / (float)_ship.MaxHp;
			float num2 = (float)gaugeMaxWidth * num;
			_gauge.width = (int)num2;
			_gauge.alpha = 1f;
			_gauge.color = Util.HpGaugeColor2(_ship.MaxHp, _ship.NowHp);
		}

		public bool CheckBtnEnabled()
		{
			if (OrganizeTaskManager.Instance.GetDetailTask().isEnabled || OrganizeTaskManager.Instance.GetListTask().isEnabled || OrganizeTaskManager.Instance.GetTopTask().isTenderAnimation())
			{
				return false;
			}
			return true;
		}

		public void UpdateBanner(bool enabled)
		{
			if (enabled)
			{
				UISelectedObject.SelectedOneObjectBlink(_frame.gameObject, value: true);
				_shutterPanel.baseClipRegion = new Vector4(-2.5f, 4f, 345f, 122f);
				if (_shipFrame.activeSelf)
				{
					setScaleAnimation(_shipFrame.gameObject, new Vector3(1f, 1f, 1f), 0.15f, 0f);
				}
				setScaleAnimation(_shutterObj.gameObject, new Vector3(1f, 1f, 1f), 0.15f, 0f);
				if (!IsSet)
				{
					CloseBanner((!_isDeckChange) ? true : false);
					UISelectedObject.SelectedOneObjectBlink(_shutterR.gameObject, value: true);
					UISelectedObject.SelectedOneObjectBlink(_shutterL.gameObject, value: true);
				}
			}
			else
			{
				_frame.color = new Color(1f, 1f, 1f);
				UISelectedObject.SelectedOneObjectBlink(_frame.gameObject, value: false);
				UISelectedObject.SelectedOneObjectBlink(_shutterR.gameObject, value: false);
				UISelectedObject.SelectedOneObjectBlink(_shutterL.gameObject, value: false);
				if (_shipFrame.activeSelf)
				{
					setScaleAnimation(_shipFrame.gameObject, new Vector3(0.9f, 0.9f, 0.9f), 0.15f, 0f);
				}
				setScaleAnimation(_shutterObj.gameObject, new Vector3(0.9f, 0.9f, 0.9f), 0.15f, 0f);
				if (!IsSet)
				{
					CloseBanner((!_isDeckChange) ? true : false);
				}
			}
		}

		public void setScaleAnimation(GameObject obj, Vector3 _scale, float _time, float _delay)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("scale", _scale);
			hashtable.Add("time", _time);
			hashtable.Add("delay", _delay);
			hashtable.Add("isLocal", true);
			hashtable.Add("easeType", iTween.EaseType.linear);
			obj.ScaleTo(hashtable);
		}

		public void CompUpdateMove()
		{
			_shutterPanel.baseClipRegion = new Vector4(-2.5f, 4f, 340f, 120f);
		}

		public void UpdateBannerFatigue()
		{
			shipBanner.SetShipData(_ship);
		}

		public void UpdateChangeBanner(bool enabled)
		{
			_frame.color = new Color(1f, 1f, 1f);
			UIWidget[] componentsInChildren = base.transform.GetComponentsInChildren<UIWidget>();
			UIWidget[] array = componentsInChildren;
			foreach (UIWidget uIWidget in array)
			{
				uIWidget.depth = uIWidget.depth;
			}
			base.gameObject.transform.localScale = Vector3.one;
		}

		public void Show(int num)
		{
			float x;
			float y;
			if (num % 2 == 0)
			{
				if (num == 0)
				{
					Vector2 localSize = _frame.localSize;
					float num2 = localSize.x * 1f;
					Vector2 localSize2 = _frame.localSize;
					float num3 = (num2 - localSize2.x) * 0.5f;
					Vector2 localSize3 = _frame.localSize;
					float num4 = localSize3.y * 1f;
					Vector2 localSize4 = _frame.localSize;
					float num5 = num4 - localSize4.y;
					x = -71f - num3 + 1.5f;
					y = 69f - num5 + 1.5f;
				}
				else
				{
					int num6 = num / 2;
					x = -71f;
					y = 69f - (float)num6 * 124f;
				}
			}
			else
			{
				int num7 = num / 2;
				x = 270f;
				y = 69f - (float)num7 * 124f;
			}
			MoveTo(base.gameObject, new Vector3(x, y, 0f), 0.3f, 0f, "showSelect");
			if (bannerDragDrop != null)
			{
				bannerDragDrop.setDefaultPosition(new Vector2(x, y));
			}
			isShow = true;
		}

		private void showSelect()
		{
			if (number == 1)
			{
				UpdateBanner(enabled: true);
			}
			_strDragDropParams.defaultPos = base.transform.localPosition;
		}

		public void DeckChangeAnimetion(bool isLeft)
		{
			if (IsSetShip())
			{
				shipBanner.StopParticle();
			}
			int num = 0;
			for (int i = 0; i < 6; i++)
			{
				if (i % 2 == 0 && i != 0)
				{
					num++;
				}
				if (i == number - 1)
				{
					break;
				}
			}
			if (_isDeckChange)
			{
				OrganizeTaskManager.Instance.GetTopTask().UpdateChangeBanner(number);
				if (!IsSet)
				{
					CloseBanner(animation: false);
				}
				else
				{
					OpenBanner(animation: false);
				}
			}
			string text = (!isLeft) ? ("OutBannerRight" + (num + 1)) : ("OutBannerLeft" + (num + 1));
			_animation.Stop();
			_animation.Play(text);
			_isDeckChange = true;
		}

		public void CompChangeRightAnimate()
		{
			OrganizeTaskManager.Instance.GetTopTask().ChangeDeckAnimate(number);
			_animation.Stop();
			_animation.Play("InBannerLeft");
			if (!IsSet)
			{
				CloseBanner(animation: false);
			}
			else
			{
				OpenBanner(animation: false);
			}
			_isDeckChange = false;
		}

		public void CompChangeLeftAnimate()
		{
			OrganizeTaskManager.Instance.GetTopTask().ChangeDeckAnimate(number);
			_animation.Stop();
			_animation.Play("InBannerRight");
			if (!IsSet)
			{
				CloseBanner(animation: false);
			}
			else
			{
				OpenBanner(animation: false);
			}
			_isDeckChange = false;
		}

		public void EndChangeAnimate()
		{
			if (number == 0)
			{
				UpdateBanner(enabled: true);
			}
		}

		private void compOpenAnimation()
		{
			_shutterPanel.alpha = 0f;
			if (_callback != null)
			{
				_callback();
			}
			shipBanner.StartParticle();
		}

		private void compCloseAnimation()
		{
			isCloseing = false;
			if (_callback != null)
			{
				_callback();
			}
		}

		private void MoveTo(GameObject o, Vector3 to, float duration)
		{
			MoveTo(o, to, duration, 0f, string.Empty);
		}

		private void MoveTo(GameObject obj, Vector3 to, float duration, float delay, string comp)
		{
			iTween.Stop(obj);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", to);
			hashtable.Add("isLocal", true);
			hashtable.Add("delay", delay);
			hashtable.Add("time", duration);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("oncomplete", comp);
			hashtable.Add("oncompletetarget", base.gameObject);
			obj.MoveTo(hashtable);
		}

		public void DetailEL(GameObject obj)
		{
			if (OrganizeTaskManager.Instance.GetListTask().isRun || !CheckBtnEnabled())
			{
				return;
			}
			TaskOrganizeTop.BannerIndex = number;
			if (IsSet)
			{
				OrganizeTaskManager.Instance.GetTopTask().setChangePhase("detail");
				OrganizeTaskManager.Instance.GetDetailTask().Show(_ship);
				if (_ship != null)
				{
					ShipUtils.PlayShipVoice(_ship, App.rand.Next(2, 4));
				}
			}
			else
			{
				OrganizeTaskManager.Instance.GetTopTask().setChangePhase("list");
				OrganizeTaskManager.Instance.GetListTask().Show(IsSet);
			}
			SoundUtils.PlaySE(SEFIleInfos.SE_002);
			OrganizeTaskManager.Instance.GetTopTask().UpdateAllSelectBanner();
			OrganizeTaskManager.Instance.GetTopTask().setControlState();
		}

		protected override void OnDragStart()
		{
			if (IsSet)
			{
				if (_ship.IsInActionEndDeck())
				{
					CommonPopupDialog.Instance.StartPopup(Util.getCancelReason(IsGoCondition.ActionEndDeck));
				}
				else if (!_ship.IsTettaiBling() && _preOnCheckDragDropTarget(this))
				{
					Dlg.Call(ref _actOnDragDropStart, this);
					_bannerPanel.depth += 5;
					_shutterPanel.depth += 5;
					OrganizeTaskManager._clsTop.deckSwitchManager.keyControlEnable = false;
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
					base.OnDragStart();
				}
			}
		}

		protected override void OnDragDropRelease(GameObject surface)
		{
			if (surface != null)
			{
				OrganizeBannerManager component = surface.GetComponent<OrganizeBannerManager>();
				if (component != null && component.IsSet)
				{
					ShipModel ship = component.ship;
					if (!_preOnDragDropRelease(component))
					{
						Dlg.Call(ref _actOnDragDropEnd);
					}
				}
				else
				{
					Dlg.Call(ref _actOnDragDropEnd);
				}
			}
			else
			{
				Dlg.Call(ref _actOnDragDropEnd);
			}
			OrganizeTaskManager._clsTop.deckSwitchManager.keyControlEnable = true;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			base.OnDragDropRelease(surface);
		}

		protected override void OnDragDropEnd()
		{
			mTrans.localPosition = _strDragDropParams.defaultPos;
			_bannerPanel.depth = _strDragDropParams.defaultShipFrameDepth;
			_shutterPanel.depth = _strDragDropParams.defaultShutterPanelDepth;
		}
	}
}
