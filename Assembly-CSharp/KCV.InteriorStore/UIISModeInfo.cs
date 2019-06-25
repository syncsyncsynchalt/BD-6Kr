using System.Collections;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIISModeInfo : MonoBehaviour
	{
		private static readonly float MODE_MOVE_TIME = 1f;

		private UISprite _uiMode;

		private Vector3[] _vPos = new Vector3[2]
		{
			new Vector3(372f, 238f, 0f),
			new Vector3(590f, 238f, 0f)
		};

		private void Awake()
		{
			base.transform.localPosition = _vPos[1];
			Util.FindParentToChild(ref _uiMode, base.transform, "Mode");
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void ReqMode(ISTaskManagerMode iMode)
		{
			Vector3 vector = (iMode != 0) ? _vPos[0] : _vPos[1];
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", vector);
			hashtable.Add("time", MODE_MOVE_TIME);
			hashtable.Add("isLocal", true);
			hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
			switch (iMode)
			{
			case ISTaskManagerMode.Interior:
				_uiMode.spriteName = "header_change";
				break;
			case ISTaskManagerMode.Store:
				_uiMode.spriteName = "header_shop";
				break;
			}
			iTween.MoveTo(base.gameObject, hashtable);
			hashtable.Clear();
		}
	}
}
