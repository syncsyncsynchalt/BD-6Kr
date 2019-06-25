using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.managers;
using local.models;
using local.models.battle;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialCombatCutinP : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] _uiAirObj;

		[SerializeField]
		private GameObject _uiShipObj;

		[SerializeField]
		private ParticleSystem _cloudParticle;

		[SerializeField]
		private UITexture[] _uiAircraft;

		[SerializeField]
		public UITexture _uiShip;

		private Animation[] _airAnimation;

		private bool isAnimeFinished;

		private bool _isPlaying;

		private Action _actCallback;

		private CutInType _iType;

		private KoukuuModel _koukuuModel;

		private BattleFieldCamera _camFieldCamera;

		private ProdAntiAerialCutIn _prodAntiAerialCutIn;

		private bool _init()
		{
			isAnimeFinished = false;
			_isPlaying = false;
			_actCallback = null;
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
				Util.FindParentToChild(ref _uiAircraft[i], _uiAirObj[i].transform, "Swing/Aircraft");
				_airAnimation[i] = _uiAircraft[i].GetComponent<Animation>();
			}
			if (_uiShipObj == null)
			{
				_uiShipObj = base.transform.FindChild("ShipObj").gameObject;
			}
			Util.FindParentToChild(ref _uiShip, _uiShipObj.transform, "Ship");
			Util.FindParentToChild<ParticleSystem>(ref _cloudParticle, base.transform, "Cloud");
			_camFieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			_camFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			return true;
		}

		private void OnDestroy()
		{
			isAnimeFinished = false;
			_isPlaying = false;
			if (_prodAntiAerialCutIn != null)
			{
				UnityEngine.Object.Destroy(_prodAntiAerialCutIn.gameObject);
			}
			_prodAntiAerialCutIn = null;
			Mem.DelArySafe(ref _uiAirObj);
			Mem.Del(ref _uiShipObj);
			Mem.Del(ref _cloudParticle);
			Mem.DelArySafe(ref _uiAircraft);
			Mem.Del(ref _uiShip);
			Mem.DelArySafe(ref _airAnimation);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _iType);
			Mem.Del(ref _koukuuModel);
			Mem.Del(ref _camFieldCamera);
		}

		public static ProdAerialCombatCutinP Instantiate(ProdAerialCombatCutinP prefab, KoukuuModel model, Transform parent)
		{
			ProdAerialCombatCutinP prodAerialCombatCutinP = UnityEngine.Object.Instantiate(prefab);
			prodAerialCombatCutinP.transform.parent = parent;
			prodAerialCombatCutinP.transform.localPosition = Vector3.zero;
			prodAerialCombatCutinP.transform.localScale = Vector3.one;
			prodAerialCombatCutinP._koukuuModel = model;
			prodAerialCombatCutinP._init();
			return prodAerialCombatCutinP;
		}

		private void _setShipTexture(bool isFriend)
		{
			ShipModel_Attacker shipModel_Attacker = (_koukuuModel.GetCaptainShip(isFriend) == null) ? null : _koukuuModel.GetCaptainShip(isFriend);
			if (shipModel_Attacker != null)
			{
				_uiShip.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(shipModel_Attacker);
				_uiShip.MakePixelPerfect();
				_uiShip.flip = ((!isFriend) ? UIBasicSprite.Flip.Horizontally : UIBasicSprite.Flip.Nothing);
				Vector3 shipOffsPos = KCV.Battle.Utils.ShipUtils.GetShipOffsPos(shipModel_Attacker, shipModel_Attacker.DamagedFlg, MstShipGraphColumn.CutInSp1);
				_uiShip.transform.localPosition = ((!isFriend) ? new Vector3(shipOffsPos.x * -1f, shipOffsPos.y, shipOffsPos.z) : shipOffsPos);
			}
			else
			{
				_uiShip.mainTexture = null;
			}
		}

		public void Play(Action callback)
		{
			_isPlaying = true;
			_actCallback = callback;
			GetComponent<UIPanel>().widgetsAreStatic = false;
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
			_uiShip.mainTexture = null;
			for (int i = 0; i < 3; i++)
			{
				_uiAircraft[i].mainTexture = null;
			}
			switch (type)
			{
			case CutInType.FriendOnly:
			case CutInType.Both:
				_setShipTexture(isFriend: true);
				_setAircraftTexture(FleetType.Friend);
				break;
			case CutInType.EnemyOnly:
				_setShipTexture(isFriend: false);
				_setAircraftTexture(FleetType.Enemy);
				_changeSeaWave(isFriend: false);
				base.transform.localEulerAngles = new Vector3(0f, -180f, 0f);
				isAnimeFinished = true;
				break;
			}
			for (int j = 0; j < 3; j++)
			{
				_airAnimation[j].Stop();
				_airAnimation[j].Play("AircraftCutin" + (j + 1));
			}
			Animation component = ((Component)base.transform).GetComponent<Animation>();
			component.Stop();
			component.Play("AircraftCutinP_1");
		}

		private void _setAircraftTexture(FleetType type)
		{
			switch (type)
			{
			case FleetType.Friend:
				_setAircraftTexture(isFriend: true);
				break;
			case FleetType.Enemy:
				_setAircraftTexture(isFriend: false);
				break;
			}
		}

		private void _setAircraftTexture(bool isFriend)
		{
			if (_koukuuModel.GetCaptainShip(isFriend) == null)
			{
				return;
			}
			PlaneModelBase[] plane = _koukuuModel.GetPlane(_koukuuModel.GetCaptainShip(isFriend).TmpId);
			if (plane == null)
			{
				return;
			}
			for (int i = 0; i < plane.Length && i < 3; i++)
			{
				if (plane[i] != null)
				{
					if (!isFriend && BattleTaskManager.GetBattleManager() is PracticeBattleManager)
					{
						_uiAirObj[i].transform.localPosition = new Vector3(267f, 176f, 0f);
						_uiAirObj[i].transform.localScale = Vector3.one;
						_uiAircraft[i].mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(plane[i].MstId, 6);
						_uiAirObj[i].transform.localEulerAngles = new Vector3(0f, 0f, -25.5f);
						_uiAircraft[i].flip = UIBasicSprite.Flip.Nothing;
						continue;
					}
					_uiAirObj[i].transform.localPosition = new Vector3(267f, 176f, 0f);
					_uiAircraft[i].mainTexture = ((!isFriend) ? KCV.Battle.Utils.SlotItemUtils.LoadTexture(plane[i]) : SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(plane[i].MstId, 6));
					_uiAirObj[i].transform.localEulerAngles = ((!isFriend) ? Vector3.zero : new Vector3(0f, 0f, -25.5f));
					if (!isFriend)
					{
						_uiAircraft[i].MakePixelPerfect();
						_uiAirObj[i].transform.localScale = ((plane[i].MstId < 500) ? Vector3.one : new Vector3(0.8f, 0.8f, 0.8f));
						AircraftOffsetInfo aircraftOffsetInfo = KCV.Battle.Utils.SlotItemUtils.GetAircraftOffsetInfo(plane[i].MstId);
						_uiAircraft[i].flip = ((!aircraftOffsetInfo.isFlipHorizontal) ? UIBasicSprite.Flip.Horizontally : UIBasicSprite.Flip.Nothing);
					}
				}
				else
				{
					_uiAircraft[i].mainTexture = null;
				}
			}
		}

		private void _changeSeaWave(bool isFriend)
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			battleField.seaLevel.waveSpeed = ((!isFriend) ? new Vector4(-4f, 500f, 5f, 400f) : new Vector4(-4f, -500f, 5f, -400f));
			_camFieldCamera.transform.localPosition = ((!isFriend) ? new Vector3(300f, 4f, 100f) : new Vector3(300f, 4f, 100f));
			_camFieldCamera.transform.rotation = ((!isFriend) ? Quaternion.Euler(new Vector3(22f, 160f, 0f)) : Quaternion.Euler(new Vector3(22f, 20f, 0f)));
		}

		private void _startAerialCombatCutIn()
		{
			_playSE(2);
			if (_iType == CutInType.Both || _iType == CutInType.FriendOnly)
			{
				KCV.Battle.Utils.ShipUtils.PlayAircraftCutInVoice(_koukuuModel.GetCaptainShip(is_friend: true));
				_changeSeaWave(isFriend: true);
			}
			else if (_iType == CutInType.EnemyOnly)
			{
				_changeSeaWave(isFriend: false);
			}
		}

		public void _playCloudParticle()
		{
			_cloudParticle.Play();
			BattleTaskManager.GetBattleField().ResetFleetAnchorPosition();
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
			switch (id)
			{
			case 1:
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_048);
				break;
			case 2:
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_041);
				break;
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
				if (!isAnimeFinished)
				{
					_setCutin(CutInType.EnemyOnly);
					Animation component = ((Component)base.transform).GetComponent<Animation>();
					component.Stop();
					component.Play("AircraftCutinP_2");
					_playSE(2);
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
				BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
				_prodAntiAerialCutIn = ProdAntiAerialCutIn.Instantiate(Resources.Load<ProdAntiAerialCutIn>("Prefabs/Battle/Production/AerialCombat/ProdAntiAerialCutIn"), _koukuuModel, cutInEffectCamera.transform);
				_prodAntiAerialCutIn.Play(_compAntiAerialCutInEnemy, isFriend: true);
			}
			else if (_koukuuModel.GetTaikuShip(is_friend: true) != null)
			{
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
					_prodAntiAerialCutIn = ProdAntiAerialCutIn.Instantiate(Resources.Load<ProdAntiAerialCutIn>("Prefabs/Battle/Production/AerialCombat/ProdAntiAerialCutIn"), _koukuuModel, BattleTaskManager.GetBattleCameras().cutInEffectCamera.transform);
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
