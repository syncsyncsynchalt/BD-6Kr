using KCV.View.ScrollView;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.Furniture.JukeBox
{
	public class UIJukeBoxPlayListChild : MonoBehaviour, UIScrollListItem<Mst_bgm_jukebox, UIJukeBoxPlayListChild>
	{
		[SerializeField]
		private UITexture mTexture_Background;

		[SerializeField]
		private UILabel mLabel_Title;

		[SerializeField]
		private UILabel mLabel_Price;

		[SerializeField]
		private UILabel mLabel_Description;

		private Mst_bgm_jukebox mMst_bgm_jukebox;

		private Action<UIJukeBoxPlayListChild> mOnTouchListener;

		private Transform mTransform;

		private int mRealIndex;

		public void Initialize(int realIndex, Mst_bgm_jukebox model)
		{
			mRealIndex = realIndex;
			Initialize(model, model.Name, model.R_coins.ToString(), model.Remarks);
		}

		public void InitializeDefault(int realIndex)
		{
			mRealIndex = realIndex;
			Initialize(null, string.Empty, string.Empty, string.Empty);
		}

		[Obsolete("Inspector上で設定して使用します")]
		private void OnClick()
		{
			if (mOnTouchListener != null)
			{
				mOnTouchListener(this);
			}
		}

		private void Initialize(Mst_bgm_jukebox jukeBoxModel, string musicTitle, string price, string description)
		{
			mMst_bgm_jukebox = jukeBoxModel;
			mLabel_Title.text = musicTitle;
			mLabel_Price.text = price;
			mLabel_Description.text = description;
			mTexture_Background.alpha = 0.0001f;
		}

		public int GetRealIndex()
		{
			return mRealIndex;
		}

		public Mst_bgm_jukebox GetModel()
		{
			return mMst_bgm_jukebox;
		}

		public int GetHeight()
		{
			return 36;
		}

		public void SetOnTouchListener(Action<UIJukeBoxPlayListChild> onTouchListener)
		{
			mOnTouchListener = onTouchListener;
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(mTexture_Background.gameObject, value: true);
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(mTexture_Background.gameObject, value: false);
			mTexture_Background.alpha = 1E-05f;
		}

		public Transform GetTransform()
		{
			if (mTransform == null)
			{
				mTransform = base.transform;
			}
			return mTransform;
		}

		private void OnDestroy()
		{
			mTexture_Background = null;
			mLabel_Title = null;
			mLabel_Price = null;
			mLabel_Description = null;
			mMst_bgm_jukebox = null;
			mOnTouchListener = null;
			mTransform = null;
		}
	}
}
