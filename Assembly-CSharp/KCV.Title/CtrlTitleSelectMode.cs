using Common.SaveManager;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlTitleSelectMode : MonoBehaviour
	{
		[SerializeField]
		private List<UISelectModeBtn> _listSelectMode;

		[SerializeField]
		private Vector3 _vPos = new Vector3(-263f, -168f, 0f);

		private SelectMode _iSelectMode;

		private bool _isInputPossible;

		private UIPanel _uiPanel;

		private Action _actOnAnyInput;

		private Action<SelectMode> _actOnDecide;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		private int maxIndex => (!VitaSaveManager.Instance.IsAllEmpty()) ? 1 : 0;

		public static CtrlTitleSelectMode Instantiate(CtrlTitleSelectMode prefab, Transform parent, Action onAnyInput)
		{
			CtrlTitleSelectMode ctrlTitleSelectMode = UnityEngine.Object.Instantiate(prefab);
			ctrlTitleSelectMode.transform.parent = parent;
			ctrlTitleSelectMode.transform.localScaleOne();
			ctrlTitleSelectMode.transform.localPosition = ctrlTitleSelectMode._vPos;
			ctrlTitleSelectMode.Init(onAnyInput);
			return ctrlTitleSelectMode;
		}

		private bool Init(Action onAnyInput)
		{
			_actOnAnyInput = onAnyInput;
			panel.alpha = 0f;
			foreach (int value in Enum.GetValues(typeof(SelectMode)))
			{
				_listSelectMode[value].Init((SelectMode)value);
				_listSelectMode[value].isFocus = false;
				_listSelectMode[value].toggle.startsActive = false;
				_listSelectMode[value].toggle.onDecide = DecideAnim;
				_listSelectMode[value].toggle.onActive = Util.CreateEventDelegateList(this, "OnActive", (SelectMode)value);
				_listSelectMode[value].toggle.enabled = false;
				if (maxIndex == 0 && value == 1)
				{
					_listSelectMode[value].transform.localPosition = Vector2.left * 9999f;
				}
			}
			if (maxIndex == 1)
			{
				_listSelectMode[1].toggle.startsActive = true;
			}
			else
			{
				_listSelectMode[0].transform.localPositionZero();
				_listSelectMode[0].toggle.startsActive = true;
				ChangeFocus(SelectMode.Appointed, isPlaySE: false);
			}
			return true;
		}

		private void OnDestroy()
		{
			Mem.DelListSafe(ref _listSelectMode);
			Mem.Del(ref _vPos);
			Mem.Del(ref _iSelectMode);
			Mem.Del(ref _isInputPossible);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _actOnDecide);
			Mem.Del(ref _actOnAnyInput);
		}

		public void Play(Action<SelectMode> onDecideMode)
		{
			_actOnDecide = onDecideMode;
			Show().setOnComplete((Action)delegate
			{
				_isInputPossible = true;
				_listSelectMode.ForEach(delegate(UISelectModeBtn x)
				{
					x.toggle.enabled = true;
				});
			});
		}

		public bool Run()
		{
			if (_isInputPossible)
			{
				KeyControl keyControl = TitleTaskManager.GetKeyControl();
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					DecideAnim();
				}
				else if (keyControl.GetDown(KeyControl.KeyName.DOWN))
				{
					PreparaNext(isFoward: false);
				}
				else if (keyControl.GetDown(KeyControl.KeyName.UP))
				{
					PreparaNext(isFoward: true);
				}
			}
			return true;
		}

		private void PreparaNext(bool isFoward)
		{
			Dlg.Call(ref _actOnAnyInput);
			SelectMode iSelectMode = _iSelectMode;
			_iSelectMode = (SelectMode)Mathe.NextElement((int)_iSelectMode, 0, maxIndex, isFoward);
			if (iSelectMode != _iSelectMode)
			{
				ChangeFocus(_iSelectMode, isPlaySE: true);
			}
		}

		private void ChangeFocus(SelectMode iMode, bool isPlaySE)
		{
			if (isPlaySE)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			_listSelectMode.ForEach(delegate(UISelectModeBtn x)
			{
				x.isFocus = ((iMode == x.mode) ? true : false);
			});
		}

		private LTDescr Show()
		{
			return panel.transform.LTValue(0f, 1f, 0.2f).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			}).setEase(LeanTweenType.linear);
		}

		private LTDescr Hide()
		{
			return panel.transform.LTValue(1f, 0f, 1f).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			}).setEase(LeanTweenType.linear);
		}

		private void DecideAnim()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			_listSelectMode.ForEach(delegate(UISelectModeBtn x)
			{
				x.toggle.enabled = false;
			});
			_isInputPossible = false;
			_listSelectMode.ForEach(delegate(UISelectModeBtn x)
			{
				CtrlTitleSelectMode ctrlTitleSelectMode = this;
				if (x.mode == _iSelectMode)
				{
					x.transform.LTMoveLocal(Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutSine).setOnComplete((Action)delegate
					{
						ctrlTitleSelectMode.OnDecide(ctrlTitleSelectMode._iSelectMode);
					});
				}
				else
				{
					x.transform.LTValue(1f, 0f, 0.3f).setOnUpdate(delegate(float a)
					{
						x.alpha = a;
					});
				}
			});
		}

		private void OnActive(SelectMode iMode)
		{
			Dlg.Call(ref _actOnAnyInput);
			if (_iSelectMode != iMode)
			{
				_iSelectMode = iMode;
				ChangeFocus(iMode, isPlaySE: false);
			}
		}

		private void OnDecide(SelectMode iMode)
		{
			Dlg.Call(ref _actOnAnyInput);
			Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate
			{
				Hide().setOnComplete((Action)delegate
				{
					Dlg.Call(ref _actOnDecide, _iSelectMode);
				});
			});
		}
	}
}
