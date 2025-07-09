using Common.Enum;
using Common.SaveManager;
using KCV.Loading;
using KCV.Title;
using KCV.Utils;
using local.managers;
using ModeProc;
using System.Collections;
using UnityEngine;

namespace KCV.Inherit
{
	public class TaskInheritLoadSelect : SceneTaskMono
	{
		public enum Mode
		{
			DoLoadSelect,
			DifficultySelect
		}

		private VitaSaveManager saveManager;

		private ModeProcessor ModeProc;

		[SerializeField]
		private UILabel Message;

		private KeyControl key;

		[SerializeField]
		private GameObject DifficultySelectPrefab;

		private CtrlDifficultySelect _ctrlDifficultySelect;

		private bool isInherit;

		private UILoadingShip _LoadingShip;

		private DifficultKind diffculty;

		protected override void Start()
		{
			if (App.GetTitleManager() == null)
			{
				App.SetTitleManager(new TitleManager());
			}
			Message.alpha = 0f;
			isInherit = true;
			ModeProc = GetComponent<ModeProcessor>();
			ModeProc.addMode(Mode.DoLoadSelect.ToString(), DoLoadSelectRun, DoLoadSelectEnter, DoLoadSelectExit);
			ModeProc.addMode(Mode.DifficultySelect.ToString(), DifficultySelectRun, DifficultySelectEnter, DifficultySelectExit);
		}

		protected override bool Init()
		{
			key = new KeyControl();
			ModeProc.FirstModeEnter();
			Message.alpha = 0f;
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
			key.Update();
			ModeProc.ModeRun();
			return true;
		}

		private void DoLoadSelectRun()
		{
		}

		private IEnumerator DoLoadSelectEnter()
		{
			yield return Util.WaitEndOfFrames(4);
			isInherit = true;
			ModeProc.ChangeMode(1);
			yield return null;
		}

		private IEnumerator DoLoadSelectExit()
		{
			bool finished2 = false;
			Message.text = ((!isInherit) ? "引継ぎをしないでゲームを開始します" : "艦娘と装備を一部引き継いで、ゲームを開始します。");
			TweenAlpha.Begin(Message.gameObject, 0.5f, 1f);
			ShipUtils.PlayPortVoice(503, delegate
			{
				finished2 = true;
			});
			while (!finished2)
			{
				yield return new WaitForEndOfFrame();
			}
			finished2 = false;
			TweenAlpha.Begin(Message.gameObject, 0.5f, 0f).SetOnFinished(delegate
			{
				finished2 = true;
			});
			while (!finished2)
			{
				yield return new WaitForEndOfFrame();
			}
		}

		public void OnYesDesideLoadSelect()
		{
			ModeProc.ChangeMode(1);
		}

		public void OnNoDesideLoadSelect()
		{
			ModeProc.ChangeMode(1);
		}

		private void DifficultySelectRun()
		{
			_ctrlDifficultySelect.Run();
		}

		private IEnumerator DifficultySelectEnter()
		{
			_ctrlDifficultySelect = CtrlDifficultySelect.Instantiate(DifficultySelectPrefab.GetComponent<CtrlDifficultySelect>(), base.transform, key, App.GetTitleManager().GetSelectableDifficulty(), OnDecideDifficulty, OnCancel);
			yield return null;
		}

		private IEnumerator DifficultySelectExit()
		{
			TweenAlpha.Begin(_ctrlDifficultySelect.gameObject, 0.8f, 0f);
			yield return new WaitForSeconds(0.8f);
			yield return null;
		}

		public void OnDecideDifficulty(DifficultKind iKind)
		{
			diffculty = iKind;
			StartCoroutine(GotoNextScene());
		}

		public void OnCancel()
		{
			ModeProc.ChangeMode(0);
			Object.Destroy(_ctrlDifficultySelect.gameObject);
		}

		private IEnumerator GotoNextScene()
		{
			_LoadingShip = GameObject.Find("UILoadingShip").GetComponent<UILoadingShip>();
			key.IsRun = false;
			yield return StartCoroutine(DifficultySelectExit());
			yield return new WaitForEndOfFrame();
			RetentionData.SetData(new Hashtable
			{
				{
					"isInherit",
					true
				},
				{
					"difficulty",
					diffculty
				}
			});
			Application.LoadLevel(Generics.Scene.Startup.ToString());
			yield return null;
		}

		private void OnDestroy()
		{
			_ctrlDifficultySelect = null;
		}
	}
}
