using KCV.Utils;
using local.managers;
using ModeProc;
using System.Collections;
using UnityEngine;

namespace KCV.Inherit
{
	public class TaskInheritSaveSelect : SceneTaskMono
	{
		public enum Mode
		{
			AdmiralRankJudge,
			DoSaveSelect,
			InheritTypeSelect,
			Confirm
		}

		private ModeProcessor ModeProc;

		[SerializeField]
		private UIWidget SaveSelect;

		[SerializeField]
		private UIWidget TypeSelect;

		[SerializeField]
		private UIWidget ConfirmSelect;

		[SerializeField]
		private UIButtonManager SaveSelectBtnMng;

		[SerializeField]
		private UIButtonManager ConfirmSelectBtnMng;

		[SerializeField]
		private UIButtonManager TypeSelectBtnMng;

		private KeyControl key;

		[SerializeField]
		private UITexture ShipTexture;

		[SerializeField]
		private UILabel ShipNum;

		private EndingManager manager;

		public bool isSaved;

		[SerializeField]
		private AdmiralRankJudge rankJudge;

		private bool JudgeFinished;

		protected override void Start()
		{
			SingletonMonoBehaviour<SoundManager>.Instance.StopBGM();
			manager = new EndingManager();
			SaveSelect.alpha = 0f;
			ConfirmSelect.alpha = 0f;
			ModeProc = GetComponent<ModeProcessor>();
			ModeProc.addMode(Mode.AdmiralRankJudge.ToString(), AdmiralRankJudgeRun, AdmiralRankJudgeEnter, AdmiralRankJudgeExit);
			ModeProc.addMode(Mode.DoSaveSelect.ToString(), DoSaveSelectRun, DoSaveSelectEnter, DoSaveSelectExit);
			ModeProc.addMode(Mode.InheritTypeSelect.ToString(), InheritTypeSelectRun, InheritTypeSelectEnter, InheritTypeSelectExit);
			ModeProc.addMode(Mode.Confirm.ToString(), ConfirmRun, ConfirmEnter, ConfirmExit);
			ShipNum.textInt = manager.GetTakeoverShipCountMax();
		}

		protected override bool Init()
		{
			Debug.Log("SaveSelect Init");
			key = new KeyControl();
			SaveSelectBtnMng.setFocus(0);
			ConfirmSelectBtnMng.setFocus(0);
			SaveSelect.alpha = 0f;
			ConfirmSelect.alpha = 0f;
			ModeProc.FirstModeEnter();
			return true;
		}

		protected override bool Run()
		{
			key.Update();
			ModeProc.ModeRun();
			return true;
		}

		private void OnDestroy()
		{
			ModeProc = null;
			SaveSelect = null;
			TypeSelect = null;
			ConfirmSelect = null;
			SaveSelectBtnMng = null;
			ConfirmSelectBtnMng = null;
			TypeSelectBtnMng = null;
			key = null;
			ShipTexture = null;
			ShipNum = null;
			manager = null;
		}

		private void AdmiralRankJudgeRun()
		{
			if (key.IsMaruDown() && JudgeFinished)
			{
				if (rankJudge != null)
				{
					rankJudge.StopParticle();
				}
				ModeProc.ChangeMode(1);
			}
		}

		private IEnumerator AdmiralRankJudgeEnter()
		{
			rankJudge.Initialize(manager);
			JudgeFinished = false;
			yield return new WaitForEndOfFrame();
			rankJudge.Play(delegate
			{
				this.JudgeFinished = true;
			});
		}

		private IEnumerator AdmiralRankJudgeExit()
		{
			yield return null;
			bool finished = false;
			TweenAlpha.Begin(rankJudge.gameObject, 1f, 0f).SetOnFinished(delegate
			{
				finished = true;
			});
			while (!finished)
			{
				yield return new WaitForEndOfFrame();
			}
		}

		private void DoSaveSelectRun()
		{
			if (key.IsRightDown())
			{
				SaveSelectBtnMng.moveNextButton();
			}
			else if (key.IsLeftDown())
			{
				SaveSelectBtnMng.movePrevButton();
			}
			else if (key.IsMaruDown())
			{
				SaveSelectBtnMng.Decide();
			}
		}

