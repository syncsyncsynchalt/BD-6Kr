using UnityEngine;

public class TileAnimationTanker : MonoBehaviour
{
	private UITexture tex;

	private Vector3 start;

	private Vector3 finish;

	private float birth;

	private bool on;

	public void Awake()
	{
		tex = GetComponent<UITexture>();
		if (tex == null)
		{
			Debug.Log("Warning: UITexture not attached");
		}
		start = Vector3.zero;
		finish = Vector3.zero;
		birth = 0f;
		on = false;
	}

	public void Initialize(Vector3 s, Vector3 f)
	{
		start = s;
		finish = f;
		birth = Time.time;
		on = true;
	}

	public void Update()
	{
		if (on)
		{
			base.transform.localPosition += Vector3.Normalize(finish - start) * 50f * Time.deltaTime;
			if (Vector3.Distance(base.transform.localPosition, start) >= Vector3.Distance(start, finish))
			{
				base.transform.localPosition = start;
			}
			if (Time.time > birth + 6f)
			{
				Object.Destroy(base.gameObject);
			}
			else if (Time.time > birth + 5f || Vector3.Distance(base.transform.localPosition, finish) < 50f)
			{
				tex.alpha -= Mathf.Min(tex.alpha, 50f * Time.deltaTime);
			}
			else
			{
				tex.alpha = Mathf.Max(0f, Mathf.Sin(4f * Time.time));
			}
		}
	}
}
