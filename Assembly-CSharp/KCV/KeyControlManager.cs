using UnityEngine;

namespace KCV
{
	public class KeyControlManager : MonoBehaviour
	{
		protected static KeyControlManager instance;

		private KeyControl keyController;

		public KeyControl CommonKeyController;

		public static KeyControlManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = (KeyControlManager)Object.FindObjectOfType(typeof(KeyControlManager));
					if (instance == null)
					{
						return null;
					}
				}
				return instance;
			}
			set
			{
				instance = value;
			}
		}

		public KeyControl KeyController
		{
			get
			{
				return keyController;
			}
			set
			{
				if (keyController != null)
				{
					keyController.ClearKeyAll();
				}
				keyController = value;
				keyController.firstUpdate = true;
			}
		}

		private void Awake()
		{
			DebugUtils.SLog("KeyControlManager" + Time.realtimeSinceStartup);
			if (!(instance != null))
			{
				instance = this;
			}
		}

		public static bool exist()
		{
			if (instance != null)
			{
				return true;
			}
			return false;
		}
	}
}
