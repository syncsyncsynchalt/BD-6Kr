using System;
using UnityEngine;

namespace KCV.Remodel
{
	public class RemodelUtils : MonoBehaviour
	{
		private static void Move(GameObject o, Vector3 from, Vector3 to, float interval, Action onComplete)
		{
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(o, interval);
			tweenPosition.ResetToBeginning();
			tweenPosition.from = from;
			tweenPosition.to = to;
			tweenPosition.PlayForward();
			tweenPosition.ignoreTimeScale = true;
			tweenPosition.onFinished.Clear();
			tweenPosition.SetOnFinished(delegate
			{
				tweenPosition.onFinished.Clear();
				if (onComplete != null)
				{
					onComplete();
				}
			});
		}

		public static TweenPosition MoveWithManual(GameObject o, Vector3 from, Vector3 to, float interval, Action onComplete)
		{
			TweenPosition component = o.GetComponent<TweenPosition>();
			if (component != null)
			{
				UnityEngine.Object.DestroyImmediate(component);
			}
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(o, interval);
			tweenPosition.from = from;
			tweenPosition.to = to;
			tweenPosition.ignoreTimeScale = true;
			tweenPosition.onFinished.Clear();
			tweenPosition.SetOnFinished(delegate
			{
				tweenPosition.onFinished.Clear();
				if (onComplete != null)
				{
					onComplete();
				}
			});
			return tweenPosition;
		}

		public static void Move(GameObject o, Vector3 to, float interval, Action onComplete = null)
		{
			Move(o, o.transform.localPosition, to, interval, onComplete);
		}

		public static TweenPosition MoveWithManual(GameObject o, Vector3 to, float interval, Action onComplete = null)
		{
			return MoveWithManual(o, o.transform.localPosition, to, interval, onComplete);
		}
	}
}
