using System;
using System.Collections.Generic;

namespace UnityEngine.UI;

[AddComponentMenu("UI/Text", 10)]
public class Text : MaskableGraphic, ILayoutElement
{
	[SerializeField]
	private FontData m_FontData = FontData.defaultFontData;

	[SerializeField]
	[TextArea(3, 10)]
	protected string m_Text = string.Empty;

	private TextGenerator m_TextCache;

	private TextGenerator m_TextCacheForLayout;

	protected static Material s_DefaultText;

	[NonSerialized]
	protected bool m_DisableFontTextureRebuiltCallback;

	private readonly UIVertex[] m_TempVerts = new UIVertex[4];

	public TextGenerator cachedTextGenerator => m_TextCache ?? (m_TextCache = ((m_Text.Length == 0) ? new TextGenerator() : new TextGenerator(m_Text.Length)));

	public TextGenerator cachedTextGeneratorForLayout => m_TextCacheForLayout ?? (m_TextCacheForLayout = new TextGenerator());

	public override Texture mainTexture
	{
		get
		{
			if (font != null && font.material != null && font.material.mainTexture != null)
			{
				return font.material.mainTexture;
			}
			if (m_Material != null)
			{
				return m_Material.mainTexture;
			}
			return base.mainTexture;
		}
	}

	public Font font
	{
		get
		{
			return m_FontData.font;
		}
		set
		{
			if (!(m_FontData.font == value))
			{
				FontUpdateTracker.UntrackText(this);
				m_FontData.font = value;
				FontUpdateTracker.TrackText(this);
				SetAllDirty();
			}
		}
	}

