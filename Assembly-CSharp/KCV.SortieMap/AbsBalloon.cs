using LT.Tweening;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public abstract class AbsBalloon : MonoBehaviour
	{
		protected UISprite _uiSprite;

		public UISprite sprite => this.GetComponentThis(ref _uiSprite);

		protected virtual void OnDestroy()
		{
			Mem.Del(ref _uiSprite);
		}

		protected abstract void SetBalloonPos(UISortieShip.Direction iDirection);

		public LTDescr ShowHide()
		{
			base.transform.LTScale(Vector3.one, 0.3f).setEase(LeanTweenType.easeInExpo);
			return base.transform.LTScale(Vector3.zero, 0.3f).setEase(LeanTweenType.easeInExpo).setDelay(1.5f);
		}
	}
}
