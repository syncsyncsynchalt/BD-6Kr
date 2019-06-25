using System.Collections.Generic;

namespace UnityEngine.Networking
{
	internal class NetworkMessageHandlers
	{
		private Dictionary<short, NetworkMessageDelegate> m_MsgHandlers = new Dictionary<short, NetworkMessageDelegate>();

		internal void RegisterHandlerSafe(short msgType, NetworkMessageDelegate handler)
		{
			if (handler == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RegisterHandlerSafe id:" + msgType + " handler is null");
				}
				return;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("RegisterHandlerSafe id:" + msgType + " handler:" + handler.Method.Name);
			}
			if (!m_MsgHandlers.ContainsKey(msgType))
			{
				m_MsgHandlers.Add(msgType, handler);
			}
		}

		public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
		{
			if (handler == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RegisterHandler id:" + msgType + " handler is null");
				}
				return;
			}
			if (msgType <= 31)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RegisterHandler: Cannot replace system message handler " + msgType);
				}
				return;
			}
			if (m_MsgHandlers.ContainsKey(msgType))
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("RegisterHandler replacing " + msgType);
				}
				m_MsgHandlers.Remove(msgType);
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("RegisterHandler id:" + msgType + " handler:" + handler.Method.Name);
			}
			m_MsgHandlers.Add(msgType, handler);
		}

		public void UnregisterHandler(short msgType)
		{
			m_MsgHandlers.Remove(msgType);
		}

		internal NetworkMessageDelegate GetHandler(short msgType)
		{
			if (m_MsgHandlers.ContainsKey(msgType))
			{
				return m_MsgHandlers[msgType];
			}
			return null;
		}

		internal Dictionary<short, NetworkMessageDelegate> GetHandlers()
		{
			return m_MsgHandlers;
		}

		internal void ClearMessageHandlers()
		{
			m_MsgHandlers.Clear();
		}
	}
}
