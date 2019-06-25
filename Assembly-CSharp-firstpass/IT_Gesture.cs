using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DragDetector))]
[RequireComponent(typeof(TapDetector))]
[RequireComponent(typeof(DualFingerDetector))]
[RequireComponent(typeof(SwipeDetector))]
[RequireComponent(typeof(BasicDetector))]
public class IT_Gesture : MonoBehaviour
{
	public delegate void MultiTapHandler(Tap tap);

	public delegate void LongTapHandler(Tap tap);

	public delegate void ChargeStartHandler(ChargedInfo cInfo);

	public delegate void ChargingHandler(ChargedInfo cInfo);

	public delegate void ChargeEndHandler(ChargedInfo cInfo);

	public delegate void MFMultiTapHandler(Tap tap);

	public delegate void MFLongTapHandler(Tap tap);

	public delegate void MFChargeStartHandler(ChargedInfo cInfo);

	public delegate void MFChargingHandler(ChargedInfo cInfo);

	public delegate void MFChargeEndHandler(ChargedInfo cInfo);

	public delegate void DraggingStartHandler(DragInfo dragInfo);

	public delegate void DraggingHandler(DragInfo dragInfo);

	public delegate void DraggingEndHandler(DragInfo dragInfo);

	public delegate void MFDraggingStartHandler(DragInfo dragInfo);

	public delegate void MFDraggingHandler(DragInfo dragInfo);

	public delegate void MFDraggingEndHandler(DragInfo dragInf);

	public delegate void SwipeStartHandler(SwipeInfo sw);

	public delegate void SwipingHandler(SwipeInfo sw);

	public delegate void SwipeEndHandler(SwipeInfo sw);

	public delegate void SwipeHandler(SwipeInfo sw);

	public delegate void PinchHandler(PinchInfo PI);

	public delegate void RotateHandler(RotateInfo RI);

	public delegate void TouchDownPosHandler(Vector2 pos);

	public delegate void TouchUpPosHandler(Vector2 pos);

	public delegate void TouchPosHandler(Vector2 pos);

	public delegate void TouchDownHandler(Touch touch);

	public delegate void TouchUpHandler(Touch touch);

	public delegate void TouchHandler(Touch touch);

	public delegate void Mouse1DownHandler(Vector2 pos);

	public delegate void Mouse1UpHandler(Vector2 pos);

	public delegate void Mouse1Handler(Vector2 pos);

	public delegate void Mouse2DownHandler(Vector2 pos);

	public delegate void Mouse2UpHandler(Vector2 pos);

	public delegate void Mouse2Handler(Vector2 pos);

	public delegate void ShortTapHandler(Vector2 pos);

	public delegate void DoubleTapHandler(Vector2 pos);

	public delegate void DFShortTapHandler(Vector2 pos);

	public delegate void DFLongTapHandler(Vector2 pos);

	public delegate void DFDoubleTapHandler(Vector2 pos);

	public delegate void DFChargingHandler(ChargedInfo cInfo);

	public delegate void DFChargeEndHandler(ChargedInfo cInfo);

	public delegate void DFDraggingHandler(DragInfo dragInfo);

	public delegate void DFDraggingEndHandler(Vector2 pos);

	public static IT_Gesture instance;

	public bool useDPIScaling = true;

	[NonSerialized]
	public float defaultDPI = 221.4f;

	private bool enableMultiTapFilter;

	private int tapExisted;

	private int MFtapExisted;

	private int maxMultiTapCount = 2;

	private float maxMultiTapInterval = 0.35f;

	public static event MultiTapHandler onMultiTapE;

	public static event LongTapHandler onLongTapE;

	public static event ChargeStartHandler onChargeStartE;

	public static event ChargingHandler onChargingE;

	public static event ChargeEndHandler onChargeEndE;

	public static event MFMultiTapHandler onMFMultiTapE;

	public static event MFLongTapHandler onMFLongTapE;

