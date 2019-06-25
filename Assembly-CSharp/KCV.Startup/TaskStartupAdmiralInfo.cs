using KCV.Utils;
using Sony.Vita.Dialog;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Startup
{
	public class TaskStartupAdmiralInfo : SceneTaskMono
	{
		[SerializeField]
		private UIButton _uiDecideButton;

		[SerializeField]
		private ButtonLightTexture _btnLight;

		[SerializeField]
		private UIInput _uiNameInput;

		private UIPanel _uiPanel;

		private string _strEditName = string.Empty;

		private int _starterShipNum;

		private Animation _ANI;

		private bool _shipSelected;

		private bool _shipCancelled;

		private void OnDestroy()
		{
			_uiPanel = null;
			_uiNameInput = null;
			_ANI = null;
		}

		public void Setup()
		{
			_uiDecideButton.state = UIButtonColor.State.Disabled;
			_uiNameInput.value = StartupTaskManager.GetData().AdmiralName;
		}

		protected override bool Init()
		{
			Ime.OnGotIMEDialogResult += OnGotIMEDialogResult;
			Main.Initialise();
			Utils.PlayAdmiralNameVoice();
			UIStartupNavigation navigation = StartupTaskManager.GetNavigation();
			navigation.SetNavigationInAdmiralInfo(StartupTaskManager.IsInheritStartup());
			Util.FindParentToChild(ref _uiPanel, base.scenePrefab, "InfoPanel");
			if (_uiNameInput.onSubmit != null)
			{
				_uiNameInput.onSubmit.Clear();
			}
			EventDelegate.Add(_uiNameInput.onSubmit, _onNameSubmit);
			if (_uiNameInput.onChange != null)
			{
				_uiNameInput.onChange.Clear();
			}
			_uiNameInput.onChange.Add(new EventDelegate(delegate
			{
				ChkButtonState();
			}));
			if (_uiDecideButton.onClick != null)
			{
				_uiDecideButton.onClick.Clear();
			}
			_uiDecideButton.onClick.Add(new EventDelegate(delegate
			{
				_onNameSubmit();
			}));
			_uiDecideButton.state = ((_uiNameInput.value == string.Empty) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal);
			_uiPanel.SetActive(isActive: true);
			_ANI = GameObject.Find("AdmiralInfoScene/InfoPanel/anchor/Feather").GetComponent<Animation>();
			_shipSelected = false;
			_shipCancelled = false;
			ChkButtonState();
			StartupTaskManager.GetStartupHeader().SetMessage("提督名入力");
			_uiNameInput.isSelected = true;
			return true;
		}

		protected override bool UnInit()
		{
			Ime.OnGotIMEDialogResult -= OnGotIMEDialogResult;
			return true;
		}

		protected override bool Run()
		{
			KeyControl keyControl = StartupTaskManager.GetKeyControl();
			Main.Update();
			if (Ime.IsDialogOpen)
			{
				return true;
			}
			if (!keyControl.GetDown(KeyControl.KeyName.SELECT))
			{
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					OnClickInputLabel();
				}
				else
				{
					if (keyControl.GetDown(KeyControl.KeyName.START))
					{
						if (_uiNameInput.value == string.Empty || _uiNameInput.value.Replace(" ", string.Empty).Replace("\u3000", string.Empty) == string.Empty)
						{
							return true;
						}
						if (Utils.ChkNGWard(_uiNameInput.value))
						{
							_ANI.Play("feather_ng");
							return true;
						}
						_uiNameInput.isSelected = false;
						_onNameSubmit();
						return false;
					}
					if (keyControl.GetDown(KeyControl.KeyName.BATU) && !StartupTaskManager.IsInheritStartup())
					{
						_uiNameInput.isSelected = false;
						SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
						{
							Application.LoadLevel(Generics.Scene.Title.ToString());
							this.DelayActionFrame(2, delegate
							{
								SingletonMonoBehaviour<FadeCamera>.Instance.FadeIn(0.2f, null);
							});
						});
						return true;
					}
				}
			}
			if (StartupTaskManager.GetMode() != StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_BEF)
			{
				return (StartupTaskManager.GetMode() == StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_ST) ? true : false;
			}
			return true;
		}

		private void _onNameSubmit()
		{
			_uiNameInput.isSelected = false;
			if (_uiNameInput.value == string.Empty || Utils.ChkNGWard(_uiNameInput.value))
			{
				_uiNameInput.value = "横須賀提督";
			}
			StartupTaskManager.GetData().AdmiralName = _uiNameInput.value;
			_uiPanel.SetActive(isActive: false);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			StartupTaskManager.ReqMode(StartupTaskManager.StartupTaskManagerMode.FirstShipSelect);
		}

		private string han2zen(string value)
		{
			string text = value;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add(" ", "\u3000");
			dictionary.Add("!", "！");
			dictionary.Add("\"", "”");
			dictionary.Add("#", "＃");
			dictionary.Add("$", "＄");
			dictionary.Add("%", "％");
			dictionary.Add("&", "＆");
			dictionary.Add("'", "’");
			dictionary.Add("(", "（");
			dictionary.Add(")", "）");
			dictionary.Add("*", "＊");
			dictionary.Add("+", "＋");
			dictionary.Add(",", "，");
			dictionary.Add("-", "－");
			dictionary.Add(".", "．");
			dictionary.Add("/", "／");
			dictionary.Add("0", "０");
			dictionary.Add("1", "１");
			dictionary.Add("2", "２");
			dictionary.Add("3", "３");
			dictionary.Add("4", "４");
			dictionary.Add("5", "５");
			dictionary.Add("6", "６");
			dictionary.Add("7", "７");
			dictionary.Add("8", "８");
			dictionary.Add("9", "９");
			dictionary.Add(":", "：");
			dictionary.Add(";", "；");
			dictionary.Add("<", "＜");
			dictionary.Add("=", "＝");
			dictionary.Add(">", "＞");
			dictionary.Add("?", "？");
			dictionary.Add("@", "＠");
			dictionary.Add("A", "Ａ");
			dictionary.Add("B", "Ｂ");
			dictionary.Add("C", "Ｃ");
			dictionary.Add("D", "Ｄ");
			dictionary.Add("E", "Ｅ");
			dictionary.Add("F", "Ｆ");
			dictionary.Add("G", "Ｇ");
			dictionary.Add("H", "Ｈ");
			dictionary.Add("I", "Ｉ");
			dictionary.Add("J", "Ｊ");
			dictionary.Add("K", "Ｋ");
			dictionary.Add("L", "Ｌ");
			dictionary.Add("M", "Ｍ");
			dictionary.Add("N", "Ｎ");
			dictionary.Add("O", "Ｏ");
			dictionary.Add("P", "Ｐ");
			dictionary.Add("Q", "Ｑ");
			dictionary.Add("R", "Ｒ");
			dictionary.Add("S", "Ｓ");
			dictionary.Add("T", "Ｔ");
			dictionary.Add("U", "Ｕ");
			dictionary.Add("V", "Ｖ");
			dictionary.Add("W", "Ｗ");
			dictionary.Add("X", "Ｘ");
			dictionary.Add("Y", "Ｙ");
			dictionary.Add("Z", "Ｚ");
			dictionary.Add("[", "［");
			dictionary.Add("\\", "￥");
			dictionary.Add("]", "］");
			dictionary.Add("^", "\uff3e");
			dictionary.Add("_", "\uff3f");
			dictionary.Add("`", "‘");
			dictionary.Add("a", "ａ");
			dictionary.Add("b", "ｂ");
			dictionary.Add("c", "ｃ");
			dictionary.Add("d", "ｄ");
			dictionary.Add("e", "ｅ");
			dictionary.Add("f", "ｆ");
			dictionary.Add("g", "ｇ");
			dictionary.Add("h", "ｈ");
			dictionary.Add("i", "ｉ");
			dictionary.Add("j", "ｊ");
			dictionary.Add("k", "ｋ");
			dictionary.Add("l", "ｌ");
			dictionary.Add("m", "ｍ");
			dictionary.Add("n", "ｎ");
			dictionary.Add("o", "ｏ");
			dictionary.Add("p", "ｐ");
			dictionary.Add("q", "ｑ");
			dictionary.Add("r", "ｒ");
			dictionary.Add("s", "ｓ");
			dictionary.Add("t", "ｔ");
			dictionary.Add("u", "ｕ");
			dictionary.Add("v", "ｖ");
			dictionary.Add("w", "ｗ");
			dictionary.Add("x", "ｘ");
			dictionary.Add("y", "ｙ");
			dictionary.Add("z", "ｚ");
			dictionary.Add("{", "｛");
			dictionary.Add("|", "｜");
			dictionary.Add("}", "｝");
			dictionary.Add("~", "～");
			dictionary.Add("｡", "。");
			dictionary.Add("｢", "「");
			dictionary.Add("｣", "」");
			dictionary.Add("､", "、");
			dictionary.Add("･", "・");
			dictionary.Add("ｶ\uff9e", "ガ");
			dictionary.Add("ｷ\uff9e", "ギ");
			dictionary.Add("ｸ\uff9e", "グ");
			dictionary.Add("ｹ\uff9e", "ゲ");
			dictionary.Add("ｺ\uff9e", "ゴ");
			dictionary.Add("ｻ\uff9e", "ザ");
			dictionary.Add("ｼ\uff9e", "ジ");
			dictionary.Add("ｽ\uff9e", "ズ");
			dictionary.Add("ｾ\uff9e", "ゼ");
			dictionary.Add("ｿ\uff9e", "ゾ");
			dictionary.Add("ﾀ\uff9e", "ダ");
			dictionary.Add("ﾁ\uff9e", "ヂ");
			dictionary.Add("ﾂ\uff9e", "ヅ");
			dictionary.Add("ﾃ\uff9e", "デ");
			dictionary.Add("ﾄ\uff9e", "ド");
			dictionary.Add("ﾊ\uff9e", "バ");
			dictionary.Add("ﾋ\uff9e", "ビ");
			dictionary.Add("ﾌ\uff9e", "ブ");
			dictionary.Add("ﾍ\uff9e", "ベ");
			dictionary.Add("ﾎ\uff9e", "ボ");
			dictionary.Add("ｳ\uff9e", "ヴ");
			dictionary.Add("ﾜ\uff9e", "ヷ");
			dictionary.Add("ｲ\uff9e", "ヸ");
			dictionary.Add("ｴ\uff9e", "ヹ");
			dictionary.Add("ｦ\uff9e", "ヺ");
			dictionary.Add("ﾊ\uff9f", "パ");
			dictionary.Add("ﾋ\uff9f", "ピ");
			dictionary.Add("ﾌ\uff9f", "プ");
			dictionary.Add("ﾍ\uff9f", "ペ");
			dictionary.Add("ﾎ\uff9f", "ポ");
			dictionary.Add("ｦ", "ヲ");
			dictionary.Add("ｧ", "ァ");
			dictionary.Add("ｨ", "ィ");
			dictionary.Add("ｩ", "ゥ");
			dictionary.Add("ｪ", "ェ");
			dictionary.Add("ｫ", "ォ");
			dictionary.Add("ｬ", "ャ");
			dictionary.Add("ｭ", "ュ");
			dictionary.Add("ｮ", "ョ");
			dictionary.Add("ｯ", "ッ");
			dictionary.Add("\uff70", "\u30fc");
			dictionary.Add("ｱ", "ア");
			dictionary.Add("ｲ", "イ");
			dictionary.Add("ｳ", "ウ");
			dictionary.Add("ｴ", "エ");
			dictionary.Add("ｵ", "オ");
			dictionary.Add("ｶ", "カ");
			dictionary.Add("ｷ", "キ");
			dictionary.Add("ｸ", "ク");
			dictionary.Add("ｹ", "ケ");
			dictionary.Add("ｺ", "コ");
			dictionary.Add("ｻ", "サ");
			dictionary.Add("ｼ", "シ");
			dictionary.Add("ｽ", "ス");
			dictionary.Add("ｾ", "セ");
			dictionary.Add("ｿ", "ソ");
			dictionary.Add("ﾀ", "タ");
			dictionary.Add("ﾁ", "チ");
			dictionary.Add("ﾂ", "ツ");
			dictionary.Add("ﾃ", "テ");
			dictionary.Add("ﾄ", "ト");
			dictionary.Add("ﾅ", "ナ");
			dictionary.Add("ﾆ", "ニ");
			dictionary.Add("ﾇ", "ヌ");
			dictionary.Add("ﾈ", "ネ");
			dictionary.Add("ﾉ", "ノ");
			dictionary.Add("ﾊ", "ハ");
			dictionary.Add("ﾋ", "ヒ");
			dictionary.Add("ﾌ", "フ");
			dictionary.Add("ﾍ", "ヘ");
			dictionary.Add("ﾎ", "ホ");
			dictionary.Add("ﾏ", "マ");
			dictionary.Add("ﾐ", "ミ");
			dictionary.Add("ﾑ", "ム");
			dictionary.Add("ﾒ", "メ");
			dictionary.Add("ﾓ", "モ");
			dictionary.Add("ﾔ", "ヤ");
			dictionary.Add("ﾕ", "ユ");
			dictionary.Add("ﾖ", "ヨ");
			dictionary.Add("ﾗ", "ラ");
			dictionary.Add("ﾘ", "リ");
			dictionary.Add("ﾙ", "ル");
			dictionary.Add("ﾚ", "レ");
			dictionary.Add("ﾛ", "ロ");
			dictionary.Add("ﾜ", "ワ");
			dictionary.Add("ﾝ", "ン");
			dictionary.Add("\uff9e", "\u309b");
			dictionary.Add("\uff9f", "\u309c");
			Dictionary<string, string> dictionary2 = dictionary;
			foreach (KeyValuePair<string, string> item in dictionary2)
			{
				string text2 = text.Replace(item.Key, item.Value);
				text = text2;
			}
			return text;
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
				imeDialogParams.title = "  貴官の提督名をお知らせ下さい。(" + 12 + "文字まで入力可能です)";
				imeDialogParams.initialText = _strEditName;
				Ime.Open(imeDialogParams);
			}
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
					_ANI.Play("feather_ng");
				}
				_strEditName = text;
				_uiNameInput.value = _strEditName;
			}
			ChkButtonState();
		}

		private void ChkButtonState()
		{
			if (_uiNameInput.value == string.Empty || _uiNameInput.value.Replace(" ", string.Empty).Replace("\u3000", string.Empty) == string.Empty)
			{
				_uiDecideButton.state = UIButtonColor.State.Disabled;
				_btnLight.StopAnim();
			}
			else if (Utils.ChkNGWard(_uiNameInput.value))
			{
				_uiDecideButton.state = UIButtonColor.State.Disabled;
				_btnLight.StopAnim();
			}
			else
			{
				_uiDecideButton.state = UIButtonColor.State.Normal;
				_btnLight.PlayAnim();
			}
			((Collider)_uiDecideButton.GetComponent<BoxCollider>()).enabled = ((_uiDecideButton.state == UIButtonColor.State.Normal) ? true : false);
		}
	}
}
