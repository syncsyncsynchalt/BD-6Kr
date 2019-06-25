using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.managers;
using local.models.battle;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleMapOpen : BaseBattleTask
	{
		private BattleResultModel _clsResultModel;

		private MapManager _clsMapManager;

		private ProdMapOpen _prodMapOpen;

		private ProdMapPoint _prodMapPoint;

		private ProdMapClear _prodMapClear;

		private ProdThalassocracy _prodThalassocracy;

		private ProdShortRewardGet _prodShortRewardGet;

		private KeyControl _clsInput;

		protected override bool Init()
		{
			_clsResultModel = BattleTaskManager.GetBattleManager().GetBattleResult();
			_clsMapManager = SortieBattleTaskManager.GetMapManager();
			_clsInput = BattleTaskManager.GetKeyControl();
			_clsState = new StatementMachine();
			if (BattleTaskManager.GetRootType() == Generics.BattleRootType.Rebellion)
			{
				if (_clsMapManager.IsNextFinal())
				{
					if (_clsResultModel.WinRank == BattleWinRankKinds.B || _clsResultModel.WinRank == BattleWinRankKinds.A || _clsResultModel.WinRank == BattleWinRankKinds.S)
					{
						_clsState.AddState(_initThalassocracyProd, _updateThalassocracyProd);
					}
					else
					{
						_clsState.AddState(_initChkNextCell, _updateChkNextCell);
					}
				}
				else
				{
					_clsState.AddState(_initChkNextCell, _updateChkNextCell);
				}
			}
			else if (_clsResultModel.FirstAreaClear && _clsResultModel.FirstClear)
			{
				_clsState.AddState(_initThalassocracyProd, _updateThalassocracyProd);
			}
			else if (!_clsResultModel.FirstAreaClear && _clsResultModel.FirstClear)
			{
				_clsState.AddState(_initMapClearProd, _updateMapClearProd);
			}
			else if (!_clsResultModel.FirstClear && _clsResultModel.NewOpenMapIDs.Length > 0)
			{
				_clsState.AddState(_initMapOpenProd, _updateMapOpenProd);
			}
			else if (_clsResultModel.SPoint > 0)
			{
				_clsState.AddState(_initStrategyPointProd, _updateStrategyPointProd);
			}
			else if (_clsResultModel.GetAreaRewardItems() != null)
			{
				_clsState.AddState(InitShortRewardGet, UpdateShortRewardGet);
			}
			else
			{
				_clsState.AddState(_initChkNextCell, _updateChkNextCell);
			}
			return true;
		}

		protected override bool UnInit()
		{
			if (_prodMapOpen != null)
			{
				_prodMapOpen.Discard();
			}
			if (_prodMapPoint != null)
			{
				_prodMapPoint.Discard();
			}
			if (_prodMapClear != null)
			{
				_prodMapClear.Discard();
			}
			if (_prodThalassocracy != null)
			{
				_prodThalassocracy.Discard();
			}
			_prodMapOpen = null;
			_prodMapClear = null;
			_prodThalassocracy = null;
			_clsState.Clear();
			return true;
		}

		protected override bool Update()
		{
			_clsState.OnUpdate(Time.deltaTime);
			return ChkChangePhase(BattlePhase.MapOpen);
		}

		private bool _initThalassocracyProd(object data)
		{
			Observable.FromCoroutine((IObserver<bool> observer) => CreateThalassocracy(observer)).Subscribe(delegate
			{
				_prodThalassocracy.Play(_onThalassocracyProdFinished, BattleTaskManager.GetRootType(), BattleTaskManager.GetBattleManager().AreaName);
			});
			return false;
		}

		private IEnumerator CreateThalassocracy(IObserver<bool> observer)
		{
			_prodThalassocracy = ProdThalassocracy.Instantiate(PrefabFile.Load<ProdThalassocracy>(PrefabFileInfos.Thalassocracy), BattleTaskManager.GetBattleCameras().cutInCamera.transform, _clsInput, SortieBattleTaskManager.GetMapManager(), _clsResultModel, BattleTaskManager.GetBattleManager().Ships_f, 120);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateThalassocracyProd(object data)
		{
			return _prodThalassocracy.Run();
		}

		private void _onThalassocracyProdFinished()
		{
			if (_clsResultModel.NewOpenMapIDs.Length > 0)
			{
				_clsState.AddState(_initMapOpenProd, _updateMapOpenProd);
			}
			else if (_clsResultModel.SPoint > 0)
			{
				_clsState.AddState(_initStrategyPointProd, _updateStrategyPointProd);
			}
			else if (_clsResultModel.GetAreaRewardItems() != null)
			{
				_clsState.AddState(InitShortRewardGet, UpdateShortRewardGet);
			}
			else
			{
				_clsState.AddState(_initChkNextCell, _updateChkNextCell);
			}
			_prodThalassocracy.Discard();
		}

		private bool _initMapClearProd(object data)
		{
			Observable.FromCoroutine((IObserver<bool> observer) => createMapClear(observer)).Subscribe(delegate
			{
				_prodMapClear.Play(_onMapClearProdFinished);
			});
			return false;
		}

		private IEnumerator createMapClear(IObserver<bool> observer)
		{
			_prodMapClear = ProdMapClear.Instantiate(PrefabFile.Load<ProdMapClear>(PrefabFileInfos.MapClear), BattleTaskManager.GetBattleCameras().cutInCamera.transform, _clsInput, BattleTaskManager.GetBattleManager().Ships_f[0], 120);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateMapClearProd(object data)
		{
			return _prodMapClear.Run();
		}

		private void _onMapClearProdFinished()
		{
			if (_clsResultModel.NewOpenMapIDs.Length > 0)
			{
				_clsState.AddState(_initMapOpenProd, _updateMapOpenProd);
			}
			else if (_clsResultModel.SPoint > 0)
			{
				_clsState.AddState(_initStrategyPointProd, _updateStrategyPointProd);
			}
			else if (_clsResultModel.GetAreaRewardItems() != null)
			{
				_clsState.AddState(InitShortRewardGet, UpdateShortRewardGet);
			}
			else
			{
				_clsState.AddState(_initChkNextCell, _updateChkNextCell);
			}
			_prodMapClear.Discard();
		}

		private bool _initMapOpenProd(object data)
		{
			Observable.FromCoroutine((IObserver<bool> observer) => createMapOpen(observer)).Subscribe(delegate
			{
				_prodMapOpen.Play(_onMapOpenProdFinished);
			});
			return false;
		}

		private IEnumerator createMapOpen(IObserver<bool> observer)
		{
			_prodMapOpen = ProdMapOpen.Instantiate(PrefabFile.Load<ProdMapOpen>(PrefabFileInfos.MapOpen), _clsResultModel, BattleTaskManager.GetBattleCameras().cutInCamera.transform, _clsInput, _clsMapManager, 120);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateMapOpenProd(object data)
		{
			return _prodMapOpen != null && _prodMapOpen.Run();
		}

		private void _onMapOpenProdFinished()
		{
			if (_clsResultModel.SPoint > 0)
			{
				_clsState.AddState(_initStrategyPointProd, _updateStrategyPointProd);
			}
			else if (_clsResultModel.GetAreaRewardItems() != null)
			{
				_clsState.AddState(InitShortRewardGet, UpdateShortRewardGet);
			}
			else
			{
				_clsState.AddState(_initChkNextCell, _updateChkNextCell);
			}
		}

		private bool _initStrategyPointProd(object data)
		{
			if (_clsResultModel.SPoint <= 0)
			{
				_onStrategyPointProdFinished();
			}
			else
			{
				Observable.FromCoroutine((IObserver<bool> observer) => createStrategyPoint(observer)).Subscribe(delegate
				{
					_prodMapPoint.Play(_onStrategyPointProdFinished);
				});
			}
			return false;
		}

		private IEnumerator createStrategyPoint(IObserver<bool> observer)
		{
			_prodMapPoint = ProdMapPoint.Instantiate(Resources.Load<ProdMapPoint>("Prefabs/Battle/Production/MapOpen/ProdMapOpenPoint"), BattleTaskManager.GetBattleCameras().cutInCamera.transform, _clsResultModel.SPoint);
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private bool _updateStrategyPointProd(object data)
		{
			return false;
		}

		private void _onStrategyPointProdFinished()
		{
			if (_clsResultModel.GetAreaRewardItems() != null)
			{
				_clsState.AddState(InitShortRewardGet, UpdateShortRewardGet);
			}
			else
			{
				_clsState.AddState(_initChkNextCell, _updateChkNextCell);
			}
		}

		private bool InitShortRewardGet(object data)
		{
			Observable.FromCoroutine((IObserver<bool> observer) => CreateShortRewardGet(observer)).Subscribe(delegate(bool _)
			{
				if (!_)
				{
					OnShortRewardGetFinished();
				}
				else
				{
					_prodShortRewardGet.Play(delegate
					{
						OnShortRewardGetFinished();
					});
				}
			});
			return false;
		}

		private bool UpdateShortRewardGet(object data)
		{
			return true;
		}

		private IEnumerator CreateShortRewardGet(IObserver<bool> observer)
		{
			if (_clsResultModel.GetAreaRewardItems() == null)
			{
				observer.OnNext(value: false);
				observer.OnCompleted();
				yield break;
			}
			_prodShortRewardGet = ProdShortRewardGet.Instantiate(Resources.Load<ProdShortRewardGet>("Prefabs/Battle/Production/MapOpen/ProdShortRewardGet"), BattleTaskManager.GetBattleCameras().cutInCamera.transform, _clsResultModel.GetAreaRewardItems());
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private void OnShortRewardGetFinished()
		{
			Mem.DelComponentSafe(ref _prodShortRewardGet);
			_clsState.AddState(_initChkNextCell, _updateChkNextCell);
		}

		private bool _initChkNextCell(object data)
		{
			return false;
		}

		private bool _updateChkNextCell(object data)
		{
			if (!_clsMapManager.IsNextFinal())
			{
				if (BattleTaskManager.GetRootType() == Generics.BattleRootType.Rebellion && BattleTaskManager.GetBattleManager().ChangeableDeck && BattleTaskManager.GetBattleManager().Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && !BattleTaskManager.GetBattleManager().Ships_f[0].HasRecoverYouin() && !BattleTaskManager.GetBattleManager().Ships_f[0].HasRecoverMegami())
				{
					BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawal);
					return true;
				}
				if (BattleTaskManager.GetBattleManager().Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && !ShipUtils.HasRepair(_clsResultModel.Ships_f[0]))
				{
					BattleTaskManager.ReqPhase(BattlePhase.FlagshipWreck);
					return true;
				}
				BattleTaskManager.ReqPhase(BattlePhase.EscortShipEvacuation);
				return true;
			}
			if (SingletonMonoBehaviour<FadeCamera>.Instance != null)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					RetentionData.SetData(BattleUtils.GetRetentionDataMapOpen(SortieBattleTaskManager.GetMapManager(), _clsResultModel));
					SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
					Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
				});
			}
			return true;
		}
	}
}
