using local.managers;
using local.models;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class StrategyRebellionTaskManager : SceneTaskMono
	{
		[SerializeField]
		private TaskRebellionEvent _clsTaskEvent;

		[SerializeField]
		private TaskRebellionOrganize _clsTaskOrgnaize;

		[SerializeField]
		private Transform _prefabProdWaringBackground;

		private KeyControl _clsInput;

		private SceneTasksMono _clsTasks;

		private StrategyRebellionTaskManagerMode _iMode;

		private StrategyRebellionTaskManagerMode _iModeReq;

		private RebellionManager _clsRebellionManager;

		public static bool RebellionForceDebug;

		public static int RebellionArea;

		public static int RebellionFromArea;

		public KeyControl keycontrol => _clsInput;

		public TaskRebellionEvent taskRebellionEvent => _clsTaskEvent;

		public TaskRebellionOrganize taskRebellionOrganize => _clsTaskOrgnaize;

		protected override bool Init()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			int rebellionArea = RebellionArea;
			_clsRebellionManager = StrategyTopTaskManager.GetLogicManager().SelectAreaForRebellion(rebellionArea);
			_clsInput = new KeyControl();
			_clsTasks = this.SafeGetComponent<SceneTasksMono>();
			_clsTasks.Init();
			_iMode = (_iModeReq = StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_BEF);
			Observable.FromCoroutine((IObserver<bool> observer) => CreateRequireInstante(observer)).Subscribe(delegate(bool x)
			{
				if (x)
				{
					_iMode = (_iModeReq = StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_ST);
				}
			}).AddTo(base.gameObject);
			return true;
		}

		protected override bool UnInit()
		{
			_clsTasks.UnInit();
			Mem.Del(ref _clsTasks);
			Mem.Del(ref _clsRebellionManager);
			return true;
		}

		protected override bool Run()
		{
			_clsInput.Update();
			_clsTasks.Run();
			UpdateMode();
			if (StrategyTaskManager.GetMode() != StrategyTaskManager.StrategyTaskManagerMode.StrategyTaskManagerMode_BEF)
			{
				return (StrategyTaskManager.GetMode() == StrategyTaskManager.StrategyTaskManagerMode.Rebellion) ? true : false;
			}
			return true;
		}

		public StrategyRebellionTaskManagerMode GetMode()
		{
			return _iModeReq;
		}

		public void ReqMode(StrategyRebellionTaskManagerMode iMode)
		{
			_iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (_iModeReq == StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_BEF)
			{
				return;
			}
			switch (_iModeReq)
			{
			case StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_ST:
				if (_clsTasks.Open(_clsTaskEvent) < 0)
				{
					return;
				}
				break;
			case StrategyRebellionTaskManagerMode.Organize:
				if (_clsTasks.Open(_clsTaskOrgnaize) < 0)
				{
					return;
				}
				break;
			}
			_iMode = _iModeReq;
			_iModeReq = StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_BEF;
		}

		public RebellionManager GetRebellionManager()
		{
			return _clsRebellionManager;
		}

		private IEnumerator CreateRequireInstante(IObserver<bool> observer)
		{
			observer.OnNext(value: false);
			yield return null;
			observer.OnNext(value: true);
			observer.OnCompleted();
		}

		public static void checkRebellionArea()
		{
			List<MapAreaModel> rebellionAreaList = StrategyTopTaskManager.GetLogicManager().GetRebellionAreaList();
			RebellionArea = rebellionAreaList[0].Id;
			List<int> neighboringAreaIDs = rebellionAreaList[0].NeighboringAreaIDs;
			int num = neighboringAreaIDs.FindIndex((int x) => !StrategyTopTaskManager.GetLogicManager().Area[x].IsOpen());
			RebellionFromArea = ((num != -1) ? rebellionAreaList[0].NeighboringAreaIDs[num] : (-1));
			DebugUtils.Log("反攻発生: 発生エリア" + RebellionArea + " 矢印エリア" + RebellionFromArea);
		}

		public void Termination()
		{
			ImmediateTermination();
		}
	}
}
