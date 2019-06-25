using Common.Enum;
using KCV.Battle;
using KCV.Battle.Production;
using KCV.SortieBattle;
using Librarys.State;
using local.managers;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.SortieMap
{
	public class TaskSortieResult : Task
	{
		private CtrlSortieResult _ctrlSortieResult;

		private ProdMapOpen _prodMapOpen;

		private ProdMapPoint _prodStrategyPoint;

		private StatementMachine _clsState;

		private BattleShutter _uiBattleShutter;

		protected override bool Init()
		{
			_clsState = new StatementMachine();
			UIAreaMapFrame uIAreaMapFrame = SortieMapTaskManager.GetUIAreaMapFrame();
			uIAreaMapFrame.Hide().setOnComplete((Action)delegate
			{
				_uiBattleShutter = BattleShutter.Instantiate(((Component)SortieBattleTaskManager.GetSortieBattlePrefabFile().prefabUIBattleShutter).GetComponent<BattleShutter>(), SortieMapTaskManager.GetSharedPlace(), 20);
				_uiBattleShutter.Init(BaseShutter.ShutterMode.Open);
				_uiBattleShutter.ReqMode(BaseShutter.ShutterMode.Close, delegate
				{
					ProdSortieEnd prodSortieEnd = ProdSortieEnd.Instantiate(((Component)SortieMapTaskManager.GetPrefabFile().prodSortieEnd).GetComponent<ProdSortieEnd>(), SortieMapTaskManager.GetSharedPlace());
					prodSortieEnd.Play(delegate
					{
						_clsState.AddState(InitResult, UpdateResult);
					});
				});
			});
			return true;
		}

		protected override bool UnInit()
		{
			Mem.Del(ref _ctrlSortieResult);
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			Mem.Del(ref _clsState);
			Mem.Del(ref _uiBattleShutter);
			return true;
		}

		protected override bool Update()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			if (SortieMapTaskManager.GetMode() != SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF)
			{
				return (SortieMapTaskManager.GetMode() == SortieMapTaskManagerMode.Result) ? true : false;
			}
			return true;
		}

		private void OnFinished()
		{
			Hashtable hashtable = new Hashtable();
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			if (mapManager is SortieMapManager)
			{
				hashtable.Add("sortieMapManager", mapManager);
				hashtable.Add("rootType", 1);
				hashtable.Add("shipRecoveryType", ShipRecoveryType.None);
			}
			else
			{
				hashtable.Add("rebellionMapManager", mapManager);
				hashtable.Add("rootType", 1);
				hashtable.Add("shipRecoveryType", ShipRecoveryType.None);
			}
			if (SingletonMonoBehaviour<FadeCamera>.Instance != null)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(isActive: true);
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
					Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
				});
			}
			else
			{
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
				Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
			}
		}

		private bool InitResult(object data)
		{
			_ctrlSortieResult = CtrlSortieResult.Instantiate(((Component)SortieMapTaskManager.GetPrefabFile().prefabCtrlSortieResult).GetComponent<CtrlSortieResult>(), SortieMapTaskManager.GetSharedPlace(), SortieBattleTaskManager.GetMapManager().Items, delegate
			{
				_clsState.AddState(InitReward, UpdateReward);
			});
			return false;
		}

		private bool UpdateResult(object data)
		{
			if (_ctrlSortieResult != null)
			{
				return _ctrlSortieResult.Run();
			}
			return false;
		}

		private bool InitReward(object data)
		{
			_clsState.Clear();
			SortieBattleTaskManager.GetMapManager();
			_clsState.AddState(InitAirRecSucccessOrFailure, UpdateAirRecSuccessOrFailure);
			return false;
		}

		private bool UpdateReward(object data)
		{
			return true;
		}

		private bool InitAirRecSucccessOrFailure(object data)
		{
			_clsState.AddState(InitMapOpen, UpdateMapOpen);
			return false;
		}

		private bool UpdateAirRecSuccessOrFailure(object data)
		{
			return true;
		}

		private bool InitMapOpen(object data)
		{
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			if (mapManager.GetNewOpenMapIDs() != null)
			{
				_prodMapOpen = ProdMapOpen.Instantiate(PrefabFile.Load<ProdMapOpen>(PrefabFileInfos.MapOpen), mapManager.GetNewOpenAreaIDs(), mapManager.GetNewOpenMapIDs(), SortieMapTaskManager.GetSharedPlace(), SortieBattleTaskManager.GetKeyControl(), 120);
				_prodMapOpen.Play(OnMapOpenFinished);
			}
			else
			{
				OnMapOpenFinished();
			}
			return false;
		}

		private bool UpdateMapOpen(object data)
		{
			return _prodMapOpen != null && _prodMapOpen.Run();
		}

		private void OnMapOpenFinished()
		{
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			if (mapManager.GetNewOpenMapIDs() != null && mapManager.GetSPoint() > 0)
			{
				_clsState.AddState(InitGetSPoint, UpdateGetSPoint);
			}
			else
			{
				OnFinished();
			}
		}

		private bool InitGetSPoint(object data)
		{
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			if (mapManager.GetNewOpenMapIDs() != null && mapManager.GetSPoint() > 0)
			{
				_prodStrategyPoint = ProdMapPoint.Instantiate(Resources.Load<ProdMapPoint>("Prefabs/Battle/Production/MapOpen/ProdMapOpenPoint"), SortieMapTaskManager.GetSharedPlace(), mapManager.GetSPoint());
				_prodStrategyPoint.Play(OnFinished);
			}
			else
			{
				OnFinished();
			}
			return false;
		}

		private bool UpdateGetSPoint(object data)
		{
			return true;
		}
	}
}
