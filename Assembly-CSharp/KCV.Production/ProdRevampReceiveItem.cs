using KCV.Scene.Port;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Production
{
	public class ProdRevampReceiveItem : MonoBehaviour
	{
		public static System.Random rand = new System.Random((int)DateTime.Now.Ticks & 0xFFFF);

		[SerializeField]
		private UITexture _uiRareBG;

		[SerializeField]
		private UITexture _uiItem1;

		[SerializeField]
		private UITexture _uiItem2;

		[SerializeField]
		private UITexture _uiMessageBG;

		[SerializeField]
		private Animation _getIconAnim;

		[SerializeField]
		private UISprite _uiGearBtn;

		[SerializeField]
		private UISprite _uiGear;

		[SerializeField]
		private Animation _anim;

		[SerializeField]
		private UIWidget _widgetJukurenParent;

		private Generics.Message _clsShipMessage;

		private SlotitemModel_Mst mSlotItemFrom;

		private SlotitemModel_Mst mSlotItemTo;

		private int _index;

		private Action _actCallback;

		private bool _isFinished;

		private bool _isInput;

		private bool _isNeedBGM;

		private bool _isBGMove;

		private bool _isUpdateMessage;

		private bool _isUpdateNextBtn;

		private KeyControl _clsInput;

		private bool _isUseJukuren;

		public bool IsFinished => _isFinished;

		private void Awake()
		{
			_anim = GetComponent<Animation>();
			Util.FindParentToChild(ref _uiRareBG, base.transform, "RareBG");
			Util.FindParentToChild(ref _uiItem1, base.transform, "Item1");
			Util.FindParentToChild(ref _uiItem2, base.transform, "Item2");
			Util.FindParentToChild(ref _uiMessageBG, base.transform, "MessageWindow/MessageBG");
			Util.FindParentToChild<Animation>(ref _getIconAnim, base.transform, "MessageWindow/Get");
			Util.FindParentToChild(ref _uiGearBtn, base.transform, "MessageWindow/NextBtn");
			Util.FindParentToChild(ref _uiGear, base.transform, "MessageWindow/NextBtn/Gear");
			_clsShipMessage = new Generics.Message(base.transform, "MessageWindow/MessageBG/ShipMessage");
			_uiItem1.alpha = 0f;
			_uiItem2.alpha = 0f;
			_uiItem1.depth = 4;
			_uiItem2.depth = 3;
			_isUpdateMessage = false;
			_isFinished = false;
		}

		private void OnDestroy()
		{
			_anim = null;
			_uiRareBG = null;
			_uiItem1 = null;
			_clsShipMessage.UnInit();
			_actCallback = null;
			_isFinished = false;
			_clsInput = null;
		}

		private void Update()
		{
			_clsShipMessage.Update();
			if (_isUpdateNextBtn)
			{
				_uiGear.transform.Rotate(-50f * Time.deltaTime * Vector3.forward);
			}
			if (_isInput && _clsInput != null)
			{
				if (_clsInput != null)
				{
					_clsInput.Update();
				}
				if (_clsInput.keyState[1].down)
				{
					FadeOutExtinguish();
					_isInput = false;
				}
			}
		}

		public static ProdRevampReceiveItem Instantiate(ProdRevampReceiveItem prefab, Transform parent, SlotitemModel_Mst from, SlotitemModel_Mst to, int nPanelDepth, bool useJukuren, KeyControl input)
		{
			ProdRevampReceiveItem component = NGUITools.AddChild(parent.gameObject, prefab.gameObject).GetComponent<ProdRevampReceiveItem>();
			component.mSlotItemFrom = from;
			component.mSlotItemTo = to;
			component._isUseJukuren = useJukuren;
			component._setRewardItem();
			component._uiRareBG.alpha = 0f;
			component._uiRareBG.mainTexture = TextureFile.LoadRareBG(1);
			component.GetComponent<UIPanel>().depth = nPanelDepth;
			component._clsInput = input;
			component.SetJukuren(useJukuren);
			return component;
		}

		public static ProdRevampReceiveItem Instantiate(ProdRevampReceiveItem prefab, Transform parent, IReward_Slotitem from, IReward_Slotitem to, int nPanelDepth, bool useJukuren, KeyControl input)
		{
			return Instantiate(prefab, parent, new SlotitemModel_Mst(from.Id), new SlotitemModel_Mst(to.Id), nPanelDepth, useJukuren, input);
		}

		private void SetJukuren(bool useJukuren)
		{
			_widgetJukurenParent.alpha = (useJukuren ? 1 : 0);
		}

		private void _setRewardItem()
		{
			try
			{
				_uiItem1.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(mSlotItemFrom.MstId, 1);
				_uiItem1.MakePixelPerfect();
				_uiItem2.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(mSlotItemTo.MstId, 1);
				_uiItem2.MakePixelPerfect();
			}
			catch (NullReferenceException)
			{
			}
			((Component)_getIconAnim).gameObject.SetActive(false);
			_uiGearBtn.alpha = 0f;
			_isUpdateNextBtn = false;
		}

		public void Play(Action callback)
		{
			_actCallback = callback;
			_uiRareBG.alpha = 1f;
			_uiItem1.alpha = 1f;
			_uiItem2.alpha = 1f;
			_anim.Stop();
			_anim.Play("startRevampGetItem");
		}

		private void _startMessage()
		{
			if (_isUseJukuren)
			{
				_clsShipMessage.Init("部隊再編中…", 0.04f);
			}
			else
			{
				_clsShipMessage.Init("装備改修中…", 0.04f);
			}
			_clsShipMessage.Play();
		}

		private void _cmpAnimation()
		{
			string empty = string.Empty;
			string message = (!_isUseJukuren) ? $"{mSlotItemTo.Name}に装備が改修更新されました！" : $"{mSlotItemTo.Name}に部隊再編完了！";
			_clsShipMessage.Init(message, 0.04f);
			_clsShipMessage.Play();
		}

		private void _changeDepth()
		{
			_uiItem1.depth = 3;
			_uiItem2.depth = 4;
		}

		private void strtMessageBox()
		{
			_uiMessageBG.alpha = 1f;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("time", 0.5f);
			hashtable.Add("y", -187f);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("isLocal", true);
			_uiMessageBG.transform.MoveTo(hashtable);
		}

		private void strtGearIcon()
		{
			_uiGear.alpha = 1f;
			_isUpdateNextBtn = true;
			_clsShipMessage.Play();
			_isInput = true;
		}

		private void strtGetIcon()
		{
		}

		private void FadeOutExtinguish()
		{
			this.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, "_onFadeOutExtinguishFinished");
		}

		private void _onFadeOutExtinguishFinished()
		{
			_isFinished = true;
			if (_actCallback != null)
			{
				_actCallback();
			}
			Discard();
		}

		private void Discard()
		{
			UnityEngine.Object.Destroy(base.gameObject, 0.1f);
		}

		private void ProdReceiveShipEL(GameObject obj)
		{
			if (_isInput)
			{
				_isInput = false;
				FadeOutExtinguish();
			}
		}

		private void OnDestriy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref _uiRareBG);
			UserInterfacePortManager.ReleaseUtils.Release(ref _uiItem1);
			UserInterfacePortManager.ReleaseUtils.Release(ref _uiItem2);
			UserInterfacePortManager.ReleaseUtils.Release(ref _uiMessageBG);
			UserInterfacePortManager.ReleaseUtils.Release(ref _uiGearBtn);
			UserInterfacePortManager.ReleaseUtils.Release(ref _uiGear);
			UserInterfacePortManager.ReleaseUtils.Release(ref _widgetJukurenParent);
			if ((UnityEngine.Object)_getIconAnim != null && _getIconAnim.isPlaying)
			{
				_getIconAnim.Stop();
			}
			_getIconAnim = null;
			if ((UnityEngine.Object)_anim != null && _anim.isPlaying)
			{
				_anim.Stop();
			}
			_anim = null;
		}
	}
}
