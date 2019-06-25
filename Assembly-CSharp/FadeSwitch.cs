using UnityEngine;

[RequireComponent(typeof(FadeCamera))]
public class FadeSwitch : MonoBehaviour
{
	[Range(0f, 5f)]
	public float time = 1f;

	private static FadeSwitch Instance
	{
		get
		{
			FadeSwitch fadeSwitch = SingletonMonoBehaviour<FadeCamera>.Instance.GetComponent<FadeSwitch>();
			if (fadeSwitch == null)
			{
				fadeSwitch = SingletonMonoBehaviour<FadeCamera>.Instance.gameObject.AddComponent<FadeSwitch>();
			}
			return fadeSwitch;
		}
	}

	public static float FadeTime
	{
		get
		{
			return Instance.time;
		}
		set
		{
			Instance.time = value;
		}
	}

	public static bool IsFadeIn
	{
		get
		{
			return Instance.enabled;
		}
		set
		{
			Instance.enabled = value;
		}
	}

	private void OnEnable()
	{
		GetComponent<FadeCamera>().FadeIn(time, Finished);
	}

	private void OnDisable()
	{
		GetComponent<FadeCamera>().FadeOut(time, Finished);
	}

	private void Finished()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameController");
		if (gameObject != null)
		{
			gameObject.SendMessage("FadeFinished", SendMessageOptions.DontRequireReceiver);
		}
	}
}
