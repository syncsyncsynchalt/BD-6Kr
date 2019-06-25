using KCV.SortieBattle;
using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	public class UISortieShipCharacter : BaseUISortieBattleShip<ShipModel>
	{
		[Serializable]
		private struct AnimParams
		{
			public Vector3 showPos;

			public Vector3 hidePos;

			public float animationTime;
		}

		[SerializeField]
		[Header("[Animation Parameter]")]
		private AnimParams _strAnimParams;

		private int _nDefaultDepth;

		private bool _isInDisplay;

		public bool isInDisplay => _isInDisplay;

		private void Awake()
		{
			_nDefaultDepth = panel.depth;
			_isInDisplay = false;
		}

		public void SetShipData(ShipModel model)
		{
			base.SetShipTexture(model);
			_uiShipTex.transform.localPosition(Util.Poi2Vec(model.Offsets.GetCutinSp1_InBattle(model.IsDamaged())));
		}

		public void DrawDefault()
		{
			_uiShipTex.alpha = 1f;
			_uiShipTex.transform.localPosition(Util.Poi2Vec(shipModel.Offsets.GetCutinSp1_InBattle(shipModel.IsDamaged())));
			_isInDisplay = true;
		}

		public void SetInDisplayNextMove(bool isInDisplay)
		{
			_isInDisplay = isInDisplay;
		}

		public void Show(Action callback)
		{
			_isInDisplay = true;
			panel.depth = _nDefaultDepth;
			ShowAnimation(callback);
		}

		public void ShowInItemGet(Action onFinished)
		{
			Observable.Timer(TimeSpan.FromSeconds(0.75)).Subscribe(delegate
			{
				ShipUtils.PlayShipVoice(shipModel, 26);
			});
			HideDelayAfterDisplay(onFinished);
		}

		public void ShowInFormation(int nPanelDepth, Action onFinished)
		{
			panel.depth = nPanelDepth;
			_uiShipTex.transform.LTCancel();
			_uiShipTex.transform.LTValue(_uiShipTex.alpha, 1f, _strAnimParams.animationTime).setEase(LeanTweenType.easeOutCubic).setOnUpdate(delegate(float x)
			{
				_uiShipTex.alpha = x;
			});
			base.transform.localPosition = _strAnimParams.hidePos;
			base.transform.LTMoveLocal(_strAnimParams.showPos, _strAnimParams.animationTime).setEase(LeanTweenType.easeOutCubic).setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinished);
			});
		}

		private void ShowAnimation(Action onFinished)
		{
			base.transform.localPosition = _strAnimParams.showPos;
			base.transform.LTCancel();
			base.transform.LTMoveLocal(_strAnimParams.hidePos, _strAnimParams.animationTime).setEase(LeanTweenType.linear).setDelay(0.6f);
			_uiShipTex.transform.LTCancel();
			_uiShipTex.transform.LTValue(_uiShipTex.alpha, 1f, _strAnimParams.animationTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiShipTex.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					Hide(delegate
					{
						Dlg.Call(ref onFinished);
					});
				});
		}

		public void Hide(Action callback)
		{
			base.transform.LTCancel();
			base.transform.LTMoveLocal(_strAnimParams.hidePos, _strAnimParams.animationTime).setEase(LeanTweenType.linear);
			_uiShipTex.transform.LTCancel();
			_uiShipTex.transform.LTValue(_uiShipTex.alpha, 0f, _strAnimParams.animationTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiShipTex.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					Dlg.Call(ref callback);
				});
		}

		private void HideDelayAfterDisplay(Action onFinished)
		{
			float delayTime = _strAnimParams.animationTime + 0.5f;
			float delayTime2 = _strAnimParams.animationTime + delayTime;
			base.transform.LTCancel();
			base.transform.LTDelayedCall(delayTime2, (Action)delegate
			{
				Dlg.Call(ref onFinished);
			}).setOnStart(delegate
			{
				base.transform.localPosition = _strAnimParams.showPos;
				base.transform.LTValue(_uiShipTex.alpha, 1f, _strAnimParams.animationTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					_uiShipTex.alpha = x;
				});
				base.transform.LTValue(1f, 0f, _strAnimParams.animationTime).setDelay(delayTime).setEase(LeanTweenType.linear)
					.setOnUpdate(delegate(float x)
					{
						_uiShipTex.alpha = x;
					});
				base.transform.LTMoveLocal(_strAnimParams.hidePos, _strAnimParams.animationTime).setDelay(delayTime).setEase(LeanTweenType.linear);
			});
		}
	}
}
