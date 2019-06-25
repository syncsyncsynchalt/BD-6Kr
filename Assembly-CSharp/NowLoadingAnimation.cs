using UnityEngine;

public class NowLoadingAnimation : SingletonMonoBehaviour<NowLoadingAnimation>
{
	[SerializeField]
	private Camera myCamera;

	[SerializeField]
	private Transform Anchor;

	[SerializeField]
	private NowLoadingText textAnimation;

	private NowLoadingYousei yousei;

	[Button("StartAnimation", "StartAnimation", new object[]
	{
		1
	})]
	public int button1;

	[Button("EndAnimation", "EndAnimation", new object[]
	{

	})]
	public int button2;

	public bool isNowLoadingAnimation;

	public bool isYouseiExist => yousei != null;

	protected override void Awake()
	{
		base.Awake();
		if (myCamera != null)
		{
			myCamera.SetActive(isActive: false);
		}
		Anchor.SetActive(isActive: false);
	}

	public void StartTextAnimation()
	{
	}

	public void StartAnimation(int youseiNo)
	{
		if (isNowLoadingAnimation)
		{
			yousei = Util.Instantiate(Resources.Load("Prefabs/Loading/NowLoadingPrefabs/NowLoadingYousei_" + youseiNo), Anchor.gameObject).GetComponent<NowLoadingYousei>();
			myCamera.SetActive(isActive: true);
			Anchor.SetActive(isActive: true);
			textAnimation.StartAnimation();
		}
		else
		{
			CreateNotAnimation(youseiNo);
		}
	}

	public void CreateNotAnimation(int youseiNo)
	{
		yousei = Util.Instantiate(Resources.Load("Prefabs/Loading/NowLoadingPrefabs/NowLoadingYousei_" + youseiNo), Anchor.gameObject).GetComponent<NowLoadingYousei>();
		myCamera.SetActive(isActive: true);
		Anchor.SetActive(isActive: true);
		textAnimation.StopAnimation();
	}

	public void EndAnimation()
	{
		if (yousei != null)
		{
			Object.Destroy(yousei.gameObject);
		}
		yousei = null;
		textAnimation.StopAnimation();
		myCamera.SetActive(isActive: false);
		Anchor.SetActive(isActive: false);
	}

	public void Hide()
	{
		if (myCamera != null)
		{
			myCamera.SetActive(isActive: false);
		}
		Anchor.SetActive(isActive: false);
	}

	private void OnDestroy()
	{
		myCamera = null;
		Anchor = null;
		textAnimation = null;
	}
}
