using System.Collections.Generic;
using UnityEngine;

public class DualFingerDetector : MonoBehaviour
{
	public enum _SmoothMethod
	{
		None,
		Average,
		WeightedAverage
	}

	private Vector2 initPos1 = Vector3.zero;

	private Vector2 initPos2 = Vector3.zero;

	private Vector2 initGradient;

	private Vector2 lastPos1;

	private Vector2 lastPos2;

	private bool firstTouch = true;

	private int currentStart;

	private List<float> rotVals = new List<float>();

	private float curAngle;

	private float prevAngle;

	private Vector2 lastTouchPos1;

	private Vector2 lastTouchPos2;

	public int rotationSmoothFrameCount = 5;

	public _SmoothMethod rotationSmoothing;

	private float[] weights = new float[0];

	private float weightTotal;

	private void Update()
	{
		if (Input.touchCount == 2)
		{
			Touch touch = Input.touches[0];
			Touch touch2 = Input.touches[1];
			Vector2 position = touch.position;
			Vector2 position2 = touch2.position;
			Vector2 vector = position - lastTouchPos1;
			Vector2 vector2 = position2 - lastTouchPos2;
			if (firstTouch)
			{
				firstTouch = false;
				initPos1 = position;
				initPos2 = position2;
				initGradient = (position - position2).normalized;
				float x = position.x - position2.x;
				float y = position.y - position2.y;
				prevAngle = IT_Gesture.VectorToAngle(new Vector2(x, y));
			}
			if (touch.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
			{
				float num = Vector2.Dot(vector, vector2);
				if (num < 0.5f)
				{
					Vector2 normalized = (position - initPos1).normalized;
					Vector2 normalized2 = (position2 - initPos2).normalized;
					float num2 = Vector2.Dot(normalized, initGradient);
					float num3 = Vector2.Dot(normalized2, initGradient);
					if (num2 < 0.7f && num3 < 0.7f)
					{
						float x2 = position.x - position2.x;
						float y2 = position.y - position2.y;
						float current = IT_Gesture.VectorToAngle(new Vector2(x2, y2));
						float num4 = Mathf.DeltaAngle(current, prevAngle);
						if (rotationSmoothing == _SmoothMethod.None)
						{
							RotateInfo rI = new RotateInfo(num4, position, position2);
							IT_Gesture.Rotate(rI);
						}
						else
						{
							if (Mathf.Abs(num4) > 0f)
							{
								AddRotVal(num4);
							}
							float averageValue = GetAverageValue();
							if (averageValue != 0f)
							{
								RotateInfo rI2 = new RotateInfo(averageValue, position, position2);
								IT_Gesture.Rotate(rI2);
							}
						}
						prevAngle = current;
					}
					else
					{
						Vector2 vector3 = position - position2;
						float num5 = (position - vector - (position2 - vector2)).magnitude - vector3.magnitude;
						if (Mathf.Abs(num5) > 0.5f)
						{
							PinchInfo pI = new PinchInfo(num5, position, position2);
							IT_Gesture.Pinch(pI);
						}
					}
				}
			}
			lastTouchPos1 = position;
			lastTouchPos2 = position2;
		}
		else if (!firstTouch)
		{
			firstTouch = true;
			if (rotationSmoothing != 0)
			{
				ClearRotVal();
			}
		}
	}

	private void AddRotVal(float val)
	{
		if (rotVals.Count < rotationSmoothFrameCount)
		{
			rotVals.Add(val);
			currentStart = rotVals.Count - 1;
			return;
		}
		rotVals[currentStart] = val;
		currentStart++;
		if (currentStart >= rotVals.Count)
		{
			currentStart = 0;
		}
	}

	private void ClearRotVal()
	{
		rotVals = new List<float>();
	}

	private float GetAverageValue()
	{
		if (rotVals.Count == 0)
		{
			return 0f;
		}
		if (rotationSmoothing == _SmoothMethod.Average)
		{
			float num = 0f;
			for (int i = 0; i < rotVals.Count; i++)
			{
				num += rotVals[i];
			}
			return num / (float)rotVals.Count;
		}
		if (rotationSmoothing == _SmoothMethod.WeightedAverage)
		{
			if (weights.Length != rotationSmoothFrameCount)
			{
				InitWeigths();
			}
			float num2 = 0f;
			int num3 = 0;
			for (int j = currentStart; j < rotVals.Count; j++)
			{
				num2 += rotVals[j] * weights[num3];
				num3++;
			}
			for (int k = 0; k < currentStart; k++)
			{
				num2 += rotVals[k] * weights[num3];
				num3++;
			}
			return num2 / weightTotal;
		}
		return 0f;
	}

	private void InitWeigths()
	{
		weights = new float[rotationSmoothFrameCount];
		weightTotal = 0f;
		for (int i = 0; i < weights.Length; i++)
		{
			weights[i] = i + 1;
			weightTotal += i + 1;
		}
	}
}
