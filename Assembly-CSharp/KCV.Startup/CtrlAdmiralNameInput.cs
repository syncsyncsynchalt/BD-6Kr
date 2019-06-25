using Sony.Vita.Dialog;
using System;
using UnityEngine;

namespace KCV.Startup
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlAdmiralNameInput : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UIInput _uiNameInput;

		[SerializeField]
		private UILabel _uiTitle;

		[SerializeField]
		private Animation _animFeather;

		[SerializeField]
		private UIButton _uiDecideButton;

		private UIPanel _uiPanel;

		private string _strEditName;

		private Action _actOnCancel;

		private UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static CtrlAdmiralNameInput Instantiate(CtrlAdmiralNameInput prefab, Transform parent, Action onCancel)
		{
			CtrlAdmiralNameInput ctrlAdmiralNameInput = UnityEngine.Object.Instantiate(prefab);
			ctrlAdmiralNameInput.Init(onCancel);
			return ctrlAdmiralNameInput;
		}

		private void OnDestroy()
		{
			base.transform.GetComponentsInChildren<UIWidget>().ForEach(delegate(UIWidget x)
			{
				if (x is UISprite)
				{
					((UISprite)x).Clear();
				}
				Mem.Del(ref x);
			});
		}

		private bool Init(Action onCancel)
		{
			_actOnCancel = onCancel;
			_strEditName = string.Empty;
			Ime.OnGotIMEDialogResult += OnGotIMEDialogResult;
			Main.Initialise();
			return true;
		}

		public bool UnInit()
		{
			Ime.OnGotIMEDialogResult -= OnGotIMEDialogResult;
			return true;
		}

		public bool Run()
		{
			KeyControl keyControl = StartupTaskManager.GetKeyControl();
			Main.Update();
			if (keyControl.GetDown(KeyControl.KeyName.SELECT))
			{
				OnNameSubmit();
				return true;
			}
			if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				if (_uiNameInput.value == string.Empty || _uiNameInput.value.Replace(" ", string.Empty).Replace("\u3000", string.Empty) == string.Empty)
				{
					_strEditName = string.Empty;
					_uiNameInput.value = string.Empty;
					OnClickInputLabel();
				}
				else if (Utils.ChkNGWard(_uiNameInput.value))
				{
					_uiNameInput.value = string.Empty;
					_animFeather.Play();
					OnClickInputLabel();
				}
				else
				{
					_uiNameInput.isSelected = false;
					OnNameSubmit();
				}
			}
			else if (keyControl.GetDown(KeyControl.KeyName.BATU))
			{
				Dlg.Call(ref _actOnCancel);
			}
			return true;
		}

		private void OnGotIMEDialogResult(Messages.PluginMessage msg)
		{
			Ime.ImeDialogResult result = Ime.GetResult();
			if (result.result == Ime.EnumImeDialogResult.RESULT_OK)
			{
				string text = result.text;
				if (Utils.ChkNGWard(text) || Utils.ChkNGWard(result.text))
				{
					text = string.Empty;
					_animFeather.Play();
				}
				_strEditName = text;
				_uiNameInput.value = _strEditName;
			}
		}

		private void OnNameSubmit()
		{
		}

		public void OnClickInputLabel()
		{
			if (_uiNameInput.isSelected)
			{
				Ime.ImeDialogParams imeDialogParams = new Ime.ImeDialogParams();
				imeDialogParams.supportedLanguages = (Ime.FlagsSupportedLanguages.LANGUAGE_JAPANESE | Ime.FlagsSupportedLanguages.LANGUAGE_ENGLISH_GB);
				imeDialogParams.languagesForced = true;
				imeDialogParams.type = Ime.EnumImeDialogType.TYPE_DEFAULT;
				imeDialogParams.option = Ime.FlagsTextBoxOption.OPTION_DEFAULT;
				imeDialogParams.canCancel = true;
				imeDialogParams.textBoxMode = Ime.FlagsTextBoxMode.TEXTBOX_MODE_WITH_CLEAR;
				imeDialogParams.enterLabel = Ime.EnumImeDialogEnterLabel.ENTER_LABEL_DEFAULT;
				imeDialogParams.maxTextLength = 12;
				imeDialogParams.title = "提督名を入力してください。（" + 12 + "文字まで）";
				imeDialogParams.initialText = _strEditName;
				Ime.Open(imeDialogParams);
			}
		}
	}
}
