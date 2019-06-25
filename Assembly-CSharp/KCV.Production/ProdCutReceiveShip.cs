using KCV.Utils;
using local.models;
using local.utils;
using System;
using UnityEngine;

namespace KCV.Production
{
	public class ProdCutReceiveShip : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiRareBG;

		[SerializeField]
		private UITexture _uiShip;

		[SerializeField]
		private UILabel _clsShipName;

		[SerializeField]
		private UILabel _clsSType;

		[SerializeField]
		private UITexture _uiMessageBG;

		[SerializeField]
		private UIButtonMessage _uiGearBtn;

		[SerializeField]
		private ParticleSystem _uiParStar1;

		[SerializeField]
		private ParticleSystem _uiParStar2;

		[SerializeField]
		private Animation _anim;

		[SerializeField]
		private Animation _getIconAnim;

		[SerializeField]
		private Animation _gearAnim;

		private Generics.Message _clsShipMessage;

		private AudioSource _Se;

		private IReward_Ship _clsRewardShip;

		private Action _actCallback;

		private bool _isFinished;

		private bool _isInput;

		private bool _isNeedBGM;

		private bool _isBGMove;

		private KeyControl _clsInput;

		public bool IsFinished => _isFinished;

		public void Init()
		{
			if ((UnityEngine.Object)_anim == null)
			{
				_anim = GetComponent<Animation>();
			}
			Util.FindParentToChild(ref _uiRareBG, base.transform, "RareBG");
			Util.FindParentToChild(ref _uiShip, base.transform, "ShipLayoutOffset/Ship");
			Util.FindParentToChild(ref _clsShipName, base.transform, "MessageWindow/ShipName");
			Util.FindParentToChild(ref _clsSType, base.transform, "MessageWindow/ShipType");
			Util.FindParentToChild(ref _uiMessageBG, base.transform, "MessageWindow/MessageBG");
			Util.FindParentToChild(ref _uiGearBtn, base.transform, "MessageWindow/NextBtn");
			Util.FindParentToChild<ParticleSystem>(ref _uiParStar1, base.transform, "ParticleStar1");
			Util.FindParentToChild<ParticleSystem>(ref _uiParStar2, base.transform, "ParticleStar2");
			Util.FindParentToChild<Animation>(ref _getIconAnim, base.transform, "MessageWindow/Get");
			if ((UnityEngine.Object)_gearAnim == null)
			{
				_gearAnim = _uiGearBtn.GetComponent<Animation>();
			}
			_clsShipMessage = new Generics.Message(base.transform, "MessageWindow/ShipMessage");
			_uiShip.alpha = 0f;
			_isFinished = false;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiRareBG);
			Mem.Del(ref _uiShip);
			Mem.Del(ref _clsShipName);
			Mem.Del(ref _clsSType);
			Mem.Del(ref _uiMessageBG);
			Mem.Del(ref _uiGearBtn);
			Mem.Del(ref _uiParStar1);
			Mem.Del(ref _uiParStar2);
			Mem.Del(ref _getIconAnim);
			Mem.Del(ref _gearAnim);
			_clsShipMessage.UnInit();
			Mem.Del(ref _Se);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _clsInput);
		}

		private void Update()
		{
			_clsShipMessage.Update();
			if (_isInput && _clsInput.keyState[1].down)
			{
				FadeOutExtinguish();
				_isInput = false;
			}
		}

		public static ProdCutReceiveShip Instantiate(ProdCutReceiveShip prefab, Transform parent, IReward_Ship rewardShip, int nPanelDepth, KeyControl input)
		{
			return Instantiate(prefab, parent, rewardShip, nPanelDepth, input, needBGM: true);
		}

		public static ProdCutReceiveShip Instantiate(ProdCutReceiveShip prefab, Transform parent, IReward_Ship rewardShip, int nPanelDepth, KeyControl input, bool needBGM)
		{
			ProdCutReceiveShip prodCutReceiveShip = UnityEngine.Object.Instantiate(prefab);
			prodCutReceiveShip.transform.parent = parent;
			prodCutReceiveShip.transform.localScale = Vector3.one;
			prodCutReceiveShip.transform.localPosition = Vector3.zero;
			prodCutReceiveShip.Init();
			prodCutReceiveShip._clsRewardShip = rewardShip;
			prodCutReceiveShip._setRewardShip();
			prodCutReceiveShip.GetComponent<UIPanel>().depth = nPanelDepth;
			prodCutReceiveShip._clsInput = input;
			prodCutReceiveShip._isNeedBGM = needBGM;
			prodCutReceiveShip._anim.Stop();
			return prodCutReceiveShip;
		}

		private void _setRewardShip()
		{
			_uiShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(_clsRewardShip.Ship.GetGraphicsMstId(), 9);
			_uiShip.MakePixelPerfect();
			_uiShip.transform.localPosition = Util.Poi2Vec(new ShipOffset(_clsRewardShip.Ship.GetGraphicsMstId()).GetShipDisplayCenter(damaged: false));
			UIButtonMessage component = _uiGearBtn.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "ProdReceiveShipEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			_clsShipMessage.Init(_clsRewardShip.GreetingText, 0.08f);
			_clsShipName.text = _clsRewardShip.Ship.Name;
			_clsSType.text = _clsRewardShip.Ship.ShipTypeName;
			_clsShipName.SetActive(isActive: false);
			_clsSType.SetActive(isActive: false);
			((Component)_getIconAnim).gameObject.SetActive(false);
			_isBGMove = false;
		}

		public void Play(Action callback)
		{
			_anim.Play("comp_GetShip");
			_actCallback = callback;
			_uiParStar1.Play();
			_uiParStar2.Play();
			_uiShip.alpha = 1f;
			_uiRareBG.mainTexture = TextureFile.LoadRareBG(_clsRewardShip.Ship.Rare);
			_uiMessageBG.alpha = 0.75f;
			_Se = SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
		}

		public void showMessage()
		{
			_clsShipName.SetActive(isActive: true);
			_clsSType.SetActive(isActive: true);
			_clsShipName.gameObject.SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _clsShipName.gameObject, string.Empty);
			_clsSType.gameObject.SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _clsSType.gameObject, string.Empty);
			TrophyUtil.Unlock_GetShip(_clsRewardShip.Ship.MstId);
		}

		private void startMessage()
		{
			StartGetAnim();
			_clsShipMessage.Play();
			showMessage();
			ShipUtils.PlayShipVoice(_clsRewardShip.Ship, 1);
			_gearAnim.Stop();
			_gearAnim.Play();
			_isInput = true;
		}

		public void StartGetAnim()
		{
			((Component)_getIconAnim).gameObject.SetActive(true);
			_getIconAnim.Stop();
			_getIconAnim.Play();
		}

		private void FadeOutExtinguish()
		{
			this.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, "_onFadeOutExtinguishFinished");
		}

		private void ProdReceiveShipEL(GameObject obj)
		{
			if (_isInput)
			{
				_isInput = false;
				FadeOutExtinguish();
				SoundUtils.StopSE(0.5f, _Se);
			}
		}

		private void _onFadeOutExtinguishFinished()
		{
			if (_actCallback != null)
			{
				_actCallback();
			}
			UnityEngine.Object.Destroy(base.gameObject, 0.1f);
		}
	}
}
