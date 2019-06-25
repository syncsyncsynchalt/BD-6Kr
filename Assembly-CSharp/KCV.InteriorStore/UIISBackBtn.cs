using KCV.Utils;
using System.Collections;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIISBackBtn : MonoBehaviour
	{
		private static readonly float BACKBTN_MOVE_TIME = 1f;

		private UIButton _uiBackBtn;

		private Vector3[] _vPos = new Vector3[2]
		{
			new Vector3(-580f, -248f, 0f),
			new Vector3(-385f, -248f, -0f)
		};

		private EventDelegate.Callback _actCallback;

		private void Awake()
		{
			Util.FindParentToChild(ref _uiBackBtn, base.transform, "BackBtn");
			base.transform.localPosition = _vPos[0];
		}

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void ReqMode(bool isScreenIn)
		{
			Vector3 vector = (!isScreenIn) ? _vPos[0] : _vPos[1];
			ColliderEnabled(isScreenIn);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", vector);
			hashtable.Add("time", BACKBTN_MOVE_TIME);
			hashtable.Add("easeType", iTween.EaseType.easeOutExpo);
			hashtable.Add("isLocal", true);
			iTween.MoveTo(base.gameObject, hashtable);
			hashtable.Clear();
		}

		public void ColliderEnabled(bool isEnabled)
		{
			_uiBackBtn.isEnabled = isEnabled;
		}

		public void AddDelegade(EventDelegate.Callback callback)
		{
			_actCallback = callback;
			_uiBackBtn.onClick.Clear();
			EventDelegate.Add(_uiBackBtn.onClick, delegate
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				_actCallback();
				this.DelayAction(0.2f, delegate
				{
					_uiBackBtn.state = UIButtonColor.State.Normal;
				});
			});
		}
	}
}
