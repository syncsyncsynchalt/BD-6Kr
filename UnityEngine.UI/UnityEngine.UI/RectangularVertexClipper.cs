namespace UnityEngine.UI;

internal class RectangularVertexClipper
{
	private readonly Vector3[] m_WorldCorners = new Vector3[4];

	private readonly Vector3[] m_CanvasCorners = new Vector3[4];

	public Rect GetCanvasRect(RectTransform t, Canvas c)
	{
		t.GetWorldCorners(m_WorldCorners);
		Transform component = c.GetComponent<Transform>();
		for (int i = 0; i < 4; i++)
		{
			ref Vector3 reference = ref m_CanvasCorners[i];
			reference = component.InverseTransformPoint(m_WorldCorners[i]);
		}
		return new Rect(m_CanvasCorners[0].x, m_CanvasCorners[0].y, m_CanvasCorners[2].x - m_CanvasCorners[0].x, m_CanvasCorners[2].y - m_CanvasCorners[0].y);
	}
}
