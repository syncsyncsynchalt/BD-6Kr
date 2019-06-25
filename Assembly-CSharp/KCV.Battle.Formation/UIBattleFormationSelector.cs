using Common.Enum;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Formation
{
	[RequireComponent(typeof(UIPanel))]
	public class UIBattleFormationSelector : MonoBehaviour
	{
		public delegate void UIBattleFormationSelectorAction(UIBattleFormationSelector calledObject, BattleFormationKinds1 selectedFormation);

		private const float MOVE_ANIMATION_TIME = 0.5f;

		[SerializeField]
		private UISprite[] mSprites_Formations;

		private UISprite mSpriteCurrentFormation;

		private UIPanel mPanelThis;

		private Vector3[] mVector3DefaultPosition;

		private Vector3[] mVector3DefaultScale;

		private HashSet<BattleFormationKinds1> mSelectableFormations;

		private UIBattleFormationSelectorAction mUIBattleFormationSelectorAction;

		private KeyControl mKeyController;

		public void SetOnUIBattleFormationSelectorAction(UIBattleFormationSelectorAction callBack)
		{
			mUIBattleFormationSelectorAction = callBack;
		}

		private void CallBackAction(UIBattleFormationSelector calledObject, BattleFormationKinds1 selectedFormation)
		{
			if (mUIBattleFormationSelectorAction != null)
			{
				mUIBattleFormationSelectorAction(this, selectedFormation);
			}
		}

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mPanelThis.alpha = 0.01f;
		}

		public void Initialize(DeckModel deck)
		{
			mSelectableFormations = DeckUtil.GetSelectableFormations(deck);
			mVector3DefaultPosition = new Vector3[mSprites_Formations.Length];
			mVector3DefaultScale = new Vector3[mSprites_Formations.Length];
			int num = 0;
			UISprite[] array = mSprites_Formations;
			foreach (UISprite uISprite in array)
			{
				uISprite.alpha = 0.01f;
				mVector3DefaultPosition[num] = uISprite.transform.localPosition;
				mVector3DefaultScale[num] = uISprite.transform.localScale;
				uISprite.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
				num++;
			}
			ChangeFocusIndex(0);
		}

		private void Update()
		{
			if (mKeyController != null)
			{
				if (mKeyController.keyState[14].down)
				{
					int currentFocusFormationIndex = Array.IndexOf(mSprites_Formations, mSpriteCurrentFormation);
					ChangeNextFormation(currentFocusFormationIndex);
				}
				else if (mKeyController.keyState[1].down)
				{
					SelectFormation();
				}
			}
		}

		private void SelectFormation()
		{
			StartCoroutine(SelectFormationCoroutine());
		}

		private IEnumerator SelectFormationCoroutine()
		{
			float animationTime = 0.3f;
			int currentIndex = Array.IndexOf(mSprites_Formations, mSpriteCurrentFormation);
			GameObject targetObject = mSprites_Formations[currentIndex].gameObject;
			targetObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
			Hashtable animationHash = GenerateSelectedScaleAnimationHash(new Vector3(1f, 1f, 1f), animationTime);
			iTween.ScaleTo(targetObject, animationHash);
			yield return new WaitForSeconds(animationTime);
			CallBackAction(this, (BattleFormationKinds1)(currentIndex + 1));
		}

		public KeyControl GetKeyController()
		{
			mKeyController = new KeyControl();
			return mKeyController;
		}

		public void RemoveKeyController()
		{
			mKeyController = null;
		}

		public void Show(Action shownCallBack)
		{
			mPanelThis.alpha = 1f;
			float duration = 1f;
			StartCoroutine(ShowCoroutine(shownCallBack, duration));
		}

		private IEnumerator ShowCoroutine(Action shownCallBack, float duration)
		{
			int formationPosition = 0;
			UISprite[] array = mSprites_Formations;
			foreach (UISprite formationSprite in array)
			{
				formationSprite.alpha = 1f;
				iTween.ScaleTo(args: GenerateShowScaleAnimationHash(mVector3DefaultScale[formationPosition], duration), target: formationSprite.gameObject);
				formationPosition++;
			}
			yield return new WaitForSeconds(duration);
			shownCallBack?.Invoke();
		}

		public void Hide(Action hiddenCallBack)
		{
			int num = 0;
			UISprite[] array = mSprites_Formations;
			foreach (UISprite uISprite in array)
			{
				TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(uISprite.gameObject, 0.5f);
				tweenAlpha.from = uISprite.alpha;
				tweenAlpha.to = 0.01f;
				tweenAlpha.PlayForward();
				if (num == mSprites_Formations.Length - 1)
				{
					tweenAlpha.SetOnFinished(delegate
					{
						if (hiddenCallBack != null)
						{
							hiddenCallBack();
						}
					});
				}
				num++;
			}
		}

		private void ChangeNextFormation(int currentFocusFormationIndex)
		{
			int num = mSprites_Formations.Length;
			int loopIndex = GetLoopIndex(num, currentFocusFormationIndex + 1);
			int loopIndex2 = GetLoopIndex(num, num - 1 - currentFocusFormationIndex);
			ChangeFocusIndex(loopIndex);
			for (int i = 0; i < num; i++)
			{
				int loopIndex3 = GetLoopIndex(num, loopIndex2 + i);
				Hashtable args = GenerateMoveAnimationHash(mVector3DefaultPosition[loopIndex3]);
				Hashtable args2 = GenerateScaleAnimationHash(mVector3DefaultScale[loopIndex3]);
				iTween.MoveTo(mSprites_Formations[i].gameObject, args);
				iTween.ScaleTo(mSprites_Formations[i].gameObject, args2);
			}
		}

		private void ChangePrevFormation(int currentFocusFormationIndex)
		{
			int num = mSprites_Formations.Length;
			int loopIndex = GetLoopIndex(num, currentFocusFormationIndex + 1);
			int loopIndex2 = GetLoopIndex(num, num - 1 - currentFocusFormationIndex);
			ChangeFocusIndex(loopIndex);
			for (int i = 0; i < num; i++)
			{
				int loopIndex3 = GetLoopIndex(num, loopIndex2 + i);
				Hashtable args = GenerateMoveAnimationHash(mVector3DefaultPosition[loopIndex3]);
				Hashtable args2 = GenerateScaleAnimationHash(mVector3DefaultScale[loopIndex3]);
				iTween.MoveTo(mSprites_Formations[i].gameObject, args);
				iTween.ScaleTo(mSprites_Formations[i].gameObject, args2);
			}
		}

		private int GetLoopIndex(int arrayLength, int value)
		{
			if (value == 0)
			{
				return 0;
			}
			if (0 < value)
			{
				return value % arrayLength;
			}
			return arrayLength + value % arrayLength;
		}

		private void ChangeFocusIndex(int index)
		{
			mSpriteCurrentFormation = mSprites_Formations[index];
		}

		private Hashtable GenerateMoveAnimationHash(Vector3 toPosition)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", toPosition.x);
			hashtable.Add("y", toPosition.y);
			hashtable.Add("time", 0.5f);
			hashtable.Add("isLocal", true);
			hashtable.Add("ignoreTimeScale", false);
			hashtable.Add("easetype", iTween.EaseType.easeInOutExpo);
			return hashtable;
		}

		private Hashtable GenerateScaleAnimationHash(Vector3 toScale)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", toScale.x);
			hashtable.Add("y", toScale.y);
			hashtable.Add("isLocal", true);
			hashtable.Add("time", 0.5f);
			hashtable.Add("easetype", iTween.EaseType.easeInBack);
			return hashtable;
		}

		private Hashtable GenerateShowScaleAnimationHash(Vector3 toScale, float duration)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", toScale.x);
			hashtable.Add("y", toScale.y);
			hashtable.Add("isLocal", true);
			hashtable.Add("time", duration);
			hashtable.Add("easetype", iTween.EaseType.easeOutElastic);
			hashtable.Add("ignoretimescale", false);
			return hashtable;
		}

		private Hashtable GenerateSelectedScaleAnimationHash(Vector3 toScale, float duration)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", toScale.x);
			hashtable.Add("y", toScale.y);
			hashtable.Add("isLocal", true);
			hashtable.Add("time", duration);
			hashtable.Add("easetype", iTween.EaseType.easeInOutElastic);
			hashtable.Add("ignoretimescale", false);
			return hashtable;
		}
	}
}
