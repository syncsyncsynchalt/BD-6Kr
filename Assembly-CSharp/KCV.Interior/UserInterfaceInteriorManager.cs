using Common.Enum;
using KCV.InteriorStore;
using KCV.Scene.Others;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Interior
{
	public class UserInterfaceInteriorManager : MonoBehaviour
	{
		private enum State
		{
			NONE,
			InteriorChange,
			FurnitureStore,
			MoveToFurnitureStore,
			MoveToInteriorChange
		}

		public class StateManager<State>
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

		[SerializeField]
		private Texture[] mTextures_Preload;

		[SerializeField]
		private UserInterfaceInteriorChangeManager mUserInterfaceInteriorChangeManager;

		private UIInteriorStoreManager mUIInteriorStoreManager;

		[SerializeField]
		private UserInterfaceInteriorTransitionManager mUserInterfaceInteriorTransitionManager;

		[SerializeField]
		private UserInterfacePortInteriorManager mUserInterfacePortInteriorManager;

		[SerializeField]
		private UIInteriorStoreManager mPrefab_UIInteriorStoreManager;

		[SerializeField]
		private Camera mCamera_SwipeEventCatch;

		private StateManager<State> mStateManager;

		private InteriorManager mInteriorManager;

		private KeyControl mKeyController;

		public static void Debug_AddFurniture()
		{
			new List<int>();
			List<int> mst_id = (from x in Mst_DataManager.Instance.Mst_furniture.Values
				where x.Saleflg == 1
				select x.Id).ToList();
			new Debug_Mod().AddFurniture(mst_id);
		}

		public static void Debug_AddCoin()
		{
			new Debug_Mod().Add_Coin(80000);
		}

		public static string FurnitureKindToString(FurnitureKinds kind)
		{
			switch (kind)
			{
			case FurnitureKinds.Chest:
				return "家具";
			case FurnitureKinds.Desk:
				return "椅子＋机";
			case FurnitureKinds.Floor:
				return "床";
			case FurnitureKinds.Hangings:
				return "装飾";
			case FurnitureKinds.Wall:
				return "壁紙";
			case FurnitureKinds.Window:
				return "窓枠＋カーテン";
			default:
				return string.Empty;
			}
		}

		private IEnumerator Start()
		{
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Reset();
			stopWatch.Start();
			yield return new WaitForEndOfFrame();
			mStateManager = new StateManager<State>(State.NONE);
			mStateManager.OnPop = OnPopState;
			mStateManager.OnPush = OnPushState;
			mStateManager.OnResume = OnResumeState;
			mStateManager.OnSwitch = OnSwitchState;
			mUserInterfaceInteriorChangeManager.SetOnRequestMoveToFurnitureStoreListener(OnRequestMoveFurnitureStore);
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.gameObject.SetActive(false);
			}
			mKeyController = new KeyControl();
			AudioClip sceneBGM = SoundFile.LoadBGM((BGMFileInfos)104);
			stopWatch.Stop();
			for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
			{
				yield return new WaitForEndOfFrame();
			}
			SoundUtils.SwitchBGM(sceneBGM);
			SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(null);
			mStateManager.PushState(State.InteriorChange);
		}

		private void PreviewFromItemStore(FurnitureKinds furnitureKind, FurnitureModel furnitureModel, Action onFinished)
		{
		}

		private void OnRequestMoveToInterior()
		{
			if (mStateManager.CurrentState == State.FurnitureStore)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				mStateManager.PopState();
				mStateManager.PushState(State.MoveToInteriorChange);
			}
		}

		private void OnRequestMoveFurnitureStore()
		{
			if (mStateManager.CurrentState == State.InteriorChange)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				mStateManager.PopState();
				mStateManager.PushState(State.MoveToFurnitureStore);
			}
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				mKeyController.Update();
				if (SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable && mKeyController.IsRDown())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
			}
		}

		public void Release()
		{
			mUserInterfaceInteriorChangeManager.Release();
			mUserInterfaceInteriorTransitionManager.Release();
			mUserInterfaceInteriorChangeManager = null;
			mUserInterfaceInteriorTransitionManager = null;
		}

		private void OnPopState(State state)
		{
			if (state == State.InteriorChange)
			{
				mUserInterfaceInteriorChangeManager.Clean();
			}
		}

		private void OnPushState(State state)
		{
			switch (state)
			{
			case State.InteriorChange:
				OnPushStateInteriorChange();
				break;
			case State.FurnitureStore:
				OnPushStateFurnitureStore();
				break;
			case State.MoveToFurnitureStore:
			{
				IEnumerator routine2 = MoveToFurnitureStoreCoroutine();
				StartCoroutine(routine2);
				break;
			}
			case State.MoveToInteriorChange:
			{
				IEnumerator routine = MoveToInteriorChangeCoroutine();
				StartCoroutine(routine);
				break;
			}
			}
		}

		private void OnPushStateInteriorChange()
		{
			StartCoroutine(OnPushStateInteriorChangeCoroutine());
		}

		private void OnPushStateFurnitureStore()
		{
			StartCoroutine(OnPushStateFurnitureStoreCoroutine());
		}

		private IEnumerator OnPushStateFurnitureStoreCoroutine()
		{
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Reset();
			stopWatch.Start();
			mUserInterfacePortInteriorManager.gameObject.SetActive(false);
			if (mUIInteriorStoreManager == null)
			{
				mUIInteriorStoreManager = Util.Instantiate(mPrefab_UIInteriorStoreManager.gameObject, base.gameObject).GetComponent<UIInteriorStoreManager>();
				mUIInteriorStoreManager.SetSwipeEventCamera(mCamera_SwipeEventCatch);
				mUIInteriorStoreManager.SetOnRequestMoveToInteriorListener(OnRequestMoveToInterior);
			}
			mUIInteriorStoreManager.gameObject.SetActive(true);
			FurnitureStoreManager furnitureStoreManager = new FurnitureStoreManager();
			mUIInteriorStoreManager.Initialize(mInteriorManager, furnitureStoreManager, mUserInterfacePortInteriorManager);
			mUIInteriorStoreManager.SetKeyController(mKeyController);
			stopWatch.Stop();
			for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
			{
				yield return new WaitForEndOfFrame();
			}
			yield return new WaitForEndOfFrame();
			mUIInteriorStoreManager.StartState();
		}

		private IEnumerator OnPushStateInteriorChangeCoroutine()
		{
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Reset();
			stopWatch.Start();
			mUserInterfacePortInteriorManager.gameObject.SetActive(true);
			mUserInterfaceInteriorChangeManager.SetActive(isActive: true);
			mUserInterfaceInteriorChangeManager.SetKeyController(mKeyController);
			mInteriorManager = new InteriorManager(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
			mUserInterfaceInteriorChangeManager.Initialize(mInteriorManager);
			mUserInterfaceInteriorChangeManager.SetSwipeEventCamera(mCamera_SwipeEventCatch);
			stopWatch.Stop();
			mUserInterfaceInteriorChangeManager.StartState();
			for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
			{
				yield return null;
			}
			yield return null;
		}

		private void OnDestroy()
		{
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.gameObject.SetActive(true);
			}
			UserInterfacePortManager.ReleaseUtils.Releases(ref mTextures_Preload);
			mUserInterfaceInteriorChangeManager = null;
			mUserInterfaceInteriorTransitionManager = null;
			mUIInteriorStoreManager = null;
			mStateManager = null;
			mInteriorManager = null;
			mKeyController = null;
			mUserInterfacePortInteriorManager = null;
		}

		private IEnumerator MoveToFurnitureStoreCoroutine()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mKeyController.IsRun = false;
			mUserInterfaceInteriorTransitionManager.SetActive(isActive: true);
			mUserInterfaceInteriorTransitionManager.SwitchToStore(delegate
			{
				this.mKeyController.IsRun = true;
				this.mUserInterfaceInteriorChangeManager.SetActive(isActive: false);
				this.mUserInterfaceInteriorTransitionManager.SetActive(isActive: false);
				this.mStateManager.PopState();
				this.mStateManager.PushState(State.FurnitureStore);
			});
			yield return null;
		}

		private IEnumerator MoveToInteriorChangeCoroutine()
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mKeyController.IsRun = false;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			mUserInterfaceInteriorTransitionManager.SetActive(isActive: true);
			mUserInterfaceInteriorTransitionManager.SwitchToHome(delegate
			{
				this.mKeyController.IsRun = true;
				this.mUIInteriorStoreManager.SetActive(isActive: false);
				this.mUserInterfaceInteriorChangeManager.SetActive(isActive: true);
				this.mUserInterfaceInteriorTransitionManager.SetActive(isActive: false);
				this.mStateManager.PopState();
				this.mStateManager.PushState(State.InteriorChange);
			});
			yield return null;
		}

		private void OnSwitchState(State state)
		{
		}

		private void OnResumeState(State state)
		{
		}
	}
}
