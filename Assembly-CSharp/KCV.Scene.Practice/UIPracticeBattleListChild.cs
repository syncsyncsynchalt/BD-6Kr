using Common.Enum;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIWidget))]
	public class UIPracticeBattleListChild : MonoBehaviour
	{
		private UIWidget mWidgetThis;

		[SerializeField]
		private UIButton mButton;

		[SerializeField]
		private UITexture mTexture_FlagShip;

		[SerializeField]
		private UITexture mTexture_DeckNo;

		private DeckModel mDeckModel;

		private List<IsGoCondition> mConditions;

		private Action<UIPracticeBattleListChild> mOnClickListener;

		public float alpha
		{
			get
			{
				if (mWidgetThis != null)
				{
					return mWidgetThis.alpha;
				}
				return 1f;
			}
			set
			{
				if (mWidgetThis != null)
				{
					mWidgetThis.alpha = value;
				}
			}
		}

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
		}

		public void Initialize(DeckModel deckModel, List<IsGoCondition> conditions)
		{
			mDeckModel = deckModel;
			mConditions = conditions;
			mTexture_DeckNo.mainTexture = Resources.Load<Texture>($"Textures/Common/DeckFlag/icon_deck{deckModel.Id}");
			mTexture_FlagShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(deckModel.GetFlagShip().MstId, (!deckModel.GetFlagShip().IsDamaged()) ? 1 : 2);
			bool battleCondition = IsStartBattleCondition();
			InitializeCondition(battleCondition);
		}

		private void InitializeCondition(bool battleCondition)
		{
			if (battleCondition)
			{
				mTexture_FlagShip.color = new Color(1f, 1f, 1f);
				mButton.defaultColor = Color.white;
			}
			else
			{
				mTexture_FlagShip.color = new Color(0.5f, 0.5f, 0.5f);
				mButton.defaultColor = Color.gray;
			}
		}

		public DeckModel GetDeckModel()
		{
			return mDeckModel;
		}

		public List<IsGoCondition> GetConditions()
		{
			return mConditions;
		}

		public bool IsStartBattleCondition()
		{
			return 0 == mConditions.Count;
		}

		public void ParentHasChanged()
		{
			mWidgetThis.ParentHasChanged();
		}

		public void SetOnClickListener(Action<UIPracticeBattleListChild> onClickListener)
		{
			mOnClickListener = onClickListener;
		}

		public void OnClickChild()
		{
			ClickChild();
		}

		private void ClickChild()
		{
			if (mOnClickListener != null)
			{
				mOnClickListener(this);
			}
		}

		public void Hover()
		{
			mButton.SetState(UIButtonColor.State.Hover, immediate: true);
		}

		public void RemoveHover()
		{
			mButton.SetState(UIButtonColor.State.Normal, immediate: true);
		}

		private void OnDestroy()
		{
			mWidgetThis = null;
			mButton = null;
			mTexture_FlagShip = null;
			mTexture_DeckNo = null;
			mDeckModel = null;
			mConditions = null;
		}
	}
}
