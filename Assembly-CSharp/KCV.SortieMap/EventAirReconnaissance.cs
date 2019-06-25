using KCV.SortieBattle;
using local.models;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	public class EventAirReconnaissance : BaseEvent
	{
		private MapEventAirReconnaissanceModel _clsEventModel;

		public EventAirReconnaissance(MapEventAirReconnaissanceModel eventModel)
		{
			_clsEventModel = eventModel;
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del(ref _clsEventModel);
			base.Dispose(disposing);
		}

		protected override IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
		{
			UIMapManager uimm = SortieMapTaskManager.GetUIMapManager();
			UISortieShip uiss = uimm.sortieShip;
			UIAreaMapFrame uiamf = SortieMapTaskManager.GetUIAreaMapFrame();
			uiamf.SetMessage(_clsEventModel.AircraftType);
			Transform airRecPoint = null;
			if (uimm.airRecPoint.ContainsKey(uimm.nextCell.cellModel.CellNo))
			{
				airRecPoint = uimm.airRecPoint[uimm.nextCell.cellModel.CellNo];
			}
			bool isWait2 = true;
			uiss.PlayAirReconnaissance(_clsEventModel.AircraftType, uiss.transform, airRecPoint, delegate
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // base._003CisWait_003E__4 = false;
			});
			while (isWait2)
			{
				yield return null;
			}
			isWait2 = true;
			uiss.PlayBalloon(_clsEventModel, SortieBattleTaskManager.GetMapManager().GetItemEvent(), delegate
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // base._003CisWait_003E__4 = false;
			});
			while (isWait2)
			{
				yield return null;
			}
			uiamf.ClearMessage();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}
	}
}
