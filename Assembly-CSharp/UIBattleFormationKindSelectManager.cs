using AnimationOrTween;
using Common.Enum;
using DG.Tweening;
using KCV;
using KCV.Battle.Formation;
using KCV.Display;
using KCV.Interface;
using System;
using UnityEngine;

public class UIBattleFormationKindSelectManager : MonoBehaviour, IUIKeyControllable
{
	public enum ActionType
	{
		Select,
		SelectAuto
	}

	[Serializable]
	private class Arrow
	{
		[SerializeField]
		public UITexture mTexture_ArrowFront;

		[SerializeField]
		public UITexture mTexture_ArrowBack;
	}

	public delegate void UIBattleFormationKindSelectManagerAction(ActionType actionType, UIBattleFormationKindSelectManager calledObject, UIBattleFormationKind centerView);

	[SerializeField]
	private Arrow mArrow_Left;

	[SerializeField]
	private Arrow mArrow_Right;

	[SerializeField]
	private UIBattleFormationKindSelector mUIBattleFormationKindSelector;

	[SerializeField]
	private UIDisplaySwipeEventRegion mUIDisplaySwipeEventRegion_FormationChange;

	[SerializeField]
	private UILabel mLabel_Formation_Center;

	[SerializeField]
	private UILabel mLabel_Formation_Right;

	[SerializeField]
	private UILabel mLabel_Formation_Left;

	[SerializeField]
	private UILabel mLabel_Formation_Temp;

	[SerializeField]
	private UIWidget mWidget_FormationSelecteNames;

	private UILabel mLabelCenter;

	private UILabel mLabelRight;

	private UILabel mLabelLeft;

	private UILabel mLabelTemp;

	private Vector3 mVector3_DefaultPositionCenter;

	private Vector3 mVector3_DefaultPositionRight;

	private Vector3 mVector3_DefaultPositionLeft;

	private Vector3 mVector3_DefaultPositionTemp;

	private Vector3 mVector3_DefaultScaleCenter;

	private Vector3 mVector3_DefaultScaleRight;

	private Vector3 mVector3_DefaultScaleLeft;

	private Vector3 mVector3_DefaultScaleTemp;

	private float mDefaultCenterAlpha;

	private float mDefaultLeftAlpha;

	private float mDefaultRightAlpha;

	private float mDefaultTempAlpha;

	private UIBattleFormationKindSelectManagerAction mUIBattleFormationKindSelectManagerAction;

	private bool mManualUpdate;

	[SerializeField]
	private float moveDuration = 0.3f;

	[SerializeField]
	private float arrowDuration = 0.3f;

	[SerializeField]
	private float arrowAlpha = 0.6f;

	private void Start()
	{
		BattleFormationKinds1[] obj = new BattleFormationKinds1[4]
		{
			BattleFormationKinds1.FukuJuu,
			BattleFormationKinds1.Rinkei,
			BattleFormationKinds1.TanJuu,
			BattleFormationKinds1.TanOu
		};
	}

	private void Awake()
	{
		mUIBattleFormationKindSelector.SetActive(isActive: false);
		mLabelCenter = mLabel_Formation_Center;
		mLabelRight = mLabel_Formation_Right;
		mLabelLeft = mLabel_Formation_Left;
		mLabelTemp = mLabel_Formation_Temp;
		mVector3_DefaultPositionCenter = mLabelCenter.transform.localPosition;
		mVector3_DefaultPositionRight = mLabelRight.transform.localPosition;
		mVector3_DefaultPositionLeft = mLabelLeft.transform.localPosition;
		mVector3_DefaultPositionTemp = mLabelTemp.transform.localPosition;
		mVector3_DefaultScaleCenter = mLabelCenter.transform.localScale;
		mVector3_DefaultScaleRight = mLabelRight.transform.localScale;
		mVector3_DefaultScaleLeft = mLabelLeft.transform.localScale;
		mVector3_DefaultScaleTemp = mLabelTemp.transform.localScale;
		mDefaultCenterAlpha = mLabelCenter.alpha;
		mDefaultRightAlpha = mLabelRight.alpha;
		mDefaultLeftAlpha = mLabelLeft.alpha;
		mDefaultTempAlpha = mLabelTemp.alpha;
	}

