using System;
using UnityEngine;
using UnityEngine.PSVita;

public class ExampleRenderTexturePlayback : MonoBehaviour
{
	public string MoviePath;

	public RenderTexture renderTexture;

	public GUISkin skin;

	private bool isPlaying;

	private void Start()
	{
		PSVitaVideoPlayer.Init(renderTexture);
		PSVitaVideoPlayer.Play(MoviePath, PSVitaVideoPlayer.Looping.Continuous, PSVitaVideoPlayer.Mode.FullscreenVideo);
	}

	private void OnPreRender()
	{
		PSVitaVideoPlayer.Update();
	}

	private void OnGUI()
	{
		GUI.skin = skin;
		GUILayout.BeginArea(new Rect(10f, 10f, 200f, Screen.height));
		if (GUILayout.Button("Stop/Play", new GUILayoutOption[0]))
		{
			if (isPlaying)
			{
				PSVitaVideoPlayer.Stop();
			}
			else
			{
                PSVitaVideoPlayer.Play(MoviePath, PSVitaVideoPlayer.Looping.Continuous, PSVitaVideoPlayer.Mode.FullscreenVideo);
			}
		}
		GUILayout.EndArea();
	}

	private void OnMovieEvent(int eventID)
	{
        //IL_0001: Unknown result type (might be due to invalid IL or missing references)
        //IL_0002: Unknown result type (might be due to invalid IL or missing references)
        //IL_0003: Unknown result type (might be due to invalid IL or missing references)
        //IL_0004: Unknown result type (might be due to invalid IL or missing references)
        //IL_0006: Unknown result type (might be due to invalid IL or missing references)
        //IL_0018: Expected I4, but got Unknown
        throw new NotImplementedException("‚È‚É‚±‚ê");
        var val = eventID;
		var val2 = val;

		switch (val2 - 1)
		{
		case 1:
			break;
		case 2:
			isPlaying = true;
			break;
		case 0:
			isPlaying = false;
			break;
		}
	}
}
