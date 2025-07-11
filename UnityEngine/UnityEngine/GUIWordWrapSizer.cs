namespace UnityEngine;

internal sealed class GUIWordWrapSizer : GUILayoutEntry
{
	private readonly GUIContent m_Content;

	private readonly float m_ForcedMinHeight;

	private readonly float m_ForcedMaxHeight;

	public GUIWordWrapSizer(GUIStyle style, GUIContent content, GUILayoutOption[] options)
		: base(0f, 0f, 0f, 0f, style)
	{
		m_Content = new GUIContent(content);
		ApplyOptions(options);
		m_ForcedMinHeight = minHeight;
		m_ForcedMaxHeight = maxHeight;
	}

	public override void CalcWidth()
	{
		if (minWidth == 0f || maxWidth == 0f)
		{
			base.style.CalcMinMaxWidth(m_Content, out var num, out var num2);
			if (minWidth == 0f)
			{
				minWidth = num;
			}
			if (maxWidth == 0f)
			{
				maxWidth = num2;
			}
		}
	}

	public override void CalcHeight()
	{
		if (m_ForcedMinHeight == 0f || m_ForcedMaxHeight == 0f)
		{
			float num = base.style.CalcHeight(m_Content, rect.width);
			if (m_ForcedMinHeight == 0f)
			{
				minHeight = num;
			}
			else
			{
				minHeight = m_ForcedMinHeight;
			}
			if (m_ForcedMaxHeight == 0f)
			{
				maxHeight = num;
			}
			else
			{
				maxHeight = m_ForcedMaxHeight;
			}
		}
	}
}
