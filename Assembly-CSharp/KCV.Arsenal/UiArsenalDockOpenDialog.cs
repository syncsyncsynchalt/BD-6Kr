using KCV.Utils;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalDockOpenDialog : MonoBehaviour
	{
		[SerializeField]
		private GameObject _dialogObj;

		[SerializeField]
		private UITexture _dialogBg;

		[SerializeField]
		private UISprite _maskBg;

		[SerializeField]
		private UILabel _keyLabel_b;

		[SerializeField]
		private UILabel _keyLabel_a;

		[SerializeField]
		private UISprite _yesBtn;

		[SerializeField]
		private UISprite _noBtn;

		[SerializeField]
		private Animation _openInfoAnim;

		public bool IsShow;

		public int Index;

		public int _dockIndex;

		public void init()
		{
			IsShow = false;
			Index = 0;
			_dockIndex = 0;
			if (_dialogObj == null)
			{
				_dialogObj = base.transform.FindChild("DialogObj").gameObject;
			}
			if (_dialogBg == null)
			{
				_dialogBg = ((Component)_dialogObj.transform.FindChild("bg/dialog_window")).GetComponent<UITexture>();
			}
			if (_maskBg == null)
			{
				_maskBg = ((Component)base.transform.FindChild("DockOverlayBtn/Background")).GetComponent<UISprite>();
			}
			if (_keyLabel_b == null)
			{
				_keyLabel_b = ((Component)_dialogObj.transform.FindChild("Text_b")).GetComponent<UILabel>();
			}
			if (_keyLabel_a == null)
			{
				_keyLabel_a = ((Component)_dialogObj.transform.FindChild("Text_a")).GetComponent<UILabel>();
			}
			if (_yesBtn == null)
			{
				_yesBtn = ((Component)_dialogObj.transform.FindChild("YesBtn")).GetComponent<UISprite>();
			}
			if (_noBtn == null)
			{
				_noBtn = ((Component)_dialogObj.transform.FindChild("NoBtn")).GetComponent<UISprite>();
			}
			if ((Object)_openInfoAnim == null)
			{
				_openInfoAnim = ((Component)base.transform).GetComponent<Animation>();
			}
			UIButtonMessage component = _yesBtn.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "OnYesButtonEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component2 = _noBtn.GetComponent<UIButtonMessage>();
			component2.target = base.gameObject;
			component2.functionName = "OnNoButtonEL";
			component2.trigger = UIButtonMessage.Trigger.OnClick;
			UIButtonMessage component3 = _maskBg.GetComponent<UIButtonMessage>();
			component3.target = base.gameObject;
			component3.functionName = "_onClickOverlayButton";
			component3.trigger = UIButtonMessage.Trigger.OnClick;
			GetComponent<UIPanel>().alpha = 0f;
		}

		public void showDialog(int num)
		{
			IsShow = true;
			Index = 0;
			_dockIndex = num;
			GetComponent<UIPanel>().alpha = 1f;
			updateDialogBtn(0);
			TaskMainArsenalManager.IsControl = false;
			_dialogObj.transform.localScale = Vector3.zero;
			_dialogObj.transform.localPosition = Vector3.zero;
			_maskBg.transform.localPosition = Vector3.zero;
			ArsenalTaskManager.GetDialogPopUp().Open(_dialogObj, 0f, 0f, 1f, 1f);
			_maskBg.SafeGetTweenAlpha(0f, 0.5f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, "compShowDialog");
			int numOfKeyPossessions = TaskMainArsenalManager.arsenalManager.NumOfKeyPossessions;
			_keyLabel_b.text = numOfKeyPossessions.ToString();
			_keyLabel_a.text = (numOfKeyPossessions - 1).ToString();
		}

		public void compShowDialog()
		{
			Debug.Log("compShowDialog");
			TaskMainArsenalManager.IsControl = true;
		}

		public void updateDialogBtn(int num)
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

		public void hideDialog()
		{
			if (IsShow)
			{
				IsShow = false;
				_maskBg.SafeGetTweenAlpha(0.5f, 0f, 0.5f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, "_compHideDialog");
				BaseDialogPopup.Close(_dialogObj, 0.5f, UITweener.Method.Linear);
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				ArsenalTaskManager._clsArsenal.hideDockOpenDialog();
			}
		}

		private void _compHideDialog()
		{
			if (Index == 1)
			{
				_onOpenInfoAnimationFinished();
			}
			else
			{
				_openInfoAnim.Play();
			}
		}

		public void OnYesButtonEL(GameObject obj)
		{
			if (TaskMainArsenalManager.IsControl)
			{
				Debug.Log("OnYesButtonEL:" + Index);
				TaskMainArsenalManager.IsControl = false;
				updateDialogBtn(0);
				TaskMainArsenalManager.dockMamager[_dockIndex].StartDockOpen();
				if (_dockIndex + 1 < TaskMainArsenalManager.dockMamager.Length)
				{
					TaskMainArsenalManager.dockMamager[_dockIndex + 1].ShowKeyLock();
				}
				hideDialog();
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		public void OnNoButtonEL(GameObject obj)
		{
			if (TaskMainArsenalManager.IsControl)
			{
				TaskMainArsenalManager.IsControl = false;
				updateDialogBtn(1);
				hideDialog();
			}
		}

		private void _onOpenInfoAnimationFinished()
		{
			GetComponent<UIPanel>().alpha = 0f;
			TaskMainArsenalManager.IsControl = true;
		}

		private void _onClickOverlayButton()
		{
			if (TaskMainArsenalManager.IsControl)
			{
				Debug.Log("_onClickOverlayButton");
				OnNoButtonEL(null);
			}
		}

		private void OnDestroy()
		{
			_dialogObj = null;
			_dialogBg = null;
			_maskBg = null;
			_keyLabel_b = null;
			_keyLabel_a = null;
			_yesBtn = null;
			_noBtn = null;
			_openInfoAnim = null;
		}
	}
}
