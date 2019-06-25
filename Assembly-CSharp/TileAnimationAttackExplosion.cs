using UnityEngine;

public class TileAnimationAttackExplosion : MonoBehaviour
{
	private UITexture tex;

	public void Awake()
	{
		tex = GetComponent<UITexture>();
		if (tex == null)
		{
			Debug.Log("Warning: UITexture not attached");
		}
		tex.alpha = 0f;
		base.transform.localScale = 0.01f * Vector3.one;
	}

	public void Initialize()
	{
		iTween.ValueTo(base.gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.1f, "onupdate", "Alpha", "onupdatetarget", base.gameObject));
		iTween.ScaleTo(base.gameObject, iTween.Hash("scale", 2f * Vector3.one, "islocal", true, "time", 0.5f, "easeType", iTween.EaseType.easeOutQuad));
		iTween.ValueTo(base.gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.1f, "delay", 0.4f, "onupdate", "Alpha", "onupdatetarget", base.gameObject));
	}

	public void Alpha(float f)
	{
		tex.alpha = f;
	}
}
