using UnityEngine;

namespace KCV
{
	public class UIHowToItem : MonoBehaviour
	{
		[SerializeField]
		private UISprite icon;

		[SerializeField]
		private UILabel label;

		public int GetWidth()
		{
			return icon.width + label.width;
		}

		public void Init(string spriteName, string labelText, int depth)
		{
			icon.spriteName = spriteName;
			icon.MakePixelPerfect();
			label.text = labelText;
			icon.depth = depth;
			label.depth = depth;
			label.transform.localPositionX(icon.width + 4);
		}

		private void OnDestroy()
		{
			icon = null;
			label = null;
		}
	}
}
