using UnityEngine;

namespace KCV.View
{
	[RequireComponent(typeof(UIButton))]
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(UIPanel))]
	public class BaseUISummary<Model> : MonoBehaviour where Model : class
	{
		protected UIPanel mPanelThis;

		private UIButton mButtonThis;

		[SerializeField]
		private int mIndex;

		private Model mModel;

		public UIPanel GetPanel()
		{
			return mPanelThis;
		}

		private void Awake()
		{
			mPanelThis = GetComponent<UIPanel>();
			mButtonThis = GetComponent<UIButton>();
			mPanelThis.alpha = 0.01f;
		}

		public virtual void Initialize(int index, Model model)
		{
			mIndex = index;
			mModel = model;
		}

		public virtual void InitializeDefault(int index, Model model)
		{
			mIndex = index;
			mModel = model;
		}

		public virtual void Show()
		{
			mPanelThis.alpha = 1f;
		}

		public virtual void Hide()
		{
			mPanelThis.alpha = 0.01f;
		}

		public Model GetModel()
		{
			return mModel;
		}

		public int GetIndex()
		{
			return mIndex;
		}

		public virtual KeyControl Focus()
		{
			return null;
		}

		public virtual void RemoveFocus()
		{
		}

		public virtual void Hover()
		{
		}

		public virtual void RemoveHover()
		{
		}

		public virtual bool CanFocus()
		{
			return false;
		}

		public virtual void Clear()
		{
		}

		public void DepthFront()
		{
			mPanelThis.depth++;
		}

		public void SetDepth(int depth)
		{
			mPanelThis.depth = depth;
		}

		public void DepthBack()
		{
			mPanelThis.depth--;
		}
	}
}
