using KCV.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace KCV
{
	public class CommonDialog : MonoBehaviour
	{
		private UIPanel myPanel;

		[SerializeField]
		private DialogAnimation dialogAnimation;

		public GameObject[] dialogMessages;

		[SerializeField]
		private BoxCollider2D BackCollider;

		public KeyControl keyController;

		[SerializeField]
		private Blur CameraBlur;

		[SerializeField]
		private CommonDialogMessage CommonMessage;

		private IEnumerator ienum;

		public bool isUseDefaultKeyController;

		[SerializeField]
		private GameObject[] Children;

		public Action ShikakuButtonAction;

		public Action BatuButtonAction;

		[Button("debugShow", "show", new object[]
		{

		})]
		public int button;

		public int showNo;

		public bool isOpen
		{
			get;
			private set;
		}

		private void Awake()
		{
			myPanel = GetComponent<UIPanel>();
			myPanel.alpha = 1f;
			isUseDefaultKeyController = true;
			if (CameraBlur != null)
			{
				CameraBlur.enabled = false;
			}
			if (CommonMessage != null)
			{
				CommonMessage.SetActive(isActive: false);
			}
			setActiveChildren(isActive: false);
		}

		private void Update()
		{
			if (keyController != null && keyController.IsRun)
			{
				keyController.Update();
				if (ShikakuButtonAction != null && keyController.IsShikakuDown())
				{
					ShikakuButtonAction();
					ShikakuButtonAction = null;
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					CloseDialog();
				}
				else if (BatuButtonAction != null && keyController.IsBatuDown())
				{
					BatuButtonAction();
					BatuButtonAction = null;
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					CloseDialog();
				}
				else if ((keyController.IsMaruDown() || keyController.IsBatuDown()) && ShikakuButtonAction == null)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					CloseDialog();
				}
			}
		}

		public void OpenDialog(int ShowMessageNo, DialogAnimation.AnimType type = DialogAnimation.AnimType.POPUP)
		{
			for (int i = 0; i < dialogMessages.Length; i++)
			{
				bool active = i == ShowMessageNo;
				dialogMessages[i].SetActive(active);
			}
			OpenDialog(type);
		}

		private void OpenDialog(DialogAnimation.AnimType type)
		{
			setActiveChildren(isActive: true);
			if (ienum != null)
			{
				StopCoroutine(ienum);
			}
			keyController = new KeyControl();
			keyController.IsRun = false;
			myPanel.alpha = 1f;
			if (isUseDefaultKeyController)
			{
				keyController.IsRun = true;
				App.OnlyController = keyController;
				App.OnlyController.ClearKeyAll();
				keyController.firstUpdate = true;
			}
			dialogAnimation.StartAnim(type, isOpen: true);
			if (CameraBlur != null)
			{
				CameraBlur.enabled = true;
			}
			isOpen = true;
		}

		public void OpenDialogWithDisableKeyControl()
		{
			myPanel.alpha = 1f;
			dialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, isOpen: true);
			if (CameraBlur != null)
			{
				CameraBlur.enabled = true;
			}
		}

		public void CloseDialogWithDisabledKeyControl()
		{
			dialogAnimation.StartAnim(DialogAnimation.AnimType.FEAD, isOpen: false);
			if (CameraBlur != null)
			{
				CameraBlur.enabled = false;
			}
			if (CommonMessage != null)
			{
				CommonMessage.SetActive(isActive: false);
			}
		}

		public void CloseDialog()
		{
			if (keyController != null && (keyController.IsRun || !isUseDefaultKeyController))
			{
				keyController.IsRun = false;
				keyController = null;
				App.OnlyController = null;
				App.isFirstUpdate = true;
				isUseDefaultKeyController = true;
				dialogAnimation.StartAnim(DialogAnimation.AnimType.FEAD, isOpen: false);
				if (CameraBlur != null)
				{
					CameraBlur.enabled = false;
				}
				if (CommonMessage != null)
				{
					CommonMessage.SetActive(isActive: false);
				}
				for (int i = 0; i < dialogMessages.Length; i++)
				{
					dialogMessages[i].SetActive(false);
				}
				BackCollider.enabled = true;
				isOpen = false;
				ienum = CloseForEndDialogAnimation();
				StartCoroutine(ienum);
			}
		}

		public void disableBackTouch()
		{
			BackCollider.enabled = false;
		}

		public void setActiveChildren(bool isActive)
		{
			for (int i = 0; i < Children.Length; i++)
			{
				Children[i].SetActive(isActive);
			}
		}

		private IEnumerator CloseForEndDialogAnimation()
		{
			while (!dialogAnimation.IsFinished)
			{
				yield return new WaitForEndOfFrame();
			}
			setActiveChildren(isActive: false);
			ienum = null;
		}

		public IEnumerator WaitForDialogClose()
		{
			while (isOpen)
			{
				yield return new WaitForEndOfFrame();
			}
		}

		public void setOpenAction(Action act)
		{
			dialogAnimation.OpenAction = act;
		}

		public void setCloseAction(Action act)
		{
			dialogAnimation.CloseAction = act;
		}

		public void SetCameraBlur(Blur blur)
		{
			CameraBlur = blur;
		}

		private void debugShow()
		{
			OpenDialog(showNo);
		}

		private void OnDestroy()
		{
			if (ienum != null)
			{
				StopCoroutine(ienum);
			}
			ienum = null;
			myPanel = null;
			dialogAnimation = null;
			dialogMessages = null;
			BackCollider = null;
			keyController = null;
			CameraBlur = null;
			CommonMessage = null;
			Children = null;
			ShikakuButtonAction = null;
		}
	}
}
