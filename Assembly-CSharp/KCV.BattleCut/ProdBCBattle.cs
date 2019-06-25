using local.managers;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdBCBattle : MonoBehaviour
	{
		[Serializable]
		public class FleetInfos
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private Transform _traShipHPBarAnchor;

			[SerializeField]
			private BtlCut_UICircleHPGauge _uiCircleGauge;

			private List<BtlCut_HPBar> _listHPBar;

			public Transform transform => _tra;

			public Transform shipHPBarAnchor => _traShipHPBarAnchor;

			public BtlCut_UICircleHPGauge circleGauge => _uiCircleGauge;

			public List<BtlCut_HPBar> hpBars
			{
				get
				{
					if (_listHPBar == null)
					{
						_listHPBar = new List<BtlCut_HPBar>(6);
					}
					return _listHPBar;
				}
				set
				{
					_listHPBar = value;
				}
			}

			public bool UnInit()
			{
				Mem.Del(ref _tra);
				Mem.Del(ref _traShipHPBarAnchor);
				Mem.Del(ref _uiCircleGauge);
				Mem.DelListSafe(ref _listHPBar);
				return true;
			}

			public void ShakeGauge()
			{
				_uiCircleGauge.transform.ShakePosition(new Vector3(0.1f, 0.1f, 0f), 1f);
			}

			public void ShakeGauge(Vector3 amount, float time)
			{
				_uiCircleGauge.transform.ShakePosition(amount, time);
			}
		}

		private const float HPGAUGE_DEFAULT_POS_X = -346.5f;

		[SerializeField]
		private List<FleetInfos> _listFleetInfos;

		private UIPanel _uiPanel;

		private bool _isNight;

		private Action _actCallback;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static ProdBCBattle Instantiate(ProdBCBattle prefab, Transform parent)
		{
			ProdBCBattle prodBCBattle = UnityEngine.Object.Instantiate(prefab);
			prodBCBattle.transform.parent = parent;
			prodBCBattle.transform.localScaleOne();
			prodBCBattle.transform.localPositionZero();
			return prodBCBattle;
		}

		private void Awake()
		{
			panel.alpha = 0f;
			for (int i = 0; i < 6; i++)
			{
				_listFleetInfos[0].hpBars.Add(((Component)_listFleetInfos[0].shipHPBarAnchor.Find("HPBar" + (i + 1))).GetComponent<BtlCut_HPBar>());
				_listFleetInfos[1].hpBars.Add(((Component)_listFleetInfos[1].shipHPBarAnchor.Find("EnemyHPBar" + (i + 1))).GetComponent<BtlCut_HPBar>());
			}
		}

		private void OnDestroy()
		{
			Mem.DelListSafe(ref _listFleetInfos);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _isNight);
			Mem.Del(ref _actCallback);
		}

		public ProdBCBattle Play(bool isNight, Action callback)
		{
			_isNight = isNight;
			_actCallback = callback;
			InitBattleData(_isNight);
			Show(Defines.PHASE_FADE_TIME).setOnComplete((Action)delegate
			{
				Hide(1.7f, LeanTweenType.easeOutCirc).setOnComplete((Action)delegate
				{
					GotoBattleEnd();
				});
				UpdateHpGauge();
			});
			return this;
		}

		private bool InitBattleData(bool isNightCombat)
		{
			BattleManager battleManager = BattleCutManager.GetBattleManager();
			BattleData battleData = BattleCutManager.GetBattleData();
			FleetInfos fleetInfos = _listFleetInfos[0];
			FleetInfos fleetInfos2 = _listFleetInfos[1];
			battleData.friendFleetHP.attackCnt = 4;
			battleData.enemyFleetHP.attackCnt = 4;
			battleData.friendFleetHP.ClearOneAttackDamage();
			battleData.enemyFleetHP.ClearOneAttackDamage();
			_listFleetInfos.ForEach(delegate(FleetInfos x)
			{
				x.circleGauge.SetActive(isActive: true);
			});
			_listFleetInfos.ForEach(delegate(FleetInfos x)
			{
				x.shipHPBarAnchor.SetActive(isActive: true);
			});
			_listFleetInfos[0].shipHPBarAnchor.localPositionX(-346.5f);
			CalcSumHp();
			for (int i = 0; i < 6; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = battleManager.Ships_f[i];
				ShipModel_BattleAll shipModel_BattleAll2 = battleManager.Ships_e[i];
				if (shipModel_BattleAll != null)
				{
					fleetInfos.hpBars[i].SetHpBar(shipModel_BattleAll);
					fleetInfos.hpBars[i].hpData.SetEndHP(shipModel_BattleAll.HpEnd);
					for (int j = 0; j < fleetInfos.hpBars[i].hpData.oneAttackDamage.Length; j++)
					{
						battleData.friendFleetHP.oneAttackDamage[j] += fleetInfos.hpBars[i].hpData.oneAttackDamage[j];
					}
				}
				else
				{
					fleetInfos.hpBars[i].Hide();
				}
				if (shipModel_BattleAll2 != null)
				{
					fleetInfos2.hpBars[i].SetHpBar(shipModel_BattleAll2);
					fleetInfos2.hpBars[i].hpData.SetEndHP(shipModel_BattleAll2.HpEnd);
					for (int k = 0; k < fleetInfos2.hpBars[i].hpData.oneAttackDamage.Length; k++)
					{
						battleData.enemyFleetHP.oneAttackDamage[k] += fleetInfos2.hpBars[i].hpData.oneAttackDamage[k];
					}
				}
				else
				{
					fleetInfos2.hpBars[i].Hide();
				}
			}
			return true;
		}

		private void CalcSumHp()
		{
			BattleData battleData = BattleCutManager.GetBattleData();
			BattleManager battleManager = BattleCutManager.GetBattleManager();
			IEnumerable<ShipModel_BattleAll> source = from x in battleManager.Ships_f
				where x != null
				select x;
			battleData.friendFleetHP.maxHP = (from x in source
				select x.HpStart).Sum();
			battleData.friendFleetHP.nowHP = (from x in source
				select x.HpPhaseStart).Sum();
			battleData.friendFleetHP.endHP = (from x in source
				select x.HpEnd).Sum();
			battleData.friendFleetHP.nextHP = battleData.friendFleetHP.nowHP;
			IEnumerable<ShipModel_BattleAll> source2 = from x in battleManager.Ships_e
				where x != null
				select x;
			battleData.enemyFleetHP.maxHP = (from x in source2
				select x.HpStart).Sum();
			battleData.enemyFleetHP.nowHP = (from x in source2
				select x.HpPhaseStart).Sum();
			battleData.enemyFleetHP.endHP = (from x in source2
				select x.HpEnd).Sum();
			battleData.enemyFleetHP.nextHP = battleData.enemyFleetHP.nowHP;
			_listFleetInfos[0].circleGauge.SetHPGauge(battleData.friendFleetHP.maxHP, battleData.friendFleetHP.nowHP, battleData.friendFleetHP.nowHP);
			_listFleetInfos[1].circleGauge.SetHPGauge(battleData.enemyFleetHP.maxHP, battleData.enemyFleetHP.nowHP, battleData.enemyFleetHP.nowHP);
		}

		private void UpdateHpGauge()
		{
			BattleData battleData = BattleCutManager.GetBattleData();
			if (battleData.friendFleetHP.attackCnt == 3)
			{
				_listFleetInfos.ForEach(delegate(FleetInfos x)
				{
					x.ShakeGauge(Vector3.one * 0.2f, 0.7f);
				});
			}
			else if (battleData.friendFleetHP.attackCnt != 2)
			{
				_listFleetInfos.ForEach(delegate(FleetInfos x)
				{
					x.ShakeGauge(new Vector3(0.075f, 0.075f, 0f), 0.15f);
				});
			}
			UpdateSumHpGauge(_listFleetInfos[0].circleGauge, battleData.friendFleetHP, UpdateHpGauge);
			UpdateSumHpGauge(_listFleetInfos[1].circleGauge, battleData.enemyFleetHP, null);
			for (int i = 0; i < _listFleetInfos[0].hpBars.Capacity; i++)
			{
				if (_listFleetInfos[0].hpBars[i].hpData != null)
				{
					_listFleetInfos[0].hpBars[i].Play();
				}
				if (_listFleetInfos[1].hpBars[i].hpData != null)
				{
					_listFleetInfos[1].hpBars[i].Play();
				}
			}
		}

		private void UpdateSumHpGauge(BtlCut_UICircleHPGauge gauge, HPData hpData, Action act)
		{
			hpData.attackCnt--;
			hpData.nextHP -= hpData.oneAttackDamage[3 - hpData.attackCnt];
			if (hpData.attackCnt != 0)
			{
				gauge.SetHPGauge(hpData.maxHP, hpData.nowHP, hpData.nextHP);
				gauge.Play(act);
				hpData.nowHP = hpData.nextHP;
			}
			else
			{
				gauge.SetHPGauge(hpData.maxHP, hpData.nowHP, hpData.endHP);
				gauge.Play(delegate
				{
				});
				hpData.nowHP = hpData.nextHP;
			}
		}

		public void setResultHPModeAdvancingWithdrawal(float y)
		{
			BattleManager battleManager = BattleCutManager.GetBattleManager();
			FleetInfos fleetInfos = _listFleetInfos[0];
			FleetInfos fleetInfo = _listFleetInfos[1];
			for (int i = 0; i < 6; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = battleManager.Ships_f[i];
				if (shipModel_BattleAll != null)
				{
					fleetInfos.hpBars[i].SetHpBarAfter(shipModel_BattleAll, battleManager);
				}
			}
			_listFleetInfos[0].shipHPBarAnchor.localPositionX(-140.5f);
			_listFleetInfos[0].shipHPBarAnchor.localPositionY(y);
			Show(1f);
			_listFleetInfos.ForEach(delegate(FleetInfos x)
			{
				x.circleGauge.SetActive(isActive: false);
			});
			_listFleetInfos[1].shipHPBarAnchor.SetActive(isActive: false);
		}

		public void SetResultHPModeToWithdrawal(float y)
		{
			_listFleetInfos[0].shipHPBarAnchor.localPositionX(-140.5f);
			_listFleetInfos[0].shipHPBarAnchor.localPositionY(y);
			_listFleetInfos.ForEach(delegate(FleetInfos x)
			{
				x.circleGauge.SetActive(isActive: false);
			});
			_listFleetInfos[1].shipHPBarAnchor.SetActive(isActive: false);
			Show(0.35f);
		}

		private LTDescr Show(float time)
		{
			return panel.transform.LTValue(0f, 1f, time).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		public LTDescr Hide(float time)
		{
			return panel.transform.LTValue(1f, 0f, time).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		private LTDescr Hide(float time, LeanTweenType iType)
		{
			return panel.transform.LTValue(1f, 0f, time).setEase(iType).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		private void GotoBattleEnd()
		{
			if (BattleCutManager.GetBattleManager().HasNightBattle() && !_isNight)
			{
				BattleCutManager.ReqPhase(BattleCutPhase.WithdrawalDecision);
			}
			else if (_actCallback != null)
			{
				_actCallback();
			}
		}

		public void GotoResult()
		{
			BattleCutManager.ReqPhase(BattleCutPhase.WithdrawalDecision);
		}
	}
}
