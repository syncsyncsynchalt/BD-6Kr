using System;
using System.Collections;
using UnityEngine;

public class DialogAnimation : MonoBehaviour
{
	public enum AnimType
	{
		POPUP,
		FEAD,
		SLIDE
	}

	private const float popTime = 0.3f;

	[SerializeField]
	private GameObject Black;

	private UIWidget BlackWidget;

	[Button("PopUpIn", "ポップアップIN", new object[]
	{

	})]
	public bool inspecterButton1;

	[Button("FadeIn", "フェードIN", new object[]
	{

	})]
	public bool inspecterButton2;

	[Button("PopUpOut", "ポップアップOUT", new object[]
	{

	})]
	public bool inspecterButton3;

	[Button("FadeOut", "フェードOUT", new object[]
	{

	})]
	public bool inspecterButton4;

	private bool isOpen;

	private bool isFinished;

	private Vector3 defaultPosition;

	private Quaternion defaultRotate;

	private Vector3 defaultScale;

	private float defaultAlpha;

	private UIPanel panel;

	private UIWidget DialogTexture;

	public Action OpenAction;

	public Action CloseAction;

	public float fadeTime = 0.2f;

	public bool IsOpen => isOpen;

	public bool IsFinished => isFinished;

	public void Awake()
	{
		defaultPosition = base.transform.position;
		defaultRotate = base.transform.localRotation;
		defaultScale = base.transform.localScale;
		panel = GetComponent<UIPanel>();
		if (panel != null)
		{
			defaultAlpha = panel.alpha;
		}
		DialogTexture = GetComponent<UIWidget>();
		if (DialogTexture != null)
		{
			DialogTexture.alpha = 0f;
		}
		if (Black != null)
		{
			BlackWidget = Black.GetComponent<UIWidget>();
			if (BlackWidget != null)
			{
				BlackWidget.alpha = 0f;
			}
		}
	}

	public void StartAnim(AnimType animType, bool isOpen)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.isOpen = isOpen;
		TweenAlpha component = GetComponent<TweenAlpha>();
		if (component != null)
		{
			component.onFinished.Clear();
		}
		iTween.Stop(base.gameObject);
		switch (animType)
		{
			case AnimType.POPUP:
				if (IsOpen)
				{
					PopUpIn();
				}
				else
				{
					PopUpOut();
				}
				break;
			case AnimType.FEAD:
				if (IsOpen)
				{
					FadeIn();
				}
				else
				{
					FadeOut();
				}
				break;
			case AnimType.SLIDE:
				if (IsOpen)
				{
					SlideIn(0, 0.5f);
				}
				else
				{
					SlideOut(0, 0.5f);
				}
				break;
		}
		if (Black != null)
		{
			float alpha = (!isOpen) ? 0f : 0.5f;
			TweenAlpha.Begin(Black, fadeTime, alpha);
		}
		isFinished = false;
	}

	private void OpenAnimEnd()
	{
		if (OpenAction != null)
		{
			OpenAction();
		}
		OpenAction = null;
		isFinished = true;
	}

	private void CloseAnimEnd()
	{
		if (CloseAction != null)
		{
			CloseAction();
		}
		CloseAction = null;
		isFinished = true;
	}

	public void PopUpIn()
	{
		if (panel != null)
		{
			TweenAlpha.Begin(base.gameObject, fadeTime, 1f);
		}
		else if (DialogTexture != null)
		{
			TweenAlpha.Begin(DialogTexture.gameObject, fadeTime, 1f);
			TweenAlpha.Begin(BlackWidget.gameObject, fadeTime, 0.5f);
		}
		base.gameObject.transform.localScale = Vector3.one;
		Hashtable hashtable = new Hashtable();
		hashtable.Add("x", 0);
		hashtable.Add("y", 0);
		hashtable.Add("time", 0.3f);
		hashtable.Add("easetype", iTween.EaseType.easeOutBack);
		hashtable.Add("oncomplete", "OpenAnimEnd");
		iTween.ScaleFrom(base.gameObject, hashtable);
	}

	public void PopUpOut()
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("x", 0);
		hashtable.Add("y", 0);
		hashtable.Add("time", 0.3f);
		hashtable.Add("easetype", iTween.EaseType.easeOutQuad);
		hashtable.Add("oncomplete", "CloseAnimEnd");
		iTween.ScaleTo(base.gameObject, hashtable);
	}

	public void FadeIn()
	{
		base.transform.localScale = Vector3.one;
		TweenAlpha tweenAlpha = TweenAlpha.Begin(base.gameObject, fadeTime, 1f);
		tweenAlpha.SetOnFinished(OpenAnimEnd);
	}

	public void FadeIn(float FadeTime)
	{
		base.transform.localScale = Vector3.one;
		TweenAlpha tweenAlpha = TweenAlpha.Begin(base.gameObject, FadeTime, 1f);
		tweenAlpha.SetOnFinished(OpenAnimEnd);
	}

	public void SlideIn(int PosX, float time)
	{
		if (panel != null)
		{
			panel.alpha = 1f;
		}
		float x = PosX;
		Vector3 localPosition = base.transform.localPosition;
		float y = localPosition.y;
		Vector3 localPosition2 = base.transform.localPosition;
		Vector3 target = new Vector3(x, y, localPosition2.z);
		base.transform.MoveTo(target, time, OpenAnimEnd);
	}

	public void SlideOut(int PosX, float time)
	{
		float x = PosX;
		Vector3 localPosition = base.transform.localPosition;
		float y = localPosition.y;
		Vector3 localPosition2 = base.transform.localPosition;
		Vector3 target = new Vector3(x, y, localPosition2.z);
		base.transform.MoveTo(target, time, CloseAnimEnd);
	}

	public void FadeOut()
	{
		TweenAlpha tweenAlpha = TweenAlpha.Begin(base.gameObject, fadeTime, 0f);
		tweenAlpha.SetOnFinished(CloseAnimEnd);
	}

	private void OnDestroy()
	{
		Black = null;
		BlackWidget = null;
		panel = null;
		DialogTexture = null;
		OpenAction = null;
		CloseAction = null;
	}
}
