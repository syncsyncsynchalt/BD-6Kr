using System;
using System.Collections;
using UnityEngine;

namespace UniRx
{
	public static class ObserveExtensions
	{
		public static IObservable<TProperty> ObserveEveryValueChanged<TSource, TProperty>(this TSource source, Func<TSource, TProperty> propertySelector, FrameCountType frameCountType = FrameCountType.Update) where TSource : class
		{
			if (source == null)
			{
				return Observable.Empty<TProperty>();
			}
			UnityEngine.Object unityObject = source as UnityEngine.Object;
			bool flag = source is UnityEngine.Object;
			if (flag && unityObject == null)
			{
				return Observable.Empty<TProperty>();
			}
			if (flag)
			{
				return Observable.FromCoroutine((IObserver<TProperty> observer, CancellationToken cancellationToken) => PublishUnityObjectValueChanged(unityObject, propertySelector, frameCountType, observer, cancellationToken));
			}
			WeakReference reference = new WeakReference(source);
			source = (TSource)null;
			return Observable.FromCoroutine((IObserver<TProperty> observer, CancellationToken cancellationToken) => PublishPocoValueChanged(reference, propertySelector, frameCountType, observer, cancellationToken));
		}

		private static IEnumerator PublishPocoValueChanged<TSource, TProperty>(WeakReference sourceReference, Func<TSource, TProperty> propertySelector, FrameCountType frameCountType, IObserver<TProperty> observer, CancellationToken cancellationToken)
		{
			bool isFirst = true;
			TProperty currentValue = default(TProperty);
			TProperty prevValue = default(TProperty);
			while (true)
			{
				if (!cancellationToken.IsCancellationRequested)
				{
					object target = sourceReference.Target;
					if (target == null)
					{
						break;
					}
					try
					{
						currentValue = propertySelector((TSource)target);
					}
					catch (Exception ex2)
					{
						Exception ex = ex2;
						observer.OnError(ex);
						yield break;
					}
					finally
					{
                        throw new NotImplementedException("‚È‚É‚±‚ê");
                        // base._003C_003E__Finally0();
					}
					if (isFirst || !object.Equals(currentValue, prevValue))
					{
						isFirst = false;
						observer.OnNext(currentValue);
						prevValue = currentValue;
					}
					yield return frameCountType.GetYieldInstruction();
					continue;
				}
				yield break;
			}
			observer.OnCompleted();
		}

		private static IEnumerator PublishUnityObjectValueChanged<TSource, TProperty>(UnityEngine.Object unityObject, Func<TSource, TProperty> propertySelector, FrameCountType frameCountType, IObserver<TProperty> observer, CancellationToken cancellationToken)
		{
			bool isFirst = true;
			TProperty prevValue = default(TProperty);
			TSource source = (TSource)(object)unityObject;
			while (true)
			{
				if (!cancellationToken.IsCancellationRequested)
				{
					if (!(unityObject != null))
					{
						break;
					}
					TProperty currentValue;
					try
					{
						currentValue = propertySelector(source);
					}
					catch (Exception ex)
					{
						observer.OnError(ex);
						yield break;
					}
					if (isFirst || !object.Equals(currentValue, prevValue))
					{
						isFirst = false;
						observer.OnNext(currentValue);
						prevValue = currentValue;
					}
					yield return frameCountType.GetYieldInstruction();
					continue;
				}
				yield break;
			}
			observer.OnCompleted();
		}
	}
}
