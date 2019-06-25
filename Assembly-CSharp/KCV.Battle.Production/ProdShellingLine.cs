using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(UIPanel))]
	public class ProdShellingLine : BaseAnimation
	{
		public enum AnimationList
		{
			ProdNormalShellingFriendLine,
			ProdNormalShellingEnemyLine
		}

		[SerializeField]
		private List<UITexture> _uiOverlayLines;

		[Range(0f, 1f)]
		[SerializeField]
		private float _fFillAmount = 1f;

		private UIPanel _uiPanel;

		public UIPanel panel
		{
			get
			{
				if (_uiPanel == null)
				{
					_uiPanel = GetComponent<UIPanel>();
				}
				return _uiPanel;
			}
		}

		public float fillAmount
		{
			get
			{
				return _fFillAmount;
			}
			set
			{
				if (_fFillAmount != value)
				{
					_fFillAmount = Mathe.MinMax2F01(value);
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			panel.widgetsAreStatic = true;
		}

		private void LateUpdate()
		{
			if (base.animation.isPlaying)
			{
				_uiOverlayLines.ForEach(delegate(UITexture x)
				{
					x.fillAmount = fillAmount;
				});
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.DelList(ref _uiOverlayLines);
			Mem.Del(ref _fFillAmount);
			Mem.Del(ref _uiPanel);
		}

		public void Play(AnimationList iList)
		{
			panel.widgetsAreStatic = false;
			base.transform.localScaleOne();
			base.Play(iList, null);
		}

		public void Play(bool isFriend)
		{
			UITexture component = ((Component)base.transform.FindChild("Anchor/OverlayA")).GetComponent<UITexture>();
			UITexture component2 = ((Component)base.transform.FindChild("Anchor/OverlayC")).GetComponent<UITexture>();
			Color color3 = component2.color = (component.color = ((!isFriend) ? new Color(1f, 0f, 0f, component.alpha) : new Color(0f, 51f / 160f, 1f, component.alpha)));
			Play((!isFriend) ? AnimationList.ProdNormalShellingEnemyLine : AnimationList.ProdNormalShellingFriendLine);
		}

		protected override void onAnimationFinished()
		{
			panel.widgetsAreStatic = true;
			base.transform.localScaleZero();
		}
	}
}
