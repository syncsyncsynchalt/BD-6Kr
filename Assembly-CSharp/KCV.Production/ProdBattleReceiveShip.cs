using KCV.Utils;
using local.models;
using local.utils;
using System;
using UnityEngine;

namespace KCV.Production
{
	public class ProdBattleReceiveShip : BaseReceiveShip
	{
		[SerializeField]
		protected ParticleSystem _uiParStar1;

		[SerializeField]
		protected ParticleSystem _uiParStar2;

		[SerializeField]
		private ParticleSystem _uiParticle;

		private bool _isNeedBGM;

		private bool _isPlayPhase1;

		private int timer;

		protected override void init()
		{
			GetComponent<UIPanel>().alpha = 0f;
			base.init();
			Util.FindParentToChild<ParticleSystem>(ref _uiParStar1, base.transform, "ParticleStar1");
			Util.FindParentToChild<ParticleSystem>(ref _uiParStar2, base.transform, "ParticleStar2");
			Util.FindParentToChild<ParticleSystem>(ref _uiParticle, base.transform, "Particle");
			_uiShip.alpha = 0f;
			((Component)_uiParticle).SetActive(isActive: false);
			((Component)_uiParStar1).SetActive(isActive: false);
			((Component)_uiParStar2).SetActive(isActive: false);
			timer = 0;
			_isPlayPhase1 = false;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiParStar1);
			Mem.Del(ref _uiParStar2);
			Mem.Del(ref _uiParticle);
		}

		private void Update()
		{
			Run();
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
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				FadeOutExtinguish();
				_isInput = false;
			}
		}

		public static ProdBattleReceiveShip Instantiate(ProdBattleReceiveShip prefab, Transform parent, IReward_Ship rewardShip, int nPanelDepth, KeyControl input)
		{
			return Instantiate(prefab, parent, rewardShip, nPanelDepth, input, needBGM: true);
		}

		public static ProdBattleReceiveShip Instantiate(ProdBattleReceiveShip prefab, Transform parent, IReward_Ship rewardShip, int nPanelDepth, KeyControl input, bool needBGM)
		{
			ProdBattleReceiveShip prodBattleReceiveShip = UnityEngine.Object.Instantiate(prefab);
			prodBattleReceiveShip.transform.parent = parent;
			prodBattleReceiveShip.transform.localScale = Vector3.one;
			prodBattleReceiveShip.transform.localPosition = Vector3.zero;
			prodBattleReceiveShip.init();
			prodBattleReceiveShip._clsRewardShip = rewardShip;
			prodBattleReceiveShip.GetComponent<UIPanel>().depth = nPanelDepth;
			prodBattleReceiveShip._clsInput = input;
			prodBattleReceiveShip._isNeedBGM = needBGM;
			prodBattleReceiveShip._anim.Stop();
			return prodBattleReceiveShip;
		}

		public void Play(Action callback)
		{
			_setRewardShip();
			_actCallback = callback;
			GetComponent<UIPanel>().alpha = 1f;
			_anim.Play("start_GetShip");
			((Component)_uiParticle).SetActive(isActive: true);
			_uiParticle.Play();
			_Se = SoundUtils.PlaySE(SEFIleInfos.RewardGet2);
			_isPlayPhase1 = true;
			_isInput = true;
		}

		public void onScreenTap()
		{
			compStartAnimation();
		}

		private void compStartAnimation()
		{
			if (_isNeedBGM)
			{
				((Component)_uiParStar1).SetActive(isActive: true);
				_uiParStar1.Play();
				((Component)_uiParStar2).SetActive(isActive: true);
				_uiParStar2.Play();
			}
			_uiParticle.Stop();
			((Component)_uiParticle).SetActive(isActive: false);
			_uiShip.alpha = 1f;
			_uiBg.mainTexture = TextureFile.LoadRareBG(_clsRewardShip.Ship.Rare);
			((Component)_getIconAnim).gameObject.SetActive(true);
			_getIconAnim.Stop();
			_getIconAnim.Play();
			_isPlayPhase1 = false;
			_isInput = false;
			_anim.Stop();
			_anim.Play("comp_GetShip");
			TrophyUtil.Unlock_GetShip(_clsRewardShip.Ship.MstId);
		}

		public void showMessage()
		{
			_clsShipName.SetActive(isActive: true);
			_clsSType.SetActive(isActive: true);
			_clsShipName.gameObject.SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _clsShipName.gameObject, string.Empty);
			_clsSType.gameObject.SafeGetTweenAlpha(0f, 1f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, _clsSType.gameObject, string.Empty);
		}

		private void startMessage()
		{
			ShipUtils.PlayShipVoice(_clsRewardShip.Ship, 1);
			_clsShipMessage.Play();
			showMessage();
			_uiGear.GetComponent<Collider2D>().enabled = true;
			_isInput = true;
			_gearAnim.Stop();
			_gearAnim.Play();
		}

		private void FadeOutExtinguish()
		{
			if (_isNeedBGM)
			{
				SingletonMonoBehaviour<SoundManager>.Instance.StopBGM();
			}
			SoundUtils.StopSE(0.5f, _Se);
			_uiParStar1.Stop();
			((Component)_uiParStar1).SetActive(isActive: false);
			_uiParStar2.Stop();
			((Component)_uiParStar2).SetActive(isActive: false);
			this.SafeGetTweenAlpha(1f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, "_onFadeOutExtinguishFinished");
		}

		private void _onFadeOutExtinguishFinished()
		{
			if (_actCallback != null)
			{
				_actCallback();
			}
			Discard();
		}

		private void prodReceiveShipEL(GameObject obj)
		{
			if (_isInput)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				FadeOutExtinguish();
				_isInput = false;
			}
		}

		private void backgroundEL(GameObject obj)
		{
			if (_isPlayPhase1)
			{
				compStartAnimation();
			}
		}
	}
}
