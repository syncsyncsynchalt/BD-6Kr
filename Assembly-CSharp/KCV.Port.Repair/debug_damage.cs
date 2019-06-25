using KCV.Utils;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class debug_damage : MonoBehaviour
	{
		public board2 bd2;

		public repair rep;

		private void OnDestroy()
		{
			Mem.Del(ref bd2);
			Mem.Del(ref rep);
		}

		public void OnClick()
		{
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			GameObject target = GameObject.Find("board1_top");
			int mode = rep.now_mode();
			SoundUtils.PlaySE(SEFIleInfos.Explode);
			iTween.Stop(target);
			iTween.MoveFrom(target, iTween.Hash("islocal", true, "y", 200f, "time", 0.5f, "easetype", iTween.EaseType.easeOutElastic));
			iTween.MoveTo(target, iTween.Hash("islocal", true, "y", 0f, "delay", 0.2f));
			bd2 = GameObject.Find("board2_top/board2").GetComponent<board2>();
			bd2.DBG_damage();
			bd2.UpdateList();
			rep.set_mode(mode);
		}
	}
}
