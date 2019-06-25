using KCV.Battle.Utils;
using KCV.Generic;
using Librarys.UnitySettings;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleField : MonoBehaviour
	{
		[SerializeField]
		private List<Transform> _traSeaLevelList;

		[SerializeField]
		private Transform _traFieldCenter;

		[SerializeField]
		private List<Transform> _traFleetAnchorList;

		[SerializeField]
		private List<Material> _listFriendSeaLevels;

		[SerializeField]
		private List<Material> _listEnemySeaLevels;

		[SerializeField]
		private List<Material> _listMatSkyboxes;

		[SerializeField]
		private List<Texture2D> _listTexReflectives;

		private KCV.Battle.Utils.TimeZone _iTimeZone;

		private Dictionary<FleetType, Transform> _dicFleetAnchor;

		private Dictionary<FleetType, Vector3> _dicFleetAnchorOrigine;

		private Dictionary<FleetType, Vector4> _dicWaveDirs;

		private Dictionary<FleetType, ParticleSystem> _dicPSClouds;

		private Dictionary<FleetType, Water> _dicSeaLevels;

		private Dictionary<CameraAnchorType, Dictionary<FleetType, Transform>> _dicCameraAnchors;

		public Transform fieldCenter => _traFieldCenter;

		public Dictionary<FleetType, Transform> dicFleetAnchor => _dicFleetAnchor;

		public KCV.Battle.Utils.TimeZone timeZoon => _iTimeZone;

		public Water seaLevel
		{
			get
			{
				return _dicSeaLevels[FleetType.Friend];
			}
			set
			{
				_dicSeaLevels[FleetType.Friend] = value;
			}
		}

		public Water enemySeaLevel
		{
			get
			{
				return _dicSeaLevels[FleetType.Enemy];
			}
			set
			{
				_dicSeaLevels[FleetType.Enemy] = value;
			}
		}

		public Dictionary<FleetType, Water> seaLevels => _dicSeaLevels;

		public Vector3 seaLevelPos => _dicSeaLevels[FleetType.Friend].transform.position;

		public bool isEnemySeaLevelActive
		{
			get
			{
				return _dicSeaLevels[FleetType.Enemy].gameObject.activeInHierarchy;
			}
			set
			{
				_dicSeaLevels[FleetType.Enemy].SetActive(value);
			}
		}

		public Dictionary<FleetType, ParticleSystem> dicParticleClouds => _dicPSClouds;

		public Dictionary<CameraAnchorType, Dictionary<FleetType, Transform>> dicCameraAnchors => _dicCameraAnchors;

		public Dictionary<FleetType, Vector3> fleetAnchorOrigine => _dicFleetAnchorOrigine;

		private void Awake()
		{
			_iTimeZone = KCV.Battle.Utils.TimeZone.DayTime;
			if (_traFieldCenter == null)
			{
				Util.FindParentToChild(ref _traFieldCenter, base.transform, "CenterAnchor");
			}
			_dicFleetAnchor = new Dictionary<FleetType, Transform>();
			int num = 0;
			foreach (Transform traFleetAnchor in _traFleetAnchorList)
			{
				_dicFleetAnchor.Add((FleetType)num, traFleetAnchor);
				num++;
			}
			_dicCameraAnchors = new Dictionary<CameraAnchorType, Dictionary<FleetType, Transform>>();
			Dictionary<FleetType, Transform> dictionary = new Dictionary<FleetType, Transform>();
			foreach (int value in Enum.GetValues(typeof(FleetType)))
			{
				if (value != 2)
				{
					dictionary.Add((FleetType)value, base.transform.FindChild($"CameraAnchors/{((FleetType)value).ToString()}OneRowAnchor").transform);
				}
			}
			_dicCameraAnchors.Add(CameraAnchorType.OneRowAnchor, dictionary);
			_dicFleetAnchorOrigine = new Dictionary<FleetType, Vector3>();
			_dicFleetAnchorOrigine.Add(FleetType.Friend, _dicFleetAnchor[FleetType.Friend].transform.position);
			_dicFleetAnchorOrigine.Add(FleetType.Enemy, _dicFleetAnchor[FleetType.Enemy].transform.position);
			_dicSeaLevels = new Dictionary<FleetType, Water>();
			int num2 = 0;
			foreach (Transform traSeaLevel in _traSeaLevelList)
			{
				_dicSeaLevels.Add((FleetType)num2, ((Component)traSeaLevel).GetComponent<Water>());
				_dicSeaLevels[(FleetType)num2].m_WaterMode = Water.WaterMode.Reflective;
				_dicSeaLevels[(FleetType)num2].waveScale = 0.02f;
				_dicSeaLevels[(FleetType)num2].reflectionDistort = 1.5f;
				num2++;
			}
			_dicSeaLevels[FleetType.Enemy].SetLayer(Generics.Layers.SplitWater.IntLayer());
			isEnemySeaLevelActive = false;
			_dicWaveDirs = new Dictionary<FleetType, Vector4>();
			_dicWaveDirs.Add(FleetType.Friend, new Vector4(-3.58f, -22.85f, 1f, -100f));
			_dicWaveDirs.Add(FleetType.Enemy, new Vector4(3.58f, 22.85f, -1f, 100f));
			_dicPSClouds = new Dictionary<FleetType, ParticleSystem>();
			foreach (int value2 in Enum.GetValues(typeof(FleetType)))
			{
				if (value2 != 2)
				{
					ParticleSystem val = ParticleFile.Instantiate<ParticleSystem>(ParticleFileInfos.BattleAdventFleetCloud);
					((UnityEngine.Object)val).name = $"Cloud{(FleetType)value2}";
					((Component)val).transform.parent = base.transform;
					((Component)val).SetRenderQueue(3500);
					((Component)val).transform.localScale = Vector3.one;
					((Component)val).transform.position = Vector3.zero;
					val.playOnAwake = false;
					((Component)val).SetActive(isActive: false);
					_dicPSClouds.Add((FleetType)value2, val);
				}
			}
		}

		private void OnDestroy()
		{
			Transform meshTrans = null;
			if (_traSeaLevelList != null)
			{
				_traSeaLevelList.ForEach(delegate(Transform x)
				{
					meshTrans = x;
					Mem.DelMeshSafe(ref meshTrans);
				});
			}
			Mem.DelListSafe(ref _traSeaLevelList);
			Mem.Del(ref _traFieldCenter);
			Mem.DelListSafe(ref _traFleetAnchorList);
			Mem.DelListSafe(ref _listFriendSeaLevels);
			Mem.DelListSafe(ref _listEnemySeaLevels);
			Mem.DelListSafe(ref _listMatSkyboxes);
			Mem.DelListSafe(ref _listTexReflectives);
			Mem.Del(ref _iTimeZone);
			Mem.DelDictionarySafe(ref _dicFleetAnchor);
			Mem.DelDictionarySafe(ref _dicFleetAnchorOrigine);
			Mem.DelDictionarySafe(ref _dicWaveDirs);
			Mem.DelDictionarySafe(ref _dicPSClouds);
			Mem.DelDictionarySafe(ref _dicSeaLevels);
			if (_dicCameraAnchors != null)
			{
				_dicCameraAnchors.ForEach(delegate(KeyValuePair<CameraAnchorType, Dictionary<FleetType, Transform>> x)
				{
					x.Value.Clear();
				});
			}
			Mem.DelDictionarySafe(ref _dicCameraAnchors);
			Mem.Del(ref meshTrans);
		}

		public void ReqTimeZone(KCV.Battle.Utils.TimeZone iTime, SkyType iSkyType)
		{
			_fogSettings(iTime);
			Color seaColor = GetSeaColor(iTime, iSkyType);
			foreach (KeyValuePair<FleetType, Water> dicSeaLevel in _dicSeaLevels)
			{
				dicSeaLevel.Value.reflectionColorTexture = _listTexReflectives[(int)iTime];
				dicSeaLevel.Value.GetComponent<MeshRenderer>().material = (dicSeaLevel.Key != 0) ? _listEnemySeaLevels[(int)iTime] : _listFriendSeaLevels[(int)iTime];
				dicSeaLevel.Value.GetComponent<MeshRenderer>().material.SetColor("_PostMultiplyColor", seaColor);
			}
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.fieldCameras.ForEach(delegate(BattleFieldCamera x)
			{
				x.skybox.material = _listMatSkyboxes[(int)iTime];
			});
			battleCameras.fieldDimCamera.skybox.material = _listMatSkyboxes[(int)iTime];
			_iTimeZone = iTime;
		}

		public void AlterWaveDirection(FleetType iType)
		{
			_dicSeaLevels[FleetType.Friend].waveSpeed = _dicWaveDirs[iType];
		}

		public void AlterWaveDirection(FleetType iFleetType, FleetType iWaveType)
		{
			_dicSeaLevels[iFleetType].waveSpeed = _dicWaveDirs[iWaveType];
		}

		public void ResetFleetAnchorPosition()
		{
			foreach (int value in Enum.GetValues(typeof(FleetType)))
			{
				if (value != 2)
				{
					_dicFleetAnchor[(FleetType)value].transform.position = _dicFleetAnchorOrigine[(FleetType)value];
					_dicFleetAnchor[(FleetType)value].transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
					_dicFleetAnchor[(FleetType)value].transform.localScaleOne();
				}
			}
		}

		private void _fogSettings(KCV.Battle.Utils.TimeZone iTime)
		{
			Fog.fog = true;
			Fog.fogMode = FogMode.Linear;
			Fog.fogDensity = 0.14f;
			Fog.fogStartDistance = 20f;
			Fog.fogEndDistance = 130f;
			Color color2 = Fog.fogColor = ((iTime != 0) ? new Color(Mathe.Rate(0f, 255f, 65f), Mathe.Rate(0f, 255f, 129f), Mathe.Rate(0f, 255f, 161f), Mathe.Rate(0f, 255f, 255f)) : new Color(Mathe.Rate(0f, 255f, 187f), Mathe.Rate(0f, 255f, 229f), Mathe.Rate(0f, 255f, 240f), Mathe.Rate(0f, 255f, 255f)));
		}

		private Color GetSeaColor(KCV.Battle.Utils.TimeZone iTime, SkyType iSkyType)
		{
			Color white = Color.white;
			if (iSkyType == SkyType.Normal)
			{
				return KCVColor.ConvertColor(90f, 173f, 177f, 255f);
			}
			int length = Enum.GetValues(typeof(SkyType)).Length;
			SkyType skyType;
			switch (iSkyType)
			{
			case SkyType.FinalArea171:
				skyType = SkyType.FinalArea172;
				break;
			case SkyType.FinalArea172:
				skyType = SkyType.FinalArea173;
				break;
			case SkyType.FinalArea173:
				skyType = SkyType.FinalArea174;
				break;
			case SkyType.FinalArea174:
				skyType = SkyType.FinalArea174;
				break;
			default:
				skyType = SkyType.FinalArea174;
				break;
			}
			float t = (float)skyType / (float)(length - 1);
			return KCVColor.ConvertColor(Mathe.Lerp(90f, 255f, t), Mathe.Lerp(173f, 68f, t), Mathe.Lerp(177f, 68f, t), 255f);
		}

		private Material GetSkyboxMaterial(KCV.Battle.Utils.TimeZone iTime, SkyType iSkyType)
		{
			object result;
			switch (iSkyType)
			{
			case SkyType.Normal:
				return (iTime != 0) ? _listMatSkyboxes[1] : _listMatSkyboxes[0];
			case SkyType.FinalArea171:
			case SkyType.FinalArea172:
				result = _listMatSkyboxes[2];
				break;
			default:
				result = _listMatSkyboxes[3];
				break;
			}
			return (Material)result;
		}
	}
}
