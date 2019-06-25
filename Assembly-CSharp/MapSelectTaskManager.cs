using UnityEngine;

public class MapSelectTaskManager : SceneTaskMono
{
	protected override void Awake()
	{
	}

	protected override void Start()
	{
	}

	protected override bool Init()
	{
		return true;
	}

	protected override bool UnInit()
	{
		return true;
	}

	protected override bool Run()
	{
		return true;
	}

	private void OnGUI()
	{
		GUI.Box(new Rect(0f, Screen.height / 2, Screen.width / 2, Screen.height / 2), string.Empty);
		GUI.Label(new Rect(0f, Screen.height / 2, Screen.width / 2, Screen.height / 2), "[MapSelectTaskManager Info]");
	}
}
