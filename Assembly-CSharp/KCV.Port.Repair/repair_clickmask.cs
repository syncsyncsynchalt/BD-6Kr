using System.Collections;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class repair_clickmask : MonoBehaviour
	{
		public void set_clickable(bool setting)
		{
			GetComponent<Collider2D>().enabled = setting;
		}

		public void unclickable_onesec()
		{
			GetComponent<Animation>().Play();
		}

		public void unclickable_sec(float time)
		{
			StartCoroutine(_unclickable_sec(time));
		}

		private IEnumerator _unclickable_sec(float time)
		{
			GetComponent<Collider2D>().enabled = true;
			yield return new WaitForSeconds(time);
			GetComponent<Collider2D>().enabled = false;
		}

		public bool get_clickable()
		{
			return GetComponent<Collider2D>().enabled;
		}
	}
}
