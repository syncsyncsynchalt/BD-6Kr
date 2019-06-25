using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[RequireComponent(typeof(RectTransform))]
	[SelectionBase]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Scroll Rect", 37)]
	[ExecuteInEditMode]
	public class ScrollRect : UIBehaviour, IEventSystemHandler, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, IScrollHandler, ICanvasElement, ILayoutElement, ILayoutController, ILayoutGroup
	{
		public enum MovementType
		{
			Unrestricted,
			Elastic,
			Clamped
		}

		public enum ScrollbarVisibility
		{
			Permanent,
			AutoHide,
			AutoHideAndExpandViewport
		}

		[Serializable]
		public class ScrollRectEvent : UnityEvent<Vector2>
		{
		}

		[SerializeField]
		private RectTransform m_Content;

		[SerializeField]
		private bool m_Horizontal = true;

		[SerializeField]
		private bool m_Vertical = true;

		[SerializeField]
		private MovementType m_MovementType = MovementType.Elastic;

		[SerializeField]
		private float m_Elasticity = 0.1f;

		[SerializeField]
		private bool m_Inertia = true;

		[SerializeField]
		private float m_DecelerationRate = 0.135f;

		[SerializeField]
		private float m_ScrollSensitivity = 1f;

		[SerializeField]
		private RectTransform m_Viewport;

		[SerializeField]
		private Scrollbar m_HorizontalScrollbar;

		[SerializeField]
		private Scrollbar m_VerticalScrollbar;

		[SerializeField]
		private ScrollbarVisibility m_HorizontalScrollbarVisibility;

		[SerializeField]
		private ScrollbarVisibility m_VerticalScrollbarVisibility;

		[SerializeField]
		private float m_HorizontalScrollbarSpacing;

		[SerializeField]
		private float m_VerticalScrollbarSpacing;

		[SerializeField]
		private ScrollRectEvent m_OnValueChanged = new ScrollRectEvent();

		private Vector2 m_PointerStartLocalCursor = Vector2.zero;

		private Vector2 m_ContentStartPosition = Vector2.zero;

		private RectTransform m_ViewRect;

		private Bounds m_ContentBounds;

		private Bounds m_ViewBounds;

		private Vector2 m_Velocity;

		private bool m_Dragging;

		private Vector2 m_PrevPosition = Vector2.zero;

		private Bounds m_PrevContentBounds;

		private Bounds m_PrevViewBounds;

		[NonSerialized]
		private bool m_HasRebuiltLayout;

		private bool m_HSliderExpand;

		private bool m_VSliderExpand;

		private float m_HSliderHeight;

		private float m_VSliderWidth;

		[NonSerialized]
		private RectTransform m_Rect;

		private RectTransform m_HorizontalScrollbarRect;

		private RectTransform m_VerticalScrollbarRect;

		private DrivenRectTransformTracker m_Tracker;

		private readonly Vector3[] m_Corners = new Vector3[4];

		public RectTransform content
		{
			get
			{
				return m_Content;
			}
			set
			{
				m_Content = value;
			}
		}

		public bool horizontal
		{
			get
			{
				return m_Horizontal;
			}
			set
			{
				m_Horizontal = value;
			}
		}

		public bool vertical
		{
			get
			{
				return m_Vertical;
			}
			set
			{
				m_Vertical = value;
			}
		}

		public MovementType movementType
		{
			get
			{
				return m_MovementType;
			}
			set
			{
				m_MovementType = value;
			}
		}

		public float elasticity
		{
			get
			{
				return m_Elasticity;
			}
			set
			{
				m_Elasticity = value;
			}
		}

		public bool inertia
		{
			get
			{
				return m_Inertia;
			}
			set
			{
				m_Inertia = value;
			}
		}

		public float decelerationRate
		{
			get
			{
				return m_DecelerationRate;
			}
			set
			{
				m_DecelerationRate = value;
			}
		}

		public float scrollSensitivity
		{
			get
			{
				return m_ScrollSensitivity;
			}
			set
			{
				m_ScrollSensitivity = value;
			}
		}

		public RectTransform viewport
		{
			get
			{
				return m_Viewport;
			}
			set
			{
				m_Viewport = value;
				SetDirtyCaching();
			}
		}

		public Scrollbar horizontalScrollbar
		{
			get
			{
				return m_HorizontalScrollbar;
			}
			set
			{
				if ((bool)m_HorizontalScrollbar)
				{
					m_HorizontalScrollbar.onValueChanged.RemoveListener(SetHorizontalNormalizedPosition);
				}
				m_HorizontalScrollbar = value;
				if ((bool)m_HorizontalScrollbar)
				{
					m_HorizontalScrollbar.onValueChanged.AddListener(SetHorizontalNormalizedPosition);
				}
				SetDirtyCaching();
			}
		}

		public Scrollbar verticalScrollbar
		{
			get
			{
				return m_VerticalScrollbar;
			}
			set
			{
				if ((bool)m_VerticalScrollbar)
				{
					m_VerticalScrollbar.onValueChanged.RemoveListener(SetVerticalNormalizedPosition);
				}
				m_VerticalScrollbar = value;
				if ((bool)m_VerticalScrollbar)
				{
					m_VerticalScrollbar.onValueChanged.AddListener(SetVerticalNormalizedPosition);
				}
				SetDirtyCaching();
			}
		}

		public ScrollbarVisibility horizontalScrollbarVisibility
		{
			get
			{
				return m_HorizontalScrollbarVisibility;
			}
			set
			{
				m_HorizontalScrollbarVisibility = value;
				SetDirtyCaching();
			}
		}

		public ScrollbarVisibility verticalScrollbarVisibility
		{
			get
			{
				return m_VerticalScrollbarVisibility;
			}
			set
			{
				m_VerticalScrollbarVisibility = value;
				SetDirtyCaching();
			}
		}

		public float horizontalScrollbarSpacing
		{
			get
			{
				return m_HorizontalScrollbarSpacing;
			}
			set
			{
				m_HorizontalScrollbarSpacing = value;
				SetDirty();
			}
		}

		public float verticalScrollbarSpacing
		{
			get
			{
				return m_VerticalScrollbarSpacing;
			}
			set
			{
				m_VerticalScrollbarSpacing = value;
				SetDirty();
			}
		}

		public ScrollRectEvent onValueChanged
		{
			get
			{
				return m_OnValueChanged;
			}
			set
			{
				m_OnValueChanged = value;
			}
		}

		protected RectTransform viewRect
		{
			get
			{
				if (m_ViewRect == null)
				{
					m_ViewRect = m_Viewport;
				}
				if (m_ViewRect == null)
				{
					m_ViewRect = (RectTransform)base.transform;
				}
				return m_ViewRect;
			}
		}

		public Vector2 velocity
		{
			get
			{
				return m_Velocity;
			}
			set
			{
				m_Velocity = value;
			}
		}

		private RectTransform rectTransform
		{
			get
			{
				if (m_Rect == null)
				{
					m_Rect = GetComponent<RectTransform>();
				}
				return m_Rect;
			}
		}

		public Vector2 normalizedPosition
		{
			get
			{
				return new Vector2(horizontalNormalizedPosition, verticalNormalizedPosition);
			}
			set
			{
				SetNormalizedPosition(value.x, 0);
				SetNormalizedPosition(value.y, 1);
			}
		}

		public float horizontalNormalizedPosition
		{
			get
			{
				UpdateBounds();
				Vector3 size = m_ContentBounds.size;
				float x = size.x;
				Vector3 size2 = m_ViewBounds.size;
				if (x <= size2.x)
				{
					Vector3 min = m_ViewBounds.min;
					float x2 = min.x;
					Vector3 min2 = m_ContentBounds.min;
					return (x2 > min2.x) ? 1 : 0;
				}
				Vector3 min3 = m_ViewBounds.min;
				float x3 = min3.x;
				Vector3 min4 = m_ContentBounds.min;
				float num = x3 - min4.x;
				Vector3 size3 = m_ContentBounds.size;
				float x4 = size3.x;
				Vector3 size4 = m_ViewBounds.size;
				return num / (x4 - size4.x);
			}
			set
			{
				SetNormalizedPosition(value, 0);
			}
		}

		public float verticalNormalizedPosition
		{
			get
			{
				UpdateBounds();
				Vector3 size = m_ContentBounds.size;
				float y = size.y;
				Vector3 size2 = m_ViewBounds.size;
				if (y <= size2.y)
				{
					Vector3 min = m_ViewBounds.min;
					float y2 = min.y;
					Vector3 min2 = m_ContentBounds.min;
					return (y2 > min2.y) ? 1 : 0;
				}
				Vector3 min3 = m_ViewBounds.min;
				float y3 = min3.y;
				Vector3 min4 = m_ContentBounds.min;
				float num = y3 - min4.y;
				Vector3 size3 = m_ContentBounds.size;
				float y4 = size3.y;
				Vector3 size4 = m_ViewBounds.size;
				return num / (y4 - size4.y);
			}
			set
			{
				SetNormalizedPosition(value, 1);
			}
		}

		private bool hScrollingNeeded
		{
			get
			{
				if (Application.isPlaying)
				{
					Vector3 size = m_ContentBounds.size;
					float x = size.x;
					Vector3 size2 = m_ViewBounds.size;
					return x > size2.x + 0.01f;
				}
				return true;
			}
		}

		private bool vScrollingNeeded
		{
			get
			{
				if (Application.isPlaying)
				{
					Vector3 size = m_ContentBounds.size;
					float y = size.y;
					Vector3 size2 = m_ViewBounds.size;
					return y > size2.y + 0.01f;
				}
				return true;
			}
		}

		public virtual float minWidth => -1f;

		public virtual float preferredWidth => -1f;

		public virtual float flexibleWidth
		{
			get;
			private set;
		}

		public virtual float minHeight => -1f;

		public virtual float preferredHeight => -1f;

		public virtual float flexibleHeight => -1f;

		public virtual int layoutPriority => -1;

		protected ScrollRect()
		{
			flexibleWidth = -1f;
		}

		public virtual void Rebuild(CanvasUpdate executing)
		{
			if (executing == CanvasUpdate.Prelayout)
			{
				UpdateCachedData();
			}
			if (executing == CanvasUpdate.PostLayout)
			{
				UpdateBounds();
				UpdateScrollbars(Vector2.zero);
				UpdatePrevData();
				m_HasRebuiltLayout = true;
			}
		}

		public virtual void LayoutComplete()
		{
		}

		public virtual void GraphicUpdateComplete()
		{
		}

		private void UpdateCachedData()
		{
			Transform transform = base.transform;
			m_HorizontalScrollbarRect = ((!(m_HorizontalScrollbar == null)) ? (m_HorizontalScrollbar.transform as RectTransform) : null);
			m_VerticalScrollbarRect = ((!(m_VerticalScrollbar == null)) ? (m_VerticalScrollbar.transform as RectTransform) : null);
			bool flag = viewRect.parent == transform;
			bool flag2 = !m_HorizontalScrollbarRect || m_HorizontalScrollbarRect.parent == transform;
			bool flag3 = !m_VerticalScrollbarRect || m_VerticalScrollbarRect.parent == transform;
			bool flag4 = flag && flag2 && flag3;
			m_HSliderExpand = (flag4 && (bool)m_HorizontalScrollbarRect && horizontalScrollbarVisibility == ScrollbarVisibility.AutoHideAndExpandViewport);
			m_VSliderExpand = (flag4 && (bool)m_VerticalScrollbarRect && verticalScrollbarVisibility == ScrollbarVisibility.AutoHideAndExpandViewport);
			m_HSliderHeight = ((!(m_HorizontalScrollbarRect == null)) ? m_HorizontalScrollbarRect.rect.height : 0f);
			m_VSliderWidth = ((!(m_VerticalScrollbarRect == null)) ? m_VerticalScrollbarRect.rect.width : 0f);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if ((bool)m_HorizontalScrollbar)
			{
				m_HorizontalScrollbar.onValueChanged.AddListener(SetHorizontalNormalizedPosition);
			}
			if ((bool)m_VerticalScrollbar)
			{
				m_VerticalScrollbar.onValueChanged.AddListener(SetVerticalNormalizedPosition);
			}
			CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
		}

		protected override void OnDisable()
		{
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if ((bool)m_HorizontalScrollbar)
			{
				m_HorizontalScrollbar.onValueChanged.RemoveListener(SetHorizontalNormalizedPosition);
			}
			if ((bool)m_VerticalScrollbar)
			{
				m_VerticalScrollbar.onValueChanged.RemoveListener(SetVerticalNormalizedPosition);
			}
			m_HasRebuiltLayout = false;
			m_Tracker.Clear();
			m_Velocity = Vector2.zero;
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
			base.OnDisable();
		}

		public override bool IsActive()
		{
			return base.IsActive() && m_Content != null;
		}

		private void EnsureLayoutHasRebuilt()
		{
			if (!m_HasRebuiltLayout && !CanvasUpdateRegistry.IsRebuildingLayout())
			{
				Canvas.ForceUpdateCanvases();
			}
		}

		public virtual void StopMovement()
		{
			m_Velocity = Vector2.zero;
		}

		public virtual void OnScroll(PointerEventData data)
		{
			if (!IsActive())
			{
				return;
			}
			EnsureLayoutHasRebuilt();
			UpdateBounds();
			Vector2 scrollDelta = data.scrollDelta;
			scrollDelta.y *= -1f;
			if (vertical && !horizontal)
			{
				if (Mathf.Abs(scrollDelta.x) > Mathf.Abs(scrollDelta.y))
				{
					scrollDelta.y = scrollDelta.x;
				}
				scrollDelta.x = 0f;
			}
			if (horizontal && !vertical)
			{
				if (Mathf.Abs(scrollDelta.y) > Mathf.Abs(scrollDelta.x))
				{
					scrollDelta.x = scrollDelta.y;
				}
				scrollDelta.y = 0f;
			}
			Vector2 anchoredPosition = m_Content.anchoredPosition;
			anchoredPosition += scrollDelta * m_ScrollSensitivity;
			if (m_MovementType == MovementType.Clamped)
			{
				anchoredPosition += CalculateOffset(anchoredPosition - m_Content.anchoredPosition);
			}
			SetContentAnchoredPosition(anchoredPosition);
			UpdateBounds();
		}

		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				m_Velocity = Vector2.zero;
			}
		}

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left && IsActive())
			{
				UpdateBounds();
				m_PointerStartLocalCursor = Vector2.zero;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out m_PointerStartLocalCursor);
				m_ContentStartPosition = m_Content.anchoredPosition;
				m_Dragging = true;
			}
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				m_Dragging = false;
			}
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (eventData.button != 0 || !IsActive() || !RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
			{
				return;
			}
			UpdateBounds();
			Vector2 b = localPoint - m_PointerStartLocalCursor;
			Vector2 vector = m_ContentStartPosition + b;
			Vector2 b2 = CalculateOffset(vector - m_Content.anchoredPosition);
			vector += b2;
			if (m_MovementType == MovementType.Elastic)
			{
				if (b2.x != 0f)
				{
					float x = vector.x;
					float x2 = b2.x;
					Vector3 size = m_ViewBounds.size;
					vector.x = x - RubberDelta(x2, size.x);
				}
				if (b2.y != 0f)
				{
					float y = vector.y;
					float y2 = b2.y;
					Vector3 size2 = m_ViewBounds.size;
					vector.y = y - RubberDelta(y2, size2.y);
				}
			}
			SetContentAnchoredPosition(vector);
		}

		protected virtual void SetContentAnchoredPosition(Vector2 position)
		{
			if (!m_Horizontal)
			{
				Vector2 anchoredPosition = m_Content.anchoredPosition;
				position.x = anchoredPosition.x;
			}
			if (!m_Vertical)
			{
				Vector2 anchoredPosition2 = m_Content.anchoredPosition;
				position.y = anchoredPosition2.y;
			}
			if (position != m_Content.anchoredPosition)
			{
				m_Content.anchoredPosition = position;
				UpdateBounds();
			}
		}

		protected virtual void LateUpdate()
		{
			if (!m_Content)
			{
				return;
			}
			EnsureLayoutHasRebuilt();
			UpdateScrollbarVisibility();
			UpdateBounds();
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			Vector2 vector = CalculateOffset(Vector2.zero);
			if (!m_Dragging && (vector != Vector2.zero || m_Velocity != Vector2.zero))
			{
				Vector2 vector2 = m_Content.anchoredPosition;
				for (int i = 0; i < 2; i++)
				{
					if (m_MovementType == MovementType.Elastic && vector[i] != 0f)
					{
						float currentVelocity = m_Velocity[i];
						vector2[i] = Mathf.SmoothDamp(m_Content.anchoredPosition[i], m_Content.anchoredPosition[i] + vector[i], ref currentVelocity, m_Elasticity, float.PositiveInfinity, unscaledDeltaTime);
						m_Velocity[i] = currentVelocity;
					}
					else if (m_Inertia)
					{
						ref Vector2 velocity = ref m_Velocity;
						ref Vector2 reference = ref velocity;
						int index;
						int index2 = index = i;
						float num = reference[index];
						velocity[index2] = num * Mathf.Pow(m_DecelerationRate, unscaledDeltaTime);
						if (Mathf.Abs(m_Velocity[i]) < 1f)
						{
							m_Velocity[i] = 0f;
						}
						int index3 = index = i;
						num = vector2[index];
						vector2[index3] = num + m_Velocity[i] * unscaledDeltaTime;
					}
					else
					{
						m_Velocity[i] = 0f;
					}
				}
				if (m_Velocity != Vector2.zero)
				{
					if (m_MovementType == MovementType.Clamped)
					{
						vector = CalculateOffset(vector2 - m_Content.anchoredPosition);
						vector2 += vector;
					}
					SetContentAnchoredPosition(vector2);
				}
			}
			if (m_Dragging && m_Inertia)
			{
				Vector3 b = (m_Content.anchoredPosition - m_PrevPosition) / unscaledDeltaTime;
				m_Velocity = Vector3.Lerp(m_Velocity, b, unscaledDeltaTime * 10f);
			}
			if (m_ViewBounds != m_PrevViewBounds || m_ContentBounds != m_PrevContentBounds || m_Content.anchoredPosition != m_PrevPosition)
			{
				UpdateScrollbars(vector);
				m_OnValueChanged.Invoke(normalizedPosition);
				UpdatePrevData();
			}
		}

		private void UpdatePrevData()
		{
			if (m_Content == null)
			{
				m_PrevPosition = Vector2.zero;
			}
			else
			{
				m_PrevPosition = m_Content.anchoredPosition;
			}
			m_PrevViewBounds = m_ViewBounds;
			m_PrevContentBounds = m_ContentBounds;
		}

		private void UpdateScrollbars(Vector2 offset)
		{
			if ((bool)m_HorizontalScrollbar)
			{
				Vector3 size = m_ContentBounds.size;
				if (size.x > 0f)
				{
					Scrollbar horizontalScrollbar = m_HorizontalScrollbar;
					Vector3 size2 = m_ViewBounds.size;
					float num = size2.x - Mathf.Abs(offset.x);
					Vector3 size3 = m_ContentBounds.size;
					horizontalScrollbar.size = Mathf.Clamp01(num / size3.x);
				}
				else
				{
					m_HorizontalScrollbar.size = 1f;
				}
				m_HorizontalScrollbar.value = horizontalNormalizedPosition;
			}
			if ((bool)m_VerticalScrollbar)
			{
				Vector3 size4 = m_ContentBounds.size;
				if (size4.y > 0f)
				{
					Scrollbar verticalScrollbar = m_VerticalScrollbar;
					Vector3 size5 = m_ViewBounds.size;
					float num2 = size5.y - Mathf.Abs(offset.y);
					Vector3 size6 = m_ContentBounds.size;
					verticalScrollbar.size = Mathf.Clamp01(num2 / size6.y);
				}
				else
				{
					m_VerticalScrollbar.size = 1f;
				}
				m_VerticalScrollbar.value = verticalNormalizedPosition;
			}
		}

		private void SetHorizontalNormalizedPosition(float value)
		{
			SetNormalizedPosition(value, 0);
		}

		private void SetVerticalNormalizedPosition(float value)
		{
			SetNormalizedPosition(value, 1);
		}

		private void SetNormalizedPosition(float value, int axis)
		{
			EnsureLayoutHasRebuilt();
			UpdateBounds();
			float num = m_ContentBounds.size[axis] - m_ViewBounds.size[axis];
			float num2 = m_ViewBounds.min[axis] - value * num;
			float num3 = m_Content.localPosition[axis] + num2 - m_ContentBounds.min[axis];
			Vector3 localPosition = m_Content.localPosition;
			if (Mathf.Abs(localPosition[axis] - num3) > 0.01f)
			{
				localPosition[axis] = num3;
				m_Content.localPosition = localPosition;
				m_Velocity[axis] = 0f;
				UpdateBounds();
			}
		}

		private static float RubberDelta(float overStretching, float viewSize)
		{
			return (1f - 1f / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1f)) * viewSize * Mathf.Sign(overStretching);
		}

		protected override void OnRectTransformDimensionsChange()
		{
			SetDirty();
		}

		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		public virtual void CalculateLayoutInputVertical()
		{
		}

		public virtual void SetLayoutHorizontal()
		{
			m_Tracker.Clear();
			if (m_HSliderExpand || m_VSliderExpand)
			{
				m_Tracker.Add(this, this.viewRect, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
				this.viewRect.anchorMin = Vector2.zero;
				this.viewRect.anchorMax = Vector2.one;
				this.viewRect.sizeDelta = Vector2.zero;
				this.viewRect.anchoredPosition = Vector2.zero;
				LayoutRebuilder.ForceRebuildLayoutImmediate(content);
				m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
				m_ContentBounds = GetBounds();
			}
			if (m_VSliderExpand && vScrollingNeeded)
			{
				RectTransform viewRect = this.viewRect;
				float x = 0f - (m_VSliderWidth + m_VerticalScrollbarSpacing);
				Vector2 sizeDelta = this.viewRect.sizeDelta;
				viewRect.sizeDelta = new Vector2(x, sizeDelta.y);
				LayoutRebuilder.ForceRebuildLayoutImmediate(content);
				m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
				m_ContentBounds = GetBounds();
			}
			if (m_HSliderExpand && hScrollingNeeded)
			{
				RectTransform viewRect2 = this.viewRect;
				Vector2 sizeDelta2 = this.viewRect.sizeDelta;
				viewRect2.sizeDelta = new Vector2(sizeDelta2.x, 0f - (m_HSliderHeight + m_HorizontalScrollbarSpacing));
				m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
				m_ContentBounds = GetBounds();
			}
			if (!m_VSliderExpand || !vScrollingNeeded)
			{
				return;
			}
			Vector2 sizeDelta3 = this.viewRect.sizeDelta;
			if (sizeDelta3.x == 0f)
			{
				Vector2 sizeDelta4 = this.viewRect.sizeDelta;
				if (sizeDelta4.y < 0f)
				{
					RectTransform viewRect3 = this.viewRect;
					float x2 = 0f - (m_VSliderWidth + m_VerticalScrollbarSpacing);
					Vector2 sizeDelta5 = this.viewRect.sizeDelta;
					viewRect3.sizeDelta = new Vector2(x2, sizeDelta5.y);
				}
			}
		}

		public virtual void SetLayoutVertical()
		{
			UpdateScrollbarLayout();
			m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
			m_ContentBounds = GetBounds();
		}

		private void UpdateScrollbarVisibility()
		{
			if ((bool)m_VerticalScrollbar && m_VerticalScrollbarVisibility != 0 && m_VerticalScrollbar.gameObject.activeSelf != vScrollingNeeded)
			{
				m_VerticalScrollbar.gameObject.SetActive(vScrollingNeeded);
			}
			if ((bool)m_HorizontalScrollbar && m_HorizontalScrollbarVisibility != 0 && m_HorizontalScrollbar.gameObject.activeSelf != hScrollingNeeded)
			{
				m_HorizontalScrollbar.gameObject.SetActive(hScrollingNeeded);
			}
		}

		private void UpdateScrollbarLayout()
		{
			if (m_VSliderExpand && (bool)m_HorizontalScrollbar)
			{
				m_Tracker.Add(this, m_HorizontalScrollbarRect, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.SizeDeltaX);
				RectTransform horizontalScrollbarRect = m_HorizontalScrollbarRect;
				Vector2 anchorMin = m_HorizontalScrollbarRect.anchorMin;
				horizontalScrollbarRect.anchorMin = new Vector2(0f, anchorMin.y);
				RectTransform horizontalScrollbarRect2 = m_HorizontalScrollbarRect;
				Vector2 anchorMax = m_HorizontalScrollbarRect.anchorMax;
				horizontalScrollbarRect2.anchorMax = new Vector2(1f, anchorMax.y);
				RectTransform horizontalScrollbarRect3 = m_HorizontalScrollbarRect;
				Vector2 anchoredPosition = m_HorizontalScrollbarRect.anchoredPosition;
				horizontalScrollbarRect3.anchoredPosition = new Vector2(0f, anchoredPosition.y);
				if (vScrollingNeeded)
				{
					RectTransform horizontalScrollbarRect4 = m_HorizontalScrollbarRect;
					float x = 0f - (m_VSliderWidth + m_VerticalScrollbarSpacing);
					Vector2 sizeDelta = m_HorizontalScrollbarRect.sizeDelta;
					horizontalScrollbarRect4.sizeDelta = new Vector2(x, sizeDelta.y);
				}
				else
				{
					RectTransform horizontalScrollbarRect5 = m_HorizontalScrollbarRect;
					Vector2 sizeDelta2 = m_HorizontalScrollbarRect.sizeDelta;
					horizontalScrollbarRect5.sizeDelta = new Vector2(0f, sizeDelta2.y);
				}
			}
			if (m_HSliderExpand && (bool)m_VerticalScrollbar)
			{
				m_Tracker.Add(this, m_VerticalScrollbarRect, DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaY);
				RectTransform verticalScrollbarRect = m_VerticalScrollbarRect;
				Vector2 anchorMin2 = m_VerticalScrollbarRect.anchorMin;
				verticalScrollbarRect.anchorMin = new Vector2(anchorMin2.x, 0f);
				RectTransform verticalScrollbarRect2 = m_VerticalScrollbarRect;
				Vector2 anchorMax2 = m_VerticalScrollbarRect.anchorMax;
				verticalScrollbarRect2.anchorMax = new Vector2(anchorMax2.x, 1f);
				RectTransform verticalScrollbarRect3 = m_VerticalScrollbarRect;
				Vector2 anchoredPosition2 = m_VerticalScrollbarRect.anchoredPosition;
				verticalScrollbarRect3.anchoredPosition = new Vector2(anchoredPosition2.x, 0f);
				if (hScrollingNeeded)
				{
					RectTransform verticalScrollbarRect4 = m_VerticalScrollbarRect;
					Vector2 sizeDelta3 = m_VerticalScrollbarRect.sizeDelta;
					verticalScrollbarRect4.sizeDelta = new Vector2(sizeDelta3.x, 0f - (m_HSliderHeight + m_HorizontalScrollbarSpacing));
				}
				else
				{
					RectTransform verticalScrollbarRect5 = m_VerticalScrollbarRect;
					Vector2 sizeDelta4 = m_VerticalScrollbarRect.sizeDelta;
					verticalScrollbarRect5.sizeDelta = new Vector2(sizeDelta4.x, 0f);
				}
			}
		}

		private void UpdateBounds()
		{
			m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
			m_ContentBounds = GetBounds();
			if (!(m_Content == null))
			{
				Vector3 size = m_ContentBounds.size;
				Vector3 center = m_ContentBounds.center;
				Vector3 vector = m_ViewBounds.size - size;
				if (vector.x > 0f)
				{
					float x = center.x;
					float x2 = vector.x;
					Vector2 pivot = m_Content.pivot;
					center.x = x - x2 * (pivot.x - 0.5f);
					Vector3 size2 = m_ViewBounds.size;
					size.x = size2.x;
				}
				if (vector.y > 0f)
				{
					float y = center.y;
					float y2 = vector.y;
					Vector2 pivot2 = m_Content.pivot;
					center.y = y - y2 * (pivot2.y - 0.5f);
					Vector3 size3 = m_ViewBounds.size;
					size.y = size3.y;
				}
				m_ContentBounds.size = size;
				m_ContentBounds.center = center;
			}
		}

		private Bounds GetBounds()
		{
			if (m_Content == null)
			{
				return default(Bounds);
			}
			Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			Matrix4x4 worldToLocalMatrix = viewRect.worldToLocalMatrix;
			m_Content.GetWorldCorners(m_Corners);
			for (int i = 0; i < 4; i++)
			{
				Vector3 lhs = worldToLocalMatrix.MultiplyPoint3x4(m_Corners[i]);
				vector = Vector3.Min(lhs, vector);
				vector2 = Vector3.Max(lhs, vector2);
			}
			Bounds result = new Bounds(vector, Vector3.zero);
			result.Encapsulate(vector2);
			return result;
		}

		private Vector2 CalculateOffset(Vector2 delta)
		{
			Vector2 zero = Vector2.zero;
			if (m_MovementType == MovementType.Unrestricted)
			{
				return zero;
			}
			Vector2 vector = m_ContentBounds.min;
			Vector2 vector2 = m_ContentBounds.max;
			if (m_Horizontal)
			{
				vector.x += delta.x;
				vector2.x += delta.x;
				float x = vector.x;
				Vector3 min = m_ViewBounds.min;
				if (x > min.x)
				{
					Vector3 min2 = m_ViewBounds.min;
					zero.x = min2.x - vector.x;
				}
				else
				{
					float x2 = vector2.x;
					Vector3 max = m_ViewBounds.max;
					if (x2 < max.x)
					{
						Vector3 max2 = m_ViewBounds.max;
						zero.x = max2.x - vector2.x;
					}
				}
			}
			if (m_Vertical)
			{
				vector.y += delta.y;
				vector2.y += delta.y;
				float y = vector2.y;
				Vector3 max3 = m_ViewBounds.max;
				if (y < max3.y)
				{
					Vector3 max4 = m_ViewBounds.max;
					zero.y = max4.y - vector2.y;
				}
				else
				{
					float y2 = vector.y;
					Vector3 min3 = m_ViewBounds.min;
					if (y2 > min3.y)
					{
						Vector3 min4 = m_ViewBounds.min;
						zero.y = min4.y - vector.y;
					}
				}
			}
			return zero;
		}

		protected void SetDirty()
		{
			if (IsActive())
			{
				LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
			}
		}

		protected void SetDirtyCaching()
		{
			if (IsActive())
			{
				CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
				LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
			}
		}

		bool ICanvasElement.IsDestroyed()
		{
			return IsDestroyed();
		}

		Transform ICanvasElement.transform
		{
            get { return base.transform;  }
		}
	}
}
