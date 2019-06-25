using UnityEngine;

namespace KCV.Inherit
{
	public class InheritLoadTaskManager : MonoBehaviour
	{
		public enum InheritTaskManagerMode
		{
			InheritTaskManagerMode_ST = 0,
			InheritTaskManagerMode_BEF = -1,
			InheritTaskManagerMode_NONE = -1,
			LoadSelect = 0,
			Privilege = 1,
			InheritTaskManagerMode_AFT = 2,
			InheritTaskManagerMode_NUM = 2,
			InheritTaskManagerMode_ED = 1
		}

		private static SceneTasksMono _clsTasks;

		private static InheritTaskManagerMode _iMode;

		private static InheritTaskManagerMode _iModeReq;

		[SerializeField]
		private TaskInheritLoadSelect _clsLoadSelect;

		[SerializeField]
		private TaskInheritPrivilege _clsPrivilege;

		private void Awake()
		{
			_clsTasks = base.gameObject.SafeGetComponent<SceneTasksMono>();
		}

		private void Start()
		{
			_iMode = (_iModeReq = InheritTaskManagerMode.Privilege);
			_clsTasks.Init();
		}

		private void OnDestroy()
		{
			_clsTasks.UnInit();
			_clsLoadSelect = null;
			_clsPrivilege = null;
		}

		private void Update()
		{
			_clsTasks.Run();
			UpdateMode();
		}

		public static InheritTaskManagerMode GetMode()
		{
			return _iModeReq;
		}

		public static void ReqMode(InheritTaskManagerMode iMode)
		{
			_iModeReq = iMode;
		}

		protected void UpdateMode()
		{
			if (_iModeReq == InheritTaskManagerMode.InheritTaskManagerMode_BEF)
			{
				return;
			}
			switch (_iModeReq)
			{
			case InheritTaskManagerMode.InheritTaskManagerMode_ST:
				if (_clsTasks.Open(_clsLoadSelect) < 0)
				{
					return;
				}
				break;
			case InheritTaskManagerMode.Privilege:
				if (_clsTasks.Open(_clsPrivilege) < 0)
				{
					return;
				}
				break;
			}
			_iMode = _iModeReq;
			_iModeReq = InheritTaskManagerMode.InheritTaskManagerMode_BEF;
		}
	}
}
