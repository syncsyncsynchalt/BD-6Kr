using DG.Tweening;
using KCV;
using KCV.View.Dialog;
using local.models;
using System;
using System.Collections;
using UnityEngine;

public class UICheckBaseDialog : UIBaseDialog
{
	public enum ActionType
	{
		Shown,
		BeginHide,
		Hidden
	}

	public delegate void UICheckBaseDialogAction(ActionType actionType, UICheckBaseDialog dialog);

	private const float ANIMATION_TIME_HIDE = 0.8f;

	private KeyControl mKeyController;

	[SerializeField]
	private UITexture[] mTexture_Ships;

	[SerializeField]
	private UITexture[] mTexture_Cards;

	[SerializeField]
	private UILabel mLabel_Message;

	private DeckModel mDeckModel;

	private UICheckBaseDialogAction mUICheckBaseDialogAction;

	public KeyControl GetKeyController()
	{
		if (mKeyController == null)
		{
			mKeyController = new KeyControl();
		}
		return mKeyController;
	}

	public void SetKeyController(KeyControl keyController)
	{
		mKeyController = keyController;
	}

	private void Update()
	{
		if (mKeyController != null)
		{
			if (mKeyController.keyState[1].down)
			{
				Hide();
			}
			else if (mKeyController.keyState[0].down)
			{
				Hide();
			}
		}
	}

	public void Begin(DeckModel deckModel)
	{
		mDeckModel = deckModel;
		Begin();
	}

	public void SetOnUICheckBaseDialogAction(UICheckBaseDialogAction callBack)
	{
		mUICheckBaseDialogAction = callBack;
	}

	private void CallBackAction(ActionType actionType, UICheckBaseDialog dialog)
	{
		if (mUICheckBaseDialogAction != null)
		{
			mUICheckBaseDialogAction(actionType, dialog);
		}
	}

	protected override Coroutine OnCallEventCoroutine(EventType eventType, UIBaseDialog dialog)
	{
		switch (eventType)
		{
		case EventType.BeginInitialize:
			return StartCoroutine(OnBeginInitializeCoroutine());
		case EventType.BeginShow:
			return StartCoroutine(OnBeginShowCoroutine());
		case EventType.BeginHide:
			return StartCoroutine(OnBeginHideCoroutine(delegate
			{
				CallBackAction(ActionType.BeginHide, this);
			}));
		case EventType.Initialized:
			OnInitialized(dialog);
			return null;
		case EventType.Shown:
			OnShown();
			CallBackAction(ActionType.Shown, this);
			return null;
		case EventType.Hidden:
			CallBackAction(ActionType.Hidden, this);
			return null;
		default:
			return null;
		}
	}

	private void OnInitialized(UIBaseDialog dialog)
	{
		dialog.Show();
	}

	private void OnShown()
	{
		Vector3 localPosition = mLabel_Message.transform.localPosition;
		float x = localPosition.x - 50f;
		Vector3 localPosition2 = mLabel_Message.transform.localPosition;
		float y = localPosition2.y;
		Vector3 localPosition3 = mLabel_Message.transform.localPosition;
		Vector3 localPosition4 = new Vector3(x, y, localPosition3.z);
		Vector3 localPosition5 = mLabel_Message.transform.localPosition;
		mLabel_Message.transform.localPosition = localPosition4;
		mLabel_Message.transform.DOLocalMove(localPosition5, 0.3f);
		mLabel_Message.alpha = 1f;
	}

	private IEnumerator OnBeginInitializeCoroutine()
	{
		mLabel_Message.text = "表示完了";
		mLabel_Message.alpha = 0.01f;
		ShipModel[] shipModels = mDeckModel.GetShips();
		for (int index = 0; index < shipModels.Length; index++)
		{
			ShipModel ship = shipModels[index];
			mTexture_Ships[index].mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(ship.MstId, 1);
			mTexture_Cards[index].mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(ship.MstId, 3);
			yield return null;
			mTexture_Ships[index].alpha = 0.01f;
			mTexture_Cards[index].alpha = 0.01f;
			yield return null;
		}
		yield return null;
	}

	private IEnumerator OnBeginShowCoroutine()
	{
		for (int index2 = 0; index2 < mTexture_Ships.Length; index2++)
		{
			mTexture_Ships[index2].alpha = 1f;
		}
		for (int index = 0; index < mTexture_Cards.Length; index++)
		{
			mTexture_Cards[index].alpha = 1f;
		}
		yield return null;
	}

	private IEnumerator OnBeginHideCoroutine(Action onFinished)
	{
		Vector3 localPosition = mLabel_Message.transform.localPosition;
		float x = localPosition.x + 50f;
		Vector3 localPosition2 = mLabel_Message.transform.localPosition;
		float y = localPosition2.y;
		Vector3 localPosition3 = mLabel_Message.transform.localPosition;
		Vector3 to = new Vector3(x, y, localPosition3.z);
		Vector3 from = mLabel_Message.transform.localPosition;
		mLabel_Message.transform.localPosition = from;
		mLabel_Message.text = "表示終了";
		yield return null;
		mLabel_Message.transform.DOLocalMove(to, 0.8f);
		bool finishedFlag = false;
		DOVirtual.Float(mLabel_Message.alpha, 0.01f, 0.8f, delegate(float alpha)
		{
			this.mLabel_Message.alpha = alpha;
		}).OnComplete(delegate
		{
            finishedFlag = true;

            throw new NotImplementedException("なにこれ");
            // if (base.onFinished != null)
			// {
			//	base.onFinished();
			// }
		});
		while (!finishedFlag)
		{
			yield return null;
		}
	}
}
