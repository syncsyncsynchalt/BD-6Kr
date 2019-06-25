using Common.Enum;
using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdTranscendenceCutIn : BaseBattleAnimation
	{
		public enum AnimationList
		{
			ProdTAMainBatteryx3,
			ProdTATorpedox2,
			ProdTAMainBatteryNTorpedo
		}

		private AnimationList _iList;

		private ShipModel_Attacker _clsAttacker;

		private List<UITexture> _listShipTexture;

		private ProdTranscendenceSlots _prodTranscendenceSlots;

		private UIPanel _uiPanel;

		public AnimationList playAnimation => _iList;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static ProdTranscendenceCutIn Instantiate(ProdTranscendenceCutIn prefab, Transform parent)
		{
			ProdTranscendenceCutIn prodTranscendenceCutIn = UnityEngine.Object.Instantiate(prefab);
			prodTranscendenceCutIn.transform.parent = parent;
			prodTranscendenceCutIn.transform.localScale = Vector3.zero;
			prodTranscendenceCutIn.transform.localPosition = Vector3.zero;
			return prodTranscendenceCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
			_prodTranscendenceSlots = base.transform.GetComponentInChildren<ProdTranscendenceSlots>();
			_prodTranscendenceSlots.Init();
			Transform transform = base.transform.FindChild("ShipAnchor").transform;
			_listShipTexture = new List<UITexture>();
			_listShipTexture.Add(((Component)transform.transform.FindChild("Ship")).GetComponent<UITexture>());
			_listShipTexture.Add(((Component)transform.transform.FindChild("Mask")).GetComponent<UITexture>());
			panel.widgetsAreStatic = true;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _iList);
			Mem.Del(ref _clsAttacker);
			Mem.DelListSafe(ref _listShipTexture);
			Mem.Del(ref _prodTranscendenceSlots);
			Mem.Del(ref _uiPanel);
		}

		public void SetShellingData(HougekiModel model)
		{
			_iList = getAnimation(model.AttackType);
			_clsAttacker = model.Attacker;
			setShipsTexture(model.Attacker);
			setSlotItems(model.GetSlotitems());
		}

		private void setShipsTexture(ShipModel_Attacker model)
		{
			Texture2D mainTexture = ShipUtils.LoadTexture(model.GetGraphicsMstId(), model.IsFriend() && model.DamagedFlg);
			Vector2 v = ShipUtils.GetShipOffsPos(model, model.DamagedFlg, GetGraphColumn(_iList));
			foreach (UITexture item in _listShipTexture)
			{
				item.mainTexture = mainTexture;
				item.MakePixelPerfect();
				item.transform.localPosition = v;
			}
		}

		private void setSlotItems(SlotitemModel_Battle[] models)
		{
			ProdShellingSlotLine prodShellingSlotLine = BattleTaskManager.GetPrefabFile().prodShellingSlotLine;
			prodShellingSlotLine.SetSlotData(models, _iList);
			setHexBtn(models);
		}

		private void setHexBtn(SlotitemModel_Battle[] models)
		{
			switch (_iList)
			{
			case AnimationList.ProdTAMainBatteryx3:
			{
				int num2 = 0;
				foreach (UISlotItemHexButton hexButton in _prodTranscendenceSlots.hexButtonList)
				{
					if (hexButton != null)
					{
						hexButton.SetSlotItem(models[num2]);
					}
					hexButton.SetActive(isActive: false);
					num2++;
				}
				break;
			}
			case AnimationList.ProdTATorpedox2:
			{
				int num = 0;
				foreach (UISlotItemHexButton hexButton2 in _prodTranscendenceSlots.hexButtonList)
				{
					if (num == 0)
					{
						hexButton2.SetSlotItem(models[0]);
					}
					else if (num > 0)
					{
						hexButton2.SetSlotItem(models[1]);
					}
					num++;
				}
				break;
			}
			case AnimationList.ProdTAMainBatteryNTorpedo:
				_prodTranscendenceSlots.hexButtonList[0].SetSlotItem(models[0]);
				_prodTranscendenceSlots.hexButtonList[1].SetSlotItem(models[1]);
				_prodTranscendenceSlots.hexButtonList[0].SetActive(isActive: false);
				_prodTranscendenceSlots.hexButtonList[1].SetActive(isActive: false);
				break;
			}
		}

		public override void Play(Action callback)
		{
			panel.widgetsAreStatic = false;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			Init();
			setGlowEffects();
			base.transform.localScale = Vector3.one;
			base.Play(_iList, callback);
		}

		public void Play(AnimationList iList, Action callback)
		{
			Init();
			_actCallback = callback;
			base.transform.localPosition = Vector3.zero;
			_iList = iList;
			setGlowEffects();
			base.transform.localScale = Vector3.one;
			_animAnimation.Play(iList.ToString());
		}

		private void PlayShellingVoice()
		{
			ShipUtils.PlayShellingVoive(_clsAttacker);
		}

		private void playShellingSE()
		{
			SoundUtils.PlayShellingSE(_clsAttacker);
		}

		private void playSlotItemSuccessiveLine()
		{
			ProdShellingSlotLine prodShellingSlotLine = BattleTaskManager.GetPrefabFile().prodShellingSlotLine;
			prodShellingSlotLine.PlayTranscendenceLine(BaseProdLine.AnimationName.ProdSuccessiveLine, _clsAttacker.IsFriend(), null);
		}

		private void playSlotItemTripleLine()
		{
			ProdShellingSlotLine prodShellingSlotLine = BattleTaskManager.GetPrefabFile().prodShellingSlotLine;
			prodShellingSlotLine.PlayTranscendenceLine(BaseProdLine.AnimationName.ProdTripleLine, _clsAttacker.IsFriend(), null);
		}

		private void playSlotItem(int nSlotNum)
		{
			iTween.ValueTo(base.gameObject, getGlowTIntHash(0.3f));
		}

		private void playSlotItems()
		{
			_prodTranscendenceSlots.Play(_iList);
		}

		protected override void onAnimationFinished()
		{
			panel.widgetsAreStatic = true;
			base.onAnimationFinished();
		}

		private void PlayGlow()
		{
			playGlowEffect();
		}

		private AnimationList getAnimation(BattleAttackKind iKind)
		{
			switch (iKind)
			{
			case BattleAttackKind.Syu_Syu_Syu:
				return AnimationList.ProdTAMainBatteryx3;
			case BattleAttackKind.Syu_Syu_Fuku:
				return AnimationList.ProdTAMainBatteryx3;
			case BattleAttackKind.Rai_Rai:
				return AnimationList.ProdTATorpedox2;
			case BattleAttackKind.Syu_Rai:
				return AnimationList.ProdTAMainBatteryNTorpedo;
			default:
				return AnimationList.ProdTAMainBatteryx3;
			}
		}

		private MstShipGraphColumn GetGraphColumn(AnimationList iList)
		{
			switch (iList)
			{
			case AnimationList.ProdTAMainBatteryx3:
				return MstShipGraphColumn.CutIn;
			case AnimationList.ProdTATorpedox2:
			case AnimationList.ProdTAMainBatteryNTorpedo:
				return MstShipGraphColumn.CutInSp1;
			default:
				return MstShipGraphColumn.CutIn;
			}
		}
	}
}
