using KCV.Battle.Utils;
using local.models;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(UIPanel))]
	[RequireComponent(typeof(Animation))]
	public class ProdDeathCry : BaseAnimation
	{
		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiForeground;

		[SerializeField]
		private Transform _traShipAnchor;

		[SerializeField]
		private UITexture _uiShipTexture;

		[SerializeField]
		private ParticleSystem _psStar;

		[SerializeField]
		private ParticleSystem _psSmoke;

		private float _fVoiceLength;

		private UIPanel _uiPanel;

		private ShipModel_BattleAll _clsShipModel;

		public UIPanel panel => _uiPanel;

		public static ProdDeathCry Instantiate(ProdDeathCry prefab, Transform parent, ShipModel_BattleAll model)
		{
			ProdDeathCry prodDeathCry = UnityEngine.Object.Instantiate(prefab);
			prodDeathCry.transform.parent = parent;
			prodDeathCry.transform.localPositionZero();
			prodDeathCry.transform.localScaleZero();
			prodDeathCry.Init(model);
			return prodDeathCry;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiForeground);
			Mem.Del(ref _traShipAnchor);
			Mem.Del(ref _uiShipTexture);
			Mem.Del(ref _psStar);
			Mem.Del(ref _psSmoke);
			Mem.Del(ref _fVoiceLength);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _clsShipModel);
		}

		private bool Init(ShipModel_BattleAll model)
		{
			_clsShipModel = model;
			_fVoiceLength = ShipUtils.GetVoiceLength(_clsShipModel, 22);
			_uiShipTexture.mainTexture = ShipUtils.LoadTexture(model, isStart: true);
			_uiShipTexture.transform.localPosition = ShipUtils.GetShipOffsPos(_clsShipModel, isDamaged: false, MstShipGraphColumn.CutInSp1);
			_psStar.Stop();
			_psSmoke.Stop();
			return true;
		}

        public override void Play(Action callback)
        {
            this._traShipAnchor.transform.localScaleOne();
            float num = Mathe.Lerp(0f, this._fVoiceLength, 0.5f);
            this._psStar.Play();
            this._psSmoke.Play();
            ShipUtils.PlayBossDeathCryVoice(this._clsShipModel);
            this._uiForeground.alpha = 0f;
            this._traShipAnchor.transform.LTScale(Vector3.one * 0.8f, this._fVoiceLength).setEase(LeanTweenType.linear).setOnComplete(new Action(this.onAnimationFinished));
            this._uiForeground.transform.LTValue(0f, 1f, num).setDelay(num).setEase(LeanTweenType.easeOutSine).setOnUpdate(delegate (float x)
            {
                this._uiForeground.alpha = x;
            });
            base.Play(callback);
        }

		protected override void onAnimationFinished()
		{
			_psStar.Stop();
			_psSmoke.Stop();
			base.onAnimationFinished();
		}
	}
}
