using UnityEngine;
using UnityEngine.EventSystems;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableEventTrigger : ObservableTriggerBase, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler
	{
		private Subject<BaseEventData> onDeselect;

		private Subject<AxisEventData> onMove;

		private Subject<PointerEventData> onPointerDown;

		private Subject<PointerEventData> onPointerEnter;

		private Subject<PointerEventData> onPointerExit;

		private Subject<PointerEventData> onPointerUp;

		private Subject<BaseEventData> onSelect;

		private Subject<PointerEventData> onPointerClick;

		private Subject<BaseEventData> onSubmit;

		private Subject<PointerEventData> onDrag;

		private Subject<PointerEventData> onBeginDrag;

		private Subject<PointerEventData> onEndDrag;

		private Subject<PointerEventData> onDrop;

		private Subject<BaseEventData> onUpdateSelected;

		private Subject<PointerEventData> onInitializePotentialDrag;

		private Subject<BaseEventData> onCancel;

		private Subject<PointerEventData> onScroll;

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			if (onDeselect != null)
			{
				onDeselect.OnNext(eventData);
			}
		}

		void IMoveHandler.OnMove(AxisEventData eventData)
		{
			if (onMove != null)
			{
				onMove.OnNext(eventData);
			}
		}

		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (onPointerDown != null)
			{
				onPointerDown.OnNext(eventData);
			}
		}

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			if (onPointerEnter != null)
			{
				onPointerEnter.OnNext(eventData);
			}
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			if (onPointerExit != null)
			{
				onPointerExit.OnNext(eventData);
			}
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (onPointerUp != null)
			{
				onPointerUp.OnNext(eventData);
			}
		}

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			if (onSelect != null)
			{
				onSelect.OnNext(eventData);
			}
		}

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			if (onPointerClick != null)
			{
				onPointerClick.OnNext(eventData);
			}
		}

		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			if (onSubmit != null)
			{
				onSubmit.OnNext(eventData);
			}
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (onDrag != null)
			{
				onDrag.OnNext(eventData);
			}
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			if (onBeginDrag != null)
			{
				onBeginDrag.OnNext(eventData);
			}
		}

		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (onEndDrag != null)
			{
				onEndDrag.OnNext(eventData);
			}
		}

		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			if (onDrop != null)
			{
				onDrop.OnNext(eventData);
			}
		}

		void IUpdateSelectedHandler.OnUpdateSelected(BaseEventData eventData)
		{
			if (onUpdateSelected != null)
			{
				onUpdateSelected.OnNext(eventData);
			}
		}

		void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
		{
			if (onInitializePotentialDrag != null)
			{
				onInitializePotentialDrag.OnNext(eventData);
			}
		}

		void ICancelHandler.OnCancel(BaseEventData eventData)
		{
			if (onCancel != null)
			{
				onCancel.OnNext(eventData);
			}
		}

		void IScrollHandler.OnScroll(PointerEventData eventData)
		{
			if (onScroll != null)
			{
				onScroll.OnNext(eventData);
			}
		}

		public IObservable<BaseEventData> OnDeselectAsObservable()
		{
			return onDeselect ?? (onDeselect = new Subject<BaseEventData>());
		}

		public IObservable<AxisEventData> OnMoveAsObservable()
		{
			return onMove ?? (onMove = new Subject<AxisEventData>());
		}

		public IObservable<PointerEventData> OnPointerDownAsObservable()
		{
			return onPointerDown ?? (onPointerDown = new Subject<PointerEventData>());
		}

		public IObservable<PointerEventData> OnPointerEnterAsObservable()
		{
			return onPointerEnter ?? (onPointerEnter = new Subject<PointerEventData>());
		}

		public IObservable<PointerEventData> OnPointerExitAsObservable()
		{
			return onPointerExit ?? (onPointerExit = new Subject<PointerEventData>());
		}

		public IObservable<PointerEventData> OnPointerUpAsObservable()
		{
			return onPointerUp ?? (onPointerUp = new Subject<PointerEventData>());
		}

		public IObservable<BaseEventData> OnSelectAsObservable()
		{
			return onSelect ?? (onSelect = new Subject<BaseEventData>());
		}

		public IObservable<PointerEventData> OnPointerClickAsObservable()
		{
			return onPointerClick ?? (onPointerClick = new Subject<PointerEventData>());
		}

		public IObservable<BaseEventData> OnSubmitAsObservable()
		{
			return onSubmit ?? (onSubmit = new Subject<BaseEventData>());
		}

		public IObservable<PointerEventData> OnDragAsObservable()
		{
			return onDrag ?? (onDrag = new Subject<PointerEventData>());
		}

		public IObservable<PointerEventData> OnBeginDragAsObservable()
		{
			return onBeginDrag ?? (onBeginDrag = new Subject<PointerEventData>());
		}

		public IObservable<PointerEventData> OnEndDragAsObservable()
		{
			return onEndDrag ?? (onEndDrag = new Subject<PointerEventData>());
		}

		public IObservable<PointerEventData> OnDropAsObservable()
		{
			return onDrop ?? (onDrop = new Subject<PointerEventData>());
		}

		public IObservable<BaseEventData> OnUpdateSelectedAsObservable()
		{
			return onUpdateSelected ?? (onUpdateSelected = new Subject<BaseEventData>());
		}

		public IObservable<PointerEventData> OnInitializePotentialDragAsObservable()
		{
			return onInitializePotentialDrag ?? (onInitializePotentialDrag = new Subject<PointerEventData>());
		}

		public IObservable<BaseEventData> OnCancelAsObservable()
		{
			return onCancel ?? (onCancel = new Subject<BaseEventData>());
		}

		public IObservable<PointerEventData> OnScrollAsObservable()
		{
			return onScroll ?? (onScroll = new Subject<PointerEventData>());
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			if (onDeselect != null)
			{
				onDeselect.OnCompleted();
			}
			if (onMove != null)
			{
				onMove.OnCompleted();
			}
			if (onPointerDown != null)
			{
				onPointerDown.OnCompleted();
			}
			if (onPointerEnter != null)
			{
				onPointerEnter.OnCompleted();
			}
			if (onPointerExit != null)
			{
				onPointerExit.OnCompleted();
			}
			if (onPointerUp != null)
			{
				onPointerUp.OnCompleted();
			}
			if (onSelect != null)
			{
				onSelect.OnCompleted();
			}
			if (onPointerClick != null)
			{
				onPointerClick.OnCompleted();
			}
			if (onSubmit != null)
			{
				onSubmit.OnCompleted();
			}
			if (onDrag != null)
			{
				onDrag.OnCompleted();
			}
			if (onBeginDrag != null)
			{
				onBeginDrag.OnCompleted();
			}
			if (onEndDrag != null)
			{
				onEndDrag.OnCompleted();
			}
			if (onDrop != null)
			{
				onDrop.OnCompleted();
			}
			if (onUpdateSelected != null)
			{
				onUpdateSelected.OnCompleted();
			}
			if (onInitializePotentialDrag != null)
			{
				onInitializePotentialDrag.OnCompleted();
			}
			if (onCancel != null)
			{
				onCancel.OnCompleted();
			}
			if (onScroll != null)
			{
				onScroll.OnCompleted();
			}
		}
	}
}
