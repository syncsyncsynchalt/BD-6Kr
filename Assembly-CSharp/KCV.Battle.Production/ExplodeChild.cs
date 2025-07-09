using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ExplodeChild : MonoBehaviour
	{
		private struct Fragment : IDisposable
		{
			public Transform transform;

			public Vector3 initialPosition;

			public Quaternion initialRotation;

			public Vector3 velocity;

			public Quaternion angularVelocity;

			public void Dispose()
			{
				Mem.DelMeshSafe(ref transform);
				Mem.Del(ref transform);
				Mem.Del(ref initialPosition);
				Mem.Del(ref initialRotation);
				Mem.Del(ref velocity);
				Mem.Del(ref angularVelocity);
			}
		}

		[SerializeField]
		[Tooltip("破片回転速度")]
		private float _anglarSpeed = 18f;

		[SerializeField]
		[Tooltip("爆発初速")]
		private float _explodePower = 4.5f;

		[Tooltip("初速力方向ランダム度数")]
		[SerializeField]
		private float _explodeDirectionRandomize = 45f;

		[SerializeField]
		[Tooltip("重力加速度")]
		private float _gravity = 15.5f;

		[SerializeField]
		[Tooltip("上方向へのふっとび補正")]
		private float _upPowerPlus = 1.3f;

		[SerializeField]
		[Tooltip("デバッグ用にループ起動するか")]
		private bool _startLoop;

		private List<Fragment> _fragments = new List<Fragment>();

		private bool _isAnimating;

		public float anglarSpeed
		{
			get
			{
				return _anglarSpeed;
			}
			set
			{
				if (value != _anglarSpeed)
				{
					_anglarSpeed = value;
				}
			}
		}

		public float explodePower
		{
			get
			{
				return _explodePower;
			}
			set
			{
				if (value != _explodePower)
				{
					_explodePower = value;
				}
			}
		}

		public float explodeDirectionRandomize
		{
			get
			{
				return _explodeDirectionRandomize;
			}
			set
			{
				if (value != _explodeDirectionRandomize)
				{
					_explodeDirectionRandomize = value;
				}
			}
		}

		public float gravity
		{
			get
			{
				return _gravity;
			}
			set
			{
				if (value != _gravity)
				{
					_gravity = value;
				}
			}
		}

		public float upPowerPlus
		{
			get
			{
				return _upPowerPlus;
			}
			set
			{
				if (value != _upPowerPlus)
				{
					_upPowerPlus = value;
				}
			}
		}

		public bool isStartLoop
		{
			get
			{
				return _startLoop;
			}
			set
			{
				if (value != _startLoop)
				{
					_startLoop = value;
				}
			}
		}

		public bool isPlaying => _isAnimating;

		private void Start()
		{
			SetupFragment();
			if (_startLoop)
			{
				PlayAnimation().DelayFrame(1).Do(delegate
				{
					ResetFragment();
				}).Repeat()
					.Subscribe(delegate
					{
					});
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref _anglarSpeed);
			Mem.Del(ref _explodePower);
			Mem.Del(ref _explodeDirectionRandomize);
			Mem.Del(ref _gravity);
			Mem.Del(ref _upPowerPlus);
			Mem.Del(ref _startLoop);
			if (_fragments != null)
			{
				_fragments.ForEach(delegate(Fragment x)
				{
					x.Dispose();
				});
			}
			Mem.DelListSafe(ref _fragments);
			Mem.Del(ref _isAnimating);
		}

		private void SetupFragment()
		{
			Transform transform = base.transform;
			if (_fragments.Count != transform.childCount)
			{
				_fragments.Capacity = transform.childCount;
				for (int i = 0; i < transform.childCount; i++)
				{
					Fragment item = default(Fragment);
					item.transform = transform.GetChild(i);
					_fragments.Add(item);
				}
			}
			for (int j = 0; j < _fragments.Count; j++)
			{
				Fragment value = _fragments[j];
				value.initialPosition = value.transform.localPosition;
				value.initialRotation = value.transform.localRotation;
				Vector3 a = Quaternion.AngleAxis(UnityEngine.Random.Range(0f - _explodeDirectionRandomize, _explodeDirectionRandomize), UnityEngine.Random.onUnitSphere) * value.initialPosition.normalized;
				if (a.y < 0f)
				{
					a.y *= -1f;
				}
				a.y *= _upPowerPlus;
				value.velocity = a * _explodePower * UnityEngine.Random.Range(0.5f, 1.5f);
				value.angularVelocity = Quaternion.AngleAxis(UnityEngine.Random.Range(90f, 180f), UnityEngine.Random.onUnitSphere);
				_fragments[j] = value;
			}
		}

		public void ResetFragment()
		{
			for (int i = 0; i < _fragments.Count; i++)
			{
				Fragment fragment = _fragments[i];
				fragment.transform.localPosition = fragment.initialPosition;
				fragment.transform.localRotation = fragment.initialRotation;
			}
		}

		public bool LateRun()
		{
			if (!_isAnimating)
			{
				return false;
			}
			for (int i = 0; i < _fragments.Count; i++)
			{
				Fragment value = _fragments[i];
				Transform transform = value.transform;
				Vector3 localPosition = transform.transform.localPosition;
				Fragment fragment = _fragments[i];
				Quaternion localRotation = fragment.transform.localRotation;
				Vector3 a = localPosition;
				Fragment fragment2 = _fragments[i];
				localPosition = a + fragment2.velocity * Time.deltaTime;
				Quaternion lhs = localRotation;
				Quaternion identity = Quaternion.identity;
				Fragment fragment3 = _fragments[i];
				localRotation = lhs * Quaternion.Slerp(identity, fragment3.angularVelocity, Time.deltaTime * _anglarSpeed);
				transform.localPosition = localPosition;
				transform.localRotation = localRotation;
				value.velocity += Vector3.down * _gravity * Time.deltaTime;
				_fragments[i] = value;
			}
			return true;
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
			SetupFragment();
			yield return new WaitForSeconds(2f);
			_isAnimating = false;
			observer.OnNext(0);
			observer.OnCompleted();
		}
	}
}
