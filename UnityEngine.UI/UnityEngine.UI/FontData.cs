using System;
using UnityEngine.Serialization;

namespace UnityEngine.UI;

[Serializable]
public class FontData : ISerializationCallbackReceiver
{
	[FormerlySerializedAs("font")]
	[SerializeField]
	private Font m_Font;

	[FormerlySerializedAs("fontSize")]
	[SerializeField]
	private int m_FontSize;

	[FormerlySerializedAs("fontStyle")]
	[SerializeField]
	private FontStyle m_FontStyle;

	[SerializeField]
	private bool m_BestFit;

	[SerializeField]
	private int m_MinSize;

	[SerializeField]
	private int m_MaxSize;

	[FormerlySerializedAs("alignment")]
	[SerializeField]
	private TextAnchor m_Alignment;

	[SerializeField]
	[FormerlySerializedAs("richText")]
	private bool m_RichText;

	[SerializeField]
	private HorizontalWrapMode m_HorizontalOverflow;

	[SerializeField]
	private VerticalWrapMode m_VerticalOverflow;

	[SerializeField]
	private float m_LineSpacing;

	public static FontData defaultFontData
	{
		get
		{
			FontData fontData = new FontData();
			fontData.m_FontSize = 14;
			fontData.m_LineSpacing = 1f;
			fontData.m_FontStyle = FontStyle.Normal;
			fontData.m_BestFit = false;
			fontData.m_MinSize = 10;
			fontData.m_MaxSize = 40;
			fontData.m_Alignment = TextAnchor.UpperLeft;
			fontData.m_HorizontalOverflow = HorizontalWrapMode.Wrap;
			fontData.m_VerticalOverflow = VerticalWrapMode.Truncate;
			fontData.m_RichText = true;
			return fontData;
		}
	}

	public Font font
	{
		get
		{
			return m_Font;
		}
		set
		{
			m_Font = value;
		}
	}

	public int fontSize
	{
		get
		{
			return m_FontSize;
		}
		set
		{
			m_FontSize = value;
		}
	}

	public FontStyle fontStyle
	{
		get
		{
			return m_FontStyle;
		}
		set
		{
			m_FontStyle = value;
		}
	}

	public bool bestFit
	{
		get
		{
			return m_BestFit;
		}
		set
		{
			m_BestFit = value;
		}
	}

	public int minSize
	{
		get
		{
			return m_MinSize;
		}
		set
		{
			m_MinSize = value;
		}
	}

	public int maxSize
	{
		get
		{
			return m_MaxSize;
		}
		set
		{
			m_MaxSize = value;
		}
	}

	public TextAnchor alignment
	{
		get
		{
			return m_Alignment;
		}
		set
		{
			m_Alignment = value;
		}
	}

	public bool richText
	{
		get
		{
			return m_RichText;
		}
		set
		{
			m_RichText = value;
		}
	}

	public HorizontalWrapMode horizontalOverflow
	{
		get
		{
			return m_HorizontalOverflow;
		}
		set
		{
			m_HorizontalOverflow = value;
		}
	}

	public VerticalWrapMode verticalOverflow
	{
		get
		{
			return m_VerticalOverflow;
		}
		set
		{
			m_VerticalOverflow = value;
		}
	}

	public float lineSpacing
	{
		get
		{
			return m_LineSpacing;
		}
		set
		{
			m_LineSpacing = value;
		}
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		m_FontSize = Mathf.Clamp(m_FontSize, 0, 300);
		m_MinSize = Mathf.Clamp(m_MinSize, 0, 300);
		m_MaxSize = Mathf.Clamp(m_MaxSize, 0, 300);
	}
}
