using UnityEngine;

namespace KCV.Strategy
{
	public class TileAnimationHukidashi : MonoBehaviour
	{
		public enum Type
		{
			Damage,
			Goutin,
			Damecon,
			TankerLost
		}

		[SerializeField]
		private UISprite hukidashi;

		private TweenPosition TwPos;

		private TweenAlpha TwAlpha;

		private readonly Vector3 TankerLostPos = new Vector3(0f, 30f, 0f);

		private readonly Vector3 DamagePos = new Vector3(-30f, 0f, 0f);

		private UILabel BreakNum;

		private UIWidget Wiget;

		private void Awake()
		{
			TwPos = GetComponent<TweenPosition>();
			TwAlpha = GetComponent<TweenAlpha>();
			Wiget = GetComponent<UIWidget>();
			BreakNum = ((Component)base.transform.GetChild(0)).GetComponent<UILabel>();
		}

		public void Init()
		{
			Wiget.alpha = 0f;
		}

		public void Play(Type type)
		{
			hukidashi.spriteName = "fuki_" + (int)(type + 1);
			hukidashi.MakePixelPerfect();
			BreakNum.text = string.Empty;
			Show(type);
		}

		public void Play(int breakNum)
		{
			hukidashi.spriteName = "fuki_4";
			hukidashi.MakePixelPerfect();
			BreakNum.textInt = breakNum;
			Show(Type.TankerLost);
		}

		private void Show(Type type)
		{
			TwPos.ResetToBeginning();
			TwAlpha.ResetToBeginning();
			if (type == Type.TankerLost)
			{
				TwPos.from = TankerLostPos;
			}
			else
			{
				TwPos.from = DamagePos;
			}
			TwPos.PlayForward();
			TwAlpha.PlayForward();
		}

		private void OnDestroy()
		{
			hukidashi = null;
			TwPos = null;
			TwAlpha = null;
			BreakNum = null;
		}
	}
}
