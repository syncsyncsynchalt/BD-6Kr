using Common.Enum;
using KCV.Battle.Utils;
using local.models.battle;
using System;
using System.Collections.Generic;

namespace KCV.Battle.Production
{
	public class ProdShellingAttack : IDisposable
	{
		private class Statement : IDisposable
		{
			private DelProdShellingAttack _delProdShellingAttack;

			private DelProdShellingUpdate _delProdShellingUpdate;

			private DelProdShellingClear _delProdShellingClear;

			public DelProdShellingAttack Init
			{
				get
				{
					return _delProdShellingAttack;
				}
				private set
				{
					_delProdShellingAttack = value;
				}
			}

			public DelProdShellingUpdate Update
			{
				get
				{
					return _delProdShellingUpdate;
				}
				private set
				{
					_delProdShellingUpdate = value;
				}
			}

			public DelProdShellingClear Clear
			{
				get
				{
					return _delProdShellingClear;
				}
				private set
				{
					_delProdShellingClear = value;
				}
			}

			public void Dispose()
			{
				Release();
			}

			public void AddState(DelProdShellingAttack initdelegate, DelProdShellingUpdate updatedelegate, DelProdShellingClear cleardelegate)
			{
				_delProdShellingAttack = initdelegate;
				_delProdShellingUpdate = updatedelegate;
				_delProdShellingClear = cleardelegate;
			}

			public void Release()
			{
				_delProdShellingAttack = null;
				_delProdShellingUpdate = null;
				_delProdShellingClear = null;
			}
		}

		private Statement _clsStatement;

		private bool _isFinished;

		private bool _isPlaying;

		private HougekiModel _clsHougekiModel;

		private Action _actCallback;

		private Dictionary<FleetType, bool> _dicAttackFleet;

		private ProdNormalAttack _prodNormalAttack;

		private ProdAntiGroundAttack _prodAntiGroundAttack;

		private ProdTorpedoAttack _prodTorpedoAttack;

		private ProdAircraftAttack _prodAircraftAttack;

		private ProdDepthChargeAttack _prodDepthChargeAttack;

		private ProdLaserAttack _prodLaserAttack;

		private ProdSuccessiveAttack _prodSuccessiveAttack;

		private ProdObservedShellingAttack _prodObservedShellingAttack;

		private ProdTranscendenceAttack _prodTranscendenceAttack;

		public HougekiModel hougekiModel => _clsHougekiModel;

		public bool isPlaying => _isPlaying;

		public bool isFinished => _isFinished;

		public Dictionary<FleetType, bool> attackFleet => _dicAttackFleet;

		public ProdShellingAttack()
		{
			_clsStatement = new Statement();
			_dicAttackFleet = new Dictionary<FleetType, bool>();
			_dicAttackFleet.Add(FleetType.Friend, value: false);
			_dicAttackFleet.Add(FleetType.Enemy, value: false);
		}

		public void Dispose()
		{
			Mem.DelIDisposableSafe(ref _clsStatement);
			Mem.Del(ref _clsHougekiModel);
			Mem.Del(ref _actCallback);
			Mem.DelDictionarySafe(ref _dicAttackFleet);
			Mem.DelIDisposableSafe(ref _prodNormalAttack);
			Mem.DelIDisposableSafe(ref _prodAntiGroundAttack);
			Mem.DelIDisposableSafe(ref _prodTorpedoAttack);
			Mem.DelIDisposableSafe(ref _prodAircraftAttack);
			Mem.DelIDisposableSafe(ref _prodDepthChargeAttack);
			Mem.DelIDisposableSafe(ref _prodLaserAttack);
			Mem.DelIDisposableSafe(ref _prodSuccessiveAttack);
			Mem.DelIDisposableSafe(ref _prodObservedShellingAttack);
			Mem.DelIDisposableSafe(ref _prodTranscendenceAttack);
		}

		public void Clear()
		{
			Mem.Del(ref _clsHougekiModel);
			_isFinished = false;
			_isPlaying = false;
			Mem.Del(ref _actCallback);
			if (_clsStatement.Clear != null)
			{
				_clsStatement.Clear();
			}
			_clsStatement.Release();
		}

