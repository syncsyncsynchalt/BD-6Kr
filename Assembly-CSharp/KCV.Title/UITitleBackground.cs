using System.Collections;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UIPanel))]
	public class UITitleBackground : MonoBehaviour
	{
		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiHexBG;

		[SerializeField]
		private UITexture _uiCloud;

		private UIPanel _uiPanel;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		private void OnDestroy()
		{
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiHexBG);
			Mem.Del(ref _uiCloud);
		}

		public IEnumerator StartBackgroundAnim()
		{
			float offset2 = 0f;
			Rect uvRect = default(Rect);
			while (!(_uiCloud == null))
			{
				offset2 += 4E-05f;
				offset2 %= (float)Screen.width;
				float left = offset2;
				float top = _uiCloud.uvRect.y;
				float width = _uiCloud.uvRect.width;
				float height = _uiCloud.uvRect.height;
				uvRect.Set(left, top, width, height);
				_uiCloud.uvRect = uvRect;
				yield return new WaitForEndOfFrame();
			}
		}
	}
}