		private IEnumerator DoSaveSelectEnter()
		{
			bool finished = false;
			ShipUtils.PlayPortVoice(501);
			TweenAlpha.Begin(SaveSelect.gameObject, 0.5f, 1f).SetOnFinished(delegate
			{
				finished = true;
			});
			while (!finished)
			{
				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator DoSaveSelectExit()
		{
			bool finished = false;
			ShipUtils.StopShipVoice();
			TweenAlpha.Begin(SaveSelect.gameObject, 0.5f, 0f).SetOnFinished(delegate
			{
				finished = true;
			});
			while (!finished)
			{
				yield return new WaitForEndOfFrame();
			}
		}

		public void OnYesDesideSaveSelect()
		{
			ModeProc.ChangeMode(2);
		}

		public void OnNoDesideSaveSelect()
		{
			if (isSaved)
			{
				StartCoroutine(GotoTitle());
			}
			else
			{
				ModeProc.ChangeMode(3);
			}
		}

		private void InheritTypeSelectRun()
		{
			if (key.IsUpDown())
			{
				TypeSelectBtnMng.movePrevButton();
			}
			else if (key.IsDownDown())
			{
				TypeSelectBtnMng.moveNextButton();
			}
			else if (key.IsMaruDown())
			{
				TypeSelectBtnMng.Decide();
			}
			else if (key.IsBatuDown())
			{
				ModeProc.ChangeMode(1);
			}
		}

		private IEnumerator InheritTypeSelectEnter()
		{
			if (isSaved)
			{
				StartCoroutine(GotoTitle());
				yield break;
			}
			ShipUtils.PlayPortVoice(502);
			bool finished = false;
			TypeSelectBtnMng.setFocus(0);
			TweenAlpha.Begin(TypeSelect.gameObject, 0.5f, 1f).SetOnFinished(delegate
			{
				finished = true;
			});
			while (!finished)
			{
				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator InheritTypeSelectExit()
		{
			bool finished = false;
			ShipUtils.StopShipVoice();
			TweenAlpha.Begin(TypeSelect.gameObject, 0.5f, 0f).SetOnFinished(delegate
			{
				finished = true;
			});
			while (!finished)
			{
				yield return new WaitForEndOfFrame();
			}
		}

		public void OnDesideYuusenButton()
		{
			manager.CreatePlusData(is_level_sort: false);
			StartCoroutine(ReqDoSaveMode());
		}

		public void OnDesideNoYusenButton()
		{
			manager.CreatePlusData(is_level_sort: true);
			StartCoroutine(ReqDoSaveMode());
		}

		private IEnumerator ReqDoSaveMode()
		{
			yield return StartCoroutine(InheritTypeSelectExit());
			InheritSaveTaskManager.ReqMode(InheritSaveTaskManager.InheritTaskManagerMode.DoSave);
			Close();
		}

		private void ConfirmRun()
		{
			if (key.IsRightDown())
			{
				ConfirmSelectBtnMng.moveNextButton();
			}
			else if (key.IsLeftDown())
			{
				ConfirmSelectBtnMng.movePrevButton();
			}
			else if (key.IsMaruDown())
			{
				ConfirmSelectBtnMng.Decide();
			}
		}

		private IEnumerator ConfirmEnter()
		{
			bool finished = false;
			ConfirmSelectBtnMng.setFocus(1);
			TweenAlpha.Begin(ConfirmSelect.gameObject, 0.5f, 1f).SetOnFinished(delegate
			{
				finished = true;
			});
			while (!finished)
			{
				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator ConfirmExit()
		{
			bool finished = false;
			TweenAlpha.Begin(ConfirmSelect.gameObject, 0.5f, 0f).SetOnFinished(delegate
			{
				finished = true;
			});
			while (!finished)
			{
				yield return new WaitForEndOfFrame();
			}
		}

		public void OnYesDesideConfirm()
		{
			StartCoroutine(GotoTitle());
			Close();
		}

		public void OnNoDesideConfirm()
		{
			ModeProc.ChangeMode(1);
		}

		private IEnumerator GotoTitle()
		{
			key.IsRun = false;
			yield return StartCoroutine(ConfirmExit());
			yield return StartCoroutine(DoSaveSelectExit());
			GameObject SingletonObj = GameObject.Find("SingletonObject");
			if (SingletonObj != null)
			{
				Object.Destroy(SingletonObj);
			}
			if (SingletonMonoBehaviour<PortObjectManager>.exist())
			{
				Object.Destroy(SingletonMonoBehaviour<PortObjectManager>.Instance.gameObject);
			}
			yield return new WaitForEndOfFrame();
			Application.LoadLevel(Generics.Scene.Title.ToString());
		}

		protected override bool UnInit()
		{
			return true;
		}
	}
}
