using UnityEngine;

namespace KCV
{
	public class UIShortCutTruss : MonoBehaviour
	{
		private TweenPosition tp;

		[SerializeField]
		private UIShortCutCrane Crane;

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
				Crane.StartAnimationNoReset();
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
					if (!isEnter)
					{
						Crane.AnimStop();
					}
				});
			}
		}
	}
}
