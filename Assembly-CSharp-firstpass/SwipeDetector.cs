using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
	private enum _SwipeState
	{
		None,
		Start,
		Swiping,
		End
	}

	private List<int> fingerIndex = new List<int>();

	private List<int> mouseIndex = new List<int>();

	public float maxSwipeDuration = 0.25f;

	public float minSpeed = 150f;

	public float minDistance = 15f;

	public float maxDirectionChange = 35f;

	public bool onlyFireWhenLiftCursor;

	public bool enableMultiSwipe = true;

	public float minDurationBetweenSwipe = 0.5f;

	private float lastSwipeTime = -10f;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.touchCount > 0)
		{
			if (enableMultiSwipe)
			{
				for (int i = 0; i < Input.touches.Length; i++)
				{
					Touch touch = Input.touches[i];
					if (fingerIndex.Count == 0 || !fingerIndex.Contains(touch.fingerId))
					{
						StartCoroutine(TouchSwipeRoutine(touch.fingerId));
					}
				}
			}
			else if (Input.touchCount == 1 && fingerIndex.Count != 1)
			{
				Touch touch2 = Input.touches[0];
				StartCoroutine(TouchSwipeRoutine(touch2.fingerId));
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
					StartCoroutine(MouseSwipeRoutine(0));
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
					StartCoroutine(MouseSwipeRoutine(1));
				}
			}
			else if (Input.GetMouseButtonUp(1) && mouseIndex.Contains(1))
			{
				mouseIndex.Remove(1);
			}
		}
	}

	private IEnumerator MouseSwipeRoutine(int index)
	{
		mouseIndex.Add(index);
		float timeStartSwipe = Time.realtimeSinceStartup;
		Vector2 initVector = Vector2.zero;
		Vector2 zero = Vector2.zero;
		_SwipeState swipeState = _SwipeState.None;
		Vector2 lastPos = Input.mousePosition;
		Vector2 startPos = lastPos;
		Vector2 initialPos = lastPos;
		int count = 0;
		yield return null;
		while (mouseIndex.Contains(index))
		{
			Vector2 curPos = Input.mousePosition;
			Vector2 curVector = curPos - lastPos;
			float mag = curVector.magnitude;
			count++;
			if (swipeState == _SwipeState.None && mag > 0f)
			{
				timeStartSwipe = Time.realtimeSinceStartup;
				startPos = initialPos;
				swipeState = _SwipeState.Swiping;
				initVector = curVector;
				count = 1;
				SwipeStart(startPos, curPos, timeStartSwipe, index, isMouse: true);
			}
			else if (swipeState == _SwipeState.Swiping)
			{
				if (count < 3)
				{
					initVector = (initVector + curVector) * 0.5f;
				}
				if (mag > 0f)
				{
					Swiping(startPos, curPos, timeStartSwipe, index, isMouse: true);
					if (curPos.x < 0f || curPos.x > (float)Screen.width || curPos.y < 0f || curPos.y > (float)Screen.height)
					{
						swipeState = _SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, isMouse: true);
						initialPos = curPos;
					}
					if (Time.realtimeSinceStartup - timeStartSwipe > maxSwipeDuration)
					{
						swipeState = _SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, isMouse: true);
						initialPos = curPos;
					}
					if (Mathf.Abs(Vector2.Angle(initVector, curVector)) > maxDirectionChange)
					{
						swipeState = _SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, isMouse: true);
						initialPos = curPos;
					}
					if (Mathf.Abs((curPos - startPos).magnitude / (Time.realtimeSinceStartup - timeStartSwipe)) < minSpeed)
					{
						swipeState = _SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, isMouse: true);
						initialPos = curPos;
					}
				}
			}
			lastPos = curPos;
			yield return null;
		}
		if (swipeState == _SwipeState.Swiping)
		{
			SwipeEnd(startPos, lastPos, timeStartSwipe, index, isMouse: true);
		}
	}

	private IEnumerator TouchSwipeRoutine(int index)
	{
		fingerIndex.Add(index);
		float timeStartSwipe = Time.realtimeSinceStartup;
		Vector2 initVector = Vector2.zero;
		Vector2 zero = Vector2.zero;
		_SwipeState swipeState = _SwipeState.None;
		Vector2 lastPos = IT_Gesture.GetTouch(index).position;
		Vector2 startPos = lastPos;
		Vector2 initialPos = lastPos;
		int count = 0;
		yield return null;
		while (Input.touchCount > 0 && (enableMultiSwipe || Input.touchCount <= 1))
		{
			Touch touch = IT_Gesture.GetTouch(index);
			if (touch.position == Vector2.zero)
			{
				break;
			}
			Vector2 curPos = touch.position;
			Vector2 curVector = curPos - lastPos;
			float mag = curVector.magnitude;
			count++;
			if (swipeState == _SwipeState.None && mag > 0f)
			{
				timeStartSwipe = Time.realtimeSinceStartup;
				startPos = initialPos;
				swipeState = _SwipeState.Swiping;
				initVector = curVector;
				count = 1;
				SwipeStart(startPos, curPos, timeStartSwipe, index, isMouse: false);
			}
			else if (swipeState == _SwipeState.Swiping)
			{
				if (count < 3)
				{
					initVector = (initVector + curVector) * 0.5f;
				}
				if (mag > 0f)
				{
					Swiping(startPos, curPos, timeStartSwipe, index, isMouse: false);
					if (curPos.x < 0f || curPos.x > (float)Screen.width || curPos.y < 0f || curPos.y > (float)Screen.height)
					{
						swipeState = _SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index, isMouse: true);
						initialPos = curPos;
					}
					if (Time.realtimeSinceStartup - timeStartSwipe > maxSwipeDuration)
					{
						swipeState = _SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index);
						initialPos = curPos;
					}
					if (Mathf.Abs(Vector2.Angle(initVector, curVector)) > maxDirectionChange)
					{
						swipeState = _SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index);
						initialPos = curPos;
					}
					if (Mathf.Abs((curPos - startPos).magnitude / (Time.realtimeSinceStartup - timeStartSwipe)) < minSpeed)
					{
						swipeState = _SwipeState.None;
						SwipeEnd(startPos, curPos, timeStartSwipe, index);
						initialPos = curPos;
					}
				}
			}
			lastPos = curPos;
			yield return null;
		}
		if (swipeState == _SwipeState.Swiping)
		{
			SwipeEnd(startPos, lastPos, timeStartSwipe, index);
		}
		fingerIndex.Remove(index);
	}

	private void SwipeStart(Vector2 startPos, Vector2 endPos, float timeStartSwipe, int index, bool isMouse)
	{
		Vector2 dir = endPos - startPos;
		SwipeInfo sw = new SwipeInfo(startPos, endPos, dir, timeStartSwipe, index, isMouse);
		IT_Gesture.SwipeStart(sw);
	}

	private void Swiping(Vector2 startPos, Vector2 endPos, float timeStartSwipe, int index, bool isMouse)
	{
		Vector2 dir = endPos - startPos;
		SwipeInfo sw = new SwipeInfo(startPos, endPos, dir, timeStartSwipe, index, isMouse);
		IT_Gesture.Swiping(sw);
	}

	private void SwipeEnd(Vector2 startPos, Vector2 endPos, float timeStartSwipe, int index)
	{
		SwipeEnd(startPos, endPos, timeStartSwipe, index, isMouse: false);
	}

	private void SwipeEnd(Vector2 startPos, Vector2 endPos, float timeStartSwipe, int index, bool isMouse)
	{
		if (onlyFireWhenLiftCursor)
		{
			if (!isMouse)
			{
				for (int i = 0; i < Input.touchCount; i++)
				{
					Touch touch = Input.touches[i];
					if (touch.fingerId == index)
					{
						return;
					}
				}
			}
			else if (mouseIndex.Contains(index) || Time.realtimeSinceStartup - timeStartSwipe > maxSwipeDuration)
			{
				return;
			}
		}
		Vector2 dir = endPos - startPos;
		SwipeInfo sw = new SwipeInfo(startPos, endPos, dir, timeStartSwipe, index, isMouse);
		IT_Gesture.SwipeEnd(sw);
		if (!(dir.magnitude < minDistance * IT_Gesture.GetDPIFactor()) && !(Time.time - lastSwipeTime < minDurationBetweenSwipe))
		{
			lastSwipeTime = Time.time;
			IT_Gesture.Swipe(sw);
		}
	}
}
