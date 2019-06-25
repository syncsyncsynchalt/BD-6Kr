using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(Animation))]
	[RequireComponent(typeof(UIPanel))]
	public class ProdObservedShellingCutIn : BaseAnimation
	{
		[SerializeField]
		private UITexture _uiAircraft;

		[SerializeField]
		private NoiseMove _clsNoiseMove;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiSeparatorLine;

		[SerializeField]
		private List<UITexture> _listShipTextures;

		[SerializeField]
		private List<UISlotItemHexButton> _listHexBtns;

		[SerializeField]
		private List<UILabel> _listSlotLabels;

		[SerializeField]
		private List<UITexture> _listOverlays;

		[SerializeField]
		private UITexture _uiTelopOverlay;

		private UIPanel _uiPanel;

		private ShipModel_Attacker _clsAttacker;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static ProdObservedShellingCutIn Instantiate(ProdObservedShellingCutIn prefab, Transform parent)
		{
			ProdObservedShellingCutIn prodObservedShellingCutIn = UnityEngine.Object.Instantiate(prefab);
			prodObservedShellingCutIn.transform.parent = parent;
			prodObservedShellingCutIn.transform.localScaleZero();
			prodObservedShellingCutIn.transform.localPositionZero();
			return prodObservedShellingCutIn;
		}

		protected override void Awake()
		{
			panel.widgetsAreStatic = true;
			base.Awake();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiAircraft);
			Mem.Del(ref _clsNoiseMove);
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiSeparatorLine);
			Mem.DelListSafe(ref _listShipTextures);
			Mem.DelListSafe(ref _listHexBtns);
			Mem.DelListSafe(ref _listSlotLabels);
			Mem.DelListSafe(ref _listOverlays);
			Mem.Del(ref _uiTelopOverlay);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _clsAttacker);
		}

		public void SetObservedShelling(HougekiModel model)
		{
			_clsAttacker = model.Attacker;
			Texture2D shipTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(model.Attacker);
			Vector3 offs = KCV.Battle.Utils.ShipUtils.GetShipOffsPos(model.Attacker, model.Attacker.DamagedFlg, MstShipGraphColumn.CutIn);
			_listShipTextures.ForEach(delegate(UITexture x)
			{
				x.mainTexture = shipTexture;
				x.MakePixelPerfect();
				x.transform.localPosition = offs;
			});
			List<SlotitemModel_Battle> list = new List<SlotitemModel_Battle>(model.GetSlotitems());
			_listSlotLabels[0].text = list[1].Name;
			_listSlotLabels[1].text = list[2].Name;
			_uiAircraft.mainTexture = KCV.Battle.Utils.SlotItemUtils.LoadUniDirTexture(list[0]);
			_uiAircraft.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE[6];
			_listHexBtns[0].SetSlotItem(list[1]);
			_listHexBtns[1].SetSlotItem(list[2]);
			Color col = (!_clsAttacker.IsFriend()) ? new Color(1f, 0f, 0f, 0.50196f) : new Color(0f, 51f / 160f, 1f, 0.50196f);
			_uiTelopOverlay.color = col;
			_listOverlays.ForEach(delegate(UITexture x)
			{
				x.color = col;
			});
		}

		public override void Play(Action callback)
		{
			panel.widgetsAreStatic = false;
			base.transform.localScaleOne();
			base.Play(callback);
		}

		private void PlayShellingVoice()
		{
			KCV.Battle.Utils.ShipUtils.PlayShellingVoive(_clsAttacker);
		}

		private void PlayHexSlot(int nNum)
		{
			_listHexBtns[nNum].SetActive(isActive: true);
			_listHexBtns[nNum].Play(UIHexButton.AnimationList.ProdTranscendenceAttackHex, null);
		}

		private void PlaySlotSE()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_939b);
		}

		private void PlayShellingSE()
		{
			KCV.Battle.Utils.SoundUtils.PlayShellingSE(_clsAttacker);
		}

		protected override void onAnimationFinished()
		{
			panel.widgetsAreStatic = true;
			base.onAnimationFinished();
		}
	}
}
