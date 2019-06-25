using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIWidget))]
	[RequireComponent(typeof(NoiseMove))]
	public class UINoiseScaleOutLabel : MonoBehaviour
	{
		[Serializable]
		private struct Params : IDisposable
		{
			public float intervalTime;

			public float scaleOutTime;

			public float scaleOutScale;

			public void Dispose()
			{
				Mem.Del(ref intervalTime);
				Mem.Del(ref scaleOutTime);
				Mem.Del(ref scaleOutScale);
			}
		}

		[SerializeField]
		private UIWidget _uiForeground;

		[SerializeField]
		[Header("[Animation Properties]")]
		private Params _strParams;

		private UIWidget _uiWidget;

		private UIWidget widget => this.GetComponentThis(ref _uiWidget);

		private void Awake()
		{
			_uiForeground.alpha = 0f;
			_uiForeground.depth = widget.depth + 1;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiForeground);
			Mem.DelIDisposableSafe(ref _strParams);
			Mem.Del(ref _uiWidget);
		}

		public void Play()
		{
			Observable.Interval(TimeSpan.FromSeconds(_strParams.intervalTime)).Subscribe(delegate
			{
				_uiForeground.alpha = 1f;
				_uiForeground.transform.LTValue(1f, 0f, _strParams.scaleOutTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					_uiForeground.alpha = x;
				});
				_uiForeground.transform.localScaleOne();
				_uiForeground.transform.LTScale(Vector3.one * _strParams.scaleOutScale, _strParams.scaleOutTime).setEase(LeanTweenType.linear);
			}).AddTo(base.gameObject);
		}
	}
}
