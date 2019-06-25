using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	[RequireComponent(typeof(UIButtonManager))]
	public class UIArsenalSelector : MonoBehaviour
	{
		public enum SelectType
		{
			None,
			Arsenal,
			Revamp
		}

		private UIButtonManager mButtonManager;

		[Header("Revamp")]
		[SerializeField]
		private Transform mTransform_Revamp;

		[SerializeField]
		private UIButton mButton_Revamp;

		[SerializeField]
		private UITexture mTexture_RevampTextHover;

		[SerializeField]
		[Header("Arsenal")]
		private Transform mTransform_Arsenal;

		[SerializeField]
		private UIButton mButton_Arsenal;

		[SerializeField]
		private UITexture mTexture_ArsenalTextHover;

		[Header("Ship")]
		[SerializeField]
		private Transform mTransform_ShipFrame;

		[SerializeField]
		private UITexture mTexture_FlagShip;

		[SerializeField]
		[Header("Focus")]
		private Transform mTexture_Focus;

		private Vector3 ShipLocate;

		private SelectType mSelectType;

		private KeyControl mKeyController;

		private ShipModel mFlagShip;

		private bool mIsShown;

		private Action<SelectType> mOnArsenaltypeSelectListener;

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Revamp);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_RevampTextHover);
			UserInterfacePortManager.ReleaseUtils.Release(ref mButton_Arsenal);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_ArsenalTextHover);
			UserInterfacePortManager.ReleaseUtils.Release(ref mTexture_FlagShip);
			mTransform_Arsenal = null;
			mTransform_ShipFrame = null;
			mTexture_Focus = null;
			mButtonManager = null;
			mTransform_Revamp = null;
			mKeyController = null;
			mFlagShip = null;
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[14].down)
			{
				if (mIsShown)
				{
					ChangeFocus(SelectType.Arsenal);
				}
			}
			else if (mKeyController.keyState[10].down)
			{
				if (mIsShown)
				{
					ChangeFocus(SelectType.Revamp);
				}
			}
			else if (mKeyController.keyState[1].down)
			{
				if (mOnArsenaltypeSelectListener != null && mIsShown && !DOTween.IsTweening(this))
				{
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
					mOnArsenaltypeSelectListener(mSelectType);
					mKeyController = null;
				}
			}
			else if (mKeyController.keyState[0].down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPort();
			}
			else if (mKeyController.IsRDown() && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				mKeyController = null;
			}
		}

		private void Awake()
		{
			mButtonManager = GetComponent<UIButtonManager>();
			mButtonManager.IndexChangeAct = delegate
			{
				if (mIsShown)
				{
					if (mButtonManager.nowForcusButton.Equals(mButton_Arsenal))
					{
						ChangeFocus(SelectType.Arsenal);
					}
					else if (mButtonManager.nowForcusButton.Equals(mButton_Revamp))
					{
						ChangeFocus(SelectType.Revamp);
					}
				}
			};
			mTransform_Arsenal.transform.localPosition = newVector3(-720f, 0f, 0f);
			mTransform_Revamp.transform.localPosition = newVector3(720f, 0f, 0f);
			mTransform_ShipFrame.localPosition = newVector3(0f, -1024f, 0f);
			ChangeFocus(SelectType.None);
		}

		private Vector3 newVector3(float x, float y, float z)
		{
			return Vector3.right * x + Vector3.up * y + Vector3.forward * z;
		}

		public void Initialize(ShipModel flagShip)
		{
			mFlagShip = flagShip;
			mTexture_FlagShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(flagShip.GetGraphicsMstId(), (!flagShip.IsDamaged()) ? 9 : 10);
			ShipLocate = Util.Poi2Vec(new ShipOffset(flagShip.GetGraphicsMstId()).GetShipDisplayCenter(flagShip.IsDamaged())) + Vector3.up * 115f;
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void SetOnArsenalSelectedListener(Action<SelectType> selectListener)
		{
			mOnArsenaltypeSelectListener = selectListener;
		}

		public void Show()
		{
			DOTween.Kill(this);
			Sequence sequence = DOTween.Sequence();
			Tween t = mTransform_Arsenal.transform.DOLocalMove(newVector3(-240f, 0f, 0f), 1f).SetEase(Ease.OutExpo);
			Tween t2 = mTransform_Revamp.transform.DOLocalMove(newVector3(240f, 0f, 0f), 1f).SetEase(Ease.OutExpo);
			Tween t3 = mTransform_ShipFrame.DOLocalMove(ShipLocate, 1.2f).SetEase(Ease.OutBack);
			sequence.SetId(this);
			t.SetId(this);
			t2.SetId(this);
			t3.SetId(this);
			sequence.Append(t).Join(t2).Join(t3);
			sequence.OnComplete(delegate
			{
				mIsShown = true;
				ChangeFocus(SelectType.Arsenal);
			});
		}

		public void Hide()
		{
			Sequence sequence = DOTween.Sequence();
			Tween t = mTransform_Arsenal.transform.DOLocalMove(newVector3(-7200f, 0f, 0f), 1f).SetEase(Ease.OutExpo);
			Tween t2 = mTransform_Revamp.transform.DOLocalMove(newVector3(720f, 0f, 0f), 1f).SetEase(Ease.OutExpo);
			Tween t3 = mTransform_ShipFrame.DOLocalMove(newVector3(0f, -1024f, 0f), 0.8f).SetEase(Ease.InBack);
			sequence.SetId(this);
			t.SetId(this);
			t2.SetId(this);
			t3.SetId(this);
			sequence.Append(t).Join(t2).Join(t3);
		}

		private void ChangeFocus(SelectType selectType)
		{
			Sequence s = DOTween.Sequence().SetId(this);
			if (mSelectType != selectType)
			{
				switch (mSelectType)
				{
				case SelectType.Arsenal:
				{
					Tween t = DOVirtual.Float(mTexture_ArsenalTextHover.alpha, 0f, 0.3f, delegate(float alpha)
					{
						mTexture_ArsenalTextHover.alpha = alpha;
					}).SetId(this);
					s.Join(t);
					break;
				}
				case SelectType.Revamp:
				{
					Tween t = DOVirtual.Float(mTexture_RevampTextHover.alpha, 0f, 0.3f, delegate(float alpha)
					{
						mTexture_RevampTextHover.alpha = alpha;
					}).SetId(this);
					s.Join(t);
					break;
				}
				case SelectType.None:
					mTexture_RevampTextHover.alpha = 0f;
					mTexture_ArsenalTextHover.alpha = 0f;
					break;
				}
				mSelectType = selectType;
				switch (mSelectType)
				{
				case SelectType.Arsenal:
				{
					Transform transform2 = mTexture_Focus.transform;
					Vector3 localPosition2 = mTransform_Arsenal.transform.localPosition;
					Tween t2 = transform2.DOLocalMoveX(localPosition2.x, 0.3f);
					Tween t3 = DOVirtual.Float(mTexture_ArsenalTextHover.alpha, 1f, 0.3f, delegate(float alpha)
					{
						mTexture_ArsenalTextHover.alpha = alpha;
					}).SetId(this);
					s.Join(t3);
					s.Join(t2);
					break;
				}
				case SelectType.Revamp:
				{
					Transform transform = mTexture_Focus.transform;
					Vector3 localPosition = mTransform_Revamp.transform.localPosition;
					Tween t2 = transform.DOLocalMoveX(localPosition.x, 0.3f);
					Tween t3 = DOVirtual.Float(mTexture_RevampTextHover.alpha, 1f, 0.3f, delegate(float alpha)
					{
						mTexture_RevampTextHover.alpha = alpha;
					}).SetId(this);
					s.Join(t3);
					s.Join(t2);
					break;
				}
				case SelectType.None:
					mTexture_RevampTextHover.alpha = 0f;
					mTexture_ArsenalTextHover.alpha = 0f;
					break;
				}
			}
		}

		[Obsolete("SerializeField上で使用")]
		public void OnClickArsenal()
		{
			if (mSelectType == SelectType.Arsenal)
			{
				OnSelected(SelectType.Arsenal);
			}
			else
			{
				ChangeFocus(SelectType.Arsenal);
			}
		}

		[Obsolete("SerializeField上で使用")]
		public void OnClickRevamp()
		{
			if (mSelectType == SelectType.Revamp)
			{
				OnSelected(SelectType.Revamp);
			}
			else
			{
				ChangeFocus(SelectType.Revamp);
			}
		}

		private void OnSelected(SelectType selectType)
		{
			if (mOnArsenaltypeSelectListener != null)
			{
				mOnArsenaltypeSelectListener(selectType);
			}
		}
	}
}
