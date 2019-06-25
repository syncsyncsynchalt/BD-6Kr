using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodelSlotItemChangePreviewInParameter : MonoBehaviour
	{
		[SerializeField]
		private UISprite icon;

		[SerializeField]
		private UILabel labelName;

		[SerializeField]
		private UILabel labelValue;

		private Color zeroColor = new Color(49f / 255f, 49f / 255f, 49f / 255f);

		private Color plusColor = new Color(0f, 164f / 255f, 1f);

		private Color minusColor = new Color(1f, 0f, 0f);

		public void Init(string name, int value, int sabun)
		{
			string str = string.Empty;
			if (name != string.Empty)
			{
				if (sabun > 0)
				{
					icon.spriteName = "icon_up";
					labelName.color = plusColor;
					labelValue.color = plusColor;
					str = "＋";
				}
				else if (sabun == 0)
				{
					icon.spriteName = "icon_steady";
					labelName.color = zeroColor;
					labelValue.color = zeroColor;
				}
				else
				{
					icon.spriteName = "icon_down";
					labelName.color = minusColor;
					labelValue.color = minusColor;
					str = "－";
				}
				labelValue.text = str + value.ToString();
				labelName.text = name;
			}
			else
			{
				icon.spriteName = string.Empty;
				labelName.color = zeroColor;
				labelValue.color = zeroColor;
				labelName.text = string.Empty;
				labelValue.text = string.Empty;
			}
		}

		private void OnDestroy()
		{
			icon = null;
			labelName = null;
			labelValue = null;
		}
	}
}
