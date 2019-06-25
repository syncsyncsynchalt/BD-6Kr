using UnityEngine;

public class UIRipples : MonoBehaviour
{
	[SerializeField]
	private UISprite[] mSpriteRipples;

	public void PlayRipple()
	{
		mSpriteRipples[0].GetComponent<Animation>()["Anim_LoadingRipple"].time = 0f;
		mSpriteRipples[1].GetComponent<Animation>()["Anim_LoadingRipple"].time = 0.75f;
		mSpriteRipples[0].GetComponent<Animation>().Play("Anim_LoadingRipple");
		mSpriteRipples[1].GetComponent<Animation>().Play("Anim_LoadingRipple");
	}
}
