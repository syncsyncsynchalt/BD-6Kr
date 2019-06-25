using Common.Enum;
using DG.Tweening;
using KCV.Scene.Port;
using System;
using UnityEngine;

public class UIMissionJudgeCutIn : MonoBehaviour
{
	[SerializeField]
	private UITexture mTexture_Background;

	[SerializeField]
	private UITexture mTexture_Text;

	[SerializeField]
	private Transform mTransform_Result;

	[SerializeField]
	private UITexture mTexture_Result;

	private Action mOnFinishedAnimationListener;

	private MissionResultKinds mMissionResultKind;

	private void Awake()
	{
		Reset();
	}

	public void Reset()
	{
		mTexture_Text.alpha = 0.001f;
		mTexture_Text.transform.localPosition = new Vector3(600f, 0f);
		mTransform_Result.localPositionX(284f);
		mTexture_Result.transform.localScale = new Vector3(1.2f, 1.2f);
		mTexture_Result.alpha = 0.001f;
		mTexture_Background.transform.localScale = new Vector3(1f, 0f, 1f);
	}

	public void Initialize(MissionResultKinds missionResultKind)
	{
		mMissionResultKind = missionResultKind;
		Vector3 zero = Vector3.zero;
		int num;
		switch (missionResultKind)
		{
		case MissionResultKinds.FAILE:
			num = 3;
			mTexture_Result.SetDimensions(276, 145);
			break;
		case MissionResultKinds.SUCCESS:
			num = 2;
			mTexture_Result.SetDimensions(276, 145);
			break;
		case MissionResultKinds.GREAT:
			num = 5;
			mTexture_Result.SetDimensions(377, 146);
			break;
		default:
			num = 1;
			break;
		}
		string path = $"Textures/Mission/operation_judge_txt_0{num}";
		mTexture_Result.mainTexture = Resources.Load<Texture2D>(path);
	}

	public void SetOnFinishedAnimationListener(Action action)
	{
		mOnFinishedAnimationListener = action;
	}

	public void Play()
	{
		base.transform.DOKill();
		Sequence sequence = DOTween.Sequence();
		Sequence sequence2 = DOTween.Sequence();
		mTexture_Background.transform.DOKill();
		sequence2.Append(mTexture_Background.transform.DOScaleY(1f, 0.8f).OnPlay(delegate
		{
			mTexture_Background.alpha = 1f;
		}));
		Sequence sequence3 = DOTween.Sequence();
		mTexture_Text.transform.DOKill();
		sequence3.Join(mTexture_Text.transform.DOLocalMoveX(-208f, 0.8f).SetEase(Ease.OutCirc).OnPlay(delegate
		{
			mTexture_Text.alpha = 1f;
		}));
		mTexture_Result.transform.DOKill();
		sequence3.Join(mTexture_Result.transform.DOScale(Vector3.one, 0.5f).SetDelay(0.9f).SetEase(Ease.OutBounce)
			.OnPlay(delegate
			{
				mTexture_Result.alpha = 1f;
			}));
		sequence.Append(sequence2);
		sequence.Append(sequence3);
		sequence.OnComplete(delegate
		{
			if (mOnFinishedAnimationListener != null)
			{
				mOnFinishedAnimationListener();
			}
		});
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Background);
		UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Text);
		UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Result);
		mTransform_Result = null;
		mOnFinishedAnimationListener = null;
	}
}
