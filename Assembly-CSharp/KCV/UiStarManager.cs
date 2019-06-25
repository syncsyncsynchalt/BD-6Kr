using KCV.Scene.Port;
using UnityEngine;

namespace KCV
{
	public class UiStarManager : MonoBehaviour
	{
		[SerializeField]
		private UISprite[] _uiStar;

		public void init(int num)
		{
			_uiStar = new UISprite[5];
			for (int i = 0; i < 5; i++)
			{
				_uiStar[i] = ((Component)base.transform.FindChild("Star" + (i + 1))).GetComponent<UISprite>();
				_uiStar[i].spriteName = "star";
			}
			SetStar(num);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref _uiStar);
		}

		public void SetStar(int num)
		{
			for (int i = 0; i < 5; i++)
			{
				if (i <= num)
				{
					_uiStar[i].spriteName = "star_on";
				}
				else
				{
					_uiStar[i].spriteName = "star";
				}
			}
		}
	}
}
