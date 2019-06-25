using KCV;
using local.managers;
using local.models;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class UICheckBaseDialogManager : MonoBehaviour
{
	public enum KEY_FOCUS_TYPE
	{
		NONE,
		THIS,
		DIALOG
	}

	private KeyControl mKeyController;

	[SerializeField]
	private UICheckBaseDialog mPrefab_UICheckBaseDialog;

	[SerializeField]
	private Camera mModalCamera;

	private KEY_FOCUS_TYPE mKeyFocusType;

	private DeckModel mDeckModel;

	private IEnumerator Start()
	{
		mDeckModel = new AlbumManager().UserInfo.GetDeck(1);
		mKeyController = new KeyControl();
		ChangeKeyController(KEY_FOCUS_TYPE.THIS);
		yield return null;
	}

	private void Update()
	{
		if (mKeyController == null)
		{
			return;
		}
		mKeyController.Update();
		if (mKeyFocusType == KEY_FOCUS_TYPE.THIS)
		{
			if (mKeyController.keyState[1].down)
			{
				StartCoroutine(UICheckBaseDialogBegin());
			}
			else if (mKeyController.keyState[0].down)
			{
				Application.LoadLevel("Strategy");
			}
		}
	}

	private IEnumerator UICheckBaseDialogBegin()
	{
		Stopwatch sw = new Stopwatch();
		sw.Reset();
		sw.Start();
		Transform prefab = mPrefab_UICheckBaseDialog.transform;
		GameObject gameObject = mModalCamera.gameObject;
		yield return null;
		float instantiateStartTime = sw.ElapsedMilliseconds;
		UICheckBaseDialog dialog = Util.Instantiate(prefab.gameObject, mModalCamera.gameObject).GetComponent<UICheckBaseDialog>();
		yield return null;
		float instantiateStopTime = sw.ElapsedMilliseconds;
		yield return new WaitForSeconds((instantiateStopTime - instantiateStartTime) / 1000f);
		dialog.SetOnUICheckBaseDialogAction(UICheckBaseDialogAction);
		ChangeKeyController(KEY_FOCUS_TYPE.NONE);
		dialog.Begin(mDeckModel);
	}

	private void UICheckBaseDialogAction(UICheckBaseDialog.ActionType actionType, UICheckBaseDialog dialog)
	{
		switch (actionType)
		{
		case UICheckBaseDialog.ActionType.Shown:
			ChangeKeyController(KEY_FOCUS_TYPE.DIALOG);
			dialog.SetKeyController(mKeyController);
			break;
		case UICheckBaseDialog.ActionType.BeginHide:
			dialog.SetKeyController(null);
			break;
		case UICheckBaseDialog.ActionType.Hidden:
			ChangeKeyController(KEY_FOCUS_TYPE.THIS);
			Object.Destroy(dialog.gameObject);
			break;
		}
	}

	private void ChangeKeyController(KEY_FOCUS_TYPE nextKeyFocusType)
	{
		mKeyFocusType = nextKeyFocusType;
		if (mKeyController != null)
		{
			mKeyController.firstUpdate = true;
		}
	}
}
