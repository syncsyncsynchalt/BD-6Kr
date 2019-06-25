using DG.Tweening;
using KCV;
using KCV.Remodel;
using KCV.Scene.Port;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UserInterfaceAlbumManager : MonoBehaviour
{
	public enum State
	{
		None,
		AlbumSelectGate,
		ShipAlbum,
		SlotItemAlbum,
		MoveToNextScene
	}

	private class StateManager<State>
	{
		private Stack<State> mStateStack;

		private State mEmptyState;

		public Action<State> OnPush
		{
			private get;
			set;
		}

		public Action<State> OnPop
		{
			private get;
			set;
		}

		public Action<State> OnResume
		{
			private get;
			set;
		}

		public Action<State> OnSwitch
		{
			private get;
			set;
		}

		public State CurrentState
		{
			get
			{
				if (0 < mStateStack.Count)
				{
					return mStateStack.Peek();
				}
				return mEmptyState;
			}
		}

		public StateManager(State emptyState)
		{
			mEmptyState = emptyState;
			mStateStack = new Stack<State>();
		}

		public void PushState(State state)
		{
			mStateStack.Push(state);
			Notify(OnPush, mStateStack.Peek());
			Notify(OnSwitch, mStateStack.Peek());
		}

		public void ReplaceState(State state)
		{
			if (0 < mStateStack.Count)
			{
				PopState();
			}
			mStateStack.Push(state);
			Notify(OnPush, mStateStack.Peek());
			Notify(OnSwitch, mStateStack.Peek());
		}

		public void PopState()
		{
			if (0 < mStateStack.Count)
			{
				State state = mStateStack.Pop();
				Notify(OnPop, state);
			}
		}

		public void ResumeState()
		{
			if (0 < mStateStack.Count)
			{
				Notify(OnResume, mStateStack.Peek());
				Notify(OnSwitch, mStateStack.Peek());
			}
		}

		public override string ToString()
		{
			mStateStack.ToArray();
			string text = string.Empty;
			foreach (State item in mStateStack)
			{
				text = item + " > " + text;
			}
			return text;
		}

		private void Notify(Action<State> target, State state)
		{
			target?.Invoke(state);
		}
	}

	public class Utils
	{
		public enum CharType
		{
			Sigle,
			Any
		}

		public static string NormalizeDescription(int maxLineInFullWidthChar, int fullWidthCharBuffer, string targetText)
		{
			int num = maxLineInFullWidthChar * 2;
			string text = "、。！？」』)";
			string text2 = targetText.Replace("\r\n", "\n");
			text2 = text2.Replace("\\n", "\n");
			text2 = text2.Replace("<br>", "\n");
			string[] array = text2.Split('\n');
			List<string> list = new List<string>();
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = 0;
				string text3 = array[i];
				StringBuilder stringBuilder = new StringBuilder();
				string text4 = text3;
				foreach (char c in text4)
				{
					int num3 = 0;
					switch (GetCharType(c))
					{
					case CharType.Sigle:
						num3 = 1;
						break;
					case CharType.Any:
						num3 = 2;
						break;
					}
					if (num2 + num3 <= num)
					{
						stringBuilder.Append(c);
						num2 += num3;
						continue;
					}
					string item = stringBuilder.ToString();
					list.Add(item);
					stringBuilder.Length = 0;
					stringBuilder.Append(c);
					num2 = num3;
				}
				if (0 < stringBuilder.Length)
				{
					list.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int k = 0; k < list.Count; k++)
			{
				if (k == 0)
				{
					stringBuilder2.Append(list[k]);
				}
				else if (-1 < text.IndexOf(list[k][0]))
				{
					string text5 = list[k];
					string value = text5.Substring(0, 1);
					stringBuilder2.Append(value);
					if (1 < text5.Length)
					{
						stringBuilder2.Append('\n');
						string value2 = text5.Substring(1);
						stringBuilder2.Append(value2);
					}
				}
				else
				{
					stringBuilder2.Append('\n');
					stringBuilder2.Append(list[k]);
				}
			}
			return stringBuilder2.ToString();
		}

		private static CharType GetCharType(char character)
		{
			int result = -1;
			if (int.TryParse(character.ToString(), out result))
			{
				return CharType.Any;
			}
			Encoding encoding = new UTF8Encoding();
			int byteCount = encoding.GetByteCount(character.ToString());
			if (byteCount == 1)
			{
				return CharType.Sigle;
			}
			return CharType.Any;
		}
	}

	[SerializeField]
	private UIHowToAlbum mUIHowToAlbum;

	[SerializeField]
	private Texture[] mTextures_Preload;

	[SerializeField]
	private UIAlbumSelectGate mUIAlbumSelectGate;

	[SerializeField]
	private UserInterfaceShipAlbumManager mUserInterfaceShipAlbumManager;

	[SerializeField]
	private UserInterfaceSlotItemAlbumManager mUserInterfaceSlotItemAlbumManager;

	private AlbumManager mAlbumManager;

	private StateManager<State> mStateManager;

	private KeyControl mKeyController;

	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		mAlbumManager = new AlbumManager();
		DOTween.Init();
		iTween.Init(base.gameObject);
		mKeyController = new KeyControl();
		mStateManager = new StateManager<State>(State.None);
		mUIAlbumSelectGate.SetOnSelectedShipAlbumListener(OnSelectedShipAlbumListener);
		mUIAlbumSelectGate.SetOnSelectedSlotItemAlbumListener(OnSelectedSlotItemAlbumListener);
		mUIAlbumSelectGate.SetOnSelectedBackListener(OnSelectedBackListener);
		mUIAlbumSelectGate.SetActive(isActive: false);
		mUserInterfaceShipAlbumManager.SetOnBackListener(OnBackShipAlbumListener);
		mUserInterfaceShipAlbumManager.SetOnChangeStateListener(OnChangeStateUserInterfaceShipAlbumManager);
		mUserInterfaceShipAlbumManager.SetActive(isActive: false);
		mUserInterfaceSlotItemAlbumManager.SetOnChangeStateListener(OnChangeStateUserInterfaceSlotItemAlbumManager);
		mUserInterfaceSlotItemAlbumManager.SetOnBackListener(OnBackSlotItemAlbumListener);
		mUserInterfaceSlotItemAlbumManager.SetActive(isActive: false);
		yield return new WaitForEndOfFrame();
		local.utils.Utils.GetSlotitemType3Name(0);
		yield return new WaitForEndOfFrame();
		mStateManager.OnPush = OnPushState;
		mStateManager.OnPop = OnPopState;
		mStateManager.OnResume = OnResumeState;
		if (SingletonMonoBehaviour<UIPortFrame>.exist())
		{
			SingletonMonoBehaviour<UIPortFrame>.Instance.gameObject.SetActive(false);
		}
		yield return new WaitForEndOfFrame();
		SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(null);
		mStateManager.PushState(State.AlbumSelectGate);
	}

	private void OnChangeStateUserInterfaceShipAlbumManager(UserInterfaceShipAlbumManager.State state)
	{
		switch (state)
		{
		case UserInterfaceShipAlbumManager.State.ShipDetail:
		case UserInterfaceShipAlbumManager.State.ShipDetailMarriaged:
			mUIHowToAlbum.ChangeGuideStatus(UIHowToAlbum.GuideState.Detail);
			break;
		case UserInterfaceShipAlbumManager.State.ShipList:
			mUIHowToAlbum.ChangeGuideStatus(UIHowToAlbum.GuideState.List);
			break;
		case UserInterfaceShipAlbumManager.State.MarriagedMovie:
			mUIHowToAlbum.ChangeGuideStatus(UIHowToAlbum.GuideState.Quiet);
			break;
		}
	}

	private void OnChangeStateUserInterfaceSlotItemAlbumManager(UserInterfaceSlotItemAlbumManager.State state)
	{
		switch (state)
		{
		case UserInterfaceSlotItemAlbumManager.State.SlotItemDetail:
			mUIHowToAlbum.ChangeGuideStatus(UIHowToAlbum.GuideState.Detail);
			break;
		case UserInterfaceSlotItemAlbumManager.State.SlotItemList:
			mUIHowToAlbum.ChangeGuideStatus(UIHowToAlbum.GuideState.List);
			break;
		}
	}

	private void Update()
	{
		if (mKeyController != null)
		{
			mKeyController.Update();
		}
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Releases(ref mTextures_Preload);
		mTextures_Preload = null;
		if (SingletonMonoBehaviour<UIPortFrame>.exist())
		{
			SingletonMonoBehaviour<UIPortFrame>.Instance.gameObject.SetActive(true);
		}
		mUIHowToAlbum = null;
		mUIAlbumSelectGate = null;
		mUserInterfaceShipAlbumManager = null;
		mUserInterfaceSlotItemAlbumManager = null;
		mAlbumManager = null;
		mStateManager = null;
		mKeyController = null;
	}

	private void OnBackShipAlbumListener()
	{
		if (mStateManager.CurrentState == State.ShipAlbum)
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mStateManager.PopState();
			mStateManager.PushState(State.AlbumSelectGate);
		}
	}

	private void OnBackSlotItemAlbumListener()
	{
		if (mStateManager.CurrentState == State.SlotItemAlbum)
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mStateManager.PopState();
			mStateManager.PushState(State.AlbumSelectGate);
		}
	}

	private void OnSelectedShipAlbumListener()
	{
		IEnumerator routine = OnSelectedShipAlbumListenerCoroutine();
		StartCoroutine(routine);
	}

	private IEnumerator OnSelectedShipAlbumListenerCoroutine()
	{
		if (!mUserInterfaceShipAlbumManager.Initialized)
		{
			mAlbumManager.Init();
			int ceilingedModelCount = (int)(Math.Ceiling((float)mAlbumManager.ShipLastNo / 10f) * 10.0);
			IAlbumModel[] albumShipModels = mAlbumManager.GetShips(1, ceilingedModelCount);
			mUserInterfaceShipAlbumManager.Initialize(albumShipModels);
			yield return new WaitForEndOfFrame();
		}
		mStateManager.PopState();
		mStateManager.PushState(State.ShipAlbum);
	}

	private void OnSelectedSlotItemAlbumListener()
	{
		IEnumerator routine = OnSelectedSlotItemAlbumListenerCoroutine();
		StartCoroutine(routine);
	}

	private IEnumerator OnSelectedSlotItemAlbumListenerCoroutine()
	{
		if (!mUserInterfaceSlotItemAlbumManager.Initialized)
		{
			mAlbumManager.Init();
			int ceilingedModelCount = (int)(Math.Ceiling((float)mAlbumManager.SlotLastNo / 10f) * 10.0);
			IAlbumModel[] albumSlotItemModels = mAlbumManager.GetSlotitems(1, ceilingedModelCount);
			mUserInterfaceSlotItemAlbumManager.Initialize(albumSlotItemModels);
			yield return new WaitForEndOfFrame();
		}
		mStateManager.PopState();
		mStateManager.PushState(State.SlotItemAlbum);
	}

	private void OnSelectedBackListener()
	{
		if (mStateManager.CurrentState == State.AlbumSelectGate)
		{
			mStateManager.PushState(State.MoveToNextScene);
		}
	}

	private void OnPushState(State state)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		switch (state)
		{
		case State.AlbumSelectGate:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			mUIAlbumSelectGate.Initialize(SingletonMonoBehaviour<AppInformation>.Instance.FlagShipModel);
			mUIAlbumSelectGate.SetActive(isActive: true);
			mUIAlbumSelectGate.SetKeyController(mKeyController);
			mUIHowToAlbum.ChangeGuideStatus(UIHowToAlbum.GuideState.Gate);
			break;
		case State.ShipAlbum:
			mUserInterfaceShipAlbumManager.SetActive(isActive: true);
			mUserInterfaceShipAlbumManager.SetKeyController(mKeyController);
			mUserInterfaceShipAlbumManager.StartState();
			break;
		case State.SlotItemAlbum:
			mUserInterfaceSlotItemAlbumManager.SetActive(isActive: true);
			mUserInterfaceSlotItemAlbumManager.SetKeyController(mKeyController);
			mUserInterfaceSlotItemAlbumManager.StartState();
			break;
		case State.MoveToNextScene:
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mKeyController = null;
			OnPushMoveToNextScene();
			break;
		}
	}

	private void OnPushMoveToNextScene()
	{
		SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
	}

	private void OnResumeState(State state)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		if (state == State.AlbumSelectGate)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			mUIAlbumSelectGate.SetActive(isActive: true);
			mUIAlbumSelectGate.SetKeyController(mKeyController);
		}
	}

	private void OnPopState(State state)
	{
		switch (state)
		{
		case State.AlbumSelectGate:
			mUIAlbumSelectGate.SetKeyController(null);
			mUIAlbumSelectGate.SetActive(isActive: false);
			break;
		case State.ShipAlbum:
			mUserInterfaceShipAlbumManager.SetActive(isActive: false);
			break;
		case State.SlotItemAlbum:
			mUserInterfaceSlotItemAlbumManager.SetActive(isActive: false);
			break;
		}
	}

	internal static bool CheckMarriaged(AlbumShipModel albumShipModel)
	{
		bool flag = albumShipModel.IsMarriage();
		int[] mstIDs = albumShipModel.MstIDs;
		int[] array = mstIDs;
		foreach (int mst_id in array)
		{
			flag |= albumShipModel.IsMarriage(mst_id);
		}
		return flag;
	}
}
