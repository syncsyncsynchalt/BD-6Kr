using UnityEngine;

namespace KCV
{
	public class UIShortCutGears : MonoBehaviour
	{
		private TweenPosition tp;

		[SerializeField]
		private TweenRotation[] tweenRots;

		private bool isEnter;

		private void Awake()
		{
			tp = GetComponent<TweenPosition>();
		}

		public void Enter()
		{
			if (!isEnter)
			{
				isEnter = true;
				tp.onFinished.Clear();
				tp.PlayForward();
				TweenRotation[] array = tweenRots;
				foreach (TweenRotation tweenRotation in array)
				{
					tweenRotation.PlayForward();
				}
			}
		}

		public void Exit()
		{
			if (isEnter)
			{
				isEnter = false;
				tp.PlayReverse();
				tp.SetOnFinished(delegate
				{
					TweenRotation[] array = tweenRots;
					foreach (TweenRotation tweenRotation in array)
					{
						tweenRotation.enabled = false;
					}
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.isCloseAnimNow = false;
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.SetActiveChildren(isActive: false);
				});
			}
		}
	}
}
