using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIInteriorStoreModeSelect : MonoBehaviour
	{
		public enum SelectMode
		{
			Store,
			Interior
		}

		private Dictionary<SelectMode, UIButton> _dicSelectBtn;

		private KeyControl _clsInput;

		public DelDecideSelectMode delDecideSelectMode
		{
			get;
			set;
		}

		private void Awake()
		{
			_dicSelectBtn = new Dictionary<SelectMode, UIButton>();
			foreach (int value in Enum.GetValues(typeof(SelectMode)))
			{
				_dicSelectBtn.Add((SelectMode)value, ((Component)base.transform.FindChild($"Btns/{(SelectMode)value}Btn")).GetComponent<UIButton>());
				_dicSelectBtn[(SelectMode)value].onClick.Add(Util.CreateEventDelegate(this, "_decideSelectBtn", (SelectMode)value));
			}
			base.gameObject.transform.localScale = Vector3.one;
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (_clsInput != null)
			{
				if (_clsInput.IsChangeIndex)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
					_setSelectedBtn((SelectMode)_clsInput.Index);
				}
				if (_clsInput.keyState[1].down)
				{
					_decideSelectBtn((SelectMode)_clsInput.Index);
				}
			}
		}

		public void SetInputEnable(KeyControl input)
		{
			new BaseDialogPopup().Open(base.gameObject, 0f, 0f, 1f, 1f);
			_clsInput = input;
			_clsInput.Index = 0;
			_clsInput.setMinMaxIndex(0, 1);
			_clsInput.setChangeValue(-1f, 1f, 1f, -1f);
			_setBtnEnabled(isEnabled: true);
			_setSelectedBtn((SelectMode)_clsInput.Index);
		}

		private void _setSelectedBtn(SelectMode nIndex)
		{
			foreach (KeyValuePair<SelectMode, UIButton> item in _dicSelectBtn)
			{
				if (item.Key == nIndex)
				{
					item.Value.state = UIButtonColor.State.Pressed;
				}
				else
				{
					item.Value.state = UIButtonColor.State.Normal;
				}
			}
		}

		private void _setBtnEnabled(bool isEnabled)
		{
			foreach (KeyValuePair<SelectMode, UIButton> item in _dicSelectBtn)
			{
				item.Value.isEnabled = isEnabled;
			}
		}

		private void _decideSelectBtn(SelectMode iMode)
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			_setBtnEnabled(isEnabled: false);
			_setSelectedBtn(iMode);
			_clsInput = null;
			new BaseDialogPopup().Close(base.gameObject, 1f, 1f, 0f, 0f);
			if (delDecideSelectMode != null)
			{
				delDecideSelectMode(iMode);
			}
		}
	}
}
