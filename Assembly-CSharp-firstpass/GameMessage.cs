using System.Collections;
using UnityEngine;

public class GameMessage : MonoBehaviour
{
	public GUIText displayText;

	[HideInInspector]
	public GUIText guiText2;

	public static GameMessage gameMessage;

	private bool displayed;

	private float timeDisplay;

	private bool displayed2;

	private float timeDisplay2;

	public static bool init;

	private GameObject messageObj;

	private void Awake()
	{
		gameMessage = this;
		messageObj = base.gameObject;
		Init();
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
		init = false;
	}

	public static void Init()
	{
		if (gameMessage == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "GameMessage";
			gameMessage = gameObject.AddComponent<GameMessage>();
			gameMessage.messageObj = gameObject;
		}
		init = true;
		if (gameMessage.displayText == null)
		{
			GameObject gameObject2 = new GameObject();
			gameObject2.name = "guiText1";
			Transform transform = gameObject2.transform;
			transform.parent = gameMessage.messageObj.transform;
			transform.position = new Vector3(1f - 10f / (float)Screen.width, 0f, 1f);
			gameMessage.displayText = gameObject2.AddComponent<GUIText>();
			gameMessage.displayText.alignment = TextAlignment.Right;
			gameMessage.displayText.anchor = TextAnchor.LowerRight;
		}
		if (!(gameMessage.guiText2 == null))
		{
		}
	}

	public static void DisplayMessage(string str)
	{
		if (!init)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "GameMessage";
			gameMessage = gameObject.AddComponent<GameMessage>();
			gameMessage.messageObj = gameObject;
			Init();
		}
		gameMessage.DisplayMsg(str);
	}

	private void DisplayMsg(string str)
	{
		timeDisplay = Time.realtimeSinceStartup;
		displayText.text = displayText.text + str + "\n";
		if (!displayed)
		{
			displayed = true;
			StartCoroutine(DisplayRoutine());
		}
	}

	private IEnumerator DisplayRoutine()
	{
		while (Time.realtimeSinceStartup - timeDisplay < 3f)
		{
			yield return null;
		}
		displayed = false;
		displayText.text = string.Empty;
	}

	public static void DisplayMessage2(string str)
	{
		gameMessage.DisplayMsg2(str);
	}

	private void DisplayMsg2(string str)
	{
		timeDisplay2 = Time.realtimeSinceStartup;
		guiText2.text = str;
		if (!displayed2)
		{
			displayed2 = true;
			StartCoroutine(DisplayRoutine2());
		}
	}

	private IEnumerator DisplayRoutine2()
	{
		while (Time.realtimeSinceStartup - timeDisplay2 < 2f)
		{
			yield return null;
		}
		displayed2 = false;
		guiText2.text = string.Empty;
	}
}
