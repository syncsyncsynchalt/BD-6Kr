using KCV.Utils;
using System;
using UniRx;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(Animation))]
	public class BaseAnimation : MonoBehaviour
	{
		protected Animation _animAnimation;

		protected bool _isFinished;

		protected bool _isForceFinished;

		protected Action _actForceCallback;

		protected Action _actCallback;

		public new Animation animation
		{
			get
			{
				if ((UnityEngine.Object)_animAnimation == null)
				{
					GetComponent<Animation>();
				}
				return _animAnimation;
			}
		}

		public bool isFinished => _isFinished;

		public bool isForceFinished => _isForceFinished;

		public virtual bool isPlaying => _animAnimation.isPlaying;

		protected virtual void Awake()
		{
			_animAnimation = ((Component)this).SafeGetComponent<Animation>();
			Init();
		}

		protected virtual void OnDestroy()
		{
			UnInit();
			_animAnimation = null;
		}

		public virtual bool Init()
		{
			_isFinished = false;
			_isForceFinished = false;
			_actCallback = null;
			_actForceCallback = null;
			return true;
		}

		public virtual bool UnInit()
		{
			_isFinished = false;
			_isForceFinished = false;
			_actForceCallback = null;
			_actCallback = null;
			return true;
		}

		public virtual void Play()
		{
			_animAnimation.Play();
		}

		public virtual void Play(Action callback)
		{
			Init();
			_actCallback = callback;
			_animAnimation.Play();
		}

		public virtual void Play(Enum iEnum, Action callback)
		{
			Init();
			_actCallback = callback;
			_animAnimation.Play(iEnum.ToString());
		}

		public virtual void Play(Action forceCallback, Action callback)
		{
			Init();
			_actCallback = callback;
			_actForceCallback = forceCallback;
			_animAnimation.Play();
		}

		public virtual void Play(Enum iEnum, Action forceCallback, Action callback)
		{
			Init();
			_actCallback = callback;
			_actForceCallback = forceCallback;
			_animAnimation.Play(iEnum.ToString());
		}

		public virtual void Discard()
		{
			if (base.gameObject != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		protected virtual void onAnimationFinished()
		{
			_isFinished = true;
			if (_actCallback != null)
			{
				_actCallback();
			}
		}

		protected virtual void onAnimationFinishedAfterDiscard()
		{
			onAnimationFinished();
			Observable.Timer(TimeSpan.FromSeconds(0.15000000596046448)).Subscribe(delegate
			{
				base.gameObject.Discard();
			});
		}

		protected virtual void OnForceAnimationFinished()
		{
			_isForceFinished = true;
			if (_actForceCallback != null)
			{
				_actForceCallback();
			}
		}

		protected virtual void _playSE(SEFIleInfos info)
		{
			SoundUtils.PlaySE(info);
		}

		protected virtual Texture2D _loadResources(string fileName)
		{
			return Resources.Load($"{fileName}") as Texture2D;
		}

		[Obsolete("", false)]
		protected virtual void OnAnimationFinished()
		{
			_isFinished = true;
			if (_actCallback != null)
			{
				_actCallback();
			}
			this.DelayAction(0.15f, delegate
			{
				base.gameObject.Discard();
			});
		}
	}
}
