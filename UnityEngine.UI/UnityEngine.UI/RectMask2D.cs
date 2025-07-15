using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("UI/2D Rect Mask", 13)]
public class RectMask2D : UIBehaviour, ICanvasRaycastFilter, IClipper
{
	[NonSerialized]
	private readonly RectangularVertexClipper m_VertexClipper = new RectangularVertexClipper();

	[NonSerialized]
	private RectTransform m_RectTransform;

	[NonSerialized]
	private List<IClippable> m_ClipTargets = new List<IClippable>();

	[NonSerialized]
	private bool m_ShouldRecalculateClipRects;

	[NonSerialized]
	private List<RectMask2D> m_Clippers = new List<RectMask2D>();

	[NonSerialized]
	private Rect m_LastClipRectCanvasSpace;

	[NonSerialized]
	private bool m_LastClipRectValid;

	public Rect canvasRect
	{
		get
		{
			Canvas c = null;
			List<Canvas> list = ListPool<Canvas>.Get();
			base.gameObject.GetComponentsInParent(includeInactive: false, list);
			if (list.Count > 0)
			{
				c = list[0];
			}
			ListPool<Canvas>.Release(list);
			return m_VertexClipper.GetCanvasRect(rectTransform, c);
		}
	}

	public RectTransform rectTransform => m_RectTransform ?? (m_RectTransform = GetComponent<RectTransform>());

	protected RectMask2D()
	{
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		m_ShouldRecalculateClipRects = true;
		ClipperRegistry.Register(this);
		MaskUtilities.Notify2DMaskStateChanged(this);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		m_ClipTargets.Clear();
		m_Clippers.Clear();
		ClipperRegistry.Unregister(this);
		MaskUtilities.Notify2DMaskStateChanged(this);
	}

	public virtual bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		if (!base.isActiveAndEnabled)
		{
			return true;
		}
		return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, sp, eventCamera);
	}

	public virtual void PerformClipping()
	{
		if (m_ShouldRecalculateClipRects)
		{
			MaskUtilities.GetRectMasksForClip(this, m_Clippers);
			m_ShouldRecalculateClipRects = false;
		}
		bool validRect = true;
		Rect rect = Clipping.FindCullAndClipWorldRect(m_Clippers, out validRect);
		if (rect != m_LastClipRectCanvasSpace)
		{
			for (int i = 0; i < m_ClipTargets.Count; i++)
			{
				m_ClipTargets[i].SetClipRect(rect, validRect);
			}
			m_LastClipRectCanvasSpace = rect;
			m_LastClipRectValid = validRect;
		}
		for (int j = 0; j < m_ClipTargets.Count; j++)
		{
			m_ClipTargets[j].Cull(m_LastClipRectCanvasSpace, m_LastClipRectValid);
		}
	}

	public void AddClippable(IClippable clippable)
	{
		if (clippable != null)
		{
			if (!m_ClipTargets.Contains(clippable))
			{
				m_ClipTargets.Add(clippable);
			}
			clippable.SetClipRect(m_LastClipRectCanvasSpace, m_LastClipRectValid);
			clippable.Cull(m_LastClipRectCanvasSpace, m_LastClipRectValid);
		}
	}

	public void RemoveClippable(IClippable clippable)
	{
		if (clippable != null)
		{
			clippable.SetClipRect(default(Rect), validRect: false);
			m_ClipTargets.Remove(clippable);
		}
	}

	protected override void OnTransformParentChanged()
	{
		base.OnTransformParentChanged();
		m_ShouldRecalculateClipRects = true;
	}

	protected override void OnCanvasHierarchyChanged()
	{
		base.OnCanvasHierarchyChanged();
		m_ShouldRecalculateClipRects = true;
	}
}
