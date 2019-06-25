using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModeProc
{
	public class ModeProcessor : MonoBehaviour
	{
		private List<Mode> Modes;

		private Mode nowMode;

		private Mode prevMode;

		private Mode firstMode;

		private Coroutine ChangeModeInstance;

		public bool isRun
		{
			get;
			private set;
		}

		private void Awake()
		{
			Modes = new List<Mode>();
			isRun = false;
		}

		public void ModeRun()
		{
			if (isRun)
			{
				nowMode.Run();
			}
		}

		public void addMode(string ModeName, Mode.ModeRun Run, Mode.ModeChange Enter, Mode.ModeChange Exit)
		{
			Mode mode = new Mode(Run, Enter, Exit);
			mode.ModeNo = Modes.Count;
			mode.Name = ModeName;
			Modes.Add(mode);
			if (nowMode == null)
			{
				nowMode = Modes[0];
				firstMode = Modes[0];
				StartCoroutine(Initialize());
			}
		}

		public void ChangeMode(int modeNo)
		{
			if (ChangeModeInstance != null)
			{
				StopCoroutine(ChangeModeInstance);
			}
			ChangeModeInstance = StartCoroutine(ChangeModeCor(modeNo));
		}

		public void FirstModeEnter()
		{
			StartCoroutine(FirstModeEnterCor(firstMode.ModeNo));
		}

		private IEnumerator Initialize()
		{
			yield return nowMode.Enter();
			isRun = true;
		}

		private IEnumerator ChangeModeCor(int modeNo)
		{
			isRun = false;
			prevMode = nowMode;
			nowMode = Modes[modeNo];
			if (prevMode.Exit != null)
			{
				yield return StartCoroutine(prevMode.Exit());
			}
			if (nowMode.Enter != null)
			{
				yield return StartCoroutine(nowMode.Enter());
			}
			Debug.Log("ModeChange:" + prevMode.Name + " → " + nowMode.Name);
			isRun = true;
			ChangeModeInstance = null;
		}

		private IEnumerator FirstModeEnterCor(int modeNo)
		{
			isRun = false;
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			if (nowMode.Enter != null)
			{
				yield return StartCoroutine(nowMode.Enter());
			}
			if (ChangeModeInstance == null)
			{
				isRun = true;
			}
		}

		public bool isNowMode(int No)
		{
			return nowMode.ModeNo == No;
		}

		public bool isPrevMode(int No)
		{
			return prevMode.ModeNo == No;
		}

		private void OnDestroy()
		{
			Modes.Clear();
			Modes = null;
			nowMode = null;
			prevMode = null;
			firstMode = null;
			ChangeModeInstance = null;
		}
	}
}
