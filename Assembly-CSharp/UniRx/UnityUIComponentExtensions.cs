using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UniRx
{
	public static class UnityUIComponentExtensions
	{
		public static IDisposable SubscribeToText(this IObservable<string> source, Text text)
		{
			return source.Subscribe(delegate(string x)
			{
				text.text = x;
			});
		}

		public static IDisposable SubscribeToText<T>(this IObservable<T> source, Text text)
		{
			return source.Subscribe(delegate(T x)
			{
				text.text = x.ToString();
			});
		}

		public static IDisposable SubscribeToText<T>(this IObservable<T> source, Text text, Func<T, string> selector)
		{
			return source.Subscribe(delegate(T x)
			{
				text.text = selector(x);
			});
		}

		public static IDisposable SubscribeToInteractable(this IObservable<bool> source, Selectable selectable)
		{
			return source.Subscribe(delegate(bool x)
			{
				selectable.interactable = x;
			});
		}

		public static IObservable<Unit> OnClickAsObservable(this Button button)
		{
			return ((UnityEvent)button.onClick).AsObservable();
		}

		public static IObservable<bool> OnValueChangedAsObservable(this Toggle toggle)
		{
			return Observable.Create(delegate(IObserver<bool> observer)
			{
				observer.OnNext(toggle.isOn);
				return ((UnityEvent<bool>)toggle.onValueChanged).AsObservable().Subscribe(observer);
			});
		}

		public static IObservable<float> OnValueChangedAsObservable(this Scrollbar scrollbar)
		{
			return Observable.Create(delegate(IObserver<float> observer)
			{
				observer.OnNext(scrollbar.value);
				return ((UnityEvent<float>)scrollbar.onValueChanged).AsObservable().Subscribe(observer);
			});
		}

		public static IObservable<Vector2> OnValueChangedAsObservable(this ScrollRect scrollRect)
		{
			return Observable.Create(delegate(IObserver<Vector2> observer)
			{
				observer.OnNext(scrollRect.normalizedPosition);
				return ((UnityEvent<Vector2>)scrollRect.onValueChanged).AsObservable().Subscribe(observer);
			});
		}

		public static IObservable<float> OnValueChangedAsObservable(this Slider slider)
		{
			return Observable.Create(delegate(IObserver<float> observer)
			{
				observer.OnNext(slider.value);
				return ((UnityEvent<float>)slider.onValueChanged).AsObservable().Subscribe(observer);
			});
		}

		public static IObservable<string> OnEndEditAsObservable(this InputField inputField)
		{
			return ((UnityEvent<string>)inputField.onEndEdit).AsObservable();
		}

		public static IObservable<string> OnValueChangeAsObservable(this InputField inputField)
		{
			return Observable.Create(delegate(IObserver<string> observer)
			{
				observer.OnNext(inputField.text);
				return ((UnityEvent<string>)inputField.onValueChange).AsObservable().Subscribe(observer);
			});
		}
	}
}
