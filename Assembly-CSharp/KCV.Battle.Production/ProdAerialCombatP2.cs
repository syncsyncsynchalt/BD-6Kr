using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.managers;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialCombatP2 : MonoBehaviour
	{
		private enum AttackState
		{
			FriendBomb,
			FriendRaigeki,
			FriendExplosion,
			EnemyBomb,
			EnemyRaigeki,
			EnemyExplosion,
			End,
			None
		}

		private enum HitType
		{
			Bomb,
			Torpedo,
			Miss,
			None
		}

		private AttackState _attackState;

		[SerializeField]
		private GameObject _mainObj;

		[SerializeField]
		private GameObject[] _aircraftObj;

		private ProdAerialRescueCutIn _rescueCutIn;

		private BattleFieldCamera _camAerial;

		private bool _isEx;

		private bool _isPlaying;

		private bool[] _isProtect;

		private Animation _anime;

		private Action _actCallback;

		private KoukuuModel _clsKoukuu;

		private List<bool> _listBombCritical;

		private List<ParticleSystem> _listExplosion;

		private List<ParticleSystem> _listMiss;

		private Dictionary<int, UIBattleShip> _fBattleship;

		private Dictionary<int, UIBattleShip> _eBattleship;

		private Dictionary<FleetType, HitType[]> _dicHitType;

		private Dictionary<FleetType, HpHitState[]> _dicHitState;

		private Dictionary<FleetType, BakuRaiDamageModel[]> _dicBakuraiModel;

		private BattleHPGauges _battleHpGauges;

		private float _explosionTime;

		private PSTorpedoWakes _torpedoWakes;

		public IEnumerator _init()
		{
			_explosionTime = 0f;
			_isEx = false;
			_isProtect = new bool[2];
			_camAerial = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			_camAerial.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			if (_mainObj == null)
			{
				_mainObj = base.transform.FindChild("Aircraft").gameObject;
			}
			if ((UnityEngine.Object)_anime == null)
			{
				_anime = ((Component)base.transform).GetComponent<Animation>();
			}
			_aircraftObj = new GameObject[3];
			for (int i = 0; i < 3; i++)
			{
				_aircraftObj[i] = _mainObj.transform.FindChild("Aircraft" + (3 - i)).gameObject;
			}
			if (_rescueCutIn == null)
			{
				_rescueCutIn = GetComponent<ProdAerialRescueCutIn>();
			}
			_rescueCutIn._init();
			yield return null;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _attackState);
			Mem.Del(ref _mainObj);
			Mem.DelArySafe(ref _aircraftObj);
			Mem.Del(ref _camAerial);
			Mem.DelArySafe(ref _isProtect);
			Mem.Del(ref _anime);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _clsKoukuu);
			Mem.DelDictionarySafe(ref _dicBakuraiModel);
			Mem.DelListSafe(ref _listBombCritical);
			Mem.DelListSafe(ref _listExplosion);
			Mem.DelListSafe(ref _listMiss);
			Mem.DelDictionarySafe(ref _dicHitType);
			Mem.DelDictionarySafe(ref _dicHitState);
			Mem.Del(ref _battleHpGauges);
			if (_torpedoWakes != null)
			{
				_torpedoWakes.SetDestroy();
			}
			if (_rescueCutIn != null)
			{
				UnityEngine.Object.Destroy(_rescueCutIn.gameObject);
			}
			Mem.Del(ref _rescueCutIn);
		}

		public static ProdAerialCombatP2 Instantiate(ProdAerialCombatP2 prefab, KoukuuModel model, Transform parent)
		{
			ProdAerialCombatP2 prodAerialCombatP = UnityEngine.Object.Instantiate(prefab);
			prodAerialCombatP.transform.parent = parent;
			prodAerialCombatP.transform.localPosition = Vector3.zero;
			prodAerialCombatP.transform.localScale = Vector3.one;
			prodAerialCombatP._clsKoukuu = model;
			prodAerialCombatP.StartCoroutine(prodAerialCombatP._init());
			return prodAerialCombatP;
		}

		private void _destroyHPGauge()
		{
			if (_battleHpGauges != null)
			{
				_battleHpGauges.Dispose();
			}
		}

		public void CreateHpGauge(FleetType type)
		{
			if (_battleHpGauges == null)
			{
				_battleHpGauges = new BattleHPGauges();
			}
			for (int i = 0; i < 6; i++)
			{
				_battleHpGauges.AddInstantiates(base.gameObject, isFriend: true, isLight: true, isT: false, isNumber: false);
			}
		}

		private void _initParticleList()
		{
			if (_torpedoWakes != null)
			{
				_torpedoWakes.Dispose();
			}
			if (_listBombCritical != null)
			{
				_listBombCritical.Clear();
			}
			_listBombCritical = null;
			if (_listExplosion != null)
			{
				foreach (ParticleSystem item in _listExplosion)
				{
					UnityEngine.Object.Destroy(((Component)item).gameObject);
				}
				_listExplosion.Clear();
			}
			_listExplosion = null;
			if (_listMiss != null)
			{
				foreach (ParticleSystem item2 in _listMiss)
				{
					UnityEngine.Object.Destroy(((Component)item2).gameObject);
				}
				_listMiss.Clear();
			}
			_listMiss = null;
		}

		public bool Update()
		{
			if ((_attackState == AttackState.FriendExplosion && _isEx) || (_attackState == AttackState.EnemyExplosion && _isEx))
			{
				_explosionTime += Time.deltaTime;
				if (_explosionTime > 3f)
				{
					_onFinishedCutIn();
					_changeState();
					_setState();
					_explosionTime = 0f;
					_isEx = false;
				}
			}
			return true;
		}

		private static CutInType _chkCutInType(KoukuuModel model)
		{
			if (model.GetCaptainShip(is_friend: true) != null && model.GetCaptainShip(is_friend: false) != null)
			{
				return CutInType.Both;
			}
			if (model.GetCaptainShip(is_friend: true) != null)
			{
				return CutInType.FriendOnly;
			}
			return CutInType.EnemyOnly;
		}

		public void Play(Action callback, Dictionary<int, UIBattleShip> fBattleShips, Dictionary<int, UIBattleShip> eBattleShips)
		{
			GetComponent<UIPanel>().widgetsAreStatic = false;
			_isPlaying = true;
			_actCallback = callback;
			_attackState = AttackState.None;
			_fBattleship = fBattleShips;
			_eBattleship = eBattleShips;
			_dicBakuraiModel = new Dictionary<FleetType, BakuRaiDamageModel[]>();
			_dicBakuraiModel.Add(FleetType.Friend, _clsKoukuu.GetRaigekiData_e());
			_dicBakuraiModel.Add(FleetType.Enemy, _clsKoukuu.GetRaigekiData_f());
			Transform transform = _camAerial.transform;
			Vector3 localPosition = _camAerial.transform.localPosition;
			transform.localPosition = new Vector3(0f, localPosition.y, 90f);
			HpHitState[] array = new HpHitState[_dicBakuraiModel[FleetType.Friend].Length];
			HpHitState[] array2 = new HpHitState[_dicBakuraiModel[FleetType.Enemy].Length];
			HitType[] array3 = new HitType[_dicBakuraiModel[FleetType.Friend].Length];
			HitType[] array4 = new HitType[_dicBakuraiModel[FleetType.Enemy].Length];
			for (int i = 0; i < _dicBakuraiModel[FleetType.Friend].Length; i++)
			{
				array[i] = HpHitState.None;
				array3[i] = HitType.None;
			}
			for (int j = 0; j < _dicBakuraiModel[FleetType.Enemy].Length; j++)
			{
				array2[j] = HpHitState.None;
				array4[j] = HitType.None;
			}
			_dicHitType = new Dictionary<FleetType, HitType[]>();
			_dicHitType.Add(FleetType.Friend, array3);
			_dicHitType.Add(FleetType.Enemy, array4);
			_dicHitState = new Dictionary<FleetType, HpHitState[]>();
			_dicHitState.Add(FleetType.Friend, array);
			_dicHitState.Add(FleetType.Enemy, array2);
			_changeState();
			_setState();
		}

		private void _changeState()
		{
			switch (_attackState)
			{
			case AttackState.End:
				break;
			case AttackState.FriendBomb:
				_attackState = (_clsKoukuu.IsRaigeki_e() ? AttackState.FriendRaigeki : AttackState.FriendExplosion);
				break;
			case AttackState.FriendRaigeki:
				_attackState = AttackState.FriendExplosion;
				break;
			case AttackState.FriendExplosion:
				if (_clsKoukuu.IsBakugeki_f())
				{
					_attackState = AttackState.EnemyBomb;
				}
				else if (_clsKoukuu.IsRaigeki_f())
				{
					_attackState = AttackState.EnemyRaigeki;
				}
				else
				{
					_attackState = AttackState.End;
				}
				break;
			case AttackState.EnemyBomb:
				_attackState = ((!_clsKoukuu.IsRaigeki_f()) ? AttackState.EnemyExplosion : AttackState.EnemyRaigeki);
				break;
			case AttackState.EnemyRaigeki:
				_attackState = AttackState.EnemyExplosion;
				break;
			case AttackState.EnemyExplosion:
				_attackState = AttackState.End;
				break;
			case AttackState.None:
				if (_clsKoukuu.IsBakugeki_e())
				{
					_attackState = AttackState.FriendBomb;
				}
				else if (_clsKoukuu.IsRaigeki_e())
				{
					_attackState = AttackState.FriendRaigeki;
				}
				else if (_clsKoukuu.IsBakugeki_f())
				{
					_attackState = AttackState.EnemyBomb;
				}
				else if (_clsKoukuu.IsRaigeki_f())
				{
					_attackState = AttackState.EnemyRaigeki;
				}
				else
				{
					_attackState = AttackState.End;
				}
				break;
			}
		}

		private void _setState()
		{
			if (_attackState == AttackState.FriendBomb)
			{
				_startState("Bomb", FleetType.Friend);
			}
			else if (_attackState == AttackState.FriendRaigeki)
			{
				_startState("Torpedo", FleetType.Friend);
			}
			else if (_attackState == AttackState.FriendExplosion)
			{
				_startState("Explosion", FleetType.Friend);
			}
			else if (_attackState == AttackState.EnemyBomb)
			{
				_startState("Bomb", FleetType.Enemy);
			}
			else if (_attackState == AttackState.EnemyRaigeki)
			{
				_startState("Torpedo", FleetType.Enemy);
			}
			else if (_attackState == AttackState.EnemyExplosion)
			{
				_startState("Explosion", FleetType.Enemy);
			}
			else if (_attackState == AttackState.End)
			{
				_startState("End", FleetType.Friend);
			}
		}

		private void _startState(string type, FleetType fleetType)
		{
			switch (type)
			{
			case "Bomb":
			{
				_battleHpGauges.Init();
				_viewAircrafts(fleetType, _attackState, _dicBakuraiModel[fleetType]);
				_camAerial.transform.localPosition = BattleDefines.AERIAL_BOMB_CAM_POSITION[(int)fleetType];
				_camAerial.transform.rotation = BattleDefines.AERIAL_BOMB_CAM_ROTATION[(int)fleetType];
				base.transform.localEulerAngles = BattleDefines.AERIAL_BOMB_TRANS_ANGLE[(int)fleetType];
				Hashtable hashtable = new Hashtable();
				hashtable.Add("rotation", (fleetType != 0) ? new Vector3(-13.5f, -95f, 0f) : new Vector3(-13.5f, 95f, 0f));
				hashtable.Add("isLocal", true);
				hashtable.Add("time", 1.35f);
				hashtable.Add("easeType", iTween.EaseType.linear);
				_camAerial.gameObject.RotateTo(hashtable);
				_anime.Stop();
				_anime.Play("AerialStartPhase2_1");
				((Component)base.transform.FindChild("CloudPanel")).GetComponent<Animation>().Play();
				break;
			}
			case "Torpedo":
				_battleHpGauges.Init();
				_initParticleList();
				BattleTaskManager.GetBattleField().seaLevel.waveSpeed = BattleDefines.AERIAL_TORPEDO_WAVESPEED[(int)fleetType];
				_viewAircrafts(fleetType, _attackState, _dicBakuraiModel[fleetType]);
				_createTorpedoWake(fleetType);
				_camAerial.transform.localPosition = BattleDefines.AERIAL_TORPEDO_CAM_POSITION[(int)fleetType];
				_camAerial.transform.rotation = BattleDefines.AERIAL_TORPEDO_CAM_ROTATION[(int)fleetType];
				base.transform.localEulerAngles = BattleDefines.AERIAL_BOMB_TRANS_ANGLE[(int)fleetType];
				_anime.Stop();
				_anime.Play("AerialStartPhase2_2");
				break;
			case "Explosion":
				_setHpGauge(fleetType);
				_moveExplosionCamera();
				break;
			case "End":
				_aerialCombatPhase1Finished();
				break;
			}
		}

		private void _viewAircrafts(FleetType type, AttackState attack, BakuRaiDamageModel[] model)
		{
			switch (attack)
			{
			case AttackState.FriendBomb:
			case AttackState.EnemyBomb:
				_viewAircraft(type, isBomb: true, model);
				break;
			case AttackState.FriendRaigeki:
			case AttackState.EnemyRaigeki:
				_viewAircraft(type, isBomb: false, model);
				break;
			}
		}

		private void _viewAircraft(FleetType type, bool isBomb, BakuRaiDamageModel[] model)
		{
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				_aircraftObj[i].SetActive(false);
			}
			for (int j = 0; j < model.Length; j++)
			{
				if (num >= 3)
				{
					break;
				}
				if (model[j] != null && ((!isBomb) ? model[j].IsRaigeki() : model[j].IsBakugeki()))
				{
					_aircraftObj[num].SetActive(true);
					UITexture component = ((Component)_aircraftObj[num].transform.FindChild("Aircraft")).GetComponent<UITexture>();
					setAircraftTexture(type, isBomb, component, num);
					num++;
				}
			}
		}

		private void setAircraftTexture(FleetType type, bool isBomb, UITexture tex, int num)
		{
			bool is_friend = (type == FleetType.Friend) ? true : false;
			SlotitemModel_Battle[] array = (!isBomb) ? _clsKoukuu.GetRaigekiPlanes(is_friend) : _clsKoukuu.GetBakugekiPlanes(is_friend);
			int num2 = (array.Length <= 3) ? array.Length : 3;
			if (num2 < num + 1)
			{
				return;
			}
			switch (type)
			{
			case FleetType.Friend:
				tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(array[num].MstId, 6);
				break;
			case FleetType.Enemy:
				if (BattleTaskManager.GetBattleManager() is PracticeBattleManager)
				{
					tex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(array[num].MstId, 6);
					break;
				}
				tex.mainTexture = KCV.Battle.Utils.SlotItemUtils.LoadTexture(array[num]);
				tex.flip = UIBasicSprite.Flip.Horizontally;
				tex.MakePixelPerfect();
				tex.transform.localScale = ((array[num].MstId > 500) ? new Vector3(0.8f, 0.8f, 0.8f) : new Vector3(1f, 1f, 1f));
				break;
			}
		}

		private void _moveExplosionCamera()
		{
			bool flag = (_attackState == AttackState.FriendExplosion) ? true : false;
			_camAerial.transform.localPosition = ((!flag) ? new Vector3(0f, 7.5f, -35f) : new Vector3(0f, 7.5f, 35f));
			_camAerial.transform.rotation = ((!flag) ? Quaternion.Euler(new Vector3(0f, 0f, 0f)) : Quaternion.Euler(new Vector3(0f, 180f, 0f)));
			base.transform.rotation = ((!flag) ? Quaternion.Euler(new Vector3(0f, 0f, 0f)) : Quaternion.Euler(new Vector3(0f, 0f, 0f)));
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", (!flag) ? new Vector3(0f, 7.5f, 40f) : new Vector3(0f, 7.5f, -40f));
			hashtable.Add("isLocal", false);
			hashtable.Add("time", 0.9f);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("oncomplete", "_playRescueCutIn");
			hashtable.Add("oncompletetarget", base.gameObject);
			_camAerial.gameObject.MoveTo(hashtable);
		}

		private void _playRescueCutIn()
		{
			if (_attackState == AttackState.FriendExplosion && _isProtect[1])
			{
				Transform transform = _camAerial.transform;
				Vector3 position = _rescueCutIn._listBattleShip[0].transform.position;
				transform.localPosition = new Vector3(position.x, 7.5f, -40f);
				_rescueCutIn.Play(_onFinishedRescueCutIn);
			}
			else if (_attackState == AttackState.EnemyExplosion && _isProtect[0])
			{
				Transform transform2 = _camAerial.transform;
				Vector3 position2 = _rescueCutIn._listBattleShip[0].transform.position;
				transform2.localPosition = new Vector3(position2.x, 7.5f, 40f);
				_rescueCutIn.Play(_onFinishedRescueCutIn);
			}
			else
			{
				_onFinishedRescueCutIn();
			}
		}

		private void _onFinishedRescueCutIn()
		{
			if (_attackState == AttackState.FriendExplosion)
			{
				_camAerial.transform.localPosition = new Vector3(0f, 7.5f, -40f);
				_camAerial.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
			}
			else if (_attackState == AttackState.EnemyExplosion)
			{
				_camAerial.transform.localPosition = new Vector3(0f, 7.5f, 40f);
				_camAerial.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			}
			_playHitAnimation();
			_isEx = true;
		}

		private void _onFinishedCutIn()
		{
			if (_torpedoWakes != null)
			{
				_torpedoWakes.ReStartAll();
			}
		}

		private void _playHitAnimation()
		{
			BattleTaskManager.GetBattleField().AlterWaveDirection(FleetType.Friend);
			if (_torpedoWakes != null)
			{
				_torpedoWakes.PlaySplashAll();
			}
			_camAerial.cameraShake.ShakeRot(null);
			_setExplotion();
		}

		private void _setProtect(FleetType type)
		{
			int num = (type == FleetType.Friend) ? 1 : 0;
			int num2 = 0;
			while (true)
			{
				if (num2 < _dicBakuraiModel[type].Length)
				{
					if (_dicBakuraiModel[type][num2] != null && _dicBakuraiModel[type][num2].GetProtectEffect())
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			_isProtect[num] = true;
			ShipModel_Defender defender = _dicBakuraiModel[type][num2].Defender;
			if (type == FleetType.Friend)
			{
				_rescueCutIn.AddShipList(_eBattleship[0], _eBattleship[defender.Index]);
			}
			else
			{
				_rescueCutIn.AddShipList(_fBattleship[0], _fBattleship[defender.Index]);
			}
		}

		private void _createTorpedoWake(FleetType type)
		{
			_torpedoWakes = new PSTorpedoWakes();
			_setProtect(type);
			_createTorupedoWakes((type == FleetType.Friend) ? true : false);
		}

		private void _createTorupedoWakes(bool isFriend)
		{
			int num = 0;
			FleetType key = (!isFriend) ? FleetType.Enemy : FleetType.Friend;
			FleetType fleetType = isFriend ? FleetType.Enemy : FleetType.Friend;
			_torpedoWakes.InitProtectVector();
			for (int i = 0; i < _dicBakuraiModel[key].Length; i++)
			{
				if (_dicBakuraiModel[key][i] != null && _dicBakuraiModel[key][i].IsRaigeki())
				{
					ShipModel_Defender defender = _dicBakuraiModel[key][i].Defender;
					int num2 = (!_dicBakuraiModel[key][i].GetProtectEffect()) ? defender.Index : 0;
					bool flag = (_dicBakuraiModel[key][i].GetHitState() == BattleHitStatus.Miss) ? true : false;
					float num3 = (!flag) ? 0f : 1f;
					Vector3 injectionVec = _setTorpedoVec(num, isFriend);
					num++;
					_dicHitType[fleetType][num2] = setHitType(fleetType, defender.Index, flag, HitType.Torpedo);
					Vector3 vector;
					if (isFriend)
					{
						Vector3 position = _eBattleship[num2].transform.position;
						float x = position.x + num3;
						Vector3 position2 = _eBattleship[num2].transform.position;
						vector = new Vector3(x, 0f, position2.z + 1f);
					}
					else
					{
						Vector3 position3 = _fBattleship[num2].transform.position;
						float x2 = position3.x + num3;
						Vector3 position4 = _fBattleship[num2].transform.position;
						vector = new Vector3(x2, 0f, position4.z - 1f);
					}
					Vector3 targetVec = vector;
					if (_dicBakuraiModel[key][i].GetProtectEffect())
					{
						Vector3 vector2 = (!isFriend) ? _fBattleship[defender.Index].transform.position : _eBattleship[defender.Index].transform.position;
						_torpedoWakes.SetProtectVector(num, (!isFriend) ? new Vector3(vector2.x - 2f, 0f, vector2.z) : new Vector3(vector2.x + 2f, 0f, vector2.z));
					}
					_torpedoWakes.AddInstantiates(BattleTaskManager.GetBattleField().transform, injectionVec, targetVec, isFull: true, i, 2f, isDet: false, flag);
				}
			}
		}

		private Vector3 _setTorpedoVec(int count, bool isFriend)
		{
			int num = (count < 3) ? count : (count - 3);
			return (!isFriend) ? BattleDefines.AERIAL_ENEMY_TORPEDO_POS[num] : BattleDefines.AERIAL_FRIEND_TORPEDO_POS[num];
		}

		private HitType setHitType(FleetType fleetType, int index, bool miss, HitType setType)
		{
			switch (_dicHitType[fleetType][index])
			{
			case HitType.None:
				return (!miss) ? setType : HitType.Miss;
			case HitType.Miss:
				return (!miss) ? setType : HitType.Miss;
			case HitType.Torpedo:
				return HitType.Torpedo;
			case HitType.Bomb:
				if (setType == HitType.Torpedo)
				{
					return (!miss) ? setType : HitType.Bomb;
				}
				return HitType.Bomb;
			default:
				return HitType.None;
			}
		}

		private void _setHpGauge(FleetType type)
		{
			int num = (type != 0) ? _fBattleship.Count : _eBattleship.Count;
			bool flag = (type == FleetType.Friend) ? true : false;
			FleetType key = flag ? FleetType.Enemy : FleetType.Friend;
			List<ShipModel_Defender> defenders = _clsKoukuu.GetDefenders((!flag) ? true : false, all: true);
			for (int i = 0; i < _dicBakuraiModel[type].Length; i++)
			{
				if (_dicBakuraiModel[type][i] == null)
				{
					continue;
				}
				ShipModel_Defender defender = _dicBakuraiModel[type][i].Defender;
				switch (_dicBakuraiModel[type][i].GetHitState())
				{
				case BattleHitStatus.Normal:
					if (_dicHitState[key][defender.Index] != HpHitState.Critical)
					{
						_dicHitState[key][defender.Index] = HpHitState.Hit;
					}
					break;
				case BattleHitStatus.Clitical:
					_dicHitState[key][defender.Index] = HpHitState.Critical;
					break;
				case BattleHitStatus.Miss:
					if (_dicHitState[key][defender.Index] == HpHitState.None)
					{
						_dicHitState[key][defender.Index] = HpHitState.Miss;
					}
					break;
				}
			}
			for (int j = 0; j < num; j++)
			{
				int damage = _clsKoukuu.GetAttackDamage(defenders[j].TmpId)?.GetDamage() ?? (-1);
				BattleHitStatus status = (_dicHitState[key][j] != HpHitState.Miss) ? ((_dicHitState[key][j] != HpHitState.Critical) ? BattleHitStatus.Normal : BattleHitStatus.Clitical) : BattleHitStatus.Miss;
				_battleHpGauges.Init();
				_battleHpGauges.SetGauge(j, flag, isLight: true, isT: false, isNumber: false);
				if (flag)
				{
					_battleHpGauges.SetHp(j, defenders[j].MaxHp, defenders[j].HpBefore, defenders[j].HpAfter, damage, status, isFriend: false);
				}
				else
				{
					_battleHpGauges.SetHp(j, defenders[j].MaxHp, defenders[j].HpBefore, defenders[j].HpAfter, damage, status, isFriend: false);
				}
			}
		}

		private Vector3[] _setHpPosition(FleetType type, int count)
		{
			Vector3[] array = new Vector3[count];
			int num = (type != 0) ? 1 : (-1);
			int num2 = (type == FleetType.Friend) ? 1 : (-1);
			switch (count)
			{
			case 1:
				array[0] = new Vector3(0f, 100f, 0f);
				break;
			case 2:
				for (int m = 0; m < 2; m++)
				{
					array[m] = new Vector3(80f * (float)num + 160f * (float)m * (float)num2, 100f, 0f);
				}
				break;
			case 3:
				for (int j = 0; j < 3; j++)
				{
					array[j] = new Vector3(165f * (float)num + 160f * (float)j * (float)num2, 100f, 0f);
				}
				break;
			case 4:
				for (int l = 0; l < 4; l++)
				{
					array[l] = new Vector3(245f * (float)num + 160f * (float)l * (float)num2, 100f, 0f);
				}
				break;
			case 5:
				for (int k = 0; k < 5; k++)
				{
					array[k] = new Vector3(320f * (float)num + 160f * (float)k * (float)num2, 100f, 0f);
				}
				break;
			case 6:
				for (int i = 0; i < 6; i++)
				{
					array[i] = new Vector3(400f * (float)num + 160f * (float)i * (float)num2, 100f, 0f);
				}
				break;
			}
			return array;
		}

		private void _setShinking(FleetType type)
		{
			if (type == FleetType.Friend)
			{
				BattleTaskManager.GetBattleShips().UpdateDamageAll(_clsKoukuu, isFriend: false);
			}
			for (int i = 0; i < _dicBakuraiModel[type].Length; i++)
			{
				if (_dicBakuraiModel[type][i] == null)
				{
					continue;
				}
				ShipModel_Defender defender = _dicBakuraiModel[type][i].Defender;
				if (defender.DmgStateAfter == DamageState_Battle.Gekichin)
				{
					switch (type)
					{
					case FleetType.Friend:
						_eBattleship[i].PlayProdSinking(null);
						break;
					case FleetType.Enemy:
						_fBattleship[i].PlayProdSinking(null);
						break;
					}
				}
			}
		}

		private void _setExplotion()
		{
			FleetType fleetType = (_attackState != AttackState.FriendExplosion) ? FleetType.Enemy : FleetType.Friend;
			int num = (fleetType != 0) ? _fBattleship.Count : _eBattleship.Count;
			Vector3[] array = _setHpPosition(fleetType, num);
			for (int i = 0; i < num; i++)
			{
				_battleHpGauges.Show(i, array[i], new Vector3(0.35f, 0.35f, 0.35f), isScale: false);
				BattleHPGauges battleHpGauges = _battleHpGauges;
				int num2 = i;
				Vector3 damagePosition = _battleHpGauges.GetDamagePosition(i);
				battleHpGauges.SetDamagePosition(num2, new Vector3(damagePosition.x, -525f, 0f));
				_battleHpGauges.PlayHp(i, null);
			}
			_setShinking(fleetType);
			_createExplotion((fleetType == FleetType.Friend) ? true : false);
			if (_listExplosion != null)
			{
				KCV.Utils.SoundUtils.PlaySE((_listExplosion.Count <= 1) ? SEFIleInfos.SE_930 : SEFIleInfos.SE_931);
				Observable.FromCoroutine(_explosionPlay).Subscribe(delegate
				{
				});
			}
			foreach (ParticleSystem item in _listMiss)
			{
				item.Play();
			}
			_playSE(fleetType);
		}

		private IEnumerator _explosionPlay()
		{
			if (_listExplosion == null)
			{
				yield break;
			}
			for (int i = 0; i < _listExplosion.Count; i++)
			{
				if (!((UnityEngine.Object)_listExplosion[i] == null))
				{
					_listExplosion[i].Play();
					yield return new WaitForSeconds(0.3f);
					if (_listExplosion == null)
					{
						break;
					}
				}
			}
		}

		private void _createExplotion(bool isFriend)
		{
			_listBombCritical = new List<bool>();
			_listExplosion = new List<ParticleSystem>();
			_listMiss = new List<ParticleSystem>();
			FleetType key = (!isFriend) ? FleetType.Enemy : FleetType.Friend;
			if (isFriend)
			{
				Dictionary<int, UIBattleShip> fBattleship = _fBattleship;
			}
			else
			{
				Dictionary<int, UIBattleShip> eBattleship = _eBattleship;
			}
			Dictionary<int, UIBattleShip> dictionary = (!isFriend) ? _fBattleship : _eBattleship;
			Vector3 position3 = default(Vector3);
			for (int i = 0; i < _dicBakuraiModel[key].Length; i++)
			{
				if (_dicBakuraiModel[key][i] == null || !_dicBakuraiModel[key][i].IsBakugeki())
				{
					continue;
				}
				ShipModel_Battle defender = _dicBakuraiModel[key][i].Defender;
				int num = (!_dicBakuraiModel[key][i].GetProtectEffect()) ? defender.Index : defender.Index;
				bool flag = (_dicBakuraiModel[key][i].GetHitState() == BattleHitStatus.Miss) ? true : false;
				FleetType fleetType = isFriend ? FleetType.Enemy : FleetType.Friend;
				_dicHitType[fleetType][num] = setHitType(fleetType, num, flag, HitType.Bomb);
				HitType hitType = (!isFriend) ? _dicHitType[FleetType.Friend][num] : _dicHitType[FleetType.Enemy][num];
				if (!flag)
				{
					if (hitType == HitType.Bomb)
					{
						Vector3 position = dictionary[num].transform.position;
						float x = position.x;
						Vector3 position2 = dictionary[num].transform.position;
						position3 = new Vector3(x, 3f, position2.z);
						ParticleSystem val = (!((UnityEngine.Object)BattleTaskManager.GetParticleFile().explosionAerial == null)) ? UnityEngine.Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().explosionAerial) : BattleTaskManager.GetParticleFile().explosionAerial;
						((Component)val).SetActive(isActive: true);
						((Component)val).transform.parent = BattleTaskManager.GetBattleField().transform;
						((Component)val).transform.position = position3;
						_listExplosion.Add(val);
						_listBombCritical.Add((_dicBakuraiModel[key][i].GetHitState() == BattleHitStatus.Clitical) ? true : false);
					}
				}
				else if (hitType == HitType.Miss)
				{
					Vector3 position4 = dictionary[num].transform.position;
					float fMin = position4.x - 0.5f;
					Vector3 position5 = dictionary[num].transform.position;
					float fLim = XorRandom.GetFLim(fMin, position5.x + 0.5f);
					Vector3 position6 = dictionary[num].transform.position;
					float fMin2 = position6.z - 1f;
					Vector3 position7 = dictionary[num].transform.position;
					float fLim2 = XorRandom.GetFLim(fMin2, position7.z + 1f);
					float num2 = fLim;
					Vector3 position8 = dictionary[num].transform.position;
					fLim = ((!(num2 >= position8.x)) ? (fLim - 0.5f) : (fLim + 0.5f));
					float num3 = fLim2;
					Vector3 position9 = dictionary[num].transform.position;
					fLim2 = ((!(num3 >= position9.z)) ? (fLim2 - 0.5f) : (fLim2 + 0.5f));
					ParticleSystem val2 = (!((UnityEngine.Object)BattleTaskManager.GetParticleFile().splashMiss == null)) ? UnityEngine.Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().splashMiss) : BattleTaskManager.GetParticleFile().splashMiss;
					((Component)val2).SetActive(isActive: true);
					((Component)val2).transform.parent = BattleTaskManager.GetBattleField().transform;
					((Component)val2).transform.position = new Vector3(fLim, 0f, fLim2);
					_listMiss.Add(val2);
				}
			}
		}

		private void _playSE(FleetType fleetType)
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < _dicHitType[fleetType].Length; i++)
			{
				if (_dicHitType[fleetType][i] == HitType.Torpedo)
				{
					flag = true;
				}
				if (_dicHitType[fleetType][i] == HitType.Miss)
				{
					flag2 = true;
				}
			}
			if (flag)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_932);
			}
			if (flag2)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_908);
			}
		}

		private void _playAnimationSE(int num)
		{
			switch (num)
			{
			case 0:
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_920);
				break;
			case 1:
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_918);
				break;
			}
		}

		private void _injectionTrpedo()
		{
			if (_torpedoWakes != null)
			{
				_torpedoWakes.InjectionAll();
			}
		}

		private void _aerialAttackPhase1Finished()
		{
			_changeState();
			_setState();
		}

		private void _aerialAttackPhase2Finished()
		{
			_aerialAttackPhase1Finished();
		}

		private void _aerialCombatPhase2Attack()
		{
			_injectionTrpedo();
		}

		private void _aerialCombatPhase1Finished()
		{
			_destroyHPGauge();
			_initParticleList();
			if (_rescueCutIn != null)
			{
				UnityEngine.Object.Destroy(_rescueCutIn.gameObject);
			}
			_rescueCutIn = null;
			if (_actCallback != null)
			{
				_actCallback();
			}
		}
	}
}
