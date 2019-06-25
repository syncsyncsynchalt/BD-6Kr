using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	public class UIRemodelShipSlider : MonoBehaviour, UIRemodelView
	{
		private const int MAX_DECK_IN_SHIPS = 6;

		[SerializeField]
		private UIWidget mWidget_SliderBackground;

		[SerializeField]
		private UIWidget mWidget_SliderLimit;

		[SerializeField]
		private UIRemodelShipSliderThumb mUIRemodelShipSliderThumb_Thumb;

		[SerializeField]
		private UILabel mLabel_Index;

		private DeckModel mDeckModel;

		private int mShipCount;

		private int mIndex;

		private KeyControl mKeyController;

		private bool initialized;

		private Vector3 showPos = new Vector3(-435f, 16f);

		private Vector3 hidePos = new Vector3(-530f, 16f);

		private int mCellHeight;

		private bool isShown;

		private void Start()
		{
			mUIRemodelShipSliderThumb_Thumb.SetOnUIRemodelShipSliderThumbActionListener(ProcessSliderThumbAction);
		}

		public void Init(KeyControl keyController)
		{
			if (keyController != null)
			{
				keyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			mKeyController = keyController;
			Hide(animation: false);
			initialized = true;
			isShown = true;
			Show();
		}

		private void Update()
		{
			if (mKeyController != null && base.enabled && isShown)
			{
				if (mKeyController.IsUpDown())
				{
					Prev();
				}
				else if (mKeyController.IsDownDown())
				{
					Next();
				}
				else if (mKeyController.IsMaruDown())
				{
					UserInterfaceRemodelManager.instance.Forward2ModeSelect();
				}
			}
		}

		private void ChangeShip()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			mLabel_Index.text = (mIndex + 1).ToString();
			int index = mIndex;
			ShipModel ship = mDeckModel.GetShip(index);
			UserInterfaceRemodelManager.instance.ChangeFocusShip(ship);
		}

		private void ProcessSliderThumbAction(UIRemodelShipSliderThumb.ActionType actionType, UIRemodelShipSliderThumb calledObject)
		{
			if (!base.enabled)
			{
				return;
			}
			switch (actionType)
			{
			case UIRemodelShipSliderThumb.ActionType.Move:
			{
				calledObject.transform.position = calledObject.mNextDragWorldPosition;
				Vector3 localPosition = calledObject.transform.localPosition;
				if (0f < localPosition.y)
				{
					calledObject.transform.localPositionY(0f);
				}
				else
				{
					Vector3 localPosition2 = calledObject.transform.localPosition;
					if (localPosition2.y < (float)(-((mShipCount - 1) * mCellHeight)))
					{
						calledObject.transform.localPositionY(-((mShipCount - 1) * mCellHeight));
					}
				}
				OnUpdateIndex(ThumbLocalPositionToIndex(calledObject.transform.localPosition));
				break;
			}
			case UIRemodelShipSliderThumb.ActionType.FingerUp:
				calledObject.transform.localPositionY(-mIndex * mCellHeight);
				break;
			}
		}

		private int ThumbLocalPositionToIndex(Vector3 localPosition)
		{
			int num = (int)localPosition.y / (mCellHeight / 2);
			if (num != 0 && num % 2 != 0)
			{
				return Mathf.Abs((num - 1) / 2);
			}
			return Mathf.Abs(num / 2);
		}

		private void OnUpdateIndex(int index)
		{
			if (mIndex < index)
			{
				mIndex = index;
				ChangeShip();
			}
			else if (index < mIndex)
			{
				mIndex = index;
				ChangeShip();
			}
		}

		public void Initialize(DeckModel deckModel)
		{
			mUIRemodelShipSliderThumb_Thumb.transform.localPosition = Vector3.zero;
			mDeckModel = deckModel;
			mShipCount = mDeckModel.GetShips().Length;
			mCellHeight = 46;
			DOVirtual.Float(mWidget_SliderLimit.height, mCellHeight * (mShipCount - 1), 0.3f, delegate(float value)
			{
				mWidget_SliderLimit.height = (int)value;
			});
			mIndex = 0;
			mLabel_Index.text = (mIndex + 1).ToString();
		}

		public void Next()
		{
			int shipCount = mDeckModel.GetShipCount();
			if (mIndex + 1 < shipCount)
			{
				mIndex++;
				mUIRemodelShipSliderThumb_Thumb.transform.localPositionY(-mCellHeight * mIndex);
				ChangeShip();
			}
		}

		public void Prev()
		{
			if (0 <= mIndex - 1)
			{
				mIndex--;
				mUIRemodelShipSliderThumb_Thumb.transform.localPositionY(-mCellHeight * mIndex);
				ChangeShip();
			}
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
			if (initialized)
			{
				RemodelUtils.MoveWithManual(base.gameObject, showPos, 0.3f, delegate
				{
					isShown = true;
				});
				base.enabled = true;
			}
		}

		public void Hide()
		{
			Hide(animation: true);
			base.enabled = false;
		}

		public void Hide(bool animation)
		{
			isShown = false;
			if (animation)
			{
				RemodelUtils.MoveWithManual(base.gameObject, hidePos, 0.3f, delegate
				{
					base.gameObject.SetActive(false);
				});
				return;
			}
			base.transform.localPosition = hidePos;
			base.gameObject.SetActive(false);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref mWidget_SliderBackground);
			UserInterfacePortManager.ReleaseUtils.Release(ref mWidget_SliderLimit);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLabel_Index);
			mUIRemodelShipSliderThumb_Thumb = null;
			mDeckModel = null;
			mKeyController = null;
		}
	}
}
