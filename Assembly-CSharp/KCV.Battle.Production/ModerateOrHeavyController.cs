using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode]
	public class ModerateOrHeavyController : MonoBehaviour
	{
		public enum Mode
		{
			Moderate,
			Heavy
		}

		[SerializeField]
		private Animation _animation;

		[SerializeField]
		private ExplodeChild _moderate;

		[SerializeField]
		private Transform _moderateFlat;

		[SerializeField]
		private ExplodeChild _heavy;

		[SerializeField]
		private Transform _heavyFlat;

		[SerializeField]
		[Tooltip("アニメーションクリップ用スケール")]
		private float _scale = 1f;

		[SerializeField]
		private float _defaultScale = 272f;

		[Tooltip("デバッグ用にループ起動するか")]
		[SerializeField]
		private bool _startLoop;

		private bool _isAnimating;

		private Mode _mode;

		private Subject<int> _shakeSubject = new Subject<int>();

		public UniRx.IObservable<int> ShakeObservable => _shakeSubject;

		public bool isPlaying => _isAnimating;

		private void Start()
		{            
			DebugUtils.dbgAssert((UnityEngine.Object) _animation != null);
			if (_startLoop)
			{
				PlayAnimation(Mode.Heavy).DelayFrame(10).Repeat().Subscribe(delegate
				{
				});
			}
		}

		private void OnDestroy()
		{
			Mem.DelMeshSafe(ref _heavyFlat);
			Mem.DelMeshSafe(ref _moderateFlat);
			Mem.Del(ref _animation);
			Mem.Del(ref _moderate);
			Mem.Del(ref _moderateFlat);
			Mem.Del(ref _heavy);
			Mem.Del(ref _heavyFlat);
			Mem.Del(ref _scale);
			Mem.Del(ref _defaultScale);
			Mem.Del(ref _startLoop);
			Mem.Del(ref _isAnimating);
			Mem.Del(ref _mode);
			if (_shakeSubject != null)
			{
				_shakeSubject.OnCompleted();
			}
			Mem.Del(ref _shakeSubject);
		}

		public bool LateRun()
		{
			if (_isAnimating)
			{
				base.transform.localScale = Vector3.one * _scale;
			}
			if (_mode == Mode.Moderate)
			{
				_moderate.LateRun();
			}
			else
			{
				_heavy.LateRun();
			}
			return true;
		}

		private void ShakeEvent()
		{
			_shakeSubject.OnNext(0);
		}

		public UniRx.IObservable<int> PlayAnimation(Mode mode)
		{
			_mode = mode;
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
			Mode mode = _mode;
			Transform flat = (mode != 0) ? _heavyFlat : _moderateFlat;
			ExplodeChild explode = (mode != 0) ? _heavy : _moderate;
			flat.localScale = Vector3.one * _defaultScale;
			string animationClip = (mode != 0) ? "ModerateOrHeavy_Heavy" : "ModerateOrHeavy";
			_animation.Play(animationClip);

            var clip = _animation.GetClip(animationClip);
            clip.SampleAnimation(base.gameObject, 0f);

            throw new NotImplementedException("なにこれ");
            //yield return new WaitForSeconds(_animation.get_Item(animationClip).length);
            yield return new WaitForSeconds(0);

            flat.localScale = Vector3.zero;
			explode.transform.localScale = Vector3.one * _defaultScale;
			yield return explode.PlayAnimation().StartAsCoroutine();
			explode.transform.localScale = Vector3.zero;
			explode.ResetFragment();
			_isAnimating = false;
			observer.OnNext(0);
			observer.OnCompleted();
		}
	}
}
