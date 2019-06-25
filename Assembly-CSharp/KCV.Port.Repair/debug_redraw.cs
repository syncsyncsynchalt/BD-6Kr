using UnityEngine;

namespace KCV.Port.Repair
{
	public class debug_redraw : MonoBehaviour
	{
		public void OnClick()
		{
			GameObject.Find("Repair Root").GetComponent<repair>();
			board component = GameObject.Find("board1_top/board").GetComponent<board>();
			component.DockStatus();
		}
	}
}
