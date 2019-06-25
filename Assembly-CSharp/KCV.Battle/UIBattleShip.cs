using KCV.Battle.Utils;
using Librarys.InspectorExtension;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class UIBattleShip : MonoBehaviour
	{
		public enum AnimationName
		{
			ProdShellingNormalAttack,
			ProdTranscendenceAttack
		}

		[Serializable]
		private class Wakes : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private ParticleSystem _psShipSpray;

			[SerializeField]
			private ParticleSystem _psSinkSpray;

			public Transform transform => _tra;

			public ParticleSystem shipSpray => _psShipSpray;

			public ParticleSystem sinkSpray => _psSinkSpray;

			public Wakes(Transform transform)
			{
				if (_tra == null)
				{
					_tra = transform;
				}
				if ((UnityEngine.Object)_psShipSpray == null)
				{
					Util.FindParentToChild<ParticleSystem>(ref _psShipSpray, this.transform, "ShipSpray");
				}
				if ((UnityEngine.Object)_psSinkSpray == null)
				{
					Util.FindParentToChild<ParticleSystem>(ref _psSinkSpray, this.transform, "SinkSpray");
				}
			}

			public void Dispose()
			{
				Transform p = ((Component)_psShipSpray).transform;
				Mem.DelMeshSafe(ref p);
				p = ((Component)_psShipSpray).transform;
				Mem.DelMeshSafe(ref p);
				foreach (Transform item in ((Component)_psSinkSpray).transform)
				{
					p = item;
					Mem.DelMeshSafe(ref p);
				}
				Mem.Del(ref _tra);
				Mem.Del(ref _psShipSpray);
				Mem.Del(ref _psSinkSpray);
				Mem.Del(ref p);
			}
		}

		[SerializeField]
		private BillboardObject _uiBillboard;

		[SerializeField]
		private Transform _traPog;

		[SerializeField]
		private Transform _traShipAnchor;

		[SerializeField]
		private Transform _traTorpedoAnchor;

		[SerializeField]
		private Transform _traSPPog;

		[SerializeField]
		private Animation _animShipAnimation;

		[SerializeField]
		private ShipModel_Battle _clsShip;

		[SerializeField]
		private Object3D _clsObject3D;

		[SerializeField]
		private Wakes _clsWakes;

		private FleetType _iFleetType;

		private StandingPositionType _iStandingPositionType;

		private Dictionary<StandingPositionType, Vector3> _dicStandingPos;

		private ShipDrawType _iShipDrawType;

		public ShipModel_Battle shipModel
		{
			get
			{
				if (_clsShip != null)
				{
					return _clsShip;
				}
				return null;
			}
		}

		public Vector3 pointOfGaze => _traPog.position;

		public Vector3 localPointOfGaze
		{
			get
			{
				return _traPog.localPosition;
			}
			set
			{
				if (_traPog.localPosition != value)
				{
					_traPog.localPosition = value;
				}
			}
		}

		public Vector3 difPointOfGazeFmFleet => pointOfGaze - base.transform.position;

		public Vector3 spPointOfGaze => _traSPPog.position;

		public Vector3 localSPPointOfGaze
		{
			get
			{
				return _traSPPog.localPosition;
			}
			set
			{
				if (_traSPPog.localPosition != value)
				{
					_traSPPog.localPosition = value;
				}
			}
		}

		public Vector3 difSPPointOfGazeFmFleet => spPointOfGaze - base.transform.position;

		public Vector3 torpedoAnchor => _traTorpedoAnchor.position;

		public BillboardObject billboard => _uiBillboard;

		public FleetType fleetType => _iFleetType;

		public Object3D object3D => _clsObject3D;

		public Dictionary<StandingPositionType, Vector3> dicStandingPos => _dicStandingPos;

		public Generics.Layers layer
		{
			get
			{
				if (object3D == null)
				{
					return Generics.Layers.Nothing;
				}
				return (Generics.Layers)object3D.gameObject.layer;
			}
			set
			{
				if (base.gameObject != null)
				{
					object3D.SetLayer(value.IntLayer());
				}
			}
		}

		public StandingPositionType standingPositionType
		{
			get
			{
				return _iStandingPositionType;
			}
			set
			{
				if (_dicStandingPos.ContainsKey(value) && base.transform.localPosition != _dicStandingPos[value])
				{
					_iStandingPositionType = value;
					base.transform.localPosition = _dicStandingPos[value];
				}
			}
		}

		public ShipDrawType drawType
		{
			get
			{
				return _iShipDrawType;
			}
			set
			{
				if (_iShipDrawType != value)
				{
					switch (value)
					{
					case ShipDrawType.Normal:
						object3D.color = Color.white;
						break;
					case ShipDrawType.Silhouette:
						object3D.color = new Color(Mathe.Rate(0f, 255f, 20f), Mathe.Rate(0f, 255f, 20f), Mathe.Rate(0f, 255f, 20f), 1f);
						break;
					}
					_iShipDrawType = value;
				}
			}
		}

		public static UIBattleShip Instantiate(UIBattleShip prefab, Transform parent)
		{
			UIBattleShip uIBattleShip = UnityEngine.Object.Instantiate(prefab);
			uIBattleShip.transform.parent = parent;
			uIBattleShip.Init();
			return uIBattleShip;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiBillboard);
			Mem.Del(ref _traPog);
			Mem.Del(ref _traShipAnchor);
			Mem.Del(ref _traTorpedoAnchor);
			Mem.Del(ref _traSPPog);
			Mem.Del(ref _animShipAnimation);
			Mem.Del(ref _clsShip);
			Mem.Del(ref _clsObject3D);
			Mem.DelIDisposableSafe(ref _clsWakes);
			Mem.Del(ref _iFleetType);
			Mem.Del(ref _iStandingPositionType);
			Mem.DelDictionarySafe(ref _dicStandingPos);
			Mem.Del(ref _iShipDrawType);
		}

		private bool Init()
		{
			if (_uiBillboard == null)
			{
				_uiBillboard = GetComponent<BillboardObject>();
			}
			_uiBillboard.isBillboard = true;
			if (_traPog == null)
			{
				Util.FindParentToChild(ref _traPog, base.transform, "POG");
			}
			if (_traSPPog == null)
			{
				Util.FindParentToChild(ref _traSPPog, base.transform, "SPPog");
			}
			if (_traTorpedoAnchor == null)
			{
				Util.FindParentToChild(ref _traTorpedoAnchor, base.transform, "TorpedoAnchor");
			}
			if (_clsObject3D == null)
			{
				Util.FindParentToChild(ref _clsObject3D, base.transform, "ShipAnchor/Object3D");
			}
			if ((UnityEngine.Object)_animShipAnimation == null)
			{
				_animShipAnimation = GetComponent<Animation>();
			}
			_animShipAnimation.playAutomatically = false;
			_animShipAnimation.Stop();
			_iStandingPositionType = StandingPositionType.Free;
			_dicStandingPos = new Dictionary<StandingPositionType, Vector3>();
			_iShipDrawType = ShipDrawType.Normal;
			return true;
		}

		public void SetStandingPosition(StandingPositionType iType)
		{
			if (_dicStandingPos.ContainsKey(iType) && base.transform.localPosition != _dicStandingPos[iType])
			{
				_iStandingPositionType = iType;
				base.transform.localPosition = _dicStandingPos[iType];
			}
		}

		public void TorpedoSalvoWakeAngle(bool isSet)
		{
			if (_clsShip != null)
			{
				Vector3 localEulerAngles = (!isSet) ? Vector3.zero : ((!_clsShip.IsFriend()) ? BattleDefines.BATTLESHIP_TORPEDOSALVO_WAKE_ANGLE[1] : BattleDefines.BATTLESHIP_TORPEDOSALVO_WAKE_ANGLE[0]);
				_clsWakes.transform.localEulerAngles = localEulerAngles;
			}
		}

		public void UpdateDamage()
		{
		}

		public void UpdateDamage(ShipModel_Defender model)
		{
			SetShipTexture(model, isAfter: true);
			SetPointOfGaze(model, isAfter: true);
			SetSPPointOfGaze(model, isAfter: true);
		}

		public void PlayShipAnimation(AnimationName iName)
		{
			_animShipAnimation.Play(iName.ToString());
		}

		public void PlayProtectAnimation()
		{
			Material material = new Material(Resources.Load("Textures/battle/Torpedo/ProtectShip") as Material);
			_clsObject3D.material = material;
			_clsObject3D.meshRenderer.sharedMaterial = material;
			_clsObject3D.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f));
			base.transform.LTValue(0.5f, 1f, 1f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_clsObject3D.material.SetColor("_TintColor", new Color(x, x, x));
			})
				.setOnComplete(_protectAfterAnimation);
		}

		private void _protectAfterAnimation()
		{
			base.transform.LTValue(1f, 0.5f, 1f).setEase(LeanTweenType.easeOutExpo).setOnUpdate(delegate(float x)
			{
				_clsObject3D.material.SetColor("_TintColor", new Color(x, x, x));
			})
				.setOnComplete(_endProtectAnimation);
		}

		private void _endProtectAnimation()
		{
			_clsObject3D.material = (Resources.Load("Materials/Battle/Ship") as Material);
			_clsObject3D.color = new Color(1f, 1f, 1f);
		}

		public void PlayProdSinking(Action callback)
		{
			((Component)_clsWakes.shipSpray).SetActive(isActive: false);
			((Component)_clsWakes.shipSpray).SetActive(isActive: true);
			_clsWakes.shipSpray.Play();
			Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
			{
				object3D.color = Color.gray;
				Vector3 localPosition = base.transform.localPosition;
				localPosition.y = -5f;
				base.transform.LTMoveLocal(localPosition, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[2]).setEase(LeanTweenType.easeInQuad).setOnComplete((Action)delegate
				{
					Dlg.Call(ref callback);
					base.transform.SetActive(isActive: false);
				});
				object3D.transform.LTRotateAround(Vector3.back, 30f, BattleDefines.SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME[2]).setEase(LeanTweenType.easeInQuad);
			});
		}

		public void Restored(ShipModel_Defender defender)
		{
			object3D.transform.LTCancel();
			SetShipTextureRestore(defender);
			SetPointOfGaze2Restore(defender);
			SetSPPointOfGaze2Restore(defender);
			((Component)_clsWakes.shipSpray).SetActive(isActive: true);
			_clsWakes.shipSpray.Play();
			((Component)_clsWakes.sinkSpray).SetActive(isActive: false);
			object3D.color = Color.white;
			standingPositionType = StandingPositionType.OneRow;
			object3D.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}

		public void SetShipInfos(ShipModel_BattleAll model, bool isStart)
		{
			if (model == null)
			{
				_clsShip = model;
				this.SetActive(isActive: false);
				return;
			}
			_clsShip = model;
			_iFleetType = ((!model.IsFriend()) ? FleetType.Enemy : FleetType.Friend);
			SetShipTexture(model, isStart);
			SetPointOfGaze(model, isStart);
			SetSPPointOfGaze(model, isStart);
			base.name = model.Name;
		}

		private void SetShipTexture(ShipModel_BattleAll model, bool isStart)
		{
			bool isDamaged = (!isStart) ? model.DamagedFlgEnd : model.DamagedFlgStart;
			int shipStandingTexID = ShipUtils.GetShipStandingTexID(model.IsFriend(), model.IsPractice(), isDamaged);
			if (!(_clsObject3D.mainTexture != null) || !(_clsObject3D.mainTexture.name == shipStandingTexID.ToString()))
			{
				_clsObject3D.mainTexture = ShipUtils.LoadTexture(model, isStart);
				_clsObject3D.MakePixelPerfect();
				_clsObject3D.transform.localScale = _clsObject3D.transform.localScale * (float)model.Offsets.GetScaleMag_InBattle(model.DamagedFlgStart);
				_clsObject3D.transform.localPosition = ShipUtils.GetShipOffsPos(model, isDamaged, MstShipGraphColumn.Foot);
			}
		}

		private void SetShipTexture(ShipModel_Defender model, bool isAfter)
		{
			bool isDamaged = (!isAfter) ? model.DamagedFlgBefore : model.DamagedFlgAfter;
			int shipStandingTexID = ShipUtils.GetShipStandingTexID(model.IsFriend(), model.IsPractice(), isDamaged);
			if (!(_clsObject3D.mainTexture != null) || !(_clsObject3D.mainTexture.name == shipStandingTexID.ToString()))
			{
				_clsObject3D.mainTexture = ShipUtils.LoadTexture(model, isAfter);
				_clsObject3D.MakePixelPerfect();
				_clsObject3D.transform.localScale = _clsObject3D.transform.localScale * (float)model.Offsets.GetScaleMag_InBattle(model.DamagedFlgAfter);
				_clsObject3D.transform.localPosition = ShipUtils.GetShipOffsPos(model, isDamaged, MstShipGraphColumn.Foot);
			}
		}

		private void SetShipTextureRestore(ShipModel_Defender model)
		{
			bool damagedFlgAfterRecovery = model.DamagedFlgAfterRecovery;
			int shipStandingTexID = ShipUtils.GetShipStandingTexID(model.IsFriend(), model.IsPractice(), damagedFlgAfterRecovery);
			if (!(_clsObject3D.mainTexture != null) || !(_clsObject3D.mainTexture.name == shipStandingTexID.ToString()))
			{
				_clsObject3D.mainTexture = ShipUtils.LoadTexture2Restore(model);
				_clsObject3D.MakePixelPerfect();
				_clsObject3D.transform.localScale = _clsObject3D.transform.localScale * (float)model.Offsets.GetScaleMag_InBattle(model.DamagedFlgAfter);
				_clsObject3D.transform.localPosition = ShipUtils.GetShipOffsPos(model, damagedFlgAfterRecovery, MstShipGraphColumn.Foot);
			}
		}

		private void SetPointOfGaze(ShipModel_BattleAll model, bool isStart)
		{
			bool isDamaged = (!isStart) ? model.DamagedFlgEnd : model.DamagedFlgStart;
			_traPog.transform.localPosition = ShipUtils.GetShipOffsPos(model, isDamaged, MstShipGraphColumn.PointOfGaze);
		}

		private void SetPointOfGaze(ShipModel_Defender model, bool isAfter)
		{
			bool isDamaged = (!isAfter) ? model.DamagedFlgBefore : model.DamagedFlgAfter;
			_traPog.transform.localPosition = ShipUtils.GetShipOffsPos(model, isDamaged, MstShipGraphColumn.PointOfGaze);
		}

		private void SetPointOfGaze2Restore(ShipModel_Defender model)
		{
			bool damagedFlgAfterRecovery = model.DamagedFlgAfterRecovery;
			_traPog.transform.localPosition = ShipUtils.GetShipOffsPos(model, damagedFlgAfterRecovery, MstShipGraphColumn.PointOfGaze);
		}

		public void SetSPPointOfGaze(ShipModel_BattleAll model, bool isStart)
		{
			bool isDamaged = (!isStart) ? model.DamagedFlgEnd : model.DamagedFlgStart;
			_traSPPog.transform.localPosition = ShipUtils.GetShipOffsPos(model, isDamaged, MstShipGraphColumn.SPPointOfGaze);
		}

		public void SetSPPointOfGaze(ShipModel_Defender model, bool isAfter)
		{
			bool isDamaged = (!isAfter) ? model.DamagedFlgBefore : model.DamagedFlgAfter;
			_traSPPog.transform.localPosition = ShipUtils.GetShipOffsPos(model, isDamaged, MstShipGraphColumn.SPPointOfGaze);
		}

		public void SetSPPointOfGaze2Restore(ShipModel_Defender model)
		{
			bool damagedFlgAfterRecovery = model.DamagedFlgAfterRecovery;
			_traSPPog.transform.localPosition = ShipUtils.GetShipOffsPos(model, damagedFlgAfterRecovery, MstShipGraphColumn.SPPointOfGaze);
		}

		public void SetSprayColor()
		{
			ParticleSystem component = ((Component)base.transform.FindChild("ShipSpray")).GetComponent<ParticleSystem>();
			if (BattleTaskManager.GetTimeZone() == KCV.Battle.Utils.TimeZone.DayTime)
			{
				((Component)component).GetComponent<Renderer>().material.shader = SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList[5];
			}
			else if (BattleTaskManager.GetTimeZone() == KCV.Battle.Utils.TimeZone.Night)
			{
				((Component)component).GetComponent<Renderer>().material.shader = SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList[6];
			}
		}
	}
}
