using KCV.Scene.Port;
using local.models;
using UnityEngine;

namespace KCV
{
	public class SlotItemLevelStar : MonoBehaviour
	{
		[SerializeField]
		private UIWidget LevelNotMaxContainer;

		[SerializeField]
		private UILabel labelLevel;

		[SerializeField]
		private UISprite levelMax;

		private int MAX_LEVEL = 10;

		public void Init(SlotitemModel m)
		{
			if (m == null || m.Level == 0)
			{
				LevelNotMaxContainer.SetActive(isActive: false);
				levelMax.SetActive(isActive: false);
			}
			else if (m.Level == MAX_LEVEL)
			{
				LevelNotMaxContainer.SetActive(isActive: false);
				levelMax.SetActive(isActive: true);
			}
			else
			{
				LevelNotMaxContainer.SetActive(isActive: true);
				levelMax.SetActive(isActive: false);
				labelLevel.text = m.Level.ToString();
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref LevelNotMaxContainer);
			UserInterfacePortManager.ReleaseUtils.Release(ref labelLevel);
			UserInterfacePortManager.ReleaseUtils.Release(ref levelMax);
		}
	}
}
