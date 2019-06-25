using DG.Tweening;
using KCV.Utils;
using System;
using UnityEngine;

namespace KCV.Scene.Port
{
	[RequireComponent(typeof(UIWidget))]
	[RequireComponent(typeof(UIButton))]
	public abstract class UIPortMenuButton : MonoBehaviour, UIButtonManager.UIButtonManagement
	{
		public interface CompositeMenu
		{
			UIPortMenuButtonKeyMap GetSubMenuKeyMap();
		}

		[Serializable]
		public class UIPortMenuButtonKeyMap
		{
			[SerializeField]
			public UIPortMenuButton mUIPortMenuButton_Top;

			[SerializeField]
			public UIPortMenuButton mUIPortMenuButton_Down;

			[SerializeField]
			public UIPortMenuButton mUIPortMenuButton_Left;

			[SerializeField]
			public UIPortMenuButton mUIPortMenuButton_Right;

			public void Release()
			{
				mUIPortMenuButton_Top = null;
				mUIPortMenuButton_Down = null;
				mUIPortMenuButton_Left = null;
				mUIPortMenuButton_Right = null;
			}
		}

		[SerializeField]
		protected UIPortMenuButtonKeyMap mUIPortMenuButtonKeyMap;

		protected UIWidget mWidgetThis;

		protected UIButton mButton_Action;

		[SerializeField]
		protected UITexture mTexture_Base;

		[SerializeField]
		protected UITexture mTexture_Glow_Back;

		[SerializeField]
		protected UITexture mTexture_Glow_Front;

		[SerializeField]
		protected UITexture mTexture_Front;

		[SerializeField]
		protected UITexture mTexture_TextShadow;

		[SerializeField]
		protected UITexture mTexture_Text;

		protected Vector3 mVector3_FrontCoverOutScale = new Vector3(0.95f, 0.95f);

		protected Vector3 mVector3_DefaultFrontScale = Vector3.one;

		private Vector3 mVector3_NormalLocalScale = Vector3.one;

		private Vector3 mVector3_DefaultLocalPosition;

		protected Vector3 mBackMinimum = new Vector3(0.94f, 0.94f);

		protected Vector3 mBackMaximum = new Vector3(1f, 1f);

		protected Vector3 mFrontMinimum = new Vector3(0.95f, 0.95f);

		protected Vector3 mFrontMaximum = new Vector3(0.98f, 0.98f);

		protected int mGlowBackWidth;

		protected int mGlowBackHeight;

		protected int mGlowFrontWidth;

		protected int mGlowFrontHeight;

		[SerializeField]
		private string mScene;

		private Action<UIPortMenuButton> mOnClickEventListener;

		public float alpha
		{
			get
			{
				if (mWidgetThis != null)
				{
					return mWidgetThis.alpha;
				}
				return -1f;
			}
			set
			{
				if (mWidgetThis != null)
				{
					mWidgetThis.alpha = value;
				}
			}
		}

		public bool IsSelectable
		{
			get;
			private set;
		}

		public UIPortMenuButtonKeyMap GetKeyMap()
		{
			return mUIPortMenuButtonKeyMap;
		}

		public Vector3 GetDefaultLocalPosition()
		{
			return mVector3_DefaultLocalPosition;
		}

		public UIButton GetButton()
		{
			return mButton_Action;
		}

