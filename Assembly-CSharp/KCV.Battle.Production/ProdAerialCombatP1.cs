using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAerialCombatP1 : MonoBehaviour
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

		[SerializeField]
		private UIPanel[] _labelPanel;

		[SerializeField]
		private UITexture[] _supremacyTxt;

		private ProdAerialAircraft _aerialAircraft;

		private Action _actCallback;

		private KoukuuModel _clsKoukuu;

		private CutInType _iType;

		private BattleFieldCamera _fieldCamera;

		private List<ProdAerialAircraft> _listAircraft;

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
			_labelPanel = new UIPanel[2];
			_supremacyTxt = new UITexture[2];
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
					Util.FindParentToChild(ref _labelPanel[value], base.transform, $"{((FleetType)value).ToString()}LabelPanel");
					Util.FindParentToChild(ref _supremacyTxt[value], _labelPanel[value].transform, "SupremacyTxt");
				}
			}
			_labelPanel[1].SetActive(isActive: false);
			if (_iType == CutInType.Both)
			{
				_labelPanel[0].SetLayer(8);
				_labelPanel[1].SetLayer(1);
				_labelPanel[1].SetActive(isActive: true);
			}
			else if (_iType == CutInType.EnemyOnly)
			{
				_supremacyTxt[0].transform.localScale = Vector3.one;
				_labelPanel[0].SetLayer(14);
			}
			else if (_iType == CutInType.FriendOnly)
			{
				_supremacyTxt[0].transform.localScale = Vector3.one;
				_labelPanel[0].SetLayer(14);
			}
			_createAsyncAircrafts();
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiPanel);
			Mem.DelArySafe(ref _uiAirObjF);
			Mem.DelArySafe(ref _uiAirObjE);
			Mem.DelArySafe(ref _bgTex);
			Mem.DelArySafe(ref _cloudPanel);
			Mem.DelArySafe(ref _cloudParPanel);
			Mem.DelArySafe(ref _cloudPar);
			Mem.DelArySafe(ref _gunPar);
			Mem.DelArySafe(ref _labelPanel);
			Mem.DelArySafe(ref _supremacyTxt);
			Mem.Del(ref _aerialAircraft);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _clsKoukuu);
			Mem.Del(ref _iType);
			Mem.Del(ref _fieldCamera);
			Mem.DelListSafe(ref _listAircraft);
		}

		public static ProdAerialCombatP1 Instantiate(ProdAerialCombatP1 prefab, KoukuuModel model, Transform parent, CutInType iType)
		{
			ProdAerialCombatP1 prodAerialCombatP = UnityEngine.Object.Instantiate(prefab);
			prodAerialCombatP.transform.parent = parent;
			prodAerialCombatP.transform.localPosition = Vector3.zero;
			prodAerialCombatP.transform.localScale = Vector3.one;
			prodAerialCombatP._clsKoukuu = model;
			prodAerialCombatP._iType = iType;
			prodAerialCombatP._init();
			return prodAerialCombatP;
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
			setAirSupremacyLabel();
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
				_labelPanel[0].transform.localPosition = new Vector3(array[1].x, array[1].y + 272f, array[1].z);
				Transform transform = _labelPanel[1].transform;
				Vector3 localPosition = cutInCamera.transform.localPosition;
				float x = localPosition.x;
				Vector3 localPosition2 = cutInCamera.transform.localPosition;
				float y = localPosition2.y - 272f;
				Vector3 localPosition3 = cutInCamera.transform.localPosition;
				transform.localPosition = new Vector3(x, y, localPosition3.z);
				Transform transform2 = ((Component)_gunPar[0]).transform;
				Vector3 localPosition4 = ((Component)_gunPar[0]).transform.localPosition;
				transform2.localPosition = new Vector3(400f, localPosition4.y, 0f);
				Transform transform3 = ((Component)_gunPar[1]).transform;
				Vector3 localPosition5 = ((Component)_gunPar[1]).transform.localPosition;
				transform3.localPosition = new Vector3(400f, localPosition5.y, 0f);
			}
			else if (_iType == CutInType.FriendOnly)
			{
				_setParticlePanel(FleetType.Friend, cutInCamera.transform);
				_labelPanel[0].transform.localPosition = new Vector3(array[0].x, array[0].y - 175f, array[0].z);
				base.transform.position = cutInCamera.transform.position;
				_uiPanel[0].transform.localPosition = Vector3.zero;
				_uiAirObjF[0].transform.localPosition = new Vector3(-280f, 0f, 0f);
				Transform transform4 = ((Component)_gunPar[0]).transform;
				Vector3 localPosition6 = ((Component)_gunPar[0]).transform.localPosition;
				transform4.localPosition = new Vector3(0f, localPosition6.y, 0f);
				_cloudPanel[1].SetActive(isActive: false);
			}
			else if (_iType == CutInType.EnemyOnly)
			{
				_setParticlePanel(FleetType.Enemy, cutInEffectCamera.transform);
				_labelPanel[0].transform.localPosition = new Vector3(array[1].x, array[1].y - 175f, array[1].z);
				base.transform.position = cutInEffectCamera.transform.position;
				_uiPanel[1].transform.localPosition = Vector3.zero;
				_uiAirObjE[1].transform.localPosition = new Vector3(280f, 0f, 0f);
				Transform transform5 = ((Component)_gunPar[1]).transform;
				Vector3 localPosition7 = ((Component)_gunPar[1]).transform.localPosition;
				transform5.localPosition = new Vector3(0f, localPosition7.y, 0f);
				_cloudPanel[0].SetActive(isActive: false);
				cutInEffectCamera.isCulling = true;
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
				Vector3 localPosition8 = _uiPanel[j].transform.localPosition;
				float x2 = localPosition8.x;
				Vector3 localPosition9 = _uiPanel[j].transform.localPosition;
				_baseMoveTo(new Vector3(x2, 0f, localPosition9.z), 1.2f, 0.5f, iTween.EaseType.easeOutBack, string.Empty, _uiPanel[j].transform);
			}
		}

		private void setAirSupremacyLabel()
		{
			switch (_clsKoukuu.SeikuKind)
			{
			case BattleSeikuKinds.Kakuho:
				_supremacyTxt[0].mainTexture = (Resources.Load("Textures/battle/Aerial/txt_control1") as Texture2D);
				_supremacyTxt[1].mainTexture = _supremacyTxt[0].mainTexture;
				break;
			case BattleSeikuKinds.Yuusei:
				_supremacyTxt[0].mainTexture = (Resources.Load("Textures/battle/Aerial/txt_superior") as Texture2D);
				_supremacyTxt[1].mainTexture = _supremacyTxt[0].mainTexture;
				break;
			case BattleSeikuKinds.Ressei:
				_labelPanel[0].SetActive(isActive: false);
				_labelPanel[1].SetActive(isActive: false);
				_supremacyTxt[0].mainTexture = null;
				_supremacyTxt[1].mainTexture = null;
				break;
			case BattleSeikuKinds.Lost:
				_supremacyTxt[0].mainTexture = (Resources.Load("Textures/battle/Aerial/txt_control2") as Texture2D);
				_supremacyTxt[1].mainTexture = _supremacyTxt[0].mainTexture;
				break;
			case BattleSeikuKinds.None:
				_labelPanel[0].SetActive(isActive: false);
				_labelPanel[1].SetActive(isActive: false);
				_supremacyTxt[0].mainTexture = null;
				_supremacyTxt[1].mainTexture = null;
				break;
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
			for (int i = 0; i < _gunPar.Length; i++)
			{
				_gunPar[i].Play();
			}
		}

		private void _stopGunParticle()
		{
			for (int i = 0; i < _gunPar.Length; i++)
			{
				_gunPar[i].Stop();
			}
		}

		private void _createAsyncAircrafts()
		{
			_listAircraft = new List<ProdAerialAircraft>();
			Dictionary<int, UIBattleShip> dicFriendBattleShips = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> dicEnemyBattleShips = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			if (_iType == CutInType.Both || _iType == CutInType.FriendOnly)
			{
				_createAircraft(dicFriendBattleShips, FleetType.Friend, _uiAirObjF);
			}
			if (_iType == CutInType.Both || _iType == CutInType.EnemyOnly)
			{
				_createAircraft(dicEnemyBattleShips, FleetType.Enemy, _uiAirObjE);
			}
		}

		private void _createAircraft(Dictionary<int, UIBattleShip> ships, FleetType type, GameObject[] objects)
		{
			int num = (type != 0) ? 1 : 0;
			int num2 = (type == FleetType.Friend) ? 1 : 0;
			for (int i = 0; i < ships.Count; i++)
			{
				if (!(ships[i] != null) || ships[i].shipModel == null)
				{
					continue;
				}
				PlaneModelBase[] plane = _clsKoukuu.GetPlane(ships[i].shipModel.TmpId);
				if (plane == null)
				{
					continue;
				}
				for (int j = 0; j < plane.Length; j++)
				{
					if (plane[j] != null)
					{
						_listAircraft.Add(_instantiateAircraft(objects[num].transform, i, plane[j], type));
						if (_iType == CutInType.Both)
						{
							_listAircraft.Add(_instantiateAircraft(objects[num2].transform, i, plane[j], type));
						}
						break;
					}
				}
			}
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
			for (int i = 0; i < 2; i++)
			{
				Animation component = ((Component)_labelPanel[i].transform).GetComponent<Animation>();
				component.Stop();
				component.Play();
			}
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_934);
		}

		private void _playSE(int id)
		{
			if (id != 1)
			{
				return;
			}
			bool flag = false;
			ShipModel_BattleAll[] ships_f = BattleTaskManager.GetBattleManager().Ships_f;
			switch (_iType)
			{
			case CutInType.Both:
				for (int j = 0; j < ships_f.Length; j++)
				{
					if (ships_f[j] != null && ships_f[j].ClassType == 54)
					{
						flag = true;
					}
				}
				break;
			case CutInType.EnemyOnly:
				for (int i = 0; i < ships_f.Length; i++)
				{
					if (ships_f[i] != null && ships_f[i].ClassType == 54)
					{
						flag = true;
					}
				}
				break;
			}
			KCV.Utils.SoundUtils.PlaySE((!flag) ? SEFIleInfos.SE_916 : SEFIleInfos.SE_917);
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
			_actCallback();
		}
	}
}
