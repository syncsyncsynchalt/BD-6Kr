using Common.Enum;
using KCV.Utils;
using KCV.View.Scroll;
using local.models;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeShipListChild : UIScrollListChild<ShipModel>
	{
		[SerializeField]
		protected UITexture _uiDeckFlag;

		[SerializeField]
		protected UISprite mSprite_TypeIcon;

		[SerializeField]
		protected UISprite mSprite_StateIcon;

		[SerializeField]
		protected UILabel mLabel_Name;

		[SerializeField]
		protected UILabel mLabel_Level;

		[SerializeField]
		protected UILabel mLabel_Taikyu;

		[SerializeField]
		protected UILabel mLabel_Karyoku;

		[SerializeField]
		protected UILabel mLabel_Taiku;

		[SerializeField]
		protected UILabel mLabel_Speed;

		[SerializeField]
		protected UISprite mSprite_LockIcon;

		[SerializeField]
		protected UISprite mSprite_ActiveState;

		[SerializeField]
		protected UISprite mSprite_ShipState;

		[SerializeField]
		protected Animation mAnimation_MarriagedRing;

		public override void OnTouchScrollListChild()
		{
			if (base.Model != null)
			{
				base.OnTouchScrollListChild();
			}
		}

		protected override void InitializeChildContents(ShipModel shipModel, bool clickable)
		{
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
			if (base.Model.IsMarriage())
			{
				StartRingAnimation();
			}
			else
			{
				StopRingAnimation();
			}
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
			if (shipModel.IsMarriage())
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

		public void SwitchShipLockState()
		{
			OrganizeTaskManager.Instance.GetTopTask().UpdateShipLock(base.Model.MemId);
			if (base.Model.IsLocked())
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
			if (base.Model.IsMarriage() && !mAnimation_MarriagedRing.isPlaying)
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

		private void InitializeActiveStateIcon(ShipModel shipModel)
		{
			if (shipModel.IsBling())
			{
				mSprite_ActiveState.spriteName = "icon_kaikou";
				mSprite_ActiveState.SetDimensions(32, 43);
			}
			else if (shipModel.IsInMission())
			{
				mSprite_ActiveState.spriteName = "shipicon_ensei";
				mSprite_ActiveState.SetDimensions(32, 43);
			}
			else if (shipModel.IsEscaped())
			{
				mSprite_ActiveState.spriteName = "shipicon_withdraw";
				mSprite_ActiveState.SetDimensions(32, 43);
			}
			else if (shipModel.IsInActionEndDeck())
			{
				mSprite_ActiveState.spriteName = "icon-s_done";
				mSprite_ActiveState.SetDimensions(49, 41);
			}
			else
			{
				mSprite_ActiveState.spriteName = string.Empty;
			}
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

		protected virtual DeckModelBase GetDeckFromShip(ShipModel shipModel)
		{
			return shipModel.getDeck();
		}

		private void InitializeNormalDeckFlag(int deckId, bool isFlagShip)
		{
			string empty = string.Empty;
			string str = (!isFlagShip) ? $"icon_deck{deckId}" : $"icon_deck{deckId}_fs";
			_uiDeckFlag.mainTexture = (Resources.Load("Textures/Common/DeckFlag/" + str) as Texture2D);
			_uiDeckFlag.SetDimensions(56, 42);
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
	}
}
