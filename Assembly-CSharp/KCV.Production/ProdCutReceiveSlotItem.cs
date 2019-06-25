using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Production
{
	public class ProdCutReceiveSlotItem : MonoBehaviour
	{
		[SerializeField]
		private Animation _anime;

		[SerializeField]
		private Animation _getAnime;

		[SerializeField]
		private UITexture _uiRareBG;

		[SerializeField]
		private UITexture _uiItem;

		[SerializeField]
		private UITexture _uiMessageBG;

		[SerializeField]
		private UISprite _uiShipGet;

		[SerializeField]
		private UISprite _uiGear;

		[SerializeField]
		private UISprite _uiInner;

		[SerializeField]
		private ParticleSystem _uiStarPar1;

		[SerializeField]
		private ParticleSystem _uiStarPar2;

		private Generics.Message _clsShipMessage;

		private AudioSource _Se;

		private bool _isStartAnim;

		private bool _isInput;

		private bool _isUpdateShipGet;

		private bool _isUpdateNextBtn;

		private KeyControl _clsInput;

		private Action _actCallback;

		private IReward_Slotitem _clsRewardItem;

		private void Awake()
		{
			Util.FindParentToChild(ref _uiRareBG, base.transform, "RareBG");
			Util.FindParentToChild(ref _uiItem, base.transform, "Item");
			Util.FindParentToChild(ref _uiMessageBG, base.transform, "MessageWindow/MessageBG");
			Util.FindParentToChild(ref _uiShipGet, base.transform, "MessageWindow/Get");
			Util.FindParentToChild(ref _uiGear, base.transform, "MessageWindow/NextBtn");
			Util.FindParentToChild(ref _uiInner, base.transform, "MessageWindow/NextBtn/Gear");
			Util.FindParentToChild<ParticleSystem>(ref _uiStarPar1, base.transform, "ParticleStar1");
			Util.FindParentToChild<ParticleSystem>(ref _uiStarPar2, base.transform, "ParticleStar2");
			_clsShipMessage = new Generics.Message(base.transform, "MessageWindow/ShipMessage");
			_anime = GetComponent<Animation>();
			_getAnime = _uiShipGet.GetComponent<Animation>();
			_uiItem.alpha = 0f;
			((Component)_uiStarPar1).SetActive(isActive: false);
			((Component)_uiStarPar2).SetActive(isActive: false);
		}

		private void OnDestroy()
		{
			_anime = null;
			_uiRareBG = null;
			_uiItem = null;
			_uiMessageBG = null;
			_uiShipGet = null;
			_uiGear = null;
			_uiInner = null;
			_clsShipMessage.UnInit();
			_actCallback = null;
			_clsInput = null;
		}

		private void Update()
		{
			if (_isUpdateNextBtn)
			{
				_clsShipMessage.Update();
			}
			if (_isUpdateNextBtn)
			{
				_uiInner.transform.Rotate(-50f * Time.deltaTime * Vector3.forward);
			}
			if (_isInput && _clsInput.keyState[1].down)
			{
				_fadeOutExtinguish();
				_isInput = false;
			}
		}

		public void Init(IReward_Slotitem rewardItem)
		{
			_setRewardItem(rewardItem);
			_uiRareBG.alpha = 0f;
			_uiRareBG.mainTexture = TextureFile.LoadRareBG(1);
			_uiItem.alpha = 0f;
			_uiStarPar1.Stop();
			_uiStarPar2.Stop();
			((Component)_uiStarPar1).SetActive(isActive: false);
			((Component)_uiStarPar2).SetActive(isActive: false);
		}

		public static ProdCutReceiveSlotItem Instantiate(ProdCutReceiveSlotItem prefab, Transform parent, IReward_Slotitem rewardItem, int nPanelDepth, KeyControl input)
		{
			ProdCutReceiveSlotItem prodCutReceiveSlotItem = UnityEngine.Object.Instantiate(prefab);
			prodCutReceiveSlotItem.transform.parent = parent;
			prodCutReceiveSlotItem.transform.localScale = Vector3.one;
			prodCutReceiveSlotItem.transform.localPosition = Vector3.zero;
			prodCutReceiveSlotItem._setRewardItem(rewardItem);
			prodCutReceiveSlotItem._uiRareBG.alpha = 0f;
			prodCutReceiveSlotItem._uiRareBG.mainTexture = TextureFile.LoadRareBG(1);
			prodCutReceiveSlotItem.GetComponent<UIPanel>().depth = nPanelDepth;
			prodCutReceiveSlotItem._clsInput = input;
			return prodCutReceiveSlotItem;
		}

		private void _setRewardItem(IReward_Slotitem rewardItem)
		{
			_clsRewardItem = rewardItem;
			_uiItem.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(_clsRewardItem.Id, 1);
			_uiItem.MakePixelPerfect();
			_uiShipGet.alpha = 0f;
			_uiGear.alpha = 0f;
			_isUpdateShipGet = false;
			_isUpdateNextBtn = false;
			_clsShipMessage.Init(_clsRewardItem.Type3Name + "「" + _clsRewardItem.Name + "」を入手しました。", 0.04f);
			UIButtonMessage component = _uiGear.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "_receiveShipEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		public void Play(Action callback)
		{
			_isStartAnim = true;
			_uiRareBG.alpha = 1f;
			_uiItem.alpha = 1f;
			_actCallback = callback;
			_anime.Stop();
			_anime.Play("start_GetSlotItemCut");
			((Component)_uiStarPar1).SetActive(isActive: true);
			((Component)_uiStarPar2).SetActive(isActive: true);
			_uiStarPar1.Play();
			_uiStarPar2.Play();
			_Se = SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
		}

		private void _startMessageBox()
		{
			_uiMessageBG.alpha = 1f;
		}

		private void _startGearIcon()
		{
			_uiGear.alpha = 1f;
			_isUpdateNextBtn = true;
			_getAnime.Stop();
			_getAnime.Play();
			_clsShipMessage.Play();
			_isInput = true;
		}

		private void _startGetIcon()
		{
			_uiShipGet.alpha = 1f;
			_isUpdateShipGet = true;
		}

		private void _fadeOutExtinguish()
		{
			SingletonMonoBehaviour<SoundManager>.Instance.StopBGM();
			SoundUtils.StopSE(0.5f, _Se);
			this.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, "_onFadeOutExtinguishFinished");
		}

		private void _onFadeOutExtinguishFinished()
		{
			if (_actCallback != null)
			{
				_actCallback();
			}
			_discard();
		}

		private void _discard()
		{
			UnityEngine.Object.Destroy(base.gameObject, 0.1f);
		}

		private void _receiveShipEL(GameObject obj)
		{
			if (_isInput)
			{
				_isInput = false;
				_fadeOutExtinguish();
			}
		}
	}
}
