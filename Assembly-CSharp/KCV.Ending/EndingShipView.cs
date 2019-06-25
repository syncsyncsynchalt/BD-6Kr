using KCV.Strategy;
using KCV.Utils;
using live2d;
using local.models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Ending
{
	public class EndingShipView : MonoBehaviour
	{
		[SerializeField]
		private UILabel ShipNameLabel;

		[SerializeField]
		private UILabel DeckNameLabel;

		[SerializeField]
		private UILabel LevelLabel;

		[SerializeField]
		private UITexture BG;

		[SerializeField]
		private TweenPosition TweenPos;

		[SerializeField]
		private TweenAlpha LableTweenAlpha;

		private List<Live2DModelUnity> Live2DCache;

		[SerializeField]
		private StrategyShipCharacter Live2DRender;

		[SerializeField]
		private ParticleSystem SakuraPar;

		[SerializeField]
		private Transform Ring;

		public float ChangeCount;

		public float EndingTime;

		public float ChangeTime;

		private Coroutine cor;

		public bool isShipChanging;

		private AudioSource ShipVoice;

		private bool isFirstCall;

		public bool isVoicePlaying
		{
			get
			{
				if ((Object)ShipVoice != null)
				{
					return ShipVoice.isPlaying;
				}
				return false;
			}
		}

		public bool isLeft => TweenPos.to.x == -25f;

		private void Awake()
		{
			isFirstCall = true;
			Live2DCache = new List<Live2DModelUnity>();
			Ring.transform.localPositionY(-54f);
		}

		public IEnumerator ShipChangeCoroutine(ShipModel Ship, int index)
		{
			bool isCharaChanged = false;
			isShipChanging = true;
			FadeLabel(isFadeIn: false);
			if (!isFirstCall)
			{
				TweenPos.onFinished.Clear();
				TweenPos.PlayReverse();
				this.DelayAction(TweenPos.duration, delegate
				{
					this.Live2DRender.ChangeCharacter(Ship, -1, isDamaged: false);
                    isCharaChanged = true;
				});
				while (!isCharaChanged)
				{
					yield return null;
				}
			}
			else
			{
				Live2DRender.ChangeCharacter(Ship, -1, isDamaged: false);
				isFirstCall = false;
			}
			float size = 1.2f;
			int posy = -48;
			if (Ship.Lov > 200)
			{
				size = 1.25f;
				posy = -60;
			}
			if (Ship.Lov > 500)
			{
				size = 1.3f;
				posy = -72;
			}
			if (Ship.Lov > 700)
			{
				size = 1.35f;
				posy = -84;
			}
			Live2DRender.transform.localScale = new Vector3(size, size, size);
			Live2DRender.transform.AddLocalPositionY(posy);
			yield return new WaitForEndOfFrame();
			if (Ship.IsMarriage())
			{
				SakuraPar.Play();
				Ring.SetActive(isActive: true);
			}
			else
			{
				SakuraPar.Stop();
				Ring.SetActive(isActive: false);
			}
			TweenPos.onFinished.Clear();
			TweenPos.PlayForward();
			this.DelayAction(TweenPos.duration, delegate
			{
				this.ShipVoice = ShipUtils.PlayEndingVoice(Ship, 28);
				this.SetLabel(Ship);
				this.FadeLabel(isFadeIn: true);
				this.isShipChanging = false;
			});
			yield return new WaitForEndOfFrame();
			SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Secret);
			SingletonMonoBehaviour<Live2DModel>.Instance.Play();
		}

		public IEnumerator CharacterExit()
		{
			if (isLeft)
			{
				TweenPos.to = new Vector3(-25f, 0f, 0f);
				TweenPos.from = new Vector3(1200f, 0f, 0f);
				TweenPos.duration = 0.8f;
				((Component)SakuraPar).transform.localPositionX(400f);
			}
			bool isCharaChanged = false;
			FadeLabel(isFadeIn: false);
			TweenPos.onFinished.Clear();
			TweenPos.PlayReverse();
			this.DelayAction(TweenPos.duration, delegate
			{
                isCharaChanged = true;
			});
			while (!isCharaChanged)
			{
				yield return null;
			}
		}

		public IEnumerator ChangeFinalShip(ShipModel ship, EndingStaffRoll StaffRoll)
		{
			ShipUtils.StopShipVoice();
			Live2DRender.ChangeCharacter(ship, -1, isDamaged: false);
			TweenPos.to = new Vector3(400f, 0f, 0f);
			TweenPos.from = new Vector3(1200f, 0f, 0f);
			while (!StaffRoll.isFinishRoll)
			{
				yield return null;
			}
			if (ship.IsMarriage())
			{
				SakuraPar.Play();
			}
			else
			{
				SakuraPar.Stop();
			}
			TweenPos.onFinished.Clear();
			TweenPos.PlayForward();
			TweenPos.duration = 2f;
			Live2DRender.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
			Live2DRender.transform.AddLocalPositionY(-120f);
			this.DelayAction(TweenPos.duration, delegate
			{
				this.isShipChanging = false;
			});
			yield return new WaitForEndOfFrame();
			SingletonMonoBehaviour<Live2DModel>.Instance.ChangeMotion(Live2DModel.MotionType.Secret);
			SingletonMonoBehaviour<Live2DModel>.Instance.Play();
		}

		private void SetLabel(ShipModel model)
		{
			ShipNameLabel.text = model.ShipTypeName + "\u3000";
			ShipNameLabel.text += model.Name;
			DeckModelBase deck = model.getDeck();
			string str = (deck == null || deck.GetFlagShip().MemId != model.MemId) ? "所属" : "旗艦";
			DeckNameLabel.supportEncoding = false;
			if (deck != null)
			{
				DeckNameLabel.text = deck.Name + str;
				BG.height = 148;
			}
			else
			{
				DeckNameLabel.text = string.Empty;
				BG.height = 95;
			}
			LevelLabel.text = "練度 " + model.Level;
		}

		private void FadeLabel(bool isFadeIn)
		{
			LableTweenAlpha.Play(isFadeIn);
		}

		public void OnSideChange()
		{
			if (TweenPos.to.x == 400f)
			{
				TweenPos.to = new Vector3(-25f, 0f, 0f);
				TweenPos.from = new Vector3(-850f, 0f, 0f);
				((Component)SakuraPar).transform.localPositionX(0f);
			}
			else
			{
				TweenPos.to = new Vector3(400f, 0f, 0f);
				TweenPos.from = new Vector3(1200f, 0f, 0f);
				((Component)SakuraPar).transform.localPositionX(400f);
			}
		}

		public void CreateLive2DCache(List<ShipModel> ShipModels)
		{
			for (int i = 0; i < ShipModels.Count; i++)
			{
				Live2DCache.Add(SingletonMonoBehaviour<Live2DModel>.Instance.CreateLive2DModelUnity(ShipModels[i].MstId));
			}
		}

		private void OnDestroy()
		{
			if (cor != null)
			{
				StopCoroutine(cor);
				cor = null;
			}
			if (Live2DCache != null)
			{
				for (int i = 0; i < Live2DCache.Count; i++)
				{
					Live2DCache[i].releaseModel();
				}
			}
			Live2DCache.Clear();
		}
	}
}
