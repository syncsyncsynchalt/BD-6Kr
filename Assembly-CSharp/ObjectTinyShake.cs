using System;
using System.Collections;
using UniRx;
using UnityEngine;

public class ObjectTinyShake : MonoBehaviour
{
	public float _shakePower = 20f;

	[Tooltip("シェイクのパワ\u30fcパラメ\u30fcタ")]
	public AnimationCurve _shakeCurve;

	[Tooltip("デバッグ用にル\u30fcプ起動するか")]
	public bool _startLoop;

	private bool _isAnimating;

	private void Start()
	{
		if (_startLoop)
		{
			PlayAnimation().DelayFrame(10).Repeat().Subscribe(delegate
			{
			});
		}
	}

	private void OnDestroy()
	{
		Mem.Del(ref _shakePower);
		Mem.Del(ref _shakeCurve);
		Mem.Del(ref _startLoop);
		Mem.Del(ref _isAnimating);
	}

	public UniRx.IObservable<int> PlayAnimation()
	{
		return Observable.FromCoroutine((UniRx.IObserver<int> observer) => AnimationCoroutine(observer));
	}

	private IEnumerator AnimationCoroutine(UniRx.IObserver<int> observer)
	{
		if (_isAnimating)
		{
			observer.OnNext(0);
			observer.OnCompleted();
			yield break;
		}
		_isAnimating = true;
		DebugUtils.dbgAssert(_shakeCurve.length >= 1);
		float duration = _shakeCurve.keys[_shakeCurve.length - 1].time;
		float beginTime = Time.time;
		Transform transform = base.transform;
		Vector3 initialPosition = transform.localPosition;
		while (true)
		{
			float t = Time.time - beginTime;
			if (t > duration)
			{
				break;
			}
			float basePower = _shakeCurve.Evaluate(t);
			float rt = t * (float)Math.PI * 2f * 20f;
			float rn = Mathf.PerlinNoise(t * 8f, 0f) * (float)Math.PI * 2f;
			Vector3 direction = new Vector3(Mathf.Cos(rt + rn), Mathf.Sin(rt + rn), 0f);
			transform.localPosition = initialPosition + direction * basePower * _shakePower;
			yield return null;
		}
		transform.localPosition = initialPosition;
		_isAnimating = false;
		observer.OnNext(0);
		observer.OnCompleted();
	}
}
