using UnityEngine;

namespace KCV.Dialog
{
	public class ModalCamera : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTextureBackground;

		private bool mShowFlag;

		private void Awake()
		{
			Close();
		}

		public void Show()
		{
			TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(mTextureBackground.gameObject, 0.1f);
			tweenAlpha.from = mTextureBackground.alpha;
			tweenAlpha.to = 0.5f;
			mTextureBackground.GetComponent<Collider2D>().enabled = true;
		}

		public void Close()
		{
			TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(mTextureBackground.gameObject, 0.1f);
			tweenAlpha.from = mTextureBackground.alpha;
			tweenAlpha.to = 0.01f;
			mTextureBackground.GetComponent<Collider2D>().enabled = false;
		}
	}
}
