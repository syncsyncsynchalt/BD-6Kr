using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDetector : MonoBehaviour
{
	private float flickTimeRange = 0.2f;

	private List<int> fingerIndex = new List<int>();

	private List<int> mouseIndex = new List<int>();

	public int minDragDistance = 15;

	public bool enableMultiDrag;

	public bool fireOnDraggingWhenNotMoving;

	private int multiDragCount;

	private void Start()
	{
	}

	private IEnumerator MultiDragRoutine(int count)
	{
		if (count <= 1)
		{
			yield break;
		}
		bool dragStarted = false;
		Vector2 startPos2 = Vector2.zero;
		for (int k = 0; k < Input.touchCount; k++)
		{
			startPos2 += Input.touches[k].position;
		}
		startPos2 /= Input.touchCount;
		Vector2 lastPos = startPos2;
		float timeStart = float.PositiveInfinity;
		while (Input.touchCount == count)
		{
			Vector2 curPos2 = Vector2.zero;
			Vector2[] allPos = new Vector2[count];
			bool moving = true;
			for (int j = 0; j < count; j++)
			{
				Touch touch = Input.touches[j];
				curPos2 += touch.position;
				allPos[j] = touch.position;
				if (touch.phase != TouchPhase.Moved)
				{
					moving = false;
				}
			}
			curPos2 /= count;
			bool sync = true;
			if (moving)
			{
				for (int i = 0; i < count - 1; i++)
				{
					Vector2 v3 = Input.touches[i].deltaPosition.normalized;
					Vector2 v2 = Input.touches[i + 1].deltaPosition.normalized;
					if (Vector2.Dot(v3, v2) < 0.85f)
					{
						sync = false;
					}
				}
			}
			if (moving && sync)
			{
				if (!dragStarted)
				{
					if (Vector2.Distance(curPos2, startPos2) > (float)minDragDistance * IT_Gesture.GetDPIFactor())
					{
						dragStarted = true;
						Vector2 delta2 = curPos2 - startPos2;
						DragInfo dragInfo3 = new DragInfo(curPos2, delta2, count);
						IT_Gesture.DraggingStart(dragInfo3);
						timeStart = Time.realtimeSinceStartup;
					}
				}
				else if (curPos2 != lastPos)
				{
					Vector2 delta = curPos2 - lastPos;
					DragInfo dragInfo2 = new DragInfo(curPos2, delta, count);
					IT_Gesture.Dragging(dragInfo2);
				}
			}
			else if (dragStarted && fireOnDraggingWhenNotMoving)
			{
				DragInfo dragInfo = new DragInfo(curPos2, Vector2.zero, count);
				IT_Gesture.Dragging(dragInfo);
			}
			lastPos = curPos2;
			yield return null;
		}
		if (dragStarted)
		{
			bool isFlick = false;
			if (Time.realtimeSinceStartup - timeStart < flickTimeRange)
			{
				isFlick = true;
			}
			Vector2 delta3 = lastPos - startPos2;
			DragInfo dragInfo4 = new DragInfo(lastPos, delta3, count, isFlick);
			IT_Gesture.DraggingEnd(dragInfo4);
		}
	}

	private IEnumerator MouseRoutine(int index)
	{
		mouseIndex.Add(index);
		bool dragStarted = false;
		Vector2 startPos = Input.mousePosition;
		Vector2 lastPos = startPos;
		float timeStart = float.PositiveInfinity;
		while (mouseIndex.Contains(index))
		{
			Vector2 curPos = Input.mousePosition;
			if (!dragStarted)
			{
				if (Vector3.Distance(curPos, startPos) > (float)minDragDistance * IT_Gesture.GetDPIFactor())
				{
					dragStarted = true;
					Vector2 delta2 = curPos - startPos;
					DragInfo dragInfo3 = new DragInfo(curPos, delta2, 1, index, im: true);
					IT_Gesture.DraggingStart(dragInfo3);
					timeStart = Time.realtimeSinceStartup;
				}
			}
			else if (curPos != lastPos)
			{
				Vector2 delta = curPos - lastPos;
				DragInfo dragInfo2 = new DragInfo(curPos, delta, 1, index, im: true);
				IT_Gesture.Dragging(dragInfo2);
			}
			else if (fireOnDraggingWhenNotMoving)
			{
				DragInfo dragInfo = new DragInfo(curPos, Vector2.zero, 1, index, im: true);
				IT_Gesture.Dragging(dragInfo);
			}
			lastPos = curPos;
			yield return null;
		}
		if (dragStarted)
		{
			bool isFlick = false;
			if (Time.realtimeSinceStartup - timeStart < flickTimeRange)
			{
				isFlick = true;
			}
			Vector2 delta3 = lastPos - startPos;
			DragInfo dragInfo4 = new DragInfo(lastPos, delta3, 1, index, isFlick, im: true);
			IT_Gesture.DraggingEnd(dragInfo4);
		}
	}

	private IEnumerator TouchRoutine(int index)
	{
		fingerIndex.Add(index);
		bool dragStarted = false;
		Vector2 startPos = IT_Gesture.GetTouch(index).position;
		Vector2 lastPos = startPos;
		float timeStart = float.PositiveInfinity;
		while ((enableMultiDrag && Input.touchCount > 0) || (!enableMultiDrag && Input.touchCount == 1))
		{
			Touch touch = IT_Gesture.GetTouch(index);
			if (touch.position == Vector2.zero)
			{
				break;
			}
			Vector2 curPos = touch.position;
			if (touch.phase == TouchPhase.Moved)
			{
				if (!dragStarted)
				{
					if (Vector3.Distance(curPos, startPos) > (float)minDragDistance * IT_Gesture.GetDPIFactor())
					{
						dragStarted = true;
						Vector2 delta3 = curPos - startPos;
						DragInfo dragInfo4 = new DragInfo(curPos, delta3, 1, index, im: false);
						IT_Gesture.DraggingStart(dragInfo4);
						timeStart = Time.realtimeSinceStartup;
					}
				}
				else if (curPos != lastPos)
				{
					Vector2 delta2 = curPos - lastPos;
					DragInfo dragInfo3 = new DragInfo(curPos, delta2, 1, index, im: false);
					IT_Gesture.Dragging(dragInfo3);
				}
				lastPos = curPos;
			}
			else if (dragStarted && fireOnDraggingWhenNotMoving)
			{
				DragInfo dragInfo2 = new DragInfo(curPos, Vector2.zero, 1, index, im: false);
				IT_Gesture.Dragging(dragInfo2);
			}
			yield return null;
		}
		if (dragStarted)
		{
			bool isFlick = false;
			if (Time.realtimeSinceStartup - timeStart < flickTimeRange)
			{
				isFlick = true;
			}
			Vector2 delta = lastPos - startPos;
			DragInfo dragInfo = new DragInfo(lastPos, delta, 1, index, isFlick, im: false);
			IT_Gesture.DraggingEnd(dragInfo);
		}
		fingerIndex.Remove(index);
	}

	private void Update()
	{
		if (Input.touchCount <= 1)
		{
			multiDragCount = 1;
		}
		if (Input.touchCount > 0)
		{
			if (enableMultiDrag)
			{
				for (int i = 0; i < Input.touches.Length; i++)
				{
					Touch touch = Input.touches[i];
					if (fingerIndex.Count == 0 || !fingerIndex.Contains(touch.fingerId))
					{
						StartCoroutine(TouchRoutine(touch.fingerId));
					}
				}
			}
			else if (Input.touchCount == 1 && fingerIndex.Count == 0)
			{
				StartCoroutine(TouchRoutine(Input.touches[0].fingerId));
			}
			if (Input.touchCount > 1 && Input.touchCount != multiDragCount)
			{
				multiDragCount = Input.touchCount;
				StartCoroutine(MultiDragRoutine(Input.touchCount));
			}
		}
		else
		{
			if (Input.touchCount != 0)
			{
				return;
			}
			if (Input.GetMouseButtonDown(0))
			{
				if (!mouseIndex.Contains(0))
				{
					StartCoroutine(MouseRoutine(0));
				}
			}
			else if (Input.GetMouseButtonUp(0) && mouseIndex.Contains(0))
			{
				mouseIndex.Remove(0);
			}
			if (Input.GetMouseButtonDown(1))
			{
				if (!mouseIndex.Contains(1))
				{
					StartCoroutine(MouseRoutine(1));
				}
			}
			else if (Input.GetMouseButtonUp(1) && mouseIndex.Contains(1))
			{
				mouseIndex.Remove(1);
			}
			if (Input.GetMouseButtonDown(2))
			{
				if (!mouseIndex.Contains(2))
				{
					StartCoroutine(MouseRoutine(2));
				}
			}
			else if (Input.GetMouseButtonUp(2) && mouseIndex.Contains(2))
			{
				mouseIndex.Remove(2);
			}
			if (Input.GetMouseButtonDown(3))
			{
				if (!mouseIndex.Contains(3))
				{
					StartCoroutine(MouseRoutine(3));
				}
			}
			else if (Input.GetMouseButtonUp(3) && mouseIndex.Contains(3))
			{
				mouseIndex.Remove(3);
			}
		}
	}
}
