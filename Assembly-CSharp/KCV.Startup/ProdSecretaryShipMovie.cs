using KCV.Utils;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdSecretaryShipMovie : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiOverlay;

		private UIPanel _uiPanel;

		private Action _actOnFinished;

		private int _nMstId;

		private UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static ProdSecretaryShipMovie Instantiate(ProdSecretaryShipMovie prefab, Transform parent, int nSecretaryShipMstId)
		{
			ProdSecretaryShipMovie prodSecretaryShipMovie = UnityEngine.Object.Instantiate(prefab);
			prodSecretaryShipMovie.transform.parent = parent;
			prodSecretaryShipMovie.transform.localScaleOne();
			prodSecretaryShipMovie.transform.localPositionZero();
			prodSecretaryShipMovie.VirtualCtor(nSecretaryShipMstId);
			return prodSecretaryShipMovie;
		}

		private bool VirtualCtor(int nSecretaryShipMstId)
		{
			_nMstId = nSecretaryShipMstId;
			panel.alpha = 0f;
			panel.widgetsAreStatic = true;
			return true;
		}

		public void Play(Action onFinished)
		{
			_actOnFinished = onFinished;
			ShowOverlay(delegate
			{
				PlayMovie();
			});
		}

		private void ShowOverlay(Action onFinished)
		{
			_uiOverlay.color = Color.black;
			panel.widgetsAreStatic = false;
			panel.transform.LTValue(panel.alpha, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					Dlg.Call(ref onFinished);
				});
		}

		private void HideOverlay(Action onFinished)
		{
			_uiOverlay.color = Color.white;
			panel.transform.LTValue(panel.alpha, 0f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					Dlg.Call(ref onFinished);
				});
		}

		private void PlayMovie()
		{
			PSVitaMovie movie = StartupTaskManager.GetPSVitaMovie();
			movie.SetLooping(0).SetMode(0).SetOnWarningID(OnFinishedMovie)
				.SetOnPlay(delegate
				{
					SoundUtils.StopFadeBGM(0.2f, null);
				})
				.SetOnBuffering(delegate
				{
					Observable.Timer(TimeSpan.FromMilliseconds(movie.movieDuration / 2)).Subscribe(delegate
					{
						_uiOverlay.color = Color.white;
					});
				})
				.SetOnFinished(OnFinishedMovie)
				.Play(MovieFileInfos.Startup.GetFilePath());
		}

		private void OnFinishedMovie()
		{
			FirstMeetingManager fmm = StartupTaskManager.GetFirstMeetingManager();
			Observable.FromCoroutine(() => fmm.Play(_nMstId, delegate
			{
				Dlg.Call(ref _actOnFinished);
			})).Subscribe();
		}
	}
}
