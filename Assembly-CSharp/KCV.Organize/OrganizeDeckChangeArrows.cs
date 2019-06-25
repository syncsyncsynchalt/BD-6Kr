using UnityEngine;

namespace KCV.Organize
{
	public class OrganizeDeckChangeArrows : MonoBehaviour
	{
		[SerializeField]
		private UITexture NextArrow;

		[SerializeField]
		private UITexture PrevArrow;

		[SerializeField]
		private CommonDeckSwitchManager deckSwitcher;

		private void OnDestroy()
		{
			Mem.Del(ref NextArrow);
			Mem.Del(ref PrevArrow);
			Mem.Del(ref deckSwitcher);
		}

		public void UpdateView()
		{
			NextArrow.SetActive(deckSwitcher.isChangeRight);
			PrevArrow.SetActive(deckSwitcher.isChangeLeft);
		}
	}
}
