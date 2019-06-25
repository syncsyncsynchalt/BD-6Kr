using System.Collections;
using UnityEngine;

public class TutorialYouseiAnimation : MonoBehaviour
{
	[SerializeField]
	private Texture ayeOpen;

	[SerializeField]
	private Texture ayeClose;

	private readonly Vector3 position = new Vector3(393f, -136f, 0f);

	private IEnumerator Start()
	{
		UITexture tex = GetComponent<UITexture>();
		tex.mainTexture = ayeClose;
		base.transform.localPosition = position;
		TweenPosition tp = TweenPosition.Begin(base.gameObject, 1.5f, base.transform.localPosition);
		tp.style = UITweener.Style.PingPong;
		tp.from = base.transform.localPosition - new Vector3(0f, 6f, 0f);
		while (true)
		{
			yield return new WaitForSeconds(1f);
			tex.mainTexture = ayeOpen;
			yield return new WaitForSeconds(0.05f);
			tex.mainTexture = ayeClose;
			yield return new WaitForSeconds(0.05f);
			tex.mainTexture = ayeOpen;
			yield return new WaitForSeconds(1f);
			tex.mainTexture = ayeClose;
			yield return new WaitForSeconds(6f);
		}
	}
}
