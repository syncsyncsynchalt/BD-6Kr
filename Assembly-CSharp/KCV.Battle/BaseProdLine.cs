using System;
using UnityEngine;

namespace KCV.Battle
{
	public class BaseProdLine : MonoBehaviour
	{
		public enum AnimationName
		{
			ProdLine,
			ProdTripleLine,
			ProdSuccessiveLine,
			ProdNormalAttackLine,
			ProdAircraftAttackLine
		}

		protected Action _actCallback;

		protected bool _isFinished;

		protected UITexture _uiOverlay;

		protected virtual void OnDestroy()
		{
			Mem.Del(ref _actCallback);
			Mem.Del(ref _isFinished);
			Mem.Del(ref _uiOverlay);
		}

		public virtual void Play(Action callback)
		{
			_isFinished = false;
			_actCallback = callback;
			GetComponent<Animation>().Play(AnimationName.ProdLine.ToString());
		}

		public virtual void Play(AnimationName iName, Action callback)
		{
			_isFinished = false;
			_actCallback = callback;
			GetComponent<Animation>().Play(iName.ToString());
		}

		protected virtual void onFinished()
		{
			_isFinished = true;
			if (_actCallback != null)
			{
				_actCallback();
			}
		}
	}
}
