using System.Collections;
using UnityEngine;

public class TextureFlash : MonoBehaviour
{
	private UIBasicSprite parentTex;

	private UIBasicSprite maskTex;

	private int frameCount;

	private bool isUpdate;

	[Button("flash", "フラッシュ", new object[]
	{
		5,
		0.05f
	})]
	public int Flash;

	[Button("MaskFadeExpanding", "マスクエフェクト", new object[]
	{
		2,
		0.5f
	})]
	public int MaskEffect;

	private void Start()
	{
		parentTex = ((Component)base.transform.parent).GetComponent<UIBasicSprite>();
		if ((bool)parentTex.gameObject.GetComponent<UITexture>())
		{
			maskTex = base.transform.AddComponent<UITexture>();
			maskTex.mainTexture = parentTex.mainTexture;
			maskTex.width = parentTex.width;
			maskTex.height = parentTex.height;
			maskTex.shader = Shader.Find("GUI/Text Shader");
		}
		else
		{
			maskTex = base.transform.AddComponent<UISprite>();
			((UISprite)maskTex).atlas = ((UISprite)parentTex).atlas;
			((UISprite)maskTex).spriteName = ((UISprite)parentTex).spriteName;
			maskTex.width = parentTex.width;
			maskTex.height = parentTex.height;
		}
		maskTex.enabled = false;
		maskTex.depth = 100;
	}

	private void Update()
	{
	}

	public void MaskFadeExpanding(float size, float time, bool isWhite = true)
	{
		init();
		maskTex.enabled = true;
		if (!isWhite)
		{
			UIBasicSprite uIBasicSprite = maskTex;
			Color color = parentTex.color;
			float r = color.r;
			Color color2 = parentTex.color;
			float g = color2.g;
			Color color3 = parentTex.color;
			uIBasicSprite.color = new Color(r, g, color3.b, 1f);
		}
		else
		{
			maskTex.color = Color.white;
		}
		TweenScale.Begin(base.gameObject, time, new Vector3(size, size, 1f));
		TweenAlpha.Begin(base.gameObject, time, 0f);
	}

	public void flash(int count, float interval)
	{
		init();
		maskTex.enabled = true;
		StartCoroutine(flashAction(count, interval));
	}

	private IEnumerator flashAction(int count, float interval)
	{
		while (0 < count)
		{
			maskTex.enabled = true;
			Debug.Log(count);
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			maskTex.enabled = false;
			yield return new WaitForSeconds(interval);
			count--;
		}
		yield return null;
	}

	public void SetTexFillAmount(float fillAmount)
	{
		if (maskTex.type != parentTex.type)
		{
			maskTex.type = parentTex.type;
			maskTex.fillDirection = parentTex.fillDirection;
		}
		maskTex.fillAmount = fillAmount;
	}

	private void init()
	{
		maskTex.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
	}
}
