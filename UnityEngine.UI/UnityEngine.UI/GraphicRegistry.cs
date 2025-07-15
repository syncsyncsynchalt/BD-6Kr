using System.Collections.Generic;
using UnityEngine.UI.Collections;

namespace UnityEngine.UI;

public class GraphicRegistry
{
	private static GraphicRegistry s_Instance;

	private readonly Dictionary<Canvas, IndexedSet<Graphic>> m_Graphics = new Dictionary<Canvas, IndexedSet<Graphic>>();

	private static readonly List<Graphic> s_EmptyList = new List<Graphic>();

	public static GraphicRegistry instance
	{
		get
		{
			if (s_Instance == null)
			{
				s_Instance = new GraphicRegistry();
			}
			return s_Instance;
		}
	}

	protected GraphicRegistry()
	{
	}

	public static void RegisterGraphicForCanvas(Canvas c, Graphic graphic)
	{
		if (!(c == null))
		{
			instance.m_Graphics.TryGetValue(c, out var value);
			if (value != null)
			{
				value.Add(graphic);
				return;
			}
			value = new IndexedSet<Graphic>();
			value.Add(graphic);
			instance.m_Graphics.Add(c, value);
		}
	}

	public static void UnregisterGraphicForCanvas(Canvas c, Graphic graphic)
	{
		if (!(c == null) && instance.m_Graphics.TryGetValue(c, out var value))
		{
			value.Remove(graphic);
		}
	}

	public static IList<Graphic> GetGraphicsForCanvas(Canvas canvas)
	{
		if (instance.m_Graphics.TryGetValue(canvas, out var value))
		{
			return value;
		}
		return s_EmptyList;
	}
}
