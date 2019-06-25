using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIISOverlayBtn1 : MonoBehaviour
	{
		private static readonly float BACKBTN_MOVE_TIME = 1f;

		private UIButton _uiOverlayBtn1;

		private void Awake()
		{
			_uiOverlayBtn1 = ((Component)base.transform.FindChild("OverlayBtn1")).GetComponent<UIButton>();
			_uiOverlayBtn1.GetComponent<Collider2D>().enabled = false;
		}

		public void SetBGButtonTouchable(bool value)
		{
			_uiOverlayBtn1.GetComponent<Collider2D>().enabled = value;
		}

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