	public static event MFChargeStartHandler onMFChargeStartE;

	public static event MFChargingHandler onMFChargingE;

	public static event MFChargeEndHandler onMFChargeEndE;

	public static event DraggingStartHandler onDraggingStartE;

	public static event DraggingHandler onDraggingE;

	public static event DraggingEndHandler onDraggingEndE;

	public static event MFDraggingStartHandler onMFDraggingStartE;

	public static event MFDraggingHandler onMFDraggingE;

	public static event MFDraggingEndHandler onMFDraggingEndE;

	public static event SwipeStartHandler onSwipeStartE;

	public static event SwipingHandler onSwipingE;

	public static event SwipeEndHandler onSwipeEndE;

	public static event SwipeHandler onSwipeE;

	public static event PinchHandler onPinchE;

	public static event RotateHandler onRotateE;

	public static event TouchDownPosHandler onTouchDownPosE;

	public static event TouchUpPosHandler onTouchUpPosE;

	public static event TouchPosHandler onTouchPosE;

	public static event TouchDownHandler onTouchDownE;

	public static event TouchUpHandler onTouchUpE;

	public static event TouchHandler onTouchE;

	public static event Mouse1DownHandler onMouse1DownE;

	public static event Mouse1UpHandler onMouse1UpE;

	public static event Mouse1Handler onMouse1E;

	public static event Mouse2DownHandler onMouse2DownE;

	public static event Mouse2UpHandler onMouse2UpE;

	public static event Mouse2Handler onMouse2E;

	public static event ShortTapHandler onShortTapE;

	public static event DoubleTapHandler onDoubleTapE;

	public static event DFShortTapHandler onDFShortTapE;

	public static event DFLongTapHandler onDFLongTapE;

	public static event DFDoubleTapHandler onDFDoubleTapE;

	public static event DFChargingHandler onDFChargingE;

	public static event DFChargeEndHandler onDFChargeEndE;

	public static event DFDraggingHandler onDualFDraggingE;

	public static event DFDraggingEndHandler onDualFDraggingEndE;

	private void Awake()
	{
		instance = this;
	}

	public static Vector2 ConvertToCurrentResolution(Vector2 inPos)
	{
		Vector2 zero = Vector2.zero;
		zero.x = (int)(inPos.x * (float)Screen.currentResolution.width / (float)Screen.width);
		zero.y = (int)(inPos.y * (float)Screen.currentResolution.height / (float)Screen.height);
		return zero;
	}

	public static float GetDefaultDPI()
	{
		return instance.defaultDPI;
	}

	public static float GetCurrentDPI()
	{
		return (Screen.dpi == 0f) ? GetDefaultDPI() : Screen.dpi;
	}

	public static float GetDPIFactor()
	{
		return instance.useDPIScaling ? (GetCurrentDPI() / GetDefaultDPI()) : 1f;
	}

	public static void SetMultiTapFilter(bool flag)
	{
		if (instance != null)
		{
			instance.enableMultiTapFilter = flag;
		}
	}

	public static void SetMaxMultiTapCount(int val)
	{
		if (instance != null)
		{
			instance.maxMultiTapCount = val;
		}
	}

	public static void SetMaxMultiTapInterval(float val)
	{
		if (instance != null)
		{
			instance.maxMultiTapInterval = val;
		}
	}

	private void CheckMultiTap(Tap tap)
	{
		tapExisted++;
		if (tap.count == maxMultiTapCount)
		{
			tapExisted = 0;
			if (IT_Gesture.onMultiTapE != null)
			{
				IT_Gesture.onMultiTapE(tap);
			}
		}
		else
		{
			StartCoroutine(TapCoroutine(tap));
		}
	}

	private IEnumerator TapCoroutine(Tap tap)
	{
		yield return new WaitForSeconds((float)maxMultiTapCount * maxMultiTapInterval);
		if (tapExisted == tap.count)
		{
			tapExisted = 0;
			if (IT_Gesture.onMultiTapE != null)
			{
				IT_Gesture.onMultiTapE(tap);
			}
		}
	}