	public virtual string text
	{
		get
		{
			return m_Text;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(m_Text))
				{
					m_Text = string.Empty;
					SetVerticesDirty();
				}
			}
			else if (m_Text != value)
			{
				m_Text = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public bool supportRichText
	{
		get
		{
			return m_FontData.richText;
		}
		set
		{
			if (m_FontData.richText != value)
			{
				m_FontData.richText = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public bool resizeTextForBestFit
	{
		get
		{
			return m_FontData.bestFit;
		}
		set
		{
			if (m_FontData.bestFit != value)
			{
				m_FontData.bestFit = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public int resizeTextMinSize
	{
		get
		{
			return m_FontData.minSize;
		}
		set
		{
			if (m_FontData.minSize != value)
			{
				m_FontData.minSize = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public int resizeTextMaxSize
	{
		get
		{
			return m_FontData.maxSize;
		}
		set
		{
			if (m_FontData.maxSize != value)
			{
				m_FontData.maxSize = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public TextAnchor alignment
	{
		get
		{
			return m_FontData.alignment;
		}
		set
		{
			if (m_FontData.alignment != value)
			{
				m_FontData.alignment = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public int fontSize
	{
		get
		{
			return m_FontData.fontSize;
		}
		set
		{
			if (m_FontData.fontSize != value)
			{
				m_FontData.fontSize = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public HorizontalWrapMode horizontalOverflow
	{
		get
		{
			return m_FontData.horizontalOverflow;
		}
		set
		{
			if (m_FontData.horizontalOverflow != value)
			{
				m_FontData.horizontalOverflow = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public VerticalWrapMode verticalOverflow
	{
		get
		{
			return m_FontData.verticalOverflow;
		}
		set
		{
			if (m_FontData.verticalOverflow != value)
			{
				m_FontData.verticalOverflow = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public float lineSpacing
	{
		get
		{
			return m_FontData.lineSpacing;
		}
		set
		{
			if (m_FontData.lineSpacing != value)
			{
				m_FontData.lineSpacing = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public FontStyle fontStyle
	{
		get
		{
			return m_FontData.fontStyle;
		}
		set
		{
			if (m_FontData.fontStyle != value)
			{
				m_FontData.fontStyle = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public float pixelsPerUnit
	{
		get
		{
			Canvas canvas = base.canvas;
			if (!canvas)
			{
				return 1f;
			}
			if (!font || font.dynamic)
			{
				return canvas.scaleFactor;
			}
			if (m_FontData.fontSize <= 0 || font.fontSize <= 0)
			{
				return 1f;
			}
			return (float)font.fontSize / (float)m_FontData.fontSize;
		}
	}

	public virtual float minWidth => 0f;

	public virtual float preferredWidth
	{
		get
		{
			TextGenerationSettings generationSettings = GetGenerationSettings(Vector2.zero);
			return cachedTextGeneratorForLayout.GetPreferredWidth(m_Text, generationSettings) / pixelsPerUnit;
		}
	}

	public virtual float flexibleWidth => -1f;

	public virtual float minHeight => 0f;

	public virtual float preferredHeight
	{
		get
		{
			TextGenerationSettings generationSettings = GetGenerationSettings(new Vector2(base.rectTransform.rect.size.x, 0f));
			return cachedTextGeneratorForLayout.GetPreferredHeight(m_Text, generationSettings) / pixelsPerUnit;
		}
	}

	public virtual float flexibleHeight => -1f;

	public virtual int layoutPriority => 0;

	protected Text()
	{
		base.useLegacyMeshGeneration = false;
	}

	public void FontTextureChanged()
	{
		if (!this)
		{
			FontUpdateTracker.UntrackText(this);
		}
		else
		{
			if (m_DisableFontTextureRebuiltCallback)
			{
				return;
			}
			cachedTextGenerator.Invalidate();
			if (IsActive())
			{
				if (CanvasUpdateRegistry.IsRebuildingGraphics() || CanvasUpdateRegistry.IsRebuildingLayout())
				{
					UpdateGeometry();
				}
				else
				{
					SetAllDirty();
				}
			}
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		cachedTextGenerator.Invalidate();
		FontUpdateTracker.TrackText(this);
	}

	protected override void OnDisable()
	{
		FontUpdateTracker.UntrackText(this);
		base.OnDisable();
	}

	protected override void UpdateGeometry()
	{
		if (font != null)
		{
			base.UpdateGeometry();
		}
	}

	public TextGenerationSettings GetGenerationSettings(Vector2 extents)
	{
		TextGenerationSettings result = new TextGenerationSettings
		{
			generationExtents = extents
		};
		if (font != null && font.dynamic)
		{
			result.fontSize = m_FontData.fontSize;
			result.resizeTextMinSize = m_FontData.minSize;
			result.resizeTextMaxSize = m_FontData.maxSize;
		}
		result.textAnchor = m_FontData.alignment;
		result.scaleFactor = pixelsPerUnit;
		result.color = base.color;
		result.font = font;
		result.pivot = base.rectTransform.pivot;
		result.richText = m_FontData.richText;
		result.lineSpacing = m_FontData.lineSpacing;
		result.fontStyle = m_FontData.fontStyle;
		result.resizeTextForBestFit = m_FontData.bestFit;
		result.updateBounds = false;
		result.horizontalOverflow = m_FontData.horizontalOverflow;
		result.verticalOverflow = m_FontData.verticalOverflow;
		return result;
	}

	public static Vector2 GetTextAnchorPivot(TextAnchor anchor)
	{
		return anchor switch
		{
			TextAnchor.LowerLeft => new Vector2(0f, 0f), 
			TextAnchor.LowerCenter => new Vector2(0.5f, 0f), 
			TextAnchor.LowerRight => new Vector2(1f, 0f), 
			TextAnchor.MiddleLeft => new Vector2(0f, 0.5f), 
			TextAnchor.MiddleCenter => new Vector2(0.5f, 0.5f), 
			TextAnchor.MiddleRight => new Vector2(1f, 0.5f), 
			TextAnchor.UpperLeft => new Vector2(0f, 1f), 
			TextAnchor.UpperCenter => new Vector2(0.5f, 1f), 
			TextAnchor.UpperRight => new Vector2(1f, 1f), 
			_ => Vector2.zero, 
		};
	}

	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		if (font == null)
		{
			return;
		}
		m_DisableFontTextureRebuiltCallback = true;
		Vector2 size = base.rectTransform.rect.size;
		TextGenerationSettings generationSettings = GetGenerationSettings(size);
		cachedTextGenerator.Populate(text, generationSettings);
		Rect rect = base.rectTransform.rect;
		Vector2 textAnchorPivot = GetTextAnchorPivot(m_FontData.alignment);
		Vector2 zero = Vector2.zero;
		zero.x = ((textAnchorPivot.x != 1f) ? rect.xMin : rect.xMax);
		zero.y = ((textAnchorPivot.y != 0f) ? rect.yMax : rect.yMin);
		Vector2 vector = PixelAdjustPoint(zero) - zero;
		IList<UIVertex> verts = cachedTextGenerator.verts;
		float num = 1f / pixelsPerUnit;
		int num2 = verts.Count - 4;
		toFill.Clear();
		if (vector != Vector2.zero)
		{
			for (int i = 0; i < num2; i++)
			{
				int num3 = i & 3;
				ref UIVertex reference = ref m_TempVerts[num3];
				reference = verts[i];
				m_TempVerts[num3].position *= num;
				m_TempVerts[num3].position.x += vector.x;
				m_TempVerts[num3].position.y += vector.y;
				if (num3 == 3)
				{
					toFill.AddUIVertexQuad(m_TempVerts);
				}
			}
		}
		else
		{
			for (int j = 0; j < num2; j++)
			{
				int num4 = j & 3;
				ref UIVertex reference2 = ref m_TempVerts[num4];
				reference2 = verts[j];
				m_TempVerts[num4].position *= num;
				if (num4 == 3)
				{
					toFill.AddUIVertexQuad(m_TempVerts);
				}
			}
		}
		m_DisableFontTextureRebuiltCallback = false;
	}

	public virtual void CalculateLayoutInputHorizontal()
	{
	}

	public virtual void CalculateLayoutInputVertical()
	{
	}
}
