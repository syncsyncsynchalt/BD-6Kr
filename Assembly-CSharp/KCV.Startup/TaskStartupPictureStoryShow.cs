using System.Collections;
using UnityEngine;

namespace KCV.Startup
{
	public class TaskStartupPictureStoryShow : SceneTaskMono
	{
		private StatementMachine _clsState;

		private UITutorialConfirmDialog _uiTutorialConfirmDialog;

		private CtrlPictureStoryShow _ctrlPictureStoryShow;

		protected override bool Init()
		{
			_clsState = new StatementMachine();
			_clsState.AddState(InitPictureStoryShowConfirm, UpdatePictureStoryShowConfirm);
			return true;
		}

		protected override bool UnInit()
		{
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			Mem.Del(ref _clsState);
			Mem.DelComponentSafe(ref _uiTutorialConfirmDialog);
			return true;
		}

		protected override bool Run()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			if (StartupTaskManager.GetMode() != StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_BEF)
			{
				return (StartupTaskManager.GetMode() == StartupTaskManager.StartupTaskManagerMode.PictureStoryShow) ? true : false;
			}
			return true;
		}

		private bool InitPictureStoryShowConfirm(object data)
		{
			UIStartupHeader startupHeader = StartupTaskManager.GetStartupHeader();
			UIStartupNavigation navigation = StartupTaskManager.GetNavigation();
			startupHeader.SetMessage("チュ\u30fcトリアル");
			navigation.Hide(null);
			_uiTutorialConfirmDialog = UITutorialConfirmDialog.Instantiate(((Component)StartupTaskManager.GetPrefabFile().prefabUITutorialConfirmDialog).GetComponent<UITutorialConfirmDialog>(), StartupTaskManager.GetSharedPlace());
			_uiTutorialConfirmDialog.Init(OnPictureStoryShowConfirmCancel, OnPictureStoryShowConfirmDecide);
			_uiTutorialConfirmDialog.Open(null);
			return false;
		}

		private bool UpdatePictureStoryShowConfirm(object data)
		{
			KeyControl keyControl = StartupTaskManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				_uiTutorialConfirmDialog.OnDecide();
				return true;
			}
			if (keyControl.GetDown(KeyControl.KeyName.BATU))
			{
				_uiTutorialConfirmDialog.OnCancel();
				return true;
			}
			return false;
		}

		private void OnPictureStoryShowConfirmCancel()
		{
			_clsState.Clear();
			Hashtable hashtable = new Hashtable();
			hashtable.Add("TutorialCancel", true);
			RetentionData.SetData(hashtable);
			OnPictureStoryShowFinished();
		}

		private void OnPictureStoryShowConfirmDecide(int nDecideIndex)
		{
			_clsState.Clear();
			if (nDecideIndex == 0)
			{
				Mem.DelComponentSafe(ref _uiTutorialConfirmDialog);
				_clsState.AddState(InitPictureStoryShowTutorial, UpdatePictureStoryShowTutorial);
			}
			else
			{
				OnPictureStoryShowConfirmCancel();
			}
		}

		private bool InitPictureStoryShowTutorial(object data)
		{
			_ctrlPictureStoryShow = CtrlPictureStoryShow.Instantiate(((Component)StartupTaskManager.GetPrefabFile().prefabCtrlPictureStoryShow).GetComponent<CtrlPictureStoryShow>(), StartupTaskManager.GetSharedPlace(), OnPictureStoryShowFinished);
			return false;
		}

		private bool UpdatePictureStoryShowTutorial(object data)
		{
			return true;
		}

		private void OnPictureStoryShowFinished()
		{
			_clsState.Clear();
			_clsState.AddState(InitSecretaryShipMovie, UpdateSecretaryShipMovie);
		}

		private bool InitSecretaryShipMovie(object data)
		{
			ProdSecretaryShipMovie prodSecretaryShipMovie = ProdSecretaryShipMovie.Instantiate(((Component)StartupTaskManager.GetPrefabFile().prefabProdSecretaryShipMovie).GetComponent<ProdSecretaryShipMovie>(), StartupTaskManager.GetSharedPlace(), StartupTaskManager.GetData().PartnerShipID);
			prodSecretaryShipMovie.Play(OnStartupAllFinished);
			return false;
		}

		private bool UpdateSecretaryShipMovie(object data)
		{
			return true;
		}

		private void OnStartupAllFinished()
		{
			SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(1f, delegate
			{
				StartupTaskManager.GetStartupHeader().transform.localScaleZero();
				StartupData data = StartupTaskManager.GetData();
				App.CreateSaveDataNInitialize(data.AdmiralName, data.PartnerShipID, data.Difficlty, data.isInherit);
				SingletonMonoBehaviour<SoundManager>.Instance.PlayVoice(Resources.Load("Sounds/Voice/kc9999/" + $"{XorRandom.GetILim(206, 211):D2}") as AudioClip, 0);
				GameObject.Find("BG Panel").transform.localScale = Vector3.zero;
				GameObject.Find("StartupTaskManager").transform.localScale = Vector3.zero;
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadType = AppInformation.LoadType.Ship;
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
				Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
			});
		}
	}
}
