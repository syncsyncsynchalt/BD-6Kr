using local.models;
using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDetail_StatusMaxIcons : MonoBehaviour
	{
		[SerializeField]
		private Transform[] Masks;

		[SerializeField]
		private UISprite[] Icons;

		[SerializeField]
		private UISprite[] StatusLabels;

		[SerializeField]
		private UISprite[] MaxLabels;

		public void SetMaxIcons(ShipModel ship)
		{
			bool[] array = new bool[4]
			{
				ship.IsMaxKaryoku(),
				ship.IsMaxSoukou(),
				ship.IsMaxRaisou(),
				ship.IsMaxTaiku()
			};
			for (int i = 0; i < 4; i++)
			{
				Masks[i].SetActive(!array[i]);
				Icons[i].spriteName = getSpriteName(Icons[i].spriteName, array[i]);
				StatusLabels[i].spriteName = getSpriteName(StatusLabels[i].spriteName, array[i]);
				MaxLabels[i].spriteName = getSpriteName(MaxLabels[i].spriteName, array[i]);
			}
		}

		private string getSpriteName(string name, bool isMax)
		{
			name = ((!isMax) ? name.Replace("_b_", "_g_") : name.Replace("_g_", "_b_"));
			return name;
		}
	}
}
