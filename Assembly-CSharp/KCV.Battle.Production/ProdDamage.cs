using Common.Enum;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UniRx;

namespace KCV.Battle.Production
{
	public class ProdDamage : IDisposable
	{
		private bool _isPlaying;

		private bool _isFinished;

		private Action _actCallback;

		private IBattlePhase _iBattlePhase;

		private Queue<ShipModel_Defender> _queFriedShipModel;

		private List<ShipModel_Defender> _listFriendShipModel;

		private List<ShipModel_Defender> _listEnemyShipModel;

		public bool isPlaying => _isPlaying;

		public bool isFinished => _isFinished;

		public int remainingCnt => _queFriedShipModel.Count;

		public ProdDamage()
		{
			_isPlaying = false;
			_isFinished = false;
			_actCallback = null;
			_iBattlePhase = null;
			_queFriedShipModel = new Queue<ShipModel_Defender>();
			_listFriendShipModel = new List<ShipModel_Defender>();
			_listEnemyShipModel = new List<ShipModel_Defender>();
		}

		public bool Init()
		{
			return true;
		}

		public bool UnInit()
		{
			_isPlaying = false;
			_isFinished = false;
			Mem.Del(ref _iBattlePhase);
			Mem.DelQueueSafe(ref _queFriedShipModel);
			Mem.DelListSafe(ref _listFriendShipModel);
			Mem.DelListSafe(ref _listEnemyShipModel);
			Mem.Del(ref _actCallback);
			return true;
		}

		public void Dispose()
		{
			UnInit();
		}

		public void Play(IBattlePhase iBattlePhase, Action callback)
		{
			List<ShipModel_Defender> list = new List<ShipModel_Defender>();
			_iBattlePhase = iBattlePhase;
			_actCallback = callback;
			_isPlaying = true;
			BattleTaskManager.GetBattleShips().UpdateDamageAll(iBattlePhase, isFriend: false);
			if (iBattlePhase.HasGekichinEvent())
			{
				iBattlePhase.GetGekichinShips().ForEach(delegate(ShipModel_Defender x)
				{
					_queFriedShipModel.Enqueue(x);
				});
				if (iBattlePhase.HasTaihaEvent())
				{
					_listFriendShipModel.AddRange(iBattlePhase.GetDefenders(is_friend: true, DamagedStates.Taiha));
				}
				if (iBattlePhase.HasChuhaEvent())
				{
					_listFriendShipModel.AddRange(iBattlePhase.GetDefenders(is_friend: true, DamagedStates.Tyuuha));
				}
				if (_listFriendShipModel.Count != 0)
				{
					Observable.Timer(TimeSpan.FromSeconds(1.5)).Subscribe(delegate
					{
						ProdDamage prodDamage = this;
						BattleShips ships = BattleTaskManager.GetBattleShips();
						_listFriendShipModel.ForEach(delegate(ShipModel_Defender x)
						{
							ships.dicFriendBattleShips[x.Index].UpdateDamage(x);
						});
					});
				}
				PlaySinking();
			}
			else if (iBattlePhase.HasTaihaEvent())
			{
				list.AddRange(iBattlePhase.GetDefenders(is_friend: true, DamagedStates.Taiha));
				if (iBattlePhase.HasChuhaEvent())
				{
					list.AddRange(iBattlePhase.GetDefenders(is_friend: true, DamagedStates.Tyuuha));
				}
				_queFriedShipModel = new Queue<ShipModel_Defender>();
				int num = 0;
				foreach (ShipModel_Defender item in list)
				{
					if (num >= 3)
					{
						break;
					}
					_queFriedShipModel.Enqueue(item);
					num++;
				}
				PlayHeavyDamage(DamagedStates.Taiha);
			}
			else if (iBattlePhase.HasChuhaEvent())
			{
				list.AddRange(iBattlePhase.GetDefenders(is_friend: true, DamagedStates.Tyuuha).ConvertAll((ShipModel_Defender s) => s));
				int num2 = 0;
				foreach (ShipModel_Defender item2 in list)
				{
					if (num2 >= 3)
					{
						break;
					}
					_queFriedShipModel.Enqueue(item2);
					num2++;
				}
				PlayHeavyDamage(DamagedStates.Tyuuha);
			}
			else
			{
				OnFinished();
			}
			list.Clear();
		}

		private void PlaySinking()
		{
			if (_queFriedShipModel.Count != 0)
			{
				ProdSinking prodSinking = BattleTaskManager.GetPrefabFile().prodSinking;
				prodSinking.SetSinkingData(_queFriedShipModel.Dequeue());
				prodSinking.Play(delegate
				{
					ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
					observerAction.Executions();
					Observable.NextFrame().Subscribe(delegate
					{
						PlaySinking();
					});
				});
			}
			else
			{
				OnFinished();
			}
		}

		private void PlayHeavyDamage(DamagedStates status)
		{
			List<ShipModel_Defender> list = new List<ShipModel_Defender>();
			list.AddRange(_queFriedShipModel.ToArray());
			_queFriedShipModel.Clear();
			ProdDamageCutIn.DamageCutInType damageCutInType = (status == DamagedStates.Taiha) ? ProdDamageCutIn.DamageCutInType.Heavy : ProdDamageCutIn.DamageCutInType.Moderate;
			ProdDamageCutIn prodDamageCutIn = BattleTaskManager.GetPrefabFile().prodDamageCutIn;
			prodDamageCutIn.SetShipData(list, damageCutInType);
			prodDamageCutIn.Play(damageCutInType, delegate
			{
				BattleShips battleShips = BattleTaskManager.GetBattleShips();
				battleShips.UpdateDamageAll(_iBattlePhase);
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Executions();
				OnFinished();
			});
		}

		private void OnFinished()
		{
			_isFinished = true;
			_isPlaying = false;
			if (_actCallback != null)
			{
				_actCallback();
			}
			Observable.NextFrame().Subscribe(delegate
			{
				Dispose();
			});
		}
	}
}
