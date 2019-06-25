using System.Collections;
using UnityEngine;

namespace KCV.View.Scroll
{
	[RequireComponent(typeof(BoxCollider2D))]
	public abstract class UIScrollListChildNew<MODEL, VIEW> : UIToggle where MODEL : class where VIEW : UIScrollListChildNew<MODEL, VIEW>
	{
		[SerializeField]
		private UIWidget background;

		protected UIScrollListParentNew<MODEL, VIEW> parent;

		private Coroutine initCoroutine;

		public MODEL model
		{
			get;
			private set;
		}

		public int modelIndex
		{
			get;
			private set;
		}

		public bool visible
		{
			get;
			private set;
		}

		protected void Awake()
		{
			EventDelegate eventDelegate = new EventDelegate();
			eventDelegate.target = this;
			eventDelegate.methodName = "UpdateHover";
			onChange.Clear();
			onChange.Add(eventDelegate);
		}

		public void Init(UIScrollListParentNew<MODEL, VIEW> parent, MODEL model, int modelIndex)
		{
			this.parent = parent;
			this.model = model;
			this.modelIndex = modelIndex;
			if (model != null)
			{
				if (initCoroutine != null)
				{
					StopCoroutine(initCoroutine);
					initCoroutine = null;
				}
				initCoroutine = StartCoroutine(InitializeCoroutine(model));
			}
		}

		protected virtual IEnumerator InitializeCoroutine(MODEL model)
		{
			yield return null;
		}

		public virtual void Show()
		{
			visible = true;
			for (int i = 0; i < base.gameObject.transform.childCount; i++)
			{
				base.gameObject.transform.GetChild(i).SetActive(isActive: true);
			}
		}

		public virtual void Hide()
		{
			visible = false;
			for (int i = 0; i < base.gameObject.transform.childCount; i++)
			{
				base.gameObject.transform.GetChild(i).SetActive(isActive: false);
			}
		}

		public void UpdateLocalPosition(float x, float y, float z)
		{
			base.transform.localPosition = new Vector3(x, y, z);
		}

		public void DoSelect()
		{
			Set(state: true);
		}

		public void OnClick()
		{
			if (visible)
			{
				Set(state: true);
				parent.OnChildSelect(this);
			}
		}

		public void UpdateHover()
		{
			UISelectedObject.SelectedOneObjectBlink(background.gameObject, base.value);
		}

		public void HideHover()
		{
			UISelectedObject.SelectedOneObjectBlink(background.gameObject, value: false);
		}

		protected virtual void OnCallDestroy()
		{
		}

		private void OnDestroy()
		{
			OnCallDestroy();
			if (background != null)
			{
				background.RemoveFromPanel();
			}
			background = null;
			parent = null;
			if (initCoroutine != null)
			{
				StopCoroutine(initCoroutine);
			}
			initCoroutine = null;
			model = (MODEL)null;
		}
	}
}
