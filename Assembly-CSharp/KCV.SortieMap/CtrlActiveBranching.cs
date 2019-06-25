using Common.Enum;
using KCV.SortieBattle;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UniRx;

namespace KCV.SortieMap
{
	public class CtrlActiveBranching : IDisposable
	{
		private List<CellModel> _listCellModel;

		private List<System.Tuple<int, UISortieMapCell>> _listUIMapCell;

		private Action<int> _actOnDecideMapCell;

		private int _nSelectIndex;

		private bool _isInputPossible;

		public CtrlActiveBranching(List<CellModel> cells, Action<int> onDecide)
		{
			Init(cells, onDecide);
		}

		public void Dispose()
		{
			Mem.DelListSafe(ref _listCellModel);
			Mem.DelListSafe(ref _listUIMapCell);
			Mem.Del(ref _actOnDecideMapCell);
		}

		private bool Init(List<CellModel> cells, Action<int> onDecide)
		{
			_nSelectIndex = 0;
			_isInputPossible = false;
			UIAreaMapFrame uiamf = SortieMapTaskManager.GetUIAreaMapFrame();
			_listCellModel = cells;
			_actOnDecideMapCell = onDecide;
			UISortieShip ship = SortieMapTaskManager.GetUIMapManager().sortieShip;
			ship.PlayBalloon(enumMapEventType.Stupid, enumMapWarType.Midnight, delegate
			{
				ship.ShowInputIcon();
				uiamf.SetMessage("艦隊の針路を選択できます。\n提督、どちらの針路を選択しますか？");
				ActiveTargetCell(cells);
				Observable.NextFrame(FrameCountType.EndOfFrame).Subscribe(delegate
				{
					_isInputPossible = true;
				});
			});
			return true;
		}

		private void ActiveTargetCell(List<CellModel> cells)
		{
			_listUIMapCell = new List<System.Tuple<int, UISortieMapCell>>();
			int cnt = 0;
			cells.ForEach(delegate(CellModel x)
			{
				UISortieMapCell uISortieMapCell = SortieMapTaskManager.GetUIMapManager().cells[x.CellNo];
				uISortieMapCell.isActiveBranchingTarget = true;
				uISortieMapCell.SetOnDecideActiveBranchingTarget(cnt, OnActive, OnDecideMapCell);
				uISortieMapCell.isFocus2ActiveBranching = ((_nSelectIndex == cnt) ? true : false);
				_listUIMapCell.Add(new System.Tuple<int, UISortieMapCell>(cnt, uISortieMapCell));
				cnt++;
			});
		}

		public bool Update()
		{
			if (!_isInputPossible)
			{
				return true;
			}
			KeyControl keyControl = SortieBattleTaskManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.LEFT))
			{
				PreparaNext(isFoward: false);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
			{
				PreparaNext(isFoward: true);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				OnDecideMapCell(_listUIMapCell[_nSelectIndex].Item2);
			}
			return true;
		}

		private void PreparaNext(bool isFoward)
		{
			int nSelectIndex = _nSelectIndex;
			_nSelectIndex = Mathe.NextElementRev(_nSelectIndex, 0, _listCellModel.Count - 1, isFoward);
			if (nSelectIndex != _nSelectIndex)
			{
				ChangeFocus(_nSelectIndex);
			}
		}

		private void ChangeFocus(int nIndex)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
			_listUIMapCell.ForEach(delegate(System.Tuple<int, UISortieMapCell> x)
			{
				x.Item2.isFocus2ActiveBranching = ((x.Item1 == nIndex) ? true : false);
			});
		}

		private void OnActive(int nIndex)
		{
			if (_nSelectIndex != nIndex)
			{
				_nSelectIndex = nIndex;
				ChangeFocus(_nSelectIndex);
			}
		}

		private void OnDecideMapCell(UISortieMapCell cell)
		{
			_isInputPossible = false;
			_listUIMapCell.ForEach(delegate(System.Tuple<int, UISortieMapCell> x)
			{
				x.Item2.isActiveBranchingTarget = false;
			});
			UISortieShip sortieShip = SortieMapTaskManager.GetUIMapManager().sortieShip;
			sortieShip.HideInputIcon();
			UIAreaMapFrame uIAreaMapFrame = SortieMapTaskManager.GetUIAreaMapFrame();
			uIAreaMapFrame.ClearMessage();
			Dlg.Call(ref _actOnDecideMapCell, cell.cellModel.CellNo);
		}
	}
}
