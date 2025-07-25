namespace UnityEngine;

public struct HumanDescription
{
	public HumanBone[] human;

	public SkeletonBone[] skeleton;

	internal float m_ArmTwist;

	internal float m_ForeArmTwist;

	internal float m_UpperLegTwist;

	internal float m_LegTwist;

	internal float m_ArmStretch;

	internal float m_LegStretch;

	internal float m_FeetSpacing;

	private bool m_HasTranslationDoF;

	public float upperArmTwist
	{
		get
		{
			return m_ArmTwist;
		}
		set
		{
			m_ArmTwist = value;
		}
	}

	public float lowerArmTwist
	{
		get
		{
			return m_ForeArmTwist;
		}
		set
		{
			m_ForeArmTwist = value;
		}
	}

	public float upperLegTwist
	{
		get
		{
			return m_UpperLegTwist;
		}
		set
		{
			m_UpperLegTwist = value;
		}
	}

	public float lowerLegTwist
	{
		get
		{
			return m_LegTwist;
		}
		set
		{
			m_LegTwist = value;
		}
	}

	public float armStretch
	{
		get
		{
			return m_ArmStretch;
		}
		set
		{
			m_ArmStretch = value;
		}
	}

	public float legStretch
	{
		get
		{
			return m_LegStretch;
		}
		set
		{
			m_LegStretch = value;
		}
	}

	public float feetSpacing
	{
		get
		{
			return m_FeetSpacing;
		}
		set
		{
			m_FeetSpacing = value;
		}
	}

	public bool hasTranslationDoF
	{
		get
		{
			return m_HasTranslationDoF;
		}
		set
		{
			m_HasTranslationDoF = value;
		}
	}
}
