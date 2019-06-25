using local.models;
using System;
using System.Collections;
using UniRx;

namespace KCV.SortieMap
{
	public class EventMailstrom : BaseEvent
	{
		private MapEventHappeningModel _clsEventHappeningModel;

		public EventMailstrom(MapEventHappeningModel eventHappeningModel)
		{
			_clsEventHappeningModel = eventHappeningModel;
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del(ref _clsEventHappeningModel);
			base.Dispose(disposing);
		}

		protected override IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
		{
			UIMapManager uimm = SortieMapTaskManager.GetUIMapManager();
			UIAreaMapFrame uiamf = SortieMapTaskManager.GetUIAreaMapFrame();
			UISortieShipCharacter uissc = SortieMapTaskManager.GetUIShipCharacter();
			bool isWait = true;
			uissc.SetInDisplayNextMove(isInDisplay: false);
			uiamf.SetMessage("艦隊の前方にうずしおが発生しました！");
			yield return Observable.NextFrame(FrameCountType.EndOfFrame).StartAsCoroutine();
			uimm.nextCell.PlayMailstrom(uimm.sortieShip, _clsEventHappeningModel, delegate
			{
				base._003CisWait_003E__3 = false;
			});
			while (isWait)
			{
				yield return null;
			}
			uiamf.SetMessage(string.Format((!_clsEventHappeningModel.Dentan) ? "{0}x{1}を\n落としてしまいました…。" : "{0}x{1}を\n落としてしまいました…。\n(電探が役立って、被害を抑えられた！)", SortieUtils.ConvertMatCategory2String(_clsEventHappeningModel.Material), _clsEventHappeningModel.Count));
			yield return Observable.Timer(TimeSpan.FromSeconds(2.0)).StartAsCoroutine();
			uiamf.ClearMessage();
			observer.OnNext(value: true);
			observer.OnCompleted();
		}
	}
}
