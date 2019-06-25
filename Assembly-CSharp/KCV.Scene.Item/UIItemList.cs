using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	public class UIItemList : MonoBehaviour
	{
		private const int SPLIT_COUNT = 7;

		[SerializeField]
		private UIItemListChild[] mItemListChildren;

		[SerializeField]
		private Transform mTransform_Focus;

		private AudioClip mAudioClip_SE_001;

		private AudioClip mAudioClip_SE_013;

		private KeyControl mKeyController;

		private Action<ItemlistModel> mOnSelectListener;

		private Action<ItemlistModel> mOnFocusChangeListener;

		private UIItemListChild mFocusListChild;

		private void Awake()
		{
			UIItemListChild[] array = mItemListChildren;
			foreach (UIItemListChild uIItemListChild in array)
			{
				uIItemListChild.SetOnTouchListener(OnTouchListChild);
			}
			mAudioClip_SE_001 = SoundFile.LoadSE(SEFIleInfos.SE_001);
			mAudioClip_SE_013 = SoundFile.LoadSE(SEFIleInfos.SE_013);
		}

		private void OnTouchListChild(UIItemListChild child)
		{
			if (mKeyController != null && child != null && child.isActiveAndEnabled)
			{
				ChangeFocus(child, playSE: true);
			}
		}

		private void Update()
		{
			if (mKeyController == null)
			{
				return;
			}
			if (mKeyController.keyState[14].down)
			{
				int num = Array.IndexOf(mItemListChildren, mFocusListChild);
				int num2 = num - 1;
				if (0 <= num2)
				{
					ChangeFocus(mItemListChildren[num2], playSE: true);
				}
			}
			else if (mKeyController.keyState[10].down)
			{
				int num3 = Array.IndexOf(mItemListChildren, mFocusListChild);
				int num4 = num3 + 1;
				if (num4 < mItemListChildren.Length)
				{
					ChangeFocus(mItemListChildren[num4], playSE: true);
				}
			}
			else if (mKeyController.keyState[8].down)
			{
				int num5 = Array.IndexOf(mItemListChildren, mFocusListChild);
				int num6 = num5 - 7;
				if (0 <= num6)
				{
					ChangeFocus(mItemListChildren[num6], playSE: true);
				}
			}
			else if (mKeyController.keyState[12].down)
			{
				int num7 = Array.IndexOf(mItemListChildren, mFocusListChild);
				int num8 = num7 + 7;
				if (num8 < mItemListChildren.Length)
				{
					ChangeFocus(mItemListChildren[num8], playSE: true);
				}
			}
			else if (mKeyController.keyState[1].down)
			{
				if (mFocusListChild != null)
				{
					ItemlistModel mModel = mFocusListChild.mModel;
					if (0 < mModel.Count && mModel.IsUsable())
					{
						SoundUtils.PlaySE(mAudioClip_SE_013);
						OnSelect(mFocusListChild.mModel);
					}
				}
			}
			else if (mKeyController.keyState[0].down)
			{
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			mKeyController = keyController;
		}

		public void SetOnSelectListener(Action<ItemlistModel> onSelectListener)
		{
			mOnSelectListener = onSelectListener;
		}

		private void OnSelect(ItemlistModel model)
		{
			if (mOnSelectListener != null)
			{
				mOnSelectListener(model);
			}
		}

		public void SetOnFocusChangeListener(Action<ItemlistModel> onFocusChangeListener)
		{
			mOnFocusChangeListener = onFocusChangeListener;
		}

		private void OnFocusChange(ItemlistModel model)
		{
			if (mOnFocusChangeListener != null)
			{
				mOnFocusChangeListener(model);
			}
		}

		public void Initialize(ItemlistModel[] models)
		{
			int num = 0;
			int num2 = models.Length;
			UIItemListChild[] array = mItemListChildren;
			foreach (UIItemListChild uIItemListChild in array)
			{
				uIItemListChild.SetActive(isActive: true);
				if (num < num2)
				{
					uIItemListChild.Initialize(models[num]);
				}
				else
				{
					uIItemListChild.Initialize(new ItemlistModel(null, null, string.Empty));
				}
				num++;
			}
			ChangeFocus(null, playSE: false);
		}

		public void FirstFocus()
		{
			ChangeFocus(mItemListChildren[0], playSE: false);
		}

		public void Refresh(ItemlistModel[] models)
		{
			int num = Array.IndexOf(mItemListChildren, mFocusListChild);
			int num2 = 0;
			int num3 = models.Length;
			UIItemListChild[] array = mItemListChildren;
			foreach (UIItemListChild uIItemListChild in array)
			{
				if (num2 < num3)
				{
					uIItemListChild.SetActive(isActive: true);
					uIItemListChild.Initialize(models[num2]);
				}
				else
				{
					uIItemListChild.SetActive(isActive: false);
				}
				num2++;
			}
			ChangeFocus(mItemListChildren[num], playSE: false);
		}

		private void ChangeFocus(UIItemListChild child, bool playSE)
		{
			if (mFocusListChild != null)
			{
				mFocusListChild.RemoveFocus();
				mFocusListChild.transform.DOScale(new Vector3(1f, 1f), 0.3f);
				mTransform_Focus.DOScale(new Vector3(1f, 1f), 0.3f);
			}
			mFocusListChild = child;
			if (mFocusListChild != null)
			{
				OnFocusChange(mFocusListChild.mModel);
				mFocusListChild.Focus();
				if (child.IsFosable())
				{
					Transform transform = mTransform_Focus;
					Vector3 localPosition = mFocusListChild.transform.localPosition;
					float x = localPosition.x;
					Vector3 localPosition2 = mFocusListChild.transform.localPosition;
					transform.localPosition = new Vector3(x, localPosition2.y - 12f);
					mTransform_Focus.DOScale(new Vector3(1.2f, 1.2f), 0.3f);
					mFocusListChild.transform.DOScale(new Vector3(1.2f, 1.2f), 0.3f);
				}
				else
				{
					Transform transform2 = mTransform_Focus;
					Vector3 localPosition3 = mFocusListChild.transform.localPosition;
					float x2 = localPosition3.x;
					Vector3 localPosition4 = mFocusListChild.transform.localPosition;
					transform2.localPosition = new Vector3(x2, localPosition4.y - 12f);
				}
				if (playSE)
				{
					SafePlaySEOneShot(mAudioClip_SE_001);
				}
			}
			else
			{
				Transform transform3 = mTransform_Focus.transform;
				Vector3 localPosition5 = mItemListChildren[0].transform.localPosition;
				float x3 = localPosition5.x;
				Vector3 localPosition6 = mItemListChildren[0].transform.localPosition;
				transform3.localPosition = new Vector3(x3, localPosition6.y - 12f);
				OnFocusChange(null);
			}
		}

		private void SafePlaySEOneShot(AudioClip audioClip)
		{
			if (SingletonMonoBehaviour<SoundManager>.exist() && SingletonMonoBehaviour<SoundManager>.Instance.seSourceObserver != null)
			{
				int index = SingletonMonoBehaviour<SoundManager>.Instance.seSourceObserver.Count - 1;
				if (SingletonMonoBehaviour<SoundManager>.Instance.seSourceObserver[index] != null && (UnityEngine.Object)SingletonMonoBehaviour<SoundManager>.Instance.seSourceObserver[index].source != null)
				{
					SingletonMonoBehaviour<SoundManager>.Instance.seSourceObserver[index].source.PlayOneShot(audioClip, SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.SE);
				}
			}
		}

		public void Clean()
		{
			mKeyController = null;
			ChangeFocus(null, playSE: false);
		}

		private void OnDestroy()
		{
			if (mItemListChildren != null)
			{
				for (int i = 0; i < mItemListChildren.Length; i++)
				{
					mItemListChildren[i] = null;
				}
				mItemListChildren = null;
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_SE_001);
			UserInterfacePortManager.ReleaseUtils.Release(ref mAudioClip_SE_013);
		}
	}
}
