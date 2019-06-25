using Common.Enum;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	public class TaskTitleNewGame : SceneTaskMono
	{
		private CtrlDifficultySelect _ctrlDifficultySelect;

		protected override bool Init()
		{
			HashSet<DifficultKind> hashSet = new HashSet<DifficultKind>();
			hashSet.Add(DifficultKind.HEI);
			hashSet.Add(DifficultKind.KOU);
			hashSet.Add(DifficultKind.OTU);
			_ctrlDifficultySelect = CtrlDifficultySelect.Instantiate(((Component)TitleTaskManager.GetPrefabFile().prefabCtrlDifficultySelect).GetComponent<CtrlDifficultySelect>(), TitleTaskManager.GetSharedPlace(), TitleTaskManager.GetKeyControl(), App.GetTitleManager().GetSelectableDifficulty(), OnDecideDifficulty, OnCancel);
			return true;
		}

		protected override bool UnInit()
		{
			if (_ctrlDifficultySelect != null)
			{
				Object.Destroy(_ctrlDifficultySelect.gameObject);
			}
			Mem.Del(ref _ctrlDifficultySelect);
			return true;
		}

		protected override bool Run()
		{
			_ctrlDifficultySelect.Run();
			if (TitleTaskManager.GetMode() != TitleTaskManagerMode.TitleTaskManagerMode_BEF)
			{
				return (TitleTaskManager.GetMode() == TitleTaskManagerMode.NewGame) ? true : false;
			}
			return true;
		}

		private void OnCancel()
		{
			TitleTaskManager.ReqMode(TitleTaskManagerMode.SelectMode);
		}

		private void OnDecideDifficulty(DifficultKind iKind)
		{
			Observable.FromCoroutine(() => LoadTutorial(iKind)).Subscribe().AddTo(base.gameObject);
		}

		private IEnumerator LoadTutorial(DifficultKind difficultKind)
		{
			RetentionData.SetData(new Hashtable
			{
				{
					"difficulty",
					difficultKind
				}
			});
			AsyncOperation async = Application.LoadLevelAsync(Generics.Scene.Startup.ToString());
			async.allowSceneActivation = false;
			while (!App.isMasterInit)
			{
				yield return new WaitForEndOfFrame();
			}
			async.allowSceneActivation = true;
		}
	}
}
