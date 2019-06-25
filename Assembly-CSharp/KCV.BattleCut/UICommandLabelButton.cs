using Common.Enum;
using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[RequireComponent(typeof(UILabel))]
	public class UICommandLabelButton : UILabelButton
	{
		private BattleCommand _iBattleCommand;

		private Func<bool> _actOnSetCommand;

		public BattleCommand battleCommand
		{
			get
			{
				return _iBattleCommand;
			}
			private set
			{
				_iBattleCommand = value;
			}
		}

		public static UICommandLabelButton Instantiate(UICommandLabelButton prefab, Transform parent, Vector3 pos, int nIndex, BattleCommand iCommand, Func<bool> onSetCommand)
		{
			UICommandLabelButton uICommandLabelButton = UnityEngine.Object.Instantiate(prefab);
			uICommandLabelButton.transform.parent = parent;
			uICommandLabelButton.transform.localScaleOne();
			uICommandLabelButton.transform.localPosition = pos;
			uICommandLabelButton.Init(nIndex, isValid: true, iCommand, onSetCommand);
			return uICommandLabelButton;
		}

		public bool Init(int nIndex, bool isValid, BattleCommand iCommand, Func<bool> onSetCommand)
		{
			_actOnSetCommand = onSetCommand;
			_iBattleCommand = iCommand;
			Init(nIndex, isValid);
			SetLabel(nIndex, iCommand);
			return true;
		}

		private void SetLabel(int nIndex, BattleCommand iCommand)
		{
			string text = $"{nIndex + 1}.{iCommand.GetString()}";
			UILabel component = base.background.GetComponent<UILabel>();
			string text2 = text;
			base.foreground.GetComponent<UILabel>().text = text2;
			component.text = text2;
		}

		public void SetCommand(BattleCommand iCommand)
		{
			if (_iBattleCommand != iCommand)
			{
				_iBattleCommand = iCommand;
				SetLabel(index, _iBattleCommand);
				if (_actOnSetCommand != null)
				{
					_actOnSetCommand();
				}
			}
		}
	}
}
