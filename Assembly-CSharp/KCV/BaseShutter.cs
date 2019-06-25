using KCV.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(UIPanel))]
	public class BaseShutter : MonoBehaviour
	{
		public enum ShutterMode
		{
			None = -1,
			Close,
			Open
		}

		protected const float SHUTTER_OPENCLOSE_TIME = 0.25f;

		[SerializeField]
		protected UIPanel _uiPanel;

		[SerializeField]
		protected Transform _traShutter;

		[SerializeField]
		protected UISprite _uiTop;

		[SerializeField]
		protected UISprite _uiBtm;

		[SerializeField]
		protected BoxCollider2D _colBox2D;

		protected bool _isTween;

		protected Vector3[] _vTopPos = new Vector3[2]
		{
			new Vector3(0f, 272f, 0f),
			new Vector3(0f, 575f, 0f)
		};

		protected Vector3[] _vBtnPos = new Vector3[2]
		{
			new Vector3(0f, -272f, 0f),
			new Vector3(0f, -575f, 0f)
		};

		protected ShutterMode _iShutterMode;

		protected Action _actCallback;

		public int panelDepth
		{
			get
			{
				return _uiPanel.depth;
			}
			set
			{
				if (_uiPanel.depth != value)
				{
					_uiPanel.depth = value;
				}
			}
		}

		protected virtual void Awake()
		{
			if (_uiPanel == null)
			{
				_uiPanel = GetComponent<UIPanel>();
			}
			if (_traShutter == null)
			{
				Util.FindParentToChild(ref _traShutter, _uiPanel.transform, "Shutter");
			}
			if (_uiBtm == null)
			{
				Util.FindParentToChild(ref _uiBtm, _traShutter, "Btm");
			}
			if (_uiTop == null)
			{
				Util.FindParentToChild(ref _uiTop, _traShutter, "Top");
			}
			_actCallback = null;
			_iShutterMode = ShutterMode.Open;
			_uiTop.transform.localPosition = _vTopPos[1];
			_uiBtm.transform.localPosition = _vBtnPos[1];
		}

		private void OnDestroy()
		{
			UnInit();
		}

		public virtual bool Init(ShutterMode iMode)
		{
			_uiTop.transform.localPosition = _vTopPos[(int)iMode];
			_uiBtm.transform.localPosition = _vBtnPos[(int)iMode];
			_iShutterMode = iMode;
			return true;
		}

		public virtual bool UnInit()
		{
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _traShutter);
			if (_uiTop != null)
			{
				_uiTop.Clear();
			}
			Mem.Del(ref _uiTop);
			if (_uiBtm != null)
			{
				_uiBtm.Clear();
			}
			Mem.Del(ref _uiBtm);
			Mem.Del(ref _isTween);
			Mem.DelArySafe(ref _vTopPos);
			Mem.DelArySafe(ref _vBtnPos);
			Mem.Del(ref _iShutterMode);
			Mem.Del(ref _actCallback);
			return true;
		}

		public virtual void SetLayer(int nPanelDepth)
		{
			_uiPanel.depth = nPanelDepth;
		}

		public virtual void ReqMode(ShutterMode iMode, Action callback)
		{
			if (iMode == ShutterMode.None || _iShutterMode == iMode)
			{
				return;
			}
			_actCallback = callback;
			if (!_isTween)
			{
				if (iMode == ShutterMode.Close)
				{
					SoundUtils.PlaySE(SEFIleInfos.SE_921);
				}
				Hashtable hashtable = new Hashtable();
				hashtable.Add("time", 0.25f);
				hashtable.Add("isLocal", true);
				hashtable.Add("easetype", iTween.EaseType.easeOutBounce);
				hashtable.Add("oncompletetarget", base.gameObject);
				hashtable.Add("oncomplete", "OnShutterActionComplate");
				hashtable.Add("position", _vTopPos[(int)iMode]);
				iTween.MoveTo(_uiTop.gameObject, hashtable);
				hashtable.Remove("position");
				hashtable.Remove("oncompletetarget");
				hashtable.Remove("oncomplete");
				hashtable.Add("position", _vBtnPos[(int)iMode]);
				iTween.MoveTo(_uiBtm.gameObject, hashtable);
			}
			_iShutterMode = iMode;
		}

		protected virtual void OnShutterActionComplate()
		{
			_isTween = false;
			if (_actCallback != null)
			{
				_actCallback();
			}
		}
	}
}