	private void CheckMFMultiTap(Tap tap)
	{
		MFtapExisted++;
		if (tap.count == maxMultiTapCount)
		{
			MFtapExisted = 0;
			if (IT_Gesture.onMFMultiTapE != null)
			{
				IT_Gesture.onMFMultiTapE(tap);
			}
		}
		else
		{
			StartCoroutine(MFTapCoroutine(tap));
		}
	}

	private IEnumerator MFTapCoroutine(Tap tap)
	{
		yield return new WaitForSeconds((float)maxMultiTapCount * maxMultiTapInterval);
		if (MFtapExisted == tap.count)
		{
			MFtapExisted = 0;
			if (IT_Gesture.onMFMultiTapE != null)
			{
				IT_Gesture.onMFMultiTapE(tap);
			}
		}
	}

	public static void MultiTap(Tap tap)
	{
		if (tap.fingerCount == 1)
		{
			if (tap.count == 1)
			{
				if (IT_Gesture.onShortTapE != null)
				{
					IT_Gesture.onShortTapE(tap.pos);
				}
			}
			else if (tap.count == 2 && IT_Gesture.onDoubleTapE != null)
			{
				IT_Gesture.onDoubleTapE(tap.pos);
			}
			if (instance.enableMultiTapFilter)
			{
				instance.CheckMultiTap(tap);
			}
			else if (IT_Gesture.onMultiTapE != null)
			{
				IT_Gesture.onMultiTapE(tap);
			}
			return;
		}
		if (tap.fingerCount == 2)
		{
			if (tap.count == 1)
			{
				DFShortTap(tap.pos);
			}
			else if (tap.count == 2)
			{
				DFDoubleTap(tap.pos);
			}
		}
		if (instance.enableMultiTapFilter)
		{
			instance.CheckMFMultiTap(tap);
		}
		else if (IT_Gesture.onMFMultiTapE != null)
		{
			IT_Gesture.onMFMultiTapE(tap);
		}
	}

	public static void LongTap(Tap tap)
	{
		if (tap.fingerCount > 1)
		{
			if (tap.fingerCount == 2 && IT_Gesture.onDFLongTapE != null)
			{
				IT_Gesture.onDFLongTapE(tap.pos);
			}
			if (IT_Gesture.onMFLongTapE != null)
			{
				IT_Gesture.onMFLongTapE(tap);
			}
		}
		else if (IT_Gesture.onLongTapE != null)
		{
			IT_Gesture.onLongTapE(tap);
		}
	}

	public static void ChargeStart(ChargedInfo cInfo)
	{
		if (cInfo.fingerCount > 1)
		{
			if (IT_Gesture.onMFChargeStartE != null)
			{
				IT_Gesture.onMFChargeStartE(cInfo);
			}
		}
		else if (IT_Gesture.onChargeStartE != null)
		{
			IT_Gesture.onChargeStartE(cInfo);
		}
	}

	public static void Charging(ChargedInfo cInfo)
	{
		if (cInfo.fingerCount > 1)
		{
			if (cInfo.fingerCount == 2)
			{
				DFCharging(cInfo);
			}
			if (IT_Gesture.onMFChargingE != null)
			{
				IT_Gesture.onMFChargingE(cInfo);
			}
		}
		else if (IT_Gesture.onChargingE != null)
		{
			IT_Gesture.onChargingE(cInfo);
		}
	}

	public static void ChargeEnd(ChargedInfo cInfo)
	{
		if (cInfo.fingerCount > 1)
		{
			if (cInfo.fingerCount == 2)
			{
				DFChargingEnd(cInfo);
			}
			if (IT_Gesture.onMFChargeEndE != null)
			{
				IT_Gesture.onMFChargeEndE(cInfo);
			}
		}
		else if (IT_Gesture.onChargeEndE != null)
		{
			IT_Gesture.onChargeEndE(cInfo);
		}
	}