		public void Play(HougekiModel hougeki, int nCurrentShellingCnt, bool isNextAttack, Action onFinished)
		{
			if (hougeki == null)
			{
				OnShellingFinished();
			}
			_clsHougekiModel = hougeki;
			_isFinished = false;
			_isPlaying = true;
			_actCallback = onFinished;
			switch (hougeki.AttackType)
			{
			case BattleAttackKind.Normal:
				if (_clsHougekiModel.GetRocketEffenct())
				{
					if (_prodAntiGroundAttack == null)
					{
						_prodAntiGroundAttack = new ProdAntiGroundAttack();
					}
					Statement clsStatement5 = _clsStatement;
					ProdAntiGroundAttack prodAntiGroundAttack = _prodAntiGroundAttack;
					DelProdShellingAttack initdelegate5 = prodAntiGroundAttack.PlayAttack;
					ProdAntiGroundAttack prodAntiGroundAttack2 = _prodAntiGroundAttack;
					DelProdShellingUpdate updatedelegate5 = prodAntiGroundAttack2.Update;
					ProdAntiGroundAttack prodAntiGroundAttack3 = _prodAntiGroundAttack;
					clsStatement5.AddState(initdelegate5, updatedelegate5, prodAntiGroundAttack3.Clear);
				}
				else
				{
					if (_prodNormalAttack == null)
					{
						_prodNormalAttack = new ProdNormalAttack();
					}
					Statement clsStatement6 = _clsStatement;
					ProdNormalAttack prodNormalAttack = _prodNormalAttack;
					DelProdShellingAttack initdelegate6 = prodNormalAttack.PlayAttack;
					ProdNormalAttack prodNormalAttack2 = _prodNormalAttack;
					DelProdShellingUpdate updatedelegate6 = prodNormalAttack2.Update;
					ProdNormalAttack prodNormalAttack3 = _prodNormalAttack;
					clsStatement6.AddState(initdelegate6, updatedelegate6, prodNormalAttack3.Clear);
				}
				break;
			case BattleAttackKind.Bakurai:
			{
				if (_prodDepthChargeAttack == null)
				{
					_prodDepthChargeAttack = new ProdDepthChargeAttack();
				}
				Statement clsStatement9 = _clsStatement;
				ProdDepthChargeAttack prodDepthChargeAttack = _prodDepthChargeAttack;
				DelProdShellingAttack initdelegate9 = prodDepthChargeAttack.PlayAttack;
				ProdDepthChargeAttack prodDepthChargeAttack2 = _prodDepthChargeAttack;
				DelProdShellingUpdate updatedelegate9 = prodDepthChargeAttack2.Update;
				ProdDepthChargeAttack prodDepthChargeAttack3 = _prodDepthChargeAttack;
				clsStatement9.AddState(initdelegate9, updatedelegate9, prodDepthChargeAttack3.Clear);
				break;
			}
			case BattleAttackKind.Gyorai:
			{
				if (_prodTorpedoAttack == null)
				{
					_prodTorpedoAttack = new ProdTorpedoAttack();
				}
				Statement clsStatement7 = _clsStatement;
				ProdTorpedoAttack prodTorpedoAttack = _prodTorpedoAttack;
				DelProdShellingAttack initdelegate7 = prodTorpedoAttack.PlayAttack;
				ProdTorpedoAttack prodTorpedoAttack2 = _prodTorpedoAttack;
				DelProdShellingUpdate updatedelegate7 = prodTorpedoAttack2.Update;
				ProdTorpedoAttack prodTorpedoAttack3 = _prodTorpedoAttack;
				clsStatement7.AddState(initdelegate7, updatedelegate7, prodTorpedoAttack3.Clear);
				break;
			}
			case BattleAttackKind.AirAttack:
			{
				if (_prodAircraftAttack == null)
				{
					_prodAircraftAttack = new ProdAircraftAttack();
				}
				Statement clsStatement3 = _clsStatement;
				ProdAircraftAttack prodAircraftAttack = _prodAircraftAttack;
				DelProdShellingAttack initdelegate3 = prodAircraftAttack.PlayAttack;
				ProdAircraftAttack prodAircraftAttack2 = _prodAircraftAttack;
				DelProdShellingUpdate updatedelegate3 = prodAircraftAttack2.Update;
				ProdAircraftAttack prodAircraftAttack3 = _prodAircraftAttack;
				clsStatement3.AddState(initdelegate3, updatedelegate3, prodAircraftAttack3.Clear);
				break;
			}
			case BattleAttackKind.Laser:
			{
				if (_prodLaserAttack == null)
				{
					_prodLaserAttack = new ProdLaserAttack();
				}
				Statement clsStatement2 = _clsStatement;
				ProdLaserAttack prodLaserAttack = _prodLaserAttack;
				DelProdShellingAttack initdelegate2 = ((BaseProdAttackShelling)prodLaserAttack).PlayAttack;
				ProdLaserAttack prodLaserAttack2 = _prodLaserAttack;
				DelProdShellingUpdate updatedelegate2 = prodLaserAttack2.Update;
				ProdLaserAttack prodLaserAttack3 = _prodLaserAttack;
				clsStatement2.AddState(initdelegate2, updatedelegate2, prodLaserAttack3.Clear);
				break;
			}
			case BattleAttackKind.Renzoku:
			{
				if (_prodSuccessiveAttack == null)
				{
					_prodSuccessiveAttack = new ProdSuccessiveAttack();
				}
				Statement clsStatement8 = _clsStatement;
				ProdSuccessiveAttack prodSuccessiveAttack = _prodSuccessiveAttack;
				DelProdShellingAttack initdelegate8 = prodSuccessiveAttack.PlayAttack;
				ProdSuccessiveAttack prodSuccessiveAttack2 = _prodSuccessiveAttack;
				DelProdShellingUpdate updatedelegate8 = prodSuccessiveAttack2.Update;
				ProdSuccessiveAttack prodSuccessiveAttack3 = _prodSuccessiveAttack;
				clsStatement8.AddState(initdelegate8, updatedelegate8, prodSuccessiveAttack3.Clear);
				break;
			}
			case BattleAttackKind.Sp1:
			case BattleAttackKind.Sp2:
			case BattleAttackKind.Sp3:
			case BattleAttackKind.Sp4:
			{
				if (_prodObservedShellingAttack == null)
				{
					_prodObservedShellingAttack = new ProdObservedShellingAttack();
				}
				Statement clsStatement4 = _clsStatement;
				ProdObservedShellingAttack prodObservedShellingAttack = _prodObservedShellingAttack;
				DelProdShellingAttack initdelegate4 = prodObservedShellingAttack.PlayAttack;
				ProdObservedShellingAttack prodObservedShellingAttack2 = _prodObservedShellingAttack;
				DelProdShellingUpdate updatedelegate4 = prodObservedShellingAttack2.Update;
				ProdObservedShellingAttack prodObservedShellingAttack3 = _prodObservedShellingAttack;
				clsStatement4.AddState(initdelegate4, updatedelegate4, prodObservedShellingAttack3.Clear);
				break;
			}
			case BattleAttackKind.Syu_Rai:
			case BattleAttackKind.Rai_Rai:
			case BattleAttackKind.Syu_Syu_Fuku:
			case BattleAttackKind.Syu_Syu_Syu:
			{
				if (_prodTranscendenceAttack == null)
				{
					_prodTranscendenceAttack = new ProdTranscendenceAttack();
				}
				Statement clsStatement = _clsStatement;
				ProdTranscendenceAttack prodTranscendenceAttack = _prodTranscendenceAttack;
				DelProdShellingAttack initdelegate = prodTranscendenceAttack.PlayAttack;
				ProdTranscendenceAttack prodTranscendenceAttack2 = _prodTranscendenceAttack;
				DelProdShellingUpdate updatedelegate = prodTranscendenceAttack2.Update;
				ProdTranscendenceAttack prodTranscendenceAttack3 = _prodTranscendenceAttack;
				clsStatement.AddState(initdelegate, updatedelegate, prodTranscendenceAttack3.Clear);
				break;
			}
			}
			_clsStatement.Init(_clsHougekiModel, nCurrentShellingCnt, isNextAttack, _dicAttackFleet[(!hougekiModel.Attacker.IsFriend()) ? FleetType.Enemy : FleetType.Friend], OnShellingFinished);
		}

		public void Update()
		{
			if (_clsHougekiModel != null && _clsStatement.Update != null)
			{
				_clsStatement.Update();
			}
		}

		private void OnShellingFinished()
		{
			FleetType key = (!_clsHougekiModel.Attacker.IsFriend()) ? FleetType.Enemy : FleetType.Friend;
			if (!_dicAttackFleet[key])
			{
				_dicAttackFleet[key] = true;
			}
			_isFinished = true;
			Dlg.Call(ref _actCallback);
		}
	}
}
