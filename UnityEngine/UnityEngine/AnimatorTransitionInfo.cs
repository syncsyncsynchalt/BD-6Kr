namespace UnityEngine;

public struct AnimatorTransitionInfo
{
	private int m_FullPath;

	private int m_UserName;

	private int m_Name;

	private float m_NormalizedTime;

	private bool m_AnyState;

	private int m_TransitionType;

	public int fullPathHash => m_FullPath;

	public int nameHash => m_Name;

	public int userNameHash => m_UserName;

	public float normalizedTime => m_NormalizedTime;

	public bool anyState => m_AnyState;

	internal bool entry => (m_TransitionType & 2) != 0;

	internal bool exit => (m_TransitionType & 4) != 0;

	public bool IsName(string name)
	{
		return Animator.StringToHash(name) == m_Name || Animator.StringToHash(name) == m_FullPath;
	}

	public bool IsUserName(string name)
	{
		return Animator.StringToHash(name) == m_UserName;
	}
}
