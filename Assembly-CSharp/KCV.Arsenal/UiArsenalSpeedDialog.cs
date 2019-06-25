using Common.Struct;
using KCV.Utils;
using local.managers;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalSpeedDialog : MonoBehaviour
	{
		[SerializeField]
		private UIPanel _maskPanel;

		[SerializeField]
		private UITexture _maskBg;

		[SerializeField]
		private UITexture _dialogBg;

		[SerializeField]
		private UILabel _fromLabel;

		[SerializeField]
		private UILabel _toLabel;

		[SerializeField]
		private UISprite _yesBtn;

		[SerializeField]
		private UISprite _noBtn;

		[SerializeField]
		private UIButton _uiOverlayBtn;

		public bool IsShow;

		public int Index;

		public void init()
		{
			IsShow = false;
			Index = 0;
			if (_maskPanel == null)
			{
				_maskPanel = GameObject.Find("ConstructBgPanel").GetComponent<UIPanel>();
			}
			if (_maskBg == null)
			{
				_maskBg = ((Component)_maskPanel.transform.FindChild("Bg")).GetComponent<UITexture>();
			}
			if (_dialogBg == null)
			{
				_dialogBg = ((Component)base.transform.FindChild("Bg")).GetComponent<UITexture>();
			}
			if (_fromLabel == null)
			{
				_fromLabel = ((Component)base.transform.FindChild("LabelFrom")).GetComponent<UILabel>();
			}
			if (_toLabel == null)
			{
				_toLabel = ((Component)base.transform.FindChild("LabelTo")).GetComponent<UILabel>();
			}
			if (_yesBtn == null)
			{
				_yesBtn = ((Component)base.transform.FindChild("BtnYes")).GetComponent<UISprite>();
			}
			if (_noBtn == null)
			{
				_noBtn = ((Component)base.transform.FindChild("BtnNo")).GetComponent<UISprite>();
			}
			if (_uiOverlayBtn == null)
			{
				_uiOverlayBtn = ((Component)base.transform.FindChild("OverlayBtn")).GetComponent<UIButton>();
			}
			UIButtonMessage component = _yesBtn.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "YesBtnEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = _noBtn.GetComponent<UIButtonMessage>();
			component2.target = base.gameObject;
			component2.functionName = "NoBtnEL";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			EventDelegate.Add(_uiOverlayBtn.onClick, _onClickOverlayButton);
			((Component)base.transform).GetComponent<UIPanel>().alpha = 0f;
		}

		private void OnDestroy()
		{
			_maskPanel = null;
			_maskBg = null;
			_dialogBg = null;
			_fromLabel = null;
			_toLabel = null;
			_yesBtn = null;
			_noBtn = null;
			_uiOverlayBtn = null;
		}

		public void showHighSpeedDialog(int dockNum)
		{
			((Component)base.transform).GetComponent<UIPanel>().alpha = 1f;
			IsShow = true;
			Index = 0;
			updateSpeedDialogBtn(0);
			_maskPanel.transform.localPosition = Vector3.zero;
			_maskBg.SafeGetTweenAlpha(0f, 0.5f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
			base.transform.localPosition = Vector3.zero;
			ArsenalTaskManager.GetDialogPopUp().Open(base.gameObject, 0f, 0f, 1f, 1f);
			_uiOverlayBtn.GetComponent<Collider2D>().isTrigger = true;
			ArsenalManager arsenalManager = new ArsenalManager();
			arsenalManager.LargeState = arsenalManager.GetDock(dockNum + 1).IsLarge();
			MaterialInfo maxForCreateShip = arsenalManager.GetMaxForCreateShip();
			int buildKit = maxForCreateShip.BuildKit;
			int buildKit2 = ArsenalTaskManager.GetLogicManager().Material.BuildKit;
			_fromLabel.textInt = buildKit2;
			_toLabel.textInt = buildKit2 - buildKit;
		}

		public void updateSpeedDialogBtn(int num)
		{
			if (Index != num)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
			Index = num;
			if (Index == 1)
			{
				_yesBtn.spriteName = "btn_yes";
				_noBtn.spriteName = "btn_no_on";
				UISelectedObject.SelectedOneButtonZoomUpDown(_yesBtn.gameObject, value: false);
				UISelectedObject.SelectedOneButtonZoomUpDown(_noBtn.gameObject, value: true);
			}
			else
			{
				_yesBtn.spriteName = "btn_yes_on";
				_noBtn.spriteName = "btn_no";
				UISelectedObject.SelectedOneButtonZoomUpDown(_yesBtn.gameObject, value: true);
				UISelectedObject.SelectedOneButtonZoomUpDown(_noBtn.gameObject, value: false);
			}
		}

		public void hideHighSpeedDialog()
		{
			if (IsShow)
			{
				IsShow = false;
				_maskBg.SafeGetTweenAlpha(0.5f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, "compHideDialog");
				BaseDialogPopup.Close(base.gameObject, 0.5f, UITweener.Method.Linear);
				_uiOverlayBtn.GetComponent<Collider2D>().isTrigger = false;
			}
		}

		private void compHideDialog()
		{
			((Component)base.transform).GetComponent<UIPanel>().alpha = 0f;
		}

		public void YesBtnEL(GameObject obj)
		{
			updateSpeedDialogBtn(0);
			ArsenalTaskManager._clsArsenal.StartHighSpeedProcess();
		}

		public void NoBtnEL(GameObject obj)
		{
			updateSpeedDialogBtn(1);
			ArsenalTaskManager._clsArsenal.StartHighSpeedProcess();
		}

		private void _onClickOverlayButton()
		{
			ArsenalTaskManager._clsArsenal.hideHighSpeedDialog();
		}
	}
}
