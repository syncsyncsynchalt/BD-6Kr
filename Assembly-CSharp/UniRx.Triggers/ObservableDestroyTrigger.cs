using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableDestroyTrigger : MonoBehaviour
	{
		private bool calledDestroy;

		private Subject<Unit> onDestroy;

		private void OnDestroy()
		{
			calledDestroy = true;
			if (onDestroy != null)
			{
				onDestroy.OnNext(Unit.Default);
				onDestroy.OnCompleted();
			}
		}

		public IObservable<Unit> OnDestroyAsObservable()
		{
			if (this == null)
			{
				return Observable.Return(Unit.Default);
			}
			if (calledDestroy)
			{
				return Observable.Return(Unit.Default);
			}
			return onDestroy ?? (onDestroy = new Subject<Unit>());
		}
	}
}
