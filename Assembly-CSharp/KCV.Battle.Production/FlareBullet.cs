using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(LensFlare))]
	public class FlareBullet : MonoBehaviour
	{
		[Serializable]
		public class Params
		{
			[Range(0f, 50f)]
			public float _noiseSpeed = 8f;

			public float _fallLength = 20f;

			public float _delay;

			public Color _flareColor = Color.white;
		}

		[Serializable]
		public struct Anim
		{
			[Range(0f, 1f)]
			public float _shine;

			public float _y;
		}

		[Serializable]
		public struct Binding
		{
			public ParticleSystem _smoke;

			public LensFlare _flare;

			public Animation _animation;
		}

		public Params _params = new Params();

		public Anim _anim = default(Anim);

		public Binding _binding;

		private float _outputPower;

		private float _moveRandomSeed;

		private bool _isAnimating;

		public float OutputPower => _outputPower;

		private static float remap(float value, float inputMin, float inputMax, float outputMin, float outputMax, bool isClamp)
		{
			if (isClamp)
			{
				value = Mathf.Clamp(value, inputMin, inputMax);
			}
			return (value - inputMin) * ((outputMax - outputMin) / (inputMax - inputMin)) + outputMin;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _binding);
			Mem.Del(ref _anim);
			Mem.Del(ref _params);
		}

		private void Start()
		{
			_moveRandomSeed = UnityEngine.Random.value * 100f;
			_binding._smoke.enableEmission = false;
			_anim._shine = 0f;
		}

		private void Update()
		{
			float time = Time.time;
			float value = 1f - Mathf.Pow(Mathf.PerlinNoise(time * _params._noiseSpeed, _moveRandomSeed), 2f);
			float num = remap(value, 0f, 1f, 0.6f, 1f, isClamp: false);
			_outputPower = _anim._shine * num;
			_binding._flare.color = _params._flareColor * _outputPower;
			_binding._flare.brightness = remap(_outputPower, 0f, 1f, 0.6f, 1f, isClamp: true);
			float num2 = 1.5f;
			float y = (0f - _anim._y) * _params._fallLength;
			float x = remap(Mathf.PerlinNoise(_anim._y * 2f, _moveRandomSeed), 0f, 1f, -1f, 1f, isClamp: false) * num2;
			base.transform.localPosition = new Vector3(x, y, 0f);
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
			_binding._smoke.enableEmission = true;
			_anim._shine = 0f;
			yield return new WaitForSeconds(_params._delay);
			string anim = "FlareBullet";
			_binding._animation.Play(anim);
            yield return new WaitForSeconds(_binding._animation.GetClip(anim).length);
			_binding._smoke.enableEmission = false;
			yield return new WaitForSeconds(3f);
			_anim._shine = 0f;
			_isAnimating = false;
			observer.OnNext(0);
			observer.OnCompleted();
		}

		public UniRx.IObservable<int> PlayAnimation()
		{
			return Observable.FromCoroutine((UniRx.IObserver<int> observer) => AnimationCoroutine(observer));
		}
	}
}
