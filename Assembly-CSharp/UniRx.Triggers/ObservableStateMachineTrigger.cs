using UnityEngine;

namespace UniRx.Triggers
{
	[DisallowMultipleComponent]
	public class ObservableStateMachineTrigger : StateMachineBehaviour
	{
		public class OnStateInfo
		{
			public Animator Animator
			{
				get;
				private set;
			}

			public AnimatorStateInfo StateInfo
			{
				get;
				private set;
			}

			public int LayerIndex
			{
				get;
				private set;
			}

			public OnStateInfo(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				Animator = animator;
				StateInfo = stateInfo;
				LayerIndex = layerIndex;
			}
		}

		public class OnStateMachineInfo
		{
			public Animator Animator
			{
				get;
				private set;
			}

			public int StateMachinePathHash
			{
				get;
				private set;
			}

			public OnStateMachineInfo(Animator animator, int stateMachinePathHash)
			{
				Animator = animator;
				StateMachinePathHash = stateMachinePathHash;
			}
		}

		private Subject<OnStateInfo> onStateExit;

		private Subject<OnStateInfo> onStateEnter;

		private Subject<OnStateInfo> onStateIK;

		private Subject<OnStateInfo> onStateMove;

		private Subject<OnStateInfo> onStateUpdate;

		private Subject<OnStateMachineInfo> onStateMachineEnter;

		private Subject<OnStateMachineInfo> onStateMachineExit;

		//public ObservableStateMachineTrigger()
		//: this()
		//{
		//}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (onStateExit != null)
			{
				onStateExit.OnNext(new OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<OnStateInfo> OnStateExitAsObservable()
		{
			return onStateExit ?? (onStateExit = new Subject<OnStateInfo>());
		}

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (onStateEnter != null)
			{
				onStateEnter.OnNext(new OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<OnStateInfo> OnStateEnterAsObservable()
		{
			return onStateEnter ?? (onStateEnter = new Subject<OnStateInfo>());
		}

		public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (onStateIK != null)
			{
				onStateIK.OnNext(new OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<OnStateInfo> OnStateIKAsObservable()
		{
			return onStateIK ?? (onStateIK = new Subject<OnStateInfo>());
		}

		public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (onStateMove != null)
			{
				onStateMove.OnNext(new OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<OnStateInfo> OnStateMoveAsObservable()
		{
			return onStateMove ?? (onStateMove = new Subject<OnStateInfo>());
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if (onStateUpdate != null)
			{
				onStateUpdate.OnNext(new OnStateInfo(animator, stateInfo, layerIndex));
			}
		}

		public IObservable<OnStateInfo> OnStateUpdateAsObservable()
		{
			return onStateUpdate ?? (onStateUpdate = new Subject<OnStateInfo>());
		}

		public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
		{
			if (onStateMachineEnter != null)
			{
				onStateMachineEnter.OnNext(new OnStateMachineInfo(animator, stateMachinePathHash));
			}
		}

		public IObservable<OnStateMachineInfo> OnStateMachineEnterAsObservable()
		{
			return onStateMachineEnter ?? (onStateMachineEnter = new Subject<OnStateMachineInfo>());
		}

		public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
		{
			if (onStateMachineExit != null)
			{
				onStateMachineExit.OnNext(new OnStateMachineInfo(animator, stateMachinePathHash));
			}
		}

		public IObservable<OnStateMachineInfo> OnStateMachineExitAsObservable()
		{
			return onStateMachineExit ?? (onStateMachineExit = new Subject<OnStateMachineInfo>());
		}
	}
}
