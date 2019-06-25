using UnityEngine;

namespace KCV.Strategy
{
	public class BannerShutter : MonoBehaviour
	{
		private UISprite ShutterL;

		private UISprite ShutterR;

		private void Awake()
		{
			ShutterL = ((Component)base.transform.FindChild("BannerShutter_L")).GetComponent<UISprite>();
			ShutterR = ((Component)base.transform.FindChild("BannerShutter_R")).GetComponent<UISprite>();
		}

		public void SetFocusLight(bool isEnable)
		{
			if (ShutterL != null)
			{
				UISelectedObject.SelectedOneObjectBlink(ShutterL.gameObject, isEnable);
				UISelectedObject.SelectedOneObjectBlink(ShutterR.gameObject, isEnable);
			}
		}
	}
}
