using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdSupportShelling : MonoBehaviour
	{
		private enum AttackType
		{
			Bomb,
			Torpedo,
			Both,
			None
		}

		private AttackType _attackType;

		private HpHitState[] _eHitState;

		private Action _actCallback;

		private ShienModel_Hou _clsShelling;

		private BattleFieldCamera _fieldCam;

		private BattleHPGauges _battleHpGauges;

		private ProdAerialRescueCutIn _rescueCutIn;

		private List<bool> _listBombCritical;

		private List<ParticleSystem> _listExplosion;

		private List<ParticleSystem> _listMiss;

		private List<ShipModel_Defender> _defenders;

		private Dictionary<int, UIBattleShip> _eBattleship;

		private UIPanel _uiHpGaugePanel;

		private bool _isProtect;

		private bool _isEx;

		private bool _isAttack;

		private float _explosionTime;

		private Vector3[] _eHpPos;

		private bool _init()
		{
			_isEx = false;
			_isAttack = false;
			_isProtect = false;
			_explosionTime = 0f;
			_eHpPos = null;
			_defenders = _clsShelling.GetDefenders(is_friend: false);
			_fieldCam = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			_fieldCam.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			_rescueCutIn = base.transform.SafeGetComponent<ProdAerialRescueCutIn>();
			_rescueCutIn._init();
			return true;
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
			for (int i = 0; i < _defenders.Count; i++)
			{
				_battleHpGauges.AddInstantiates(base.gameObject, isFriend: true, isLight: true, isT: false, isNumber: false);
			}
		}

		private void OnDestroy()
		{
			_initParticleList();
			Mem.Del(ref _attackType);
			Mem.Del(ref _eHitState);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _clsShelling);
			Mem.Del(ref _fieldCam);
			Mem.Del(ref _uiHpGaugePanel);
			Mem.DelListSafe(ref _listBombCritical);
			Mem.DelListSafe(ref _listExplosion);
			Mem.DelListSafe(ref _listMiss);
			Mem.DelListSafe(ref _defenders);
			if (_battleHpGauges != null)
			{
				_battleHpGauges.Dispose();
			}
			Mem.Del(ref _battleHpGauges);
			Mem.Del(ref _rescueCutIn);
			if (_uiHpGaugePanel != null)
			{
				UnityEngine.Object.Destroy(_uiHpGaugePanel.gameObject);
			}
			Mem.Del(ref _uiHpGaugePanel);
		}

		public static ProdSupportShelling Instantiate(ProdSupportShelling prefab, ShienModel_Hou model, Transform parent)
		{
			ProdSupportShelling prodSupportShelling = UnityEngine.Object.Instantiate(prefab);
			prodSupportShelling.transform.parent = parent;
			prodSupportShelling.transform.localPosition = Vector3.zero;
			prodSupportShelling.transform.localScale = Vector3.one;
			prodSupportShelling._clsShelling = model;
			prodSupportShelling._init();
			return prodSupportShelling;
		}

		public bool Update()
		{
			if (_attackType == AttackType.Bomb && _isEx)
			{
				_explosionTime += Time.deltaTime;
				if (_explosionTime > 3f)
				{
					_finishedShelling();
					_explosionTime = 0f;
					_isEx = false;
				}
			}
			return true;
		}

		public void Play(Action callback)
		{
			_actCallback = callback;
			_eBattleship = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			Transform transform = _fieldCam.transform;
			Vector3 localPosition = _fieldCam.transform.localPosition;
			transform.localPosition = new Vector3(0f, localPosition.y, 90f);
			BattleTaskManager.GetBattleCameras().cutInCamera.enabled = true;
			_setHpGauge();
			_createExplosion();
			_moveCamera();
		}

		private void _initParticleList()
		{
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

		private void _moveCamera()
		{
			_fieldCam.transform.localPosition = new Vector3(0f, 3f, 30f);
			_fieldCam.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
			_fieldCam.transform.LTMoveLocal(new Vector3(0f, 3f, -40f), 0.9f).setEase(LeanTweenType.easeInCubic).setOnComplete((Action)delegate
			{
				_playRescueCutIn();
			});
			_attackType = AttackType.Bomb;
		}

		private void _playRescueCutIn()
		{
			if (_isProtect)
			{
				Transform transform = _fieldCam.transform;
				Vector3 position = _rescueCutIn._listBattleShip[0].transform.position;
				transform.localPosition = new Vector3(position.x, 3f, -40f);
				_rescueCutIn.Play(_finishedRescueCutIn);
			}
			else
			{
				_finishedRescueCutIn();
			}
		}

		private void _finishedRescueCutIn()
		{
			_fieldCam.transform.localPosition = new Vector3(0f, 3f, -40f);
			_fieldCam.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
			_hitExplosion();
		}

		private void _createExplosion()
		{
			_listBombCritical = new List<bool>();
			_listExplosion = new List<ParticleSystem>();
			_listMiss = new List<ParticleSystem>();
			Vector3 position5 = default(Vector3);
			for (int i = 0; i < _defenders.Count; i++)
			{
				DamageModel attackDamage = _clsShelling.GetAttackDamage(_defenders[i].TmpId);
				int key = (!attackDamage.GetProtectEffect()) ? _defenders[i].Index : _defenders[i].Index;
				if (attackDamage.GetHitState() != 0)
				{
					_isAttack = true;
					ParticleSystem val = (!((UnityEngine.Object)BattleTaskManager.GetParticleFile().explosionAerial == null)) ? UnityEngine.Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().explosionAerial) : BattleTaskManager.GetParticleFile().explosionAerial;
					((Component)val).SetActive(isActive: true);
					((Component)val).transform.parent = BattleTaskManager.GetBattleField().transform;
					Transform transform = ((Component)val).transform;
					Vector3 position = _eBattleship[key].transform.position;
					float x = position.x;
					Vector3 position2 = _eBattleship[key].transform.position;
					transform.position = new Vector3(x, 3f, position2.z);
					_listExplosion.Add(val);
					_listBombCritical.Add((attackDamage.GetHitState() == BattleHitStatus.Clitical) ? true : false);
				}
				else
				{
					int iLim = XorRandom.GetILim(0, 2);
					Vector3[] array = new Vector3[3]
					{
						new Vector3(5f, 0f, -5f),
						new Vector3(-3f, 0f, 5f),
						new Vector3(4f, 0f, -7f)
					};
					Vector3 position3 = _eBattleship[key].transform.position;
					float x2 = position3.x + array[iLim].x;
					Vector3 position4 = _eBattleship[key].transform.position;
					position5 = new Vector3(x2, 0f, position4.z + array[iLim].z);
					ParticleSystem val2 = (!((UnityEngine.Object)BattleTaskManager.GetParticleFile().splashMiss == null)) ? UnityEngine.Object.Instantiate<ParticleSystem>(BattleTaskManager.GetParticleFile().splashMiss) : BattleTaskManager.GetParticleFile().splashMiss;
					((Component)val2).SetActive(isActive: true);
					((Component)val2).transform.parent = BattleTaskManager.GetBattleField().transform;
					((Component)val2).transform.position = position5;
					val2.Stop();
					_listMiss.Add(val2);
				}
			}
		}

		private void _hitExplosion()
		{
			BattleTaskManager.GetBattleField().AlterWaveDirection(FleetType.Friend);
			BattleTaskManager.GetBattleCameras().cutInCamera.enabled = true;
			if (_isAttack)
			{
				_fieldCam.cameraShake.ShakeRot(null);
			}
			for (int i = 0; i < _defenders.Count; i++)
			{
				DamageModel attackDamage = _clsShelling.GetAttackDamage(_defenders[i].TmpId);
				_battleHpGauges.Show(i, _eHpPos[i], new Vector3(0.35f, 0.35f, 0.35f), isScale: false);
				BattleHPGauges battleHpGauges = _battleHpGauges;
				int num = i;
				Vector3 damagePosition = _battleHpGauges.GetDamagePosition(i);
				battleHpGauges.SetDamagePosition(num, new Vector3(damagePosition.x, -525f, 0f));
				_battleHpGauges.PlayHp(i, null);
				if (attackDamage.GetHitState() == BattleHitStatus.Miss)
				{
					_battleHpGauges.PlayMiss(i);
				}
				else if (attackDamage.GetHitState() == BattleHitStatus.Clitical)
				{
					_battleHpGauges.PlayCritical(i);
				}
			}
			_setShinking();
			int eCnt = (_listExplosion != null) ? _listExplosion.Count : 0;
			int mCnt = (_listMiss != null) ? _listMiss.Count : 0;
			Observable.FromCoroutine(_playExplosion).Subscribe(delegate
			{
				if (eCnt >= mCnt)
				{
					_isEx = true;
				}
			});
			Observable.FromCoroutine(_playMissSplash).Subscribe(delegate
			{
				if (mCnt >= eCnt)
				{
					_isEx = true;
				}
			});
			if (_listExplosion == null && _listMiss == null)
			{
				_isEx = true;
			}
			if ((_listExplosion != null) & (_listExplosion.Count > 0))
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.Explode);
			}
			if ((_listMiss != null) & (_listMiss.Count > 0))
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.BattleTorpedo);
			}
		}

		private IEnumerator _playExplosion()
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
					KCV.Utils.SoundUtils.PlaySE((!_listBombCritical[i]) ? SEFIleInfos.SE_906 : SEFIleInfos.SE_907);
					yield return new WaitForSeconds(0.3f);
				}
			}
		}

		private IEnumerator _playMissSplash()
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

		private void _setShinking()
		{
			for (int i = 0; i < _defenders.Count; i++)
			{
				if (_defenders[i].DmgStateAfter == DamageState_Battle.Gekichin)
				{
					_eBattleship[i].PlayProdSinking(null);
				}
			}
		}

		private void _setHpGauge()
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			bool flag = false;
			_eHpPos = _setHpGaugePosition(_eBattleship.Count);
			_eHitState = new HpHitState[_eBattleship.Count];
			UIPanel uIPanel = Resources.Load<UIPanel>("Prefabs/Battle/UI/UICircleHpPanel");
			_uiHpGaugePanel = Util.Instantiate(uIPanel.gameObject, cutInCamera.transform.gameObject).GetComponent<UIPanel>();
			for (int i = 0; i < _defenders.Count; i++)
			{
				DamageModel attackDamage = _clsShelling.GetAttackDamage(_defenders[i].TmpId);
				if (attackDamage.GetHitState() == BattleHitStatus.Clitical)
				{
					_eHitState[attackDamage.Defender.Index] = HpHitState.Critical;
				}
				else if (attackDamage.GetHitState() == BattleHitStatus.Miss)
				{
					if (_eHitState[attackDamage.Defender.Index] == HpHitState.None)
					{
						_eHitState[attackDamage.Defender.Index] = HpHitState.Miss;
					}
				}
				else if (attackDamage.GetHitState() == BattleHitStatus.Normal && _eHitState[attackDamage.Defender.Index] != HpHitState.Critical)
				{
					_eHitState[attackDamage.Defender.Index] = HpHitState.Hit;
				}
				BattleHitStatus status = (_eHitState[i] != HpHitState.Miss) ? ((_eHitState[i] != HpHitState.Critical) ? BattleHitStatus.Normal : BattleHitStatus.Clitical) : BattleHitStatus.Miss;
				_battleHpGauges.SetGauge(i, isFriend: false, isLight: true, isT: false, isNumber: false);
				_battleHpGauges.SetHp(i, _defenders[i].MaxHp, _defenders[i].HpBefore, _defenders[i].HpAfter, attackDamage.GetDamage(), status, isFriend: false);
				if (attackDamage.GetProtectEffect() && !flag)
				{
					flag = true;
					_isProtect = true;
					_rescueCutIn.AddShipList(_eBattleship[0], _eBattleship[i]);
				}
			}
		}

		private Vector3[] _setHpGaugePosition(int count)
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

		private void _finishedShelling()
		{
			_initParticleList();
			if (_actCallback != null)
			{
				_actCallback();
			}
		}
	}
}
