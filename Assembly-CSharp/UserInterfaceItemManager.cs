using KCV;
using KCV.Scene.Item;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UserInterfaceItemManager : MonoBehaviour
{
	public enum StartAt
	{
		ItemList,
		ItemStore
	}

	public enum State
	{
		NONE,
		Akashi,
		ItemList,
		ItemStore,
		SwitchItemListToItemStore,
		SwitchItemStoreToItemList
	}

	public static readonly object SHARE_DATA_START_AT_KEY = "share_data_start_at_key";

	public static readonly object SHARE_DATA_START_AT_VALUE_ITEMLIST = "share_data_start_at_value_itemlist";

	public static readonly object SHARE_DATA_START_AT_VALUE_ITEMSTORE = "share_data_start_at_value_itemstore";

	[SerializeField]
	private Texture[] mTextures_Preload;

	[SerializeField]
	private UIItemListManager mUIItemListManager;

	[SerializeField]
	private UIItemStoreManager mUIItemStoreManager;

	[SerializeField]
	private UIItemAkashi mUIItemAkashi;

	[SerializeField]
	private Transform mTransform_SwitchViewRoot;

	private ItemlistManager __ItemlistManager__;

	private ItemStoreManager __ItemStoreManager__;

	private KeyControl mKeyController;

	private AudioClip mAudioClip_SE002;

	private AudioClip mAudioClip_CommonCancel1;

	private AudioClip mAudioClip_SceneBGM;

	private StartAt mStartAt;

	private Stack<State> mStateStack = new Stack<State>();

	private ItemlistManager itemListManager
	{
		get
		{
			if (__ItemlistManager__ == null)
			{
				if (__ItemStoreManager__ == null)
				{
					__ItemlistManager__ = new ItemlistManager();
					__ItemlistManager__.Init();
				}
				else
				{
					__ItemlistManager__ = __ItemStoreManager__.CreateListManager();
					__ItemlistManager__.Init();
				}
			}
			return __ItemlistManager__;
		}
	}

	private ItemStoreManager itemStoreManager
	{
		get
		{
			if (__ItemStoreManager__ == null)
			{
				if (__ItemlistManager__ == null)
				{
					__ItemStoreManager__ = new ItemStoreManager();
					__ItemStoreManager__.Init();
				}
				else
				{
					__ItemStoreManager__ = __ItemlistManager__.CreateStoreManager();
					__ItemStoreManager__.Init();
				}
			}
			return __ItemStoreManager__;
		}
	}

	public State CurrentState
	{
		get
		{
			if (0 < mStateStack.Count)
			{
				return mStateStack.Peek();
			}
			return State.NONE;
		}
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Releases(ref mTextures_Preload);
		UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_SE002);
		UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_CommonCancel1);
		mAudioClip_SceneBGM = null;
		mUIItemListManager = null;
		mUIItemStoreManager = null;
		mUIItemAkashi = null;
		mTransform_SwitchViewRoot = null;
		__ItemlistManager__ = null;
		__ItemStoreManager__ = null;
		mKeyController = null;
	}

	public static Texture RequestItemStoreIcon(int masterId)
	{
		return Resources.Load<Texture>("Textures/Item/purchase_items/" + masterId.ToString());
	}

	private IEnumerator Start()
	{
		UserInterfacePortManager.ReleaseUtils.OverwriteCheck();
		mStartAt = GetStartAt();
		Stopwatch stopWatch = new Stopwatch();
		stopWatch.Reset();
		stopWatch.Start();
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		mKeyController = new KeyControl();
		mAudioClip_SE002 = SoundFile.LoadSE(SEFIleInfos.SE_002);
		mAudioClip_CommonCancel1 = SoundFile.LoadSE(SEFIleInfos.CommonCancel1);
		mUIItemListManager.SetOnBackListener(OnItemListBack);
		mUIItemListManager.SetOnSwitchItemStoreListener(OnSwitchToItemStore);
		mUIItemListManager.SetKeyController(null);
		mUIItemStoreManager.SetOnBackListener(OnItemStoreBackListener);
		mUIItemStoreManager.SetOnSwitchItemListListener(OnSwitchToItemList);
		mUIItemStoreManager.SetKeyController(null);
		mUIItemAkashi.SetOnHiddenCallBack(OnAkashiHidenListener);
		int bgmId = 101;
		switch (mStartAt)
		{
		case StartAt.ItemList:
			mUIItemListManager.SetActive(isActive: true);
			mUIItemStoreManager.SetActive(isActive: false);
			mTransform_SwitchViewRoot.transform.localPosition = new Vector3(0f, 0f, 0f);
			bgmId = itemListManager.UserInfo.GetPortBGMId(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
			mUIItemListManager.Initialize(itemListManager);
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(itemListManager);
			}
			break;
		case StartAt.ItemStore:
			mUIItemListManager.SetActive(isActive: false);
			mUIItemStoreManager.SetActive(isActive: true);
			mTransform_SwitchViewRoot.transform.localPosition = new Vector3(-960f, 0f, 0f);
			bgmId = itemStoreManager.UserInfo.GetPortBGMId(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckID);
			mUIItemStoreManager.Initialize(itemStoreManager);
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.CircleUpdateInfo(itemStoreManager);
			}
			break;
		}
		mAudioClip_SceneBGM = SoundFile.LoadBGM((BGMFileInfos)bgmId);
		stopWatch.Stop();
		if (RetentionData.GetData() != null)
		{
			RetentionData.GetData().Clear();
		}
		for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
		{
			yield return new WaitForEndOfFrame();
		}
		SingletonMonoBehaviour<PortObjectManager>.Instance.PortTransition.EndTransition(delegate
		{
			SoundUtils.PlayBGM(this.mAudioClip_SceneBGM, isLoop: true);
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			this.mUIItemAkashi.SetKeyController(this.mKeyController);
			this.mUIItemAkashi.Show();
			this.mUIItemAkashi.SetClickable(clickable: true);
			this.DelayAction(0.2f, delegate
			{
				ShipUtils.PlayPortVoice(4);
			});
			this.ChangeState(State.Akashi, popStack: false);
		});
	}

	private StartAt GetStartAt()
	{
		if (RetentionData.GetData() != null && RetentionData.GetData().Contains(SHARE_DATA_START_AT_KEY))
		{
			object obj = RetentionData.GetData()[SHARE_DATA_START_AT_KEY];
			if (obj == SHARE_DATA_START_AT_VALUE_ITEMLIST)
			{
				return StartAt.ItemList;
			}
			if (obj == SHARE_DATA_START_AT_VALUE_ITEMSTORE)
			{
				return StartAt.ItemStore;
			}
		}
		return StartAt.ItemList;
	}

	private void OnAkashiHidenListener()
	{
		if (CurrentState == State.Akashi)
		{
			switch (mStartAt)
			{
			case StartAt.ItemList:
				ChangeState(State.ItemList, popStack: true);
				break;
			case StartAt.ItemStore:
				ChangeState(State.ItemStore, popStack: true);
				break;
			}
		}
	}

	private void Update()
	{
		if (mKeyController != null)
		{
			mKeyController.Update();
		}
	}

	private void SwitchView(State moveToState, Action onFinishedSwitch)
	{
		SoundUtils.PlaySE(mAudioClip_SE002);
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(mTransform_SwitchViewRoot.gameObject, 0.8f);
		switch (moveToState)
		{
		case State.ItemList:
			tweenPosition.from = mTransform_SwitchViewRoot.transform.localPosition;
			tweenPosition.to = new Vector3(0f, tweenPosition.from.y, tweenPosition.from.z);
			tweenPosition.SetOnFinished(delegate
			{
				if (onFinishedSwitch != null)
				{
					onFinishedSwitch();
				}
			});
			break;
		case State.ItemStore:
			tweenPosition.from = mTransform_SwitchViewRoot.transform.localPosition;
			tweenPosition.to = new Vector3(-960f, tweenPosition.from.y, tweenPosition.from.z);
			tweenPosition.SetOnFinished(delegate
			{
				if (onFinishedSwitch != null)
				{
					onFinishedSwitch();
				}
			});
			break;
		}
	}

	private void ChangeState(State state, bool popStack)
	{
		if (popStack && 0 < mStateStack.Count)
		{
			PopState();
		}
		mStateStack.Push(state);
		OnPushState(mStateStack.Peek());
	}

	private void PopState()
	{
		if (0 < mStateStack.Count)
		{
			State state = mStateStack.Pop();
			OnPopState(state);
			if (0 < mStateStack.Count)
			{
				OnResumeState(mStateStack.Peek());
			}
		}
	}

	private void OnPushState(State state)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		switch (state)
		{
		case State.ItemList:
			mUIItemListManager.SetKeyController(mKeyController);
			mUIItemListManager.StartState();
			break;
		case State.ItemStore:
			mUIItemStoreManager.SetKeyController(mKeyController);
			mUIItemStoreManager.StartState();
			break;
		}
	}

	private void OnPopState(State state)
	{
		switch (state)
		{
		case State.Akashi:
			OnPopStateAkashi();
			break;
		case State.ItemList:
			OnPopStateItemList();
			break;
		}
	}

	private void OnResumeState(State state)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
	}

	private void OnPopStateAkashi()
	{
		mUIItemAkashi.SetOnHiddenCallBack(null);
		mUIItemAkashi.SetClickable(clickable: false);
	}

	private void OnItemListBack()
	{
		if (CurrentState == State.ItemList)
		{
			mKeyController.IsRun = false;
			SoundUtils.PlaySE(mAudioClip_CommonCancel1);
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
		}
	}

	private void OnSwitchToItemStore()
	{
		ChangeState(State.SwitchItemListToItemStore, popStack: true);
		StartCoroutine(OnSwitchToItemStoreCoroutine());
	}

	private IEnumerator OnSwitchToItemStoreCoroutine()
	{
		Stopwatch stopWatch = new Stopwatch();
		stopWatch.Reset();
		stopWatch.Start();
		mStateStack.Clear();
		mUIItemListManager.SetKeyController(null);
		mUIItemStoreManager.SetActive(isActive: true);
		if (__ItemStoreManager__ != null)
		{
			__ItemStoreManager__.Init();
		}
		mUIItemStoreManager.Initialize(itemStoreManager);
		stopWatch.Stop();
		for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
		{
			yield return new WaitForEndOfFrame();
		}
		SwitchView(State.ItemStore, delegate
		{
			this.mUIItemListManager.Clean();
			this.mUIItemListManager.SetActive(isActive: false);
			this.ChangeState(State.ItemStore, popStack: true);
		});
	}

	private void OnSwitchToItemList()
	{
		ChangeState(State.SwitchItemStoreToItemList, popStack: true);
		StartCoroutine(OnSwitchToItemListCoroutine());
	}

	private IEnumerator OnSwitchToItemListCoroutine()
	{
		Stopwatch stopWatch = new Stopwatch();
		stopWatch.Reset();
		stopWatch.Start();
		mStateStack.Clear();
		mUIItemStoreManager.SetKeyController(null);
		mUIItemStoreManager.LockControl();
		mUIItemListManager.SetActive(isActive: true);
		if (__ItemlistManager__ != null)
		{
			__ItemlistManager__.Init();
		}
		mUIItemListManager.Initialize(itemListManager);
		stopWatch.Stop();
		for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
		{
			yield return new WaitForEndOfFrame();
		}
		SwitchView(State.ItemList, delegate
		{
			this.mUIItemListManager.SetKeyController(this.mKeyController);
			this.mUIItemStoreManager.Release();
			this.mUIItemStoreManager.SetActive(isActive: false);
			this.ChangeState(State.ItemList, popStack: true);
		});
	}

	private void OnPopStateItemList()
	{
		mUIItemListManager.SetKeyController(null);
	}

	private void OnItemStoreBackListener()
	{
		if (CurrentState == State.ItemStore)
		{
			mKeyController.IsRun = false;
			SoundUtils.PlaySE(mAudioClip_CommonCancel1);
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
		}
	}

	private void OnPopStateItemStore()
	{
		mUIItemStoreManager.Release();
	}

	private string StateToString()
	{
		mStateStack.ToArray();
		string text = string.Empty;
		foreach (State item in mStateStack)
		{
			text = item + " > " + text;
		}
		return text;
	}
}
