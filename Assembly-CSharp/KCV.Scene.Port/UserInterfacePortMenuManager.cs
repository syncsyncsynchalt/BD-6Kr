using DG.Tweening;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Port
{
	[RequireComponent(typeof(UIButtonManager))]
	[RequireComponent(typeof(UIPanel))]
	public class UserInterfacePortMenuManager : MonoBehaviour
	{
		public enum State
		{
			None,
			FirstOpenMain,
			MainMenu,
			SubMenu,
			MoveToSubMenu,
			MoveToMainMenu,
			CallingNextScene,
			Wait
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

		private UIButtonManager mButtonManager;

		[Header("Sally")]
		[SerializeField]
		private UIPortMenuCenterButton mUIPortMenuButton_Sally;

		[SerializeField]
		[Header("Engage")]
		private UIPortMenuEngageButton mUIPortMenuButton_Engage;

		[SerializeField]
		[Header("Main")]
		private UIPortMenuButton mUIPortMenuButton_Organize;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Remodel;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Arsenal;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Duty;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Repair;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Supply;

		[SerializeField]
		[Header("Sub")]
		private UIPortMenuButton mUIPortMenuButton_Record;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Album;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Item;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Option;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Interior;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Save;

		[SerializeField]
		private UITexture mTexture_Shaft;

		[SerializeField]
		private UIPortMenuAnimation mUIPortMenuAnimation;

		private UIPortMenuButton[] mUIPortMenuButtons_MainMenu;

		private UIPortMenuButton[] mUIPortMenuButtons_SubMenu;

		private UIPortMenuButton[] mUIPortMenuButtons_Current;

		private UIPortMenuButton mUIPortMenuButton_Current;

		private AudioClip mAudioClip_MainMenuOnMouse;

		private UIPanel mPanelThis;

		private StateManager<State> mStateManager;

		private KeyControl mKeyController;

		private PortManager mPortManager;

		private DeckModel mDeckModel;

		private Action<Generics.Scene> mOnSelectedSceneListener;

		private Action mOnFirstOpendListener;

		public float alpha
		{
			get
			{
				if (mPanelThis != null)
				{
					return mPanelThis.alpha;
				}
				return -1f;
			}
			set
			{
				if (mPanelThis != null)
				{
					mPanelThis.alpha = value;
				}
			}
		}

		public State GetCurrentState()
		{
			return mStateManager.CurrentState;
		}

		private void Awake()
		{
			mButtonManager = GetComponent<UIButtonManager>();
			mPanelThis = GetComponent<UIPanel>();
			mButtonManager.IndexChangeAct = delegate
			{
				UIPortMenuButton uIPortMenuButton = mUIPortMenuButtons_Current[mButtonManager.nowForcusIndex];
				bool isSelectable = mUIPortMenuButtons_Current[mButtonManager.nowForcusIndex].IsSelectable;
				bool flag = mStateManager.CurrentState == State.MainMenu || mStateManager.CurrentState == State.SubMenu;
				bool flag2 = mUIPortMenuButton_Current != null && mUIPortMenuButton_Current.Equals(uIPortMenuButton);
				bool flag3 = IsControllable();
				if (flag && isSelectable && !flag2 && flag3)
				{
					ChangeFocusButton(uIPortMenuButton);
				}
			};
			mAudioClip_MainMenuOnMouse = SoundFile.LoadSE(SEFIleInfos.MainMenuOnMouse);
		}

		private bool IsControllable()
		{
			if (mKeyController != null && mKeyController.IsRun)
			{
				return true;
			}
			return false;
		}

		private UIPortMenuButton[] GeneratePortMenuMain()
		{
			List<UIPortMenuButton> list = new List<UIPortMenuButton>();
			list.Add(mUIPortMenuButton_Sally);
			list.Add(mUIPortMenuButton_Supply);
			list.Add(mUIPortMenuButton_Organize);
			list.Add(mUIPortMenuButton_Remodel);
			list.Add(mUIPortMenuButton_Arsenal);
			list.Add(mUIPortMenuButton_Duty);
			list.Add(mUIPortMenuButton_Repair);
			list.Add(mUIPortMenuButton_Engage);
			mUIPortMenuButton_Sally.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			mUIPortMenuButton_Sally.SetOnLongPressListener(OnLongPressEventListener);
			mUIPortMenuButton_Organize.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			mUIPortMenuButton_Remodel.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			mUIPortMenuButton_Arsenal.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			mUIPortMenuButton_Duty.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			mUIPortMenuButton_Repair.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			mUIPortMenuButton_Supply.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			return list.ToArray();
		}

		private UIPortMenuButton[] GeneratePortMenuSub()
		{
			List<UIPortMenuButton> list = new List<UIPortMenuButton>();
			list.Add(mUIPortMenuButton_Sally);
			list.Add(mUIPortMenuButton_Save);
			list.Add(mUIPortMenuButton_Record);
			list.Add(mUIPortMenuButton_Album);
			list.Add(mUIPortMenuButton_Item);
			list.Add(mUIPortMenuButton_Option);
			list.Add(mUIPortMenuButton_Interior);
			list.Add(mUIPortMenuButton_Engage);
			mUIPortMenuButton_Record.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			mUIPortMenuButton_Album.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			mUIPortMenuButton_Item.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			mUIPortMenuButton_Option.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			mUIPortMenuButton_Interior.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			mUIPortMenuButton_Save.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			mUIPortMenuButton_Engage.SetOnClickEventListener(OnPortMenuButtonClickEventListener);
			return list.ToArray();
		}

		private void OnLongPressEventListener()
		{
			bool flag = mStateManager.CurrentState == State.MainMenu || mStateManager.CurrentState == State.SubMenu;
			bool flag2 = IsControllable();
			if (!flag || !flag2)
			{
				return;
			}
			switch (mStateManager.CurrentState)
			{
			case State.MainMenu:
				mStateManager.PopState();
				mStateManager.PushState(State.MoveToSubMenu);
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
				}
				break;
			case State.SubMenu:
				mStateManager.PopState();
				mStateManager.PushState(State.MoveToMainMenu);
				if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
				}
				break;
			}
		}

		private void OnPortMenuButtonClickEventListener(UIPortMenuButton calledObject)
		{
			bool flag = -1 < Array.IndexOf(mUIPortMenuButtons_Current, calledObject);
			bool isSelectable = calledObject.IsSelectable;
			bool flag2 = mStateManager.CurrentState == State.MainMenu || mStateManager.CurrentState == State.SubMenu;
			bool flag3 = IsControllable();
			if (flag2 && flag && isSelectable && flag3)
			{
				mStateManager.PushState(State.CallingNextScene);
				ChangeFocusButton(calledObject);
				Tween t = calledObject.GenerateTweenClick();
				t.OnComplete(delegate
				{
					bool flag4 = mUIPortMenuButton_Current is UIPortMenuButton.CompositeMenu;
					if (mStateManager.CurrentState == State.SubMenu && !flag4)
					{
						mUIPortMenuAnimation.Initialize(calledObject);
						mUIPortMenuAnimation.PlayCollectSubAnimation();
					}
					else
					{
						OnSelectedScene(calledObject.GetScene());
					}
				});
			}
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[14].down)
			{
				if (mStateManager.CurrentState == State.MainMenu || mStateManager.CurrentState == State.SubMenu)
				{
					OnPressKeyLeft();
				}
			}
			else if (mKeyController.keyState[10].down)
			{
				if (mStateManager.CurrentState == State.MainMenu || mStateManager.CurrentState == State.SubMenu)
				{
					OnPressKeyRight();
				}
			}
			else if (mKeyController.keyState[8].down)
			{
				if (mStateManager.CurrentState == State.MainMenu || mStateManager.CurrentState == State.SubMenu)
				{
					OnPressKeyUp();
				}
			}
			else if (mKeyController.keyState[12].down)
			{
				if (mStateManager.CurrentState == State.MainMenu || mStateManager.CurrentState == State.SubMenu)
				{
					OnPressKeyDown();
				}
			}
			else if (mKeyController.keyState[4].down)
			{
				if (mStateManager.CurrentState != State.MainMenu && mStateManager.CurrentState != State.SubMenu)
				{
					return;
				}
				switch (mStateManager.CurrentState)
				{
				case State.MainMenu:
					mStateManager.PopState();
					mStateManager.PushState(State.MoveToSubMenu);
					if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
					}
					break;
				case State.SubMenu:
					mStateManager.PopState();
					mStateManager.PushState(State.MoveToMainMenu);
					if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
					}
					break;
				}
			}
			else if (mKeyController.keyState[5].down)
			{
				if (mStateManager.CurrentState == State.MainMenu || mStateManager.CurrentState == State.SubMenu)
				{
					ChangeFocusButton(mUIPortMenuButton_Sally);
					mUIPortMenuButton_Current.ClickEvent();
				}
			}
			else if (mKeyController.keyState[1].down && (mStateManager.CurrentState == State.MainMenu || mStateManager.CurrentState == State.SubMenu))
			{
				mUIPortMenuButton_Current.ClickEvent();
			}
		}

		private void OnPressKeyLeft()
		{
			UIPortMenuButton uIPortMenuButton = null;
			if (mStateManager.CurrentState == State.MainMenu)
			{
				uIPortMenuButton = mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Left;
			}
			else if (mStateManager.CurrentState == State.SubMenu)
			{
				uIPortMenuButton = ((!(mUIPortMenuButton_Current is UIPortMenuButton.CompositeMenu)) ? mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Left : ((UIPortMenuButton.CompositeMenu)mUIPortMenuButton_Current).GetSubMenuKeyMap().mUIPortMenuButton_Left);
			}
			if (uIPortMenuButton != null && uIPortMenuButton.IsSelectable)
			{
				ChangeFocusButton(uIPortMenuButton);
			}
		}

		private void OnPressKeyDown()
		{
			UIPortMenuButton uIPortMenuButton = null;
			if (mStateManager.CurrentState == State.MainMenu)
			{
				uIPortMenuButton = mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Down;
			}
			else if (mStateManager.CurrentState == State.SubMenu)
			{
				uIPortMenuButton = ((!(mUIPortMenuButton_Current is UIPortMenuButton.CompositeMenu)) ? mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Down : ((UIPortMenuButton.CompositeMenu)mUIPortMenuButton_Current).GetSubMenuKeyMap().mUIPortMenuButton_Down);
			}
			if (uIPortMenuButton != null && uIPortMenuButton.IsSelectable)
			{
				ChangeFocusButton(uIPortMenuButton);
			}
		}

		private void OnPressKeyRight()
		{
			UIPortMenuButton uIPortMenuButton = null;
			if (mStateManager.CurrentState == State.MainMenu)
			{
				uIPortMenuButton = mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Right;
			}
			else if (mStateManager.CurrentState == State.SubMenu)
			{
				uIPortMenuButton = ((!(mUIPortMenuButton_Current is UIPortMenuButton.CompositeMenu)) ? mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Right : ((UIPortMenuButton.CompositeMenu)mUIPortMenuButton_Current).GetSubMenuKeyMap().mUIPortMenuButton_Right);
			}
			if (uIPortMenuButton != null && uIPortMenuButton.IsSelectable)
			{
				ChangeFocusButton(uIPortMenuButton);
			}
		}

		private void OnPressKeyUp()
		{
			UIPortMenuButton uIPortMenuButton = null;
			if (mStateManager.CurrentState == State.MainMenu)
			{
				uIPortMenuButton = mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Top;
			}
			else if (mStateManager.CurrentState == State.SubMenu)
			{
				uIPortMenuButton = ((!(mUIPortMenuButton_Current is UIPortMenuButton.CompositeMenu)) ? mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Top : ((UIPortMenuButton.CompositeMenu)mUIPortMenuButton_Current).GetSubMenuKeyMap().mUIPortMenuButton_Top);
			}
			if (uIPortMenuButton != null && uIPortMenuButton.IsSelectable)
			{
				ChangeFocusButton(uIPortMenuButton);
			}
		}

		private void OnFinishedCollectAnimationListener()
		{
			OnSelectedScene(mUIPortMenuButton_Current.GetScene());
		}

		public void StartState()
		{
			mStateManager.PushState(State.FirstOpenMain);
		}

		public void StartWaitingState()
		{
			if ((mStateManager.CurrentState == State.MainMenu) | (mStateManager.CurrentState == State.SubMenu))
			{
				mStateManager.PushState(State.Wait);
			}
		}

		public void ResumeState()
		{
			mStateManager.PopState();
			mStateManager.ResumeState();
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void Initialize(PortManager portManager, DeckModel deckModel)
		{
			mPortManager = portManager;
			mDeckModel = deckModel;
			mUIPortMenuButton_Sally.ChangeState(UIPortMenuCenterButton.State.MainMenu);
			mUIPortMenuAnimation.Initialize(null);
			if (mUIPortMenuButton_Current != null)
			{
				mUIPortMenuButton_Current.GenerateTweenRemoveHover();
				mUIPortMenuButton_Current.RemoveHover();
				mUIPortMenuButton_Current = null;
			}
			mUIPortMenuButtons_MainMenu = GeneratePortMenuMain();
			mUIPortMenuButtons_SubMenu = GeneratePortMenuSub();
			mTexture_Shaft.transform.localScale = Vector3.zero;
			UIPortMenuButton[] array = mUIPortMenuButtons_SubMenu;
			foreach (UIPortMenuButton uIPortMenuButton in array)
			{
				bool selectable = IsValidSelectable(mPortManager, mDeckModel, uIPortMenuButton);
				Vector3 localPosition = uIPortMenuButton.transform.localPosition;
				uIPortMenuButton.transform.localPosition = mUIPortMenuButton_Sally.transform.localPosition;
				uIPortMenuButton.Initialize(selectable);
				uIPortMenuButton.gameObject.SetActive(false);
				uIPortMenuButton.alpha = 0f;
			}
			UIPortMenuButton[] array2 = mUIPortMenuButtons_MainMenu;
			foreach (UIPortMenuButton uIPortMenuButton2 in array2)
			{
				bool selectable2 = IsValidSelectable(mPortManager, mDeckModel, uIPortMenuButton2);
				Vector3 localPosition2 = uIPortMenuButton2.transform.localPosition;
				uIPortMenuButton2.transform.localPosition = mUIPortMenuButton_Sally.transform.localPosition;
				uIPortMenuButton2.transform.localScale = Vector3.zero;
				uIPortMenuButton2.Initialize(selectable2);
				uIPortMenuButton2.gameObject.SetActive(false);
				uIPortMenuButton2.alpha = 0f;
			}
			mUIPortMenuAnimation.SetOnFinishedCollectAnimationListener(OnFinishedCollectAnimationListener);
			UIPortMenuButton[] array3 = mUIPortMenuButtons_MainMenu;
			foreach (UIPortMenuButton uIPortMenuButton3 in array3)
			{
				bool selectable3 = IsValidSelectable(mPortManager, mDeckModel, uIPortMenuButton3);
				uIPortMenuButton3.Initialize(selectable3);
			}
			UIPortMenuButton[] array4 = mUIPortMenuButtons_SubMenu;
			foreach (UIPortMenuButton uIPortMenuButton4 in array4)
			{
				bool selectable4 = IsValidSelectable(mPortManager, mDeckModel, uIPortMenuButton4);
				uIPortMenuButton4.Initialize(selectable4);
			}
			mStateManager = new StateManager<State>(State.None);
			mStateManager.OnPush = OnPushState;
			mStateManager.OnPop = OnPopState;
			mStateManager.OnResume = OnResumeState;
		}

		public void SetOnSelectedSceneListener(Action<Generics.Scene> onSelectedSceneListener)
		{
			mOnSelectedSceneListener = onSelectedSceneListener;
		}

		private void OnSelectedScene(Generics.Scene selectedScene)
		{
			if (mOnSelectedSceneListener != null)
			{
				mOnSelectedSceneListener(selectedScene);
			}
		}

		private bool IsValidSelectable(PortManager portManager, DeckModel deckModel, UIPortMenuButton portMenuButton)
		{
			Generics.Scene scene = portMenuButton.GetScene();
			if (scene == Generics.Scene.Marriage)
			{
				return portManager.IsValidMarriage(deckModel.GetFlagShip().MemId);
			}
			return SingletonMonoBehaviour<AppInformation>.Instance.IsValidMoveToScene(portMenuButton.GetScene());
		}

		private void OnPushState(State state)
		{
			switch (state)
			{
			case State.FirstOpenMain:
				OnPushFirstOpenMain();
				break;
			case State.MoveToMainMenu:
				OnPushMoveToMainMenu();
				break;
			case State.MoveToSubMenu:
				OnPushMoveToSubMenu();
				break;
			case State.MainMenu:
				OnPushMainMenu();
				break;
			case State.SubMenu:
				OnPushSubMenu();
				break;
			}
		}

		private void OnPushSubMenu()
		{
			mUIPortMenuButtons_Current = mUIPortMenuButtons_SubMenu;
			mButtonManager.UpdateButtons(mUIPortMenuButtons_Current);
			SetKeyController(mKeyController);
		}

		private void OnPushMainMenu()
		{
			mUIPortMenuButtons_Current = mUIPortMenuButtons_MainMenu;
			mButtonManager.UpdateButtons(mUIPortMenuButtons_Current);
			SetKeyController(mKeyController);
		}

		private void OnPushFirstOpenMain()
		{
			StartCoroutine(OnPushFirstOpenMainCoroutine());
		}

		public void SetOnFirstOpendListener(Action onFirstOpendListener)
		{
			mOnFirstOpendListener = onFirstOpendListener;
		}

		private IEnumerator OnPushFirstOpenMainCoroutine()
		{
			mUIPortMenuButtons_Current = mUIPortMenuButtons_MainMenu;
			mUIPortMenuButton_Current = mUIPortMenuButton_Sally;
			mButtonManager.UpdateButtons(mUIPortMenuButtons_Current);
			mUIPortMenuButtons_MainMenu.ForEach(delegate(UIPortMenuButton targetObject)
			{
				if (!targetObject.Equals(this.mUIPortMenuButton_Sally) && !targetObject.Equals(this.mUIPortMenuButton_Engage))
				{
					targetObject.gameObject.SetActive(true);
				}
			});
			if (mUIPortMenuButton_Engage.IsSelectable)
			{
				mUIPortMenuButton_Engage.transform.localPosition = mUIPortMenuButton_Engage.GetDefaultLocalPosition();
				mUIPortMenuButton_Engage.gameObject.SetActive(true);
				mUIPortMenuButton_Engage.alpha = 1f;
			}
			else
			{
				mUIPortMenuButton_Engage.gameObject.SetActive(false);
				mUIPortMenuButton_Engage.alpha = 0f;
			}
			yield return new WaitForEndOfFrame();
			Sequence shaftSequence = DOTween.Sequence().SetId(this);
			shaftSequence.OnPlay(delegate
			{
				this.mTexture_Shaft.transform.localScale = Vector3.zero;
				this.mUIPortMenuButton_Sally.alpha = 1f;
			});
			Tween shaftIn = mTexture_Shaft.transform.DOScale(new Vector3(1f, 1f), 0.6f).SetId(this);
			shaftSequence.Append(shaftIn);
			Sequence sallyIn = DOTween.Sequence().SetId(this);
			sallyIn.OnPlay(delegate
			{
				this.mUIPortMenuButton_Sally.transform.localScale = Vector3.zero;
				this.mUIPortMenuButton_Sally.gameObject.SetActive(true);
			});
			sallyIn.Append(mUIPortMenuButton_Sally.transform.DOScale(new Vector3(1.1f, 1.1f), 0.1f).SetId(this));

            var showOthers = DOTween.Sequence().SetId(this);

            mUIPortMenuButtons_MainMenu.ForEach(delegate(UIPortMenuButton targetObject)
			{
				var _003COnPushFirstOpenMainCoroutine_003Ec__IteratorAE = this;
				if (!targetObject.Equals(this.mUIPortMenuButton_Sally) && !targetObject.Equals(this.mUIPortMenuButton_Engage))
				{
					Vector3 defaultLocalPosition = targetObject.GetDefaultLocalPosition();
					Tween t = targetObject.transform.DOLocalMove(defaultLocalPosition, 0.25f).SetId(this);
					t.OnPlay(delegate
					{
						targetObject.alpha = 1f;
					});
                    showOthers.Join(t);
				}
			});
			Sequence mainSequence = DOTween.Sequence().SetId(this);
			mainSequence.Join(shaftSequence);
			mainSequence.Join(sallyIn);
			mainSequence.OnComplete(delegate
			{
				Sequence s = DOTween.Sequence().SetId(this);
				s.AppendInterval(0.3f);
				s.Append(showOthers);
				this.mUIPortMenuButtons_MainMenu.ForEach(delegate(UIPortMenuButton targetObject)
				{
					if (!targetObject.Equals(this.mUIPortMenuButton_Current))
					{
						targetObject.GenerateTweenRemoveHover();
						targetObject.RemoveHover();
					}
				});
				if (this.mOnFirstOpendListener != null)
				{
					this.mOnFirstOpendListener();
				}
			});
			mStateManager.PopState();
			mStateManager.PushState(State.MainMenu);
			mUIPortMenuButton_Sally.GenerateTweenFocus();
			YieldInstruction mainSequenceInstruction = mainSequence.WaitForCompletion();
			yield return new WaitForEndOfFrame();
			yield return mainSequenceInstruction;
		}

		private void OnPopState(State state)
		{
			switch (state)
			{
			case State.FirstOpenMain:
				break;
			case State.MainMenu:
				ChangeActiveMenuButtons(mUIPortMenuButtons_MainMenu, activeState: false);
				break;
			case State.SubMenu:
				ChangeActiveMenuButtons(mUIPortMenuButtons_SubMenu, activeState: false);
				break;
			}
		}

		private void OnResumeState(State state)
		{
			switch (state)
			{
			case State.FirstOpenMain:
			case State.MainMenu:
			case State.SubMenu:
				ChangeFocusButton(mUIPortMenuButton_Current);
				break;
			}
		}

		private Tween GenerateTweenCloseMain()
		{
			ChangeActiveMenuButtons(mUIPortMenuButtons_MainMenu, activeState: false);
			return GenerateTweenCloseButtons(mUIPortMenuButtons_MainMenu);
		}

		private Tween GenerateTweenCloseSubMenu()
		{
			ChangeActiveMenuButtons(mUIPortMenuButtons_SubMenu, activeState: false);
			return GenerateTweenCloseButtons(mUIPortMenuButtons_SubMenu);
		}

		private Tween GenerateTweenOpenMainMenu()
		{
			ChangeActiveMenuButtons(mUIPortMenuButtons_MainMenu, activeState: true);
			return GenerateTweenOpenButtons(mUIPortMenuButtons_MainMenu);
		}

		private Tween GenerateTweenOpenSubMenu()
		{
			ChangeActiveMenuButtons(mUIPortMenuButtons_SubMenu, activeState: true);
			return GenerateTweenOpenButtons(mUIPortMenuButtons_SubMenu);
		}

		private Tween GenerateTweenCloseButtons(UIPortMenuButton[] targetPortMenuButtons)
		{
			Sequence sequence = DOTween.Sequence().SetId(this);
			foreach (UIPortMenuButton uIPortMenuButton in targetPortMenuButtons)
			{
				if (uIPortMenuButton.Equals(mUIPortMenuButton_Sally))
				{
					Tween t = uIPortMenuButton.transform.DOScale(new Vector3(0.6f, 0.6f), 0.15f);
					sequence.Join(t);
				}
				else if (!uIPortMenuButton.Equals(mUIPortMenuButton_Engage))
				{
					Vector3 defaultLocalPosition = mUIPortMenuButton_Sally.GetDefaultLocalPosition();
					Tween t2 = uIPortMenuButton.transform.DOLocalMove(defaultLocalPosition, 0.15f).SetEase(Ease.OutQuad).SetId(this);
					sequence.Join(t2);
				}
			}
			return sequence;
		}

		private Tween GenerateTweenOpenButtons(UIPortMenuButton[] targetPortMenuButtons)
		{
			Sequence sequence = DOTween.Sequence().SetId(this);
			foreach (UIPortMenuButton uIPortMenuButton in targetPortMenuButtons)
			{
				if (uIPortMenuButton.Equals(mUIPortMenuButton_Sally))
				{
					Tween t = uIPortMenuButton.transform.DOScale(Vector3.one, 0.15f);
					sequence.Append(t);
				}
				else if (!uIPortMenuButton.Equals(mUIPortMenuButton_Engage))
				{
					Vector3 defaultLocalPosition = uIPortMenuButton.GetDefaultLocalPosition();
					Tween t2 = uIPortMenuButton.transform.DOLocalMove(defaultLocalPosition, 0.15f).SetEase(Ease.OutQuad).SetId(this);
					sequence.Join(t2);
				}
			}
			return sequence;
		}

		private void ChangeActiveMenuButtons(UIPortMenuButton[] targetButtons, bool activeState)
		{
			foreach (UIPortMenuButton uIPortMenuButton in targetButtons)
			{
				bool flag = !uIPortMenuButton.Equals(mUIPortMenuButton_Sally);
				if (flag & !uIPortMenuButton.Equals(mUIPortMenuButton_Engage))
				{
					if (activeState)
					{
						uIPortMenuButton.transform.localScale = Vector3.one;
						uIPortMenuButton.alpha = 1f;
						uIPortMenuButton.SetActive(isActive: true);
					}
					else
					{
						uIPortMenuButton.transform.localScale = Vector3.zero;
						uIPortMenuButton.alpha = 0f;
						uIPortMenuButton.SetActive(isActive: false);
					}
					uIPortMenuButton.gameObject.SetActive(activeState);
				}
			}
		}

		private void OnPushMoveToMainMenu()
		{
			ChangeActiveMenuButtons(mUIPortMenuButtons_MainMenu, activeState: true);
			ChangeFocusableButtons(mUIPortMenuButtons_MainMenu);
			DOTween.Kill(this);
			Tween t = GenerateTweenCloseSubMenu();
			TweenCallback callback = delegate
			{
				ChangeFocusButton(mUIPortMenuButton_Sally);
				mUIPortMenuButton_Sally.ChangeState(UIPortMenuCenterButton.State.MainMenu);
			};
			Tween t2 = GenerateTweenOpenMainMenu();
			TweenCallback action = delegate
			{
				ChangeActiveMenuButtons(mUIPortMenuButtons_SubMenu, activeState: false);
				mStateManager.PopState();
				mStateManager.PushState(State.MainMenu);
			};
			Sequence sequence = DOTween.Sequence().SetId(this);
			sequence.Append(t);
			sequence.AppendCallback(callback);
			sequence.Append(t2);
			sequence.OnComplete(action);
		}

		private void OnPushMoveToSubMenu()
		{
			ChangeActiveMenuButtons(mUIPortMenuButtons_SubMenu, activeState: true);
			ChangeFocusableButtons(mUIPortMenuButtons_SubMenu);
			DOTween.Kill(this);
			Tween t = GenerateTweenCloseMain();
			TweenCallback callback = delegate
			{
				ChangeFocusButton(mUIPortMenuButton_Sally);
				mUIPortMenuButton_Sally.ChangeState(UIPortMenuCenterButton.State.SubMenu);
			};
			Tween t2 = GenerateTweenOpenSubMenu();
			TweenCallback action = delegate
			{
				ChangeActiveMenuButtons(mUIPortMenuButtons_MainMenu, activeState: false);
				mStateManager.PopState();
				mStateManager.PushState(State.SubMenu);
			};
			Sequence sequence = DOTween.Sequence().SetId(this);
			sequence.Append(t);
			sequence.AppendCallback(callback);
			sequence.Append(t2);
			sequence.OnComplete(action);
		}

		private void ChangeFocusButton(UIPortMenuButton targetButton)
		{
			if (mUIPortMenuButton_Current != null)
			{
				DOTween.Kill(mUIPortMenuButton_Current);
				Sequence s = DOTween.Sequence().SetId(mUIPortMenuButton_Current).SetId(mUIPortMenuButton_Current);
				Tween t = mUIPortMenuButton_Current.GenerateTweenRemoveHover().SetId(mUIPortMenuButton_Current).SetId(mUIPortMenuButton_Current);
				Tween t2 = mUIPortMenuButton_Current.GenerateTweenRemoveFocus().SetId(mUIPortMenuButton_Current).SetId(mUIPortMenuButton_Current);
				s.Join(t);
				s.Join(t2);
				mUIPortMenuButton_Current.RemoveHover();
			}
			mUIPortMenuButton_Current = targetButton;
			if (mUIPortMenuButton_Current != null)
			{
				DOTween.Kill(mUIPortMenuButton_Current);
				Sequence s2 = DOTween.Sequence().SetId(mUIPortMenuButton_Current).SetId(mUIPortMenuButton_Current);
				Tween t3 = mUIPortMenuButton_Current.GenerateTweenHoverScale().SetId(mUIPortMenuButton_Current);
				Tween t4 = mUIPortMenuButton_Current.GenerateTweenFocus().SetId(mUIPortMenuButton_Current);
				s2.Join(t3);
				s2.Join(t4);
				SoundUtils.PlaySE(mAudioClip_MainMenuOnMouse);
				mUIPortMenuButton_Current.Hover();
			}
		}

		private void ChangeFocusableButtons(UIPortMenuButton[] target)
		{
			mUIPortMenuButtons_Current = target;
		}

		public override string ToString()
		{
			if (mStateManager != null)
			{
				return mStateManager.ToString();
			}
			return base.ToString();
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_MainMenuOnMouse);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Shaft);
			UserInterfacePortManager.ReleaseUtils.Release(ref mPanelThis);
			mButtonManager = null;
			mUIPortMenuButton_Sally = null;
			mUIPortMenuButton_Engage = null;
			mUIPortMenuButton_Organize = null;
			mUIPortMenuButton_Remodel = null;
			mUIPortMenuButton_Arsenal = null;
			mUIPortMenuButton_Duty = null;
			mUIPortMenuButton_Repair = null;
			mUIPortMenuButton_Supply = null;
			mUIPortMenuButton_Record = null;
			mUIPortMenuButton_Album = null;
			mUIPortMenuButton_Item = null;
			mUIPortMenuButton_Option = null;
			mUIPortMenuButton_Interior = null;
			mUIPortMenuButton_Save = null;
			mUIPortMenuAnimation = null;
			mUIPortMenuButtons_MainMenu = null;
			mUIPortMenuButtons_SubMenu = null;
			mUIPortMenuButtons_Current = null;
			mUIPortMenuButton_Current = null;
			mStateManager = null;
			mKeyController = null;
			mPortManager = null;
		}

		public string StateToString()
		{
			return mStateManager.ToString();
		}
	}
}
