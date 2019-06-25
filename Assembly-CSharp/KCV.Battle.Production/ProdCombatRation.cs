using Common.Struct;
using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	[RequireComponent(typeof(Animation))]
	public class ProdCombatRation : MonoBehaviour
	{
		private const int PARTICIPANTS_NUM = 3;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiForeground;

		[SerializeField]
		private UITexture _uiRationIcon;

		[SerializeField]
		private UIWidget _uiShipsAnchor;

		[SerializeField]
		private List<UITexture> _listShipTexture;

		private UIPanel _uiPanel;

		private Animation _anim;

		private RationModel _clsRationModel;

		private Action _actOnStageReady;

		private Action _actOnFinished;

		private List<ShipModel_Eater> _listShips;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		private new Animation animation => this.GetComponentThis(ref _anim);

		public static ProdCombatRation Instantiate(ProdCombatRation prefab, Transform parent, RationModel model)
		{
			ProdCombatRation prodCombatRation = UnityEngine.Object.Instantiate(prefab);
			prodCombatRation.transform.parent = parent;
			prodCombatRation.transform.localScaleZero();
			prodCombatRation.transform.localPositionZero();
			prodCombatRation.Init(model);
			return prodCombatRation;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiForeground);
			Mem.Del(ref _uiRationIcon);
			Mem.Del(ref _uiShipsAnchor);
			Mem.DelListSafe(ref _listShipTexture);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _anim);
			Mem.Del(ref _clsRationModel);
			Mem.Del(ref _actOnFinished);
			Mem.DelListSafe(ref _listShips);
		}

		private bool Init(RationModel model)
		{
			_clsRationModel = model;
			_uiShipsAnchor.alpha = 0.01f;
			_listShips = new List<ShipModel_Eater>(3);
			SetShipsInfos(model.EatingShips, model.SharedShips);
			SetShipTexture(_listShips);
			return true;
		}

		private void SetShipsInfos(List<ShipModel_Eater> eatingShips, List<ShipModel_Eater> sharedShips)
		{
			foreach (ShipModel_Eater eatingShip in eatingShips)
			{
				if (eatingShip != null && _listShips.Count < _listShips.Capacity)
				{
					_listShips.Add(eatingShip);
				}
			}
			foreach (ShipModel_Eater sharedShip in sharedShips)
			{
				if (sharedShip != null && _listShips.Count < _listShips.Capacity)
				{
					_listShips.Add(sharedShip);
				}
			}
		}

		private void SetShipTexture(List<ShipModel_Eater> ships)
		{
			switch (ships.Count)
			{
			case 1:
			{
				ShipModel_Eater shipModel_Eater3 = ships[0];
				Point cutinSp1_InBattle3 = shipModel_Eater3.Offsets.GetCutinSp1_InBattle(shipModel_Eater3.DamagedFlg);
				_listShipTexture[1].mainTexture = ShipUtils.LoadTexture(shipModel_Eater3);
				_listShipTexture[1].MakePixelPerfect();
				_listShipTexture[1].transform.localPosition = new Vector3(cutinSp1_InBattle3.x, cutinSp1_InBattle3.y, 0f);
				break;
			}
			case 2:
			{
				ShipModel_Eater shipModel_Eater2 = ships[0];
				Point cutinSp1_InBattle2 = shipModel_Eater2.Offsets.GetCutinSp1_InBattle(shipModel_Eater2.DamagedFlg);
				_listShipTexture[0].mainTexture = ShipUtils.LoadTexture(shipModel_Eater2);
				_listShipTexture[0].MakePixelPerfect();
				_listShipTexture[0].transform.localPosition = new Vector3(cutinSp1_InBattle2.x, cutinSp1_InBattle2.y, 0f);
				shipModel_Eater2 = ships[1];
				cutinSp1_InBattle2 = shipModel_Eater2.Offsets.GetCutinSp1_InBattle(shipModel_Eater2.DamagedFlg);
				_listShipTexture[2].mainTexture = ShipUtils.LoadTexture(shipModel_Eater2);
				_listShipTexture[2].MakePixelPerfect();
				_listShipTexture[2].transform.localPosition = new Vector3(cutinSp1_InBattle2.x, cutinSp1_InBattle2.y, 0f);
				break;
			}
			case 3:
			{
				ShipModel_Eater shipModel_Eater = ships[0];
				Point cutinSp1_InBattle = shipModel_Eater.Offsets.GetCutinSp1_InBattle(shipModel_Eater.DamagedFlg);
				_listShipTexture[1].mainTexture = ShipUtils.LoadTexture(shipModel_Eater);
				_listShipTexture[1].MakePixelPerfect();
				_listShipTexture[1].transform.localPosition = new Vector3(cutinSp1_InBattle.x, cutinSp1_InBattle.y, 0f);
				shipModel_Eater = ships[1];
				cutinSp1_InBattle = shipModel_Eater.Offsets.GetCutinSp1_InBattle(shipModel_Eater.DamagedFlg);
				_listShipTexture[0].mainTexture = ShipUtils.LoadTexture(shipModel_Eater);
				_listShipTexture[0].MakePixelPerfect();
				_listShipTexture[0].transform.localPosition = new Vector3(cutinSp1_InBattle.x, cutinSp1_InBattle.y, 0f);
				shipModel_Eater = ships[2];
				cutinSp1_InBattle = shipModel_Eater.Offsets.GetCutinSp1_InBattle(shipModel_Eater.DamagedFlg);
				_listShipTexture[2].mainTexture = ShipUtils.LoadTexture(shipModel_Eater);
				_listShipTexture[2].MakePixelPerfect();
				_listShipTexture[2].transform.localPosition = new Vector3(cutinSp1_InBattle.x, cutinSp1_InBattle.y, 0f);
				break;
			}
			}
		}

		public ProdCombatRation SetOnStageReady(Action onStageReady)
		{
			_actOnStageReady = onStageReady;
			return this;
		}

		public void Play(Action onFinished)
		{
			_actOnFinished = onFinished;
			animation.Play();
			base.transform.localScaleOne();
		}

		private void OnStageReady()
		{
			Dlg.Call(ref _actOnStageReady);
		}

		private void PlayEatingVoice()
		{
			ShipUtils.PlayEatingVoice(_listShips[0]);
		}

		private void OnFinished()
		{
			Dlg.Call(ref _actOnFinished);
		}
	}
}
