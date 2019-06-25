using KCV.Battle.Utils;
using KCV.Generic;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdDamageCutIn : BaseBattleAnimation
	{
		public enum DamageCutInType
		{
			Moderate,
			Heavy
		}

		private enum DamageCutInList
		{
			ProdDamageCutInModerateFirst,
			ProdDamageCutInModerateSecond,
			ProdDamageCutInHeavyFirst,
			ProdDamageCutInHeavySecond
		}

		[Serializable]
		private class DamageShip : IDisposable
		{
			[SerializeField]
			private Transform _traShip;

			[SerializeField]
			private UITexture _uiShipTex;

			private List<Texture2D> _listShipTexture;

			private List<Vector3> _listShipOffs;

			private ObjectTinyShake _clsTinyShake;

			private ShipModel_Defender _clsDefender;

			private bool _isActive;

			public Transform transform
			{
				get
				{
					return _traShip;
				}
				set
				{
					_traShip = value;
				}
			}

			public UITexture shipTexture
			{
				get
				{
					return _uiShipTex;
				}
				set
				{
					_uiShipTex = value;
				}
			}

			public ObjectTinyShake tinyShake
			{
				get
				{
					if (_clsTinyShake == null)
					{
						_clsTinyShake = _uiShipTex.GetComponent<ObjectTinyShake>();
					}
					return _clsTinyShake;
				}
			}

			public ShipModel_Defender defender => _clsDefender;

			public bool isActive
			{
				get
				{
					return _isActive;
				}
				set
				{
					_isActive = value;
					transform.localScale = ((!isActive) ? Vector3.zero : Vector3.one);
				}
			}

			public DamageShip(Transform parent, string objName)
			{
				Util.FindParentToChild(ref _traShip, parent, objName);
				Util.FindParentToChild(ref _uiShipTex, _traShip, "ShipTex");
				_isActive = false;
			}

			public void Dispose()
			{
				Mem.Del(ref _traShip);
				Mem.Del(ref _uiShipTex);
				Mem.DelListSafe(ref _listShipTexture);
				Mem.DelListSafe(ref _listShipOffs);
				Mem.Del(ref _clsTinyShake);
				Mem.Del(ref _clsDefender);
				Mem.Del(ref _isActive);
			}

			public void SetShipInfos(ShipModel_Defender defender, List<Texture2D> tex, List<Vector3> offs)
			{
				_listShipTexture = tex;
				_listShipOffs = offs;
				SwitchMainTexture(isAfter: false);
			}

			public void SetShipInfos(ShipModel_Defender defender, List<Texture2D> tex, List<Vector3> offs, bool isAfter)
			{
				_listShipTexture = tex;
				_listShipOffs = offs;
				SwitchMainTexture(isAfter);
			}

			public void SwitchMainTexture(bool isAfter)
			{
				if (_listShipTexture != null && _listShipOffs != null)
				{
					int index = isAfter ? 1 : 0;
					_uiShipTex.mainTexture = _listShipTexture[index];
					_uiShipTex.MakePixelPerfect();
					_uiShipTex.transform.localPosition = _listShipOffs[index];
				}
			}
		}

		[SerializeField]
		[Button("InitMode", "[ModerateMode]", new object[]
		{
			DamageCutInType.Moderate
		})]
		private int SetModerateButton;

		[SerializeField]
		[Button("InitMode", "[HeavyMode]", new object[]
		{
			DamageCutInType.Heavy
		})]
		private int SetHeavyButton;

		[SerializeField]
		private Transform _prefabModerateOrHeavy;

		[SerializeField]
		private List<DamageShip> _listDamageShips;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiMask;

		[SerializeField]
		private Transform _traShipShakeAnchor;

		[SerializeField]
		private Transform _traShips;

		[SerializeField]
		private ParticleSystem _psModerateSmoke;

		[SerializeField]
		private ParticleSystem _psHeavyBack;

		[SerializeField]
		private List<ParticleSystem> _psHeavySmokes;

		[SerializeField]
		private ParticleSystem _psLargeExplosion;

		private int _nDrawShipNum;

		private UIPanel _uiPanel;

		private ShipModel_Defender _clsShipModel;

		private DamageCutInType _iType;

		private ModerateOrHeavyController _ctrlModerateOrHeavy;

		public UIPanel panel
		{
			get
			{
				if (_uiPanel == null)
				{
					_uiPanel = GetComponent<UIPanel>();
				}
				return _uiPanel;
			}
		}

		public static ProdDamageCutIn Instantiate(ProdDamageCutIn prefab, Transform parent)
		{
			ProdDamageCutIn prodDamageCutIn = UnityEngine.Object.Instantiate(prefab);
			prodDamageCutIn.transform.parent = parent;
			prodDamageCutIn.transform.localPosition = Vector3.zero;
			prodDamageCutIn.transform.localScale = Vector3.one;
			return prodDamageCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
			if (_uiBackground == null)
			{
				Util.FindParentToChild(ref _uiBackground, base.transform, "Background");
			}
			if (_listDamageShips == null)
			{
				_listDamageShips = new List<DamageShip>();
				for (int i = 0; i < 3; i++)
				{
					_listDamageShips.Add(new DamageShip(base.transform, $"ShipShakeAnchor/Ships/Ship{i + 1}"));
				}
			}
			Transform transform = UnityEngine.Object.Instantiate(_prefabModerateOrHeavy, Vector3.zero, Quaternion.identity) as Transform;
			transform.parent = BattleTaskManager.GetBattleCameras().cutInCamera.transform;
			transform.transform.localPosition = new Vector3(0f, 0f, -30f);
			_ctrlModerateOrHeavy = ((Component)transform).GetComponent<ModerateOrHeavyController>();
			List<ParticleSystem> list = new List<ParticleSystem>(base.transform.GetComponentsInChildren<ParticleSystem>());
			list.ForEach(delegate(ParticleSystem x)
			{
				((Component)x).SetActive(isActive: false);
			});
			_nDrawShipNum = 0;
			_psModerateSmoke.Stop();
			panel.widgetsAreStatic = true;
			base.transform.localScale = Vector3.zero;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref SetModerateButton);
			Mem.Del(ref SetHeavyButton);
			Mem.Del(ref _prefabModerateOrHeavy);
			if (_listDamageShips != null)
			{
				_listDamageShips.ForEach(delegate(DamageShip x)
				{
					x.Dispose();
				});
			}
			Mem.DelListSafe(ref _listDamageShips);
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiMask);
			Mem.Del(ref _traShipShakeAnchor);
			Mem.Del(ref _traShips);
			Mem.Del(ref _psModerateSmoke);
			Mem.Del(ref _psHeavyBack);
			Mem.DelListSafe(ref _psHeavySmokes);
			Mem.Del(ref _psLargeExplosion);
			Mem.Del(ref _nDrawShipNum);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _clsShipModel);
			Mem.Del(ref _iType);
			Mem.DelComponentSafe(ref _ctrlModerateOrHeavy);
		}

		private void LateUpdate()
		{
			_ctrlModerateOrHeavy.LateRun();
		}

		private void InitMode(DamageCutInType iType)
		{
			_iType = iType;
			Color color = (iType != 0) ? new Color(KCVColor.ColorRate(0f), KCVColor.ColorRate(8f), KCVColor.ColorRate(20f), 1f) : new Color(KCVColor.ColorRate(62f), KCVColor.ColorRate(187f), KCVColor.ColorRate(229f), 1f);
			_uiBackground.color = color;
			_uiMask.SetActive(iType == DamageCutInType.Moderate);
			((Component)_psModerateSmoke).SetActive(iType == DamageCutInType.Moderate);
			((Component)_psHeavyBack).SetActive(iType == DamageCutInType.Heavy);
		}

		public void SetShipData(List<ShipModel_Defender> defenderList, DamageCutInType iType)
		{
			InitMode(iType);
			_nDrawShipNum = defenderList.Take(_listDamageShips.Count).Count();
			_clsShipModel = defenderList[0];
			SetDamageShipData(defenderList);
		}

		private void SetDamageShipData(List<ShipModel_Defender> defenderList)
		{
			switch (defenderList.Count)
			{
			case 1:
			{
				List<Texture2D> shipTextures = (_iType != 0) ? GetHeavyShipTexture(defenderList[0]) : GetModerateShipTexture(defenderList[0]);
				List<Vector3> shipOffes = (_iType != 0) ? GetHeavyShipOffs(defenderList[0]) : GetModerateShipOffs(defenderList[0]);
				_listDamageShips.ForEach(delegate(DamageShip x)
				{
					x.SetShipInfos(defenderList[0], shipTextures, shipOffes, _iType == DamageCutInType.Heavy);
				});
				break;
			}
			case 2:
			{
				List<Texture2D> shipTextures2 = (_iType != 0) ? GetHeavyShipTexture(defenderList[0]) : GetModerateShipTexture(defenderList[0]);
				List<Vector3> shipOffes2 = (_iType != 0) ? GetHeavyShipOffs(defenderList[0]) : GetModerateShipOffs(defenderList[0]);
				_listDamageShips[0].SetShipInfos(defenderList[0], shipTextures2, shipOffes2, _iType == DamageCutInType.Heavy);
				shipTextures2 = ((_iType != 0) ? GetHeavyShipTexture(defenderList[1]) : GetModerateShipTexture(defenderList[1]));
				shipOffes2 = ((_iType != 0) ? GetHeavyShipOffs(defenderList[1]) : GetModerateShipOffs(defenderList[1]));
				_listDamageShips.Skip(1).ForEach(delegate(DamageShip x)
				{
					x.SetShipInfos(defenderList[1], shipTextures2, shipOffes2, _iType == DamageCutInType.Heavy);
				});
				break;
			}
			case 3:
			{
				int cnt = 0;
				_listDamageShips.ForEach(delegate
				{
					List<Texture2D> tex = (_iType != 0) ? GetHeavyShipTexture(defenderList[cnt]) : GetModerateShipTexture(defenderList[cnt]);
					List<Vector3> offs = (_iType != 0) ? GetHeavyShipOffs(defenderList[cnt]) : GetModerateShipOffs(defenderList[cnt]);
					_listDamageShips[cnt].SetShipInfos(defenderList[cnt], tex, offs, _iType == DamageCutInType.Heavy);
					cnt++;
				});
				break;
			}
			default:
				_listDamageShips.ForEach(delegate(DamageShip x)
				{
					x.isActive = false;
				});
				break;
			}
		}

		public override void Play(Enum iEnum, Action callback)
		{
			if (!isPlaying)
			{
				_iType = (DamageCutInType)(object)iEnum;
				panel.widgetsAreStatic = false;
				base.transform.localScale = Vector3.one;
				BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
				cutInEffectCamera.motionBlur.enabled = true;
				cutInEffectCamera.motionBlur.blurAmount = 0.3f;
				cutInEffectCamera.glowEffect.enabled = false;
				cutInEffectCamera.isCulling = true;
				_traShips.localPositionZero();
				DamageCutInList damageCutInList = (_iType != 0) ? DamageCutInList.ProdDamageCutInHeavyFirst : DamageCutInList.ProdDamageCutInModerateFirst;
				if (_iType == DamageCutInType.Moderate)
				{
					((Component)_psModerateSmoke).SetActive(isActive: true);
					_psModerateSmoke.Play();
				}
				else
				{
					((Component)_psHeavyBack).SetActive(isActive: true);
					_psHeavyBack.Play();
				}
				base.Play(damageCutInList, callback);
			}
		}

		public void Play(DamageCutInType iType, Action onStart, Action onFinished)
		{
			if (!isPlaying)
			{
				_iType = iType;
				panel.widgetsAreStatic = false;
				base.transform.localScale = Vector3.one;
				BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
				cutInEffectCamera.motionBlur.enabled = true;
				cutInEffectCamera.motionBlur.blurAmount = 0.3f;
				cutInEffectCamera.glowEffect.enabled = false;
				cutInEffectCamera.isCulling = true;
				_traShips.localPositionZero();
				DamageCutInList damageCutInList = (_iType != 0) ? DamageCutInList.ProdDamageCutInHeavyFirst : DamageCutInList.ProdDamageCutInModerateFirst;
				if (_iType == DamageCutInType.Moderate)
				{
					((Component)_psModerateSmoke).SetActive(isActive: true);
					_psModerateSmoke.Play();
				}
				else
				{
					((Component)_psHeavyBack).SetActive(isActive: true);
					_psHeavyBack.Play();
				}
				Dlg.Call(ref onStart);
				base.Play(damageCutInList, onFinished);
			}
		}

		private void onPlayDamageTextScaling()
		{
			ModerateOrHeavyController.Mode mode = (_iType != 0) ? ModerateOrHeavyController.Mode.Heavy : ModerateOrHeavyController.Mode.Moderate;
			_ctrlModerateOrHeavy.ShakeObservable.Take(1).Subscribe(delegate
			{
				DebugUtils.dbgAssert(_traShipShakeAnchor != null);
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_052);
				ObjectTinyShake component = ((Component)_traShipShakeAnchor).GetComponent<ObjectTinyShake>();
				component.PlayAnimation().Subscribe(delegate
				{
				});
			}).AddTo(base.gameObject);
			_ctrlModerateOrHeavy.PlayAnimation(mode).Subscribe(delegate
			{
			}).AddTo(base.gameObject);
		}

		private void OnPlayHeavyExplosion(int nNum)
		{
			if ((UnityEngine.Object)_psHeavySmokes[nNum] != null)
			{
				((Component)_psHeavySmokes[nNum]).SetActive(isActive: true);
				_psHeavySmokes[nNum].Play();
				Observable.Timer(TimeSpan.FromSeconds(_psHeavySmokes[nNum].duration)).Subscribe(delegate
				{
					((Component)_psHeavySmokes[nNum]).SetActive(isActive: false);
				}).AddTo(base.gameObject);
			}
			Observable.NextFrame().Subscribe(delegate
			{
				_listDamageShips[nNum].tinyShake.PlayAnimation().Subscribe();
			}).AddTo(base.gameObject);
		}

		private void OnPlayLargeExplosion()
		{
			((Component)_psLargeExplosion).SetActive(isActive: true);
			_psLargeExplosion.Play();
			Observable.Timer(TimeSpan.FromSeconds(_psLargeExplosion.duration)).Subscribe(delegate
			{
				((Component)_psLargeExplosion).SetActive(isActive: false);
			}).AddTo(base.gameObject);
			OnSetBlur(1);
		}

		private void onFirstAnimationFinished()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.motionBlur.enabled = false;
			int num = 0;
			foreach (DamageShip listDamageShip in _listDamageShips)
			{
				listDamageShip.transform.localPosition = BattleDefines.DAMAGE_CUT_IN_SHIP_DRAW_POS[_nDrawShipNum][num];
				listDamageShip.transform.localScale = Vector3.one;
				listDamageShip.shipTexture.alpha = 1f;
				num++;
			}
			_traShips.transform.localScale = Vector3.one * 7.5f;
			_traShips.transform.localPosition = Vector3.down * 70f;
			if (_clsShipModel != null)
			{
				KCV.Battle.Utils.ShipUtils.PlayDamageCutInVoice(_clsShipModel);
			}
			DamageCutInList damageCutInList = (_iType == DamageCutInType.Moderate) ? DamageCutInList.ProdDamageCutInModerateSecond : DamageCutInList.ProdDamageCutInHeavySecond;
			_animAnimation.Play(damageCutInList.ToString());
		}

		private void OnSetBlur(int isEnabled)
		{
		}

		private void OnSwitchModerateDamageShipTexture()
		{
			_listDamageShips.ForEach(delegate(DamageShip x)
			{
				x.SwitchMainTexture(isAfter: true);
			});
		}

		protected override void onAnimationFinished()
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Register(delegate
			{
				List<ParticleSystem> list = new List<ParticleSystem>(base.transform.GetComponentsInChildren<ParticleSystem>());
				list.ForEach(delegate(ParticleSystem x)
				{
					((Component)x).SetActive(isActive: false);
				});
				base.transform.localScaleZero();
				panel.widgetsAreStatic = true;
				BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
				GameObject gameObject = cutInEffectCamera.transform.Find("TorpedoLine/OverlayLine").gameObject;
				if (gameObject != null)
				{
					UITexture component = gameObject.GetComponent<UITexture>();
					if (component != null && component.alpha <= 0.1f)
					{
						cutInEffectCamera.isCulling = false;
					}
				}
				cutInEffectCamera.motionBlur.enabled = false;
				cutInEffectCamera.blur.enabled = false;
			});
			base.onAnimationFinished();
		}

		private List<Texture2D> GetModerateShipTexture(ShipModel_Defender defender)
		{
			List<Texture2D> list = new List<Texture2D>();
			list.Add(KCV.Battle.Utils.ShipUtils.LoadTexture(defender, isAfter: false));
			list.Add(KCV.Battle.Utils.ShipUtils.LoadTexture(defender, isAfter: true));
			return list;
		}

		private List<Vector3> GetModerateShipOffs(ShipModel_Defender defender)
		{
			List<Vector3> list = new List<Vector3>();
			list.Add(KCV.Battle.Utils.ShipUtils.GetShipOffsPos(defender, isDamaged: false, MstShipGraphColumn.CutInSp1));
			list.Add(KCV.Battle.Utils.ShipUtils.GetShipOffsPos(defender, isDamaged: true, MstShipGraphColumn.CutInSp1));
			return list;
		}

		private List<Texture2D> GetHeavyShipTexture(ShipModel_Defender defender)
		{
			List<Texture2D> list = new List<Texture2D>();
			list.Add(null);
			list.Add(KCV.Battle.Utils.ShipUtils.LoadTexture(defender, isAfter: true));
			return list;
		}

		private List<Vector3> GetHeavyShipOffs(ShipModel_Defender defender)
		{
			List<Vector3> list = new List<Vector3>();
			list.Add(Vector3.zero);
			list.Add(KCV.Battle.Utils.ShipUtils.GetShipOffsPos(defender, isDamaged: true, MstShipGraphColumn.CutInSp1));
			return list;
		}
	}
}
