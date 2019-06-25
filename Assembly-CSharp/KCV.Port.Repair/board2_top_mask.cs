using UnityEngine;

namespace KCV.Port.Repair
{
	public class board2_top_mask : MonoBehaviour
	{
		private repair rep;

		private board3 bd3;

		private sw sw;

		private board2_top_mask bd2m;

		private GameObject board_mask2;

		private dialog dia;

		private void Start()
		{
			_init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del(ref rep);
			Mem.Del(ref bd3);
			Mem.Del(ref sw);
			Mem.Del(ref bd2m);
			Mem.Del(ref board_mask2);
			Mem.Del(ref dia);
		}

		public void _init_repair()
		{
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			board_mask2 = GameObject.Find("Repair Root/board2_top_mask");
			bd3 = GameObject.Find("board3").GetComponent<board3>();
			board_mask2.GetComponent<UIPanel>().depth = 132;
			dia = GameObject.Find("dialog_top/dialog").GetComponent<dialog>();
		}

		private void OnClick()
		{
			Debug.Log("mask2に触られた。");
			if (!dia.get_dialog_anime() && !bd3.get_board3_anime())
			{
				rep.set_mode(-2);
				bd3.board3_appear(bstat: false, isSirent: true);
				sw = GameObject.Find("board3/sw01").GetComponent<sw>();
				sw.setSW(stat: false);
				bd3.Cancelled(NotChangeMode: false);
				rep = GameObject.Find("Repair Root").GetComponent<repair>();
				rep.setmask(2, value: false);
				rep.set_mode(2);
			}
		}
	}
}
