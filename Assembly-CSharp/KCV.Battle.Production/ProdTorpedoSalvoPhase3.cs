using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdTorpedoSalvoPhase3
	{
		public enum StateType
		{
			None,
			Attack,
			End
		}

		private float _fTime;

		private bool[] _isTorpedoHit;

		private bool[] _isProtect;

		private bool _isPlaying;

		private Action _actCallback;

		private Dictionary<int, UIBattleShip> _fBattleship;

		private Dictionary<int, UIBattleShip> _eBattleship;

		private Dictionary<FleetType, bool[]> _dicIsCriticall;

		private Dictionary<FleetType, bool[]> _dicIsMiss;

		private RaigekiModel _clsRaigeki;

		private BattleFieldCamera[] _fieldCam;

		private List<PSTorpedoWake> _listPSTorpedoWake;

		private ProdTorpedoProtect _torpedoProtect;

		private PSTorpedoWake _torpedoParticle;

		private StateType stateType;

		public Transform transform;

		public ProdTorpedoSalvoPhase3(Transform obj)
		{
			transform = obj;
		}

		public bool Initialize(RaigekiModel model, PSTorpedoWake psTorpedo)
		{
			_fTime = 0f;
			_isTorpedoHit = new bool[2];
			_isProtect = new bool[2];
			stateType = StateType.None;
			_clsRaigeki = model;
			_torpedoParticle = psTorpedo;
			_fBattleship = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			_eBattleship = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			_listPSTorpedoWake = new List<PSTorpedoWake>();
			_fieldCam = new BattleFieldCamera[2];
			_fieldCam[0] = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			_fieldCam[1] = BattleTaskManager.GetBattleCameras().enemyFieldCamera;
			_torpedoProtect = transform.SafeGetComponent<ProdTorpedoProtect>();
			_torpedoProtect._init();
			return true;
		}

		public void OnSetDestroy()
		{
			if (_listPSTorpedoWake != null)
			{
				foreach (PSTorpedoWake item in _listPSTorpedoWake)
				{
					UnityEngine.Object.Destroy(item);
				}
				_listPSTorpedoWake.Clear();
			}
			_listPSTorpedoWake = null;
			if (_torpedoProtect != null)
			{
				UnityEngine.Object.Destroy(_torpedoProtect.gameObject);
			}
			_torpedoProtect = null;
			Mem.Del(ref _isTorpedoHit);
			Mem.Del(ref _isProtect);
			Mem.Del(ref _actCallback);
			Mem.Del(ref stateType);
			Mem.Del(ref transform);
			Mem.DelListSafe(ref _listPSTorpedoWake);
			Mem.DelDictionarySafe(ref _dicIsCriticall);
			Mem.DelDictionarySafe(ref _dicIsMiss);
			if (_torpedoProtect != null)
			{
				UnityEngine.Object.Destroy(_torpedoProtect.gameObject);
			}
			_torpedoProtect = null;
			_torpedoParticle = null;
			_clsRaigeki = null;
			Mem.Del(ref _fieldCam);
		}

		public void Play(Action callBack)
		{
			_isPlaying = true;
			_actCallback = callBack;
			stateType = StateType.Attack;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetBollboardTarget(isFriend: true, _fieldCam[0].transform);
			battleShips.SetBollboardTarget(isFriend: false, _fieldCam[1].transform);
			BattleTaskManager.GetBattleShips().SetTorpedoSalvoWakeAngle(isSet: true);
			CreateTorpedoWake();
			if (_isProtect[0])
			{
				Transform obj = _fieldCam[0].transform;
				Vector3 position = _torpedoProtect._listBattleShipF[0].transform.position;
				obj.position = new Vector3(position.x, 7.5f, -40f);
				_fieldCam[0].transform.localRotation = Quaternion.Euler(new Vector3(10.5f, 0f, 0f));
			}
			if (_isProtect[1])
			{
				Transform obj2 = _fieldCam[1].transform;
				Vector3 position2 = _torpedoProtect._listBattleShipE[0].transform.position;
				obj2.position = new Vector3(position2.x, 7.5f, 42f);
				_fieldCam[1].transform.localRotation = Quaternion.Euler(new Vector3(10.5f, 180f, 0f));
			}
			if (_isProtect[0] || _isProtect[1])
			{
				_torpedoProtect.Play(_torpedoInjection);
			}
			else
			{
				_torpedoInjection();
			}
		}

		public bool Update()
		{
			if (_isPlaying && stateType == StateType.End)
			{
				_fTime += Time.deltaTime;
				if (_fTime > 2f)
				{
					_setState(StateType.None);
					_onTorpedoAttackFinished();
					return true;
				}
			}
			return false;
		}

		private void _setState(StateType state)
		{
			stateType = state;
			_fTime = 0f;
		}

		public void SetHpGauge()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			if (BattleTaskManager.GetTorpedoHpGauges().UiPanel == null)
			{
				UIPanel uIPanel = Resources.Load<UIPanel>("Prefabs/Battle/UI/UICircleHpPanel");
				BattleTaskManager.GetTorpedoHpGauges().InstancePanel(uIPanel.gameObject, cutInEffectCamera.transform.gameObject);
			}
			else
			{
				BattleTaskManager.GetTorpedoHpGauges().UiPanel.alpha = 0f;
			}
			_dicIsCriticall = new Dictionary<FleetType, bool[]>();
			_dicIsMiss = new Dictionary<FleetType, bool[]>();
			if (BattleTaskManager.GetTorpedoHpGauges().FHpGauge == null)
			{
				BattleTaskManager.GetTorpedoHpGauges().FHpGauge = new BattleHPGauges();
			}
			int[] array = new int[_fBattleship.Count];
			bool[] array2 = new bool[_fBattleship.Count];
			bool[] array3 = new bool[_fBattleship.Count];
			List<ShipModel_Defender> defenders = _clsRaigeki.GetDefenders(is_friend: true, all: true);
			for (int i = 0; i < _fBattleship.Count; i++)
			{
				BattleTaskManager.GetTorpedoHpGauges().FHpGauge.AddInstantiatesSafe(BattleTaskManager.GetTorpedoHpGauges().UiPanel.gameObject, isFriend: true, isLight: false, isT: true, isNumber: false, i);
				array[i] = defenders[i].HpBefore;
				array2[i] = false;
				array3[i] = false;
			}
			_dicIsCriticall.Add(FleetType.Friend, array3);
			_dicIsMiss.Add(FleetType.Friend, array3);
			if (BattleTaskManager.GetTorpedoHpGauges().EHpGauge == null)
			{
				BattleTaskManager.GetTorpedoHpGauges().EHpGauge = new BattleHPGauges();
			}
			int[] array4 = new int[_eBattleship.Count];
			bool[] array5 = new bool[_eBattleship.Count];
			bool[] array6 = new bool[_eBattleship.Count];
			List<ShipModel_Defender> defenders2 = _clsRaigeki.GetDefenders(is_friend: false, all: true);
			int count = _fBattleship.Count;
			for (int j = 0; j < _eBattleship.Count; j++)
			{
				BattleTaskManager.GetTorpedoHpGauges().EHpGauge.AddInstantiatesSafe(BattleTaskManager.GetTorpedoHpGauges().UiPanel.gameObject, isFriend: false, isLight: false, isT: true, isNumber: false, j);
				array4[j] = defenders2[j].HpBefore;
				array5[j] = false;
				array6[j] = false;
			}
			_dicIsCriticall.Add(FleetType.Enemy, array6);
			_dicIsMiss.Add(FleetType.Enemy, array6);
			for (int k = 0; k < _fBattleship.Count; k++)
			{
				int damage = _clsRaigeki.GetAttackDamage(defenders[k].TmpId)?.GetDamage() ?? (-1);
				BattleHitStatus status = BattleHitStatus.Normal;
				if (_dicIsMiss[FleetType.Friend][k])
				{
					status = BattleHitStatus.Miss;
				}
				if (_dicIsCriticall[FleetType.Friend][k])
				{
					status = BattleHitStatus.Clitical;
				}
				BattleTaskManager.GetTorpedoHpGauges().FHpGauge.SetHp(k, defenders[k].MaxHp, array[k], defenders[k].HpAfter, damage, status, isFriend: false);
			}
			for (int l = 0; l < _eBattleship.Count; l++)
			{
				int damage2 = _clsRaigeki.GetAttackDamage(defenders2[l].TmpId)?.GetDamage() ?? (-1);
				BattleHitStatus status2 = BattleHitStatus.Normal;
				if (_dicIsMiss[FleetType.Enemy][l])
				{
					status2 = BattleHitStatus.Miss;
				}
				if (_dicIsCriticall[FleetType.Enemy][l])
				{
					status2 = BattleHitStatus.Clitical;
				}
				BattleTaskManager.GetTorpedoHpGauges().EHpGauge.SetHp(l, defenders2[l].MaxHp, array4[l], defenders2[l].HpAfter, damage2, status2, isFriend: false);
			}
		}

		private int[] addHpGauge(bool isFriend, BattleHPGauges hpGauges)
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			int num = (!isFriend) ? _eBattleship.Count : _fBattleship.Count;
			if (isFriend)
			{
			}
			int[] array = new int[num];
			List<ShipModel_Defender> defenders = _clsRaigeki.GetDefenders(isFriend, all: true);
			for (int i = 0; i < num; i++)
			{
				hpGauges.AddInstantiates(cutInCamera.transform.gameObject, isFriend, isLight: false, isT: true, isNumber: true);
				array[i] = defenders[i].HpBefore;
				if (isFriend)
				{
					_dicIsMiss[FleetType.Friend][i] = false;
					_dicIsCriticall[FleetType.Friend][i] = false;
				}
				else
				{
					_dicIsMiss[FleetType.Enemy][i] = false;
					_dicIsCriticall[FleetType.Enemy][i] = false;
				}
			}
			return array;
		}

		private Vector3 setHpGaugePosition(bool isFriend, int shipCount, int index)
		{
			Vector3[] array = new Vector3[shipCount];
			if (isFriend)
			{
				switch (shipCount)
				{
				case 1:
					array[0] = new Vector3(-237f, -210f, 0f);
					break;
				case 2:
					for (int m = 0; m < 2; m++)
					{
						array[m] = new Vector3(-200f - 80f * (float)m, -210f, 0f);
					}
					break;
				case 3:
					for (int j = 0; j < 3; j++)
					{
						array[j] = new Vector3(-165f - 80f * (float)j, -210f, 0f);
					}
					break;
				case 4:
					for (int l = 0; l < 4; l++)
					{
						array[l] = new Vector3(-120f - 80f * (float)l, -210f, 0f);
					}
					break;
				case 5:
					for (int k = 0; k < 5; k++)
					{
						array[k] = new Vector3(-80f - 80f * (float)k, -210f, 0f);
					}
					break;
				case 6:
					for (int i = 0; i < 6; i++)
					{
						array[i] = new Vector3(-42f - 80f * (float)i, -210f, 0f);
					}
					break;
				}
			}
			else
			{
				switch (shipCount)
				{
				case 1:
					array[0] = new Vector3(237f, -210f, 0f);
					break;
				case 2:
					for (int num4 = 0; num4 < 2; num4++)
					{
						array[num4] = new Vector3(200f + 80f * (float)num4, -210f, 0f);
					}
					break;
				case 3:
					for (int num = 0; num < 3; num++)
					{
						array[num] = new Vector3(165f + 80f * (float)num, -210f, 0f);
					}
					break;
				case 4:
					for (int num3 = 0; num3 < 4; num3++)
					{
						array[num3] = new Vector3(120f + 80f * (float)num3, -210f, 0f);
					}
					break;
				case 5:
					for (int num2 = 0; num2 < 5; num2++)
					{
						array[num2] = new Vector3(80f + 80f * (float)num2, -210f, 0f);
					}
					break;
				case 6:
					for (int n = 0; n < 6; n++)
					{
						array[n] = new Vector3(42f + 80f * (float)n, -210f, 0f);
					}
					break;
				}
			}
			return array[index];
		}

		private void _setProtect()
		{
			_addProtectShip(_fBattleship, _eBattleship, FleetType.Enemy);
			_addProtectShip(_eBattleship, _fBattleship, FleetType.Friend);
		}

		private void _addProtectShip(Dictionary<int, UIBattleShip> attackers, Dictionary<int, UIBattleShip> defenders, FleetType defType)
		{
			bool is_friend = (defType == FleetType.Friend) ? true : false;
			ShipModel_BattleAll[] array = (defType != 0) ? BattleTaskManager.GetBattleManager().Ships_f : BattleTaskManager.GetBattleManager().Ships_e;
			int num = 0;
			ShipModel_Defender attackTo;
			while (true)
			{
				if (num >= attackers.Count)
				{
					return;
				}
				ShipModel_BattleAll shipModel_BattleAll = array[num];
				attackTo = _clsRaigeki.GetAttackTo(shipModel_BattleAll);
				if (shipModel_BattleAll != null && attackTo != null)
				{
					RaigekiDamageModel attackDamage = _clsRaigeki.GetAttackDamage(attackTo.Index, is_friend);
					if (attackDamage.GetProtectEffect(shipModel_BattleAll.TmpId))
					{
						break;
					}
				}
				num++;
			}
			_isProtect[(int)defType] = true;
			_torpedoProtect.AddShipList(defenders[0], defenders[attackTo.Index], defType);
		}

		public void CreateTorpedoWake()
		{
			ShipModel_BattleAll[] ships_f = BattleTaskManager.GetBattleManager().Ships_f;
			ShipModel_BattleAll[] ships_e = BattleTaskManager.GetBattleManager().Ships_e;
			_setProtect();
			Vector3 injection = default(Vector3);
			Vector3 target = default(Vector3);
			for (int i = 0; i < _fBattleship.Count; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = ships_f[i];
				ShipModel_Defender attackTo = _clsRaigeki.GetAttackTo(shipModel_BattleAll);
				if (shipModel_BattleAll != null && attackTo != null)
				{
					RaigekiDamageModel attackDamage = _clsRaigeki.GetAttackDamage(attackTo.Index, is_friend: false);
					if (!attackDamage.GetProtectEffect(shipModel_BattleAll.TmpId))
					{
						int index = attackTo.Index;
					}
					if (attackDamage.GetHitState(shipModel_BattleAll.TmpId) == BattleHitStatus.Miss)
					{
						_dicIsMiss[FleetType.Enemy][attackTo.Index] = true;
					}
					if (attackDamage.GetHitState(shipModel_BattleAll.TmpId) == BattleHitStatus.Clitical)
					{
						_dicIsCriticall[FleetType.Enemy][attackTo.Index] = true;
					}
					Vector3 position = _eBattleship[attackTo.Index].transform.position;
					float x = position.x;
					Vector3 position2 = _eBattleship[attackTo.Index].transform.position;
					injection = new Vector3(x, 1f, position2.z + 13f);
					Vector3 position3 = _eBattleship[attackTo.Index].transform.position;
					float x2 = position3.x;
					Vector3 position4 = _eBattleship[attackTo.Index].transform.position;
					float y = position4.y + 0.5f;
					Vector3 position5 = _eBattleship[attackTo.Index].transform.position;
					target = new Vector3(x2, y, position5.z);
					_listPSTorpedoWake.Add(_createTorpedo(injection, target, isRescue: true));
					_isTorpedoHit[0] = true;
				}
			}
			Vector3 injection2 = default(Vector3);
			Vector3 target2 = default(Vector3);
			for (int j = 0; j < _eBattleship.Count; j++)
			{
				ShipModel_BattleAll shipModel_BattleAll2 = ships_e[j];
				ShipModel_Defender attackTo2 = _clsRaigeki.GetAttackTo(shipModel_BattleAll2);
				if (shipModel_BattleAll2 != null && attackTo2 != null)
				{
					RaigekiDamageModel attackDamage2 = _clsRaigeki.GetAttackDamage(attackTo2.Index, is_friend: true);
					if (!attackDamage2.GetProtectEffect(shipModel_BattleAll2.TmpId))
					{
						int index2 = attackTo2.Index;
					}
					if (attackDamage2.GetHitState(shipModel_BattleAll2.TmpId) == BattleHitStatus.Miss)
					{
						_dicIsMiss[FleetType.Friend][attackTo2.Index] = true;
					}
					if (attackDamage2.GetHitState(shipModel_BattleAll2.TmpId) == BattleHitStatus.Clitical)
					{
						_dicIsCriticall[FleetType.Friend][attackTo2.Index] = true;
					}
					Vector3 position6 = _fBattleship[attackTo2.Index].transform.position;
					float x3 = position6.x;
					Vector3 position7 = _fBattleship[attackTo2.Index].transform.position;
					injection2 = new Vector3(x3, 1f, position7.z - 13f);
					Vector3 position8 = _fBattleship[attackTo2.Index].transform.position;
					float x4 = position8.x;
					Vector3 position9 = _fBattleship[attackTo2.Index].transform.position;
					float y2 = position9.y + 0.5f;
					Vector3 position10 = _fBattleship[attackTo2.Index].transform.position;
					target2 = new Vector3(x4, y2, position10.z);
					_listPSTorpedoWake.Add(_createTorpedo(injection2, target2, isRescue: true));
					_isTorpedoHit[1] = true;
				}
			}
		}

		private void _torpedoInjection()
		{
			_fieldCam[0].transform.position = new Vector3(-51f, 8f, 90f);
			_fieldCam[0].transform.localRotation = Quaternion.Euler(new Vector3(10.5f, 70f, 0f));
			_fieldCam[1].transform.position = new Vector3(-51f, 8f, -90f);
			_fieldCam[1].transform.localRotation = Quaternion.Euler(new Vector3(10.5f, 111f, 0f));
			bool isFirst = false;
			foreach (PSTorpedoWake item in _listPSTorpedoWake)
			{
				item.SetActive(isActive: true);
				item.Injection(iTween.EaseType.linear, isPlaySE: false, isTC: false, delegate
				{
					if (!isFirst)
					{
						_compTorpedoAttack();
						isFirst = true;
					}
				});
			}
		}

		private void _setShinking(bool isFriend)
		{
			Dictionary<int, UIBattleShip> dictionary = (!isFriend) ? _fBattleship : _eBattleship;
			List<ShipModel_Defender> list = (!isFriend) ? _clsRaigeki.GetDefenders(is_friend: true) : _clsRaigeki.GetDefenders(is_friend: false);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].DmgStateAfter == DamageState_Battle.Gekichin)
				{
					dictionary[list[i].Index].PlayProdSinking(null);
				}
			}
		}

		private void _compTorpedoAttack()
		{
			if (_isTorpedoHit[0])
			{
				bool flag = false;
				for (int i = 0; i < _dicIsCriticall[FleetType.Friend].Length; i++)
				{
					if (_dicIsCriticall[FleetType.Friend][i])
					{
						flag = true;
					}
				}
				KCV.Utils.SoundUtils.PlaySE((!flag) ? SEFIleInfos.SE_909 : SEFIleInfos.SE_910);
				_fieldCam[1].cameraShake.ShakeRot(_enemyCameraShakeFinished);
			}
			if (_isTorpedoHit[1])
			{
				bool flag2 = false;
				for (int j = 0; j < _dicIsCriticall[FleetType.Enemy].Length; j++)
				{
					if (_dicIsCriticall[FleetType.Enemy][j])
					{
						flag2 = true;
					}
				}
				KCV.Utils.SoundUtils.PlaySE((!flag2) ? SEFIleInfos.SE_909 : SEFIleInfos.SE_910);
				_fieldCam[0].cameraShake.ShakeRot(_friendCameraShakeFinished);
			}
			for (int k = 0; k < _eBattleship.Count; k++)
			{
				BattleTaskManager.GetTorpedoHpGauges().EHpGauge.Show(k, setHpGaugePosition(isFriend: false, _eBattleship.Count, k), new Vector3(0.22f, 0.22f, 0.22f), isScale: true);
			}
			for (int l = 0; l < _fBattleship.Count; l++)
			{
				BattleTaskManager.GetTorpedoHpGauges().FHpGauge.Show(l, setHpGaugePosition(isFriend: true, _fBattleship.Count, l), new Vector3(0.22f, 0.22f, 0.22f), isScale: true);
			}
			BattleTaskManager.GetTorpedoHpGauges().UiPanel.alpha = 1f;
			if (_isTorpedoHit[1])
			{
				BattleTaskManager.GetTorpedoHpGauges().FHpGauge.PlayHpAll(null);
			}
			if (_isTorpedoHit[0])
			{
				BattleTaskManager.GetTorpedoHpGauges().EHpGauge.PlayHpAll(null);
			}
			_setShinking(isFriend: true);
			_setShinking(isFriend: false);
		}

		private PSTorpedoWake _createTorpedo(Vector3 injection, Vector3 target, bool isRescue)
		{
			PSTorpedoWake pSTorpedoWake = PSTorpedoWake.Instantiate(_torpedoParticle, BattleTaskManager.GetBattleField().transform, injection, target, 0, 0.6f, isDet: true, isMiss: false);
			pSTorpedoWake.SetActive(isActive: false);
			return pSTorpedoWake;
		}

		private void _friendCameraShakeFinished()
		{
			if (_fieldCam != null)
			{
				_fieldCam[0].transform.rotation = Quaternion.Euler(new Vector3(10.5f, 70f, 0f));
				_fieldCam[0].transform.position = new Vector3(-51f, 8f, 90f);
			}
			_setState(StateType.End);
		}

		private void _enemyCameraShakeFinished()
		{
			if (_fieldCam != null)
			{
				_fieldCam[1].transform.rotation = Quaternion.Euler(new Vector3(10.5f, 111f, 0f));
				_fieldCam[1].transform.position = new Vector3(-51f, 8f, -90f);
			}
			_setState(StateType.End);
		}

		private void _onTorpedoAttackFinished()
		{
			if (_actCallback != null)
			{
				_actCallback();
			}
			OnSetDestroy();
		}
	}
}
