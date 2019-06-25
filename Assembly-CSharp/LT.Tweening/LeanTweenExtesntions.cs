using System;
using System.Collections.Generic;
using UnityEngine;

namespace LT.Tweening
{
	public static class LeanTweenExtesntions
	{
		public static void LTCancel(this Transform self)
		{
			LeanTween.cancel(self.gameObject);
		}

		public static void LTCancel(this Transform self, bool callComplete)
		{
			LeanTween.cancel(self.gameObject, callComplete);
		}

		public static void LTCancel(this Transform self, int uniqueId)
		{
			LeanTween.cancel(self.gameObject, uniqueId);
		}

		public static void LTCancel(this Transform self, LTRect ltRect, int uniqueId)
		{
			LeanTween.cancel(ltRect, uniqueId);
		}

		public static LTDescr LTDesctiption(this Transform self, int uniqueId)
		{
			return LeanTween.description(uniqueId);
		}

		public static void LTPause(this Transform self, int uniqueId)
		{
			LeanTween.pause(uniqueId);
		}

		public static void LTPause(this Transform self)
		{
			LeanTween.pause(self.gameObject);
		}

		public static void LTPauseAll(this Transform self)
		{
			LeanTween.pauseAll();
		}

		public static void LTResumeAll(this Transform self)
		{
			LeanTween.resumeAll();
		}

		public static void LTResume(this Transform self, int uniqueId)
		{
			LeanTween.resume(uniqueId);
		}

		public static void LTResume(this Transform self)
		{
			LeanTween.resume(self.gameObject);
		}

		public static bool LTIsTweening(this Transform self)
		{
			return LeanTween.isTweening(self.gameObject);
		}

		public static bool LTIsTweening(this Transform self, int uniqueId)
		{
			return LeanTween.isTweening(uniqueId);
		}

		public static bool LTIsTweening(this Transform self, LTRect ltRect)
		{
			return LeanTween.isTweening(ltRect);
		}

		public static void LTDrawBezierPath(this Transform self, Vector3 a, Vector3 b, Vector3 c, Vector3 d, float arrowSize, Transform arrowTransform)
		{
			LeanTween.drawBezierPath(a, b, c, d, arrowSize, arrowTransform);
		}

		public static void LTLogError(this Transform self, string error)
		{
			LeanTween.logError(error);
		}

		public static LTDescr LTDelayedCall(this Transform self, float delayTime, Action callback)
		{
			return LeanTween.delayedCall(self.gameObject, delayTime, callback);
		}

		public static LTDescr LTDelayedCall(this Transform self, float delayTime, Action<object> callback)
		{
			return LeanTween.delayedCall(self.gameObject, delayTime, callback);
		}

		public static LTDescr LTMove(this Transform self, Vector3 to, float time)
		{
			return LeanTween.move(self.gameObject, to, time);
		}

		public static LTDescr LTMove(this Transform self, Vector2 to, float time)
		{
			return LeanTween.move(self.gameObject, to, time);
		}

		public static LTDescr LTMove(this Transform self, Vector3[] to, float time)
		{
			return LeanTween.move(self.gameObject, to, time);
		}

		public static LTDescr LTMove(this Transform self, List<Vector3> to, float time)
		{
			return LeanTween.move(self.gameObject, to.ToArray(), time);
		}

		public static LTDescr LTMoveSpline(this Transform self, Vector3[] to, float time)
		{
			return LeanTween.moveSpline(self.gameObject, to, time);
		}

		public static LTDescr LTMoveSpline(this Transform self, List<Vector3> to, float time)
		{
			return LeanTween.moveSpline(self.gameObject, to.ToArray(), time);
		}

		public static LTDescr LTMoveSplineLocal(this Transform self, Vector3[] to, float time)
		{
			return LeanTween.moveSplineLocal(self.gameObject, to, time);
		}

		public static LTDescr LTMoveSplineLocal(this Transform self, List<Vector3> to, float time)
		{
			return LeanTween.moveSplineLocal(self.gameObject, to.ToArray(), time);
		}

		public static LTDescr LTMoveX(this Transform self, float to, float time)
		{
			return LeanTween.moveX(self.gameObject, to, time);
		}

		public static LTDescr LTMoveY(this Transform self, float to, float time)
		{
			return LeanTween.moveY(self.gameObject, to, time);
		}

		public static LTDescr LTMoveZ(this Transform self, float to, float time)
		{
			return LeanTween.moveZ(self.gameObject, to, time);
		}

