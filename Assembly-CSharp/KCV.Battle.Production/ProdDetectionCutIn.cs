using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdDetectionCutIn : BaseAnimation
	{
		private enum AnimationList
		{
			ProdDetectionCutIn
		}

		private static readonly string BASE_PATH = "Textures/Battle/Detection";

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private List<UITexture> _listAircraft;

		[SerializeField]
		private UITexture _uiOverlayWhite;

		[SerializeField]
		private ParticleSystem _psCloud;

		[SerializeField]
		private UINoiseScaleOutLabel _uiDetectionLabel;

		[SerializeField]
		private List<Transform> _animatingAircrafts;

		private List<Vector3> _aircraftBasePositions = new List<Vector3>();

		[SerializeField]
		private float _bNoiseSize = 10f;

		[SerializeField]
		private float _bNoiseSpeed = 10f;

		[SerializeField]
		private float _sNoiseSize = 10f;

		[SerializeField]
		private float _sNoiseSpeed = 10f;

		private DetectionProductionType _iType;

		private bool _isAircraft;

		public DetectionProductionType detectionType => _iType;

		public bool isAircraft => _isAircraft;

		public static ProdDetectionCutIn Instantiate(ProdDetectionCutIn prefab, Transform parent, SakutekiModel model)
		{
			ProdDetectionCutIn prodDetectionCutIn = UnityEngine.Object.Instantiate(prefab);
			prodDetectionCutIn.transform.parent = parent;
			prodDetectionCutIn.transform.localScale = Vector3.zero;
			prodDetectionCutIn.transform.localPosition = Vector3.zero;
			prodDetectionCutIn.setAircraft(KCV.Battle.Utils.SlotItemUtils.GetDetectionScoutingPlane(model.planes_f));
			return prodDetectionCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
			_isAircraft = false;
			if (_listAircraft == null)
			{
				_listAircraft = new List<UITexture>();
				for (int i = 0; i < 3; i++)
				{
					_listAircraft.Add(((Component)base.transform.FindChild($"AircraftAnchor{i}/Aircraft")).GetComponent<UITexture>());
				}
			}
			if (_uiBackground == null)
			{
				Util.FindParentToChild(ref _uiBackground, base.transform, "Background");
			}
			if (_uiOverlayWhite == null)
			{
				Util.FindParentToChild(ref _uiOverlayWhite, base.transform, "OverlayWhite");
			}
			_uiOverlayWhite.alpha = 1f;
			if ((UnityEngine.Object)_psCloud == null)
			{
				Util.FindParentToChild<ParticleSystem>(ref _psCloud, base.transform, "PSCloud");
			}
			_psCloud.Stop();
			_animAnimation.Stop();
			for (int j = 0; j < _animatingAircrafts.Count; j++)
			{
				_aircraftBasePositions.Add(_animatingAircrafts[j].localPosition);
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiBackground);
			Mem.DelListSafe(ref _listAircraft);
			Mem.Del(ref _uiOverlayWhite);
			Mem.Del(ref _psCloud);
			Mem.Del(ref _uiDetectionLabel);
			Mem.DelListSafe(ref _animatingAircrafts);
			Mem.DelListSafe(ref _aircraftBasePositions);
			Mem.Del(ref _bNoiseSize);
			Mem.Del(ref _bNoiseSpeed);
			Mem.Del(ref _sNoiseSize);
			Mem.Del(ref _sNoiseSpeed);
			Mem.Del(ref _iType);
			Mem.Del(ref _isAircraft);
		}

		private void Update()
		{
			for (int i = 0; i < _animatingAircrafts.Count; i++)
			{
				float num = (float)i * 46f;
				Vector3 a = _aircraftBasePositions[i];
				Transform transform = _animatingAircrafts[i];
				float y = (Mathf.PerlinNoise(Time.time * _sNoiseSpeed, 0f + num) * 2f - 1f) * _sNoiseSize;
				float x = (Mathf.PerlinNoise(Time.time * _bNoiseSpeed, 10f + num) * 2f - 1f) * _bNoiseSize;
				float y2 = (Mathf.PerlinNoise(Time.time * _bNoiseSpeed, 20f + num) * 2f - 1f) * _bNoiseSize;
				transform.localPosition = a + new Vector3(0f, y, 0f) + new Vector3(x, y2, 0f);
			}
		}

		public override void Play(Action forceCallback, Action callback)
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_946);
			base.transform.localScale = Vector3.one;
			_uiDetectionLabel.Play();
			base.Play(forceCallback, callback);
		}

		private void setAircraft(SlotitemModel_Battle model)
		{
			if (model == null)
			{
				_isAircraft = false;
				_listAircraft.ForEach(delegate(UITexture x)
				{
					x.mainTexture = null;
				});
			}
			else
			{
				_isAircraft = true;
				Texture2D mainTexture = KCV.Battle.Utils.SlotItemUtils.LoadUniDirTexture(model);
				foreach (UITexture item in _listAircraft)
				{
					item.mainTexture = mainTexture;
					item.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE[6];
				}
			}
		}

		private SlotitemModel_Battle getProdPlane(SakutekiModel model)
		{
			foreach (List<SlotitemModel_Battle> item in model.planes_f)
			{
				if (item != null)
				{
					foreach (SlotitemModel_Battle item2 in item)
					{
						if (item2 != null)
						{
							return item2;
						}
					}
				}
			}
			return null;
		}

		private void playCloudParticle()
		{
			_psCloud.Play();
		}

		private void OnAnimationAfterDiscard()
		{
			base.onAnimationFinishedAfterDiscard();
		}
	}
}
