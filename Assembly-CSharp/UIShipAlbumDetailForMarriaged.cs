using KCV;
using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIShipAlbumDetailForMarriaged : UIShipAlbumDetail
{
	[SerializeField]
	private Animation mAnimation_MarriagedRing;

	[SerializeField]
	private UIButton mButton_PlayMarriageMovie;

	private Action mOnRequestPlayMarriageMovieListener;

	protected override void OnSelectCircleButton()
	{
		if (mButton_Prev.Equals(mCurrentFocusButton))
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			PrevImage();
		}
		else if (mButton_Voice.Equals(mCurrentFocusButton))
		{
			PlayVoice();
		}
		else if (mButton_Next.Equals(mCurrentFocusButton))
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			NextImage();
		}
		else if (mButton_PlayMarriageMovie.Equals(mCurrentFocusButton))
		{
			RequestPlayMarriageMovie();
		}
	}

	public void SetOnRequestPlayMarriageMovieListener(Action onRequestPlayMarriageMovieListener)
	{
		mOnRequestPlayMarriageMovieListener = onRequestPlayMarriageMovieListener;
	}

	private void RequestPlayMarriageMovie()
	{
		if (mOnRequestPlayMarriageMovieListener != null)
		{
			mOnRequestPlayMarriageMovieListener();
		}
	}

	[Obsolete("Inspector上で設定して使用します")]
	public void OnTouchPlayMarriageMovie()
	{
		RequestPlayMarriageMovie();
	}

	protected override UIButton[] GetFocasableButtons()
	{
		List<UIButton> list = new List<UIButton>();
		list.Add(mButton_Prev);
		list.Add(mButton_Voice);
		list.Add(mButton_PlayMarriageMovie);
		list.Add(mButton_Next);
		return list.ToArray();
	}
}
