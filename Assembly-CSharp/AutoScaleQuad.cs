using UnityEngine;

public class AutoScaleQuad : MonoBehaviour
{
	public enum ScaleType
	{
		Fit,
		Box
	}

	public Transform targetQuad;

	public ScaleType scaleType = ScaleType.Box;

	public bool scalableMask;

	private void Update()
	{
		UpdateScale();
	}

	[ContextMenu("execute")]
	private void UpdateScale()
	{
		float num = GetComponent<Camera>().orthographicSize * 2f;
		float num2 = num * GetComponent<Camera>().aspect;
		if (scaleType == ScaleType.Box)
		{
			float num3 = Mathf.Max(num2, num);
			targetQuad.transform.localScale = new Vector3(num3, num3, 0f);
		}
		else
		{
			targetQuad.transform.localScale = new Vector3(num2, num, 0f);
		}
		targetQuad.transform.localPosition = Vector3.zero + base.transform.forward;
		if (scalableMask)
		{
			float num4 = num / num2;
			((Component)targetQuad).GetComponent<Renderer>().material.SetTextureScale("_MaskTex", new Vector2(1f, num4));
			((Component)targetQuad).GetComponent<Renderer>().material.SetTextureOffset("_MaskTex", new Vector2(0f, (1f - num4) / 2f));
		}
		base.enabled = false;
	}
}
