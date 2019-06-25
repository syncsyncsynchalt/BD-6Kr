using KCV.Utils;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(Animation))]
	public class UILevelUpIcon : BaseAnimation
	{
		[SerializeField]
		private UISprite _uiLevelUpIcon;

		public float alpha
		{
			get
			{
				return _uiLevelUpIcon.alpha;
			}
			set
			{
				_uiLevelUpIcon.alpha = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			if (_uiLevelUpIcon == null)
			{
				Util.FindParentToChild(ref _uiLevelUpIcon, base.transform, "Icon");
			}
			base.transform.localScaleZero();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			_uiLevelUpIcon = null;
		}

		public override void Play()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_058);
			base.transform.localScaleOne();
			base.Play();
		}

		protected override void onAnimationFinished()
		{
			base.transform.localScaleZero();
			base.onAnimationFinished();
		}
	}
}
