using KCV.Battle.Utils;
using KCV.Generic;
using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(UIPanel))]
	public class UIVeteransReportFleet : MonoBehaviour
	{
		[SerializeField]
		private Transform _prefabVeteransReportShipBanner;

		[SerializeField]
		private Transform _traShipBannerAnchor;

		[SerializeField]
		private float _fBannerVerticalOffs = -50f;

		[SerializeField]
		private float _fWarVateransSliderOffs = 20f;

		[SerializeField]
		private UILabel _uiFleetName;

		[SerializeField]
		private UISlider _uiWarVateransSlider;

		private UIPanel _uiPanel;

		private List<UIVeteransReportShipBanner> _listShipBanners;

		private List<int> _listWarVateransVal;

		private BattleResultModel _clsResultModel;

		private FleetType _iType;

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

		public FleetType fleetType => _iType;

		public List<UIVeteransReportShipBanner> veteransReportShipBanner => _listShipBanners;

		private string fleetName => (_iType != 0) ? _clsResultModel.EnemyName : _clsResultModel.DeckName;

		private List<int> warVateransVal
		{
			get
			{
				if (_listWarVateransVal.Count == 0)
				{
					_listWarVateransVal = new List<int>();
					_listWarVateransVal.Add((_iType != 0) ? _clsResultModel.HPStart_e : _clsResultModel.HPStart_f);
					_listWarVateransVal.Add((_iType != 0) ? _clsResultModel.HPEnd_e : _clsResultModel.HPEnd_f);
				}
				return _listWarVateransVal;
			}
		}

		private UIVeteransReportShipBanner lastShipBanner => _listShipBanners.Last();

		public static UIVeteransReportFleet Instantiate(UIVeteransReportFleet prefab, Transform parent, Vector3 pos, BattleResultModel model, FleetType iType)
		{
			UIVeteransReportFleet uIVeteransReportFleet = UnityEngine.Object.Instantiate(prefab);
			uIVeteransReportFleet.transform.parent = parent;
			uIVeteransReportFleet.transform.localScaleOne();
			uIVeteransReportFleet.transform.localPosition = pos;
			uIVeteransReportFleet._iType = iType;
			uIVeteransReportFleet.name = $"{iType}Fleet";
			uIVeteransReportFleet._clsResultModel = model;
			uIVeteransReportFleet.Init();
			return uIVeteransReportFleet;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabVeteransReportShipBanner);
			Mem.Del(ref _traShipBannerAnchor);
			Mem.Del(ref _fBannerVerticalOffs);
			Mem.Del(ref _fWarVateransSliderOffs);
			Mem.Del(ref _uiFleetName);
			Mem.Del(ref _uiWarVateransSlider);
			Mem.Del(ref _uiPanel);
			Mem.DelListSafe(ref _listShipBanners);
			Mem.DelListSafe(ref _listWarVateransVal);
			Mem.Del(ref _clsResultModel);
			Mem.Del(ref _iType);
		}

		private bool Init()
		{
			_listShipBanners = new List<UIVeteransReportShipBanner>();
			_listWarVateransVal = new List<int>();
			_uiFleetName.text = fleetName;
			_uiFleetName.supportEncoding = false;
			_uiWarVateransSlider.value = Mathe.Rate(0f, warVateransVal[0], 0f);
			_uiWarVateransSlider.foregroundWidget.color = ((fleetType != 0) ? KCVColor.WarVateransGaugeRed : KCVColor.WarVateransGaugeGreen);
			_uiWarVateransSlider.alpha = 0f;
			_uiWarVateransSlider.transform.localPosition = _uiWarVateransSlider.transform.localPosition + ((fleetType != 0) ? Vector3.left : Vector3.right) * _fWarVateransSliderOffs;
			return true;
		}

		public void CreateInstance()
		{
			List<ShipModel_BattleResult> list = new List<ShipModel_BattleResult>((_iType != 0) ? _clsResultModel.Ships_e : _clsResultModel.Ships_f);
			int num = 0;
			foreach (ShipModel_BattleResult item in list)
			{
				if (item != null)
				{
					_listShipBanners.Add(UIVeteransReportShipBanner.Instantiate(((Component)_prefabVeteransReportShipBanner).GetComponent<UIVeteransReportShipBanner>(), _traShipBannerAnchor, Vector3.down * _fBannerVerticalOffs * num, item));
					num++;
				}
			}
		}

		public void ChangeBannerMode(UIVeteransReportShipBanner.BannerMode iMode)
		{
			_listShipBanners.ForEach(delegate(UIVeteransReportShipBanner x)
			{
				x.ChangeMode(iMode);
			});
		}

		public IEnumerator PlayBannersSlotIn(Action callback)
		{
			foreach (UIVeteransReportShipBanner banner in _listShipBanners)
			{
				if (banner == lastShipBanner)
				{
					banner.PlayBannerSlotIn(callback);
				}
				else
				{
					banner.PlayBannerSlotIn(null);
				}
				yield return new WaitForSeconds(0.05f);
			}
			yield return null;
		}

		public IEnumerator PlayShipInfosIn(Action callback)
		{
			foreach (UIVeteransReportShipBanner banner in _listShipBanners)
			{
				if (banner == lastShipBanner)
				{
					banner.PlayShipInfosIn(callback);
				}
				else
				{
					banner.PlayShipInfosIn(null);
				}
				yield return new WaitForSeconds(0.05f);
			}
			yield return null;
		}

		public void PlayBannerHPUpdate(Action callback)
		{
			_listShipBanners.ForEach(delegate(UIVeteransReportShipBanner x)
			{
				if (x == lastShipBanner)
				{
					x.PlayHPUpdate(callback);
				}
				else
				{
					x.PlayHPUpdate(null);
				}
			});
		}

		public void PlayWarVateransGauge(Action callback)
		{
			_uiWarVateransSlider.transform.LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiWarVateransSlider.alpha = x;
			});
			Vector3 to = _uiWarVateransSlider.transform.localPosition + ((fleetType != 0) ? Vector3.right : Vector3.left) * _fWarVateransSliderOffs;
			_uiWarVateransSlider.transform.LTMoveLocal(to, 0.5f).setEase(LeanTweenType.linear);
			_uiWarVateransSlider.transform.LTValue(0f, warVateransVal[1], 0.5f).setDelay(1f).setEase(LeanTweenType.linear)
				.setOnUpdate(delegate(float x)
				{
					_uiWarVateransSlider.value = Mathe.Rate(0f, warVateransVal[0], x);
				})
				.setOnComplete((Action)delegate
				{
					Observable.Timer(TimeSpan.FromSeconds(0.5)).Subscribe(delegate
					{
						Dlg.Call(ref callback);
					});
				});
		}

		public void PlayEXPUpdate(Action callback)
		{
			_listShipBanners.ForEach(delegate(UIVeteransReportShipBanner x)
			{
				x.PlayEXPUpdate((!(x == lastShipBanner)) ? null : callback);
			});
		}
	}
}
