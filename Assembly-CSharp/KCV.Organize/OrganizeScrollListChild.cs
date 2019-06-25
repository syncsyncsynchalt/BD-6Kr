using Common.Enum;
using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Organize
{
	[RequireComponent(typeof(UIWidget))]
	public class OrganizeScrollListChild : MonoBehaviour, UIScrollListItem<ShipModel, OrganizeScrollListChild>
	{
		[SerializeField]
		protected UITexture _uiDeckFlag;

		[SerializeField]
		protected UISprite mSprite_TypeIcon;

		[SerializeField]
		protected UILabel mLabel_Name;

		[SerializeField]
		protected UILabel mLabel_Speed;

		[SerializeField]
		protected UILabel mLabel_Level;

		[SerializeField]
		protected UILabel mLabel_Taikyu;

		[SerializeField]
		protected UILabel mLabel_Karyoku;

		[SerializeField]
		protected UILabel mLabel_Taiku;

		[SerializeField]
		protected UISprite mSprite_LockIcon;

		[SerializeField]
		protected UISprite mSprite_ShipState;

		[SerializeField]
		protected Animation mAnimation_MarriagedRing;

		[SerializeField]
		private Transform mTransform_Overlay;

		[SerializeField]
		private Transform mTransform_Bling;

		[SerializeField]
		private Transform mTransform_Deploy;

		[SerializeField]
		private UISprite mSprite_ActiveState;

		private int mRealIndex;

		private ShipModel mShipModel;

		private UIWidget mWidgetThis;

		private IOrganizeManager _OrganizeManager;

		private Transform mTransform;

		private Action<OrganizeScrollListChild> mOnTouchListener;

		private void OnDisable()
		{
			mLabel_Name.text = string.Empty;
			mLabel_Speed.text = string.Empty;
		}

		private void OnEnable()
		{
			if (GetModel() != null)
			{
				mLabel_Name.text = GetModel().Name;
				InitializeShipSpeed(GetModel());
			}
		}

		private void Awake()
		{
			mWidgetThis = GetComponent<UIWidget>();
			mWidgetThis.alpha = 1E-09f;
		}

		public void setManager(IOrganizeManager manager)
		{
			_OrganizeManager = manager;
		}

		public void Initialize(int realIndex, ShipModel shipModel)
		{
			mShipModel = shipModel;
			mRealIndex = realIndex;
			InitializeDeckFlag(shipModel);
			InitializeActiveStateIcon(shipModel);
			InitializeShipTypeIcon(shipModel);
			InitializeShipStateIcon(shipModel);
			mLabel_Name.text = shipModel.Name;
			mLabel_Level.text = shipModel.Level.ToString();
			mLabel_Taikyu.text = shipModel.MaxHp.ToString();
			mLabel_Karyoku.text = shipModel.Karyoku.ToString();
			mLabel_Taiku.text = shipModel.Taiku.ToString();
			InitializeShipSpeed(shipModel);
			InitializeLockIcon(shipModel);
			InitializeRing(shipModel);
			if (shipModel.IsMarriage())
			{
				StartRingAnimation();
			}
			else
			{
				StopRingAnimation();
			}
			mWidgetThis.alpha = 1f;
		}

		public void InitializeDefault(int realIndex)
		{
			mRealIndex = realIndex;
			mShipModel = null;
			mWidgetThis.alpha = 1E-09f;
		}

		private void InitializeShipStateIcon(ShipModel shipModel)
		{
			string spriteName = string.Empty;
			if (shipModel.IsInRepair())
			{
				spriteName = "icon-ss_syufuku";
			}
			else
			{
				switch (shipModel.DamageStatus)
				{
				case DamageState.Shouha:
					spriteName = "icon-ss_shoha";
					break;
				case DamageState.Tyuuha:
					spriteName = "icon-ss_chuha";
					break;
				case DamageState.Taiha:
					spriteName = "icon-ss_taiha";
					break;
				}
			}
			mSprite_ShipState.spriteName = spriteName;
		}

		private void InitializeActiveStateIcon(ShipModel shipModel)
		{
			mTransform_Overlay.transform.SetActive(isActive: false);
			mTransform_Bling.SetActive(isActive: false);
			mTransform_Deploy.SetActive(isActive: false);
			mSprite_ActiveState.SetActive(isActive: false);
			if (shipModel.IsBling())
			{
				mTransform_Overlay.transform.SetActive(isActive: true);
				mTransform_Bling.SetActive(isActive: true);
				mSprite_ActiveState.spriteName = "icon_kaikou";
				mSprite_ActiveState.SetDimensions(32, 43);
				return;
			}
			if (shipModel.IsBlingWait())
			{
				mTransform_Overlay.transform.SetActive(isActive: true);
				mTransform_Deploy.SetActive(isActive: true);
			}
			if (shipModel.IsInMission())
			{
				mTransform_Overlay.transform.SetActive(isActive: true);
				mSprite_ActiveState.SetActive(isActive: true);
				mSprite_ActiveState.spriteName = "shipicon_ensei";
				mSprite_ActiveState.SetDimensions(32, 43);
			}
			else if (shipModel.IsEscaped())
			{
				mTransform_Overlay.transform.SetActive(isActive: true);
				mSprite_ActiveState.SetActive(isActive: true);
				mSprite_ActiveState.spriteName = "shipicon_withdraw";
				mSprite_ActiveState.SetDimensions(32, 43);
			}
			else if (shipModel.IsInActionEndDeck())
			{
				mTransform_Overlay.transform.SetActive(isActive: true);
				mSprite_ActiveState.SetActive(isActive: true);
				mSprite_ActiveState.spriteName = "icon-s_done";
				mSprite_ActiveState.SetDimensions(49, 41);
			}
		}

		private void InitializeLockIcon(ShipModel shipModel)
		{
			if (shipModel.IsLocked())
			{
				mSprite_LockIcon.spriteName = "lock_ship";
			}
			else
			{
				mSprite_LockIcon.spriteName = "lock_ship_open";
			}
		}

		private void InitializeRing(ShipModel shipModel)
		{
			bool flag = shipModel.IsMarriage();
			mAnimation_MarriagedRing.playAutomatically = true;
			if (flag)
			{
				((Component)mAnimation_MarriagedRing).gameObject.SetActive(true);
				foreach (Transform item in ((Component)mAnimation_MarriagedRing).transform)
				{
					item.SetActive(isActive: true);
				}
			}
			else
			{
				StopRingAnimation();
				foreach (Transform item2 in ((Component)mAnimation_MarriagedRing).transform)
				{
					item2.SetActive(isActive: false);
				}
			}
		}

		private void InitializeShipSpeed(ShipModel shipModel)
		{
			if (shipModel.Soku == 10)
			{
				mLabel_Speed.text = "高速";
			}
			else
			{
				mLabel_Speed.text = "低速";
			}
		}

		private void InitializeShipTypeIcon(ShipModel shipIcon)
		{
			mSprite_TypeIcon.spriteName = "ship" + shipIcon.ShipType;
		}

		private void InitializeDeckFlag(ShipModel shipModel)
		{
			if (IsDeckInShip(shipModel))
			{
				DeckModelBase deckFromShip = GetDeckFromShip(shipModel);
				bool isFlagShip = deckFromShip.GetFlagShip().MemId == shipModel.MemId;
				int id = deckFromShip.Id;
				if (deckFromShip.IsEscortDeckMyself())
				{
					InitializeEscortDeckFlag(id, isFlagShip);
				}
				else
				{
					InitializeNormalDeckFlag(id, isFlagShip);
				}
			}
			else
			{
				RemoveDeckFlag();
			}
		}

		protected virtual bool IsDeckInShip(ShipModel shipModel)
		{
			DeckModelBase deckFromShip = GetDeckFromShip(shipModel);
			return deckFromShip != null;
		}

		public void SwitchShipLockState()
		{
			_OrganizeManager.Lock(mShipModel.MemId);
			if (mShipModel.IsLocked())
			{
				mSprite_LockIcon.spriteName = "lock_ship";
				SoundUtils.PlaySE(SEFIleInfos.SE_005);
			}
			else
			{
				mSprite_LockIcon.spriteName = "lock_ship_open";
				SoundUtils.PlaySE(SEFIleInfos.SE_006);
			}
		}

		public void StartRingAnimation()
		{
			if (mShipModel.IsMarriage() && !mAnimation_MarriagedRing.isPlaying)
			{
				mAnimation_MarriagedRing.Play();
			}
		}

		public void StopRingAnimation()
		{
			if (mAnimation_MarriagedRing.isPlaying)
			{
				mAnimation_MarriagedRing.Stop();
			}
		}

		protected virtual DeckModelBase GetDeckFromShip(ShipModel shipModel)
		{
			return shipModel.getDeck();
		}

		private void InitializeNormalDeckFlag(int deckId, bool isFlagShip)
		{
			string empty = string.Empty;
			string str = (!isFlagShip) ? $"icon_deck{deckId}" : $"icon_deck{deckId}_fs";
			_uiDeckFlag.mainTexture = (Resources.Load("Textures/Common/DeckFlag/" + str) as Texture2D);
			_uiDeckFlag.SetDimensions(60, 56);
		}

		private void RemoveDeckFlag()
		{
			_uiDeckFlag.SetDimensions(0, 0);
			_uiDeckFlag.mainTexture = null;
		}

		private void InitializeEscortDeckFlag(int deckId, bool isFlagShip)
		{
			string empty = string.Empty;
			int w = 56;
			int h = 64;
			string str = (!isFlagShip) ? "icon_guard" : "icon_guard_fs";
			_uiDeckFlag.mainTexture = (Resources.Load("Textures/Common/DeckFlag/" + str) as Texture2D);
			_uiDeckFlag.SetDimensions(w, h);
		}

		public Transform GetTransform()
		{
			if (mTransform == null)
			{
				mTransform = base.transform;
			}
			return mTransform;
		}

		public ShipModel GetModel()
		{
			return mShipModel;
		}

		public int GetHeight()
		{
			return 61;
		}

		private void OnClick()
		{
			if (mOnTouchListener != null)
			{
				mOnTouchListener(this);
			}
		}

		public void SetOnTouchListener(Action<OrganizeScrollListChild> onTouchListener)
		{
			mOnTouchListener = onTouchListener;
		}

		public void Hover()
		{
			UISelectedObject.SelectedOneObjectBlink(base.transform.FindChild("Background").gameObject, value: true);
		}

		public void RemoveHover()
		{
			UISelectedObject.SelectedOneObjectBlink(base.transform.FindChild("Background").gameObject, value: false);
		}

		public float GetBottomPositionY()
		{
			Vector3 localPosition = GetTransform().localPosition;
			return localPosition.y + (float)GetHeight();
		}

		public float GetHeadPositionY()
		{
			Vector3 localPosition = GetTransform().localPosition;
			return localPosition.y;
		}

		public int GetRealIndex()
		{
			return mRealIndex;
		}
	}
}
