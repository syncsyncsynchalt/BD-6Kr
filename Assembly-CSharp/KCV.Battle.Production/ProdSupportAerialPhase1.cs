using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdSupportAerialPhase1 : MonoBehaviour
	{
		[SerializeField]
		private UIPanel[] _uiPanel;

		[SerializeField]
		private GameObject[] _uiAirObjF;

		[SerializeField]
		private GameObject[] _uiAirObjE;

		[SerializeField]
		private UITexture[] _bgTex;

		[SerializeField]
		private UIPanel[] _cloudPanel;

		[SerializeField]
		private UIPanel[] _cloudParPanel;

		[SerializeField]
		private ParticleSystem[] _cloudPar;

		[SerializeField]
		private ParticleSystem[] _gunPar;

		private ProdAerialAircraft _aerialAircraft;

		private bool _isPlaying;

		private CutInType _iType;

		private Action _actCallback;

		private ShienModel_Air _clsAerial;

		private BattleFieldCamera _fieldCamera;

		private List<ProdAerialAircraft> _listAircraft;

		private Dictionary<int, UIBattleShip> _fBattleship;

		private Dictionary<int, UIBattleShip> _eBattleship;

		public bool _init()
		{
			_fieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			_uiPanel = new UIPanel[2];
			_uiAirObjF = new GameObject[2];
			_uiAirObjE = new GameObject[2];
			_cloudPanel = new UIPanel[2];
			_bgTex = new UITexture[2];
			_cloudParPanel = new UIPanel[2];
			_cloudPar = (ParticleSystem[])new ParticleSystem[2];
			_gunPar = (ParticleSystem[])new ParticleSystem[2];
			foreach (int value in Enum.GetValues(typeof(FleetType)))
			{
				if (value != 2)
				{
					Util.FindParentToChild(ref _uiPanel[value], base.transform, $"{((FleetType)value).ToString()}Panel");
					if (_uiAirObjF[value] == null)
					{
						_uiAirObjF[value] = _uiPanel[value].transform.FindChild("FAircraft").gameObject;
					}
					if (_uiAirObjE[value] == null)
					{
						_uiAirObjE[value] = _uiPanel[value].transform.FindChild("EAircraft").gameObject;
					}
					Util.FindParentToChild(ref _cloudPanel[value], base.transform, $"{((FleetType)value).ToString()}CloudPanel");
					Util.FindParentToChild(ref _bgTex[value], _cloudPanel[value].transform, "Bg");
					Util.FindParentToChild(ref _cloudParPanel[value], base.transform, $"{((FleetType)value).ToString()}CloudParPanel");
					Util.FindParentToChild<ParticleSystem>(ref _gunPar[value], _cloudPanel[value].transform, "Gun");
				}
			}
			bool flag = false;
			bool flag2 = false;
			if (_clsAerial.IsBakugeki_f() || _clsAerial.IsRaigeki_f())
			{
				flag = true;
			}
			if (_clsAerial.IsBakugeki_e() || _clsAerial.IsRaigeki_e())
			{
				flag2 = true;
			}
			if (flag && flag2)
			{
				_iType = CutInType.Both;
			}
			else if (flag && !flag2)
			{
				_iType = CutInType.FriendOnly;
			}
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleCutInCamera cutInCamera = battleCameras.cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			if (_iType == CutInType.Both)
			{
				battleCameras.SetSplitCameras2D(isSplit: true);
				cutInCamera.isCulling = true;
				cutInEffectCamera.isCulling = true;
			}
			else if (_iType == CutInType.FriendOnly)
			{
				cutInCamera.isCulling = true;
				cutInEffectCamera.isCulling = true;
			}
			Observable.FromCoroutine(_createAsyncAircrafts).Subscribe(delegate
			{
			});
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _uiAirObjF);
			Mem.Del(ref _uiAirObjE);
			Mem.Del(ref _bgTex);
			Mem.Del(ref _cloudParPanel);
			Mem.Del(ref _cloudPar);
			Mem.Del(ref _gunPar);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _clsAerial);
			Mem.Del(ref _fieldCamera);
			Mem.Del(ref _iType);
			Mem.DelListSafe(ref _listAircraft);
		}

		public static ProdSupportAerialPhase1 Instantiate(ProdSupportAerialPhase1 prefab, ShienModel_Air model, Transform parent, Dictionary<int, UIBattleShip> fShips, Dictionary<int, UIBattleShip> eShips)
		{
			ProdSupportAerialPhase1 prodSupportAerialPhase = UnityEngine.Object.Instantiate(prefab);
			prodSupportAerialPhase.transform.parent = parent;
			prodSupportAerialPhase.transform.localPosition = Vector3.zero;
			prodSupportAerialPhase.transform.localScale = Vector3.one;
			prodSupportAerialPhase._clsAerial = model;
			prodSupportAerialPhase._fBattleship = fShips;
			prodSupportAerialPhase._eBattleship = eShips;
			prodSupportAerialPhase._init();
			return prodSupportAerialPhase;
		}

		public void Play(Action callback)
		{
			_actCallback = callback;
			_fieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			_fieldCamera.transform.localPosition = new Vector3(0f, 12f, 0f);
			_fieldCamera.transform.localRotation = Quaternion.Euler(-16f, 0f, 0f);
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInCamera.cullingMask = (Generics.Layers.TransparentFX | Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInCamera.depth = 4f;
			cutInEffectCamera.cullingMask = (Generics.Layers.Background | Generics.Layers.CutIn);
			cutInEffectCamera.depth = 5f;
			cutInEffectCamera.glowEffect.enabled = false;
			Vector3[] array = new Vector3[2]
			{
				cutInCamera.transform.localPosition,
				cutInEffectCamera.transform.localPosition
			};
			foreach (int value in Enum.GetValues(typeof(FleetType)))
			{
				if (value != 2)
				{
					_uiPanel[value].transform.localPosition = array[value];
					_cloudPanel[value].transform.parent = ((value != 0) ? cutInEffectCamera.transform : cutInCamera.transform);
					_cloudParPanel[value] = null;
					_cloudPanel[value].transform.localPosition = Vector3.zero;
				}
			}
			if (_iType == CutInType.Both)
			{
				_setParticlePanel(FleetType.Friend, cutInCamera.transform);
				_setParticlePanel(FleetType.Enemy, cutInEffectCamera.transform);
				Transform transform = ((Component)_gunPar[0]).transform;
				Vector3 localPosition = ((Component)_gunPar[0]).transform.localPosition;
				transform.localPosition = new Vector3(400f, localPosition.y, 0f);
				Transform transform2 = ((Component)_gunPar[1]).transform;
				Vector3 localPosition2 = ((Component)_gunPar[1]).transform.localPosition;
				transform2.localPosition = new Vector3(400f, localPosition2.y, 0f);
			}
			else if (_iType == CutInType.FriendOnly)
			{
				_setParticlePanel(FleetType.Friend, cutInCamera.transform);
				base.transform.position = cutInCamera.transform.position;
				_uiPanel[0].transform.localPosition = Vector3.zero;
				_uiAirObjF[0].transform.localPosition = new Vector3(-280f, 0f, 0f);
				Transform transform3 = ((Component)_gunPar[0]).transform;
				Vector3 localPosition3 = ((Component)_gunPar[0]).transform.localPosition;
				transform3.localPosition = new Vector3(0f, localPosition3.y, 0f);
				_cloudPanel[1].SetActive(isActive: false);
			}
			else if (_iType != CutInType.EnemyOnly)
			{
			}
			for (int i = 0; i < 2; i++)
			{
				((Component)_cloudPanel[i].transform).GetComponent<Animation>().Play();
				if (_cloudParPanel[i] != null)
				{
					_cloudPar[i].Play();
				}
			}
			_playAircraft();
			_playGunParticle();
			Animation component = ((Component)base.transform).GetComponent<Animation>();
			component.Stop();
			component.Play("AerialStartPhase1_1");
			for (int j = 0; j < 2; j++)
			{
				Vector3 localPosition4 = _uiPanel[j].transform.localPosition;
				float x = localPosition4.x;
				Vector3 localPosition5 = _uiPanel[j].transform.localPosition;
				_baseMoveTo(new Vector3(x, 0f, localPosition5.z), 1.2f, 0.5f, iTween.EaseType.easeOutBack, string.Empty, _uiPanel[j].transform);
			}
		}

		private void _setParticlePanel(FleetType type, Transform trans)
		{
			_cloudParPanel[(int)type] = ((Component)base.transform.FindChild($"{type.ToString()}CloudParPanel")).GetComponent<UIPanel>();
			_cloudParPanel[(int)type].transform.parent = trans;
			_cloudParPanel[(int)type].transform.localScale = Vector3.one;
			_cloudParPanel[(int)type].transform.localPosition = Vector3.one;
			_cloudPar[(int)type] = ((Component)_cloudParPanel[(int)type].transform.FindChild("Smoke")).GetComponent<ParticleSystem>();
			((Component)_cloudPar[(int)type]).transform.localEulerAngles = ((type != 0) ? new Vector3(0f, 90f, 90f) : new Vector3(0f, -90f, 90f));
			_cloudPar[(int)type].Play();
		}

		private void _playGunParticle()
		{
		}

		private void _stopGunParticle()
		{
		}

		private IEnumerator _createAsyncAircrafts()
		{
			_listAircraft = new List<ProdAerialAircraft>();
			PlaneModelBase[] fPlanes = _clsAerial.GetPlanes(is_friend: true);
			PlaneModelBase[] ePlanes = _clsAerial.GetPlanes(is_friend: false);
			for (int j = 0; j < fPlanes.Length; j++)
			{
				if (fPlanes[j] != null)
				{
					if (j >= 6)
					{
						break;
					}
					_listAircraft.Add(_instantiateAircraft(_uiAirObjF[0].transform, j, fPlanes[j], FleetType.Friend));
					if (_iType == CutInType.Both)
					{
						_listAircraft.Add(_instantiateAircraft(_uiAirObjF[1].transform, j, fPlanes[j], FleetType.Friend));
					}
				}
			}
			yield return null;
			for (int i = 0; i < ePlanes.Length; i++)
			{
				if (ePlanes[i] != null)
				{
					if (i >= 6)
					{
						break;
					}
					_listAircraft.Add(_instantiateAircraft(_uiAirObjE[0].transform, i, ePlanes[i], FleetType.Enemy));
					if (_iType == CutInType.Both)
					{
						_listAircraft.Add(_instantiateAircraft(_uiAirObjE[1].transform, i, ePlanes[i], FleetType.Enemy));
					}
				}
			}
			yield return null;
		}

		private void _playAircraft()
		{
			for (int i = 0; i < _listAircraft.Count; i++)
			{
				_listAircraft[i].SubPlay();
			}
		}

		private void _playAircraftPhase2()
		{
			bool flag = false;
			for (int i = 0; i < _listAircraft.Count; i++)
			{
				_listAircraft[i].Injection(null);
				if (_listAircraft[i].GetPlane().State_Stage2End == PlaneState.Crush)
				{
					flag = true;
				}
			}
			if (flag)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_906);
			}
		}

		private void _moveCamera()
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			_uiPanel[0].transform.parent = cutInCamera.transform;
			_uiPanel[0].transform.localPosition = Vector3.one;
			_uiPanel[1].transform.parent = cutInEffectCamera.transform;
			_uiPanel[1].transform.localPosition = Vector3.one;
			for (int i = 0; i < _listAircraft.Count; i++)
			{
				if (_listAircraft[i] == null)
				{
					continue;
				}
				if (_listAircraft[i].transform.parent.parent.name == "FriendPanel")
				{
					if (_listAircraft[i]._fleetType == FleetType.Friend)
					{
						_listAircraft[i].EndMove(2000f, 0.8f);
					}
					else if (_listAircraft[i]._fleetType == FleetType.Enemy)
					{
						_listAircraft[i].EndMove(3000f, 0.8f);
					}
				}
				if (_listAircraft[i].transform.parent.parent.name == "EnemyPanel")
				{
					if (_listAircraft[i]._fleetType == FleetType.Friend)
					{
						_listAircraft[i].EndMove(3000f, 0.8f);
					}
					else if (_listAircraft[i]._fleetType == FleetType.Enemy)
					{
						_listAircraft[i].EndMove(2000f, 0.8f);
					}
				}
			}
			for (int j = 0; j < 2; j++)
			{
				_baseMoveTo(Vector3.zero, 1f, 0f, iTween.EaseType.linear, string.Empty, _bgTex[j].transform);
			}
		}

		private ProdAerialAircraft _instantiateAircraft(Transform target, int num, PlaneModelBase plane, FleetType fleetType)
		{
			if (_aerialAircraft == null)
			{
				_aerialAircraft = Resources.Load<ProdAerialAircraft>("Prefabs/Battle/Production/AerialCombat/Aircraft");
			}
			return ProdAerialAircraft.Instantiate(Resources.Load<ProdAerialAircraft>("Prefabs/Battle/Production/AerialCombat/Aircraft"), target, num, 0, plane, fleetType);
		}

		private void _baseMoveTo(Vector3 pos, float time, float delay, iTween.EaseType easeType, string comp, Transform trans)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("position", pos);
			hashtable.Add("isLocal", true);
			hashtable.Add("delay", delay);
			hashtable.Add("time", time);
			hashtable.Add("easeType", easeType);
			hashtable.Add("oncomplete", comp);
			hashtable.Add("oncompletetarget", base.gameObject);
			trans.MoveTo(hashtable);
		}

		private void _compAerialAttack()
		{
			_playAircraftPhase2();
		}

		private void _aerialMoveContact()
		{
			_moveCamera();
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_934);
		}

		private void _playSE(int id)
		{
		}

		private void _aerialCombatPhase1Finished()
		{
			if (_listAircraft != null)
			{
				foreach (ProdAerialAircraft item in _listAircraft)
				{
					UnityEngine.Object.Destroy(item.gameObject);
				}
				_listAircraft.Clear();
			}
			_listAircraft = null;
			_cloudPanel[0].transform.parent = base.transform;
			_cloudPanel[1].transform.parent = base.transform;
			if (_cloudParPanel[0] != null)
			{
				_cloudParPanel[0].transform.parent = base.transform;
			}
			if (_cloudParPanel[1] != null)
			{
				_cloudParPanel[1].transform.parent = base.transform;
			}
			UnityEngine.Object.Destroy(_uiPanel[0].gameObject);
			UnityEngine.Object.Destroy(_uiPanel[1].gameObject);
			if (_actCallback != null)
			{
				_actCallback();
			}
		}
	}
}
