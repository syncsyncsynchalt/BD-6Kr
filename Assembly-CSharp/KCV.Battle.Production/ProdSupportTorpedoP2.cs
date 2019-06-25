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
	public class ProdSupportTorpedoP2
	{
		public enum StateType
		{
			None,
			Attack,
			End
		}

		public StateType stateType;

		private HpHitState[] _eHitState;

		private Action _actCallback;

		private ShienModel_Rai _clsTorpedo;

		private BattleFieldCamera _camFriend;

		private PSTorpedoWake _torpedoParticle;

		private List<PSTorpedoWake> _listPSTorpedoWake;

		private ProdAerialRescueCutIn _rescueCutIn;

		private BattleHPGauges _battleHpGauges;

		private Dictionary<int, UIBattleShip> _eBattleship;

		private UIPanel _uiHpGaugePanel;

		private bool _isPlaying;

		private bool _isProtect;

		private bool _isAttackE;

		private float _fTime;

		public Transform transform;

		public ProdSupportTorpedoP2(Transform obj)
		{
			transform = obj;
		}

		public void Initialize(ShienModel_Rai model, PSTorpedoWake torpedoWake)
		{
			_fTime = 0f;
			stateType = StateType.None;
			_clsTorpedo = model;
			_isAttackE = false;
			_camFriend = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetBollboardTarget(isFriend: false, _camFriend.transform);
			_torpedoParticle = torpedoWake;
			BattleTaskManager.GetTaskTorpedoSalvo();
			Transform prefabProdTorpedoResucueCutIn = BattleTaskManager.GetPrefabFile().prefabProdTorpedoResucueCutIn;
			_rescueCutIn = transform.SafeGetComponent<ProdAerialRescueCutIn>();
			_rescueCutIn._init();
		}

		public void OnSetDestroy()
		{
			Mem.Del(ref stateType);
			Mem.Del(ref _eHitState);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _clsTorpedo);
			Mem.Del(ref _camFriend);
			Mem.Del(ref _torpedoParticle);
			Mem.Del(ref transform);
			Mem.DelListSafe(ref _listPSTorpedoWake);
			if (_rescueCutIn != null)
			{
				_rescueCutIn.gameObject.Discard();
			}
			Mem.Del(ref _rescueCutIn);
			if (_battleHpGauges != null)
			{
				_battleHpGauges.Dispose();
			}
			Mem.Del(ref _battleHpGauges);
			if (_uiHpGaugePanel != null)
			{
				UnityEngine.Object.Destroy(_uiHpGaugePanel.gameObject);
			}
			Mem.Del(ref _uiHpGaugePanel);
		}

		public void CreateHpGauge(FleetType type)
		{
			if (_battleHpGauges == null)
			{
				_battleHpGauges = new BattleHPGauges();
			}
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UIPanel uIPanel = Resources.Load<UIPanel>("Prefabs/Battle/UI/UICircleHpPanel");
			_uiHpGaugePanel = Util.Instantiate(uIPanel.gameObject, cutInEffectCamera.transform.gameObject).GetComponent<UIPanel>();
			for (int i = 0; i < 6; i++)
			{
				_battleHpGauges.AddInstantiates(_uiHpGaugePanel.gameObject, isFriend: true, isLight: true, isT: false, isNumber: false);
			}
		}

		public void Play(Action callBack)
		{
			_listPSTorpedoWake = new List<PSTorpedoWake>();
			_isPlaying = true;
			stateType = StateType.Attack;
			_actCallback = callBack;
			_eBattleship = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			_setHpGauge();
			_createTorpedoWake();
			if (_isProtect)
			{
				Transform obj = _camFriend.transform;
				Vector3 position = _rescueCutIn._listBattleShip[0].transform.position;
				obj.localPosition = new Vector3(position.x, 3f, -40f);
				_camFriend.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
				_rescueCutIn.Play(_torpedoInjection);
			}
			else
			{
				_torpedoInjection();
			}
			BattleTaskManager.GetBattleShips().SetTorpedoSalvoWakeAngle(isSet: true);
			_camFriend.motionBlur.enabled = false;
		}

		public bool Update()
		{
			if (_isPlaying && stateType == StateType.End)
			{
				_fTime += Time.deltaTime;
				if (_fTime > 2.4f)
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

		private void _setHpGauge()
		{
			bool flag = false;
			_eHitState = new HpHitState[_eBattleship.Count];
			List<ShipModel_Defender> defenders = _clsTorpedo.GetDefenders(is_friend: false);
			for (int i = 0; i < defenders.Count; i++)
			{
				DamageModel attackDamage = _clsTorpedo.GetAttackDamage(defenders[i].TmpId);
				switch (attackDamage.GetHitState())
				{
				case BattleHitStatus.Normal:
					if (_eHitState[i] != HpHitState.Critical)
					{
						_eHitState[i] = HpHitState.Hit;
					}
					break;
				case BattleHitStatus.Clitical:
					_eHitState[i] = HpHitState.Critical;
					break;
				case BattleHitStatus.Miss:
					if (_eHitState[i] == HpHitState.None)
					{
						_eHitState[i] = HpHitState.Miss;
					}
					break;
				}
				BattleHitStatus status = (_eHitState[i] != HpHitState.Miss) ? ((_eHitState[i] != HpHitState.Critical) ? BattleHitStatus.Normal : BattleHitStatus.Clitical) : BattleHitStatus.Miss;
				_battleHpGauges.SetGauge(i, isFriend: false, isLight: false, isT: true, isNumber: false);
				_battleHpGauges.SetHp(i, defenders[i].MaxHp, defenders[i].HpBefore, defenders[i].HpAfter, attackDamage.GetDamage(), status, isFriend: false);
				if (attackDamage.GetProtectEffect() && !flag)
				{
					flag = true;
					_isProtect = true;
					_rescueCutIn.AddShipList(_eBattleship[0], _eBattleship[i]);
				}
			}
		}

		private void _createTorpedoWake()
		{
			ShipModel_BattleAll[] ships_e = BattleTaskManager.GetBattleManager().Ships_e;
			List<ShipModel_Defender> defenders = _clsTorpedo.GetDefenders(is_friend: false);
			for (int i = 0; i < defenders.Count; i++)
			{
				DamageModel attackDamage = _clsTorpedo.GetAttackDamage(defenders[i].TmpId);
				Vector3 vector = (!attackDamage.GetProtectEffect()) ? _eBattleship[i].transform.position : _eBattleship[i].transform.position;
				Vector3 target = (attackDamage.GetHitState() != 0) ? new Vector3(vector.x, vector.y + 1f, vector.z) : new Vector3(vector.x - 1.5f, vector.y - 1.5f, vector.z - 20f);
				List<PSTorpedoWake> listPSTorpedoWake = _listPSTorpedoWake;
				Vector3 position = _eBattleship[i].transform.position;
				float x = position.x;
				Vector3 position2 = _eBattleship[i].transform.position;
				listPSTorpedoWake.Add(_createTorpedo(new Vector3(x, 1f, position2.z + 40f), target, (attackDamage.GetHitState() != 0) ? 0.6f : 1f, (attackDamage.GetHitState() != 0) ? true : false));
				if (attackDamage.GetHitState() != 0)
				{
					_isAttackE = true;
				}
			}
		}

		private void _torpedoInjection()
		{
			_camFriend.transform.position = new Vector3(-38f, 8f, -74f);
			_camFriend.transform.localRotation = Quaternion.Euler(new Vector3(9.5f, 137.5f, 0f));
			bool isFirst = false;
			foreach (PSTorpedoWake item in _listPSTorpedoWake)
			{
				item.Injection(iTween.EaseType.linear, isPlaySE: false, isTC: false, delegate
				{
					if (!isFirst)
					{
						_setShinking();
						_compTorpedoAttack();
						isFirst = true;
					}
				});
			}
		}

		private void _setShinking()
		{
			List<ShipModel_Defender> defenders = _clsTorpedo.GetDefenders(is_friend: false);
			for (int i = 0; i < defenders.Count; i++)
			{
				if (defenders[i].DmgStateAfter == DamageState_Battle.Gekichin)
				{
					_eBattleship[i].PlayProdSinking(null);
				}
			}
		}

		private void _compTorpedoAttack()
		{
			bool flag = false;
			float[] array = new float[6]
			{
				-416f,
				-310f,
				-205f,
				-70f,
				115f,
				350f
			};
			List<ShipModel_Defender> defenders = _clsTorpedo.GetDefenders(is_friend: false);
			for (int i = 0; i < defenders.Count; i++)
			{
				_battleHpGauges.Show(i, new Vector3(setHpGaugePosition(defenders.Count, i), -210f, 0f), new Vector3(0.22f, 0.22f, 0.22f), isScale: false);
				_battleHpGauges.PlayHp(i, null);
				DamageModel attackDamage = _clsTorpedo.GetAttackDamage(defenders[i].TmpId);
				if (attackDamage.GetHitState() == BattleHitStatus.Miss)
				{
					_battleHpGauges.PlayMiss(i);
				}
				else if (attackDamage.GetHitState() == BattleHitStatus.Clitical)
				{
					flag = true;
					_battleHpGauges.PlayCritical(i);
				}
			}
			if (_isAttackE)
			{
				KCV.Utils.SoundUtils.PlaySE((!flag) ? SEFIleInfos.SE_909 : SEFIleInfos.SE_910);
				_camFriend.cameraShake.ShakeRot(null);
			}
			_setState(StateType.End);
		}

		private float setHpGaugePosition(int shipCount, int index)
		{
			float[] array = null;
			switch (shipCount)
			{
			case 1:
				return -150f;
			case 2:
				array = new float[2]
				{
					-209f,
					-61f
				};
				break;
			case 3:
				array = new float[3]
				{
					-278f,
					-150f,
					-12f
				};
				break;
			case 4:
				array = new float[4]
				{
					-321f,
					-202f,
					-72f,
					110f
				};
				break;
			case 5:
				array = new float[5]
				{
					-372f,
					-265f,
					-160f,
					-9f,
					229f
				};
				break;
			case 6:
				array = new float[6]
				{
					-416.5f,
					-310f,
					-205f,
					-70f,
					115f,
					350f
				};
				break;
			}
			return array[index];
		}

		private PSTorpedoWake _createTorpedo(Vector3 injection, Vector3 target, float time, bool isDet)
		{
			return PSTorpedoWake.Instantiate(_torpedoParticle, BattleTaskManager.GetBattleField().transform, injection, target, 0, time, isDet, isMiss: false);
		}

		private void _onTorpedoAttackFinished()
		{
			if (_battleHpGauges != null)
			{
				_battleHpGauges.Dispose();
			}
			_battleHpGauges = null;
			if (_listPSTorpedoWake != null)
			{
				foreach (PSTorpedoWake item in _listPSTorpedoWake)
				{
					UnityEngine.Object.Destroy(item);
				}
				_listPSTorpedoWake.Clear();
			}
			_listPSTorpedoWake = null;
			if (_actCallback != null)
			{
				_actCallback();
			}
			OnSetDestroy();
		}
	}
}
