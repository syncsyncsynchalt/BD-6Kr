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
	public class ProdSupportAerialPhase2 : MonoBehaviour
	{
		private enum AttackState
		{
			FriendBomb,
			FriendRaigeki,
			FriendExplosion,
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

		private bool _isMiss;

		private bool _isProtectE;

		private bool _isPlaying;

		private Vector3[] _eHpPos;

		private Animation _anime;

		private Action _actCallback;

		private ShienModel_Air _clsAerial;

		private BakuRaiDamageModel[] _fBakuraiModel;

		private BakuRaiDamageModel[] _eBakuraiModel;

		private PSTorpedoWakes _torpedoWakes;

		private List<bool> _listBombCritical;

		private List<ParticleSystem> _listExplosion;

		private List<ParticleSystem> _listMiss;

		private Dictionary<int, UIBattleShip> _fBattleship;

		private Dictionary<int, UIBattleShip> _eBattleship;

		private BattleHPGauges _battleHpGauges;

		private HpHitState[] _eHitState;

		private HitType[] _dicHitType;

		private float _explosionTime;

		public IEnumerator _init()
		{
			_explosionTime = 0f;
			_isEx = false;
			_isMiss = false;
			_isProtectE = false;
			_fBattleship = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			_eBattleship = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
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
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			if (_rescueCutIn == null)
			{
				_rescueCutIn = GetComponent<ProdAerialRescueCutIn>();
			}
			_rescueCutIn._init();
			yield return null;
		}

		private void _initHPGauge()
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

		private void OnDestroy()
		{
			_initParticleList();
			Mem.Del(ref _attackState);
			Mem.Del(ref _mainObj);
			Mem.Del(ref _aircraftObj);
			Mem.Del(ref _camAerial);
			Mem.Del(ref _eHpPos);
			Mem.Del(ref _anime);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _clsAerial);
			Mem.Del(ref _fBakuraiModel);
			Mem.Del(ref _eBakuraiModel);
			Mem.Del(ref _dicHitType);
			Mem.DelListSafe(ref _listBombCritical);
			Mem.DelListSafe(ref _listExplosion);
			Mem.DelListSafe(ref _listMiss);
			Mem.Del(ref _eHitState);
			Mem.Del(ref _eBakuraiModel);
			Mem.Del(ref _rescueCutIn);
			if (_battleHpGauges != null)
			{
				_battleHpGauges.Dispose();
			}
			Mem.Del(ref _battleHpGauges);
			if (_torpedoWakes != null)
			{
				_torpedoWakes.SetDestroy();
			}
		}

		public static ProdSupportAerialPhase2 Instantiate(ProdSupportAerialPhase2 prefab, ShienModel_Air model, Transform parent)
		{
			ProdSupportAerialPhase2 prodSupportAerialPhase = UnityEngine.Object.Instantiate(prefab);
			prodSupportAerialPhase.transform.parent = parent;
			prodSupportAerialPhase.transform.localPosition = Vector3.zero;
			prodSupportAerialPhase.transform.localScale = Vector3.one;
			prodSupportAerialPhase._clsAerial = model;
			prodSupportAerialPhase.StartCoroutine(prodSupportAerialPhase._init());
			return prodSupportAerialPhase;
		}

		public bool Update()
		{
			if (_attackState == AttackState.FriendExplosion && _isEx)
			{
				_explosionTime += Time.deltaTime;
				if (_explosionTime > 3f)
				{
					_onFinishedCutIn();
					_changeState();
					setState();
					_explosionTime = 0f;
					_isEx = false;
					_isMiss = false;
				}
			}
			return true;
		}

		public void Play(Action callback)
		{
			GetComponent<UIPanel>().widgetsAreStatic = false;
			_isPlaying = true;
			_actCallback = callback;
			_attackState = AttackState.None;
			_fBakuraiModel = _clsAerial.GetRaigekiData_e();
			_eBakuraiModel = _clsAerial.GetRaigekiData_f();
			Transform transform = _camAerial.transform;
			Vector3 localPosition = _camAerial.transform.localPosition;
			transform.localPosition = new Vector3(0f, localPosition.y, 90f);
			_eHitState = new HpHitState[_eBakuraiModel.Length];
			_dicHitType = new HitType[_eBakuraiModel.Length];
			for (int i = 0; i < _eBakuraiModel.Length; i++)
			{
				_eHitState[i] = HpHitState.None;
				_dicHitType[i] = HitType.None;
			}
			_changeState();
			setState();
		}

		private void _changeState()
		{
			switch (_attackState)
			{
			case AttackState.End:
				break;
			case AttackState.FriendBomb:
				_attackState = (_clsAerial.IsRaigeki_e() ? AttackState.FriendRaigeki : AttackState.FriendExplosion);
				break;
			case AttackState.FriendRaigeki:
				_attackState = AttackState.FriendExplosion;
				break;
			case AttackState.FriendExplosion:
				_attackState = AttackState.End;
				break;
			case AttackState.None:
				if (_clsAerial.IsBakugeki_e())
				{
					_attackState = AttackState.FriendBomb;
				}
				else if (_clsAerial.IsRaigeki_e())
				{
					_attackState = AttackState.FriendRaigeki;
				}
				else
				{
					_attackState = AttackState.End;
				}
				break;
			}
		}

		private void setState()
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
				_viewAircrafts(fleetType, _attackState, _fBakuraiModel);
				_camAerial.transform.localPosition = new Vector3(20f, 15f, 0f);
				_camAerial.transform.rotation = Quaternion.Euler(new Vector3(-16f, 90f, 0f));
				Hashtable hashtable = new Hashtable();
				hashtable.Add("rotation", new Vector3(-13.5f, 95f, 0f));
				hashtable.Add("isLocal", true);
				hashtable.Add("time", 1.349f);
				hashtable.Add("easeType", iTween.EaseType.linear);
				_camAerial.gameObject.RotateTo(hashtable);
				_anime.Play("AerialStartPhase2_1");
				Animation component = ((Component)base.transform.FindChild("CloudPanel")).GetComponent<Animation>();
				component.Play();
				break;
			}
			case "Torpedo":
				_initParticleList();
				_battleHpGauges.Init();
				_viewAircrafts(fleetType, _attackState, _fBakuraiModel);
				_createTorpedoWake();
				_camAerial.transform.localPosition = new Vector3(-21.3f, 6.2f, -7f);
				_camAerial.transform.rotation = Quaternion.Euler(new Vector3(16.29f, 90f, 0f));
				BattleTaskManager.GetBattleField().seaLevel.waveSpeed = new Vector4(-4f, -2000f, 5f, -1600f);
				_anime.Play("AerialStartPhase2_2");
				break;
			case "Explosion":
				_setHpGauge();
				_moveExplosionCamera();
				break;
			case "End":
				_battleHpGauges.Init();
				_aerialCombatPhase1Finished();
				break;
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

		private void _viewAircrafts(FleetType type, AttackState attack, BakuRaiDamageModel[] model)
		{
			switch (attack)
			{
			case AttackState.FriendBomb:
				_viewAircraft(type, isBomb: true, model);
				break;
			case AttackState.FriendRaigeki:
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
			SlotitemModel_Battle[] array = (!isBomb) ? _clsAerial.GetRaigekiPlanes(is_friend) : _clsAerial.GetBakugekiPlanes(is_friend);
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
			_camAerial.transform.localPosition = new Vector3(0f, 3f, 35f);
			_camAerial.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", new Vector3(0f, 3f, -40f));
			hashtable.Add("isLocal", false);
			hashtable.Add("time", 0.9f);
			hashtable.Add("easeType", iTween.EaseType.linear);
			hashtable.Add("oncomplete", "_playRescueCutIn");
			hashtable.Add("oncompletetarget", base.gameObject);
			_camAerial.gameObject.MoveTo(hashtable);
		}

		private void _playRescueCutIn()
		{
			if (_attackState == AttackState.FriendExplosion && _isProtectE)
			{
				Transform transform = _camAerial.transform;
				Vector3 position = _rescueCutIn._listBattleShip[0].transform.position;
				transform.localPosition = new Vector3(position.x, 3f, -40f);
				_rescueCutIn.Play(_onFinishedRescueCutIn);
			}
			else
			{
				_onFinishedRescueCutIn();
			}
		}

		private void _onFinishedRescueCutIn()
		{
			_camAerial.transform.localPosition = new Vector3(0f, 3f, -40f);
			_camAerial.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
			_onFinishedMoveCamera();
		}

		private void _onFinishedCutIn()
		{
			if (_torpedoWakes != null)
			{
				_torpedoWakes.ReStartAll();
			}
		}

		private void _setProtect()
		{
			int num = 0;
			ShipModel_Defender defender;
			while (true)
			{
				if (num >= _fBakuraiModel.Length)
				{
					return;
				}
				if (_fBakuraiModel[num] != null)
				{
					defender = _fBakuraiModel[num].Defender;
					if (_fBakuraiModel[num].GetProtectEffect())
					{
						break;
					}
				}
				num++;
			}
			_isProtectE = true;
			_rescueCutIn.AddShipList(_eBattleship[0], _eBattleship[defender.Index]);
		}

		private void _createTorpedoWake()
		{
			_torpedoWakes = new PSTorpedoWakes();
			int num = 0;
			_setProtect();
			Vector3 targetVec = default(Vector3);
			for (int i = 0; i < _fBakuraiModel.Length; i++)
			{
				if (_fBakuraiModel[i] != null && _fBakuraiModel[i].IsRaigeki())
				{
					ShipModel_Defender defender = _fBakuraiModel[i].Defender;
					int num2 = (!_fBakuraiModel[i].GetProtectEffect()) ? defender.Index : 0;
					bool flag = (_fBakuraiModel[i].GetHitState() == BattleHitStatus.Miss) ? true : false;
					float num3 = (!flag) ? 0f : 1f;
					Vector3 injectionVec = _setTorpedoVec(num, isFriend: true);
					num++;
					_dicHitType[num2] = setHitType(FleetType.Enemy, defender.Index, flag, HitType.Torpedo);
					Vector3 position = _eBattleship[num2].transform.position;
					float x = position.x + num3;
					Vector3 position2 = _eBattleship[num2].transform.position;
					targetVec = new Vector3(x, 0f, position2.z + 1f);
					if (_fBakuraiModel[i].GetProtectEffect())
					{
						Vector3 position3 = _eBattleship[defender.Index].transform.position;
						_torpedoWakes.SetProtectVector(num, new Vector3(position3.x + 2f, 0f, position3.z));
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
			switch (_dicHitType[index])
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

		private void _setHpGauge()
		{
			_eHpPos = new Vector3[_eBattleship.Count];
			_eHpPos = _setHpPosition(FleetType.Enemy, _eBattleship.Count);
			List<ShipModel_Defender> defenders = _clsAerial.GetDefenders(is_friend: false, all: true);
			for (int i = 0; i < _fBakuraiModel.Length; i++)
			{
				if (_fBakuraiModel[i] == null)
				{
					continue;
				}
				ShipModel_Defender defender = _fBakuraiModel[i].Defender;
				if (_fBakuraiModel[i].GetHitState() == BattleHitStatus.Clitical)
				{
					_eHitState[defender.Index] = HpHitState.Critical;
				}
				else if (_fBakuraiModel[i].GetHitState() == BattleHitStatus.Miss)
				{
					if (_eHitState[defender.Index] == HpHitState.None)
					{
						_eHitState[defender.Index] = HpHitState.Miss;
					}
				}
				else if (_fBakuraiModel[i].GetHitState() == BattleHitStatus.Normal && _eHitState[defender.Index] != HpHitState.Critical)
				{
					_eHitState[defender.Index] = HpHitState.Hit;
				}
			}
			for (int j = 0; j < _eBattleship.Count; j++)
			{
				BattleHitStatus status = (_eHitState[j] != HpHitState.Miss) ? ((_eHitState[j] != HpHitState.Critical) ? BattleHitStatus.Normal : BattleHitStatus.Clitical) : BattleHitStatus.Miss;
				_battleHpGauges.SetGauge(j, isFriend: false, isLight: true, isT: false, isNumber: false);
				_battleHpGauges.SetHp(j, defenders[j].MaxHp, defenders[j].HpBefore, defenders[j].HpAfter, _clsAerial.GetAttackDamage(defenders[j].TmpId).GetDamage(), status, isFriend: false);
			}
		}

		private Vector3[] _setHpPosition(FleetType type, int count)
		{
			Vector3[] array = new Vector3[count];
			switch (count)
			{
			case 1:
				array[0] = new Vector3(0f, 170f, 0f);
				break;
			case 2:
				for (int m = 0; m < 2; m++)
				{
					array[m] = new Vector3(-80f + 160f * (float)m, 170f, 0f);
				}
				break;
			case 3:
				for (int j = 0; j < 3; j++)
				{
					array[j] = new Vector3(-165f + 160f * (float)j, 170f, 0f);
				}
				break;
			case 4:
				for (int l = 0; l < 4; l++)
				{
					array[l] = new Vector3(-250f + 160f * (float)l, 170f, 0f);
				}
				break;
			case 5:
				for (int k = 0; k < 5; k++)
				{
					array[k] = new Vector3(-320f + 160f * (float)k, 170f, 0f);
				}
				break;
			case 6:
				for (int i = 0; i < 6; i++)
				{
					array[i] = new Vector3(-400f + 160f * (float)i, 170f, 0f);
				}
				break;
			}
			return array;
		}

		private void _setShinking(FleetType type)
		{
			for (int i = 0; i < _fBakuraiModel.Length; i++)
			{
				if (_fBakuraiModel[i] == null)
				{
					continue;
				}
				ShipModel_Defender defender = _fBakuraiModel[i].Defender;
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

		private void _createExplotion()
		{
			_listBombCritical = new List<bool>();
			_listExplosion = new List<ParticleSystem>();
			if (_attackState == AttackState.FriendExplosion)
			{
				for (int i = 0; i < _eBattleship.Count; i++)
				{
					_battleHpGauges.Show(i, _eHpPos[i], new Vector3(0.35f, 0.35f, 0.35f), isScale: false);
					BattleHPGauges battleHpGauges = _battleHpGauges;
					int num = i;
					Vector3 damagePosition = _battleHpGauges.GetDamagePosition(i);
					battleHpGauges.SetDamagePosition(num, new Vector3(damagePosition.x, -525f, 0f));
					_battleHpGauges.PlayHp(i, null);
				}
				_setShinking(FleetType.Friend);
				Vector3 position3 = default(Vector3);
				for (int j = 0; j < _fBakuraiModel.Length; j++)
				{
					if (_fBakuraiModel[j] == null || !_fBakuraiModel[j].IsBakugeki())
					{
						continue;
					}
					ShipModel_Battle defender = _fBakuraiModel[j].Defender;
					int num2 = (!_fBakuraiModel[j].GetProtectEffect()) ? defender.Index : defender.Index;
					bool flag = (_fBakuraiModel[j].GetHitState() == BattleHitStatus.Miss) ? true : false;
					FleetType fleetType = FleetType.Enemy;
					_dicHitType[num2] = setHitType(fleetType, num2, flag, HitType.Bomb);
					HitType hitType = _dicHitType[num2];
					if (!flag)
					{
						if (hitType == HitType.Bomb)
						{
							Vector3 position = _eBattleship[num2].transform.position;
							float x = position.x;
							Vector3 position2 = _eBattleship[num2].transform.position;
							position3 = new Vector3(x, 3f, position2.z);
							ParticleSystem val = (!((UnityEngine.Object)BattleTaskManager.GetParticleFile().explosionAerial == null)) ? UnityEngine.Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().explosionAerial) : BattleTaskManager.GetParticleFile().explosionAerial;
							((Component)val).SetActive(isActive: true);
							((Component)val).transform.parent = BattleTaskManager.GetBattleField().transform;
							((Component)val).transform.position = position3;
							_listExplosion.Add(val);
							_listBombCritical.Add((_fBakuraiModel[j].GetHitState() == BattleHitStatus.Clitical) ? true : false);
						}
					}
					else if (hitType == HitType.Miss)
					{
						Vector3 position4 = _eBattleship[num2].transform.position;
						float fMin = position4.x - 0.5f;
						Vector3 position5 = _eBattleship[num2].transform.position;
						float fLim = XorRandom.GetFLim(fMin, position5.x + 0.5f);
						Vector3 position6 = _eBattleship[num2].transform.position;
						float fMin2 = position6.z - 1f;
						Vector3 position7 = _eBattleship[num2].transform.position;
						float fLim2 = XorRandom.GetFLim(fMin2, position7.z + 1f);
						float num3 = fLim;
						Vector3 position8 = _eBattleship[num2].transform.position;
						fLim = ((!(num3 >= position8.x)) ? (fLim - 0.5f) : (fLim + 0.5f));
						float num4 = fLim2;
						Vector3 position9 = _eBattleship[num2].transform.position;
						fLim2 = ((!(num4 >= position9.z)) ? (fLim2 - 0.5f) : (fLim2 + 0.5f));
						ParticleSystem val2 = (!((UnityEngine.Object)BattleTaskManager.GetParticleFile().splashMiss == null)) ? UnityEngine.Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().splashMiss) : BattleTaskManager.GetParticleFile().splashMiss;
						((Component)val2).SetActive(isActive: true);
						((Component)val2).transform.parent = BattleTaskManager.GetBattleField().transform;
						((Component)val2).transform.position = new Vector3(fLim, 0f, fLim2);
						_listMiss.Add(val2);
					}
				}
			}
			if (_listExplosion != null)
			{
				int count = _listExplosion.Count;
			}
			if (_listMiss != null)
			{
				int count2 = _listMiss.Count;
			}
			_isEx = true;
			if (_listExplosion != null)
			{
				KCV.Utils.SoundUtils.PlaySE((_listExplosion.Count <= 1) ? SEFIleInfos.SE_930 : SEFIleInfos.SE_931);
			}
			Observable.FromCoroutine(_explosionPlay).Subscribe(delegate
			{
				_isEx = true;
			});
			Observable.FromCoroutine(_missSplashPlay).Subscribe(delegate
			{
				_isMiss = true;
			});
			_playSE(FleetType.Friend);
		}

		private IEnumerator _explosionPlay()
		{
			if (_listExplosion != null)
			{
				for (int i = 0; i < _listExplosion.Count; i++)
				{
					_listExplosion[i].Play();
					yield return new WaitForSeconds(0.3f);
				}
			}
		}

		private IEnumerator _missSplashPlay()
		{
			if (_listMiss != null)
			{
				foreach (ParticleSystem miss in _listMiss)
				{
					miss.Play();
					KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_908);
					yield return new WaitForSeconds(0.3f);
				}
			}
		}

		private void _playSE(FleetType fleetType)
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < _dicHitType.Length; i++)
			{
				if (_dicHitType[i] == HitType.Torpedo)
				{
					flag = true;
				}
				if (_dicHitType[i] == HitType.Miss)
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
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_914);
				break;
			case 1:
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_915);
				break;
			}
		}

		private void _onFinishedMoveCamera()
		{
			BattleTaskManager.GetBattleField().AlterWaveDirection(FleetType.Friend);
			if (_torpedoWakes != null)
			{
				_torpedoWakes.PlaySplashAll();
			}
			_camAerial.cameraShake.ShakeRot(null);
			_createExplotion();
			_isEx = true;
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
			setState();
		}

		private void _aerialAttackPhase2Finished()
		{
			_changeState();
			setState();
		}

		private void _aerialCombatPhase2Attack()
		{
			_injectionTrpedo();
		}

		private void _aerialCombatPhase1Finished()
		{
			_initParticleList();
			if (_rescueCutIn != null)
			{
				UnityEngine.Object.Destroy(_rescueCutIn.gameObject);
			}
			if (_actCallback != null)
			{
				_actCallback();
			}
		}
	}
}
