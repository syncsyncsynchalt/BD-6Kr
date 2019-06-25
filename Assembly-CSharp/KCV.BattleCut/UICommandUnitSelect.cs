using Common.Enum;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UIWidget))]
	public class UICommandUnitSelect : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiOverlay;

		[SerializeField]
		private UIInvisibleCollider _uiInvisibleCollider;

		[SerializeField]
		private UIGrid _uiCommandAnchor;

		private int _nIndex;

		private UIWidget _uiWidget;

		private List<UICommandLabelButton> _listCommandUnitLabel;

		private Action<BattleCommand> _actOnDecide;

		private Action _actOnCancel;

		private UIWidget widget => this.GetComponentThis(ref _uiWidget);

		private UICommandLabelButton selectedUnit => _listCommandUnitLabel[_nIndex];

		public bool isColliderEnabled
		{
			set
			{
				_listCommandUnitLabel.ForEach(delegate(UICommandLabelButton x)
				{
					x.toggle.enabled = (x.isValid && value);
				});
				_uiInvisibleCollider.enabled = value;
			}
		}

		public static UICommandUnitSelect Instantiate(UICommandUnitSelect prefab, Transform parent, HashSet<BattleCommand> validCommands, Action<BattleCommand> onDecide, Action onCancel)
		{
			UICommandUnitSelect uICommandUnitSelect = UnityEngine.Object.Instantiate(prefab);
			uICommandUnitSelect.transform.parent = parent;
			uICommandUnitSelect.transform.localPosition = Vector2.down * 12f;
			uICommandUnitSelect.transform.localScale = Vector3.one * 0.8f;
			uICommandUnitSelect.Init(validCommands, onDecide, onCancel);
			return uICommandUnitSelect;
		}

		private bool Init(HashSet<BattleCommand> validCommands, Action<BattleCommand> onDecide, Action onCancel)
		{
			_actOnDecide = onDecide;
			_actOnCancel = onCancel;
			widget.alpha = 0f;
			_nIndex = 0;
			_uiInvisibleCollider.SetOnTouch(OnTouchInvisible);
			SetCommandUnit(validCommands);
			isColliderEnabled = false;
			return true;
		}

		private void SetCommandUnit(HashSet<BattleCommand> validCommands)
		{
			_listCommandUnitLabel = new List<UICommandLabelButton>();
			foreach (int value in Enum.GetValues(typeof(BattleCommand)))
			{
				if (value != -1)
				{
					_listCommandUnitLabel.Add(((Component)_uiCommandAnchor.transform.FindChild($"CommandUnit{value}")).GetComponent<UICommandLabelButton>());
					_listCommandUnitLabel[value].Init(value, validCommands.Contains((BattleCommand)value) ? true : false, (BattleCommand)value, null);
					_listCommandUnitLabel[value].toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", _listCommandUnitLabel[value].index);
					_listCommandUnitLabel[value].toggle.onDecide = delegate
					{
						OnDecide();
					};
					_listCommandUnitLabel[value].toggle.group = 10;
					_listCommandUnitLabel[value].toggle.enabled = (_listCommandUnitLabel[value].isValid ? true : false);
				}
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

		private void PreparaNext(bool isFoward)
		{
			int nIndex = _nIndex;
			_nIndex = Mathe.NextElement(_nIndex, 0, _listCommandUnitLabel.Count - 1, isFoward, (int x) => _listCommandUnitLabel[x].isValid);
			if (nIndex != _nIndex)
			{
				ChangeFocus(_nIndex);
			}
		}

		private void ChangeFocus(int nIndex)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove2);
			_listCommandUnitLabel.ForEach(delegate(UICommandLabelButton x)
			{
				x.isFocus = ((x.index == nIndex) ? true : false);
			});
		}

		public void Show(BattleCommand iCommand, Action onFinished)
		{
			int num = _nIndex = ((iCommand != BattleCommand.None) ? _listCommandUnitLabel.Find((UICommandLabelButton x) => x.battleCommand == iCommand).index : 0);
			_listCommandUnitLabel.ForEach(delegate(UICommandLabelButton x)
			{
				x.isFocus = ((x.index == _nIndex) ? true : false);
			});
			base.transform.LTCancel();
			float time = 0.15f;
			LeanTweenType ease = LeanTweenType.linear;
			base.transform.localScale = Vector3.one * 0.8f;
			base.transform.LTScale(Vector3.one, time).setEase(ease);
			base.transform.LTValue(widget.alpha, 1f, time).setEase(ease).setOnUpdate(delegate(float x)
			{
				widget.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					isColliderEnabled = true;
					Dlg.Call(ref onFinished);
				});
		}

		public void Hide(Action onFinished)
		{
			isColliderEnabled = false;
			base.transform.LTCancel();
			float time = 0.15f;
			LeanTweenType ease = LeanTweenType.linear;
			base.transform.localScale = Vector3.one;
			base.transform.LTScale(Vector3.one * 0.8f, time).setEase(ease);
			base.transform.LTValue(widget.alpha, 0f, time).setEase(ease).setOnUpdate(delegate(float x)
			{
				widget.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					Dlg.Call(ref onFinished);
				});
		}

		private void OnTouchInvisible()
		{
			OnCancel();
		}

		private void OnActive(int nIndex)
		{
			if (_nIndex != nIndex)
			{
				_nIndex = nIndex;
				ChangeFocus(nIndex);
			}
		}

		public bool OnDecide()
		{
			Dlg.Call(ref _actOnDecide, selectedUnit.battleCommand);
			return true;
		}

		public bool OnCancel()
		{
			Dlg.Call(ref _actOnCancel);
			return true;
		}
	}
}
