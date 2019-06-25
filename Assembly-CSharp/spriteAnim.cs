using UnityEngine;

public class spriteAnim : MonoBehaviour
{
	private UISprite sprite;

	private int count;

	private void Start()
	{
		sprite = GetComponent<UISprite>();
	}

	private void Update()
	{
		count++;
		sprite.spriteName = sprite.atlas.spriteList[count % 220 / 2].name;
	}
}
