using DG.Tweening;
using UnityEngine;

namespace KCV.Furniture.JukeBox
{
	[RequireComponent(typeof(UIPanel))]
	public class UIJukeBoxMusicPlayingRollLabel : MonoBehaviour
	{
		[SerializeField]
		private UIPanel mPanelThis;

		[SerializeField]
		private UILabel mLabel_Title;

		private float mLeft;

		private float mRight;

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mLeft = 0f - mPanelThis.width / 2f;
			mRight = mPanelThis.width;
		}

		public void Initialize(string title)
		{
			mLabel_Title.text = title;
		}

		public void StartRoll()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
			Sequence sequence = DOTween.Sequence().SetId(this);
			mLabel_Title.transform.localPositionX(mRight + (float)(mLabel_Title.width / 2));
			Tween t = mLabel_Title.transform.DOLocalMoveX(mLabel_Title.width / 2, 3f);
			Tween t2 = mLabel_Title.transform.DOLocalMoveX(mLeft, 3f).OnComplete(delegate
			{
				mLabel_Title.transform.localPositionX(mRight + (float)(mLabel_Title.width / 2));
			}).SetEase(Ease.Linear);
			sequence.Append(t);
			sequence.AppendInterval(1.5f);
			sequence.Append(t2);
			sequence.AppendInterval(0.5f);
			sequence.SetLoops(int.MaxValue, LoopType.Restart);
		}

		public void StopRoll()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this);
			}
		}
	}
}
