using UnityEngine;

namespace KCV.Arsenal
{
	public class ArsenalTankerDialog : MonoBehaviour
	{
		[SerializeField]
		private UILabel Message;

		[SerializeField]
		private UILabel TankerNum;

		private void Start()
		{
			Message = GetComponent<UILabel>();
		}

		public void setMessage(int CreateNum, int beforeNum, int afterNum)
		{
			Message.text = "輸送船を" + CreateNum + "隻入手しました";
			TankerNum.text = beforeNum + "  ▶  " + afterNum;
		}

		private void OnDestroy()
		{
			Message = null;
			TankerNum = null;
		}
	}
}
