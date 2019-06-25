using KCV.Scene.Port;
using UnityEngine;

namespace KCV.Remodel
{
	[RequireComponent(typeof(UITexture))]
	public class CategoryMenuItem : UIToggle
	{
		private const float notFocusedScale = 0.9f;

		private const float focusedScale = 1.1f;

		private const int focusedDepth = 50;

		[SerializeField]
		private Texture onTexture;

		[SerializeField]
		private Texture offTexture;

		private CategoryMenu parent;

		private UITexture texture;

		private int originDepth;

		public int index
		{
			get;
			private set;
		}

		public void Awake()
		{
			texture = ((Component)base.transform).GetComponent<UITexture>();
			originDepth = texture.depth;
		}

		public void Init(CategoryMenu parent, int index, bool enabled)
		{
			this.parent = parent;
			this.index = index;
			group = parent.group;
			base.enabled = enabled;
			onChange.Clear();
			EventDelegate.Add(onChange, OnValueChange);
			texture.mainTexture = ((!enabled) ? offTexture : onTexture);
			base.transform.localScaleX(0.9f);
			base.transform.localScaleY(0.9f);
		}

		public virtual void OnClick()
		{
			if (base.enabled)
			{
				parent.OnItemClick(this);
			}
		}

		public void OnValueChange()
		{
			if (base.value)
			{
				base.transform.localScaleX(1.1f);
				base.transform.localScaleY(1.1f);
				texture.depth = 50;
			}
			else
			{
				base.transform.localScaleX(0.9f);
				base.transform.localScaleY(0.9f);
				texture.depth = originDepth;
			}
		}

		internal void Release()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref onTexture);
			UserInterfacePortManager.ReleaseUtils.Release(ref offTexture);
			UserInterfacePortManager.ReleaseUtils.Release(ref texture);
			parent = null;
		}
	}
}
