using Common.Enum;
using Common.Struct;
using DG.Tweening;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PSVita;

namespace KCV.Scene.Practice.Deck
{
	[RequireComponent(typeof(Camera))]
	[RequireComponent(typeof(UIWidget))]
	public class UIDeckPracticeProductionMovieClip : MonoBehaviour
	{
		public static class Util
		{
			public static List<int> GenerateRandomIndexMap(DeckModel deckModel)
			{
				List<int> list = new List<int>();
				while (list.Count() < deckModel.GetShips().Length)
				{
					int item = UnityEngine.Random.Range(0, deckModel.GetShips().Length);
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
				return list;
			}
		}

		[SerializeField]
		private Texture mTexture2d_Overlay;

		[SerializeField]
		private UITexture mTexture_MovieClipRendrer;

		[SerializeField]
		private RenderTexture mRenderTexture_MovieClipRendrer;

		private UIWidget mWidgetThis;

		private bool mIsPlaying;

		private bool mIsCallPlay;

		protected DeckPracticeResultModel mDeckPracticeResultModel;

		protected DeckModel mDeckModel;

		private Action<ShipModel, ShipExpModel, PowUpInfo> mOnShipParameterUpEvent;

		private Action mOnFinishedProduction;

		private void Awake()
		{
			mTexture_MovieClipRendrer.mainTexture = mTexture2d_Overlay;
			mTexture_MovieClipRendrer.color = Color.clear;
			mWidgetThis = GetComponent<UIWidget>();
			mWidgetThis.alpha = 1E-06f;
		}

		private void OnPreRender()
		{
			if (mIsCallPlay)
			{
				PSVitaVideoPlayer.Update();
			}
		}

		private void OnMovieEvent(int eventID)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			var val = eventID;
			var val2 = val;
			if ((int)val2 == 3 && !mIsPlaying)
			{
				mIsPlaying = true;
				mTexture_MovieClipRendrer.mainTexture = mRenderTexture_MovieClipRendrer;
				mTexture_MovieClipRendrer.color = Color.white;
				mTexture_MovieClipRendrer.alpha = 1E-06f;
				DOVirtual.Float(0f, 1f, 0.3f, delegate(float alpha)
				{
					mTexture_MovieClipRendrer.alpha = alpha;
				}).SetDelay(0.3f).SetId(this);
			}
			Debug.Log("End OF OnMovieEvent:" + eventID);
		}

		public void Initialize(DeckModel deckModel, DeckPracticeResultModel deckPracticeResultModel)
		{
			mDeckModel = deckModel;
			mDeckPracticeResultModel = deckPracticeResultModel;
		}

		public void Play()
		{
			DOVirtual.Float(mWidgetThis.alpha, 1f, 0.5f, delegate(float alpha)
			{
				mWidgetThis.alpha = alpha;
			}).SetDelay(0.3f).SetId(this);
			PlaySlotParamUpProduction();
			string text = FindPracticeMovieClipPath(mDeckPracticeResultModel.PracticeType);
			PSVitaVideoPlayer.Init(mRenderTexture_MovieClipRendrer);
			PSVitaVideoPlayer.Play(text, PSVitaVideoPlayer.Looping.Continuous, PSVitaVideoPlayer.Mode.FullscreenVideo);
			mIsCallPlay = true;
		}

		public void Stop()
		{
			if (mIsPlaying)
			{
				mRenderTexture_MovieClipRendrer.Release();
				PSVitaVideoPlayer.Init((RenderTexture)null);
				mIsPlaying = false;
			}
		}

		private static string FindPracticeMovieClipPath(DeckPracticeType practiceType)
		{
			switch (practiceType)
			{
			case DeckPracticeType.Normal:
				return "StreamingAssets/Movies/Practice/Practice_Type_A.mp4";
			case DeckPracticeType.Hou:
				return "StreamingAssets/Movies/Practice/Practice_Type_B.mp4";
			case DeckPracticeType.Rai:
				return "StreamingAssets/Movies/Practice/Practice_Type_C.mp4";
			case DeckPracticeType.Taisen:
				return "StreamingAssets/Movies/Practice/Practice_Type_D.mp4";
			case DeckPracticeType.Kouku:
				return "StreamingAssets/Movies/Practice/Practice_Type_E.mp4";
			case DeckPracticeType.Sougou:
				return "StreamingAssets/Movies/Practice/Practice_Type_F.mp4";
			default:
				throw new Exception("Unknown DeckPracticeType Exception");
			}
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			mDeckPracticeResultModel = null;
			mDeckModel = null;
			mOnShipParameterUpEvent = null;
			mOnFinishedProduction = null;
			mTexture2d_Overlay = null;
			if (mRenderTexture_MovieClipRendrer != null)
			{
				mRenderTexture_MovieClipRendrer.Release();
			}
			mTexture_MovieClipRendrer.mainTexture = null;
			mTexture_MovieClipRendrer = null;
			mRenderTexture_MovieClipRendrer = null;
		}

		private void PlaySlotParamUpProduction()
		{
			List<int> randomIndexMap = Util.GenerateRandomIndexMap(mDeckModel);
			Sequence randomPowerUpSequence = DOTween.Sequence().SetId(this);
			ShipModel[] shipModels = mDeckPracticeResultModel.Ships;
			DOVirtual.DelayedCall(3.5f, delegate
			{
				Sequence s = DOTween.Sequence().SetId(this);
				int num = 0;
				foreach (int item in randomIndexMap)
				{
					Tween t = GeneratePowerUpNotifyTween(shipModels[item], num++).SetId(this);
					randomPowerUpSequence.Append(t);
				}
				s.Join(randomPowerUpSequence);
				s.Join(DOVirtual.DelayedCall(7f, delegate
				{
					FinishedProduction();
				}).SetId(this));
			}).SetId(this);
		}

		public void SetOnShipParameterUpEventListener(Action<ShipModel, ShipExpModel, PowUpInfo> onShipParameterUpEvent)
		{
			mOnShipParameterUpEvent = onShipParameterUpEvent;
		}

		public void SetOnFinishedProductionListener(Action onFinishedProduction)
		{
			mOnFinishedProduction = onFinishedProduction;
		}

		private void OnShipParameterUpEventListener(ShipModel shipModel, ShipExpModel shipExpModel, PowUpInfo powUpInfo)
		{
			if (mOnShipParameterUpEvent != null)
			{
				mOnShipParameterUpEvent(shipModel, shipExpModel, powUpInfo);
			}
		}

		private void FinishedProduction()
		{
			if (mOnFinishedProduction != null)
			{
				mOnFinishedProduction();
			}
		}

		private void FadeOut()
		{
			DOVirtual.Float(mWidgetThis.alpha, 0f, 0.3f, delegate(float alpha)
			{
				mWidgetThis.alpha = alpha;
			}).SetId(this);
		}

		private Tween GeneratePowerUpNotifyTween(ShipModel shipModel, float delay)
		{
			TweenCallback callback = delegate
			{
				OnPowerUpNotify(shipModel);
			};
			return DOVirtual.DelayedCall(delay, callback).SetId(this);
		}

		private void OnPowerUpNotify(ShipModel shipModel)
		{
			ShipExpModel shipExpInfo = mDeckPracticeResultModel.GetShipExpInfo(shipModel.MemId);
			PowUpInfo shipPowupInfo = mDeckPracticeResultModel.GetShipPowupInfo(shipModel.MemId);
			OnShipParameterUpEventListener(shipModel, shipExpInfo, shipPowupInfo);
		}
	}
}
