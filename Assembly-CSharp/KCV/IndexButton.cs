using UnityEngine;

namespace KCV
{
	public class IndexButton : MonoBehaviour
	{
		public int myIndexNo;

		public KeyControl keyController;

		public bool isClicked;

		private void Awake()
		{
			base.gameObject.tag = "IndexButton";
			isClicked = false;
			UIButton component = ((Component)base.gameObject.transform).GetComponent<UIButton>();
			EventDelegate.Add(component.onClick, onClick);
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void changeIndex()
		{
			if (keyController != null)
			{
				KeyControlManager.Instance.KeyController = keyController;
			}
			KeyControlManager.Instance.KeyController.Index = myIndexNo;
		}

		public void addIndex()
		{
			if (KeyControlManager.exist())
			{
				Debug.Log("addindex");
				Debug.Log(KeyControlManager.Instance.KeyController.Index);
				KeyControlManager.Instance.KeyController.Index += myIndexNo;
				Debug.Log(KeyControlManager.Instance.KeyController.Index);
			}
		}

		public void onClick()
		{
			isClicked = true;
		}
	}
}
