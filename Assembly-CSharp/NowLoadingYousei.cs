using System.Collections;
using UnityEngine;

public class NowLoadingYousei : MonoBehaviour
{
	[SerializeField]
	private UISprite YouseiBody;

	[SerializeField]
	private UISprite YouseiFace;

	[SerializeField]
	private UISprite YouseiOption;

	public int YouseiNo;

	private float aye1Time = 0.7f;

	private float aye2Time = 1f;

	public Vector3 position;

	private Coroutine cor;

	private void Start()
	{
		if (SingletonMonoBehaviour<NowLoadingAnimation>.exist() && SingletonMonoBehaviour<NowLoadingAnimation>.Instance.isNowLoadingAnimation)
		{
			StartAyeAnimation();
		}
	}

	public void SetYousei(int youseiNo)
	{
	}

	public void StartAyeAnimation()
	{
		if (YouseiNo != 2 && YouseiNo != 5)
		{
			cor = StartCoroutine(AyeAnimation());
		}
		if (YouseiNo == 2)
		{
			cor = StartCoroutine(AyeAnimation2());
		}
	}

	private IEnumerator AyeAnimation()
	{
		while (true)
		{
			YouseiFace.spriteName = YouseiNo + "-2";
			YouseiFace.MakePixelPerfect();
			yield return new WaitForSeconds(aye1Time);
			YouseiFace.spriteName = YouseiNo + "-3";
			YouseiFace.MakePixelPerfect();
			yield return new WaitForSeconds(aye2Time);
		}
	}

	private IEnumerator AyeAnimation2()
	{
		while (true)
		{
			YouseiBody.spriteName = YouseiNo + "-1";
			YouseiBody.MakePixelPerfect();
			yield return new WaitForSeconds(aye1Time);
			YouseiBody.spriteName = YouseiNo + "-2";
			YouseiBody.MakePixelPerfect();
			yield return new WaitForSeconds(aye2Time);
		}
	}

	private IEnumerator AyeAnimation5()
	{
		while (true)
		{
			YouseiBody.spriteName = YouseiNo + "-1";
			YouseiBody.MakePixelPerfect();
			yield return new WaitForSeconds(aye1Time);
			YouseiBody.spriteName = YouseiNo + "-2";
			YouseiBody.MakePixelPerfect();
			yield return new WaitForSeconds(aye2Time);
		}
	}

	private IEnumerator AyeAnimation8()
	{
		while (true)
		{
			YouseiBody.spriteName = YouseiNo + "-1";
			YouseiBody.MakePixelPerfect();
			yield return new WaitForSeconds(aye1Time);
			YouseiBody.spriteName = YouseiNo + "-2";
			YouseiBody.MakePixelPerfect();
			yield return new WaitForSeconds(aye2Time);
		}
	}

	private IEnumerator AyeAnimation9()
	{
		while (true)
		{
			YouseiBody.spriteName = YouseiNo + "-1";
			YouseiBody.MakePixelPerfect();
			yield return new WaitForSeconds(aye1Time);
			YouseiBody.spriteName = YouseiNo + "-2";
			YouseiBody.MakePixelPerfect();
			yield return new WaitForSeconds(aye2Time);
		}
	}

	private void OnDestroy()
	{
		if (cor != null)
		{
			StopCoroutine(cor);
		}
		YouseiBody = null;
		YouseiFace = null;
		YouseiOption = null;
	}
}
