using KCV.Production;
using KCV.Utils;
using local.models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.Scene.Revamp
{
	[RequireComponent(typeof(UIPanel))]
	public class UIRevampIcon : MonoBehaviour
	{
		private const int MAX_LEVEL_ITEM = 10;

		private const int MIN_LEVEL_ITEM = 0;

		[SerializeField]
		private ProdRevampReceiveItem mPreafab_ProdRevampReceiveItem;

		[SerializeField]
		private UITexture mTexture_Icon;

		[SerializeField]
		private UITexture mTexture_Icon2;

		[SerializeField]
		private GameObject mGameObject_LevelBackgroundTextImage;

		[SerializeField]
		private GameObject mGameObject_LevelTag;

		[SerializeField]
		private GameObject mGameObject_LevelMaxTag;

		[SerializeField]
		private UILabel mLabel_Level;

		[SerializeField]
		private Animation mAnimation_Effect;

		private UIPanel mPanelThis;

		private Coroutine mAnimationCoroutine;

		private SlotitemModel mSlotitemModel;

		private bool mIsGradeUpItem;

		private Camera mCameraProduction;

		private int _before;

		public int mBeforeSlotItemMasterId
		{
			get;
			private set;
		}

		public int mBeforeSlotItemLevel
		{
			get;
			private set;
		}

		public int mAfterSlotItemMasterId
		{
			get;
			private set;
		}

		public int mAfterSlotItemLevel
		{
			get;
			private set;
		}

		public string mAfterSlotItemName
		{
			get;
			private set;
		}

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mPanelThis.alpha = 0.01f;
		}

		public void Initialize(int beforeMasterId, int beforeLevel, Camera cameraProduction)
		{
			mCameraProduction = cameraProduction;
			mBeforeSlotItemMasterId = beforeMasterId;
			mBeforeSlotItemLevel = beforeLevel;
			_before = beforeLevel;
			UpdateIcon(mBeforeSlotItemMasterId);
			UpdateLevel(mBeforeSlotItemLevel, anime: false);
			mPanelThis.alpha = 1f;
		}

		private void UpdateLevel(int slotItemLevel, bool anime)
		{
			if (slotItemLevel <= 0)
			{
				mGameObject_LevelBackgroundTextImage.SetActive(false);
				mGameObject_LevelMaxTag.SetActive(false);
				mGameObject_LevelTag.SetActive(false);
				mLabel_Level.SetActive(isActive: false);
			}
			else if (10 <= slotItemLevel)
			{
				mGameObject_LevelBackgroundTextImage.SetActive(false);
				mGameObject_LevelMaxTag.SetActive(true);
				mGameObject_LevelTag.SetActive(false);
				mLabel_Level.SetActive(isActive: false);
				if (anime && _before != slotItemLevel)
				{
					LevelIconAnime(mLabel_Level.transform);
				}
			}
			else
			{
				mGameObject_LevelTag.SetActive(true);
				mGameObject_LevelBackgroundTextImage.SetActive(true);
				mGameObject_LevelMaxTag.SetActive(false);
				mLabel_Level.SetActive(isActive: true);
				mLabel_Level.text = slotItemLevel.ToString();
				if (anime && _before != slotItemLevel)
				{
					LevelIconAnime(mLabel_Level.transform);
				}
			}
			_before = slotItemLevel;
		}

		private void LevelIconAnime(Transform tf)
		{
			tf.parent.localScale = Vector3.one * 2f;
			TweenScale tweenScale = TweenScale.Begin(tf.parent.gameObject, 0.3f, Vector3.one);
			tweenScale.animationCurve = UtilCurves.TweenEaseOutBack;
			tweenScale.delay = 0.1f;
		}

		private void UpdateIcon(int slotItemMasterId)
		{
			mTexture_Icon.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(slotItemMasterId, 1);
			UITexture uITexture = mTexture_Icon;
			Vector2 vector = ResourceManager.SLOTITEM_TEXTURE_SIZE[1];
			int w = (int)vector.x;
			Vector2 vector2 = ResourceManager.SLOTITEM_TEXTURE_SIZE[1];
			uITexture.SetDimensions(w, (int)vector2.y);
		}

		public void StartRevamp(int afterSlotItemMasterId, int afterSlotItemLevel, string afterSlotItemName, Action finishedCallBack)
		{
			mAfterSlotItemLevel = afterSlotItemLevel;
			mAfterSlotItemMasterId = afterSlotItemMasterId;
			mAfterSlotItemName = afterSlotItemName;
			mIsGradeUpItem = (mBeforeSlotItemMasterId != mAfterSlotItemMasterId);
			if (mAnimationCoroutine != null)
			{
				StopCoroutine(mAnimationCoroutine);
			}
			mAnimationCoroutine = StartCoroutine(StartRevampCoroutine(delegate
			{
				UpdateLevel(mAfterSlotItemLevel, anime: true);
				UpdateIcon(mAfterSlotItemMasterId);
				mAnimationCoroutine = null;
				if (finishedCallBack != null)
				{
					finishedCallBack();
				}
			}));
		}

		public void StartFailRevamp(Action finishedCallBack)
		{
			if (mAnimationCoroutine != null)
			{
				StopCoroutine(mAnimationCoroutine);
			}
			mAnimationCoroutine = StartCoroutine(StartRevampCoroutine(delegate
			{
				mAnimationCoroutine = null;
				if (finishedCallBack != null)
				{
					finishedCallBack();
				}
			}));
		}

		private IEnumerator StartRevampCoroutine(Action finishedCallBack)
		{
			SoundUtils.PlaySE(SEFIleInfos.Revamp_Yousetu);

            var anim_revamp_sparks = mAnimation_Effect.GetClip("Anim_RevampSparks").length;

			Vector3 defaultPosition = mTexture_Icon.gameObject.transform.localPosition;
			for (int shakeAnimationCount = 0; shakeAnimationCount < 4; shakeAnimationCount++)
			{
				mAnimation_Effect.Play("Anim_RevampSparks");
				iTween.ShakePosition(base.gameObject, iTween.Hash("x", defaultPosition.x + 5f, "y", defaultPosition.y + 5f, "isLocal", true, "time", 0.3f));
				yield return new WaitForSeconds(0.9f);
			}
			if (mIsGradeUpItem)
			{
				ProdRevampReceiveItem prodRevampReciveItem = ProdRevampReceiveItem.Instantiate(mPreafab_ProdRevampReceiveItem, mCameraProduction.transform, new SlotitemModel_Mst(mBeforeSlotItemMasterId), new SlotitemModel_Mst(mAfterSlotItemMasterId), 500, useJukuren: false, new KeyControl());
				prodRevampReciveItem.Play(delegate
				{
					if (finishedCallBack != null)
					{
						mIsGradeUpItem = false;
						finishedCallBack();
					}
				});
			}
			else
			{
				finishedCallBack?.Invoke();
			}
			yield return null;
		}

		private void PlaySE(SEFIleInfos seType)
		{
			if (SingletonMonoBehaviour<SoundManager>.Instance != null)
			{
				if (true)
				{
					SoundUtils.PlaySE(seType);
				}
			}
			else
			{
				Debug.LogError("Plz Place SoundManager GameObject !! X(");
			}
		}

		private void OnDestroy()
		{
			mPreafab_ProdRevampReceiveItem = null;
			mTexture_Icon = null;
			mTexture_Icon2 = null;
			mGameObject_LevelBackgroundTextImage = null;
			mGameObject_LevelTag = null;
			mGameObject_LevelMaxTag = null;
			mLabel_Level = null;
			mAnimation_Effect = null;
			mPanelThis = null;
			mSlotitemModel = null;
			mCameraProduction = null;
		}
	}
}
