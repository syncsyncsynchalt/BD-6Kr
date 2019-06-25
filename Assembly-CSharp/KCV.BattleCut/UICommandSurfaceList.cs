using Common.Enum;
using KCV.Generic;
using KCV.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.BattleCut
{
	public class UICommandSurfaceList : MonoBehaviour
	{
		[Serializable]
		private struct Frame : IDisposable
		{
			public UILabelButton battleStart;

			public UITexture bottomLine;

			public UITexture topLine;

			public UILabel label;

			public UILabel detectionResult;

			public void Dispose()
			{
				Mem.Del(ref battleStart);
				Mem.Del(ref bottomLine);
				Mem.Del(ref topLine);
				Mem.Del(ref label);
				Mem.Del(ref detectionResult);
			}
		}

		[SerializeField]
		private Transform _prefabUILabelButton;

		[SerializeField]
		private Transform _uiSurfaceAnchor;

		[SerializeField]
		private Frame _strFrame;

		private List<ISelectedObject<int>> _listISelectSurface;

		private int _nIndex;

		private UIWidget _uiWidget;

		private Action<UICommandLabelButton> _actOnSelectedSurface;

		private Predicate<List<BattleCommand>> _preOnDecideCommand;

		public int index
		{
			get
			{
				return _nIndex;
			}
			private set
			{
				_nIndex = value;
			}
		}

		public UICommandLabelButton selectedSurface
		{
			get
			{
				if (_listISelectSurface[index] is UICommandLabelButton)
				{
					return _listISelectSurface[index] as UICommandLabelButton;
				}
				return null;
			}
		}

		public bool isColliderEnabled
		{
			set
			{
				_listISelectSurface.ForEach(delegate(ISelectedObject<int> x)
				{
					x.toggle.enabled = (x.isValid && value);
				});
			}
		}

		public bool Init(List<BattleCommand> commands, Action<UICommandLabelButton> onSelectedSurface, Predicate<List<BattleCommand>> onDecideCommand)
		{
			SetDetectionResult(BattleCutManager.GetBattleManager().GetSakutekiData().value_f);
			index = 0;
			_actOnSelectedSurface = onSelectedSurface;
			_preOnDecideCommand = onDecideCommand;
			_listISelectSurface = new List<ISelectedObject<int>>();
			CreateCommandLabel(commands);
			int firstFocusIndex = CheckBattleStartState() ? commands.Count : 0;
			index = firstFocusIndex;
			_listISelectSurface.ForEach(delegate(ISelectedObject<int> x)
			{
				x.isFocus = ((x.index == firstFocusIndex) ? true : false);
			});
			return true;
		}

		private void CreateCommandLabel(List<BattleCommand> presetList)
		{
			int num = 0;
			foreach (BattleCommand preset in presetList)
			{
				_listISelectSurface.Add(UICommandLabelButton.Instantiate(((Component)_prefabUILabelButton).GetComponent<UICommandLabelButton>(), _uiSurfaceAnchor.transform, Vector3.zero, num, preset, CheckBattleStartState));
				_listISelectSurface[num].toggle.transform.localPosition = Vector3.down * (50 * num);
				num++;
			}
			UILabelButton battleStart = _strFrame.battleStart;
			battleStart.Init(presetList.Count, isValid: false, KCVColor.ConvertColor(170f, 170f, 170f, 255f), KCVColor.ConvertColor(170f, 170f, 170f, 128f));
			_listISelectSurface.Add(battleStart);
			_listISelectSurface.ForEach(delegate(ISelectedObject<int> x)
			{
				UICommandSurfaceList uICommandSurfaceList = this;
				x.toggle.group = 15;
				x.toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", x.index);
				x.toggle.onDecide = delegate
				{
					if (x is UICommandLabelButton)
					{
						uICommandSurfaceList.OnSelectSurface(x as UICommandLabelButton);
					}
					else
					{
						uICommandSurfaceList.OnSelectBattleStart();
					}
				};
			});
		}

		private void SetDetectionResult(BattleSearchValues iValues)
		{
			string arg = string.Empty;
			switch (iValues)
			{
			case BattleSearchValues.Success:
			case BattleSearchValues.Success_Lost:
			case BattleSearchValues.Found:
				arg = "成功";
				break;
			case BattleSearchValues.Lost:
			case BattleSearchValues.Faile:
			case BattleSearchValues.NotFound:
				arg = "失敗";
				break;
			}
			_strFrame.detectionResult.text = $"索敵結果[{arg}]";
		}

		private void PreparaNext(bool isFoward)
		{
			int index = this.index;
			this.index = Mathe.NextElement(this.index, 0, _listISelectSurface.Count - 1, isFoward, (int x) => _listISelectSurface[x].isValid);
			if (index != this.index)
			{
				ChangeFocus(this.index);
			}
		}

		public void Prev()
		{
			PreparaNext(isFoward: false);
		}

		public void Next()
		{
			PreparaNext(isFoward: true);
		}

		private void ChangeFocus(int nIndex)
		{
			_listISelectSurface.ForEach(delegate(ISelectedObject<int> x)
			{
				x.isFocus = ((x.index == nIndex) ? true : false);
			});
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
		}

		public void RemoveUnit()
		{
			if (!(selectedSurface == null))
			{
				selectedSurface.SetCommand(BattleCommand.None);
				CheckBattleStartState();
			}
		}

		public void RemoveUnitAll()
		{
			_listISelectSurface.ForEach(delegate(ISelectedObject<int> x)
			{
				if (x is UICommandLabelButton)
				{
					((UICommandLabelButton)x).SetCommand(BattleCommand.None);
				}
			});
			CheckBattleStartState();
			_nIndex = 0;
			ChangeFocus(_nIndex);
		}

		private bool CheckBattleStartState()
		{
			bool flag = (from x in _listISelectSurface
				where x is UICommandLabelButton
				select x).All((ISelectedObject<int> x) => ((UICommandLabelButton)x).battleCommand != BattleCommand.None);
			_strFrame.battleStart.SetValid(flag);
			return flag;
		}

		private void OnActive(int nIndex)
		{
			if (index != nIndex)
			{
				index = nIndex;
				ChangeFocus(index);
			}
		}

		private void OnSelectSurface(UICommandLabelButton selectedSurface)
		{
			Dlg.Call(ref _actOnSelectedSurface, selectedSurface);
		}

		public void OnSelectSurface()
		{
			if (selectedSurface == null)
			{
				OnSelectBattleStart();
			}
			else
			{
				OnSelectSurface(selectedSurface);
			}
		}

		private void OnSelectBattleStart()
		{
			List<BattleCommand> obj = (from x in _listISelectSurface
				where x is UICommandLabelButton
				select ((UICommandLabelButton)x).battleCommand).ToList();
			_preOnDecideCommand(obj);
		}
	}
}
