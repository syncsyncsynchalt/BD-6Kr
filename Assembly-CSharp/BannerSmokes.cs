using UnityEngine;

public class BannerSmokes : MonoBehaviour
{
	private void Awake()
	{
		UISprite component = ((Component)base.transform.GetChild(0)).GetComponent<UISprite>();
		UISprite component2 = ((Component)base.transform.GetChild(1)).GetComponent<UISprite>();
		component.depth = 50;
		component2.depth = 50;
	}
}
