using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerGroup
{
	private enum _ChargeState
	{
		Clear,
		Charging,
		Charged
	}

	public List<int> indexes = new List<int>();

	public List<Vector2> positions = new List<Vector2>();

	public Vector2 posAvg;

	public float triggerTime;

	public bool routineEnded;

	public int count;

	public bool longTap;

	private _ChargeState chargeState;

	private int chargeDir = 1;

	private int chargeConst;

	private float startTimeCharge = Time.realtimeSinceStartup;

	private int[] indexList;

	private Vector2[] posList;

	private TapDetector tapDetector;

	public FingerGroup(float time, int index, Vector2 pos)
	{
		indexes.Add(index);
		positions.Add(pos);
	}

	public IEnumerator Routine(TapDetector tapD)
	{
		tapDetector = tapD;
		triggerTime = Time.realtimeSinceStartup;
		startTimeCharge = Time.realtimeSinceStartup;
		yield return new WaitForSeconds(0.075f);
		if (indexes.Count < 2)
		{
			routineEnded = true;
			yield break;
		}
		count = indexes.Count;
		posAvg = Vector2.zero;
		for (int i = 0; i < positions.Count; i++)
		{
			posAvg += positions[i];
		}
		posAvg /= (float)positions.Count;
		this.posList = new Vector2[positions.Count];
		positions.CopyTo(this.posList);
		indexList = new int[indexes.Count];
		indexes.CopyTo(indexList);
		bool isOn = true;
		float liftTime = -1f;
		while (isOn)
		{
			for (int j = 0; j < indexes.Count; j++)
			{
				Touch touch = IT_Gesture.GetTouch(indexes[j]);
				if (touch.phase == TouchPhase.Moved)
				{
					isOn = false;
				}
				if (touch.position == Vector2.zero)
				{
					if (indexes.Count == count)
					{
						liftTime = Time.realtimeSinceStartup;
					}
					indexes.RemoveAt(j);
					j--;
				}
			}
			if (Time.realtimeSinceStartup - startTimeCharge > tapDetector.minChargeTime && chargeState == _ChargeState.Clear)
			{
				chargeState = _ChargeState.Charging;
				ChargedInfo cInfo3 = new ChargedInfo(val: Mathf.Clamp((float)chargeConst + (float)chargeDir * ((Time.realtimeSinceStartup - startTimeCharge) / tapDetector.maxChargeTime), 0f, 1f), p: posAvg, posL: this.posList, inds: indexList);
				IT_Gesture.ChargeStart(cInfo3);
			}
			else if (chargeState == _ChargeState.Charging)
			{
				float chargedValue = Mathf.Clamp((float)chargeConst + (float)chargeDir * ((Time.realtimeSinceStartup - startTimeCharge) / tapDetector.maxChargeTime), 0f, 1f);
				ChargedInfo cInfo2 = new ChargedInfo(posAvg, this.posList, chargedValue, indexList);
				if (tapDetector.chargeMode == _ChargeMode.PingPong)
				{
					if (chargedValue == 1f || chargedValue == 0f)
					{
						chargeDir *= -1;
						if (chargeDir == 1)
						{
							chargeConst = 0;
						}
						else if (chargeDir == -1)
						{
							chargeConst = 1;
						}
						startTimeCharge = Time.realtimeSinceStartup;
					}
					IT_Gesture.Charging(cInfo2);
				}
				else if (chargedValue < 1f)
				{
					IT_Gesture.Charging(cInfo2);
				}
				else
				{
					cInfo2.percent = 1f;
					if (tapDetector.chargeMode == _ChargeMode.Once)
					{
						chargeState = _ChargeState.Charged;
						IT_Gesture.ChargeEnd(cInfo2);
						startTimeCharge = float.PositiveInfinity;
					}
					else if (tapDetector.chargeMode == _ChargeMode.Clamp)
					{
						chargeState = _ChargeState.Charged;
						IT_Gesture.Charging(cInfo2);
					}
					else if (tapDetector.chargeMode == _ChargeMode.Loop)
					{
						chargeState = _ChargeState.Clear;
						IT_Gesture.ChargeEnd(cInfo2);
						startTimeCharge = Time.realtimeSinceStartup;
					}
				}
			}
			if (!longTap && Time.realtimeSinceStartup - triggerTime > tapDetector.longTapTime && indexes.Count == count)
			{
				Vector2[] posList2 = new Vector2[positions.Count];
				positions.CopyTo(posList2);
				Tap tap = new Tap(1, count, posList2, indexList);
				IT_Gesture.LongTap(tap);
				longTap = true;
			}
			if (indexes.Count < count && (Time.realtimeSinceStartup - liftTime > 0.075f || indexes.Count == 0))
			{
				if (indexes.Count == 0 && liftTime - triggerTime < tapDetector.shortTapTime + 0.1f)
				{
					Vector2[] posList = new Vector2[positions.Count];
					positions.CopyTo(posList);
					tapDetector.CheckMultiTapMFTouch(count, posList, indexList);
				}
				break;
			}
			yield return null;
		}
		if (chargeState == _ChargeState.Charging || (chargeState == _ChargeState.Charged && tapDetector.chargeMode != 0))
		{
			ChargedInfo cInfo = new ChargedInfo(val: Mathf.Clamp((float)chargeConst + (float)chargeDir * ((Time.realtimeSinceStartup - startTimeCharge) / tapDetector.maxChargeTime), 0f, 1f), p: posAvg, posL: this.posList, inds: indexList);
			IT_Gesture.ChargeEnd(cInfo);
		}
		routineEnded = true;
	}
}