	public static void DraggingStart(DragInfo dragInfo)
	{
		if (dragInfo.fingerCount > 1)
		{
			if (IT_Gesture.onMFDraggingStartE != null)
			{
				IT_Gesture.onMFDraggingStartE(dragInfo);
			}
		}
		else if (IT_Gesture.onDraggingStartE != null)
		{
			IT_Gesture.onDraggingStartE(dragInfo);
		}
	}

	public static void Dragging(DragInfo dragInfo)
	{
		if (dragInfo.fingerCount > 1)
		{
			if (dragInfo.fingerCount == 2)
			{
				DFDragging(dragInfo);
			}
			if (IT_Gesture.onMFDraggingE != null)
			{
				IT_Gesture.onMFDraggingE(dragInfo);
			}
		}
		else if (IT_Gesture.onDraggingE != null)
		{
			IT_Gesture.onDraggingE(dragInfo);
		}
	}

	public static void DraggingEnd(DragInfo dragInfo)
	{
		if (dragInfo.fingerCount > 1)
		{
			if (dragInfo.fingerCount == 2)
			{
				DFDraggingEnd(dragInfo);
			}
			if (IT_Gesture.onMFDraggingEndE != null)
			{
				IT_Gesture.onMFDraggingEndE(dragInfo);
			}
		}
		else if (IT_Gesture.onDraggingEndE != null)
		{
			IT_Gesture.onDraggingEndE(dragInfo);
		}
	}

	public static void SwipeStart(SwipeInfo sw)
	{
		if (IT_Gesture.onSwipeStartE != null)
		{
			IT_Gesture.onSwipeStartE(sw);
		}
	}

	public static void Swiping(SwipeInfo sw)
	{
		if (IT_Gesture.onSwipingE != null)
		{
			IT_Gesture.onSwipingE(sw);
		}
	}

	public static void SwipeEnd(SwipeInfo sw)
	{
		if (IT_Gesture.onSwipeEndE != null)
		{
			IT_Gesture.onSwipeEndE(sw);
		}
	}

	public static void Swipe(SwipeInfo sw)
	{
		if (IT_Gesture.onSwipeE != null)
		{
			IT_Gesture.onSwipeE(sw);
		}
	}

	public static void Pinch(PinchInfo PI)
	{
		if (IT_Gesture.onPinchE != null)
		{
			IT_Gesture.onPinchE(PI);
		}
	}

	public static void Rotate(RotateInfo RI)
	{
		if (IT_Gesture.onRotateE != null)
		{
			IT_Gesture.onRotateE(RI);
		}
	}

	public static void OnTouchDown(Touch touch)
	{
		if (IT_Gesture.onTouchDownPosE != null)
		{
			IT_Gesture.onTouchDownPosE(touch.position);
		}
		if (IT_Gesture.onTouchDownE != null)
		{
			IT_Gesture.onTouchDownE(touch);
		}
	}

	public static void OnTouchUp(Touch touch)
	{
		if (IT_Gesture.onTouchUpPosE != null)
		{
			IT_Gesture.onTouchUpPosE(touch.position);
		}
		if (IT_Gesture.onTouchUpE != null)
		{
			IT_Gesture.onTouchUpE(touch);
		}
	}

	public static void OnTouch(Touch touch)
	{
		if (IT_Gesture.onTouchPosE != null)
		{
			IT_Gesture.onTouchPosE(touch.position);
		}
		if (IT_Gesture.onTouchE != null)
		{
			IT_Gesture.onTouchE(touch);
		}
	}

	public static void OnMouse1Down(Vector2 pos)
	{
		if (IT_Gesture.onMouse1DownE != null)
		{
			IT_Gesture.onMouse1DownE(pos);
		}
	}

