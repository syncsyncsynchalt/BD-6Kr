using Common.Enum;
using DG.Tweening;
using KCV.View.Scroll;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIInteriorFurnitureChangeScrollList : UIScrollListParent<UIInteriorFurnitureChangeScrollListChildModel, UIInteriorFurnitureChangeScrollListChild>
{
	private enum TweenAnimationType
	{
		ShowHide
	}

	[SerializeField]
	private UITexture mTexture_Header;

	[SerializeField]
	private UILabel mLabel_Genre;

	[SerializeField]
	private UITexture mTexture_TouchBackArea;

	private Dictionary<TweenAnimationType, UITweener> mTweeners;

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchBack()
	{
		base.OnAction(ActionType.OnBack, this, ViewFocus);
	}

	protected override void OnStart()
	{
		mTweeners = new Dictionary<TweenAnimationType, UITweener>();
	}

	public void Show()
	{
		if (DOTween.IsTweening(TweenAnimationType.ShowHide))
		{
			DOTween.Kill(TweenAnimationType.ShowHide);
		}
		DOVirtual.Float(mTexture_TouchBackArea.alpha, 0.5f, 0.15f, delegate(float alpha)
		{
			mTexture_TouchBackArea.alpha = alpha;
		}).SetId(TweenAnimationType.ShowHide).SetDelay(0.3f);
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.gameObject, 0.3f);
		tweenPosition.from = base.transform.localPosition;
		TweenPosition tweenPosition2 = tweenPosition;
		Vector3 localPosition = base.transform.localPosition;
		float y = localPosition.y;
		Vector3 localPosition2 = base.transform.localPosition;
		tweenPosition2.to = new Vector3(0f, y, localPosition2.z);
		tweenPosition.ignoreTimeScale = true;
	}

	public void Hide()
	{
		if (DOTween.IsTweening(TweenAnimationType.ShowHide))
		{
			DOTween.Kill(TweenAnimationType.ShowHide);
		}
		Sequence sequence = DOTween.Sequence();
		Tween t = DOVirtual.Float(mTexture_TouchBackArea.alpha, 1E-06f, 0.15f, delegate(float alpha)
		{
			mTexture_TouchBackArea.alpha = alpha;
		});
		Tween t2 = base.transform.DOLocalMoveX(-960f, 0.6f).SetEase(Ease.OutCirc);
		sequence.Append(t2);
		sequence.Join(t);
		sequence.SetId(TweenAnimationType.ShowHide);
	}

	public void Initialize(int deckId, FurnitureModel[] models)
	{
		UIInteriorFurnitureChangeScrollListChildModel[] models2 = GenerateChildrenDTOModel(deckId, models);
		base.Initialize(models2);
	}

	public void Refresh()
	{
		Refresh(Models);
	}

	private UIInteriorFurnitureChangeScrollListChildModel[] GenerateChildrenDTOModel(int deckId, FurnitureModel[] models)
	{
		List<UIInteriorFurnitureChangeScrollListChildModel> list = new List<UIInteriorFurnitureChangeScrollListChildModel>();
		foreach (FurnitureModel model in models)
		{
			UIInteriorFurnitureChangeScrollListChildModel item = new UIInteriorFurnitureChangeScrollListChildModel(deckId, model);
			list.Add(item);
		}
		return list.ToArray();
	}

	public void UpdateHeader(FurnitureKinds kinds)
	{
		switch (kinds)
		{
		case FurnitureKinds.Wall:
			mLabel_Genre.text = "壁紙";
			mTexture_Header.color = new Color(161f, 121f / 255f, 91f / 255f);
			break;
		case FurnitureKinds.Floor:
			mLabel_Genre.text = "床";
			mTexture_Header.color = new Color(56f / 85f, 36f / 85f, 0.4117647f);
			break;
		case FurnitureKinds.Desk:
			mLabel_Genre.text = "椅子＋机";
			mTexture_Header.color = new Color(134f / 255f, 39f / 85f, 148f / 255f);
			break;
		case FurnitureKinds.Window:
			mLabel_Genre.text = "窓枠＋カ\u30fcテン";
			mTexture_Header.color = new Color(20f / 51f, 152f / 255f, 0.5882353f);
			break;
		case FurnitureKinds.Hangings:
			mLabel_Genre.text = "装飾";
			mTexture_Header.color = new Color(0.470588237f, 0.7058824f, 26f / 51f);
			break;
		case FurnitureKinds.Chest:
			mLabel_Genre.text = "家具";
			mTexture_Header.color = new Color(0.5882353f, 0.5882353f, 20f / 51f);
			break;
		}
	}

	public void Release()
	{
		mTexture_Header.mainTexture = null;
		mTexture_Header = null;
		mLabel_Genre = null;
	}

	private void OnDestroy()
	{
		if (DOTween.IsTweening(TweenAnimationType.ShowHide))
		{
			DOTween.Kill(TweenAnimationType.ShowHide);
		}
	}
}
