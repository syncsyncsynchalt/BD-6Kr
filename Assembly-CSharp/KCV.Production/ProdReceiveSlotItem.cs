using KCV.Utils;
using local.models;
using local.utils;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.Production
{
	public class ProdReceiveSlotItem : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _uiPar;

		[SerializeField]
		private ParticleSystem _uiParticleComp;

		[SerializeField]
		private ParticleSystem _uiParticleStar1;

		[SerializeField]
		private ParticleSystem _uiParticleStar2;

		[SerializeField]
		private ParticleSystem _uiParticleMiss;

		[SerializeField]
		private UITexture _uiRareBG;

		[SerializeField]
		private UITexture _uiItem;

		[SerializeField]
		private GameObject _missObj;

		[SerializeField]
		private UISprite _uiShipGet;

		[SerializeField]
		private UISprite _uiGear;

		[SerializeField]
		private UIButton _uiBG;

		[SerializeField]
		protected Animation _getIconAnim;

		private Generics.Message _clsShipMessage;

		protected AudioSource _Se;

		private IReward_Slotitem _clsRewardItem;

		private Animation _anime;

		private Animation _gearAnime;

		private Action _actCallback;

		private int _rewardCount;

		private bool _isStartAnim;

		private bool _isEnabled;

		private bool _isExtinguish;

		private bool _isInput;

		private bool _isArsenal;

		private bool _isPlayPhase1;

		private bool _isUpdateNextBtn;

		private int timer;

		private KeyControl _clsInput;

		private int debugIndex;

		private bool _gearTouchableflag;

		private void Awake()
		{
			Util.FindParentToChild<ParticleSystem>(ref _uiPar, base.transform, "Particle");
			Util.FindParentToChild<ParticleSystem>(ref _uiParticleComp, base.transform, "ParticleComp");
			Util.FindParentToChild<ParticleSystem>(ref _uiParticleMiss, base.transform, "Miss/Reaf/MissPar");
			Util.FindParentToChild<ParticleSystem>(ref _uiParticleStar1, base.transform, "ParticleStar1");
			Util.FindParentToChild<ParticleSystem>(ref _uiParticleStar2, base.transform, "ParticleStar2");
			Util.FindParentToChild(ref _uiRareBG, base.transform, "RareBG");
			Util.FindParentToChild(ref _uiBG, base.transform, "BG");
			Util.FindParentToChild(ref _uiItem, base.transform, "Item");
			Util.FindParentToChild(ref _uiShipGet, base.transform, "MessageWindow/Get");
			Util.FindParentToChild(ref _uiGear, base.transform, "MessageWindow/NextBtn");
			Util.FindParentToChild<Animation>(ref _getIconAnim, base.transform, "MessageWindow/Get");
			_anime = GetComponent<Animation>();
			_gearAnime = _uiGear.GetComponent<Animation>();
			_missObj = base.transform.FindChild("Miss").gameObject;
			_clsShipMessage = new Generics.Message(base.transform, "MessageWindow/ShipMessage");
			((Component)_uiParticleComp).SetActive(isActive: false);
			((Component)_uiParticleStar1).SetActive(isActive: false);
			((Component)_uiParticleStar2).SetActive(isActive: false);
			_uiItem.alpha = 0f;
			_isExtinguish = false;
			debugIndex = 0;
			_rewardCount = 0;
		}

		public void Init()
		{
			((Component)_uiParticleComp).SetActive(isActive: false);
			((Component)_uiParticleStar1).SetActive(isActive: false);
			((Component)_uiParticleStar2).SetActive(isActive: false);
			_uiItem.alpha = 0f;
			_isExtinguish = false;
			_uiRareBG.alpha = 0f;
			_uiPar.Stop();
			_isStartAnim = false;
			_isPlayPhase1 = false;
			_isEnabled = base.enabled;
			_uiBG.enabled = true;
			_gearTouchableflag = false;
		}

		private void OnDestroy()
		{
			_anime = null;
			_uiPar = null;
			_uiRareBG = null;
			_uiItem = null;
			_clsShipMessage.UnInit();
			_actCallback = null;
			_isStartAnim = false;
			_clsInput = null;
			Mem.Del(ref _Se);
			Mem.Del(ref _getIconAnim);
		}

		private void Update()
		{
			if (_isUpdateNextBtn)
			{
				_clsShipMessage.Update();
			}
			if (timer <= 1)
			{
				timer++;
			}
			else if (_isInput && _clsInput.keyState[1].down)
			{
				if (_isPlayPhase1)
				{
					compStartAnimation();
					return;
				}
				fadeOutExtinguish();
				_isInput = false;
			}
		}

		public static ProdReceiveSlotItem Instantiate(ProdReceiveSlotItem prefab, Transform parent, IReward_Slotitem rewardItem, int nPanelDepth, KeyControl input, bool enabled, bool arsenal)
		{
			ProdReceiveSlotItem prodReceiveSlotItem = UnityEngine.Object.Instantiate(prefab);
			prodReceiveSlotItem.transform.parent = parent;
			prodReceiveSlotItem.transform.localScale = Vector3.one;
			prodReceiveSlotItem.transform.localPosition = Vector3.zero;
			prodReceiveSlotItem._isStartAnim = false;
			prodReceiveSlotItem._isPlayPhase1 = false;
			input.keyState[1].down = false;
			prodReceiveSlotItem._clsInput = input;
			prodReceiveSlotItem.GetComponent<UIPanel>().depth = nPanelDepth;
			if (enabled)
			{
				prodReceiveSlotItem._setRewardItem(rewardItem);
				prodReceiveSlotItem._uiRareBG.alpha = 0f;
			}
			else
			{
				prodReceiveSlotItem._setRewardMiss();
			}
			prodReceiveSlotItem._missObj.SetActive(false);
			if (parent.transform.parent.name == "TaskArsenalMain" || parent.transform.parent.name == "DescriptionCamera")
			{
				prodReceiveSlotItem._isArsenal = true;
			}
			prodReceiveSlotItem._uiPar.Stop();
			prodReceiveSlotItem._isStartAnim = true;
			prodReceiveSlotItem._isEnabled = enabled;
			prodReceiveSlotItem._isArsenal = arsenal;
			return prodReceiveSlotItem;
		}

		private void _setRewardItem(IReward_Slotitem rewardItem)
		{
			_clsRewardItem = rewardItem;
			_uiItem.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(_clsRewardItem.Id, 1);
			_uiItem.MakePixelPerfect();
			_uiShipGet.alpha = 0f;
			_isUpdateNextBtn = false;
			_clsShipMessage.Init(_clsRewardItem.Type3Name + "「" + _clsRewardItem.Name + "」を入手しました。", 0.04f);
			int num = 0;
			switch (_clsRewardItem.Rare)
			{
			case 0:
				num = 1;
				break;
			case 1:
				num = 2;
				break;
			case 2:
				num = 6;
				break;
			case 3:
				num = 6;
				break;
			case 4:
				num = 6;
				break;
			case 5:
				num = 7;
				break;
			}
			string str = (_clsRewardItem.Rare < 2) ? "i_rare" : "s_rare_";
			_uiRareBG.mainTexture = (Resources.Load(string.Format("Textures/Common/RareBG/" + str + "{0}", num)) as Texture2D);
			Debug.Log(string.Format("Textures/Common/RareBG/" + str + "{0}", num));
			UIButtonMessage component = _uiGear.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "prodReceiveShipEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		private void _debugRewardItem()
		{
			Debug.Log("ItemID:" + debugIndex);
			if (Mst_DataManager.Instance.Mst_Slotitem.ContainsKey(debugIndex))
			{
				IReward_Slotitem rewardItem = new Reward_Slotitem(debugIndex);
				_setRewardItem(rewardItem);
			}
			_anime.Stop();
			_anime.Play("comp_GetSlotItem");
		}

		private void _setRewardMiss()
		{
			_uiShipGet.alpha = 0f;
			_isUpdateNextBtn = false;
			_clsShipMessage.Init(" 装備の開発に失敗しました。\n\n『開発資材』は消費しませんでした。", 0.04f);
			UIButtonMessage component = _uiGear.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "prodReceiveShipEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		public void Play(Action callback)
		{
			_anime.Play("start_GetSlotItem");
			_uiPar.Play();
			_isStartAnim = true;
			_isInput = true;
			_isPlayPhase1 = true;
			timer = 0;
			_actCallback = callback;
			_rewardCount++;
			_Se = SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
		}

		public void NextPlay(Action callback)
		{
			_anime.Stop();
			_anime.Play("start_GetSlotItem");
			_uiPar.Play();
			_isStartAnim = true;
			_actCallback = callback;
			_Se = SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
		}

		public void onClick()
		{
			compStartAnimation();
		}

		public void onScreenTap()
		{
			compStartAnimation();
		}

		private void compStartAnimation()
		{
			_uiBG.enabled = false;
			_isInput = false;
			_isPlayPhase1 = false;
			_uiPar.Stop();
			_uiPar.time = 0f;
			((Component)_uiPar).gameObject.SetActive(false);
			if (_isEnabled)
			{
				_uiRareBG.alpha = 1f;
				_uiItem.alpha = 1f;
				if (_isArsenal)
				{
					((Component)_uiParticleComp).SetActive(isActive: true);
					_uiParticleComp.Play();
				}
				else
				{
					((Component)_uiParticleStar1).SetActive(isActive: true);
					((Component)_uiParticleStar2).SetActive(isActive: true);
					_uiParticleStar1.Play();
					_uiParticleStar2.Play();
				}
				_anime.Stop();
				_anime.Play("comp_GetSlotItem");
				TrophyUtil.Unlock_AlbumSlotNum();
			}
			else
			{
				_anime.Stop();
				_anime.Play("miss_GetSlotItem");
				_missObj.SetActive(true);
				_uiParticleMiss.Play();
			}
		}

		private void Success_Voice()
		{
			if (_isEnabled && base.transform.parent.parent.name == "TaskArsenalMain")
			{
				ShipUtils.PlayShipVoice(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.GetFlagShip(), 26);
			}
		}

		private void Success_SE()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_010);
		}

		private void Failure_SE()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_011);
		}

		private void compMissAnimation()
		{
			_uiParticleMiss.Stop();
		}

		private void startGearIcon()
		{
			_isUpdateNextBtn = true;
			_gearAnime.Stop();
			_gearAnime.Play();
			_clsShipMessage.Play();
			_isInput = true;
			_gearTouchableflag = true;
		}

		private void startGetIcon()
		{
			_uiShipGet.alpha = 1f;
			_getIconAnim.Stop();
			_getIconAnim.Play();
		}

		private void fadeOutExtinguish()
		{
			if (_isArsenal)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.StopSE();
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			SoundUtils.StopSE(0.5f, _Se);
			_uiParticleComp.Stop();
			((Component)_uiParticleComp).SetActive(isActive: false);
			_uiParticleMiss.Stop();
			((Component)_uiParticleMiss).SetActive(isActive: false);
			_uiParticleStar1.Stop();
			((Component)_uiParticleStar1).SetActive(isActive: false);
			_uiParticleStar2.Stop();
			((Component)_uiParticleStar2).SetActive(isActive: false);
			this.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, "_onFadeOutExtinguishFinished");
		}

		private void _onFadeOutExtinguishFinished()
		{
			if (_actCallback != null)
			{
				_actCallback();
			}
			discard();
		}

		private void discard()
		{
			UnityEngine.Object.Destroy(base.gameObject, 0.1f);
		}

		private void prodReceiveShipEL(GameObject obj)
		{
			if (_gearTouchableflag && _isInput)
			{
				_isInput = false;
				fadeOutExtinguish();
			}
		}

		public void ReleaseTextures()
		{
			_uiPar = null;
			if ((UnityEngine.Object)_uiParticleComp != null)
			{
				_uiParticleComp.Stop();
			}
			_uiParticleComp = null;
			if ((UnityEngine.Object)_uiParticleStar1 != null)
			{
				_uiParticleStar1.Stop();
			}
			_uiParticleStar1 = null;
			if ((UnityEngine.Object)_uiParticleStar2 != null)
			{
				_uiParticleStar2.Stop();
			}
			_uiParticleStar2 = null;
			if ((UnityEngine.Object)_uiParticleMiss != null)
			{
				_uiParticleMiss.Stop();
			}
			_uiParticleMiss = null;
			if (_uiRareBG != null)
			{
				if (_uiRareBG.mainTexture != null)
				{
					Resources.UnloadAsset(_uiRareBG.mainTexture);
				}
				_uiRareBG.mainTexture = null;
			}
			_uiRareBG = null;
			if (_uiItem != null)
			{
				if (_uiItem.mainTexture != null)
				{
					Resources.UnloadAsset(_uiItem.mainTexture);
				}
				_uiItem.mainTexture = null;
			}
			_uiItem = null;
			_missObj = null;
			_uiShipGet = null;
			_uiGear = null;
			_uiBG = null;
			if ((UnityEngine.Object)_getIconAnim != null)
			{
				_getIconAnim.Stop();
			}
			_getIconAnim = null;
			_Se = null;
			_clsRewardItem = null;
			if ((UnityEngine.Object)_anime != null)
			{
				_anime.Stop();
			}
			_anime = null;
			if ((UnityEngine.Object)_gearAnime != null)
			{
				_gearAnime.Stop();
			}
			_gearAnime = null;
			_actCallback = null;
			_clsInput = null;
		}
	}
}
