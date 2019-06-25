using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[AddComponentMenu("Layout/Canvas Scaler", 101)]
	[RequireComponent(typeof(Canvas))]
	[ExecuteInEditMode]
	public class CanvasScaler : UIBehaviour
	{
		public enum ScaleMode
		{
			ConstantPixelSize,
			ScaleWithScreenSize,
			ConstantPhysicalSize
		}

		public enum ScreenMatchMode
		{
			MatchWidthOrHeight,
			Expand,
			Shrink
		}

		public enum Unit
		{
			Centimeters,
			Millimeters,
			Inches,
			Points,
			Picas
		}

		private const float kLogBase = 2f;

		[Tooltip("Determines how UI elements in the Canvas are scaled.")]
		[SerializeField]
		private ScaleMode m_UiScaleMode;

		[SerializeField]
		[Tooltip("If a sprite has this 'Pixels Per Unit' setting, then one pixel in the sprite will cover one unit in the UI.")]
		protected float m_ReferencePixelsPerUnit = 100f;

		[Tooltip("Scales all UI elements in the Canvas by this factor.")]
		[SerializeField]
		protected float m_ScaleFactor = 1f;

		[SerializeField]
		[Tooltip("The resolution the UI layout is designed for. If the screen resolution is larger, the UI will be scaled up, and if it's smaller, the UI will be scaled down. This is done in accordance with the Screen Match Mode.")]
		protected Vector2 m_ReferenceResolution = new Vector2(800f, 600f);

		[Tooltip("A mode used to scale the canvas area if the aspect ratio of the current resolution doesn't fit the reference resolution.")]
		[SerializeField]
		protected ScreenMatchMode m_ScreenMatchMode;

		[Range(0f, 1f)]
		[Tooltip("Determines if the scaling is using the width or height as reference, or a mix in between.")]
		[SerializeField]
		protected float m_MatchWidthOrHeight;

		[SerializeField]
		[Tooltip("The physical unit to specify positions and sizes in.")]
		protected Unit m_PhysicalUnit = Unit.Points;

		[Tooltip("The DPI to assume if the screen DPI is not known.")]
		[SerializeField]
		protected float m_FallbackScreenDPI = 96f;

		[Tooltip("The pixels per inch to use for sprites that have a 'Pixels Per Unit' setting that matches the 'Reference Pixels Per Unit' setting.")]
		[SerializeField]
		protected float m_DefaultSpriteDPI = 96f;

		[SerializeField]
		[Tooltip("The amount of pixels per unit to use for dynamically created bitmaps in the UI, such as Text.")]
		protected float m_DynamicPixelsPerUnit = 1f;

		private Canvas m_Canvas;

		[NonSerialized]
		private float m_PrevScaleFactor = 1f;

		[NonSerialized]
		private float m_PrevReferencePixelsPerUnit = 100f;

		public ScaleMode uiScaleMode
		{
			get
			{
				return m_UiScaleMode;
			}
			set
			{
				m_UiScaleMode = value;
			}
		}

		public float referencePixelsPerUnit
		{
			get
			{
				return m_ReferencePixelsPerUnit;
			}
			set
			{
				m_ReferencePixelsPerUnit = value;
			}
		}

		public float scaleFactor
		{
			get
			{
				return m_ScaleFactor;
			}
			set
			{
				m_ScaleFactor = value;
			}
		}

		public Vector2 referenceResolution
		{
			get
			{
				return m_ReferenceResolution;
			}
			set
			{
				m_ReferenceResolution = value;
			}
		}

		public ScreenMatchMode screenMatchMode
		{
			get
			{
				return m_ScreenMatchMode;
			}
			set
			{
				m_ScreenMatchMode = value;
			}
		}

		public float matchWidthOrHeight
		{
			get
			{
				return m_MatchWidthOrHeight;
			}
			set
			{
				m_MatchWidthOrHeight = value;
			}
		}

		public Unit physicalUnit
		{
			get
			{
				return m_PhysicalUnit;
			}
			set
			{
				m_PhysicalUnit = value;
			}
		}

		public float fallbackScreenDPI
		{
			get
			{
				return m_FallbackScreenDPI;
			}
			set
			{
				m_FallbackScreenDPI = value;
			}
		}

		public float defaultSpriteDPI
		{
			get
			{
				return m_DefaultSpriteDPI;
			}
			set
			{
				m_DefaultSpriteDPI = value;
			}
		}

		public float dynamicPixelsPerUnit
		{
			get
			{
				return m_DynamicPixelsPerUnit;
			}
			set
			{
				m_DynamicPixelsPerUnit = value;
			}
		}

		protected CanvasScaler()
		{
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			m_Canvas = GetComponent<Canvas>();
			Handle();
		}

		protected override void OnDisable()
		{
			SetScaleFactor(1f);
			SetReferencePixelsPerUnit(100f);
			base.OnDisable();
		}

		protected virtual void Update()
		{
			Handle();
		}

		protected virtual void Handle()
		{
			if (m_Canvas == null || !m_Canvas.isRootCanvas)
			{
				return;
			}
			if (m_Canvas.renderMode == RenderMode.WorldSpace)
			{
				HandleWorldCanvas();
				return;
			}
			switch (m_UiScaleMode)
			{
			case ScaleMode.ConstantPixelSize:
				HandleConstantPixelSize();
				break;
			case ScaleMode.ScaleWithScreenSize:
				HandleScaleWithScreenSize();
				break;
			case ScaleMode.ConstantPhysicalSize:
				HandleConstantPhysicalSize();
				break;
			}
		}

		protected virtual void HandleWorldCanvas()
		{
			SetScaleFactor(m_DynamicPixelsPerUnit);
			SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit);
		}

		protected virtual void HandleConstantPixelSize()
		{
			SetScaleFactor(m_ScaleFactor);
			SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit);
		}

		protected virtual void HandleScaleWithScreenSize()
		{
			Vector2 vector = new Vector2(Screen.width, Screen.height);
			float scaleFactor = 0f;
			switch (m_ScreenMatchMode)
			{
			case ScreenMatchMode.MatchWidthOrHeight:
			{
				float a = Mathf.Log(vector.x / m_ReferenceResolution.x, 2f);
				float b = Mathf.Log(vector.y / m_ReferenceResolution.y, 2f);
				float p = Mathf.Lerp(a, b, m_MatchWidthOrHeight);
				scaleFactor = Mathf.Pow(2f, p);
				break;
			}
			case ScreenMatchMode.Expand:
				scaleFactor = Mathf.Min(vector.x / m_ReferenceResolution.x, vector.y / m_ReferenceResolution.y);
				break;
			case ScreenMatchMode.Shrink:
				scaleFactor = Mathf.Max(vector.x / m_ReferenceResolution.x, vector.y / m_ReferenceResolution.y);
				break;
			}
			SetScaleFactor(scaleFactor);
			SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit);
		}

		protected virtual void HandleConstantPhysicalSize()
		{
			float dpi = Screen.dpi;
			float num = (dpi != 0f) ? dpi : m_FallbackScreenDPI;
			float num2 = 1f;
			switch (m_PhysicalUnit)
			{
			case Unit.Centimeters:
				num2 = 2.54f;
				break;
			case Unit.Millimeters:
				num2 = 25.4f;
				break;
			case Unit.Inches:
				num2 = 1f;
				break;
			case Unit.Points:
				num2 = 72f;
				break;
			case Unit.Picas:
				num2 = 6f;
				break;
			}
			SetScaleFactor(num / num2);
			SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit * num2 / m_DefaultSpriteDPI);
		}

		protected void SetScaleFactor(float scaleFactor)
		{
			if (scaleFactor != m_PrevScaleFactor)
			{
				m_Canvas.scaleFactor = scaleFactor;
				m_PrevScaleFactor = scaleFactor;
			}
		}

		protected void SetReferencePixelsPerUnit(float referencePixelsPerUnit)
		{
			if (referencePixelsPerUnit != m_PrevReferencePixelsPerUnit)
			{
				m_Canvas.referencePixelsPerUnit = referencePixelsPerUnit;
				m_PrevReferencePixelsPerUnit = referencePixelsPerUnit;
			}
		}
	}
}
