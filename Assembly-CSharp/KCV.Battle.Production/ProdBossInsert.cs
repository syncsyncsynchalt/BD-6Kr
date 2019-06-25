using KCV.Battle.Utils;
using local.models;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[RequireComponent(typeof(Animation))]
	[RequireComponent(typeof(UIPanel))]
	public class ProdBossInsert : BaseAnimation
	{
		private enum AnimationList
		{
			ProdBossInsertMistIn,
			ProdBossInsertMistOut
		}

		[SerializeField]
		private UITexture _uiShipTexture;

		[SerializeField]
		private ParticleSystem _psSmoke;

		private UIPanel _uiPanel;

		private float _fVoiceLength;

		private ShipModel_BattleAll _clsShipModel;

		private UIPanel panel
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

		public ShipModel_BattleAll shipmodel => _clsShipModel;

		public static ProdBossInsert Instantiate(ProdBossInsert prefab, Transform parent, ShipModel_BattleAll model)
		{
			ProdBossInsert prodBossInsert = UnityEngine.Object.Instantiate(prefab);
			prodBossInsert.transform.parent = parent;
			prodBossInsert.transform.localScaleZero();
			prodBossInsert.transform.localPositionZero();
			prodBossInsert.Init(model);
			return prodBossInsert;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiShipTexture);
			Mem.Del(ref _psSmoke);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _fVoiceLength);
			Mem.Del(ref _clsShipModel);
		}

		private bool Init(ShipModel_BattleAll model)
		{
			_clsShipModel = model;
			_fVoiceLength = ShipUtils.GetVoiceLength(_clsShipModel, 1);
			_psSmoke.Stop();
			_uiShipTexture.mainTexture = ShipUtils.LoadTexture(model, isStart: true);
			_uiShipTexture.transform.localPosition = ShipUtils.GetShipOffsPos(_clsShipModel, isDamaged: false, MstShipGraphColumn.CutInSp1);
			_uiShipTexture.transform.localScale = Vector3.one * 0.95f;
			return true;
		}

		public override void Play(Action callback)
		{
			base.transform.localScaleOne();
			_psSmoke.Play();
			_uiShipTexture.transform.ScaleTo(Vector3.one, _fVoiceLength, iTween.EaseType.easeOutSine, null);
			base.Play(AnimationList.ProdBossInsertMistIn, callback);
		}

		private void PlayBossVoice()
		{
			float voiceLength = ShipUtils.GetVoiceLength(_clsShipModel, 1);
			ShipUtils.PlayBossInsertVoice(_clsShipModel);
			Observable.Timer(TimeSpan.FromSeconds(Mathe.Lerp(0f, voiceLength, 0.9f))).Subscribe(delegate
			{
                throw new NotImplementedException("‚È‚É‚±‚ê");
                // base.animation.get_Item(AnimationList.ProdBossInsertMistOut.ToString()).set_time(base.animation.get_Item(AnimationList.ProdBossInsertMistOut.ToString()).clip.length);
				// base.animation.get_Item(AnimationList.ProdBossInsertMistOut.ToString()).set_speed(-1f);

				Play(AnimationList.ProdBossInsertMistOut, null);
			});
		}

		protected override void onAnimationFinishedAfterDiscard()
		{
			base.onAnimationFinishedAfterDiscard();
		}
	}
}
