using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapDetector : MonoBehaviour
{
	private enum _DTapState
	{
		Clear,
		Tap1,
		Complete
	}

	private enum _ChargeState
	{
		Clear,
		Charging,
		Charged
	}

	private List<int> fingerIndex = new List<int>();

	private List<int> mouseIndex = new List<int>();

	private MultiTapTracker[] multiTapMFTouch = new MultiTapTracker[5];

	private MultiTapTracker[] multiTapTouch = new MultiTapTracker[5];

	private MultiTapTracker[] multiTapMouse = new MultiTapTracker[2];

	private float tapStartTime;

	private _DTapState dTapState;

	private Vector2 lastPos;

	private Vector2 startPos;

	private bool posShifted;

	private float lastShortTapTime;

	private Vector2 lastShortTapPos;

	public bool enableMultiTapFilter;

	public int maxTapDisplacementAllowance = 5;

	public float shortTapTime = 0.2f;

	public float longTapTime = 0.8f;

	public float multiTapInterval = 0.35f;

	public float multiTapPosSpacing = 20f;

	public int maxMultiTapCount = 2;

	public _ChargeMode chargeMode;

	public float minChargeTime = 0.15f;

	public float maxChargeTime = 2f;

	public static float tapPosDeviation = 10f;

	private bool firstTouch = true;

	private List<int> indexes = new List<int>();

	private List<FingerGroup> fingerGroup = new List<FingerGroup>();

	public float maxFingerGroupDist = 200f;

	public void SetChargeMode(_ChargeMode mode)
	{
		chargeMode = mode;
	}

	private void Start()
	{
		if (enableMultiTapFilter)
		{
			IT_Gesture.SetMultiTapFilter(enableMultiTapFilter);
			IT_Gesture.SetMaxMultiTapCount(maxMultiTapCount);
			IT_Gesture.SetMaxMultiTapInterval(multiTapInterval);
		}
		for (int i = 0; i < multiTapMouse.Length; i++)
		{
			multiTapMouse[i] = new MultiTapTracker(i);
		}
		for (int j = 0; j < multiTapTouch.Length; j++)
		{
			multiTapTouch[j] = new MultiTapTracker(j);
		}
		for (int k = 0; k < multiTapMFTouch.Length; k++)
		{
			multiTapMFTouch[k] = new MultiTapTracker(k);
		}
		StartCoroutine(CheckMultiTapCount());
		StartCoroutine(MultiFingerRoutine());
	}

	private void CheckMultiTapMouse(int index, Vector2 startPos, Vector2 lastPos)
	{
		if (multiTapMouse[index].lastTapTime > Time.realtimeSinceStartup - multiTapInterval)
		{
			if (Vector2.Distance(startPos, multiTapMouse[index].lastPos) < multiTapPosSpacing * IT_Gesture.GetDPIFactor())
			{
				multiTapMouse[index].count++;
				multiTapMouse[index].lastPos = startPos;
				multiTapMouse[index].lastTapTime = Time.realtimeSinceStartup;
				IT_Gesture.MultiTap(new Tap(startPos, lastPos, multiTapMouse[index].count, index, im: true));
				if (multiTapMouse[index].count >= maxMultiTapCount)
				{
					multiTapMouse[index].count = 0;
				}
			}
			else
			{
				multiTapMouse[index].count = 1;
				multiTapMouse[index].lastPos = startPos;
				multiTapMouse[index].lastTapTime = Time.realtimeSinceStartup;
				IT_Gesture.MultiTap(new Tap(startPos, lastPos, 1, index, im: true));
			}
		}
		else
		{
			multiTapMouse[index].count = 1;
			multiTapMouse[index].lastPos = startPos;
			multiTapMouse[index].lastTapTime = Time.realtimeSinceStartup;
			IT_Gesture.MultiTap(new Tap(startPos, lastPos, 1, index, im: true));
		}
	}

	private void CheckMultiTapTouch(int index, Vector2 startPos, Vector2 lastPos)
	{
		if (index >= multiTapTouch.Length)
		{
			return;
		}
		if (multiTapTouch[index].lastTapTime > Time.realtimeSinceStartup - multiTapInterval)
		{
			if (Vector2.Distance(startPos, multiTapTouch[index].lastPos) < multiTapPosSpacing * IT_Gesture.GetDPIFactor())
			{
				multiTapTouch[index].count++;
				multiTapTouch[index].lastPos = startPos;
				multiTapTouch[index].lastTapTime = Time.realtimeSinceStartup;
				IT_Gesture.MultiTap(new Tap(startPos, lastPos, multiTapTouch[index].count, index, im: false));
				if (multiTapTouch[index].count >= maxMultiTapCount)
				{
					multiTapTouch[index].count = 0;
				}
			}
			else
			{
				multiTapTouch[index].count = 1;
				multiTapTouch[index].lastPos = startPos;
				multiTapTouch[index].lastTapTime = Time.realtimeSinceStartup;
				IT_Gesture.MultiTap(new Tap(startPos, lastPos, 1, index, im: false));
			}
		}
		else
		{
			multiTapTouch[index].count = 1;
			multiTapTouch[index].lastPos = startPos;
			multiTapTouch[index].lastTapTime = Time.realtimeSinceStartup;
			IT_Gesture.MultiTap(new Tap(startPos, lastPos, 1, index, im: false));
		}
	}

	public void CheckMultiTapMFTouch(int fCount, Vector2[] posL, int[] indexes)
	{
		Vector2 a = Vector2.zero;
		for (int i = 0; i < posL.Length; i++)
		{
			a += posL[i];
		}
		a /= posL.Length;
		int num = 0;
		bool flag = false;
		for (int j = 0; j < multiTapMFTouch.Length; j++)
		{
			MultiTapTracker multiTapTracker = multiTapMFTouch[j];
			if (multiTapTracker.fingerCount == fCount && Vector2.Distance(a, multiTapTracker.lastPos) < multiTapPosSpacing * IT_Gesture.GetDPIFactor())
			{
				flag = true;
				break;
			}
			num++;
		}
		if (!flag)
		{
			num = 0;
			for (int k = 0; k < multiTapMFTouch.Length; k++)
			{
				MultiTapTracker multiTapTracker2 = multiTapMFTouch[k];
				if (multiTapTracker2.lastPos == Vector2.zero && multiTapTracker2.count == 0)
				{
					break;
				}
				num++;
			}
		}
		if (multiTapMFTouch[num].lastTapTime > Time.realtimeSinceStartup - multiTapInterval)
		{
			multiTapMFTouch[num].count++;
			multiTapMFTouch[num].lastPos = a;
			multiTapMFTouch[num].fingerCount = fCount;
			multiTapMFTouch[num].lastTapTime = Time.realtimeSinceStartup;
			IT_Gesture.MultiTap(new Tap(multiTapMFTouch[num].count, fCount, posL, indexes));
			if (multiTapMFTouch[num].count >= maxMultiTapCount)
			{
				multiTapMFTouch[num].count = 0;
			}
		}
		else
		{
			multiTapMFTouch[num].count = 1;
			multiTapMFTouch[num].lastPos = a;
			multiTapMFTouch[num].fingerCount = fCount;
			multiTapMFTouch[num].lastTapTime = Time.realtimeSinceStartup;
			IT_Gesture.MultiTap(new Tap(multiTapMFTouch[num].count, fCount, posL, indexes));
		}
	}

	private IEnumerator CheckMultiTapCount()
	{
		while (true)
		{
			for (int k = 0; k < multiTapMouse.Length; k++)
			{
				MultiTapTracker multiTap = multiTapMouse[k];
				if (multiTap.count > 0 && multiTap.lastTapTime + multiTapInterval < Time.realtimeSinceStartup)
				{
					multiTap.count = 0;
					multiTap.lastPos = Vector2.zero;
				}
			}
			for (int j = 0; j < multiTapTouch.Length; j++)
			{
				MultiTapTracker multiTap2 = multiTapTouch[j];
				if (multiTap2.count > 0 && multiTap2.lastTapTime + multiTapInterval < Time.realtimeSinceStartup)
				{
					multiTap2.count = 0;
					multiTap2.lastPos = Vector2.zero;
				}
			}
			for (int i = 0; i < multiTapMFTouch.Length; i++)
			{
				MultiTapTracker multiTap3 = multiTapMFTouch[i];
				if (multiTap3.count > 0 && multiTap3.lastTapTime + multiTapInterval < Time.realtimeSinceStartup)
				{
					multiTap3.count = 0;
					multiTap3.lastPos = Vector2.zero;
					multiTap3.fingerCount = 1;
				}
			}
			yield return null;
		}
	}

	private IEnumerator FingerRoutine(int index)
	{
		fingerIndex.Add(index);
		Touch touch2 = IT_Gesture.GetTouch(index);
		float startTime = Time.realtimeSinceStartup;
		Vector2 startPos = touch2.position;
		Vector2 lastPos = startPos;
		bool longTap = false;
		_ChargeState chargeState = _ChargeState.Clear;
		int chargeDir = 1;
		int chargeConst = 0;
		float startTimeCharge = Time.realtimeSinceStartup;
		Vector2 startPosCharge = touch2.position;
		while (true)
		{
			touch2 = IT_Gesture.GetTouch(index);
			if (touch2.position == Vector2.zero)
			{
				break;
			}
			Vector2 curPos = touch2.position;
			if (Time.realtimeSinceStartup - startTimeCharge > minChargeTime && chargeState == _ChargeState.Clear)
			{
				chargeState = _ChargeState.Charging;
				float chargedValue3 = Mathf.Clamp((float)chargeConst + (float)chargeDir * ((Time.realtimeSinceStartup - startTimeCharge) / maxChargeTime), 0f, 1f);
				ChargedInfo cInfo3 = new ChargedInfo(curPos, chargedValue3, index, im: false);
				IT_Gesture.ChargeStart(cInfo3);
				startPosCharge = curPos;
			}
			else if (chargeState == _ChargeState.Charging)
			{
				if (Vector3.Distance(curPos, startPosCharge) > tapPosDeviation)
				{
					chargeState = _ChargeState.Clear;
					float chargedValue2 = Mathf.Clamp((float)chargeConst + (float)chargeDir * ((Time.realtimeSinceStartup - startTimeCharge) / maxChargeTime), 0f, 1f);
					ChargedInfo cInfo2 = new ChargedInfo(lastPos, chargedValue2, index, im: false);
					IT_Gesture.ChargeEnd(cInfo2);
				}
				else
				{
					float chargedValue = Mathf.Clamp((float)chargeConst + (float)chargeDir * ((Time.realtimeSinceStartup - startTimeCharge) / maxChargeTime), 0f, 1f);
					ChargedInfo cInfo = new ChargedInfo(curPos, chargedValue, index, im: false);
					if (chargeMode == _ChargeMode.PingPong)
					{
						if (chargedValue == 1f || chargedValue == 0f)
						{
							chargeDir *= -1;
							switch (chargeDir)
							{
							case 1:
								chargeConst = 0;
								break;
							case -1:
								chargeConst = 1;
								break;
							}
							startTimeCharge = Time.realtimeSinceStartup;
						}
						IT_Gesture.Charging(cInfo);
					}
					else if (chargedValue < 1f)
					{
						IT_Gesture.Charging(cInfo);
					}
					else
					{
						cInfo.percent = 1f;
						if (chargeMode == _ChargeMode.Once)
						{
							chargeState = _ChargeState.Charged;
							IT_Gesture.ChargeEnd(cInfo);
							startTimeCharge = float.PositiveInfinity;
						}
						else if (chargeMode == _ChargeMode.Clamp)
						{
							chargeState = _ChargeState.Charged;
							IT_Gesture.Charging(cInfo);
						}
						else if (chargeMode == _ChargeMode.Loop)
						{
							chargeState = _ChargeState.Clear;
							IT_Gesture.ChargeEnd(cInfo);
							startTimeCharge = Time.realtimeSinceStartup;
						}
					}
				}
			}
			if (!longTap && Time.realtimeSinceStartup - startTime > longTapTime && Vector2.Distance(lastPos, startPos) < (float)maxTapDisplacementAllowance * IT_Gesture.GetDPIFactor())
			{
				IT_Gesture.LongTap(new Tap(curPos, 1, index, im: false));
				longTap = true;
			}
			lastPos = curPos;
			yield return null;
		}
		if (Time.realtimeSinceStartup - startTime <= shortTapTime && Vector2.Distance(lastPos, startPos) < (float)maxTapDisplacementAllowance * IT_Gesture.GetDPIFactor())
		{
			CheckMultiTapTouch(index, startPos, lastPos);
		}
		if (chargeState == _ChargeState.Charging || (chargeState == _ChargeState.Charged && chargeMode != 0))
		{
			float chargedValue4 = Mathf.Clamp((float)chargeConst + (float)chargeDir * ((Time.realtimeSinceStartup - startTimeCharge) / maxChargeTime), 0f, 1f);
			ChargedInfo cInfo4 = new ChargedInfo(lastPos, chargedValue4, index, im: false);
			IT_Gesture.ChargeEnd(cInfo4);
		}
		fingerIndex.Remove(index);
	}

	private IEnumerator MouseRoutine(int index)
	{
		mouseIndex.Add(index);
		float startTime = Time.realtimeSinceStartup;
		Vector2 startPos = Input.mousePosition;
		Vector2 lastPos = startPos;
		bool longTap = false;
		_ChargeState chargeState = _ChargeState.Clear;
		int chargeDir = 1;
		float chargeConst = 0f;
		float startTimeCharge = Time.realtimeSinceStartup;
		Vector2 startPosCharge = Input.mousePosition;
		yield return null;
		while (mouseIndex.Contains(index))
		{
			Vector2 curPos = Input.mousePosition;
			if (Time.realtimeSinceStartup - startTimeCharge > minChargeTime && chargeState == _ChargeState.Clear)
			{
				chargeState = _ChargeState.Charging;
				float chargedValue3 = Mathf.Clamp(chargeConst + (float)chargeDir * ((Time.realtimeSinceStartup - startTimeCharge) / maxChargeTime), 0f, 1f);
				ChargedInfo cInfo3 = new ChargedInfo(curPos, chargedValue3, index, im: true);
				IT_Gesture.ChargeStart(cInfo3);
				startPosCharge = curPos;
			}
			else if (chargeState == _ChargeState.Charging)
			{
				if (Vector3.Distance(curPos, startPosCharge) > tapPosDeviation)
				{
					chargeState = _ChargeState.Clear;
					float chargedValue2 = Mathf.Clamp(chargeConst + (float)chargeDir * ((Time.realtimeSinceStartup - startTimeCharge) / maxChargeTime), 0f, 1f);
					ChargedInfo cInfo2 = new ChargedInfo(lastPos, chargedValue2, index, im: true);
					IT_Gesture.ChargeEnd(cInfo2);
				}
				else
				{
					float chargedValue = Mathf.Clamp(chargeConst + (float)chargeDir * ((Time.realtimeSinceStartup - startTimeCharge) / maxChargeTime), 0f, 1f);
					ChargedInfo cInfo = new ChargedInfo(curPos, chargedValue, index, im: true);
					if (chargeMode == _ChargeMode.PingPong)
					{
						if (chargedValue == 1f || chargedValue == 0f)
						{
							chargeDir *= -1;
							switch (chargeDir)
							{
							case 1:
								chargeConst = 0f;
								break;
							case -1:
								chargeConst = 1f;
								break;
							}
							startTimeCharge = Time.realtimeSinceStartup;
						}
						IT_Gesture.Charging(cInfo);
					}
					else if (chargedValue < 1f)
					{
						IT_Gesture.Charging(cInfo);
					}
					else
					{
						cInfo.percent = 1f;
						if (chargeMode == _ChargeMode.Once)
						{
							chargeState = _ChargeState.Charged;
							IT_Gesture.ChargeEnd(cInfo);
							startTimeCharge = float.PositiveInfinity;
						}
						else if (chargeMode == _ChargeMode.Clamp)
						{
							chargeState = _ChargeState.Charged;
							IT_Gesture.Charging(cInfo);
						}
						else if (chargeMode == _ChargeMode.Loop)
						{
							chargeState = _ChargeState.Clear;
							IT_Gesture.ChargeEnd(cInfo);
							startTimeCharge = Time.realtimeSinceStartup;
						}
					}
				}
			}
			if (!longTap && Time.realtimeSinceStartup - startTime > longTapTime && Vector2.Distance(lastPos, startPos) < (float)maxTapDisplacementAllowance * IT_Gesture.GetDPIFactor())
			{
				IT_Gesture.LongTap(new Tap(curPos, 1, index, im: true));
				longTap = true;
			}
			lastPos = curPos;
			yield return null;
		}
		if (Time.realtimeSinceStartup - startTime <= shortTapTime && Vector2.Distance(lastPos, startPos) < (float)maxTapDisplacementAllowance * IT_Gesture.GetDPIFactor())
		{
			CheckMultiTapMouse(index, startPos, lastPos);
		}
		switch (chargeState)
		{
		default:
			yield break;
		case _ChargeState.Charged:
			if (chargeMode == _ChargeMode.Once)
			{
				yield break;
			}
			break;
		case _ChargeState.Charging:
			break;
		}
		float chargedValue4 = Mathf.Clamp(chargeConst + (float)chargeDir * ((Time.realtimeSinceStartup - startTimeCharge) / maxChargeTime), 0f, 1f);
		ChargedInfo cInfo4 = new ChargedInfo(lastPos, chargedValue4, index, im: true);
		IT_Gesture.ChargeEnd(cInfo4);
	}

	private void Update()
	{
		if (Input.touchCount > 0)
		{
			if (indexes.Count < Input.touchCount)
			{
				for (int i = 0; i < Input.touches.Length; i++)
				{
					Touch touch = Input.touches[i];
					if (!fingerIndex.Contains(touch.fingerId))
					{
						CheckFingerGroup(touch);
					}
				}
			}
			for (int j = 0; j < Input.touches.Length; j++)
			{
				Touch touch2 = Input.touches[j];
				if (fingerIndex.Count == 0 || !fingerIndex.Contains(touch2.fingerId))
				{
					StartCoroutine(FingerRoutine(touch2.fingerId));
				}
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
		}
	}

	private IEnumerator MultiFingerRoutine()
	{
		while (true)
		{
			if (fingerGroup.Count > 0)
			{
				for (int i = 0; i < fingerGroup.Count; i++)
				{
					if (fingerGroup[i].routineEnded)
					{
						fingerGroup.RemoveAt(i);
						i--;
					}
				}
			}
			yield return null;
		}
	}

	private void CheckFingerGroup(Touch touch)
	{
		bool flag = false;
		for (int i = 0; i < this.fingerGroup.Count; i++)
		{
			FingerGroup fingerGroup = this.fingerGroup[i];
			if (!(Time.realtimeSinceStartup - fingerGroup.triggerTime < shortTapTime / 2f))
			{
				continue;
			}
			bool flag2 = true;
			for (int j = 0; j < fingerGroup.indexes.Count; j++)
			{
				int iD = fingerGroup.indexes[j];
				if (Vector2.Distance(IT_Gesture.GetTouch(iD).position, touch.position) > maxFingerGroupDist * IT_Gesture.GetDPIFactor())
				{
					flag2 = false;
				}
			}
			if (flag2)
			{
				fingerGroup.indexes.Add(touch.fingerId);
				fingerGroup.positions.Add(touch.position);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.fingerGroup.Add(new FingerGroup(Time.realtimeSinceStartup, touch.fingerId, touch.position));
			StartCoroutine(this.fingerGroup[this.fingerGroup.Count - 1].Routine(this));
		}
	}
}
