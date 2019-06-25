using KCV.Utils;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class board3_btn : MonoBehaviour
	{
		private repair rep;

		private dialog dia;

		private UISprite ele_s;

		private UIPortFrame UIP;

		private int number;

		private board3_top_mask bd3m;

		private board3 bd3;

		private GameObject cursor;

		private void Start()
		{
			_init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del(ref rep);
			Mem.Del(ref bd3);
			Mem.Del(ref ele_s);
			Mem.Del(ref UIP);
			Mem.Del(ref bd3m);
			Mem.Del(ref bd3);
			Mem.Del(ref cursor);
		}

		public void _init_repair()
		{
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			dia = GameObject.Find("dialog").GetComponent<dialog>();
			bd3 = GameObject.Find("board3_top/board3").GetComponent<board3>();
		}

		public void UpdateInfo(int no)
		{
		}

		public void OnClick()
		{
			if (rep.now_mode() == 3 && !dia.get_dialog_anime() && !bd3.get_board3_anime())
			{
				rep.set_mode(-1);
				dia.set_dialog_anime(value: true);
				bd3.Set_Button_Sprite(value: false);
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
				dia.dialog_appear(bstat: true);
				rep.setmask(3, value: true);
				rep.set_mode(4);
			}
		}
	}
}
