using KCV.Battle.Utils;
using KCV.Generic;
using KCV.SortieBattle;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class UIVeteransReportShipBanner : BaseUISortieBattleShipBanner<ShipModel_BattleResult>
	{
		public enum BannerMode
		{
			HP,
			EXP
		}

		[SerializeField]
		private UILabel _uiLv;

		[SerializeField]
		private UILabel _uiLvLabel;

		[SerializeField]
		private UISlider _uiEXPSlider;

		[SerializeField]
		private UIWidget _uiShipInfos;

		[SerializeField]
		private UILevelUpIcon _uiLevelUpIcon;

		[SerializeField]
		private UIMVPIcon _uiMVPIcon;

		[SerializeField]
		private List<Vector3> _listBannerMaskPos;

		[SerializeField]
		private List<Vector3> _listShipInfosPos;

		[SerializeField]
		private float _fStartVerticalOffs = 10f;

		[SerializeField]
		private float _fStartHorizontalOffs = 10f;

		private int _nDrawNowLv;

		private bool isFriend => base.shipModel.IsFriend();

		private Vector3 bannerMaskPos => _listBannerMaskPos[(!isFriend) ? 1 : 0];

		private Vector3 shipInfosPos => _listShipInfosPos[(!isFriend) ? 1 : 0];

		private ShipExpModel expModel => base.shipModel.ExpInfo;

		public static UIVeteransReportShipBanner Instantiate(UIVeteransReportShipBanner prefab, Transform parent, Vector3 pos, ShipModel_BattleResult model)
		{
			UIVeteransReportShipBanner uIVeteransReportShipBanner = UnityEngine.Object.Instantiate(prefab);
			uIVeteransReportShipBanner.transform.parent = parent;
			uIVeteransReportShipBanner.transform.localScaleOne();
			uIVeteransReportShipBanner.transform.localPosition = pos;
			uIVeteransReportShipBanner.VirtualCtor(model);
			return uIVeteransReportShipBanner;
		}

		protected override void VirtualCtor(ShipModel_BattleResult model)
		{
			base.VirtualCtor(model);
			base.name = $"VeteransReportShipBanner{model.Index}";
			SetShipInfos(model);
			SetUISettings(model.IsFriend());
		}

		protected override void OnUnInit()
		{
			Mem.Del(ref _uiLv);
			Mem.Del(ref _uiLvLabel);
			Mem.Del(ref _uiEXPSlider);
			Mem.Del(ref _uiShipInfos);
			Mem.Del(ref _uiLevelUpIcon);
			Mem.Del(ref _uiLevelUpIcon);
			Mem.Del(ref _uiMVPIcon);
			Mem.DelListSafe(ref _listBannerMaskPos);
			Mem.DelListSafe(ref _listShipInfosPos);
			Mem.Del(ref _fStartVerticalOffs);
			Mem.Del(ref _fStartHorizontalOffs);
			Mem.Del(ref _nDrawNowLv);
		}

		protected override void SetShipInfos(ShipModel_BattleResult model)
		{
			SetShipName(model.Name);
			SetShipBannerTexture(ShipUtils.LoadBannerTextureInVeteransReport(model), model.DamagedFlgEnd, model.DmgStateEnd, model.IsEscape());
			SetShipIcons(model.DmgStateEnd, model.IsGroundFacility(), model.IsEscape());
			SetHPSliderValue(model.HpStart, model.MaxHp);
			_uiLv.textInt = (_nDrawNowLv = model.Level);
			if (!isFriend)
			{
				_uiLv.SetActive(isActive: false);
				_uiLvLabel.SetActive(isActive: false);
				_uiShipName.SetActive(isActive: true);
			}
			else
			{
				_uiEXPSlider.value = Mathe.Rate(0f, 100f, model.ExpInfo.ExpRateBefore);
				_uiEXPSlider.foregroundWidget.color = KCVColor.WarVateransEXPGaugeGreen;
				_uiShipName.SetActive(isActive: false);
			}
			_uiEXPSlider.alpha = 0f;
			_uiHPSlider.alpha = 1f;
		}

		protected override void SetUISettings(bool isFriend)
		{
			UITexture uiShipTexture = _uiShipTexture;
			float alpha = 0f;
			_uiShipInfos.alpha = alpha;
			uiShipTexture.alpha = alpha;
			_uiMVPIcon.transform.localScale = Vector3.one * 1.25f;
			_uiMVPIcon.SetActive(isActive: false);
			_uiShipInfos.transform.localPosition = shipInfosPos;
			_uiShipTexture.transform.localPosition = bannerMaskPos;
			_uiShipTexture.transform.localPosition = _uiShipTexture.transform.localPosition + Vector3.down * _fStartVerticalOffs;
			_uiShipInfos.transform.localPosition = _uiShipInfos.transform.localPosition + ((!isFriend) ? (Vector3.left * _fStartHorizontalOffs) : (Vector3.right * _fStartHorizontalOffs));
		}

		public void ChangeMode(BannerMode iMode)
		{
			if (iMode == BannerMode.EXP)
			{
				_uiHPSlider.transform.LTCancel();
				_uiHPSlider.transform.LTValue(_uiHPSlider.alpha, 0f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					_uiHPSlider.alpha = x;
				});
				_uiEXPSlider.transform.LTCancel();
				_uiEXPSlider.transform.LTValue(_uiEXPSlider.alpha, 1f, 0.25f).setDelay(0.25f).setEase(LeanTweenType.linear)
					.setOnUpdate(delegate(float x)
					{
						_uiEXPSlider.alpha = x;
					});
			}
			else
			{
				_uiEXPSlider.transform.LTCancel();
				_uiEXPSlider.transform.LTValue(_uiEXPSlider.alpha, 0f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					_uiEXPSlider.alpha = x;
				});
				_uiHPSlider.transform.LTCancel();
				_uiHPSlider.transform.LTValue(_uiHPSlider.alpha, 1f, 0.25f).setDelay(0.25f).setEase(LeanTweenType.linear)
					.setOnUpdate(delegate(float x)
					{
						_uiHPSlider.alpha = x;
					});
			}
		}

		public void PlayBannerSlotIn(Action callback)
		{
			_uiShipTexture.transform.LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
			{
				_uiShipTexture.alpha = x;
			})
				.setOnComplete(callback);
			_uiShipTexture.transform.LTMoveLocal(bannerMaskPos, 0.5f).setEase(LeanTweenType.easeOutSine);
		}

		public void PlayShipInfosIn(Action callback)
		{
			_uiShipInfos.transform.LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate(float x)
			{
				_uiShipInfos.alpha = x;
			});
			_uiShipInfos.transform.LTMoveLocal(shipInfosPos, 0.5f).setEase(LeanTweenType.easeOutSine).setOnComplete(callback);
		}

		public void PlayHPUpdate(Action callback)
		{
			_uiHPSlider.transform.LTValue(base.shipModel.HpStart, base.shipModel.HpEnd, 0.85f).setDelay(0.5f).setEase(LeanTweenType.linear)
				.setOnUpdate(delegate(float x)
				{
					int num = Convert.ToInt32(x);
					_uiHPSlider.value = Mathe.Rate(0f, base.shipModel.MaxHp, num);
					_uiHPSlider.foregroundWidget.color = Util.HpGaugeColor2(base.shipModel.MaxHp, num);
				})
				.setOnComplete(callback);
		}

		public void PlayEXPUpdate(Action callback)
		{
			float intervalTime = 2f / (float)expModel.ExpRateAfter.Count;
			Queue<int> exp = new Queue<int>(expModel.ExpRateAfter);
			UpdateEXP(expModel.ExpRateBefore, exp, intervalTime, callback);
		}

		private void UpdateEXP(int nowEXPRate, Queue<int> exp, float intervalTime, Action callback)
		{
			int num = exp.Dequeue();
			_uiEXPSlider.transform.LTValue(nowEXPRate, num, intervalTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiEXPSlider.value = x / 100f;
				if (_uiEXPSlider.value == 1f)
				{
					PlayLevelUp();
				}
			})
				.setOnComplete((Action)delegate
				{
					if (exp.Count != 0)
					{
						UpdateEXP(0, exp, intervalTime, callback);
					}
					else
					{
						Dlg.Call(ref callback);
					}
				});
		}

		private void PlayLevelUp()
		{
			_nDrawNowLv++;
			_uiLv.textInt = _nDrawNowLv;
			_uiLevelUpIcon.Play();
		}

		public void PlayMVP()
		{
			_uiMVPIcon.SetActive(isActive: true);
			_uiMVPIcon.Play();
		}
	}
}
