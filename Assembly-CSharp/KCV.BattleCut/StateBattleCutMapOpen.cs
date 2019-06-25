using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.managers;
using local.models.battle;
using Server_Models;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutMapOpen : BaseBattleCutState
	{
		private BattleResultModel _clsResultModel;

		private MapManager _clsMapManger;

		private StatementMachine _clsState;

		private ProdMapOpen _prodMapOpen;

		private ProdMapClear _prodMapClear;

		private ProdThalassocracy _prodThalassocracy;

		private ProdShortRewardGet _prodShortRewardGet;

		private KeyControl _clsInput;

		public override bool Init(object data)
		{
			_clsResultModel = BattleCutManager.GetBattleManager().GetBattleResult();
			_clsMapManger = BattleCutManager.GetMapManager();
			_clsInput = BattleCutManager.GetKeyControl();
			_clsState = new StatementMachine();
			if (BattleCutManager.GetBattleType() == Generics.BattleRootType.Rebellion)
			{
				if (_clsMapManger.IsNextFinal())
				{
					if (_clsResultModel.WinRank == BattleWinRankKinds.B || _clsResultModel.WinRank == BattleWinRankKinds.A || _clsResultModel.WinRank == BattleWinRankKinds.S)
					{
						_clsState.AddState(_initThalassocracyProd, _updateThalassocracyProd);
					}
					else
					{
						ChkNextCell();
					}
				}
				else
				{
					ChkNextCell();
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
				_clsState.AddState(InitMapOpen, UpdateMapOpen);
			}
			else if (_clsResultModel.GetAreaRewardItems() != null)
			{
				_clsState.AddState(InitShortRewardGet, UpdateShortRewardGet);
			}
			else
			{
				ChkNextCell();
			}
			return false;
		}

		public override bool Terminate(object data)
		{
			return base.Terminate(data);
		}

		public override bool Run(object data)
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return IsCheckPhase(BattleCutPhase.MapOpen);
		}

		private bool _initThalassocracyProd(object data)
		{
			_prodThalassocracy = ProdThalassocracy.Instantiate(PrefabFile.Load<ProdThalassocracy>(PrefabFileInfos.Thalassocracy), BattleCutManager.GetSharedPlase(), _clsInput, SortieBattleTaskManager.GetMapManager(), _clsResultModel, BattleCutManager.GetBattleManager().Ships_f, 120);
			_prodThalassocracy.Play(_onThalassocracyProdFinished, BattleCutManager.GetBattleType(), BattleCutManager.GetBattleManager().Map.Name);
			return false;
		}

		private bool _updateThalassocracyProd(object data)
		{
			return _prodThalassocracy.Run();
		}

		private void _onThalassocracyProdFinished()
		{
			if (_clsResultModel.NewOpenMapIDs.Length > 0)
			{
				_clsState.AddState(InitMapOpen, UpdateMapOpen);
			}
			else
			{
				ChkNextCell();
			}
			_prodThalassocracy.Discard();
		}

		private bool _initMapClearProd(object data)
		{
			_prodMapClear = ProdMapClear.Instantiate(PrefabFile.Load<ProdMapClear>(PrefabFileInfos.MapClear), BattleCutManager.GetSharedPlase(), _clsInput, BattleCutManager.GetBattleManager().Ships_f[0], 120);
			_prodMapClear.Play(_onMapClearProdFinished);
			return false;
		}

		private bool _updateMapClearProd(object data)
		{
			return _prodMapClear.Run();
		}

		private void _onMapClearProdFinished()
		{
			if (_clsResultModel.NewOpenMapIDs.Length > 0)
			{
				_clsState.AddState(InitMapOpen, UpdateMapOpen);
			}
			else
			{
				ChkNextCell();
			}
			_prodMapClear.Discard();
		}

		private bool InitMapOpen(object data)
		{
			_prodMapOpen = ProdMapOpen.Instantiate(PrefabFile.Load<ProdMapOpen>(PrefabFileInfos.MapOpen), _clsResultModel, BattleCutManager.GetSharedPlase(), BattleCutManager.GetKeyControl(), BattleCutManager.GetMapManager(), 120);
			_prodMapOpen.Play(delegate
			{
				_clsState.AddState(InitShortRewardGet, UpdateShortRewardGet);
			});
			return false;
		}

		private bool UpdateMapOpen(object data)
		{
			return _prodMapOpen.Run();
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
			_prodShortRewardGet = ProdShortRewardGet.Instantiate(Resources.Load<ProdShortRewardGet>("Prefabs/Battle/Production/MapOpen/ProdShortRewardGet"), BattleCutManager.GetSharedPlase(), _clsResultModel.GetAreaRewardItems());
			yield return new WaitForEndOfFrame();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private void OnShortRewardGetFinished()
		{
			Mem.DelComponentSafe(ref _prodShortRewardGet);
			ChkNextCell();
		}

		private void ChkNextCell()
		{
			if (!_clsMapManger.IsNextFinal())
			{
				if (BattleCutManager.GetBattleType() == Generics.BattleRootType.Rebellion && BattleCutManager.GetBattleManager().ChangeableDeck && BattleCutManager.GetBattleManager().Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && !BattleCutManager.GetBattleManager().Ships_f[0].HasRecoverMegami() && !BattleCutManager.GetBattleManager().Ships_f[0].HasRecoverYouin())
				{
					BattleCutManager.ReqPhase(BattleCutPhase.AdvancingWithdrawal);
				}
				else if (_clsResultModel.Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && !ShipUtils.HasRepair(_clsResultModel.Ships_f[0]))
				{
					BattleCutManager.ReqPhase(BattleCutPhase.FlagshipWreck);
				}
				else
				{
					BattleCutManager.ReqPhase(BattleCutPhase.EscortShipEvacuation);
				}
			}
			else if (SingletonMonoBehaviour<FadeCamera>.Instance != null)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(isActive: true);
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = BattleDefines.SOUND_KEEP.BGMVolume;
					SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = BattleDefines.SOUND_KEEP.BGMVolume;
					Mst_DataManager.Instance.PurgeUIBattleMaster();
					RetentionData.SetData(BattleUtils.GetRetentionDataMapOpen(BattleCutManager.GetMapManager(), _clsResultModel));
					SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
					Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
				});
			}
		}
	}
}
