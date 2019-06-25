using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	public class ProdSinking : BaseAnimation
	{
		public enum SinkingType
		{
			None,
			ProdSinkingRepairGoddess,
			ProdSinkingRepairTeam,
			ProdSinking
		}

		[SerializeField]
		private UISprite _uiLightLine;

		[SerializeField]
		private UISprite _uiMask;

		[SerializeField]
		private UISprite _uiRipple;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiBrightSpot;

		[SerializeField]
		private UITexture _uiOverlay;

		[SerializeField]
		private UITexture _uiRepairCard;

		[SerializeField]
		private UITexture _uiShipTexture;

		[SerializeField]
		private ParticleSystem _psSinkingSmoke;

		[SerializeField]
		private List<UISprite> _listDrops;

		[SerializeField]
		private List<UISprite> _listLostMessages;

		private Action _actOnRestore;

		private UIPanel _uiPanel;

		private List<Vector3> _listShipOffs;

		private List<Texture2D> _listShipTexture;

		private ShipModel_Defender _clsShipModel;

		private SinkingType _iType;

		public ShipModel_Defender shipModel => _clsShipModel;

		public SinkingType sinkingType => _iType;

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

		private bool isRepair => _iType == SinkingType.ProdSinkingRepairGoddess || _iType == SinkingType.ProdSinkingRepairTeam;

		public static ProdSinking Instantiate(ProdSinking prefab, Transform parent)
		{
			ProdSinking prodSinking = UnityEngine.Object.Instantiate(prefab);
			prodSinking.transform.parent = parent;
			prodSinking.transform.localScaleZero();
			prodSinking.transform.localPositionZero();
			return prodSinking;
		}

		protected override void Awake()
		{
			base.Awake();
			_iType = SinkingType.None;
			_listShipTexture = new List<Texture2D>(2);
			_listShipOffs = new List<Vector3>(2);
			panel.widgetsAreStatic = true;
			((Component)_psSinkingSmoke).SetActive(isActive: false);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiLightLine);
			Mem.Del(ref _uiMask);
			Mem.Del(ref _uiRipple);
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiBrightSpot);
			Mem.Del(ref _uiOverlay);
			Mem.Del(ref _uiRepairCard);
			Mem.Del(ref _uiShipTexture);
			Mem.Del(ref _psSinkingSmoke);
			if (_listDrops != null)
			{
				_listDrops.ForEach(delegate(UISprite x)
				{
					x.Clear();
				});
			}
			Mem.DelListSafe(ref _listDrops);
			if (_listLostMessages != null)
			{
				_listLostMessages.ForEach(delegate(UISprite x)
				{
					x.Clear();
				});
			}
			Mem.DelListSafe(ref _listLostMessages);
			Mem.Del(ref _actOnRestore);
			Mem.Del(ref _uiPanel);
			Mem.DelListSafe(ref _listShipOffs);
			Mem.DelListSafe(ref _listShipTexture);
			Mem.Del(ref _clsShipModel);
			Mem.Del(ref _iType);
		}

		public void SetSinkingData(ShipModel_Defender ship)
		{
			_clsShipModel = ship;
			_iType = GetSinkingType(ship);
			_listShipTexture = KCV.Battle.Utils.ShipUtils.LoadTexture2Sinking(ship, isRepair);
			_listShipOffs = KCV.Battle.Utils.ShipUtils.GetShipOffsPos2Sinking(ship, isRepair, MstShipGraphColumn.CutInSp1);
			_uiShipTexture.mainTexture = _listShipTexture[0];
			_uiShipTexture.MakePixelPerfect();
			_uiShipTexture.transform.localPosition = _listShipOffs[0];
		}

		private void SetRepairCard(SinkingType iType)
		{
			switch (iType)
			{
			case SinkingType.None:
			case SinkingType.ProdSinking:
				_uiRepairCard.mainTexture = null;
				_uiRepairCard.localSize = Vector3.zero;
				break;
			case SinkingType.ProdSinkingRepairTeam:
				_uiRepairCard.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(42, 1);
				_uiRepairCard.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE[1];
				break;
			case SinkingType.ProdSinkingRepairGoddess:
				_uiRepairCard.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.SlotItemTexture.Load(43, 1);
				_uiRepairCard.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE[1];
				break;
			}
		}

		public override void Play(Action callback)
		{
			Play(null, null, callback);
		}

		public void Play(Action onStart, Action onRestore, Action onFinished)
		{
			if (_iType == SinkingType.None)
			{
				onFinished?.Invoke();
				return;
			}
			_actOnRestore = onRestore;
			base.transform.localScaleOne();
			SetRepairCard(_iType);
			panel.widgetsAreStatic = false;
			base.Play(_iType, onFinished);
			Dlg.Call(ref onStart);
		}

		private SinkingType GetSinkingType(ShipModel_Defender model)
		{
			switch (model.DamageEventAfter)
			{
			case DamagedStates.Gekichin:
				return SinkingType.ProdSinking;
			case DamagedStates.Megami:
				return SinkingType.ProdSinkingRepairGoddess;
			case DamagedStates.Youin:
				return SinkingType.ProdSinkingRepairTeam;
			default:
				return SinkingType.None;
			}
		}

		private void PlaySmoke()
		{
			((Component)_psSinkingSmoke).SetActive(isActive: true);
			_psSinkingSmoke.Play();
		}

		private void PlaySinkingVoice()
		{
			KCV.Battle.Utils.ShipUtils.PlaySinkingVoice(_clsShipModel);
		}

		private void RestoredShip()
		{
			_uiShipTexture.mainTexture = _listShipTexture[1];
			_uiShipTexture.MakePixelPerfect();
			_uiShipTexture.transform.localPosition = _listShipOffs[1];
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.Restored(_clsShipModel);
			Dlg.Call(ref _actOnRestore);
		}

		protected override void onAnimationFinished()
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Register(delegate
			{
				((Component)_psSinkingSmoke).SetActive(isActive: false);
				base.transform.localScaleZero();
				panel.widgetsAreStatic = true;
			});
			base.onAnimationFinished();
		}

		private void PlayDamageSE()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_053);
		}

		private void CheckTrophy()
		{
			BattleTaskManager.GetBattleManager().IncrementRecoveryItemCountWithTrophyUnlock();
		}
	}
}
