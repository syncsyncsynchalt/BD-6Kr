using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIISOverlayBtn2 : MonoBehaviour
	{
		private static readonly float BACKBTN_MOVE_TIME = 1f;

		private UIButton _uiOverlayBtn2;

		private void Awake()
		{
			_uiOverlayBtn2 = ((Component)base.transform.FindChild("OverlayBtn2")).GetComponent<UIButton>();
			_uiOverlayBtn2.GetComponent<Collider2D>().enabled = false;
		}

		public void SetBGButtonTouchable(bool value)
		{
			_uiOverlayBtn2.GetComponent<Collider2D>().enabled = value;
		}

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
