using System.Collections;
using UnityEngine;

namespace KCV.Ending
{
	public class EndingStaffRoll : MonoBehaviour
	{
		public UILabel StaffRollLabel;

		private Coroutine cor;

		public int RollSize;

		public float Speed = 60f;

		public bool isFinishRoll;

		public void StartStaffRoll()
		{
			cor = StartCoroutine(UpdateRoll());
		}

		private IEnumerator UpdateRoll()
		{
			while (true)
			{
				Vector3 localPosition = base.transform.localPosition;
				if (!(localPosition.y < (float)RollSize))
				{
					break;
				}
				Transform transform = base.transform;
				Vector3 localPosition2 = base.transform.localPosition;
				transform.localPositionY(localPosition2.y + Speed * Time.deltaTime);
				yield return null;
			}
			isFinishRoll = true;
			yield return null;
		}

		private void OnDestroy()
		{
			if (cor != null)
			{
				StopCoroutine(cor);
				cor = null;
			}
		}
	}
}
