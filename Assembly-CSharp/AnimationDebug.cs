using System.Collections.Generic;
using UnityEngine;

public class AnimationDebug : MonoBehaviour
{
	public Animation anim;

	public int animNo;

	[Button("Play", "Play", new object[]
	{

	})]
	public int Button1;

	public List<string> AnimationList;

	private void Start()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		anim = GetComponent<Animation>();
		AnimationList = new List<string>();
		foreach (AnimationState item in anim)
		{
			AnimationState val = item;
			AnimationList.Add(val.name);
		}
	}

	public void Play()
	{
		anim.Play(AnimationList[animNo]);
	}

	private void OnDestroy()
	{
		anim = null;
		AnimationList.Clear();
		AnimationList = null;
	}
}
