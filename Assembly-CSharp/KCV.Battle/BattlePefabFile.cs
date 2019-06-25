using KCV.Battle.Production;
using KCV.SortieBattle;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[Serializable]
	public class BattlePefabFile : BasePrefabFile
	{
		[SerializeField]
		[Header("[Common]")]
		private Transform _prefabBattleField;

		[SerializeField]
		private Transform _prefabBattleShip;

		[SerializeField]
		private Transform _prefabFieldDimCamera;

		[SerializeField]
		private Transform _prefabProdCloud;

		[SerializeField]
		private Transform _prefabUICircleHPGauge;

		[SerializeField]
		private Transform _prefabUIBattleNavigation;

		[SerializeField]
		[Header("[BossInsert]")]
		private Transform _prefabProdBossInsert;

		[SerializeField]
		[Header("[Detection]")]
		private Transform _prefabProdDetectionStartCutIn;

		[SerializeField]
		private Transform _prefabProdDetectionCutIn;

		[SerializeField]
		private Transform _prefabProdDetectionResultCutIn;

		[Header("[Command]")]
		[SerializeField]
		private Transform _prefabProdBattleCommandSelect;

		[SerializeField]
		private Transform _prefabProdBattleCommandBuffer;

		[SerializeField]
		private Transform _prefabProdBufferEffect;

		[SerializeField]
		private Transform _prefabUIBufferFleetCircle;

		[SerializeField]
		private Transform _prefabUIBufferShipCircle;

		[Header("[AerialCombat]")]
		[SerializeField]
		private Transform _prefabProdAerialCombatCutinP;

		[SerializeField]
		private Transform _prefabProdAerialCombatP1;

		[Header("[SupportingFire]")]
		[SerializeField]
		private Transform _prefabProdSupportCutIn;

		[Header("[OpeningTorpedoSalvo]")]
		[SerializeField]
		private Transform _prefabProdOpeningTorpedoCutIn;

		[SerializeField]
		[Header("[Shelling]")]
		private Transform _prefabProdShellingFormationJudge;

		[SerializeField]
		private Transform _prefabProdShellingSlotLine;

		[SerializeField]
		[Header("[TorpedoSalvo]")]
		private Transform _prefabProdTorpedoCutIn;

		[SerializeField]
		private Transform _prefabTorpedoStraightController;

		[SerializeField]
		private Transform _prefabProdTorpedoResucueCutIn;

		[SerializeField]
		[Header("[WithdrawalDecision]")]
		private Transform _prefabProdWithdrawalDecisionSelection;

		[SerializeField]
		[Header("[NightCombat]")]
		private Transform _prefabProdNightRadarDeployment;

		[SerializeField]
		private Transform _prefabSearchLightSceneController;

		[SerializeField]
		private Transform _prefabFlareBulletSceneController;

		[SerializeField]
		private Transform _prefabProdDeathCry;

		[SerializeField]
		[Header("[Result]")]
		private Transform _prefabProdVeteransReport;

		[Header("[AdvancingWithDrawal]")]
		[SerializeField]
		private Transform _prefabProdAdvancingWithDrawalSelect;

		[Header("[AdvancingWithDrawalDC]")]
		[SerializeField]
		private Transform _prefabProdAdvancingWithDrawalDC;

		[Header("[FlagshipWreck]")]
		[SerializeField]
		private Transform _prefabProdFlagshipWreck;

		[SerializeField]
		[Header("Others")]
		private Transform _prefabProdCombatRation;

		[Header("[Damage]")]
		[SerializeField]
		private Transform _prefabDamageCutIn;

		[SerializeField]
		private Transform _prefabProdSinking;

		private UIBattleNavigation _uiBattleNavigation;

		private ProdCloud _prodCloud;

		private UICircleHPGauge _uiCircleHPGauge;

		private ProdShellingSlotLine _prodShellingSlotLine;

		private ProdBattleCommandBuffer _prodBattleCommandBuffer;

		private ProdDamageCutIn _prodDamageCutIn;

		private ProdSinking _prodSinking;

		private BattleShutter _clsBattleShutter;

		private ProdBufferEffect _prodBufferEffect;

		public Transform prefabBattleField => BasePrefabFile.PassesPrefab(ref _prefabBattleField);

		public Transform prefabBattleShip => BasePrefabFile.PassesPrefab(ref _prefabBattleShip);

		public Transform prefabFieldDimCamera => BasePrefabFile.PassesPrefab(ref _prefabFieldDimCamera);

		public Transform prefabProdCloud => BasePrefabFile.PassesPrefab(ref _prefabProdCloud);

		public Transform prefabProdBossInsert => BasePrefabFile.PassesPrefab(ref _prefabProdBossInsert);

		public Transform prefabProdDetectionStartCutIn => BasePrefabFile.PassesPrefab(ref _prefabProdDetectionStartCutIn);

		public Transform prefabProdDetectionCutIn => BasePrefabFile.PassesPrefab(ref _prefabProdDetectionCutIn);

		public Transform prefabProdDetectionResultCutIn => BasePrefabFile.PassesPrefab(ref _prefabProdDetectionResultCutIn);

		public Transform prefabProdBattleCommandSelect => BasePrefabFile.PassesPrefab(ref _prefabProdBattleCommandSelect);

		public Transform prefabProdBattleCommandBuffer => _prefabProdBattleCommandBuffer;

		public Transform prefabUIBufferFleetCircle => BasePrefabFile.PassesPrefab(ref _prefabUIBufferFleetCircle);

		public Transform prefabUIBufferShipCircle => BasePrefabFile.PassesPrefab(ref _prefabUIBufferShipCircle);

		public Transform prefabProdAerialCombatCutinP => _prefabProdAerialCombatCutinP;

		public Transform prefabProdAerialCombatP1 => _prefabProdAerialCombatP1;

		public Transform prefabProdSupportCutIn => BasePrefabFile.PassesPrefab(ref _prefabProdSupportCutIn);

		public Transform prefabProdOpeningTorpedoCutIn => BasePrefabFile.PassesPrefab(ref _prefabProdOpeningTorpedoCutIn);

		public Transform prefabProdShellingFormationJudge => BasePrefabFile.PassesPrefab(ref _prefabProdShellingFormationJudge);

		public Transform prefabProdShellingSlotLine => BasePrefabFile.PassesPrefab(ref _prefabProdShellingSlotLine);

		public Transform prefabProdTorpedoCutIn => _prefabProdTorpedoCutIn;

		public Transform prefabTorpedoStraightController => _prefabTorpedoStraightController;

		public Transform prefabProdTorpedoResucueCutIn => _prefabProdTorpedoResucueCutIn;

		public Transform prefabProdWithdrawalDecisionSelection => BasePrefabFile.PassesPrefab(ref _prefabProdWithdrawalDecisionSelection);

		public Transform prefabProdNightRadarDeployment => BasePrefabFile.PassesPrefab(ref _prefabProdNightRadarDeployment);

		public Transform prefabSearchLightSceneController => BasePrefabFile.PassesPrefab(ref _prefabSearchLightSceneController);

		public Transform prefabFlareBulletSceneController => BasePrefabFile.PassesPrefab(ref _prefabFlareBulletSceneController);

		public Transform prefabProdDeathCry => BasePrefabFile.PassesPrefab(ref _prefabProdDeathCry);

		public Transform prefabProdVeteransReport => BasePrefabFile.PassesPrefab(ref _prefabProdVeteransReport);

		public Transform prefabProdAdvancingWithDrawalSelect => BasePrefabFile.PassesPrefab(ref _prefabProdAdvancingWithDrawalSelect);

		public Transform prefabProdAdvancingWithDrawalDC => BasePrefabFile.PassesPrefab(ref _prefabProdAdvancingWithDrawalDC);

		public Transform prefabProdFlagshipWreck => BasePrefabFile.PassesPrefab(ref _prefabProdFlagshipWreck);

		public Transform prefabProdCombatRation => BasePrefabFile.PassesPrefab(ref _prefabProdCombatRation);

		public UIBattleNavigation battleNavigation
		{
			get
			{
				if (_uiBattleNavigation == null)
				{
					_uiBattleNavigation = UIBattleNavigation.Instantiate(((Component)BasePrefabFile.PassesPrefab(ref _prefabUIBattleNavigation)).GetComponent<UIBattleNavigation>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform, BattleTaskManager.GetSettingModel());
				}
				return _uiBattleNavigation;
			}
		}

		public ProdCloud prodCloud
		{
			get
			{
				if (_prodCloud == null)
				{
					_prodCloud = ProdCloud.Instantiate(((Component)BasePrefabFile.PassesPrefab(ref _prefabProdCloud)).GetComponent<ProdCloud>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform);
				}
				return _prodCloud;
			}
		}

		public UICircleHPGauge circleHPGauge
		{
			get
			{
				if (_uiCircleHPGauge == null)
				{
					BasePrefabFile.InstantiatePrefab(ref _uiCircleHPGauge, ref _prefabUICircleHPGauge, BattleTaskManager.GetBattleCameras().cutInCamera.transform);
				}
				return _uiCircleHPGauge;
			}
		}

		public ProdShellingSlotLine prodShellingSlotLine
		{
			get
			{
				if (_prodShellingSlotLine == null)
				{
					BasePrefabFile.InstantiatePrefab(ref _prodShellingSlotLine, ref _prefabProdShellingSlotLine, BattleTaskManager.GetBattleCameras().cutInCamera.transform);
				}
				return _prodShellingSlotLine;
			}
			set
			{
				_prodShellingSlotLine = value;
			}
		}

		public BattleShutter battleShutter
		{
			get
			{
				if (_clsBattleShutter == null)
				{
					_clsBattleShutter = BattleShutter.Instantiate((SortieBattleTaskManager.GetSortieBattlePrefabFile() == null) ? Resources.Load<BattleShutter>("Prefabs/Battle/UI/BattleShutter") : ((Component)SortieBattleTaskManager.GetSortieBattlePrefabFile().prefabUIBattleShutter).GetComponent<BattleShutter>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform, 20);
				}
				return _clsBattleShutter;
			}
		}

		public ProdDamageCutIn prodDamageCutIn
		{
			get
			{
				if (_prodDamageCutIn == null)
				{
					BasePrefabFile.InstantiatePrefab(ref _prodDamageCutIn, ref _prefabDamageCutIn, BattleTaskManager.GetBattleCameras().cutInEffectCamera.transform);
				}
				return _prodDamageCutIn;
			}
		}

		public ProdBattleCommandBuffer prodBattleCommandBuffer
		{
			get
			{
				return _prodBattleCommandBuffer;
			}
			set
			{
				_prodBattleCommandBuffer = value;
			}
		}

		public ProdSinking prodSinking
		{
			get
			{
				if (_prodSinking == null)
				{
					BasePrefabFile.InstantiatePrefab(ref _prodSinking, ref _prefabProdSinking, BattleTaskManager.GetBattleCameras().cutInCamera.transform);
				}
				return _prodSinking;
			}
		}

		public ProdBufferEffect prodBufferEffect
		{
			get
			{
				if (_prodBufferEffect == null)
				{
					_prodBufferEffect = ProdBufferEffect.Instantiate(((Component)_prefabProdBufferEffect).GetComponent<ProdBufferEffect>(), BattleTaskManager.GetBattleCameras().cutInCamera.transform);
					Mem.Del(ref _prefabProdBufferEffect);
				}
				return _prodBufferEffect;
			}
		}

		public void DisposeProdCloud()
		{
			if (_prodCloud != null)
			{
				Mem.DelComponentSafe(ref _prodCloud);
			}
		}

		public void DisposeProdCommandBuffer()
		{
			if (prodBattleCommandBuffer != null)
			{
				prodBattleCommandBuffer.BufferObjectConvergence();
				Mem.DelComponentSafe(ref _prodBattleCommandBuffer);
			}
		}

		public void DisposeUnneccessaryObject2Result()
		{
			Mem.Del(ref _prefabBattleField);
			Mem.Del(ref _prefabUICircleHPGauge);
			Mem.Del(ref _prefabProdBossInsert);
			Mem.Del(ref _prefabProdDetectionStartCutIn);
			Mem.Del(ref _prefabProdDetectionCutIn);
			Mem.Del(ref _prefabProdDetectionResultCutIn);
			Mem.Del(ref _prefabProdBattleCommandSelect);
			Mem.Del(ref _prefabProdBattleCommandBuffer);
			Mem.Del(ref _prefabProdBufferEffect);
			Mem.Del(ref _prefabUIBufferFleetCircle);
			Mem.Del(ref _prefabUIBufferShipCircle);
			Mem.Del(ref _prefabProdAerialCombatCutinP);
			Mem.Del(ref _prefabProdAerialCombatP1);
			Mem.Del(ref _prefabProdSupportCutIn);
			Mem.Del(ref _prefabProdOpeningTorpedoCutIn);
			Mem.Del(ref _prefabProdShellingFormationJudge);
			Mem.Del(ref _prefabProdShellingSlotLine);
			Mem.Del(ref _prefabProdTorpedoCutIn);
			Mem.Del(ref _prefabTorpedoStraightController);
			Mem.Del(ref _prefabProdTorpedoResucueCutIn);
			Mem.Del(ref _prefabProdWithdrawalDecisionSelection);
			Mem.Del(ref _prefabProdNightRadarDeployment);
			Mem.Del(ref _prefabSearchLightSceneController);
			Mem.Del(ref _prefabFlareBulletSceneController);
			Mem.Del(ref _prefabProdDeathCry);
			Mem.Del(ref _prodCloud);
			Mem.DelComponentSafe(ref _uiCircleHPGauge);
			Mem.DelComponentSafe(ref _prodShellingSlotLine);
			Mem.DelComponentSafe(ref _prodDamageCutIn);
			Mem.DelComponentSafe(ref _prodBattleCommandBuffer);
			Mem.DelComponentSafe(ref _prodSinking);
			Mem.DelComponentSafe(ref _prodBufferEffect);
			Mem.Del(ref _prefabDamageCutIn);
			Mem.Del(ref _prefabProdSinking);
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del(ref _prefabBattleShip);
			Mem.Del(ref _prefabFieldDimCamera);
			Mem.Del(ref _prefabProdCloud);
			Mem.Del(ref _prefabProdVeteransReport);
			Mem.Del(ref _prefabProdAdvancingWithDrawalSelect);
			Mem.Del(ref _prefabProdAdvancingWithDrawalDC);
			Mem.Del(ref _prefabProdFlagshipWreck);
			Mem.Del(ref _prefabProdCombatRation);
			Mem.Del(ref _prodShellingSlotLine);
			Mem.Del(ref _prodBattleCommandBuffer);
			Mem.Del(ref _prodDamageCutIn);
			Mem.Del(ref _prodSinking);
			Mem.Del(ref _clsBattleShutter);
			Mem.Del(ref _prodBufferEffect);
			Mem.Del(ref _uiBattleNavigation);
			base.Dispose(disposing);
		}
	}
}
