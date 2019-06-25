using Common.Enum;
using KCV.Utils;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIPanel))]
	public class UICommandUnitList : MonoBehaviour
	{
		[Serializable]
		private struct Params : IDisposable
		{
			public float showTime;

			public LeanTweenType showEase;

			public Vector3 unitListPos;

			public void Dispose()
			{
				Mem.Del(ref showTime);
				Mem.Del(ref showEase);
				Mem.Del(ref unitListPos);
			}
		}

		[SerializeField]
		private float _fUnitIconLabelDrawBorderLineLocalPosX = -100f;

		[SerializeField]
		private UISelectCommandInfo _uiSelectCommandInfo;

		private List<UICommandUnitIcon> _listCommandUnits;

		private List<UISprite> _listCommandUnitOrigs;

		private UIPanel _uiPanel;

		private Vector2 _vSelectCommandPos;

		private BattleCommand[,] _aryCommandsPos;

		private Action _actOnDragAndDropRelease;

		[SerializeField]
		[Header("[Animation Properties]")]
		private Params _strParams;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public List<UICommandUnitIcon> listCommandUnits => _listCommandUnits;

		public float unitIconLabelDrawBorderLineLocalPosX => _fUnitIconLabelDrawBorderLineLocalPosX;

		public UICommandUnitIcon focusUnitIcon
		{
			get
			{
				BattleCommand target = GetCommandType(_vSelectCommandPos);
				return listCommandUnits.Find((UICommandUnitIcon x) => x.commandType == target);
			}
		}

		public bool isColliderEnabled
		{
			set
			{
				_listCommandUnits.ForEach(delegate(UICommandUnitIcon x)
				{
					x.colliderBox2D.enabled = (x.isValid && value);
				});
			}
		}

		public bool isAnyDrag
		{
			get
			{
				foreach (UICommandUnitIcon listCommandUnit in _listCommandUnits)
				{
					if (listCommandUnit.isFocus)
					{
						return true;
					}
				}
				return false;
			}
		}

		private void Awake()
		{
			base.transform.localPosition = _strParams.unitListPos;
			panel.alpha = 0f;
			_uiSelectCommandInfo.ClearInfo();
		}

		private void OnDestroy()
		{
			Mem.Del(ref _fUnitIconLabelDrawBorderLineLocalPosX);
			Mem.Del(ref _uiSelectCommandInfo);
			Mem.DelListSafe(ref _listCommandUnits);
			Mem.DelListSafe(ref _listCommandUnitOrigs);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _vSelectCommandPos);
			Mem.Del(ref _aryCommandsPos);
			Mem.Del(ref _actOnDragAndDropRelease);
			Mem.DelIDisposableSafe(ref _strParams);
		}

		public bool Init(CommandPhaseModel model, Action onDragAndDropRelease)
		{
			_actOnDragAndDropRelease = onDragAndDropRelease;
			bool flag = !model.GetSelectableCommands().Contains(BattleCommand.Kouku) && !model.GetSelectableCommands().Contains(BattleCommand.Totugeki) && !model.GetSelectableCommands().Contains(BattleCommand.Tousha);
			Transform transform = base.transform.Find("CommandOrig");
			transform.localPosition = ((!flag) ? Vector3.zero : (Vector3.right * 100f));
			_listCommandUnits = new List<UICommandUnitIcon>();
			_listCommandUnitOrigs = new List<UISprite>();
			foreach (int value in Enum.GetValues(typeof(BattleCommand)))
			{
				if (value != -1)
				{
					_listCommandUnits.Add(((Component)base.transform.FindChild($"CommandUnit{value}")).GetComponent<UICommandUnitIcon>());
					_listCommandUnitOrigs.Add(((Component)base.transform.FindChild($"CommandOrig/Icon{value}")).GetComponent<UISprite>());
					Vector3 localPosition = _listCommandUnits[value].transform.localPosition;
					localPosition.x = ((!flag) ? localPosition.x : (localPosition.x + 100f));
					_listCommandUnits[value].transform.localPosition = localPosition;
					_listCommandUnits[value].Init((BattleCommand)value, model.GetSelectableCommands().Contains((BattleCommand)value) ? true : false, OnDragStart, OnDragAndDropRelease);
					_listCommandUnits[value].SetActive(_listCommandUnits[value].isValid);
					_listCommandUnitOrigs[value].SetActive(_listCommandUnits[value].isValid);
				}
			}
			_vSelectCommandPos = new Vector2(0f, 0f);
			_aryCommandsPos = new BattleCommand[3, 3]
			{
				{
					BattleCommand.Sekkin,
					BattleCommand.Ridatu,
					BattleCommand.Kouku
				},
				{
					BattleCommand.Hougeki,
					BattleCommand.Taisen,
					BattleCommand.Totugeki
				},
				{
					BattleCommand.Raigeki,
					BattleCommand.Kaihi,
					BattleCommand.Tousha
				}
			};
			return true;
		}

		public LTDescr Show()
		{
			return base.transform.LTValue(panel.alpha, 1f, _strParams.showTime).setEase(_strParams.showEase).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		public void Active2FocusUnit2(UICommandSurface surface, List<BattleCommand> invalidCommands)
		{
			if (surface.commandType == BattleCommand.None)
			{
				_vSelectCommandPos = Vector2.zero;
				ChangeFocus(_vSelectCommandPos);
			}
			else
			{
				BattleCommand iCommand = (!invalidCommands.Contains(surface.commandType)) ? surface.commandType : BattleCommand.Sekkin;
				_vSelectCommandPos = GetCommandPos(iCommand);
				ChangeFocus(_vSelectCommandPos);
			}
		}

		public void ActiveAll2Unit(bool isActive)
		{
			_listCommandUnits.ForEach(delegate(UICommandUnitIcon x)
			{
				x.isFocus = isActive;
			});
			_uiSelectCommandInfo.ClearInfo();
		}

		public void Reset2Unit()
		{
			BattleCommand target = GetCommandType(_vSelectCommandPos);
			_listCommandUnits.Find((UICommandUnitIcon x) => x.commandType == target).Reset();
		}

		public void PrevLine()
		{
			PreparaNext(isLine: true, isFoward: false);
		}

		public void NextLine()
		{
			PreparaNext(isLine: true, isFoward: true);
		}

		public void PrevColumn()
		{
			PreparaNext(isLine: false, isFoward: false);
		}

		public void NextColumn()
		{
			PreparaNext(isLine: false, isFoward: true);
		}

		private void PreparaNext(bool isLine, bool isFoward)
		{
			if (isLine)
			{
				float y2 = _vSelectCommandPos.y;
				_vSelectCommandPos.y = Mathe.NextElement((int)_vSelectCommandPos.y, 0, 2, isFoward, delegate(int y)
				{
					Vector2 vPos2 = new Vector2(_vSelectCommandPos.x, y);
					return _listCommandUnits[(int)GetCommandType(vPos2)].isValid;
				});
				if (y2 != _vSelectCommandPos.y)
				{
					ChangeFocus(_vSelectCommandPos);
				}
			}
			else
			{
				float x2 = _vSelectCommandPos.x;
				_vSelectCommandPos.x = Mathe.NextElement((int)_vSelectCommandPos.x, 0, 2, isFoward, delegate(int x)
				{
					Vector2 vPos = new Vector2(x, _vSelectCommandPos.y);
					return _listCommandUnits[(int)GetCommandType(vPos)].isValid;
				});
				if (x2 != _vSelectCommandPos.x)
				{
					ChangeFocus(_vSelectCommandPos);
				}
			}
		}

		private void ChangeFocus(Vector2 vPos)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
			BattleCommand target = GetCommandType(vPos);
			_listCommandUnits.ForEach(delegate(UICommandUnitIcon x)
			{
				x.isFocus = ((x.commandType == target) ? true : false);
			});
			_uiSelectCommandInfo.SetInfo(target);
		}

		private BattleCommand GetCommandType(Vector2 vPos)
		{
			return _aryCommandsPos[(int)vPos.x, (int)vPos.y];
		}

		private Vector2 GetCommandPos(BattleCommand iCommand)
		{
			Vector2 result = Vector2.zero;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (_aryCommandsPos[i, j] == iCommand)
					{
						result = new Vector2(i, j);
						break;
					}
				}
			}
			return result;
		}

		private void OnDragStart(BattleCommand iCommand)
		{
			_uiSelectCommandInfo.SetInfo(iCommand);
		}

		private void OnDragAndDropRelease()
		{
			Dlg.Call(ref _actOnDragAndDropRelease);
			_uiSelectCommandInfo.ClearInfo();
		}
	}
}
