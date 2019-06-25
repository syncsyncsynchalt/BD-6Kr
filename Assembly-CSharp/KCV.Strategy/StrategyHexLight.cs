using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyHexLight : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _uiLightPar;

		[SerializeField]
		private Animation _anim;

		private Action _callback;

		private void OnDestroy()
		{
			_callback = null;
			if ((UnityEngine.Object)_uiLightPar != null)
			{
				_uiLightPar.Stop();
			}
			_uiLightPar = null;
			if ((UnityEngine.Object)_anim != null)
			{
				_anim.Stop();
			}
			_anim = null;
		}

		public void Play(Action callback)
		{
			Util.FindParentToChild<ParticleSystem>(ref _uiLightPar, base.transform, "Particle");
			if ((UnityEngine.Object)_anim == null)
			{
				_anim = GetComponent<Animation>();
			}
			_callback = null;
			_anim.Stop();
			((Component)_uiLightPar).SetActive(isActive: false);
			_callback = callback;
			_anim.Stop();
			_anim.Play();
			if ((UnityEngine.Object)_uiLightPar != null)
			{
				((Component)_uiLightPar).SetActive(isActive: true);
				_uiLightPar.Play();
			}
		}

		private void stopParticle()
		{
			if (!((UnityEngine.Object)_uiLightPar == null))
			{
				_uiLightPar.Stop();
			}
		}

		private void onAnimationFinished()
		{
			if (_callback != null)
			{
				_callback();
			}
			if ((UnityEngine.Object)_uiLightPar != null)
			{
				((Component)_uiLightPar).SetActive(isActive: false);
			}
		}

		public static StrategyHexLight Instantiate(StrategyHexLight prefab, Transform fromParent)
		{
			StrategyHexLight strategyHexLight = UnityEngine.Object.Instantiate(prefab);
			strategyHexLight.transform.parent = fromParent;
			strategyHexLight.transform.localScale = Vector3.one;
			strategyHexLight.transform.localPosition = Vector3.zero;
			return strategyHexLight;
		}
	}
}
