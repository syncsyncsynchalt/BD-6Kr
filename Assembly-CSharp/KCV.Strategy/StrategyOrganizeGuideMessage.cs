using local.managers;
using local.models;
using System.Linq;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyOrganizeGuideMessage : MonoBehaviour
	{
		public bool isVisible
		{
			get;
			private set;
		}

		private void Start()
		{
			GetComponent<UIWidget>().alpha = 0f;
			isVisible = true;
			setVisible(isVisible: false);
		}

		public void UpdateVisible()
		{
			StrategyMapManager logicManager = StrategyTopTaskManager.GetLogicManager();
			if (logicManager.Area[1].GetDecks().Any((DeckModel x) => x.GetFlagShip() == null))
			{
				setVisible(isVisible: true);
			}
			else
			{
				setVisible(isVisible: false);
			}
		}

		public void setVisible(bool isVisible)
		{
			if (this.isVisible != isVisible)
			{
				this.isVisible = isVisible;
				if (isVisible)
				{
					TweenAlpha ta = GetComponent<TweenAlpha>();
					ta.onFinished.Clear();
					ta.to = 1f;
					ta.from = 0f;
					ta.duration = 1f;
					ta.style = UITweener.Style.Once;
					ta.SetOnFinished(delegate
					{
						ta.onFinished.Clear();
						ta.from = 1f;
						ta.to = 0.6f;
						ta.style = UITweener.Style.PingPong;
						ta.duration = 2f;
						ta.ResetToBeginning();
						this.DelayActionFrame(1, delegate
						{
							ta.PlayForward();
						});
					});
					ta.ResetToBeginning();
					this.DelayActionFrame(1, delegate
					{
						ta.PlayForward();
					});
				}
				else
				{
					TweenAlpha ta2 = GetComponent<TweenAlpha>();
					ta2.onFinished.Clear();
					ta2.duration = 0.3f;
					ta2.from = GetComponent<UIWidget>().alpha;
					ta2.to = 0f;
					ta2.style = UITweener.Style.Once;
					ta2.ResetToBeginning();
					ta2.SetOnFinished(delegate
					{
					});
					this.DelayActionFrame(1, delegate
					{
						ta2.PlayForward();
					});
				}
			}
		}
	}
}
