using System.Collections;
using UnityEngine;

public class CommonPopupDialogMessage : MonoBehaviour
{
	public enum PlayType
	{
		Short,
		Long
	}

	[Button("StartPopup", "Start", new object[]
	{
		"Test",
		0
	})]
	public int button1;

	[SerializeField]
	private UILabel Message;

	private UIPanel myPanel;

	private TweenAlpha ta;

	public float moveY;

	public iTween.EaseType ease;

	public float duration;

	private float hideDelay;

	public Vector3 defaultPos;

	private Hashtable tweenHash;

	private void Awake()
	{
		myPanel = GetComponent<UIPanel>();
		defaultPos = base.transform.localPosition;
		moveY = 120f;
		ease = iTween.EaseType.easeOutSine;
		duration = 0.3f;
		hideDelay = 1f;
		Message.alignment = NGUIText.Alignment.Left;
		myPanel.widgetsAreStatic = true;
		tweenHash = new Hashtable();
	}

	private void OnDestroy()
	{
		Mem.Del(ref Message);
		Mem.Del(ref myPanel);
		Mem.Del(ref ta);
		Mem.Del(ref defaultPos);
		Mem.DelHashtableSafe(ref tweenHash);
	}

	public void StartPopup(string mes, int messageNo, PlayType type = PlayType.Long)
	{
		if (Message == null)
		{
			Message = GetComponentInChildren<UILabel>();
		}
		Message.text = mes;
		hideDelay = ((type != 0) ? 1 : 0);
		StartCoroutine(StartPopupCor(messageNo));
	}

	private IEnumerator StartPopupCor(int messageNo)
	{
		iTween.Stop(base.gameObject);
		myPanel.widgetsAreStatic = false;
		tweenHash.Clear();
		tweenHash.Add("y", defaultPos.y + moveY * (float)(messageNo + 1));
		tweenHash.Add("time", duration);
		tweenHash.Add("easetype", ease);
		tweenHash.Add("islocal", true);
		yield return new WaitForEndOfFrame();
		iTween.MoveTo(base.gameObject, tweenHash);
		ta = TweenAlpha.Begin(base.gameObject, 0.5f, 1f);
		ta.onFinished.Clear();
		ta.SetOnFinished(delegate
		{
			this.OnComplete();
		});
		ta.delay = 0f;
	}

	private void OnComplete()
	{
		ta.onFinished.Clear();
		ta = TweenAlpha.Begin(base.gameObject, 0.5f, 0f);
		ta.SetOnFinished(delegate
		{
			base.transform.localPosition = defaultPos;
			myPanel.widgetsAreStatic = true;
		});
		ta.delay = hideDelay;
	}
}