	private void Update()
	{
		if (mUIBattleFormationKindSelector != null && mManualUpdate)
		{
			mUIBattleFormationKindSelector.OnUpdatedKeyController();
		}
	}

	public void Initialize(Camera eventCamera, BattleFormationKinds1[] formations)
	{
		Initialize(eventCamera, formations, manualUpdate: false);
	}

	public void Initialize(Camera eventCamera, BattleFormationKinds1[] formations, bool manualUpdate)
	{
		mWidget_FormationSelecteNames.alpha = 1f;
		mManualUpdate = manualUpdate;
		mLabelCenter.text = App.GetFormationText(formations[0]);
		mLabelRight.text = App.GetFormationText(formations[1]);
		mLabelLeft.text = App.GetFormationText(formations[formations.Length - 1]);
		mLabelTemp.text = string.Empty;
		mUIBattleFormationKindSelector.SetActive(isActive: true);
		mUIBattleFormationKindSelector.SetOnUIBattleFormationKindSelectorAction(delegate(UIBattleFormationKindSelector.ActionType actionType, UIBattleFormationKind centerView)
		{
			UIBattleFormationKind[] views = mUIBattleFormationKindSelector.GetViews();
			switch (actionType)
			{
			case UIBattleFormationKindSelector.ActionType.Select:
			{
				UIBattleFormationKind[] array = views;
				foreach (UIBattleFormationKind uIBattleFormationKind in array)
				{
					if (!(centerView == uIBattleFormationKind))
					{
						uIBattleFormationKind.Hide();
					}
				}
				mArrow_Left.mTexture_ArrowBack.alpha = 0f;
				mArrow_Left.mTexture_ArrowFront.alpha = 0f;
				mArrow_Right.mTexture_ArrowBack.alpha = 0f;
				mArrow_Right.mTexture_ArrowFront.alpha = 0f;
				mLabelRight.text = string.Empty;
				mLabelLeft.text = string.Empty;
				mLabelTemp.text = string.Empty;
				SetKeyController(null);
				mUIDisplaySwipeEventRegion_FormationChange.enabled = false;
				CallBack(ActionType.Select, this, centerView);
				break;
			}
			case UIBattleFormationKindSelector.ActionType.SelectAuto:
				CallBack(ActionType.SelectAuto, this, null);
				break;
			case UIBattleFormationKindSelector.ActionType.OnNextChanged:
			{
				UILabel uILabel = mLabelTemp;
				UILabel from = mLabelLeft;
				UILabel from2 = mLabelCenter;
				UILabel from3 = mLabelRight;
				int num = Array.IndexOf(formations, centerView.Category);
				int num2 = (int)Util.LoopValue(num + 1, 0f, formations.Length - 1);
				uILabel.text = App.GetFormationText(formations[num2]);
				if (DOTween.IsTweening(this))
				{
					DOTween.Kill(this, complete: true);
				}
				Sequence sequence2 = DOTween.Sequence().SetId(this);
				Tween t5 = GenerateTweenMove(from2, mDefaultLeftAlpha, mVector3_DefaultPositionLeft, mVector3_DefaultScaleLeft, moveDuration);
				Tween t6 = GenerateTweenMove(from, mDefaultTempAlpha, mVector3_DefaultPositionTemp, mVector3_DefaultScaleTemp, moveDuration);
				Tween t7 = GenerateTweenMove(uILabel, mDefaultRightAlpha, mVector3_DefaultPositionRight, mVector3_DefaultScaleRight, moveDuration);
				Tween t8 = GenerateTweenMove(from3, mDefaultCenterAlpha, mVector3_DefaultPositionCenter, mVector3_DefaultScaleCenter, moveDuration);
				TweenCallback action2 = delegate
				{
				};
				sequence2.Append(t5);
				sequence2.Join(t6);
				sequence2.Join(t7);
				sequence2.Join(t8);
				sequence2.OnComplete(action2);
				mLabelCenter = from3;
				mLabelLeft = from2;
				mLabelTemp = from;
				mLabelRight = uILabel;
				mLabelCenter.depth = 2;
				mLabelLeft.depth = 1;
				mLabelRight.depth = 1;
				mLabelTemp.depth = 0;
				GenerateTweenArrow(mArrow_Right, Direction.Reverse);
				break;
			}
			case UIBattleFormationKindSelector.ActionType.OnPrevChanged:
			{
				UILabel uILabel = mLabelTemp;
				UILabel from = mLabelLeft;
				UILabel from2 = mLabelCenter;
				UILabel from3 = mLabelRight;
				int num = Array.IndexOf(formations, centerView.Category);
				int num2 = (int)Util.LoopValue(num - 1, 0f, formations.Length - 1);
				uILabel.text = App.GetFormationText(formations[num2]);
				if (DOTween.IsTweening(this))
				{
					DOTween.Kill(this, complete: true);
				}
				Sequence sequence = DOTween.Sequence().SetId(this);
				Tween t = GenerateTweenMove(from2, mDefaultRightAlpha, mVector3_DefaultPositionRight, mVector3_DefaultScaleRight, moveDuration);
				Tween t2 = GenerateTweenMove(from3, mDefaultTempAlpha, mVector3_DefaultPositionTemp, mVector3_DefaultScaleTemp, moveDuration);
				Tween t3 = GenerateTweenMove(uILabel, mDefaultLeftAlpha, mVector3_DefaultPositionLeft, mVector3_DefaultScaleLeft, moveDuration);
				Tween t4 = GenerateTweenMove(from, mDefaultCenterAlpha, mVector3_DefaultPositionCenter, mVector3_DefaultScaleCenter, moveDuration);
				TweenCallback action = delegate
				{
				};
				sequence.Append(t);
				sequence.Join(t2);
				sequence.Join(t3);
				sequence.Join(t4);
				sequence.OnComplete(action);
				mLabelRight = from2;
				mLabelTemp = from3;
				mLabelLeft = uILabel;
				mLabelCenter = from;
				mLabelCenter.depth = 2;
				mLabelLeft.depth = 1;
				mLabelRight.depth = 1;
				mLabelTemp.depth = 0;
				GenerateTweenArrow(mArrow_Left, Direction.Forward);
				break;
			}
			}
		});
		mUIBattleFormationKindSelector.Initialize(formations);
		mUIBattleFormationKindSelector.Show();
		mUIDisplaySwipeEventRegion_FormationChange.SetEventCatchCamera(eventCamera);
		mUIDisplaySwipeEventRegion_FormationChange.SetOnSwipeActionJudgeCallBack(OnSwipeEvent);
	}

