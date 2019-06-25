using KCV.Battle.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleCameras : IDisposable
	{
		private BattleCutInCamera _camCutInCamera;

		private BattleCutInEffectCamera _camCutInEffectCamera;

		private List<BattleFieldCamera> _listCameras;

		private BattleFieldDimCamera _camFieldDimCamera;

		private bool _isSplit;

		private bool _isSplit2d;

		public BattleCutInCamera cutInCamera => _camCutInCamera;

		public BattleCutInEffectCamera cutInEffectCamera => _camCutInEffectCamera;

		public List<BattleFieldCamera> fieldCameras => _listCameras;

		public BattleFieldCamera friendFieldCamera => fieldCameras[0];

		public BattleFieldCamera enemyFieldCamera => fieldCameras[1];

		public BattleFieldDimCamera fieldDimCamera => _camFieldDimCamera;

		public bool isSplit
		{
			get
			{
				return _isSplit;
			}
			set
			{
				SetSplitCameras(value);
			}
		}

		public bool isSplit2D
		{
			get
			{
				return _isSplit2d;
			}
			set
			{
				SetSplitCameras2D(value);
			}
		}

		public bool isFieldDimCameraEnabled
		{
			get
			{
				return _camFieldDimCamera.enabled;
			}
			set
			{
				if (!value)
				{
					_camFieldDimCamera.cullingMask = Generics.Layers.Nothing;
					_camFieldDimCamera.isCulling = false;
					_camFieldDimCamera.isSync = false;
					_camFieldDimCamera.maskAlpha = 0f;
				}
				_camFieldDimCamera.enabled = value;
			}
		}

		public BattleCameras()
		{
			_camCutInCamera = GameObject.Find("UIRoot/CutInCamera").GetComponent<BattleCutInCamera>();
			_camCutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			_camCutInCamera.depth = 6f;
			_camCutInCamera.clearFlags = CameraClearFlags.Depth;
			_camCutInEffectCamera = GameObject.Find("UIRoot/CutInEffectCamera").GetComponent<BattleCutInEffectCamera>();
			_camCutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			_camCutInEffectCamera.isCulling = false;
			_camCutInEffectCamera.depth = 5f;
			_camCutInEffectCamera.clearFlags = CameraClearFlags.Depth;
			_listCameras = new List<BattleFieldCamera>();
			foreach (int value in Enum.GetValues(typeof(FleetType)))
			{
				if (value != 2)
				{
					BattleFieldCamera item = (!GameObject.Find($"Stage/{(FleetType)value}FieldCamera")) ? null : GameObject.Find($"Stage/{(FleetType)value}FieldCamera").GetComponent<BattleFieldCamera>();
					_listCameras.Add(item);
					if (_listCameras[value] != null)
					{
						_listCameras[value].cullingMask = GetDefaultLayers();
						_listCameras[value].ResetMotionBlur();
						_listCameras[value].depth = 0f;
					}
				}
			}
			_camFieldDimCamera = BattleFieldDimCamera.Instantiate(((Component)BattleTaskManager.GetPrefabFile().prefabFieldDimCamera).GetComponent<BattleFieldDimCamera>(), BattleTaskManager.GetStage());
			_camFieldDimCamera.syncTarget = _listCameras[0].transform;
			_camFieldDimCamera.cullingMask = GetDefaultDimLayers();
			_camFieldDimCamera.isCulling = false;
			_camFieldDimCamera.depth = -1f;
		}

		public bool Init()
		{
			return true;
		}

		public void Dispose()
		{
			Mem.Del(ref _camCutInCamera);
			Mem.Del(ref _camCutInEffectCamera);
			Mem.DelListSafe(ref _listCameras);
			Mem.Del(ref _camFieldDimCamera);
			Mem.Del(ref _isSplit);
			Mem.Del(ref _isSplit2d);
		}

		public bool ReleaseBeforeResult()
		{
			if (_listCameras != null)
			{
				_listCameras.ForEach(delegate(BattleFieldCamera x)
				{
					if (x.gameObject != null)
					{
						UnityEngine.Object.Destroy(x.gameObject);
					}
				});
				Mem.DelListSafe(ref _listCameras);
			}
			Mem.DelComponentSafe(ref _camFieldDimCamera);
			return true;
		}

		public bool InitEnemyFieldCameraDefault()
		{
			enemyFieldCamera.isCulling = false;
			enemyFieldCamera.eyePosition = new Vector3(0f, 4f, 0f);
			enemyFieldCamera.eyeRotation = Quaternion.identity;
			enemyFieldCamera.fieldOfView = 30f;
			return true;
		}

		public bool InitShellingPhaseCamera()
		{
			Vector3 eyePosition = new Vector3(0f, 4f, 0f);
			Quaternion identity = Quaternion.identity;
			_listCameras[0].eyePosition = eyePosition;
			_listCameras[0].eyeRotation = identity;
			_listCameras[1].eyePosition = eyePosition;
			_listCameras[1].eyeRotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
			return true;
		}

		public void SwitchMainCamera(FleetType iType)
		{
			int cnt = 0;
			_listCameras.ForEach(delegate(BattleFieldCamera x)
			{
				x.isCulling = ((cnt == (int)iType) ? true : false);
				cnt++;
			});
		}

		public void ResetFieldCamSettings(FleetType iType)
		{
			_listCameras[(int)iType].ResetMotionBlur();
			_listCameras[(int)iType].clearFlags = CameraClearFlags.Skybox;
			_listCameras[(int)iType].cullingMask = GetDefaultLayers();
		}

		public void fieldDimCameraEnabled(bool isEnabled)
		{
			if (isEnabled)
			{
				_camFieldDimCamera.SyncCameraProperty();
				_camFieldDimCamera.isCulling = true;
				_listCameras[0].cullingMask = Generics.Layers.FocusDim;
			}
			else
			{
				_camFieldDimCamera.cullingMask = Generics.Layers.Nothing;
				_camFieldDimCamera.isCulling = false;
				_camFieldDimCamera.isSync = false;
				_camFieldDimCamera.maskAlpha = 0f;
			}
		}

		public void SetFieldCameraEnabled(bool isEnabled)
		{
			_listCameras.ForEach(delegate(BattleFieldCamera x)
			{
				x.isCulling = isEnabled;
			});
			_camFieldDimCamera.isCulling = isEnabled;
		}

		public Generics.Layers GetDefaultLayers()
		{
			return Generics.Layers.TransparentFX | Generics.Layers.Water | Generics.Layers.Background | Generics.Layers.ShipGirl | Generics.Layers.Effects | Generics.Layers.UnRefrectEffects;
		}

		public Generics.Layers GetEnemyCamSplitLayers()
		{
			return Generics.Layers.Background | Generics.Layers.Transition | Generics.Layers.ShipGirl | Generics.Layers.Effects | Generics.Layers.UnRefrectEffects | Generics.Layers.SplitWater;
		}

		public Generics.Layers GetDefaultDimLayers()
		{
			return Generics.Layers.TransparentFX | Generics.Layers.Water | Generics.Layers.Background | Generics.Layers.ShipGirl | Generics.Layers.Effects;
		}

		public void SetSplitCameras(bool isSplit)
		{
			Rect viewportRect = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0.5f, 1f, 1f);
			Rect viewportRect2 = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0f, 1f, 0.5f);
			bool isCulling = isSplit ? true : false;
			_listCameras[0].viewportRect = viewportRect;
			_listCameras[0].isCulling = true;
			_listCameras[1].viewportRect = viewportRect2;
			_listCameras[1].isCulling = isCulling;
			if (!isSplit)
			{
				_camCutInCamera.camera.rect = new Rect(0f, 0f, 1f, 1f);
				_camCutInCamera.camera.enabled = true;
				_camCutInCamera.camera.orthographicSize = 1f;
			}
			_isSplit = isSplit;
		}

		public void SetSplitCameras(bool isSplit, bool is2DCamUpScreen, FleetType iType)
		{
			Rect rect = (!is2DCamUpScreen) ? new Rect(0f, 0f, 1f, 0.5f) : new Rect(0f, 0.5f, 1f, 1f);
			Rect rect3DCam = (!is2DCamUpScreen) ? new Rect(0f, 0.5f, 1f, 1f) : new Rect(0f, 0f, 1f, 0.5f);
			if (isSplit)
			{
				_camCutInCamera.camera.rect = rect;
				_camCutInCamera.camera.orthographicSize = 0.5f;
				_camCutInCamera.enabled = true;
				int cnt = 0;
				_listCameras.ForEach(delegate(BattleFieldCamera x)
				{
					if (cnt == (int)iType)
					{
						x.viewportRect = rect3DCam;
						x.isCulling = true;
					}
					else
					{
						x.isCulling = false;
					}
					cnt++;
				});
			}
			else
			{
				SetSplitCameras(isSplit);
			}
			_isSplit = isSplit;
		}

		public void SetVerticalSplitCameras(bool isSplit)
		{
			Rect viewportRect = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0f, 0.5f, 1f);
			Rect viewportRect2 = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0.5f, 0f, 0.5f, 1f);
			bool isCulling = isSplit ? true : false;
			_listCameras[0].viewportRect = viewportRect;
			_listCameras[0].isCulling = true;
			_listCameras[1].viewportRect = viewportRect2;
			_listCameras[1].isCulling = isCulling;
			if (!isSplit)
			{
				_camCutInCamera.camera.rect = new Rect(0f, 0f, 1f, 1f);
				_camCutInCamera.camera.enabled = true;
				_camCutInCamera.camera.orthographicSize = 1f;
			}
			_isSplit = isSplit;
		}

		public void SetSplitCameras2D(bool isSplit)
		{
			Rect viewportRect = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0.5f, 1f, 0.5f);
			Vector3 localPosition = (!isSplit) ? Vector3.zero : (Vector3.left * Screen.width);
			Rect viewportRect2 = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0f, 1f, 0.5f);
			Vector3 localPosition2 = (!isSplit) ? (Vector3.down * 3000f) : (Vector3.right * Screen.width);
			bool isCulling = isSplit ? true : false;
			cutInCamera.viewportRect = viewportRect;
			cutInCamera.isCulling = true;
			cutInCamera.transform.localPosition = localPosition;
			cutInEffectCamera.viewportRect = viewportRect2;
			cutInEffectCamera.isCulling = isCulling;
			cutInEffectCamera.transform.localPosition = localPosition2;
			_isSplit2d = isSplit;
		}

		public void SetSplitCamera2DSamePos(bool isSplit)
		{
			Rect viewportRect = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0.5f, 1f, 0.5f);
			Vector3 localPosition = (!isSplit) ? Vector3.zero : Vector3.zero;
			Rect viewportRect2 = (!isSplit) ? new Rect(0f, 0f, 1f, 1f) : new Rect(0f, 0f, 1f, 0.5f);
			Vector3 localPosition2 = (!isSplit) ? (Vector3.down * 3000f) : Vector3.zero;
			bool isCulling = isSplit ? true : false;
			cutInCamera.viewportRect = viewportRect;
			cutInCamera.isCulling = true;
			cutInCamera.transform.localPosition = localPosition;
			cutInEffectCamera.viewportRect = viewportRect2;
			cutInEffectCamera.isCulling = isCulling;
			cutInEffectCamera.transform.localPosition = localPosition2;
			_isSplit2d = isSplit;
		}
	}
}
