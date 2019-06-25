using KCV.Utils;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class high_repair : MonoBehaviour
	{
		private repair rep;

		private board2 bd2;

		private dialog2 dia2;

		private UISprite ele_s;

		private board3_top_mask bd3m;

		private GameObject cursor;

		private void Start()
		{
			_init_repair();
		}

		private void OnDestroy()
		{
			Mem.Del(ref rep);
			Mem.Del(ref bd2);
			Mem.Del(ref dia2);
			Mem.Del(ref ele_s);
			Mem.Del(ref bd3m);
			Mem.Del(ref cursor);
		}

		public void _init_repair()
		{
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			bd2 = ((Component)rep.transform.FindChild("board2_top/board2")).GetComponent<board2>();
			dia2 = GameObject.Find("dialog2").GetComponent<dialog2>();
		}

		public void OnClick()
		{
			if (!dia2.get_dialog2_anime() && rep.now_mode() != -1)
			{
				rep.set_mode(-5);
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
				Debug.Log("high_repair.cs 高速修復が押された!");
				GameObject gameObject = base.gameObject.transform.parent.gameObject.transform.parent.gameObject;
				int.TryParse(gameObject.name, out int result);
				Debug.Log("押された番号：" + result);
				dia2.UpdateInfo(result);
				dia2.SetDock(result);
				rep.setmask(3, value: true);
				dia2.dialog2_appear(bstat: true);
				rep.set_mode(5);
			}
		}
	}
}