	public static void OnMouse1Up(Vector2 pos)
	{
		if (IT_Gesture.onMouse1UpE != null)
		{
			IT_Gesture.onMouse1UpE(pos);
		}
	}

	public static void OnMouse1(Vector2 pos)
	{
		if (IT_Gesture.onMouse1E != null)
		{
			IT_Gesture.onMouse1E(pos);
		}
	}

	public static void OnMouse2Down(Vector2 pos)
	{
		if (IT_Gesture.onMouse2DownE != null)
		{
			IT_Gesture.onMouse2DownE(pos);
		}
	}

	public static void OnMouse2Up(Vector2 pos)
	{
		if (IT_Gesture.onMouse2UpE != null)
		{
			IT_Gesture.onMouse2UpE(pos);
		}
	}

	public static void OnMouse2(Vector2 pos)
	{
		if (IT_Gesture.onMouse2E != null)
		{
			IT_Gesture.onMouse2E(pos);
		}
	}

	public static float VectorToAngle(Vector2 dir)
	{
		if (dir.x == 0f)
		{
			if (dir.y > 0f)
			{
				return 90f;
			}
			if (dir.y < 0f)
			{
				return 270f;
			}
			return 0f;
		}
		if (dir.y == 0f)
		{
			if (dir.x > 0f)
			{
				return 0f;
			}
			if (dir.x < 0f)
			{
				return 180f;
			}
			return 0f;
		}
		float num = Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y);
		float num2 = Mathf.Asin(dir.y / num) * 57.29578f;
		if (dir.y > 0f)
		{
			if (dir.x < 0f)
			{
				num2 = 180f - num2;
			}
		}
		else
		{
			if (dir.x > 0f)
			{
				num2 = 360f + num2;
			}
			if (dir.x < 0f)
			{
				num2 = 180f - num2;
			}
		}
		return num2;
	}

	public static Touch GetTouch(int ID)
	{
		Touch result = default(Touch);
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touches.Length; i++)
			{
				Touch result2 = Input.touches[i];
				if (result2.fingerId == ID)
				{
					return result2;
				}
			}
		}
		return result;
	}

	public static void ShortTap(Vector2 pos)
	{
		if (IT_Gesture.onShortTapE != null)
		{
			IT_Gesture.onShortTapE(pos);
		}
	}

	public static void DoubleTap(Vector2 pos)
	{
		if (IT_Gesture.onDoubleTapE != null)
		{
			IT_Gesture.onDoubleTapE(pos);
		}
	}

	public static void DFShortTap(Vector2 pos)
	{
		if (IT_Gesture.onDFShortTapE != null)
		{
			IT_Gesture.onDFShortTapE(pos);
		}
	}

	public static void DFLongTap(Vector2 pos)
	{
		if (IT_Gesture.onDFLongTapE != null)
		{
			IT_Gesture.onDFLongTapE(pos);
		}
	}

	public static void DFDoubleTap(Vector2 pos)
	{
		if (IT_Gesture.onDFDoubleTapE != null)
		{
			IT_Gesture.onDFDoubleTapE(pos);
		}
	}

	public static void DFCharging(ChargedInfo cInfo)
	{
		if (IT_Gesture.onDFChargingE != null)
		{
			IT_Gesture.onDFChargingE(cInfo);
		}
	}

	public static void DFChargingEnd(ChargedInfo cInfo)
	{
		if (IT_Gesture.onDFChargeEndE != null)
		{
			IT_Gesture.onDFChargeEndE(cInfo);
		}
	}

	public static void DFDragging(DragInfo dragInfo)
	{
		if (IT_Gesture.onDualFDraggingE != null)
		{
			IT_Gesture.onDualFDraggingE(dragInfo);
		}
	}

	public static void DFDraggingEnd(DragInfo dragInfo)
	{
		if (IT_Gesture.onDualFDraggingEndE != null)
		{
			IT_Gesture.onDualFDraggingEndE(dragInfo.pos);
		}
	}
}
