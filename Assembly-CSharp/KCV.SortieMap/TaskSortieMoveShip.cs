using Common.Enum;
using KCV.SortieBattle;
using Librarys.State;
using local.managers;
using local.models;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	public class TaskSortieMoveShip : Task
	{
		private IDisposable _disShipMoveObserver;

		public TaskSortieMoveShip()
		{
			_disShipMoveObserver = null;
		}

		protected override void Dispose(bool isDisposing)
		{
			Mem.DelIDisposableSafe(ref _disShipMoveObserver);
		}

		protected override bool Init()
		{
			UISortieShipCharacter uIShipCharacter = SortieMapTaskManager.GetUIShipCharacter();
			uIShipCharacter.SetShipData(SortieBattleTaskManager.GetMapManager().Deck.GetFlagShip());
			TutorialModel tutorial = SortieBattleTaskManager.GetMapManager().UserInfo.Tutorial;
			if (SortieBattleTaskManager.GetMapManager().UserInfo.StartMapCount >= 5)
			{
				SingletonMonoBehaviour<TutorialGuideManager>.Instance.CheckAndShowFirstTutorial(tutorial, TutorialGuideManager.TutorialID.BattleShortCutInfo, null, delegate
				{
					_disShipMoveObserver = Observable.FromCoroutine((UniRx.IObserver<bool> observer) => ShipMove(observer)).Subscribe(delegate
					{
						SortieMapTaskManager.ReqMode(SortieMapTaskManagerMode.Event);
					});
				});
			}
			else
			{
				_disShipMoveObserver = Observable.FromCoroutine((UniRx.IObserver<bool> observer) => ShipMove(observer)).Subscribe(delegate
				{
					SortieMapTaskManager.ReqMode(SortieMapTaskManagerMode.Event);
				});
			}
			return true;
		}

		protected override bool UnInit()
		{
			Mem.DelIDisposableSafe(ref _disShipMoveObserver);
			return true;
		}

		protected override bool Update()
		{
			if (SortieMapTaskManager.GetMode() != SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF)
			{
				return (SortieMapTaskManager.GetMode() == SortieMapTaskManagerMode.SortieMapTaskManagerMode_ST) ? true : false;
			}
			return true;
		}

		private IEnumerator ShipMove(UniRx.IObserver<bool> observer)
		{
			MapManager manager = SortieBattleTaskManager.GetMapManager();
			UIMapManager uiMapManager = SortieMapTaskManager.GetUIMapManager();
			UISortieShip sortieShip = uiMapManager.sortieShip;
            UniRx.IObservable<Unit> underwayReplenishmentUnit = Observable.FromCoroutine(() => this.ChkUnderwayReplenishment(manager));
            UniRx.IObservable<Unit> compassUnit = Observable.FromCoroutine(() => this.ChkCompass(manager, uiMapManager, sortieShip));
            UniRx.IObservable<Unit> productionUnit = Observable.FromCoroutine(() => this.ChkProduction(manager, uiMapManager, sortieShip));
            UniRx.IObservable<Unit> synthesisUnit = Observable.SelectMany(other: Observable.FromCoroutine(() => this.ChkComment(manager, sortieShip)), source: underwayReplenishmentUnit.SelectMany(compassUnit).SelectMany(productionUnit));
			yield return synthesisUnit.StartAsCoroutine();
			CheckNextBossCell(manager);
			sortieShip.Move(uiMapManager.nextCell, delegate
			{
				uiMapManager.UpdateRouteState(uiMapManager.nextCell.cellModel.CellNo);
				observer.OnNext(value: true);
				observer.OnCompleted();
			});
			yield return null;
		}

		private IEnumerator ChkUnderwayReplenishment(MapManager manager)
		{
			if (manager.GetMapSupplyInfo() != null)
			{
				ProdUnderwayReplenishment pur = ProdUnderwayReplenishment.Instantiate(((Component)SortieMapTaskManager.GetPrefabFile().prefabProdUnderwayReplenishment).GetComponent<ProdUnderwayReplenishment>(), SortieMapTaskManager.GetSharedPlace(), manager.GetMapSupplyInfo());
				yield return pur.Play().StartAsCoroutine();
				Mem.DelComponentSafe(ref pur);
				yield break;
			}
			MapManager mm = SortieBattleTaskManager.GetMapManager();
			UISortieShipCharacter uissc = SortieMapTaskManager.GetUIShipCharacter();
			if (uissc.isInDisplay)
			{
				uissc.SetShipData(mm.Deck.GetFlagShip());
				uissc.Show(null);
				yield return Observable.Timer(TimeSpan.FromSeconds(2.0)).StartAsCoroutine();
			}
		}

		private IEnumerator ChkCompass(MapManager manager, UIMapManager uiManager, UISortieShip ship)
		{
			if (manager.hasCompass())
			{
				SortieMapTaskManager.GetUIAreaMapFrame().SetMessage("どこに進む？");
				UICompassManager uicm = UICompassManager.Instantiate(((Component)SortieMapTaskManager.GetPrefabFile().prefabUICompassManager).GetComponent<UICompassManager>(), SortieMapTaskManager.GetSharedPlace(), SortieBattleTaskManager.GetMapManager().CompassId, ship.transform, uiManager.nextCell.transform);
				bool isWait = false;
				uicm.Play(delegate
				{
                    isWait = true;
				});
				while (!isWait)
				{
					yield return new WaitForEndOfFrame();
				}
				Mem.DelComponentSafe(ref uicm);
			}
		}

		private IEnumerator ChkProduction(MapManager manager, UIMapManager uiManager, UISortieShip ship)
		{
			MapProductionKind production = manager.Production;
			if (production == MapProductionKind.WaterPlane && uiManager.nowCell != null)
			{
				bool isFinished = false;
				ship.PlayDetectionAircraft(uiManager.nowCell, uiManager.nextCell, delegate
				{
                    isFinished = true;
				});
				while (!isFinished)
				{
					yield return null;
				}
			}
		}

		private IEnumerator ChkComment(MapManager manager, UISortieShip ship)
		{
			if (manager.Comment != 0)
			{
				bool isFinished = false;
				ship.PlayBalloon(manager.Comment, delegate
				{
                    isFinished = true;
				});
				while (!isFinished)
				{
					yield return null;
				}
			}
		}

		private void CheckNextBossCell(MapManager manager)
		{
			UIShortCutSwitch shortCutSwitch = SortieMapTaskManager.GetShortCutSwitch();
			if (manager.NextCategory != enumMapEventType.War_Boss)
			{
				shortCutSwitch.SetIsValid(isValid: true, isAnimation: true);
			}
			else
			{
				shortCutSwitch.SetIsValid(manager.Map.ClearedOnce ? true : false, isAnimation: true);
			}
		}
	}
}
