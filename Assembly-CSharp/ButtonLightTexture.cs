using UnityEngine;

public class ButtonLightTexture : MonoBehaviour
{
	private UITexture tex;

	private TweenAlpha ta;

	[Button("PlayAnim", "PlayAnim", new object[]
	{

	})]
	public int button1;

	[Button("StopAnim", "StopAnim", new object[]
	{

	})]
	public int button2;

	private bool _isPlay;

	public bool NowPlay()
	{
		return _isPlay;
	}

	private void Awake()
	{
		tex = GetComponent<UITexture>();
		tex.alpha = 0f;
		ta = GetComponent<TweenAlpha>();
		ta.enabled = false;
		_isPlay = false;
	}

	private void OnDestroy()
	{
		Mem.Del(ref tex);
		Mem.Del(ref ta);
		Mem.Del(ref _isPlay);
	}

	public void PlayAnim()
	{
		ta.enabled = true;
		ta.ResetToBeginning();
		ta.PlayForward();
		_isPlay = true;
	}

	public void StopAnim()
	{
		ta.enabled = false;
		tex.alpha = 0f;
		_isPlay = false;
	}
}
