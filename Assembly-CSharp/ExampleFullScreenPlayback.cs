using System;
using UnityEngine;
using UnityEngine.PSVita;

public class ExampleFullScreenPlayback : MonoBehaviour
{
	public string MoviePath;

	public RenderTexture renderTexture;

	public GUISkin skin;

	public float volume = 1f;

	public int audioStreamIndex;

	private int audioStreamMaxIndex = 4;

	public int textStreamIndex;

	private int textStreamMaxIndex = 4;

	private GUIStyle timeTextStyle;

	private GUIStyle subtitleTextStyle;

	private string subtitleText = string.Empty;

	private long subtitleTimeStamp;

	public bool isPlaying;

	private void Start()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		PSVitaVideoPlayer.Init(renderTexture);
        var val = default(PSVitaVideoPlayer.PlayParams);
		val.volume = volume;
		val.loopSetting = PSVitaVideoPlayer.Looping.Continuous;
		val.modeSetting = PSVitaVideoPlayer.Mode.FullscreenVideo;
		val.audioStreamIndex = audioStreamIndex;
		val.textStreamIndex = textStreamIndex;
		PSVitaVideoPlayer.PlayEx(MoviePath, val);
	}

	private void OnPostRender()
	{
		PSVitaVideoPlayer.Update();
	}

	private void OnMovieEvent(int eventID)
	{
        //IL_0001: Unknown result type (might be due to invalid IL or missing references)
        //IL_0002: Unknown result type (might be due to invalid IL or missing references)
        //IL_0003: Unknown result type (might be due to invalid IL or missing references)
        //IL_0004: Unknown result type (might be due to invalid IL or missing references)
        //IL_0006: Unknown result type (might be due to invalid IL or missing references)
        //IL_0018: Expected I4, but got Unknown
        //IL_0018: Unknown result type (might be due to invalid IL or missing references)
        //IL_001b: Invalid comparison between Unknown and I4
        throw new NotImplementedException("‚È‚É‚±‚ê");
        var val = eventID;
		var val2 = val;
		switch (val2 - 1)
		{
		case 2:
			isPlaying = true;
			return;
		case 0:
			isPlaying = false;
			subtitleText = string.Empty;
			return;
		}
		if ((int)val2 == 16)
		{
			subtitleText = PSVitaVideoPlayer.subtitleText;
			subtitleTimeStamp = PSVitaVideoPlayer.subtitleTimeStamp;
		}
	}
}
