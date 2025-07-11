using System.Runtime.InteropServices;

namespace UnityEngine.Cloud.Service;

[StructLayout(LayoutKind.Sequential)]
internal sealed class CloudServiceConfig
{
	private int m_MaxNumberOfEventInGroup;

	private string m_SessionHeaderName;

	private string m_EventsHeaderName;

	private string m_EventsEndPoint;

	private int[] m_NetworkFailureRetryTimeoutInSec;

	public int maxNumberOfEventInGroup
	{
		get
		{
			return m_MaxNumberOfEventInGroup;
		}
		set
		{
			m_MaxNumberOfEventInGroup = value;
		}
	}

	public string sessionHeaderName
	{
		get
		{
			return m_SessionHeaderName;
		}
		set
		{
			m_SessionHeaderName = value;
		}
	}

	public string eventsHeaderName
	{
		get
		{
			return m_EventsHeaderName;
		}
		set
		{
			m_EventsHeaderName = value;
		}
	}

	public string eventsEndPoint
	{
		get
		{
			return m_EventsEndPoint;
		}
		set
		{
			m_EventsEndPoint = value;
		}
	}

	public int[] networkFailureRetryTimeoutInSec
	{
		get
		{
			return m_NetworkFailureRetryTimeoutInSec;
		}
		set
		{
			m_NetworkFailureRetryTimeoutInSec = value;
		}
	}
}
