using System.Collections;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyAreaName : MonoBehaviour
	{
		[SerializeField]
		private UISprite AreaNameSprite;

		private TweenAlpha tweenAlpha;

		private void Start()
		{
			AreaNameSprite.alpha = 0f;
		}

		public void setAreaName(int areaID)
		{
			AreaNameSprite.spriteName = "map_txt" + areaID.ToString("D2");
			AreaNameSprite.MakePixelPerfect();
		}

		public void StartAnimation()
		{
			AreaNameSprite.transform.localPosition = Vector3.zero;
			AreaNameSprite.alpha = 0f;
			if (tweenAlpha != null)
			{
				tweenAlpha.ResetToBeginning();
				tweenAlpha.from = 0f;
			}
			tweenAlpha = TweenAlpha.Begin(AreaNameSprite.gameObject, 0.2f, 1f);
		}

		private void Animation()
		{
			tweenAlpha = TweenAlpha.Begin(AreaNameSprite.gameObject, 0.2f, 1f);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("x", -50);
			hashtable.Add("easetype", iTween.EaseType.easeOutQuint);
			hashtable.Add("islocal", true);
			hashtable.Add("time", 0.2f);
			iTween.MoveFrom(AreaNameSprite.gameObject, hashtable);
		}
	}
}
