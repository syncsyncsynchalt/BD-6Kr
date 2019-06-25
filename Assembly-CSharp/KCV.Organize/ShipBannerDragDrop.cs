using System;
using UnityEngine;

namespace KCV.Organize
{
	public class ShipBannerDragDrop : UIDragDropItem
	{
		public delegate bool OnDragEndCallBackDele(int index);

		[SerializeField]
		private UIPanel BannerPanel;

		private int OriginalDepth;

		private Vector3 OriginalPosition;

		[SerializeField]
		private OrganizeBannerManager BannerManager;

		private Action<int> OnDragStartCallBack;

		private OnDragEndCallBackDele OnDragEndCallBack;

		private Action<int> OnDragAnimationEndCallBack;

		public BoxCollider2D col
		{
			get;
			private set;
		}

		private void Awake()
		{
			col = GetComponent<BoxCollider2D>();
			base.enabled = false;
			BannerPanel = ((Component)base.transform.FindChild("ShipFrame")).GetComponent<UIPanel>();
			BannerManager = GetComponent<OrganizeBannerManager>();
		}

		protected override void OnDragStart()
		{
		}

		protected override void OnDragEnd()
		{
			if (OnDragEndCallBack(BannerManager.number))
			{
				Util.MoveTo(base.gameObject, 0.3f, OriginalPosition, iTween.EaseType.easeOutQuint);
				this.DelayAction(0.3f, delegate
				{
					BannerPanel.depth = OriginalDepth;
					base.OnDragEnd();
					OnDragAnimationEndCallBack(BannerManager.number);
				});
				BannerManager.UpdateBanner(enabled: true);
			}
		}

		public void setDefaultPosition(Vector2 pos)
		{
			OriginalPosition = pos;
		}

		public void setOnDragStartCallBack(Action<int> CallBack)
		{
			OnDragStartCallBack = CallBack;
		}

		public void setOnDragEndCallBack(OnDragEndCallBackDele CallBack)
		{
			OnDragEndCallBack = CallBack;
		}

		public void setOnDragAnimationEndCallBack(Action<int> CallBack)
		{
			OnDragAnimationEndCallBack = CallBack;
		}

		public void setColliderEnable(bool isEnable)
		{
			col.enabled = isEnable;
		}

		private void OnDestroy()
		{
			BannerManager = null;
			BannerPanel = null;
			col = null;
			OnDragStartCallBack = null;
			OnDragEndCallBack = null;
		}
	}
}
