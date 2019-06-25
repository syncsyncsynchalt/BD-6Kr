using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.managers;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialSecondCutIn : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] _uiAirObj;

		[SerializeField]
		private GameObject[] _uiShipObj;

		[SerializeField]
		private ParticleSystem _cloudParticle;

		[SerializeField]
		private UITexture[] _uiAircraft;

		[SerializeField]
		public UITexture[] _uiShip;

		private Animation[] _airAnimation;

		private bool _isAnimeFinished;

		private bool _isPlaying;

		private Action _actCallback;

		private CutInType _iType;

		private BattleFieldCamera _camFieldCamera;

		private KoukuuModel _koukuuModel;

		private ProdAntiAerialCutIn _prodAntiAerialCutIn;

		private bool _init()
		{
			_isAnimeFinished = false;
			GameObject gameObject = base.transform.FindChild("Aircraft").gameObject;
			_uiAirObj = new GameObject[3];
			_uiAircraft = new UITexture[3];
			_airAnimation = (Animation[])new Animation[3];
			for (int i = 0; i < 3; i++)
			{
				if (_uiAirObj[i] == null)
				{
					_uiAirObj[i] = gameObject.transform.FindChild("Aircraft" + (i + 1)).gameObject;
				}
				if (_uiAircraft[i] == null)
				{
					_uiAircraft[i] = ((Component)_uiAirObj[i].transform.FindChild("Swing/Aircraft")).GetComponent<UITexture>();
				}
				_airAnimation[i] = _uiAircraft[i].GetComponent<Animation>();
			}
			_uiShipObj = new GameObject[2];
			_uiShip = new UITexture[2];
			for (int j = 0; j < 2; j++)
			{
				if (_uiShipObj[j] == null)
				{
					_uiShipObj[j] = base.transform.FindChild("ShipObj" + (j + 1)).gameObject;
				}
				if (_uiShip[j] == null)
				{
					_uiShip[j] = ((Component)_uiShipObj[j].transform.FindChild("Ship")).GetComponent<UITexture>();
				}
			}
			if ((UnityEngine.Object)_cloudParticle == null)
			{
				_cloudParticle = ((Component)base.transform.FindChild("Cloud")).GetComponent<ParticleSystem>();
			}
			_camFieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			_camFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			return true;
		}

		private void OnDestroy()
		{
			Mem.DelArySafe(ref _uiAirObj);
			Mem.DelArySafe(ref _uiShipObj);
			Mem.Del(ref _cloudParticle);
			Mem.DelArySafe(ref _uiAircraft);
			Mem.DelArySafe(ref _uiShip);
			Mem.DelArySafe(ref _airAnimation);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _iType);
			Mem.Del(ref _camFieldCamera);
			Mem.Del(ref _koukuuModel);
			if (_prodAntiAerialCutIn != null)
			{
				UnityEngine.Object.Destroy(_prodAntiAerialCutIn.gameObject);
			}
			Mem.Del(ref _prodAntiAerialCutIn);
		}

		public static ProdAerialSecondCutIn Instantiate(ProdAerialSecondCutIn prefab, KoukuuModel model, Transform parent)
		{
			ProdAerialSecondCutIn prodAerialSecondCutIn = UnityEngine.Object.Instantiate(prefab);
			prodAerialSecondCutIn.transform.parent = parent;
			prodAerialSecondCutIn.transform.localPosition = Vector3.zero;
			prodAerialSecondCutIn.transform.localScale = Vector3.one;
			prodAerialSecondCutIn._koukuuModel = model;
			return prodAerialSecondCutIn;
		}

		private void _setShipTexture(FleetType type)
		{
			switch (type)
			{
			case FleetType.Friend:
			{
				List<ShipModel_Attacker> list2 = (_koukuuModel.GetCaptainShip(is_friend: true) == null) ? null : _koukuuModel.GetAttackers(is_friend: true);
				if (list2 != null)
				{
					_uiShip[0].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(list2[0]);
					_uiShip[0].MakePixelPerfect();
					_uiShip[0].transform.localPosition = KCV.Battle.Utils.ShipUtils.GetShipOffsPos(list2[0], MstShipGraphColumn.CutInSp1);
					if (list2.Count >= 2)
					{
						_uiShip[1].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(list2[1]);
						_uiShip[1].MakePixelPerfect();
						_uiShip[1].transform.localPosition = KCV.Battle.Utils.ShipUtils.GetShipOffsPos(list2[1], MstShipGraphColumn.CutInSp1);
					}
					else
					{
						_uiShip[1].mainTexture = null;
					}
				}
				else
				{
					_uiShip[0].mainTexture = null;
					_uiShip[1].mainTexture = null;
				}
				break;
			}
			case FleetType.Enemy:
			{
				List<ShipModel_Attacker> list = (_koukuuModel.GetCaptainShip(is_friend: false) == null) ? null : _koukuuModel.GetAttackers(is_friend: false);
				if (list != null)
				{
					_uiShip[0].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(list[0]);
					_uiShip[0].MakePixelPerfect();
					_uiShip[0].flip = UIBasicSprite.Flip.Horizontally;
					Vector3 shipOffsPos = KCV.Battle.Utils.ShipUtils.GetShipOffsPos(list[0], MstShipGraphColumn.CutInSp1);
					_uiShip[0].transform.localPosition = new Vector3(shipOffsPos.x * -1f, shipOffsPos.y, shipOffsPos.z);
					if (list.Count >= 2)
					{
						_uiShip[1].mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(list[1]);
						_uiShip[1].MakePixelPerfect();
						_uiShip[1].flip = UIBasicSprite.Flip.Horizontally;
						Vector3 shipOffsPos2 = KCV.Battle.Utils.ShipUtils.GetShipOffsPos(list[1], MstShipGraphColumn.CutInSp1);
						_uiShip[1].transform.localPosition = new Vector3(shipOffsPos2.x * -1f, shipOffsPos2.y, shipOffsPos2.z);
					}
					else
					{
						_uiShip[1].mainTexture = null;
					}
				}
				else
				{
					_uiShip[0].mainTexture = null;
					_uiShip[1].mainTexture = null;
				}
				break;
			}
			}
		}

		public void Play(Action callback)
		{
			_isPlaying = true;
			_actCallback = callback;
			((Component)base.transform).GetComponent<UIPanel>().widgetsAreStatic = false;
			_init();
			_iType = _chkCutInType();
			_setCutin(_iType);
		}

		public CutInType _chkCutInType()
		{
			if (_koukuuModel.GetCaptainShip(is_friend: true) != null && _koukuuModel.GetCaptainShip(is_friend: false) != null)
			{
				return CutInType.Both;
			}
			if (_koukuuModel.GetCaptainShip(is_friend: true) != null)
			{
				return CutInType.FriendOnly;
			}
			return CutInType.EnemyOnly;
		}

		public bool _cutinPhaseCheck()
		{
			if (_iType == CutInType.Both)
			{
				return true;
			}
			return false;
		}

		private void _setCutin(CutInType type)
		{
			switch (type)
			{
			case CutInType.FriendOnly:
			case CutInType.Both:
			{
				_setShipTexture(FleetType.Friend);
				_setAircraftTexture(FleetType.Friend);
				for (int j = 0; j < 3; j++)
				{
					_airAnimation[j].Stop();
					_airAnimation[j].Play("AircraftCutin" + (j + 1));
				}
				Animation component2 = ((Component)base.transform).GetComponent<Animation>();
				component2.Stop();
				component2.Play("AerialSecondCutIn1");
				break;
			}
			case CutInType.EnemyOnly:
			{
				_setShipTexture(FleetType.Enemy);
				_setAircraftTexture(FleetType.Enemy);
				_changeSeaWave(FleetType.Enemy);
				base.transform.localEulerAngles = new Vector3(0f, -180f, 0f);
				for (int i = 0; i < 3; i++)
				{
					_airAnimation[i].Stop();
					_airAnimation[i].Play("AircraftCutin" + (i + 1));
				}
				Animation component = ((Component)base.transform).GetComponent<Animation>();
				component.Stop();
				component.Play("AerialSecondCutIn1");
				_isAnimeFinished = true;
				break;
			}
			}
		}

		private void _setAircraftTexture(FleetType type)
		{
			switch (type)
			{
			case FleetType.Friend:
			{
				if (_koukuuModel.GetCaptainShip(is_friend: true) == null)
				{
					break;
				}
				PlaneModelBase[] plane = _koukuuModel.GetPlane(_koukuuModel.GetCaptainShip(is_friend: true).TmpId);
				if (plane == null)
				{
					break;
				}
				for (int j = 0; j < plane.Length && j < 3; j++)
				{
					if (plane[j] != null)
					{
						_uiAirObj[j].transform.localPosition = new Vector3(267f, 176f, 0f);
						_uiAircraft[j].mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(plane[j].MstId, 6);
						_uiAirObj[j].transform.localEulerAngles = new Vector3(0f, 0f, -25.5f);
						KCV.Battle.Utils.SlotItemUtils.GetAircraftOffsetInfo(plane[j].MstId);
					}
					else
					{
						_uiAircraft[j].mainTexture = null;
					}
				}
				break;
			}
			case FleetType.Enemy:
			{
				if (_koukuuModel.GetCaptainShip(is_friend: false) == null)
				{
					break;
				}
				PlaneModelBase[] plane = _koukuuModel.GetPlane(_koukuuModel.GetCaptainShip(is_friend: false).TmpId);
				if (plane == null)
				{
					break;
				}
				for (int i = 0; i < plane.Length && i < 3; i++)
				{
					if (plane[i] != null)
					{
						if (BattleTaskManager.GetBattleManager() is PracticeBattleManager)
						{
							_uiAircraft[i].mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(plane[i].MstId, 6);
							continue;
						}
						_uiAirObj[i].transform.localPosition = new Vector3(267f, 176f, 0f);
						_uiAircraft[i].mainTexture = KCV.Battle.Utils.SlotItemUtils.LoadTexture(plane[i]);
						_uiAircraft[i].MakePixelPerfect();
						_uiAirObj[i].transform.localEulerAngles = Vector3.zero;
						if (KCV.Battle.Utils.SlotItemUtils.GetAircraftOffsetInfo(plane[i].MstId).isFlipHorizontal)
						{
							_uiAircraft[i].flip = UIBasicSprite.Flip.Nothing;
						}
						else
						{
							_uiAircraft[i].flip = UIBasicSprite.Flip.Horizontally;
						}
					}
					else
					{
						_uiAircraft[i].mainTexture = null;
					}
				}
				break;
			}
			}
		}

		private void _changeSeaWave(FleetType type)
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			switch (type)
			{
			case FleetType.Friend:
				battleField.seaLevel.waveSpeed = new Vector4(-4f, -500f, 5f, -400f);
				_camFieldCamera.transform.localPosition = new Vector3(50f, 4f, 100f);
				_camFieldCamera.transform.rotation = Quaternion.Euler(new Vector3(22f, 20f, 0f));
				break;
			case FleetType.Enemy:
				battleField.seaLevel.waveSpeed = new Vector4(-4f, 500f, 5f, 400f);
				_camFieldCamera.transform.localPosition = new Vector3(50f, 4f, 100f);
				_camFieldCamera.transform.rotation = Quaternion.Euler(new Vector3(22f, 160f, 0f));
				break;
			}
		}

		private void _startAerialCombatCutIn()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_041);
			if (_iType == CutInType.Both || _iType == CutInType.FriendOnly)
			{
				KCV.Battle.Utils.ShipUtils.PlayAircraftCutInVoice(_koukuuModel.GetCaptainShip(is_friend: true));
				_changeSeaWave(FleetType.Friend);
			}
			else if (_iType == CutInType.EnemyOnly)
			{
				_changeSeaWave(FleetType.Enemy);
			}
		}

		public void _playCloudParticle()
		{
			_cloudParticle.Play();
			BattleField battleField = BattleTaskManager.GetBattleField();
			battleField.ResetFleetAnchorPosition();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
			battleShips.SetStandingPosition(StandingPositionType.OneRow);
			battleShips.RadarDeployment(isDeploy: false);
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.SwitchMainCamera(FleetType.Friend);
			battleCameras.InitEnemyFieldCameraDefault();
			BattleTaskManager.GetPrefabFile().DisposeProdCommandBuffer();
		}

		private void _playSE(int id)
		{
			if (id == 0)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_048);
			}
		}

		public void _cutinAnimationFinished()
		{
			if (_iType == CutInType.FriendOnly || _iType == CutInType.EnemyOnly)
			{
				_startAntiAerialCutIn();
			}
			else if (_iType == CutInType.Both)
			{
				if (!_isAnimeFinished)
				{
					_setCutin(CutInType.EnemyOnly);
					Animation component = ((Component)base.transform).GetComponent<Animation>();
					component.Play("AerialSecondCutIn2");
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_041);
				}
				else
				{
					_startAntiAerialCutIn();
				}
			}
		}

		public void _startAntiAerialCutIn()
		{
			if (_koukuuModel.GetTaikuShip(is_friend: true) != null)
			{
				if (_prodAntiAerialCutIn == null)
				{
					BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
					_prodAntiAerialCutIn = ProdAntiAerialCutIn.Instantiate(Resources.Load<ProdAntiAerialCutIn>("Prefabs/Battle/Production/AerialCombat/ProdAntiAerialCutIn"), _koukuuModel, cutInEffectCamera.transform);
				}
				_prodAntiAerialCutIn.Play(_compAntiAerialCutInEnemy, isFriend: true);
			}
			else if (_koukuuModel.GetTaikuShip(is_friend: false) != null)
			{
				if (_prodAntiAerialCutIn == null)
				{
					BattleCutInEffectCamera cutInEffectCamera2 = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
					_prodAntiAerialCutIn = ProdAntiAerialCutIn.Instantiate(Resources.Load<ProdAntiAerialCutIn>("Prefabs/Battle/Production/AerialCombat/ProdAntiAerialCutIn"), _koukuuModel, cutInEffectCamera2.transform);
				}
				_prodAntiAerialCutIn.Play(_compAntiAerialCutInEnemy, isFriend: false);
			}
			else
			{
				_compAntiAerialCutInEnemy();
			}
		}

		private void _compAntiAerialCutInFriend()
		{
			if (_koukuuModel.GetTaikuShip(is_friend: false) != null)
			{
				if (_prodAntiAerialCutIn == null)
				{
					BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
					_prodAntiAerialCutIn = ProdAntiAerialCutIn.Instantiate(Resources.Load<ProdAntiAerialCutIn>("Prefabs/Battle/Production/AerialCombat/ProdAntiAerialCutIn"), _koukuuModel, cutInEffectCamera.transform);
				}
				_prodAntiAerialCutIn.Play(_compAntiAerialCutInEnemy, isFriend: false);
			}
			else
			{
				_compAntiAerialCutInEnemy();
			}
		}

		private void _compAntiAerialCutInEnemy()
		{
			if (_actCallback != null)
			{
				_actCallback();
			}
		}
	}
}