	private Tween GenerateTweenArrow(Arrow arrow, Direction direction)
	{
		int num = 0;
		UITexture arrowBack = UnityEngine.Object.Instantiate(arrow.mTexture_ArrowBack);
		arrowBack.transform.parent = arrow.mTexture_ArrowBack.transform.parent;
		arrowBack.transform.localScale = arrow.mTexture_ArrowBack.transform.localScale;
		arrowBack.transform.localPosition = arrow.mTexture_ArrowBack.transform.localPosition;
		UITexture arrowFront = UnityEngine.Object.Instantiate(arrow.mTexture_ArrowFront);
		arrowFront.transform.parent = arrow.mTexture_ArrowFront.transform.parent;
		arrowFront.transform.localScale = arrow.mTexture_ArrowFront.transform.localScale;
		arrowFront.transform.localPosition = arrow.mTexture_ArrowFront.transform.localPosition;
		switch (direction)
		{
		case Direction.Forward:
			num = -20;
			break;
		case Direction.Reverse:
			num = 20;
			break;
		}
		Sequence sequence = DOTween.Sequence();
		Transform transform = arrowBack.transform;
		Vector3 localPosition = arrowBack.transform.localPosition;
		Tween t = transform.DOLocalMoveX(localPosition.x + (float)num, arrowDuration);
		Transform transform2 = arrowFront.transform;
		Vector3 localPosition2 = arrowFront.transform.localPosition;
		Tween t2 = transform2.DOLocalMoveX(localPosition2.x + (float)num, arrowDuration);
		Tween t3 = DOVirtual.Float(arrowBack.alpha, 0f, arrowAlpha, delegate(float alpha)
		{
			arrowBack.alpha = alpha;
			arrowFront.alpha = alpha;
		});
		TweenCallback action = delegate
		{
			UnityEngine.Object.Destroy(arrowBack.gameObject);
			UnityEngine.Object.Destroy(arrowFront.gameObject);
		};
		sequence.Append(t);
		sequence.Join(t2);
		sequence.Join(t3);
		sequence.OnComplete(action);
		return sequence;
	}

