using local.managers;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIStoreUserInfo : MonoBehaviour
	{
		private static readonly float MOVE_TIME = 1f;

		private UILabel _uiWorkerVal;

		private UILabel _uiFCoinVal;

		private Vector3[] _vPos = new Vector3[2]
		{
			new Vector3(0f, -272f, 0f),
			new Vector3(-1000f, -272f, 0f)
		};

		private void Awake()
		{
			base.transform.localPosition = _vPos[1];
			Util.FindParentToChild(ref _uiWorkerVal, base.transform, "Worker/Val");
			Util.FindParentToChild(ref _uiFCoinVal, base.transform, "FCoin/Val");
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void SetUserInfos(FurnitureStoreManager manager)
		{
			_uiWorkerVal.textInt = manager.GetWorkerCount();
			_uiFCoinVal.textInt = manager.UserInfo.FCoin;
		}

		public void ReqMode(ISTaskManagerMode iMode)
		{
			base.transform.localPosition = ((iMode != ISTaskManagerMode.Store) ? _vPos[1] : _vPos[0]);
		}
	}
}
