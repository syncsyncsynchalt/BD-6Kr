using Common.Enum;
using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.BattleCut
{
	public class BtlCut_ResultShip : MonoBehaviour
	{
		private const string HP_DRAW_FORMAT = "{0} / {1}";

		[SerializeField]
		private UITexture _uiBannerTexBefore;

		[SerializeField]
		private UITexture _uiBannerTexAfter;

		[SerializeField]
		private UILabel Level;

		[SerializeField]
		private UISlider _uiHPSlider;

		[SerializeField]
		private UISlider _uiEXPSlider;

		[SerializeField]
		private UILabel HPLabel;

		[SerializeField]
		private UISprite DamageSmoke;

		[SerializeField]
		private UISprite DamageIcon;

		private int levelUpCount;

		private ShipModel_BattleResult _clsShipModel;

		private int beforeHP;

		public Action act;

		public static BtlCut_ResultShip Instantiate(BtlCut_ResultShip prefab, Transform parent, Vector3 pos, ShipModel_BattleResult model)
		{
			BtlCut_ResultShip btlCut_ResultShip = UnityEngine.Object.Instantiate(prefab);
			btlCut_ResultShip.transform.parent = parent;
			btlCut_ResultShip.transform.localPosition = pos;
			btlCut_ResultShip.transform.localScaleOne();
			return btlCut_ResultShip.VitualCtor(model);
		}

		private BtlCut_ResultShip VitualCtor(ShipModel_BattleResult model)
		{
			_clsShipModel = model;
			int texNum = (!model.DamagedFlgStart) ? 1 : 2;
			_uiBannerTexBefore.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(_clsShipModel.MstId, texNum);
			if (model.DmgStateStart == DamageState_Battle.Gekichin || model.IsEscape())
			{
				_uiBannerTexBefore.shader = SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList[0];
			}
			if (model.DmgStateStart != 0)
			{
				DamageSmoke.spriteName = $"icon-ss_burned_{model.DmgStateStart.ToString()}";
				DamageSmoke.alpha = 1f;
			}
			DamageIcon.spriteName = ((!model.IsEscape()) ? ("icon-ss_" + model.DmgStateStart.ToString()) : "icon-ss_taihi");
			DamageIcon.alpha = (model.IsEscape() ? 1f : ((model.DmgStateStart == DamageState_Battle.Normal) ? 0f : 1f));
			Level.textInt = model.Level;
			HPLabel.text = $"{model.HpStart} / {model.MaxHp}";
			_uiHPSlider.value = Mathe.Rate(0f, model.MaxHp, model.HpStart);
			_uiHPSlider.foregroundWidget.color = Util.HpGaugeColor2(model.MaxHp, model.HpStart);
			beforeHP = model.HpStart;
			levelUpCount = 0;
			_uiEXPSlider.value = Mathe.Rate(0f, 100f, model.ExpInfo.ExpRateBefore);
			return this;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiBannerTexBefore);
			Mem.Del(ref _uiBannerTexAfter);
			Mem.Del(ref Level);
			Mem.Del(ref _uiHPSlider);
			Mem.Del(ref _uiEXPSlider);
			Mem.Del(ref HPLabel);
			Mem.Del(ref DamageSmoke);
			Mem.Del(ref DamageIcon);
			Mem.Del(ref levelUpCount);
			Mem.Del(ref _clsShipModel);
			Mem.Del(ref beforeHP);
			Mem.Del(ref act);
		}

		public void UpdateHP()
		{
			HPLabel.transform.LTValue(_clsShipModel.HpStart, _clsShipModel.HpEnd, 1f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				int num = Convert.ToInt32(x);
				_uiHPSlider.value = Mathe.Rate(0f, _clsShipModel.MaxHp, x);
				_uiHPSlider.foregroundWidget.color = Util.HpGaugeColor2(_clsShipModel.MaxHp, num);
				HPLabel.text = $"{num} / {_clsShipModel.MaxHp}";
			})
				.setOnComplete((Action)delegate
				{
					ShipTexUpdate();
				});
		}

		private void ShipTexUpdate()
		{
			if (act != null)
			{
				act();
			}
			int texNum = (!_clsShipModel.DamagedFlgEnd) ? 1 : 2;
			_uiBannerTexAfter.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(_clsShipModel.MstId, texNum);
			if (_clsShipModel.DmgStateEnd == DamageState_Battle.Gekichin || _clsShipModel.IsEscape())
			{
				_uiBannerTexAfter.shader = SingletonMonoBehaviour<ResourceManager>.Instance.shader.shaderList[0];
			}
			_uiBannerTexAfter.transform.ValueTo(0f, 1f, 0.8f, iTween.EaseType.linear, delegate(object x)
			{
				_uiBannerTexAfter.alpha = Convert.ToSingle(x);
			}, null);
			_uiBannerTexBefore.transform.ValueTo(1f, 0f, 0.8f, iTween.EaseType.linear, delegate(object x)
			{
				_uiBannerTexBefore.alpha = Convert.ToSingle(x);
			}, null);
			DamageSmoke.spriteName = "icon-ss_burned_" + _clsShipModel.DmgStateEnd.ToString();
			DamageIcon.spriteName = ((!_clsShipModel.IsEscape()) ? ("icon-ss_" + _clsShipModel.DmgStateEnd.ToString()) : "icon-ss_taihi");
			TweenAlpha.Begin(DamageSmoke.gameObject, 0.8f, 1f);
			TweenAlpha.Begin(DamageIcon.gameObject, 0.8f, 1f);
		}

		public void UpdateEXPGauge()
		{
			float num = (float)_clsShipModel.ExpInfo.ExpRateAfter[levelUpCount] / 100f;
			bool isOver = false;
			if (_clsShipModel.ExpInfo.ExpRateAfter.Count > levelUpCount + 1)
			{
				num = 1f;
				isOver = true;
				levelUpCount++;
			}
			_uiEXPSlider.transform.ValueTo(_uiEXPSlider.value, num, 0.5f, iTween.EaseType.easeOutQuad, delegate(object x)
			{
				_uiEXPSlider.value = Convert.ToSingle(x);
			}, delegate
			{
				if (isOver)
				{
					Level.textInt = _clsShipModel.Level + levelUpCount;
					_uiEXPSlider.value = 0f;
					UpdateEXPGauge();
				}
			});
		}

		public void ShowMVPIcon()
		{
			Util.InstantiatePrefab("SortieMap/BattleCut/MVPIcon", base.gameObject, doTween: true).transform.localPosition(-100f, 4f, 0f);
		}
	}
}
