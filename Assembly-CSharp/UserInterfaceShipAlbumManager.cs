using DG.Tweening;
using KCV;
using KCV.PortTop;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class UserInterfaceShipAlbumManager : MonoBehaviour
{
	public enum State
	{
		None,
		ShipList,
		ShipDetail,
		ShipDetailMarriaged,
		MarriagedMovie
	}

	private class Context
	{
		private IAlbumModel mAlbumModel;

		public IAlbumModel GetAlbumModel()
		{
			return mAlbumModel;
		}

		public void SetAlbumModel(IAlbumModel albumModel)
		{
			mAlbumModel = albumModel;
		}
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

	private BGMFileInfos DefaultBGM = BGMFileInfos.Port;

	[SerializeField]
	private UIShipAlbumList mUIShipAlbumList;

	[SerializeField]
	private UIShipAlbumDetail mUIShipAlbumDetail;

	[SerializeField]
	private UIShipAlbumDetailForMarriaged mUIShipAlbumDetailForMarriaged;

	private MarriageCutManager mMarriageCutManager;

	private KeyControl mKeyController;

	private IAlbumModel[] mAlbumModels;

	private StateManager<State> mStateManager;

	private Context mContext;

	private Action mOnBackListener;

	private Action<State> mOnChangeStateUserInterfaceShipAlbumManager;

	public bool Initialized
	{
		get;
		private set;
	}

	public void Initialize(IAlbumModel[] albumModels)
	{
		mAlbumModels = albumModels;
		mUIShipAlbumList.SetOnSelectedListItemListener(OnSelectedListItemListener);
		mUIShipAlbumList.SetOnBackListener(OnBackListListener);
		mUIShipAlbumDetail.SetActive(isActive: false);
		mUIShipAlbumDetail.SetOnBackListener(OnBackAlbumDetailListener);
		mUIShipAlbumDetailForMarriaged.SetActive(isActive: false);
		mUIShipAlbumDetailForMarriaged.SetOnBackListener(OnBackAlbumDetailForMarriagedListener);
		mUIShipAlbumDetailForMarriaged.SetOnRequestPlayMarriageMovieListener(OnRequestPlayMarriageMovie);
		mStateManager = new StateManager<State>(State.None);
		mStateManager.OnPush = OnPushState;
		mStateManager.OnPop = OnPopState;
		mStateManager.OnResume = OnResumeState;
		mStateManager.OnSwitch = OnChangeState;
		Initialized = true;
	}

	private void OnRequestPlayMarriageMovie()
	{
		if (mStateManager.CurrentState == State.ShipDetailMarriaged)
		{
			mStateManager.PushState(State.MarriagedMovie);
		}
	}

	private void OnBackAlbumDetailListener(Tween closeTween)
	{
		if (mStateManager.CurrentState == State.ShipDetail)
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mUIShipAlbumDetail.SetKeyController(null);
			closeTween.OnComplete(delegate
			{
				mStateManager.PopState();
				mStateManager.ResumeState();
				mUIShipAlbumDetail.SetActive(isActive: false);
			});
		}
	}

	private void OnBackAlbumDetailForMarriagedListener(Tween closeTween)
	{
		if (mStateManager.CurrentState == State.ShipDetailMarriaged)
		{
			mKeyController.ClearKeyAll();
			mKeyController.firstUpdate = true;
			mUIShipAlbumDetailForMarriaged.SetKeyController(null);
			closeTween.OnComplete(delegate
			{
				mStateManager.PopState();
				mStateManager.ResumeState();
				mUIShipAlbumDetailForMarriaged.SetActive(isActive: false);
			});
		}
	}

	private void OnBackListListener()
	{
		if (mStateManager.CurrentState == State.ShipList)
		{
			mStateManager.PopState();
			OnBack();
		}
	}

	public void SetOnBackListener(Action onBack)
	{
		mOnBackListener = onBack;
	}

	private void OnBack()
	{
		if (mOnBackListener != null)
		{
			mOnBackListener();
		}
	}

	private void OnSelectedListItemListener(IAlbumModel albumModel)
	{
		if (mStateManager.CurrentState == State.ShipList && albumModel is AlbumShipModel)
		{
			AlbumShipModel albumShipModel = (AlbumShipModel)albumModel;
			mContext.SetAlbumModel(albumShipModel);
			mUIShipAlbumList.SetKeyController(null);
			if (UserInterfaceAlbumManager.CheckMarriaged(albumShipModel))
			{
				mStateManager.PushState(State.ShipDetailMarriaged);
			}
			else
			{
				mStateManager.PushState(State.ShipDetail);
			}
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	public void StartState()
	{
		mContext = new Context();
		mStateManager.PushState(State.ShipList);
	}

	private void OnPushState(State state)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		switch (state)
		{
		case State.ShipList:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			OnPushShipListState();
			break;
		case State.ShipDetail:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			OnPushShipDetailState();
			break;
		case State.ShipDetailMarriaged:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			OnPushShipDetailForMarriagedState();
			break;
		case State.MarriagedMovie:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			mUIShipAlbumDetailForMarriaged.SetKeyController(null);
			OnPushMarriagedMovieState();
			break;
		}
	}

	private void OnPushShipListState()
	{
		IEnumerator routine = OnPushShipListStateCoroutine();
		StartCoroutine(routine);
	}

	private IEnumerator OnPushShipListStateCoroutine()
	{
		mUIShipAlbumList.SetActive(isActive: true);
		mUIShipAlbumList.Initialize(mAlbumModels);
		mUIShipAlbumList.StartState();
		yield return new WaitForEndOfFrame();
		mUIShipAlbumList.SetKeyController(mKeyController);
	}

	private void OnPushMarriagedMovieState()
	{
		SingletonMonoBehaviour<SoundManager>.Instance.StopVoice();
		int graphicShipId = mUIShipAlbumDetailForMarriaged.FocusTextureInfo().GetGraphicShipId();
		IEnumerator routine = OnPushMarriagedMovieStateCoroutine(graphicShipId);
		StartCoroutine(routine);
	}

	private IEnumerator OnPushMarriagedMovieStateCoroutine(int graphicShipId)
	{
		ResourceRequest requestPrefabMarriageCut = Resources.LoadAsync("Prefabs/PortTop/MarriageCut");
		yield return requestPrefabMarriageCut;
		GameObject prefabMarriageCut = requestPrefabMarriageCut.asset as GameObject;
		mMarriageCutManager = Util.Instantiate(prefabMarriageCut, base.transform.gameObject).GetComponent<MarriageCutManager>();
		yield return new WaitForEndOfFrame();
		mMarriageCutManager.Initialize(graphicShipId, mKeyController, OnFinishedMarriageMovie);
		yield return StartCoroutine(mMarriageCutManager.Play());
	}

	private void OnPushShipDetailState()
	{
		IEnumerator routine = OnPushShipDetailStateCoroutine();
		StartCoroutine(routine);
	}

	private IEnumerator OnPushShipDetailStateCoroutine()
	{
		mUIShipAlbumDetail.SetActive(isActive: true);
		mUIShipAlbumDetail.Initialize((AlbumShipModel)mContext.GetAlbumModel());
		yield return null;
		mUIShipAlbumDetail.SetKeyController(mKeyController);
		mUIShipAlbumDetail.Show();
		mUIShipAlbumDetail.StartState();
	}

	private void OnPushShipDetailForMarriagedState()
	{
		IEnumerator routine = OnPushShipDetailForMarriagedStateCoroutine();
		StartCoroutine(routine);
	}

	private IEnumerator OnPushShipDetailForMarriagedStateCoroutine()
	{
		mUIShipAlbumDetailForMarriaged.SetActive(isActive: true);
		mUIShipAlbumDetailForMarriaged.Initialize((AlbumShipModel)mContext.GetAlbumModel());
		yield return null;
		mUIShipAlbumDetailForMarriaged.SetKeyController(mKeyController);
		mUIShipAlbumDetailForMarriaged.Show();
		mUIShipAlbumDetailForMarriaged.StartState();
	}

	private void OnFinishedMarriageMovie()
	{
		if (mStateManager.CurrentState == State.MarriagedMovie)
		{
			DOVirtual.Float(mMarriageCutManager.Alpha, 0f, 0.3f, delegate(float alpha)
			{
				mMarriageCutManager.Alpha = alpha;
			}).OnComplete(delegate
			{
				SingletonMonoBehaviour<SoundManager>.Instance.SwitchBGM(DefaultBGM);
				mMarriageCutManager.SetActive(isActive: false);
				UnityEngine.Object.Destroy(mMarriageCutManager.gameObject);
				mMarriageCutManager = null;
				mStateManager.PopState();
				mStateManager.ResumeState();
			});
		}
	}

	private void OnPopState(State state)
	{
		if (state == State.MarriagedMovie)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
		}
	}

	private void OnResumeState(State state)
	{
		switch (state)
		{
		case State.ShipDetail:
			break;
		case State.ShipList:
			mUIShipAlbumList.SetKeyController(mKeyController);
			mUIShipAlbumList.ResumeState();
			break;
		case State.ShipDetailMarriaged:
			mUIShipAlbumDetailForMarriaged.SetKeyController(mKeyController);
			break;
		}
	}

	internal void SetOnChangeStateListener(Action<State> onChangeStateUserInterfaceShipAlbumManager)
	{
		mOnChangeStateUserInterfaceShipAlbumManager = onChangeStateUserInterfaceShipAlbumManager;
	}

	private void OnChangeState(State state)
	{
		if (mOnChangeStateUserInterfaceShipAlbumManager != null)
		{
			mOnChangeStateUserInterfaceShipAlbumManager(state);
		}
	}

	private void OnDestroy()
	{
		mUIShipAlbumList = null;
		mUIShipAlbumDetail = null;
		mUIShipAlbumDetailForMarriaged = null;
		mMarriageCutManager = null;
		mKeyController = null;
		mAlbumModels = null;
		mStateManager = null;
		mContext = null;
		mOnChangeStateUserInterfaceShipAlbumManager = null;
	}
}
