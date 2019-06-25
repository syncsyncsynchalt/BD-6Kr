using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.SortieMap
{
	public class ProdShipRipple : MonoBehaviour
	{
		[SerializeField]
		private UISprite Hammon1;

		[SerializeField]
		private UISprite Hammon2;

		private Vector3 MaxHammonScale;

		private List<Action> setEndAnimation;

		public float duration;

		public float SecondHammonDelay;

		private void Awake()
		{
			setEndAnimation = new List<Action>();
			MaxHammonScale = new Vector3(7f, 7f, 1f);
		}

		public void Play(Color color)
		{
			Hammon1.color = color;
			Hammon2.color = color;
			StartHamonnEffect(Hammon1.gameObject, 0f);
			StartHamonnEffect(Hammon2.gameObject, SecondHammonDelay);
		}

		private void StartHamonnEffect(GameObject go, float delay)
		{
			TweenScale ts = TweenScale.Begin(go, duration, MaxHammonScale);
			TweenAlpha ta = TweenAlpha.Begin(go, duration, 0f);
			ts.style = UITweener.Style.Loop;
			ta.style = UITweener.Style.Loop;
			ts.delay = delay;
			ta.delay = delay;
			setEndAnimation.Add(delegate
			{
				ts.style = UITweener.Style.Once;
				ta.style = UITweener.Style.Once;
			});
		}

		public void Stop()
		{
			foreach (Action item in setEndAnimation)
			{
				item();
			}
		}
	}
}
