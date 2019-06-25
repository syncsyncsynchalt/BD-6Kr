using local.models;
using System;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(UIWidget))]
	public class UIHowTo : MonoBehaviour
	{
		[SerializeField]
		private UISprite container;

		[SerializeField]
		private Vector3 showPos;

		[SerializeField]
		private Vector3 hidePos;

		[SerializeField]
		private UIHowToItem itemPrefab;

		[SerializeField]
		private float animationDuration = 0.2f;

		[SerializeField]
		private float horizontalStartMargin = 30f;

		[SerializeField]
		private float horizontalItemMargin = 30f;

		[SerializeField]
		private UIHowToHorizontalAlign horizontalAlign;

		private SettingModel _model;

		private void Start()
		{
			_model = new SettingModel();
			Hide(animation: false);
		}

		private void OnDestroy()
		{
			Mem.Del(ref container);
			Mem.Del(ref itemPrefab);
			Mem.Del(ref _model);
		}

		public void Refresh(params UIHowToDetail[] details)
		{
			float horizontalDirection = (horizontalAlign == UIHowToHorizontalAlign.left) ? 1 : (-1);
			float x = horizontalStartMargin * horizontalDirection;
			int childCount = base.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				NGUITools.Destroy(base.transform.GetChild(0));
			}
			int childDepth = GetComponent<UIWidget>().depth + 1;
			GameObject parent = base.gameObject;
			details.ForEach(delegate(UIHowToDetail e)
			{
				UIHowToItem component = Util.Instantiate(itemPrefab.gameObject, parent).GetComponent<UIHowToItem>();
				component.Init(Enum.GetName(typeof(HowToKey), e.key), e.label, childDepth);
				component.transform.localPositionY(0f);
				switch (horizontalAlign)
				{
				case UIHowToHorizontalAlign.left:
					component.transform.localPositionX(x);
					break;
				case UIHowToHorizontalAlign.right:
					component.transform.localPositionX(x - (float)component.GetWidth());
					break;
				}
				x += ((float)component.GetWidth() + horizontalItemMargin) * horizontalDirection;
			});
		}

		public void Show()
		{
			Show(animation: true);
		}

		public void Show(bool animation)
		{
			if (_model.GuideDisplay)
			{
				if (animation)
				{
					Move(showPos);
				}
				else
				{
					base.transform.localPosition = showPos;
				}
			}
		}

		public void Hide()
		{
			Hide(animation: true);
		}

		public void Hide(bool animation)
		{
			if (animation)
			{
				Move(hidePos);
			}
			else
			{
				base.transform.localPosition = hidePos;
			}
		}

		private void Move(Vector3 to)
		{
			TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(base.gameObject, animationDuration);
			tweenPosition.from = base.gameObject.transform.localPosition;
			tweenPosition.to = to;
			tweenPosition.PlayForward();
		}

		public void SetHorizontalAlign(UIHowToHorizontalAlign iHorizontalAlign)
		{
			horizontalAlign = iHorizontalAlign;
		}
	}
}
