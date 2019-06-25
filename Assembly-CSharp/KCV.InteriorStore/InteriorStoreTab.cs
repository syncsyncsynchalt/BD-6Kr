using UnityEngine;

namespace KCV.InteriorStore
{
	public class InteriorStoreTab : MonoBehaviour
	{
		public UIButton button
		{
			get;
			private set;
		}

		private void Awake()
		{
			button = GetComponent<UIButton>();
		}
	}
}
