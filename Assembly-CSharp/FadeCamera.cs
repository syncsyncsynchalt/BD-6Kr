using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FadeCamera : SingletonMonoBehaviour<FadeCamera>
{
	public enum TransitionRule
	{
		NONE,
		Transition1,
		Transition2
	}

	public MeshRenderer targetRender;

	private float goalTime;

	private float time;

	private Material material;

	private List<Action> action;

	private static readonly string cutoff = "_Cutoff";

	private static readonly string mainTex = "_MainTex";

	private static readonly string maskTex = "_MaskTex";

	public bool isCutoff;

	public bool isDrawNowLoading;

	private bool isWithOutNowLoading = true;

	private string[] TransitionFilePath = new string[3]
	{
		"Textures/Common/Mask/Overlay",
		"Textures/rule/101",
		"Textures/rule/160"
	};

	private TransitionRule nowRule = TransitionRule.Transition1;

	public bool isFadeOut
	{
		get;
		private set;
	}

	public bool fading => goalTime > Time.time;

	public void SetWithOutNowLoading(bool isWithOut)
	{
		isWithOutNowLoading = isWithOut;
	}

	protected override void Awake()
	{
		base.Awake();
		material = targetRender.material;
		bool enabled = isCutoff ? true : false;
		float value = (!isCutoff) ? 1f : (-1f);
		GetComponent<Camera>().enabled = enabled;
		material.SetFloat(cutoff, value);
		action = new List<Action>();
		isFadeOut = false;
		isDrawNowLoading = false;
	}

	private void OnEnable()
	{
		GetComponent<Camera>().enabled = true;
	}

	private void OnDisable()
	{
		if (!isFadeOut)
		{
			GetComponent<Camera>().enabled = false;
		}
	}

	public void UpdateTexture(Texture texture)
	{
		material.SetTexture(mainTex, texture);
	}

	public void UpdateMaskTexture(Texture texture)
	{
		material.SetTexture(maskTex, texture);
	}

	public void FadeOut(float requestTime, Action act)
	{
		isFadeOut = true;
		TimerSetup(requestTime, act);
	}

	public void FadeOutNotNowLoading(float requestTime, Action act)
	{
		isWithOutNowLoading = true;
		FadeOut(requestTime, act);
	}

	public void FadeIn(float requestTime, Action act)
	{
		if (!isWithOutNowLoading)
		{
			SingletonMonoBehaviour<NowLoadingAnimation>.Instance.EndAnimation();
		}
		isFadeOut = false;
		TimerSetup(requestTime, act);
	}

	private void TimerSetup(float requestTime, Action act)
	{
		((Component)targetRender).gameObject.SetActive(true);
		action.Clear();
		if (act != null)
		{
			action.Add(act);
		}
		time = requestTime;
		goalTime = Time.time + time;
		base.enabled = true;
		float num = -1.2f;
		float num2 = 1.2f;
		iTween.Stop(base.gameObject);
		Hashtable hashtable = new Hashtable();
		float @float = material.GetFloat(cutoff);
		if (isFadeOut)
		{
			hashtable.Add("from", @float);
			hashtable.Add("to", num);
			float num3 = (1f - @float) / 2f;
			requestTime *= 1f - num3;
		}
		else
		{
			hashtable.Add("from", @float);
			hashtable.Add("to", num2);
			float num4 = (1f + @float) / 2f;
			requestTime *= 1f - num4;
		}
		if (requestTime < 0f)
		{
			requestTime = 0f;
		}
		hashtable.Add("time", requestTime);
		hashtable.Add("easetype", iTween.EaseType.linear);
		hashtable.Add("onupdate", "UpdateHandler");
		hashtable.Add("oncomplete", "OnCompleteHandler");
		iTween.ValueTo(base.gameObject, hashtable);
	}

	private void UpdateHandler(float value)
	{
		material.SetFloat(cutoff, value);
	}

	private void OnCompleteHandler()
	{
		if (action.Count != 0)
		{
			for (int i = 0; i < action.Count; i++)
			{
				action[i]();
				action[i] = null;
			}
		}
		action.Clear();
		if (isFadeOut && isDrawNowLoading && !isWithOutNowLoading && !SingletonMonoBehaviour<NowLoadingAnimation>.Instance.isYouseiExist)
		{
			SingletonMonoBehaviour<NowLoadingAnimation>.Instance.StartAnimation(UnityEngine.Random.Range(1, 10));
		}
		isWithOutNowLoading = false;
		((Component)targetRender).gameObject.SetActive(isFadeOut);
		base.enabled = false;
	}

	public void SetTransitionRule(TransitionRule rule)
	{
	}

	private void OnDestroy()
	{
		targetRender = null;
	}
}
