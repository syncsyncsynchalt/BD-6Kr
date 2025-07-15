namespace UnityEngine.UI;

public interface IClippable
{
	RectTransform rectTransform { get; }

	void RecalculateClipping();

	void Cull(Rect clipRect, bool validRect);

	void SetClipRect(Rect value, bool validRect);
}
