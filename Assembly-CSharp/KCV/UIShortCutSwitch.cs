using LT.Tweening;
using System;
using UnityEngine;

namespace KCV
{
	public class UIShortCutSwitch : MonoBehaviour
	{
		[Serializable]
		private struct Param
		{
			public float showPosX;

			public float hidePosX;

			public float moveTime;
		}

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiGear;

		[SerializeField]
		private UILabel _uiStatus;

		[SerializeField]
		private Param _strAnimParam;

		private bool _isShortCut;

		private bool _isValid;

		private Color _colHalfRed = new Color(1f, 0f, 0f, 0.5f);

		private Color _colHalfBlack = new Color(0f, 0f, 0f, 0.5f);

		public bool isShortCut
		{
			get
			{
				return _isShortCut;
			}
			private set
			{
				_isShortCut = value;
			}
		}

		public bool isValid
		{
			get
			{
				return _isValid;
			}
			private set
			{
				_isValid = value;
			}
		}

		private void Awake()
		{
			_isShortCut = false;
			base.transform.localPositionX(_strAnimParam.hidePosX);
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiGear);
			Mem.Del(ref _uiStatus);
			Mem.Del(ref _strAnimParam);
			Mem.Del(ref _isShortCut);
			Mem.Del(ref _colHalfRed);
			Mem.Del(ref _colHalfBlack);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				SetIsValid(isValid: true, isAnimation: true);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				SetIsValid(isValid: false, isAnimation: true);
			}
		}

		public void SetDefault(bool isShortCut)
		{
			this.isShortCut = isShortCut;
			base.transform.localPositionX((!isShortCut) ? _strAnimParam.hidePosX : _strAnimParam.showPosX);
			SetIsValid(isValid: true, isAnimation: false);
		}

		public void SetIsValid(bool isValid, bool isAnimation)
		{
			_isValid = isValid;
			_uiStatus.text = ((!isValid) ? "決戦！ショ\u30fcトカット不可" : "戦闘ショ\u30fcトカット");
			if (isAnimation)
			{
				_uiBackground.transform.LTCancel();
				_uiBackground.transform.LTValue(_uiBackground.color, (!isValid) ? _colHalfRed : _colHalfBlack, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(Color x)
				{
					_uiBackground.color = x;
				});
			}
			else
			{
				_uiBackground.color = ((!isValid) ? _colHalfRed : _colHalfBlack);
			}
		}

		public void Switch()
		{
			if (isShortCut)
			{
				Hide();
			}
			else
			{
				Show();
			}
		}

		public LTDescr Show()
		{
			isShortCut = true;
			base.transform.LTCancel();
			return base.transform.LTMoveLocalX(_strAnimParam.showPosX, _strAnimParam.moveTime).setEase(LeanTweenType.easeOutQuad);
		}

		public LTDescr Hide()
		{
			isShortCut = false;
			base.transform.LTCancel();
			return base.transform.LTMoveLocalX(_strAnimParam.hidePosX, _strAnimParam.moveTime).setEase(LeanTweenType.easeOutQuad);
		}
	}
}
