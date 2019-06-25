using UnityEngine;

namespace KCV.Port.Repair
{
	public class mess_done : MonoBehaviour
	{
		private repair rep;

		private void Start()
		{
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
		}

		private void OnDestroy()
		{
			Mem.Del(ref rep);
		}

		public void rep_reset(int mode)
		{
			rep = GameObject.Find("Repair Root").GetComponent<repair>();
			rep.set_mode(mode);
		}
	}
}