	private Tween GenerateTweenMove(UILabel from, float toAlpha, Vector3 toPosition, Vector3 toScale, float duration)
	{
		Sequence sequence = DOTween.Sequence();
		Tween t = from.transform.DOLocalMoveX(toPosition.x, duration);
		Tween t2 = from.transform.DOScale(toScale, duration);
		Tween t3 = DOVirtual.Float(from.alpha, toAlpha, duration, delegate(float alpha)
		{
			from.alpha = alpha;
		});
		sequence.Append(t);
		sequence.Join(t2);
		sequence.Join(t3);
		return sequence;
	}

	public void SetOnUIBattleFormationKindSelectManagerAction(UIBattleFormationKindSelectManagerAction actionMethod)
	{
		mUIBattleFormationKindSelectManagerAction = actionMethod;
	}

	private void OnSwipeEvent(UIDisplaySwipeEventRegion.ActionType actionType, float deltaX, float deltaY, float movedPercentageX, float movedPercentageY, float elapsedTime)
	{
		if (actionType != UIDisplaySwipeEventRegion.ActionType.FingerUp)
		{
			return;
		}
		float num = 0.2f;
		if (num < Math.Abs(movedPercentageX))
		{
			if (0f < movedPercentageX)
			{
				mUIBattleFormationKindSelector.Prev();
			}
			else
			{
				mUIBattleFormationKindSelector.Next();
			}
		}
	}

	private void CallBack(ActionType actionType, UIBattleFormationKindSelectManager calledObject, UIBattleFormationKind centerView)
	{
		if (mUIBattleFormationKindSelectManagerAction != null)
		{
			mUIBattleFormationKindSelectManagerAction(actionType, this, centerView);
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		if (mUIBattleFormationKindSelector != null)
		{
			mUIBattleFormationKindSelector.SetKeyController(keyController, mUIDisplaySwipeEventRegion_FormationChange);
		}
	}

	public void SetKeyController(KeyControl keyController, UIDisplaySwipeEventRegion region)
	{
		if (mUIBattleFormationKindSelector != null)
		{
			mUIBattleFormationKindSelector.SetKeyController(keyController, mUIDisplaySwipeEventRegion_FormationChange);
		}
	}

	public void OnUpdatedKeyController()
	{
		if (mUIBattleFormationKindSelector != null)
		{
			mUIBattleFormationKindSelector.OnUpdatedKeyController();
		}
	}

	public void OnReleaseKeyController()
	{
		if (mUIBattleFormationKindSelector != null)
		{
			mUIBattleFormationKindSelector.OnReleaseKeyController();
		}
	}

	private void OnDestroy()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this);
		}
		mArrow_Left = null;
		mArrow_Right = null;
		mUIBattleFormationKindSelector = null;
		mUIDisplaySwipeEventRegion_FormationChange = null;
		mLabel_Formation_Center = null;
		mLabel_Formation_Right = null;
		mLabel_Formation_Left = null;
		mLabel_Formation_Temp = null;
		mWidget_FormationSelecteNames = null;
		mLabelCenter = null;
		mLabelRight = null;
		mLabelLeft = null;
		mLabelTemp = null;
		mUIBattleFormationKindSelectManagerAction = null;
	}
}
