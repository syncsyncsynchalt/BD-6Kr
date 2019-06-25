using UnityEngine;

namespace KCV.Port.Repair
{
	public class sentaku : MonoBehaviour
	{
		private board bd1;

		private board2 bd2;

		private repair rep;

		private board1_top_mask bd1m;

		private GameObject cursor;

		private dialog dia;

		private dialog2 dia2;

		private void Start()
		{
			_init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del(ref bd1);
			Mem.Del(ref bd2);
			Mem.Del(ref rep);
			Mem.Del(ref bd1m);
			Mem.Del(ref cursor);
			Mem.Del(ref dia);
			Mem.Del(ref dia2);
		}

		public void _init_repair()
		{
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			bd1 = GameObject.Find("board").GetComponent<board>();
			bd2 = GameObject.Find("board2").GetComponent<board2>();
			dia = GameObject.Find("dialog_top/dialog").GetComponent<dialog>();
			dia2 = GameObject.Find("dialog2_top/dialog2").GetComponent<dialog2>();
		}

		private void OnClick()
		{
			_init_repair();
			if (!dia.get_dialog_anime() && !bd2.get_board2_anime() && !dia2.get_dialog2_anime() && rep.now_mode() != -1)
			{
				int.TryParse(base.transform.parent.gameObject.name, out int result);
				Debug.Log("get_dock_touchable_count: " + bd1.get_dock_touchable_count() + " Touched:" + result);
				if (bd1.get_dock_touchable_count() > result)
				{
					bd1.dock_selected(result);
				}
			}
		}
	}
}
