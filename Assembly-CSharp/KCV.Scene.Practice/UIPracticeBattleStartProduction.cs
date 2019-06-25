using DG.Tweening;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIPanel))]
	public class UIPracticeBattleStartProduction : MonoBehaviour
	{
		private UIPanel mPanelThis;

		[SerializeField]
		private UIPanel mPanel_HideCover;

		[SerializeField]
		private Transform mTransform_FriendFlagShipFrame;

		[SerializeField]
		private Transform mTransform_TargetFlagShipFrame;

		[SerializeField]
		private UITexture mTexture_FriendFlagShip;

		[SerializeField]
		private UITexture mTexture_TargetFlagShip;

		[SerializeField]
		private UIPracticeBattleDeckInShip[] mUIPracticeBattleDeckInShips_Friend;

		[SerializeField]
		private UIPracticeBattleDeckInShip[] mUIPracticeBattleDeckInShips_Enemy;

		[SerializeField]
		private Transform mTransform_UIPracticeShortCutSwitch;

		[SerializeField]
		private Vector3 mVector3_FriendShipShowPosition;

		[SerializeField]
		private Vector3 mVector3_TargetShipShowPosition;

		private DeckModel mFriendDeck;

		private DeckModel mTargetDeck;

		private bool mIsShortCutPlayMode;

		private KeyControl mKeyController;

		private Action<bool> mOnAnimationFinished;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mPanelThis.alpha = 0f;
			mPanel_HideCover.alpha = 1E-08f;
		}

		public void Initialize(DeckModel friend, DeckModel target)
		{
			mFriendDeck = friend;
			mTargetDeck = target;
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		private void Update()
		{
			if (mKeyController != null && mKeyController.IsLDown())
			{
				mIsShortCutPlayMode = !mIsShortCutPlayMode;
				if (mIsShortCutPlayMode)
				{
					ShowShortCutStateView();
				}
				else
				{
					HideShortCutStateView();
				}
			}
		}

		private void ShowShortCutStateView()
		{
			mTransform_UIPracticeShortCutSwitch.DOKill();
			mTransform_UIPracticeShortCutSwitch.DOLocalMoveX(-325f, 0.3f);
		}

		private void HideShortCutStateView()
		{
			mTransform_UIPracticeShortCutSwitch.DOKill();
			mTransform_UIPracticeShortCutSwitch.DOLocalMoveX(-600f, 0.3f);
		}

		public void Play()
		{
			StartCoroutine(PlayCoroutine());
		}

		private IEnumerator PlayCoroutine()
		{
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Reset();
			stopWatch.Start();
			ShipModel friendFlagShipModel = mFriendDeck.GetFlagShip();
			mTexture_FriendFlagShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(friendFlagShipModel.GetGraphicsMstId(), (!friendFlagShipModel.IsDamaged()) ? 9 : 10);
			mTexture_FriendFlagShip.MakePixelPerfect();
			mTexture_FriendFlagShip.transform.localPosition = Util.Poi2Vec(friendFlagShipModel.Offsets.GetFace(friendFlagShipModel.IsDamaged()));
			ShipModel targetFlagShipModel = mTargetDeck.GetFlagShip();
			mTexture_TargetFlagShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(targetFlagShipModel.GetGraphicsMstId(), (!targetFlagShipModel.IsDamaged()) ? 9 : 10);
			mTexture_TargetFlagShip.MakePixelPerfect();
			mTexture_TargetFlagShip.transform.localPosition = Util.Poi2Vec(targetFlagShipModel.Offsets.GetFace(targetFlagShipModel.IsDamaged()));
			InitializeFriendDeckInShips(mFriendDeck);
			InitializeTargetDeckInShips(mTargetDeck);
			yield return new WaitForEndOfFrame();
			stopWatch.Stop();
			for (int frame = 0; frame < stopWatch.Elapsed.Milliseconds / 60; frame++)
			{
				yield return new WaitForEndOfFrame();
			}
			yield return new WaitForEndOfFrame();
			mPanelThis.alpha = 1f;
			mTransform_FriendFlagShipFrame.localPositionY(-1024f);
			mTransform_TargetFlagShipFrame.localPositionY(1024f);
			Sequence tweenDeckInShipViewsSequence = DOTween.Sequence().SetId(this);
			tweenDeckInShipViewsSequence.Join(mTransform_FriendFlagShipFrame.DOLocalMove(mVector3_FriendShipShowPosition, 0.3f).SetId(this));
			tweenDeckInShipViewsSequence.Join(mTransform_TargetFlagShipFrame.DOLocalMove(mVector3_TargetShipShowPosition, 0.3f).SetId(this));
			int shipCount = (mTargetDeck.Count > mFriendDeck.Count) ? mTargetDeck.Count : mFriendDeck.Count;
			for (int index = 0; index < shipCount; index++)
			{
				Sequence tweenObject = DOTween.Sequence().SetId(this);
				if (index < mFriendDeck.Count)
				{
					mUIPracticeBattleDeckInShips_Friend[index].transform.localPositionX(-500f);
					tweenObject.Join(mUIPracticeBattleDeckInShips_Friend[index].transform.DOLocalMoveX(0f, 0.5f).SetId(this));
				}
				if (index < mTargetDeck.Count)
				{
					mUIPracticeBattleDeckInShips_Enemy[index].transform.localPositionX(500f);
					tweenObject.Join(mUIPracticeBattleDeckInShips_Enemy[index].transform.DOLocalMoveX(0f, 0.5f).SetId(this));
				}
				tweenObject.SetDelay(0.25f);
				tweenDeckInShipViewsSequence.Join(tweenObject);
			}
			Sequence tweenCrashFlagShipsTween = DOTween.Sequence().SetId(this);
			Sequence s = tweenCrashFlagShipsTween;
			Transform target = mTransform_FriendFlagShipFrame;
			Vector3 localPosition = mTransform_FriendFlagShipFrame.localPosition;
			s.Join(target.DOLocalMoveX(localPosition.x + 100f, 0.15f).SetLoops(2, LoopType.Yoyo).SetId(this));
			Sequence s2 = tweenCrashFlagShipsTween;
			Transform target2 = mTransform_TargetFlagShipFrame;
			Vector3 localPosition2 = mTransform_TargetFlagShipFrame.localPosition;
			s2.Join(target2.DOLocalMoveX(localPosition2.x - 100f, 0.15f).SetLoops(2, LoopType.Yoyo).SetId(this));
			float friendHideLocalPositionX = -960 - mTexture_FriendFlagShip.width / 2;
			float targetHideLocalPositionX = 960 + mTexture_TargetFlagShip.width / 2;
			Sequence tweenCrashFlagShipOutTween = DOTween.Sequence().SetId(this);
			tweenCrashFlagShipOutTween.Join(mTransform_FriendFlagShipFrame.DOLocalMoveX(friendHideLocalPositionX, 1f).SetId(this)).SetEase(Ease.InQuart);
			tweenCrashFlagShipOutTween.Join(mTransform_TargetFlagShipFrame.DOLocalMoveX(targetHideLocalPositionX, 1f).SetId(this)).SetEase(Ease.InQuart);
			Sequence playAllSequence = DOTween.Sequence().SetId(this);
			playAllSequence.Append(tweenDeckInShipViewsSequence);
			playAllSequence.AppendInterval(1f);
			playAllSequence.Append(tweenCrashFlagShipsTween);
			playAllSequence.Append(tweenCrashFlagShipOutTween);
			playAllSequence.OnComplete(delegate
			{
				this.OnAnimationFinished();
			});
			yield return playAllSequence.WaitForCompletion();
			UnityEngine.Debug.Log("ProductionFinished:" + stopWatch.Elapsed.Milliseconds.ToString());
		}

		private void InitializeFriendDeckInShips(DeckModel deckModel)
		{
			InitializeDeckInShips(deckModel, mUIPracticeBattleDeckInShips_Friend);
		}

		private void InitializeTargetDeckInShips(DeckModel deckModel)
		{
			InitializeDeckInShips(deckModel, mUIPracticeBattleDeckInShips_Enemy);
		}

		private void InitializeDeckInShips(DeckModel deckModel, UIPracticeBattleDeckInShip[] deckInShipViews)
		{
			int num = 0;
			int count = deckModel.Count;
			foreach (UIPracticeBattleDeckInShip uIPracticeBattleDeckInShip in deckInShipViews)
			{
				if (num < count)
				{
					uIPracticeBattleDeckInShip.transform.SetActive(isActive: true);
					uIPracticeBattleDeckInShip.Initialize(deckModel.GetShip(num));
				}
				else
				{
					uIPracticeBattleDeckInShip.transform.SetActive(isActive: false);
				}
				num++;
			}
		}

		public void SetOnAnimationFinishedListener(Action<bool> onAnimationFinishedListener)
		{
			mOnAnimationFinished = onAnimationFinishedListener;
		}

		private void OnAnimationFinished()
		{
			if (mOnAnimationFinished != null)
			{
				mOnAnimationFinished(mIsShortCutPlayMode);
			}
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			mPanelThis = null;
			mTransform_FriendFlagShipFrame = null;
			mTransform_TargetFlagShipFrame = null;
			mTexture_FriendFlagShip = null;
			mTexture_TargetFlagShip = null;
			mUIPracticeBattleDeckInShips_Friend = null;
			mUIPracticeBattleDeckInShips_Enemy = null;
			mTransform_UIPracticeShortCutSwitch = null;
			mFriendDeck = null;
			mTargetDeck = null;
		}

		internal void ShowCover()
		{
			HideShortCutStateView();
			mPanel_HideCover.alpha = 1f;
		}
	}
}
