using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.managers;
using local.models;
using local.models.battle;
using local.utils;
using Server_Models;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdThalassocracy : BaseAnimation
	{
		[SerializeField]
		private ParticleSystem _uiParticle;

		[SerializeField]
		private UIPanel[] _uiShipPanel;

		[SerializeField]
		private UITexture[] _uiShip;

		[SerializeField]
		private UILabel _uiLabel;

		[SerializeField]
		private Animation _anime;

		private bool _isDebugDamage;

		private int index;

		private bool _isRebelion;

		private bool _isControl;

		private KeyControl _keyControl;

		private BattleResultModel _model;

		private ShipModel_BattleAll[] _ships;

		private MapManager _clsMapManager;

		private void _init()
		{
			index = 0;
			_isDebugDamage = false;
			_isControl = false;
			_isFinished = false;
			_isRebelion = false;
			Util.FindParentToChild<ParticleSystem>(ref _uiParticle, base.transform, "BgPanel/Particle");
			Util.FindParentToChild(ref _uiLabel, base.transform, "ShipPanel/SlotLabel");
			_uiShip = new UITexture[6];
			_uiShipPanel = new UIPanel[6];
			Util.FindParentToChild(ref _uiShip[0], base.transform, "ShipPanel/ShipObj/Ship");
			Util.FindParentToChild(ref _uiShipPanel[0], base.transform, "ShipPanel");
			for (int i = 1; i < 6; i++)
			{
				Util.FindParentToChild(ref _uiShip[i], base.transform, "LeftPanel/ShipObj" + i + "/ShipObj/Ship");
				Util.FindParentToChild(ref _uiShipPanel[i], base.transform, "LeftPanel/ShipObj" + i);
			}
			if ((UnityEngine.Object)_anime == null)
			{
				_anime = GetComponent<Animation>();
			}
			((Component)_uiParticle).SetActive(isActive: false);
			_anime.Stop();
		}

		private new void OnDestroy()
		{
			Mem.Del(ref _uiParticle);
			Mem.Del(ref _uiShipPanel);
			Mem.Del(ref _uiLabel);
			Mem.Del(ref _anime);
			Mem.Del(ref _uiShip);
			Mem.Del(ref _keyControl);
			Mem.Del(ref _model);
			Mem.Del(ref _ships);
			Mem.Del(ref _clsMapManager);
		}

		public bool Run()
		{
			if (_isControl && _keyControl != null && _keyControl.keyState[1].down)
			{
				onAnimationComp();
			}
			return _isFinished;
		}

		private void setShipView()
		{
			if (_model == null || _ships == null)
			{
				return;
			}
			int num = (_model.MvpShip.MstId == _ships[0].MstId) ? 1 : 2;
			setShipTexture(0, _ships[0], isShink: false);
			for (int i = 1; i < 6; i++)
			{
				if (_ships[i] == null)
				{
					_uiShip[i].SetActive(isActive: false);
				}
				else if (_ships[i].DmgStateEnd == DamageState_Battle.Gekichin)
				{
					setShipTexture(num, _ships[i], isShink: true);
					num++;
				}
				else if (_model.MvpShip.MstId == _ships[i].MstId)
				{
					setShipTexture(1, _ships[i], isShink: false);
				}
				else
				{
					setShipTexture(num, _ships[i], isShink: false);
					num++;
				}
			}
		}

		private void setShipTexture(int index, ShipModel_BattleAll ship, bool isShink)
		{
			_uiShip[index].SetActive(isActive: true);
			_uiShip[index].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(ship.GetGraphicsMstId(), isDamaged: false);
			_uiShip[index].MakePixelPerfect();
			_uiShip[index].color = ((!isShink) ? new Color(1f, 1f, 1f, 0f) : new Color(0.3f, 0.3f, 0.3f, 0f));
			if (index == 0)
			{
				_uiShip[0].transform.localPosition = Util.Poi2Vec(new ShipOffset(ship.GetGraphicsMstId()).GetShipDisplayCenter(damaged: false));
			}
			else
			{
				_uiShip[index].transform.localPosition = Util.Poi2Vec(new ShipOffset(ship.GetGraphicsMstId()).GetFace(damaged: false));
			}
		}

		private void testShipView()
		{
			BattleResultModel battleResult = BattleTaskManager.GetBattleManager().GetBattleResult();
			ShipModel_BattleAll[] ships_f = BattleTaskManager.GetBattleManager().Ships_f;
			if (battleResult.MvpShip.MstId == ships_f[0].MstId)
			{
			}
			if (Mst_DataManager.Instance.Mst_shipgraph.ContainsKey(index))
			{
				_uiShip[0].SetActive(isActive: true);
				_uiShip[0].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(index, _isDebugDamage);
				_uiShip[0].MakePixelPerfect();
				_uiShip[0].transform.localPosition = Util.Poi2Vec(new ShipOffset(index).GetShipDisplayCenter(_isDebugDamage));
				for (int i = 1; i < 6; i++)
				{
					new ShipModelMst(1);
					_uiShip[i].SetActive(isActive: true);
					_uiShip[i].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(index, _isDebugDamage);
					_uiShip[i].MakePixelPerfect();
					_uiShip[i].transform.localPosition = Util.Poi2Vec(new ShipOffset(index).GetFace(_isDebugDamage));
				}
			}
		}

		public override void Play(Action callback)
		{
		}

		public void Play(Action callback, Generics.BattleRootType rootType, string mapName)
		{
			_actCallback = callback;
			if (_model == null || _ships == null)
			{
				onAnimationComp();
			}
			_animAnimation.Stop();
			if (rootType == Generics.BattleRootType.Rebellion)
			{
				_isRebelion = true;
				_uiLabel.text = "敵反攻作戦 迎撃成功!!";
				_animAnimation.Play("Rebelion");
			}
			else
			{
				_uiLabel.text = mapName + " 制海権確保!!";
				_animAnimation.Play("Thalassocracy");
			}
			((Component)_uiParticle).SetActive(isActive: true);
			_uiParticle.Stop();
			_uiParticle.Play();
		}

		private void startControl()
		{
			if (!_isRebelion)
			{
				TrophyUtil.Unlock_At_AreaClear(_clsMapManager.Map.AreaId);
			}
			_isControl = true;
		}

		private void _playAnimationSE(int num)
		{
			switch (num)
			{
			case 0:
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.ClearAA);
				break;
			case 1:
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.BattleAdmission);
				break;
			}
		}

		private void onAnimationComp()
		{
			_animAnimation.Stop();
			if (_actCallback != null)
			{
				_actCallback();
			}
			_isFinished = true;
		}

		public static ProdThalassocracy Instantiate(ProdThalassocracy prefab, Transform parent, KeyControl keyControl, MapManager mapManager, BattleResultModel model, ShipModel_BattleAll[] ships, int nPanelDepth)
		{
			ProdThalassocracy prodThalassocracy = UnityEngine.Object.Instantiate(prefab);
			prodThalassocracy.transform.parent = parent;
			prodThalassocracy.transform.localScale = Vector3.one;
			prodThalassocracy.transform.localPosition = Vector3.zero;
			prodThalassocracy._init();
			prodThalassocracy._keyControl = keyControl;
			prodThalassocracy._model = model;
			prodThalassocracy._ships = ships;
			prodThalassocracy._clsMapManager = mapManager;
			prodThalassocracy.setShipView();
			return prodThalassocracy;
		}
	}
}
