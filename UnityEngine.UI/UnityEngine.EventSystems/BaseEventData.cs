namespace UnityEngine.EventSystems
{
	public class BaseEventData
	{
		private readonly EventSystem m_EventSystem;

		private bool m_Used;

		public bool used => m_Used;

		public BaseInputModule currentInputModule => m_EventSystem.currentInputModule;

		public GameObject selectedObject
		{
			get
			{
				return m_EventSystem.currentSelectedGameObject;
			}
			set
			{
				m_EventSystem.SetSelectedGameObject(value, this);
			}
		}

		public BaseEventData(EventSystem eventSystem)
		{
			m_EventSystem = eventSystem;
		}

		public void Reset()
		{
			m_Used = false;
		}

		public void Use()
		{
			m_Used = true;
		}
	}
}
