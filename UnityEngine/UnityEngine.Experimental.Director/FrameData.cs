namespace UnityEngine.Experimental.Director;

public struct FrameData
{
	internal int m_UpdateId;

	internal double m_Time;

	internal double m_LastTime;

	internal double m_TimeScale;

	public int updateId => m_UpdateId;

	public float time => (float)m_Time;

	public float lastTime => (float)m_LastTime;

	public float deltaTime => (float)m_Time - (float)m_LastTime;

	public float timeScale => (float)m_TimeScale;

	public double dTime => m_Time;

	public double dLastTime => m_LastTime;

	public double dDeltaTime => m_Time - m_LastTime;

	public double dtimeScale => m_TimeScale;
}