		private void Start()
		{
			OnStart();
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mWidgetThis);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Action);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Base);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Glow_Back);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Glow_Front);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Front);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_TextShadow);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_Text);
			if (mUIPortMenuButtonKeyMap != null)
			{
				mUIPortMenuButtonKeyMap.Release();
			}
			mUIPortMenuButtonKeyMap = null;
			OnCallDestroy();
		}

		protected virtual void OnCallDestroy()
		{
		}

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
			mButton_Action = GetComponent<UIButton>();
			OnAwake();
		}

		public Generics.Scene GetScene()
		{
			switch (mScene)
			{
			case "Organize":
				return Generics.Scene.Organize;
			case "Remodel":
				return Generics.Scene.Remodel;
			case "Arsenal":
				return Generics.Scene.Arsenal;
			case "Duty":
				return Generics.Scene.Duty;
			case "Repair":
				return Generics.Scene.Repair;
			case "Supply":
				return Generics.Scene.Supply;
			case "Strategy":
				return Generics.Scene.Strategy;
			case "Record":
				return Generics.Scene.Record;
			case "Album":
				return Generics.Scene.Album;
			case "Item":
				return Generics.Scene.Item;
			case "Option":
				return Generics.Scene.Option;
			case "Interior":
				return Generics.Scene.Interior;
			case "SaveLoad":
				return Generics.Scene.SaveLoad;
			case "Marriage":
				return Generics.Scene.Marriage;
			default:
				return Generics.Scene.Strategy;
			}
		}

		public void Initialize(bool selectable)
		{
			base.transform.localScale = mVector3_NormalLocalScale;
			IsSelectable = selectable;
			OnInitialize(selectable);
		}

		public void SetOnClickEventListener(Action<UIPortMenuButton> onClickEventListener)
		{
			mOnClickEventListener = onClickEventListener;
		}

		[Obsolete("Inspector上で使用します。")]
		public void TouchEvent()
		{
			if (IsSelectable)
			{
				ClickEvent();
			}
		}

		public void ClickEvent()
		{
			if (mOnClickEventListener != null)
			{
				mOnClickEventListener(this);
			}
		}

		public void Hover()
		{
			OnHoverEvent();
		}

		public void RemoveHover()
		{
			OnRemoveHoverEvent();
		}

		protected virtual void OnAwake()
		{
			mVector3_DefaultLocalPosition = base.transform.localPosition;
			mTexture_Text.alpha = 0f;
			mTexture_Glow_Back.transform.localScale = Vector3.zero;
			mTexture_Glow_Front.transform.localScale = Vector3.zero;
		}

		protected virtual void OnStart()
		{
		}

		public virtual Tween GenerateTweenRemoveHover()
		{
			Vector3 one = Vector3.one;
			return base.transform.DOScale(one, 0.15f).SetId(this);
		}

		public virtual Tween GenerateTweenClick()
		{
			SoundUtils.PlaySE(SEFIleInfos.MainMenuOnClick);
			Sequence sequence = DOTween.Sequence().SetId(this);
			Tween t = base.transform.DOScale(new Vector3(0.9f, 0.9f), 0.075f).SetEase(Ease.Linear).SetId(this);
			Tween t2 = base.transform.DOScale(new Vector3(1f, 1f), 0.075f).SetEase(Ease.Linear).SetId(this);
			TweenCallback callback = delegate
			{
			};
			sequence.Append(t);
			sequence.Append(t2);
			sequence.AppendCallback(callback);
			return sequence;
		}

		public virtual Tween GenerateTweenHoverScale()
		{
			return ShortcutExtensions.DOScale(endValue: new Vector3(1.15f, 1.15f), target: base.transform, duration: 0.15f).SetId(this);
		}

		public virtual Tween GenerateTweenRemoveFocus()
		{
			Sequence sequence = DOTween.Sequence().SetId(this);
			Tween t = DOVirtual.Float(mTexture_Text.alpha, 0f, 0.15f, delegate(float alpha)
			{
				mTexture_Text.alpha = alpha;
			}).SetId(this);
			Tween t2 = mTexture_Front.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutExpo).SetId(this);
			Sequence sequence2 = DOTween.Sequence().SetId(this);
			Tween t3 = mTexture_Glow_Back.transform.DOScale(Vector3.zero, 0.2f).SetId(this);
			Tween t4 = mTexture_Glow_Front.transform.DOScale(Vector3.zero, 0.2f).SetId(this);
			sequence2.Append(t3);
			sequence2.Join(t4);
			sequence2.SetEase(Ease.InOutCubic);
			sequence.Append(t2);
			sequence.Join(sequence2);
			sequence.Join(t);
			return sequence;
		}

		public virtual Tween GenerateTweenFocus()
		{
			Sequence sequence = DOTween.Sequence().SetId(this);
			Tween t = mTexture_Front.transform.DOScale(mVector3_FrontCoverOutScale, 0.15f).SetEase(Ease.OutExpo).SetId(this);
			Tween t2 = mTexture_Front.transform.DOScale(mVector3_DefaultFrontScale, 0.15f).SetEase(Ease.Linear).SetId(this);
			sequence.Append(t);
			sequence.Append(t2);
			Sequence sequence2 = DOTween.Sequence().SetId(this);
			Tween t3 = mTexture_Glow_Back.transform.DOScale(mBackMinimum, 0.1f).SetId(this);
			Tween t4 = mTexture_Glow_Front.transform.DOScale(mFrontMinimum, 0.1f).SetId(this);
			sequence2.Append(t3);
			sequence2.Join(t4);
			Sequence sequence3 = DOTween.Sequence().SetId(this);
			Tween t5 = mTexture_Glow_Back.transform.DOScale(mBackMaximum, 1f).SetId(this);
			Tween t6 = mTexture_Glow_Front.transform.DOScale(mFrontMaximum, 1f).SetId(this);
			sequence3.Append(t5);
			sequence3.Join(t6);
			sequence3.SetEase(Ease.InOutSine);
			Sequence sequence4 = DOTween.Sequence().SetId(this);
			Tween t7 = mTexture_Glow_Back.transform.DOScale(mBackMinimum, 2f).SetId(this);
			Tween t8 = mTexture_Glow_Front.transform.DOScale(mFrontMinimum, 2f).SetId(this);
			sequence4.Append(t7);
			sequence4.Join(t8);
			sequence4.SetEase(Ease.InOutSine);
			Sequence sequence5 = DOTween.Sequence().SetId(this);
			sequence5.Append(sequence3);
			sequence5.Append(sequence4);
			sequence5.SetLoops(int.MaxValue);
			Sequence sequence6 = DOTween.Sequence().SetId(this);
			Tween t9 = DOVirtual.Float(mTexture_Text.alpha, 1f, 0.15f, delegate(float alpha)
			{
				mTexture_Text.alpha = alpha;
			}).SetId(this);
			sequence6.Append(sequence2);
			sequence6.Join(t9);
			sequence6.Append(sequence);
			sequence6.Append(sequence5);
			return sequence6;
		}

		protected virtual void OnHoverEvent()
		{
		}

		protected virtual void OnRemoveHoverEvent()
		{
		}

		protected virtual void OnInitialize(bool isSelectable)
		{
			if (isSelectable)
			{
				mTexture_TextShadow.color = new Color(56f / 85f, 169f / 255f, 173f / 255f, mTexture_TextShadow.alpha);
				return;
			}
			mTexture_Front.mainTexture = (Resources.Load("Textures/PortTop/menu_bg6") as Texture);
			mTexture_Base.mainTexture = (Resources.Load("Textures/PortTop/menu_bg5") as Texture);
			mTexture_TextShadow.color = new Color(48f / 85f, 48f / 85f, 48f / 85f, mTexture_TextShadow.alpha);
			mTexture_Text.color = new Color(48f / 85f, 48f / 85f, 48f / 85f, mTexture_Text.alpha);
		}
	}
}
