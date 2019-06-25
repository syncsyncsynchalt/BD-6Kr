using KCV.Utils;
using local.models;
using local.utils;
using Server_Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Production
{
	public class ProdBattleRewardItem : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _uiPar;

		[SerializeField]
		private ParticleSystem _uiParticleComp;

		[SerializeField]
		private ParticleSystem _uiParticleStar1;

		[SerializeField]
		private UITexture _uiRareBG;

		[SerializeField]
		private UITexture _uiItem;

		[SerializeField]
		private UISprite _uiShipGet;

		[SerializeField]
		private UISprite _uiGear;

		[SerializeField]
		private Animation _getAnim;

		[SerializeField]
		private Animation _anim;

		private Generics.Message _clsShipMessage;

		private List<IReward> _iRewardList;

		private Animation _anime;

		private Animation _gearAnime;

		private Action _actCallback;

		private int _rewardCount;

		private bool _isInput;

		private KeyControl _clsInput;

		private int debugIndex;

		public void Init()
		{
			Util.FindParentToChild<ParticleSystem>(ref _uiPar, base.transform, "Particle");
			Util.FindParentToChild<ParticleSystem>(ref _uiParticleComp, base.transform, "ParticleComp");
			Util.FindParentToChild<ParticleSystem>(ref _uiParticleStar1, base.transform, "ParticleStar1");
			Util.FindParentToChild(ref _uiRareBG, base.transform, "RareBG");
			Util.FindParentToChild(ref _uiItem, base.transform, "Item");
			Util.FindParentToChild(ref _uiShipGet, base.transform, "MessageWindow/Get");
			Util.FindParentToChild(ref _uiGear, base.transform, "MessageWindow/NextBtn");
			Util.FindParentToChild<Animation>(ref _getAnim, base.transform, "MessageWindow/Get");
			if ((UnityEngine.Object)_anim == null)
			{
				_anim = GetComponent<Animation>();
			}
			if ((UnityEngine.Object)_gearAnime == null)
			{
				_gearAnime = _uiGear.GetComponent<Animation>();
			}
			_clsShipMessage = new Generics.Message(base.transform, "MessageWindow/ShipMessage");
			((Component)_uiParticleComp).SetActive(isActive: false);
			((Component)_uiParticleStar1).SetActive(isActive: false);
			_uiItem.alpha = 0f;
			debugIndex = 0;
			_rewardCount = 0;
			((Component)_uiParticleComp).SetActive(isActive: false);
			((Component)_uiParticleStar1).SetActive(isActive: false);
			_uiItem.alpha = 0f;
			_setRewardItem();
			_uiRareBG.alpha = 0f;
			_uiPar.Stop();
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiPar);
			Mem.Del(ref _uiParticleComp);
			Mem.Del(ref _uiParticleStar1);
			Mem.Del(ref _uiRareBG);
			Mem.Del(ref _uiItem);
			Mem.Del(ref _uiShipGet);
			Mem.Del(ref _uiGear);
			Mem.Del(ref _getAnim);
			Mem.Del(ref _anim);
			_clsShipMessage.UnInit();
			Mem.Del(ref _anime);
			Mem.Del(ref _gearAnime);
			Mem.Del(ref _actCallback);
			Mem.DelList(ref _iRewardList);
			_clsInput = null;
		}

		private void Update()
		{
			_clsShipMessage.Update();
			if (_isInput && _clsInput.keyState[1].down)
			{
				fadeOutExtinguish();
				_isInput = false;
			}
		}

		public static ProdBattleRewardItem Instantiate(ProdBattleRewardItem prefab, Transform parent, List<IReward> iReward, int nDepth, KeyControl input)
		{
			ProdBattleRewardItem prodBattleRewardItem = UnityEngine.Object.Instantiate(prefab);
			prodBattleRewardItem.transform.parent = parent;
			prodBattleRewardItem.transform.localScale = Vector3.one;
			prodBattleRewardItem.transform.localPosition = Vector3.zero;
			prodBattleRewardItem.Init();
			prodBattleRewardItem._clsInput = input;
			prodBattleRewardItem.GetComponent<UIPanel>().depth = nDepth;
			prodBattleRewardItem._iRewardList = iReward;
			prodBattleRewardItem._setRewardItem();
			prodBattleRewardItem._uiRareBG.alpha = 0f;
			prodBattleRewardItem._uiPar.Stop();
			return prodBattleRewardItem;
		}

		private void _setRewardItem()
		{
			int slotItemID = 0;
			int num = 0;
			int num2 = 0;
			string message = string.Empty;
			if (_iRewardList[_rewardCount] is IReward_Materials)
			{
				IReward_Materials reward_Materials = (IReward_Materials)_iRewardList[_rewardCount];
				num = 1;
				message = "「" + reward_Materials.Rewards[0].Name + "」を" + reward_Materials.Rewards[0].Count + "入手しました。";
			}
			if (_iRewardList[_rewardCount] is IReward_Slotitem)
			{
				IReward_Slotitem reward_Slotitem = (IReward_Slotitem)_iRewardList[_rewardCount];
				slotItemID = reward_Slotitem.Id;
				switch (reward_Slotitem.Rare)
				{
				case 0:
					num = 1;
					num2 = 1;
					break;
				case 1:
					num = 2;
					num2 = 1;
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
				message = reward_Slotitem.Type3Name + "「" + reward_Slotitem.Name + "」を入手しました。";
			}
			if (_iRewardList[_rewardCount] is IReward_Useitem)
			{
				IReward_Useitem reward_Useitem = (IReward_Useitem)_iRewardList[_rewardCount];
				slotItemID = reward_Useitem.Id;
				num = 1;
				message = "「" + reward_Useitem.Name + "」を入手しました。";
			}
			_uiItem.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(slotItemID, 1);
			_uiItem.MakePixelPerfect();
			_uiShipGet.alpha = 0f;
			_clsShipMessage.Init(message, 0.04f);
			string str = (num2 != 0) ? "i_rare" : "s_rare";
			_uiRareBG.mainTexture = (Resources.Load(string.Format("Textures/Common/RareBG/" + str + "{0}", num)) as Texture2D);
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
				new Reward_Slotitem(debugIndex);
				_setRewardItem();
			}
			_anime.Stop();
			_anime.Play("comp_GetSlotItem");
		}

		public void Play(Action callback)
		{
			_anime.Play("start_GetSlotItem");
			_uiPar.Play();
			_actCallback = callback;
			_rewardCount++;
			SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
		}

		public void NextPlay(Action callback)
		{
			_anime.Stop();
			_anime.Play("start_GetSlotItem");
			_uiPar.Play();
			_actCallback = callback;
			SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
		}

		private void onClick()
		{
			compStartAnimation();
		}

		private void compStartAnimation()
		{
			_uiPar.Stop();
			_uiPar.time = 0f;
			((Component)_uiPar).gameObject.SetActive(false);
			_uiRareBG.alpha = 1f;
			_uiItem.alpha = 1f;
			((Component)_uiParticleComp).SetActive(isActive: true);
			_uiParticleComp.Play();
			((Component)_uiParticleStar1).SetActive(isActive: true);
			_uiParticleStar1.Play();
			_anime.Stop();
			_anime.Play("comp_GetSlotItem");
			TrophyUtil.Unlock_AlbumSlotNum();
		}

		private void startGearIcon()
		{
			_gearAnime.Stop();
			_gearAnime.Play();
			_clsShipMessage.Play();
			_isInput = true;
		}

		private void startGetIcon()
		{
			_uiShipGet.alpha = 1f;
			((Component)_getAnim).gameObject.SetActive(true);
			_getAnim.Stop();
			_getAnim.Play();
		}

		private void fadeOutExtinguish()
		{
			SingletonMonoBehaviour<SoundManager>.Instance.StopSE();
			_uiParticleComp.Stop();
			((Component)_uiParticleComp).SetActive(isActive: false);
			_uiParticleStar1.Stop();
			((Component)_uiParticleStar1).SetActive(isActive: false);
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
			if (_isInput)
			{
				_isInput = false;
				fadeOutExtinguish();
			}
		}
	}
}
