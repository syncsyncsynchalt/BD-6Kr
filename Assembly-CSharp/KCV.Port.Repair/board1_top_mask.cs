using UnityEngine;

namespace KCV.Port.Repair
{
	public class board1_top_mask : MonoBehaviour
	{
		private repair rep;

		private board2 bd2;

		private board1_top_mask bd1m;

		private GameObject board_mask1;

		private void Start()
		{
			_init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del(ref rep);
			Mem.Del(ref bd2);
			Mem.Del(ref bd1m);
			Mem.Del(ref board_mask1);
		}

		public void _init_repair()
		{
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			board_mask1 = GameObject.Find("Repair Root/board1_top_mask");
			board_mask1.GetComponent<UIPanel>().depth = 50;
			bd2 = GameObject.Find("board2").GetComponent<board2>();
		}

		private void OnClick()
		{
			if (!bd2.get_board2_anime())
			{
				rep = GameObject.Find("Repair Root").GetComponent<repair>();
				rep.set_mode(-1);
				bd2.board2_appear(boardStart: false);
				rep.setmask(1, value: false);
				rep.set_mode(1);
			}
		}
	}
}
