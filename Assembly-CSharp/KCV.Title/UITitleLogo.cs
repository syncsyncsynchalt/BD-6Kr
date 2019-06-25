using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UIPanel))]
	public class UITitleLogo : MonoBehaviour
	{
		[Serializable]
		private struct Params
		{
			[SerializeField]
			private float _fShowTime;

			[SerializeField]
			private float _fHideTime;

			[SerializeField]
			private float _fMinAlpha;

			[SerializeField]
			private float _fMaxAlpha;

			[SerializeField]
			private float _fAddTime;

			[SerializeField]
			private float _fSubTime;

			[SerializeField]
			private LeanTweenType _iAddEase;

			[SerializeField]
			private LeanTweenType _iSubEase;

			public float showTime => _fShowTime;

			public float hideTime => _fHideTime;

			public float minAlpha => Mathe.Rate(0f, 255f, _fMinAlpha);

			public float maxAlpha => Mathe.Rate(0f, 255f, _fMaxAlpha);

			public float addTime => _fAddTime;

			public float subTime => _fSubTime;

			public LeanTweenType addEase => _iAddEase;

			public LeanTweenType subEase => _iSubEase;

			public Params(float showTime, float hideTime, float minAlpha, float maxAlpha, float addTime, float subTime, LeanTweenType addEase, LeanTweenType subEase)
			{
				_fShowTime = showTime;
				_fHideTime = hideTime;
				_fMinAlpha = minAlpha;
				_fMaxAlpha = maxAlpha;
				_fAddTime = addTime;
				_fSubTime = subTime;
				_iAddEase = addEase;
				_iSubEase = subEase;
			}
		}

		[SerializeField]
		private UITexture _uiLogo;

		[SerializeField]
		[Header("[Logo Animation Param]")]
		private Params _strParams;

		private UIPanel _uiPanel;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		private void Awake()
		{
			panel.alpha = 0f;
		}

		private void OnDestroy()
		{
			_uiLogo.transform.LTCancel();
			panel.transform.LTCancel();
			Mem.Del(ref _uiLogo);
			Mem.Del(ref _strParams);
			Mem.Del(ref _uiPanel);
		}

		public void StartLogoAnim()
		{
			_uiLogo.transform.LTValue(_strParams.maxAlpha, _strParams.minAlpha, _strParams.subTime).setEase(_strParams.subEase).setOnUpdate(delegate(float x)
			{
				_uiLogo.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					_uiLogo.transform.LTValue(_strParams.minAlpha, _strParams.maxAlpha, _strParams.addTime).setOnUpdate(delegate(float x)
					{
						_uiLogo.alpha = x;
					}).setEase(_strParams.addEase)
						.setOnComplete(StartLogoAnim);
				});
		}

		public LTDescr Show()
		{
			panel.alpha = 0f;
			return panel.transform.LTValue(panel.alpha, 1f, _strParams.showTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			});
		}

		public LTDescr Hide()
		{
			return panel.transform.LTValue(panel.alpha, 0f, _strParams.hideTime).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panel.alpha = x;
			})
				.setOnComplete((Action)delegate
				{
					_uiLogo.transform.LTCancel();
					_uiLogo.alpha = _strParams.maxAlpha;
				});
		}
	}
}
