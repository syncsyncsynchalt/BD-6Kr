using KCV.Battle.Utils;
using KCV.Utils;
using local.models.battle;
using local.utils;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdVeteransReport : MonoBehaviour
	{
		[Serializable]
		private class VeteransReportCommon : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UILabel _uiAreaName;

			[SerializeField]
			private UITexture _uiBackground;

			[SerializeField]
			private UITexture _uiCenterLine;

			[SerializeField]
			private UISprite _uiResultLabel;

			[SerializeField]
			private UITexture _uiOverlay;

			public Transform transform => _tra;

			public VeteransReportCommon(Transform obj)
			{
			}

			public bool Init(BattleResultModel model)
			{
				_uiAreaName.text = model.MapName;
				_uiOverlay.alpha = 0f;
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _tra);
				Mem.Del(ref _uiAreaName);
				Mem.Del(ref _uiBackground);
				Mem.Del(ref _uiCenterLine);
				Mem.Del(ref _uiResultLabel);
				Mem.Del(ref _uiOverlay);
			}

			public LTDescr ShowOverlay()
			{
				return _uiOverlay.transform.LTValue(_uiOverlay.alpha, 0.5f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					_uiOverlay.alpha = x;
				});
			}

			public LTDescr HideOverlay()
			{
				return _uiOverlay.transform.LTValue(_uiOverlay.alpha, 0f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					_uiOverlay.alpha = x;
				});
			}
		}

		[SerializeField]
		private VeteransReportCommon _clsCommon;

		[SerializeField]
		private UIGearButton _uiGearBtn;

		[SerializeField]
		private Transform _prefabVeteransReportFleet;

		[SerializeField]
		private Transform _prefabVeteransReportBonus;

		[SerializeField]
		private Transform _prefabVeteransReportMVPShip;

		[SerializeField]
		private Transform _prefabProdWinRankJudge;

		[SerializeField]
		private List<Vector2> _listVeteransReportFleetsPos;

		[SerializeField]
		private Vector3 _vBounusPos = new Vector3(240f, 0f, 0f);

		[SerializeField]
		private Vector3 _vMVPShipAnchorPos = new Vector3(270f, -160f, 0f);

		private bool _isInputEnabled;

		private bool _isWinRunkFinished;

		private bool _isEXPUpdateFinished;

		private bool _isProdFinished;

		private BattleResultModel _clsResultModel;

		private List<UIVeteransReportFleet> _listVeteransReportFleets;

		private UIVeteransReportBonus _uiBonus;

		private UIVeteransReportMVPShip _uiMVPShip;

		private ProdWinRankJudge _prodWinRankJudge;

		private StatementMachine _clsState;

		public static ProdVeteransReport Instantiate(ProdVeteransReport prefab, Transform parent, BattleResultModel model)
		{
			ProdVeteransReport prodVeteransReport = UnityEngine.Object.Instantiate(prefab);
			prodVeteransReport.transform.parent = parent;
			prodVeteransReport.transform.localPositionZero();
			prodVeteransReport.transform.localScaleOne();
			prodVeteransReport._clsResultModel = model;
			return prodVeteransReport.VirtualCtor();
		}

		private void OnDestroy()
		{
			Mem.DelIDisposableSafe(ref _clsCommon);
			Mem.Del(ref _uiGearBtn);
			Mem.Del(ref _prefabVeteransReportFleet);
			Mem.Del(ref _prefabVeteransReportBonus);
			Mem.Del(ref _prefabVeteransReportMVPShip);
			Mem.Del(ref _prefabProdWinRankJudge);
			Mem.DelListSafe(ref _listVeteransReportFleetsPos);
			Mem.Del(ref _vBounusPos);
			Mem.Del(ref _vMVPShipAnchorPos);
			Mem.Del(ref _isInputEnabled);
			Mem.Del(ref _isWinRunkFinished);
			Mem.Del(ref _isEXPUpdateFinished);
			Mem.Del(ref _isProdFinished);
			Mem.Del(ref _clsResultModel);
			Mem.DelListSafe(ref _listVeteransReportFleets);
			Mem.Del(ref _uiBonus);
			Mem.Del(ref _uiMVPShip);
			Mem.Del(ref _prodWinRankJudge);
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			Mem.Del(ref _clsState);
		}

		private ProdVeteransReport VirtualCtor()
		{
			_clsState = new StatementMachine();
			_isInputEnabled = false;
			_isWinRunkFinished = false;
			_isEXPUpdateFinished = false;
			_isProdFinished = false;
			_uiGearBtn.isColliderEnabled = false;
			_uiGearBtn.widget.alpha = 0f;
			_clsCommon.Init(_clsResultModel);
			return this;
		}

		public IEnumerator CreateInstance(bool isPractice)
		{
			_listVeteransReportFleets = new List<UIVeteransReportFleet>();
			for (int i = 0; (float)i < 2f; i++)
			{
				_listVeteransReportFleets.Add(UIVeteransReportFleet.Instantiate(((Component)_prefabVeteransReportFleet).GetComponent<UIVeteransReportFleet>(), base.transform, _listVeteransReportFleetsPos[i], _clsResultModel, (FleetType)i));
				_listVeteransReportFleets[i].CreateInstance();
			}
			yield return null;
			_prodWinRankJudge = ProdWinRankJudge.Instantiate(((Component)_prefabProdWinRankJudge).GetComponent<ProdWinRankJudge>(), base.transform, _clsResultModel, isBattleCut: false);
			yield return null;
			_uiBonus = UIVeteransReportBonus.Instantiate(((Component)_prefabVeteransReportBonus).GetComponent<UIVeteransReportBonus>(), base.transform, _vBounusPos, _clsResultModel, isPractice);
			_uiBonus.panel.alpha = 0f;
			yield return null;
			_uiMVPShip = UIVeteransReportMVPShip.Instantiate(((Component)_prefabVeteransReportMVPShip).GetComponent<UIVeteransReportMVPShip>(), base.transform, _vMVPShipAnchorPos, _clsResultModel.MvpShip);
			_uiMVPShip.panel.alpha = 0f;
			yield return null;
		}

		private void SortPanelDepth()
		{
		}

		public bool Run()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			return _isProdFinished;
		}

		public void PlayVeteransReport()
		{
			PlayWarVateransSlotIn();
		}

		private void PlayWarVateransSlotIn()
		{
			FleetType callbackFleetType = FleetType.Friend;
			if (_listVeteransReportFleets[1].veteransReportShipBanner.Count >= _listVeteransReportFleets[0].veteransReportShipBanner.Count)
			{
				callbackFleetType = FleetType.Enemy;
			}
			_listVeteransReportFleets.ForEach(delegate(UIVeteransReportFleet x)
			{
				Action act = (x.fleetType != callbackFleetType) ? null : new Action(PlayWarVateransShipInfosIn);
				Observable.FromCoroutine(() => x.PlayBannersSlotIn(act)).Subscribe().AddTo(base.gameObject);
			});
		}

		private void PlayWarVateransShipInfosIn()
		{
			_listVeteransReportFleets.ForEach(delegate(UIVeteransReportFleet x)
			{
				ProdVeteransReport prodVeteransReport = this;
				Action act = (x.fleetType != 0) ? null : new Action(PlayWarVeteransShipHPUpdate);
				Observable.FromCoroutine(() => x.PlayShipInfosIn(act)).Subscribe().AddTo(base.gameObject);
			});
		}

		private void PlayWarVeteransShipHPUpdate()
		{
			Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate
			{
				_listVeteransReportFleets.ForEach(delegate(UIVeteransReportFleet x)
				{
					x.PlayBannerHPUpdate(null);
				});
			});
			Observable.Timer(TimeSpan.FromSeconds(0.34999999403953552)).Subscribe(delegate
			{
				PlayWarVateransGauge();
			});
		}

		private void PlayWarVateransGauge()
		{
			_listVeteransReportFleets.ForEach(delegate(UIVeteransReportFleet x)
			{
				if (x.fleetType == FleetType.Friend)
				{
					x.PlayWarVateransGauge(delegate
					{
						_clsState.AddState(initWinRunkJudge, updateWinRunkJudge);
					});
				}
				else
				{
					x.PlayWarVateransGauge(null);
				}
			});
		}

		private bool initWinRunkJudge(object data)
		{
			_listVeteransReportFleets[1].transform.LTMoveLocal(Vector3.right * 1000f, 0.3f).setEase(LeanTweenType.easeInSine).setOnComplete((Action)delegate
			{
				_listVeteransReportFleets[1].panel.widgetsAreStatic = true;
				BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
				cutInEffectCamera.blur.enabled = false;
				_clsCommon.ShowOverlay();
			});
			Observable.FromCoroutine(_prodWinRankJudge.StartBattleJudge).Subscribe(delegate
			{
				OnDecideWinRunkGearBtn();
				Mem.DelComponentSafe(ref _prodWinRankJudge);
			});
			return false;
		}

		private bool updateWinRunkJudge(object data)
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			if (_isInputEnabled && keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				_uiGearBtn.Decide();
				return true;
			}
			return _isWinRunkFinished;
		}

		private void OnDecideWinRunkGearBtn()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.blur.enabled = false;
			_clsCommon.HideOverlay();
			_isWinRunkFinished = true;
			_isInputEnabled = false;
			_clsState.AddState(initEXPReflection, updateEXPReflection);
			_listVeteransReportFleets[0].ChangeBannerMode(UIVeteransReportShipBanner.BannerMode.EXP);
			if (_uiMVPShip.shipModel != null)
			{
				_listVeteransReportFleets[0].veteransReportShipBanner.Find((UIVeteransReportShipBanner x) => x.shipModel == _uiMVPShip.shipModel).PlayMVP();
			}
		}

		private bool initEXPReflection(object data)
		{
			TrophyUtil.Unlock_UserLevel();
			_uiMVPShip.Show(BattleUtils.IsPlayMVPVoice(_clsResultModel.WinRank), null);
			_uiBonus.Show(null);
			Observable.Timer(TimeSpan.FromSeconds(0.25)).Subscribe(delegate
			{
				_listVeteransReportFleets[0].PlayEXPUpdate(delegate
				{
					UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
					battleNavigation.SetNavigationInResult();
					battleNavigation.Show(0.2f, null);
					_uiGearBtn.Show(delegate
					{
						_isInputEnabled = true;
					});
				});
				_uiGearBtn.SetDecideAction(OnDecideEXPUodateGearBtn);
			});
			return false;
		}

		private bool updateEXPReflection(object data)
		{
			KeyControl keyControl = BattleTaskManager.GetKeyControl();
			if (_isInputEnabled && keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				_uiGearBtn.Decide();
				return true;
			}
			return _isEXPUpdateFinished;
		}

		private void OnDecideEXPUodateGearBtn()
		{
			UIBattleNavigation battleNavigation = BattleTaskManager.GetPrefabFile().battleNavigation;
			battleNavigation.Hide(0.2f, null);
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			_isEXPUpdateFinished = true;
			_isProdFinished = true;
			_uiGearBtn.Hide(null);
		}
	}
}
