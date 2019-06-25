using Common.Enum;
using KCV.SortieBattle;
using local.models;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	public class Events : IDisposable
	{
		private bool _isNormalBattle;

		private Action<bool> _actOnFinished;

		public Events()
		{
			_actOnFinished = null;
			_isNormalBattle = false;
		}

		public void Dispose()
		{
			Mem.Del(ref _isNormalBattle);
			Mem.Del(ref _actOnFinished);
		}

		public void Play(enumMapEventType iEventType, enumMapWarType iWarType, Action<bool> onFinished)
		{
			_actOnFinished = onFinished;
			switch (iEventType)
			{
			case enumMapEventType.NOT_USE:
				OnFinished();
				break;
			case enumMapEventType.None:
				OnFinished();
				break;
			case enumMapEventType.ItemGet:
			{
				UIMapManager uIMapManager5 = SortieMapTaskManager.GetUIMapManager();
				uIMapManager5.UpdateCellState(uIMapManager5.nextCell.cellModel.CellNo, isPassed: true);
				MapEventItemModel itemEvent = SortieBattleTaskManager.GetMapManager().GetItemEvent();
				EventItemGet eig = new EventItemGet(itemEvent);
				eig.PlayAnimation().Subscribe(delegate
				{
					eig.Dispose();
					Mem.Del(ref eig);
					OnFinished();
				});
				break;
			}
			case enumMapEventType.Uzushio:
			{
				UIMapManager uIMapManager4 = SortieMapTaskManager.GetUIMapManager();
				uIMapManager4.UpdateCellState(uIMapManager4.nextCell.cellModel.CellNo, isPassed: true);
				MapEventHappeningModel happeningEvent = SortieBattleTaskManager.GetMapManager().GetHappeningEvent();
				EventMailstrom em = new EventMailstrom(happeningEvent);
				em.PlayAnimation().Subscribe(delegate
				{
					em.Dispose();
					Mem.Del(ref em);
					OnFinished();
				});
				break;
			}
			case enumMapEventType.War_Normal:
			case enumMapEventType.War_Boss:
			{
				UIMapManager uIMapManager3 = SortieMapTaskManager.GetUIMapManager();
				uIMapManager3.UpdateCellState(uIMapManager3.nextCell.cellModel.CellNo, isPassed: true);
				Observable.FromCoroutine(() => EventEnemy(iEventType)).Subscribe();
				break;
			}
			case enumMapEventType.Stupid:
				Observable.FromCoroutine((UniRx.IObserver<bool> observer) => PlayStupid(observer, iWarType)).Subscribe(delegate
				{
					OnFinished();
				});
				break;
			case enumMapEventType.AirReconnaissance:
			{
				UIMapManager uIMapManager2 = SortieMapTaskManager.GetUIMapManager();
				uIMapManager2.UpdateCellState(uIMapManager2.nextCell.cellModel.CellNo, isPassed: true);
				MapEventAirReconnaissanceModel airReconnaissanceEvent = SortieBattleTaskManager.GetMapManager().GetAirReconnaissanceEvent();
				EventAirReconnaissance ear = new EventAirReconnaissance(airReconnaissanceEvent);
				ear.PlayAnimation().Subscribe(delegate
				{
					ear.Dispose();
					Mem.Del(ref ear);
					OnFinished();
				});
				break;
			}
			case enumMapEventType.PortBackEo:
			{
				UIMapManager uIMapManager = SortieMapTaskManager.GetUIMapManager();
				uIMapManager.UpdateCellState(uIMapManager.nextCell.cellModel.CellNo, isPassed: true);
				Observable.FromCoroutine((UniRx.IObserver<bool> observer) => PlayPortBackEo(observer)).Subscribe(delegate
				{
					OnFinished();
				});
				break;
			}
			}
		}

		private IEnumerator EventEnemy(enumMapEventType iEventType)
		{
			SortieBattleTaskManager.GetMapManager();
			UIMapManager uimm = SortieMapTaskManager.GetUIMapManager();
			UIAreaMapFrame uiamf = SortieMapTaskManager.GetUIAreaMapFrame();
			uimm.nextCell.PlayRipple(Color.red);
			yield return SortieMapTaskManager.GetUIMapManager().sortieShip.PlayExclamationPoint().StartAsCoroutine();
			if (SortieMapTaskManager.GetShortCutSwitch().isShortCut && SortieMapTaskManager.GetShortCutSwitch().isValid)
			{
				UIWobblingIcon uiwi2 = uimm.wobblingIcons.wobblingIcons[uimm.nextCell.cellModel.CellNo];
				if (uiwi2 != null)
				{
					bool isWait2 = true;
					uiwi2.Show().setOnComplete((Action)delegate
					{
                        isWait2 = false;
					});
					while (!isWait2)
					{
						yield return Observable.NextFrame(FrameCountType.EndOfFrame).StartAsCoroutine();
					}
					yield return new WaitForSeconds(1.5f);
				}
				SortieBattleTaskManager.ReqMode(SortieBattleMode.BattleCut);
				SortieMapTaskManager.GetUIAreaMapFrame().Hide();
				uimm.nextCell.StopRipple();
				if (uiwi2 != null)
				{
					uiwi2.Hide().setOnComplete((Action)delegate
					{
						UnityEngine.Object.Destroy(uiwi2.gameObject);
					});
				}
				yield return null;
				yield break;
			}
			UIWobblingIcon uiwi = uimm.wobblingIcons.wobblingIcons[uimm.nextCell.cellModel.CellNo];
			if (uiwi != null)
			{
				bool isWait = true;
				uiwi.Show().setOnComplete((Action)delegate
				{
                    throw new NotImplementedException("‚È‚É‚±‚ê");
                    // base._003CisWait_003E__6 = false;
				});
				while (!isWait)
				{
					yield return Observable.NextFrame(FrameCountType.EndOfFrame).StartAsCoroutine();
				}
				yield return new WaitForSeconds(1.5f);
			}
			_isNormalBattle = true;
			uiamf.ClearMessage();
			uimm.nextCell.StopRipple();
			OnFinished();
			yield return null;
		}

		private IEnumerator PlayStupid(UniRx.IObserver<bool> observer, enumMapWarType iWarType)
		{
			UIMapManager uimm = SortieMapTaskManager.GetUIMapManager();
			UIAreaMapFrame uiamf = SortieMapTaskManager.GetUIAreaMapFrame();
			UIMapManager uiMapManager = SortieMapTaskManager.GetUIMapManager();
			uimm.nextCell.PlayRipple(Color.red);
			uiMapManager.nextCell.SetPassedDefaultColor();
			yield return SortieMapTaskManager.GetUIMapManager().sortieShip.PlayExclamationPoint().StartAsCoroutine();
			uiamf.SetMessage(enumMapEventType.Stupid, iWarType);
			yield return new WaitForSeconds(1f);
			uimm.nextCell.StopRipple();
			uiMapManager.UpdateCellState(uiMapManager.nextCell.cellModel.CellNo, isPassed: true);
			uiamf.ClearMessage();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private IEnumerator PlayPortBackEo(UniRx.IObserver<bool> observer)
		{
			SortieBattleTaskManager.GetMapManager();
			UISortieShip uiss = SortieMapTaskManager.GetUIMapManager().sortieShip;
			MapEventItemModel meim = SortieBattleTaskManager.GetMapManager().GetItemEvent();
			yield return new WaitForSeconds(1.5f);
			uiss.PlayBalloon(meim, delegate
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // base.observer.OnNext(value: true);
				// base.observer.OnCompleted();
			});
		}

		private void OnFinished()
		{
			Dlg.Call(ref _actOnFinished, _isNormalBattle);
		}
	}
}
