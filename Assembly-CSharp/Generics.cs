using KCV;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Generics
{
	public enum Tag
	{
		Untagged,
		Respawn,
		Finish,
		EditorOverlay,
		MainCamera,
		Player,
		GameController,
		Tiles,
		MainMenuButton,
		TransitionButton,
		IndexButton,
		ItemLabel,
		DeckIcon,
		ShipGirl,
		Search_Object,
		StrategyTile,
		SuppryIcon,
		CommonShipBanner,
		CommandSurface
	}

	[Flags]
	public enum Layers
	{
		Nothing = 0x0,
		Everything = -1,
		Default = 0x1,
		TransparentFX = 0x2,
		IgnoreRaycast = 0x4,
		Water = 0x10,
		UI = 0x20,
		Background = 0x100,
		UI2D = 0x200,
		UI3D = 0x400,
		Transition = 0x800,
		ShipGirl = 0x1000,
		TopMost = 0x2000,
		CutIn = 0x4000,
		SaveData = 0x8000,
		Effects = 0x10000,
		FocusDim = 0x20000,
		UnRefrectEffects = 0x40000,
		SplitWater = 0x80000
	}

	public enum Scene
	{
		Scene_ST = 0,
		Scene_BEF = -1,
		Scene_None = -1,
		SplashScreen = 0,
		Title = 1,
		Startup = 2,
		Port = 3,
		PortTop = 4,
		Battle = 5,
		Organize = 6,
		Supply = 7,
		Remodel = 8,
		Repair = 9,
		Arsenal = 10,
		ImprovementArsenal = 11,
		Duty = 12,
		Record = 13,
		Item = 14,
		Interior = 0xF,
		Option = 0x10,
		SaveLoad = 17,
		InheritSave = 18,
		InheritLoad = 19,
		Album = 20,
		Strategy = 21,
		MapSelect = 22,
		SortieAreaMap = 23,
		Practice = 24,
		ExercisesPartnerSelection = 25,
		Expedition = 26,
		Deploy = 27,
		EscortFleetOrganization = 28,
		Marriage = 29,
		ArsenalSelector = 30,
		Ending = 0x1F,
		LoadingScene = 0x20,
		Scene_Empty = 33,
		Scene_AFT = 34,
		Scene_NUM = 34,
		Scene_ED = 33
	}

	public enum BattleRootType
	{
		Practice,
		SortieMap,
		Rebellion
	}

	[Serializable]
	public class InnerCamera
	{
		[SerializeField]
		protected Camera _camCamera;

		[SerializeField]
		protected UICamera _camUICamera;

		public virtual Layers sameMask
		{
			set
			{
				_camCamera.cullingMask = (_camUICamera.eventReceiverMask = (int)value);
			}
		}

		public virtual float depth
		{
			get
			{
				return _camCamera.depth;
			}
			set
			{
				_camCamera.depth = value;
			}
		}

		public InnerCamera(Transform instance)
		{
			_camCamera = ((Component)instance).GetComponent<Camera>();
			_camUICamera = ((Component)instance).GetComponent<UICamera>();
		}

		public InnerCamera(Transform parent, string objName)
		{
			Util.FindParentToChild(ref _camCamera, parent, objName);
			Util.FindParentToChild(ref _camUICamera, parent, objName);
		}

		public bool Init()
		{
			return true;
		}

		public bool UnInit()
		{
			_camCamera = null;
			_camUICamera = null;
			return true;
		}

		public virtual void CullingMask(Layers layer)
		{
			_camCamera.cullingMask = (int)layer;
		}

		public virtual void EventMask(Layers layer)
		{
			_camUICamera.eventReceiverMask = (int)layer;
		}

		public virtual void SameMask(Layers layer)
		{
			_camCamera.cullingMask = (_camUICamera.eventReceiverMask = (int)layer);
		}
	}

	[Serializable]
	public class Message
	{
		private UILabel _uiLabel;

		private string _strTemp;

		private int _nPos;

		private float _fInterVal;

		private float _fTimer;

		private bool _isSkip;

		private bool _isTalk;

		private bool _isStartMessage;

		private Action _actCallback;

		public bool IsMessageEnd => _isTalk;

		public Message(Transform parent, string objName)
		{
			Util.FindParentToChild(ref _uiLabel, parent, objName);
			_uiLabel.text = string.Empty;
		}

		public Message(UILabel label)
		{
			_uiLabel = label;
			_uiLabel.text = string.Empty;
		}

		public bool Init(string message, float interval = 0.03f, Action callback = null)
		{
			_strTemp = Util.Indentision(message);
			_nPos = 0;
			_isSkip = false;
			_fInterVal = interval;
			_fTimer = _fInterVal;
			_isTalk = false;
			_isStartMessage = false;
			_actCallback = callback;
			return true;
		}

		public bool UnInit()
		{
			_uiLabel = null;
			_actCallback = null;
			_strTemp = string.Empty;
			return true;
		}

		public void Update()
		{
			if (_isTalk)
			{
			}
			if (!_isStartMessage)
			{
				return;
			}
			_fTimer -= Time.deltaTime;
			if (!(_fTimer <= 0f))
			{
				return;
			}
			if (_nPos < _strTemp.Length)
			{
				_nPos++;
				_uiLabel.text = _strTemp.Substring(0, _nPos);
			}
			else
			{
				_isTalk = true;
				_isStartMessage = false;
				if (_actCallback != null)
				{
					_actCallback();
				}
			}
			_fTimer = _fInterVal;
		}

		public void Reset()
		{
			_uiLabel.text = string.Empty;
			_nPos = 0;
			_isSkip = false;
			_isTalk = false;
			_isSkip = false;
		}

		public void Play()
		{
			_isStartMessage = true;
		}
	}

	[Serializable]
	public class NextIndexInfos
	{
		public int Up = -1;

		public int UpLeft = -1;

		public int UpRight = -1;

		public int Left = -1;

		public int Right = -1;

		public int Center = -1;

		public int Down = -1;

		public int DownLeft = -1;

		public int DownRight = -1;

		public NextIndexInfos(int up, int upleft, int upright, int center, int left, int right, int down, int downleft, int downright)
		{
			Up = up;
			UpLeft = upleft;
			UpRight = upright;
			Right = right;
			Left = left;
			Center = center;
			Down = down;
			DownLeft = downleft;
			DownRight = downright;
		}

		public NextIndexInfos(int up, int down, int left, int right)
		{
			Up = up;
			Down = down;
			Left = left;
			Right = right;
			UpLeft = (UpLeft = (DownLeft = (DownRight = (Left = (Right = -1)))));
		}

		public NextIndexInfos()
		{
			Up = (Down = (Left = (Right = (UpLeft = (UpLeft = (DownLeft = (DownRight = (Left = (Right = -1)))))))));
		}

		public int GetIndex(KeyControl.KeyName iName, int defVal)
		{
			int num = -1;
			switch (iName)
			{
			case KeyControl.KeyName.UP:
				num = Up;
				break;
			case KeyControl.KeyName.DOWN:
				num = Down;
				break;
			case KeyControl.KeyName.LEFT:
				num = Left;
				break;
			case KeyControl.KeyName.RIGHT:
				num = Right;
				break;
			case KeyControl.KeyName.UP_LEFT:
				num = UpLeft;
				break;
			case KeyControl.KeyName.UP_RIGHT:
				num = UpRight;
				break;
			case KeyControl.KeyName.DOWN_LEFT:
				num = DownLeft;
				break;
			case KeyControl.KeyName.DOWN_RIGHT:
				num = DownRight;
				break;
			}
			return (num != -1) ? num : defVal;
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct BBCodeColor
	{
		public static string kGreen => "1DBDC0";

		public static string red => "FF0000";

		public static string UIBlue => "64C8FF";

		public static string kGreenDark => "0E5E60";

		public static Color ToRGBA(uint color16, float alpha = 1f)
		{
			float num = 0.003921569f;
			Color black = Color.black;
			black.r = num * (float)(double)((color16 >> 16) & 0xFF);
			black.g = num * (float)(double)((color16 >> 8) & 0xFF);
			black.b = num * (float)(double)(color16 & 0xFF);
			black.a = alpha;
			return black;
		}
	}
}
