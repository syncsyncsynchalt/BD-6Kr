using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	public class TaskTitleSelectMode : SceneTaskMono
	{
		private UIPressAnyKey _uiPressAnyKey;

		private CtrlTitleSelectMode _ctrlTitleSelectMode;

		private StatementMachine _clsState;

		private IDisposable _disLeaveSubscription;

		protected override bool Init()
		{
			App.InitLoadMasterDataManager();
			App.InitSystems();
			UIPanel maskPanel = TitleTaskManager.GetMaskPanel();
			maskPanel.transform.LTCancel();
			maskPanel.transform.LTValue(maskPanel.alpha, 0f, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				maskPanel.alpha = x;
			});
			SoundUtils.PlaySceneBGM(BGMFileInfos.Strategy);
			_clsState = new StatementMachine();
			UITitleLogo logo = TitleTaskManager.GetUITitleLogo();
			if (logo.panel.alpha == 0f)
			{
				logo.Show().setOnComplete((Action)delegate
				{
					Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
					{
						logo.StartLogoAnim();
						_clsState.AddState(InitPressAnyKey, UpdatePressAnyKey);
						SetupLeaveTimer();
					});
				});
			}
			else
			{
				_clsState.AddState(InitPressAnyKey, UpdatePressAnyKey);
				SetupLeaveTimer();
			}
			Observable.FromCoroutine(NoticeMasterInitComplete).Subscribe().AddTo(base.gameObject);
			return true;
		}

		protected override bool UnInit()
		{
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			Mem.Del(ref _clsState);
			_disLeaveSubscription.Dispose();
			if (_uiPressAnyKey != null && _uiPressAnyKey.gameObject != null)
			{
				UnityEngine.Object.Destroy(_uiPressAnyKey.gameObject);
			}
			Mem.Del(ref _uiPressAnyKey);
			if (_ctrlTitleSelectMode != null && _ctrlTitleSelectMode.gameObject != null)
			{
				UnityEngine.Object.Destroy(_ctrlTitleSelectMode.gameObject);
			}
			Mem.Del(ref _ctrlTitleSelectMode);
			return true;
		}

		protected override bool Run()
		{
			KeyControl keyControl = TitleTaskManager.GetKeyControl();
			if (keyControl.IsAnyKey)
			{
				SetupLeaveTimer();
			}
			_clsState.OnUpdate(Time.deltaTime);
			if (TitleTaskManager.GetMode() != TitleTaskManagerMode.TitleTaskManagerMode_BEF)
			{
				return (TitleTaskManager.GetMode() == TitleTaskManagerMode.SelectMode) ? true : false;
			}
			return true;
		}

		private bool InitPressAnyKey(object data)
		{
			_uiPressAnyKey = UIPressAnyKey.Instantiate(((Component)TitleTaskManager.GetPrefabFile().prefabUIPressAnyKey).GetComponent<UIPressAnyKey>(), TitleTaskManager.GetSharedPlace(), OnPressAnyKeyFinished);
			return false;
		}

		private bool UpdatePressAnyKey(object data)
		{
			if (_uiPressAnyKey != null)
			{
				if (_uiPressAnyKey.Run())
				{
					SetupLeaveTimer();
					return true;
				}
				return false;
			}
			return false;
		}

		private void OnPressAnyKeyFinished()
		{
			if (_uiPressAnyKey != null)
			{
				UnityEngine.Object.Destroy(_uiPressAnyKey.gameObject);
			}
			Mem.Del(ref _uiPressAnyKey);
			_clsState.AddState(InitSelectMode, UpdateSelectMode);
		}

		private bool InitSelectMode(object data)
		{
			_ctrlTitleSelectMode = CtrlTitleSelectMode.Instantiate(((Component)TitleTaskManager.GetPrefabFile().prefabCtrlTitleSelectMode).GetComponent<CtrlTitleSelectMode>(), TitleTaskManager.GetSharedPlace(), SetupLeaveTimer);
			_ctrlTitleSelectMode.Play(OnDecideMode);
			return false;
		}

		private bool UpdateSelectMode(object data)
		{
			if (_ctrlTitleSelectMode != null)
			{
				_ctrlTitleSelectMode.Run();
				return false;
			}
			_disLeaveSubscription.Dispose();
			return true;
		}

		private void SetupLeaveTimer()
		{
			if (_disLeaveSubscription != null)
			{
				_disLeaveSubscription.Dispose();
			}
			_disLeaveSubscription = Observable.Timer(TimeSpan.FromSeconds(30.0)).Subscribe(delegate
			{
				_clsState.Clear();
				UITitleLogo uITitleLogo = TitleTaskManager.GetUITitleLogo();
				uITitleLogo.Hide();
				SoundUtils.StopFadeBGM(0.3f, delegate
				{
					TitleTaskManager.ReqMode(TitleTaskManagerMode.TitleTaskManagerMode_ST);
				});
			}).AddTo(base.gameObject);
		}

		private IEnumerator NoticeMasterInitComplete()
		{
			if (!App.isMasterInit)
			{
				while (!App.isMasterInit)
				{
					yield return new WaitForEndOfFrame();
				}
				SoundUtils.PlaySE(SEFIleInfos.RewardGet);
				yield return null;
			}
			yield return null;
		}

		private void OnDecideMode(SelectMode iMode)
		{
			UnityEngine.Object.Destroy(_ctrlTitleSelectMode.gameObject);
			Mem.Del(ref _ctrlTitleSelectMode);
			switch (iMode)
			{
			case SelectMode.Appointed:
				TitleTaskManager.ReqMode(TitleTaskManagerMode.NewGame);
				break;
			case SelectMode.Inheriting:
				Observable.FromCoroutine((UniRx.IObserver<AsyncOperation> observer) => TitleTaskManager.GotoLoadScene(observer)).Subscribe(delegate(AsyncOperation x)
				{
					x.allowSceneActivation = true;
				}).AddTo(base.gameObject);
				break;
			}
		}
	}
}
