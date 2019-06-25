using KCV.SortieBattle;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace KCV.SortieMap
{
	public class EventItemGet : BaseEvent
	{
		private MapEventItemModel _clsEventItemModel;

		public EventItemGet(MapEventItemModel eventItemModel)
		{
			_clsEventItemModel = eventItemModel;
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del(ref _clsEventItemModel);
			base.Dispose(disposing);
		}

        //protected override IEnumerator AnimationObserver(IObserver<bool> observer)
        protected IEnumerator AnimationObserver(UniRx.IObserver<bool> observer)
        {
            MapManager mm = SortieBattleTaskManager.GetMapManager();
			UIMapManager uimm = SortieMapTaskManager.GetUIMapManager();
			UIAreaMapFrame uiamf = SortieMapTaskManager.GetUIAreaMapFrame();
			UISortieShipCharacter uissc = SortieMapTaskManager.GetUIShipCharacter();
			bool isShipWait = true;
			bool isItemGetWait = true;
			string empty = string.Empty;
			string str = (!_clsEventItemModel.IsMaterial()) ? $"{SortieUtils.ConvertItem2String(_clsEventItemModel.ItemID)}x{_clsEventItemModel.Count.ToString()}\nを入手しました！" : $"{SortieUtils.ConvertMatCategory2String(_clsEventItemModel.MaterialCategory)}x{_clsEventItemModel.Count.ToString()}\nを入手しました！";
			uiamf.SetMessage(str);
			yield return null;
			uissc.SetShipData(GetTargetShip(mm.Deck));
			uissc.ShowInItemGet(delegate
			{
                throw new NotImplementedException("なにこれ");
                // base._003CisShipWait_003E__4 = false;
			});
			uissc.SetInDisplayNextMove(isInDisplay: false);
			uimm.sortieShip.PlayGetMaterialOrItem(_clsEventItemModel, delegate
			{
                throw new NotImplementedException("なにこれ");
                // base._003Cuiamf_003E__2.ClearMessage();
				// base._003CisItemGetWait_003E__5 = false;
			});
			while (isShipWait || isItemGetWait)
			{
				yield return null;
			}
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		private ShipModel GetTargetShip(DeckModel model)
		{
			List<ShipModel> list = (from x in model.GetShips()
				where !x.IsEscaped()
				select x).ToList();
			return list[XorRandom.GetILim(0, list.Count - 1)];
		}
	}
}
