using KCV.Utils;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	public class TaskTitleOpening : SceneTaskMono
	{
		private IDisposable _disMovieSubscription;

		private bool _isInputPossible;

		protected override bool Init()
		{
			PSVitaMovie movie = TitleTaskManager.GetPSVitaMovie();
			UIPanel maskPanel = TitleTaskManager.GetMaskPanel();
			_isInputPossible = false;
			SoundUtils.StopBGM();
			if (!movie.isPlaying)
			{
				movie.SetLooping(0).SetMode(0).SetOnWarningID(delegate
				{
					movie.ImmediateOnFinished();
				})
					.SetOnPlay(delegate
					{
						maskPanel.transform.LTCancel();
						maskPanel.transform.LTValue(maskPanel.alpha, 1f, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
						{
							maskPanel.alpha = x;
						});
					})
					.SetOnBuffering(delegate
					{
						Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
						{
							_isInputPossible = true;
						});
						((Component)maskPanel.transform.GetChild(0)).GetComponent<UITexture>().color = Color.white;
					})
					.SetOnFinished(OnMovieFinished)
					.Play(MovieFileInfos.MOVIE_FILE_INFOS_ID_ST.GetFilePath());
			}
			if (SingletonMonoBehaviour<FadeCamera>.Instance != null && SingletonMonoBehaviour<FadeCamera>.Instance.isFadeOut)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, null);
			}
			return true;
		}

		protected override bool UnInit()
		{
			Mem.Del(ref _disMovieSubscription);
			return true;
		}

		protected override bool Run()
		{
			KeyControl keyControl = TitleTaskManager.GetKeyControl();
			PSVitaMovie pSVitaMovie = TitleTaskManager.GetPSVitaMovie();
			if (_isInputPossible && pSVitaMovie.isPlaying && (keyControl.GetDown(KeyControl.KeyName.MARU) || keyControl.GetDown(KeyControl.KeyName.START) || Input.touchCount != 0))
			{
				_isInputPossible = false;
				UIPanel maskPanel = TitleTaskManager.GetMaskPanel();
				((Component)maskPanel.transform.GetChild(0)).GetComponent<UITexture>().color = Color.black;
				pSVitaMovie.ImmediateOnFinished();
			}
			if (TitleTaskManager.GetMode() != TitleTaskManagerMode.TitleTaskManagerMode_BEF)
			{
				return (TitleTaskManager.GetMode() == TitleTaskManagerMode.TitleTaskManagerMode_ST) ? true : false;
			}
			return true;
		}

		private void OnMovieFinished()
		{
			TitleTaskManager.ReqMode(TitleTaskManagerMode.SelectMode);
		}

		public void PlayImmediateOpeningMovie()
		{
			PSVitaMovie movie = TitleTaskManager.GetPSVitaMovie();
			UIPanel maskPanel = TitleTaskManager.GetMaskPanel();
			movie = TitleTaskManager.GetPSVitaMovie();
			movie.SetLooping(0).SetMode(0).SetOnPlay(delegate
			{
				maskPanel.transform.LTCancel();
				maskPanel.transform.LTValue(maskPanel.alpha, 1f, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					maskPanel.alpha = x;
				});
			})
				.SetOnBuffering(delegate
				{
					Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
					{
						_isInputPossible = true;
					});
					((Component)maskPanel.transform.GetChild(0)).GetComponent<UITexture>().color = Color.white;
				})
				.SetOnWarningID(delegate
				{
					movie.ImmediateOnFinished();
				})
				.SetOnFinished(OnMovieFinished)
				.Play(MovieFileInfos.MOVIE_FILE_INFOS_ID_ST.GetFilePath());
		}
	}
}