		public static LTDescr LTMoveLocal(this Transform self, Vector3 to, float time)
		{
			return LeanTween.moveLocal(self.gameObject, to, time);
		}

		public static LTDescr LTMoveLocal(this Transform self, Vector3[] to, float time)
		{
			return LeanTween.moveLocal(self.gameObject, to, time);
		}

		public static LTDescr LTMoveLocal(this Transform self, List<Vector3> to, float time)
		{
			return LeanTween.moveLocal(self.gameObject, to.ToArray(), time);
		}

		public static LTDescr LTMoveLocalX(this Transform self, float to, float time)
		{
			return LeanTween.moveLocalX(self.gameObject, to, time);
		}

		public static LTDescr LTMoveLocalY(this Transform self, float to, float time)
		{
			return LeanTween.moveLocalY(self.gameObject, to, time);
		}

		public static LTDescr LTMoveLocalZ(this Transform self, float to, float time)
		{
			return LeanTween.moveLocalZ(self.gameObject, to, time);
		}

		public static LTDescr LTRotate(this Transform self, Vector3 to, float time)
		{
			return LeanTween.rotate(self.gameObject, to, time);
		}

		public static LTDescr LTRotateLocal(this Transform self, Vector3 to, float time)
		{
			return LeanTween.rotateLocal(self.gameObject, to, time);
		}

		public static LTDescr LTRotateX(this Transform self, float to, float time)
		{
			return LeanTween.rotateX(self.gameObject, to, time);
		}

		public static LTDescr LTRotateY(this Transform self, float to, float time)
		{
			return LeanTween.rotateY(self.gameObject, to, time);
		}

		public static LTDescr LTRotateZ(this Transform self, float to, float time)
		{
			return LeanTween.rotateZ(self.gameObject, to, time);
		}

		public static LTDescr LTRotateAround(this Transform self, Vector3 axis, float add, float time)
		{
			return LeanTween.rotateAround(self.gameObject, axis, add, time);
		}

		public static LTDescr LTRotateAroundLocal(this Transform self, Vector3 axis, float add, float time)
		{
			return LeanTween.rotateAroundLocal(self.gameObject, axis, add, time);
		}

		public static LTDescr LTScale(this Transform self, Vector3 to, float time)
		{
			return LeanTween.scale(self.gameObject, to, time);
		}

		public static LTDescr LTScaleX(this Transform self, float to, float time)
		{
			return LeanTween.scaleX(self.gameObject, to, time);
		}

		public static LTDescr LTScaleY(this Transform self, float to, float time)
		{
			return LeanTween.scaleY(self.gameObject, to, time);
		}

		public static LTDescr LTScaleZ(this Transform self, float to, float time)
		{
			return LeanTween.scaleZ(self.gameObject, to, time);
		}

		public static LTDescr LTValue(this Transform self, float from, float to, float time)
		{
			return LeanTween.value(self.gameObject, from, to, time);
		}

		public static LTDescr LTValue(this Transform self, Vector2 from, Vector2 to, float time)
		{
			return LeanTween.value(self.gameObject, from, to, time);
		}

		public static LTDescr LTValue(this Transform self, Vector3 from, Vector3 to, float time)
		{
			return LeanTween.value(self.gameObject, from, to, time);
		}

		public static LTDescr LTValue(this Transform self, Color from, Color to, float time)
		{
			return LeanTween.value(self.gameObject, from, to, time);
		}

		public static LTDescr LTValue(this Transform self, Action<float> callOnUpdate, float from, float to, float time)
		{
			return LeanTween.value(self.gameObject, callOnUpdate, from, to, time);
		}

		public static LTDescr LTValue(this Transform self, Action<Color> callOnUpdate, Color from, Color to, float time)
		{
			return LeanTween.value(self.gameObject, callOnUpdate, from, to, time);
		}

		public static LTDescr LTValue(this Transform self, Action<Vector2> callOnUpdate, Vector2 from, Vector2 to, float time)
		{
			return LeanTween.value(self.gameObject, callOnUpdate, from, to, time);
		}

		public static LTDescr LTValue(this Transform self, Action<Vector3> callOnUpdate, Vector3 from, Vector3 to, float time)
		{
			return LeanTween.value(self.gameObject, from, to, time);
		}

		public static LTDescr LTValue(this Transform self, Action<float, object> callOnUpdate, float from, float to, float time)
		{
			return LeanTween.value(self.gameObject, callOnUpdate, from, to, time);
		}
	}
}
