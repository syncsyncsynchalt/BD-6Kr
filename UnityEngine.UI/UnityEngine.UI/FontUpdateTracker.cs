using System.Collections.Generic;

namespace UnityEngine.UI;

public static class FontUpdateTracker
{
	private static Dictionary<Font, List<Text>> m_Tracked = new Dictionary<Font, List<Text>>();

	public static void TrackText(Text t)
	{
		if (t.font == null)
		{
			return;
		}
		m_Tracked.TryGetValue(t.font, out var value);
		if (value == null)
		{
			value = new List<Text>();
			m_Tracked.Add(t.font, value);
			Font.textureRebuilt += RebuildForFont;
		}
		for (int i = 0; i < value.Count; i++)
		{
			if (value[i] == t)
			{
				return;
			}
		}
		value.Add(t);
	}

	private static void RebuildForFont(Font f)
	{
		m_Tracked.TryGetValue(f, out var value);
		if (value != null)
		{
			for (int i = 0; i < value.Count; i++)
			{
				value[i].FontTextureChanged();
			}
		}
	}

	public static void UntrackText(Text t)
	{
		if (t.font == null)
		{
			return;
		}
		m_Tracked.TryGetValue(t.font, out var value);
		if (value != null)
		{
			value.Remove(t);
			if (value.Count == 0)
			{
				m_Tracked.Remove(t.font);
			}
		}
	}
}
