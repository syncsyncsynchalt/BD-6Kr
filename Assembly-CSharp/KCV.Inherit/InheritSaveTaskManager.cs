using UnityEngine;

namespace KCV.Inherit
{
	public class InheritSaveTaskManager : MonoBehaviour
	{
		public enum InheritTaskManagerMode
		{
			InheritTaskManagerMode_ST = 0,
			InheritTaskManagerMode_BEF = -1,
			InheritTaskManagerMode_NONE = -1,
			SaveSelect = 0,
			DoSave = 1,
			InheritTaskManagerMode_AFT = 2,
			InheritTaskManagerMode_NUM = 2,
			InheritTaskManagerMode_ED = 1
		}

		private static SceneTasksMono _clsTasks;

		private static InheritTaskManagerMode _iMode;

		private static InheritTaskManagerMode _iModeReq;

		[SerializeField]
		private TaskInheritSaveSelect _clsSaveSelect;

		[SerializeField]
		private TaskInheritDoSave _clsDoSave;

		public bool isSaved;

		private void OnSaved()
		{
			_clsSaveSelect.isSaved = true;
		}

		private void Awake()
		{
			_clsTasks = base.gameObject.SafeGetComponent<SceneTasksMono>();
			_clsDoSave.OnSavedCallBack = OnSaved;
		}

		private void Start()
		{
			_iMode = (_iModeReq = InheritTaskManagerMode.InheritTaskManagerMode_ST);
			_clsTasks.Init();
		}

		private void OnDestroy()
		{
			_clsTasks.UnInit();
			_clsSaveSelect = null;
			_clsDoSave = null;
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
				if (_clsTasks.Open(_clsSaveSelect) < 0)
				{
					return;
				}
				break;
			case InheritTaskManagerMode.DoSave:
				if (_clsTasks.Open(_clsDoSave) < 0)
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
