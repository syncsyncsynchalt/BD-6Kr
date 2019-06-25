using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
	private float updateInterval = 0.5f;

	private float accum;

	private float frames;

	private float timeleft;

	private void Start()
	{
		if (!GetComponent<GUIText>())
		{
			MonoBehaviour.print("FramesPerSecond needs a GUIText component!");
			base.enabled = false;
		}
		else
		{
			timeleft = updateInterval;
		}
	}

	private void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames += 1f;
		if ((double)timeleft <= 0.0)
		{
			GetComponent<GUIText>().text = "FrameRate = " + (accum / frames).ToString("f2");
			timeleft = updateInterval;
			accum = 0f;
			frames = 0f;
		}
	}
}
