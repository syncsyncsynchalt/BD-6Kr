using Common.Enum;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Title
{
	[RequireComponent(typeof(UIToggle))]
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(UISprite))]
	public class UIDifficultyBtn : MonoBehaviour
	{
		private const int PAPER_PAGE_MAX = 21;

		[SerializeField]
		private UISprite _uiForeground;

		[SerializeField]
		private UISprite _uiForegroundPaper;

		[SerializeField]
		private UISprite _uiDifficultyLabel;

		[SerializeField]
		private UISprite _uiDifficultyRedLabel;

		private UISprite _uiBackground;

		private DifficultKind _iKind;

		private bool _isFocus;

		private int _nIndex;

		private int _nPaperIndex;

		private UIWidget _uiWidget;

		private UIToggle _uiToggle;

		private BoxCollider2D _colBox2D;

		private IDisposable _disPaperAnimCancel;

		private UISprite background => this.GetComponentThis(ref _uiBackground);

		public DifficultKind difficultKind
		{
			get
			{
				return _iKind;
			}
			private set
			{
				_iKind = value;
				_uiForeground.spriteName = $"txt_diff{(int)_iKind}_gray";
				_uiForeground.MakePixelPerfect();
				_uiDifficultyLabel.spriteName = $"btn_diff{(int)_iKind}_txt";
				_uiDifficultyLabel.MakePixelPerfect();
				_uiDifficultyRedLabel.spriteName = $"btn_diff{(int)_iKind}_txt_red";
				_uiDifficultyRedLabel.MakePixelPerfect();
			}
		}

		public int index
		{
			get
			{
				return _nIndex;
			}
			private set
			{
				_nIndex = value;
			}
		}

		private int paperIndex
		{
			get
			{
				return _nPaperIndex;
			}
			set
			{
				_nPaperIndex = Mathe.MinMax2(value, 2, 23);
				background.spriteName = $"btn_diff1_{_nPaperIndex + 2:D5}";
			}
		}

		public bool isFocus
		{
			get
			{
				return _isFocus;
			}
			set
			{
				if (value)
				{
					if (_isFocus != value)
					{
						_isFocus = true;
						_uiForeground.spriteName = $"txt_diff{(int)_iKind}";
						_uiForeground.MakePixelPerfect();
						base.transform.LTCancel();
						base.transform.LTScale(Vector3.one * 1.1f, Mathe.Frame2Sec(5)).setEase(LeanTweenType.easeOutSine);
						base.transform.LTMoveLocalY(15f, Mathe.Frame2Sec(5)).setEase(LeanTweenType.linear);
						PlayPaperAnimation();
						toggle.value = true;
					}
				}
				else if (_isFocus != value)
				{
					_isFocus = false;
					_uiForeground.spriteName = $"txt_diff{(int)_iKind}_gray";
					_uiForeground.MakePixelPerfect();
					_uiBackground.localSize = new Vector2(138f, 234f);
					base.transform.LTCancel();
					base.transform.LTScale(Vector3.one, Mathe.Frame2Sec(22)).setEase(LeanTweenType.easeOutSine);
					base.transform.LTMoveLocalY(0f, Mathe.Frame2Sec(22)).setEase(LeanTweenType.easeOutCubic);
					if (_disPaperAnimCancel != null)
					{
						_disPaperAnimCancel.Dispose();
					}
					background.spriteName = "btn_diff1_gray";
					toggle.value = false;
					HideForegroundObjests();
				}
			}
		}

		public UIWidget widget => this.GetComponentThis(ref _uiWidget);

		public UIToggle toggle => this.GetComponentThis(ref _uiToggle);

		public new BoxCollider2D collider => this.GetComponentThis(ref _colBox2D);

		public Transform wrightBrushAnchor => _uiForeground.transform;

		public static UIDifficultyBtn Instantiate(UIDifficultyBtn prefab, Transform parent, DifficultKind iKind, int nIndex)
		{
			UIDifficultyBtn uIDifficultyBtn = UnityEngine.Object.Instantiate(prefab);
			uIDifficultyBtn.transform.parent = parent;
			uIDifficultyBtn.transform.localScaleOne();
			uIDifficultyBtn.transform.localPositionZero();
			uIDifficultyBtn.Init(iKind, nIndex);
			return uIDifficultyBtn;
		}

		private bool Init(DifficultKind iKind, int nIndex)
		{
			difficultKind = iKind;
			index = nIndex;
			_isFocus = false;
			_uiForeground.spriteName = $"txt_diff{(int)iKind}_gray";
			_uiForeground.MakePixelPerfect();
			background.spriteName = "btn_diff1_gray";
			_uiForegroundPaper.alpha = 0f;
			_uiDifficultyLabel.alpha = 0f;
			_uiDifficultyRedLabel.alpha = 0f;
			return true;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiForeground);
			Mem.Del(ref _uiForegroundPaper);
			Mem.Del(ref _uiDifficultyLabel);
			Mem.Del(ref _uiDifficultyRedLabel);
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _iKind);
			Mem.Del(ref _isFocus);
			Mem.Del(ref _nIndex);
			Mem.Del(ref _nPaperIndex);
			Mem.Del(ref _uiWidget);
			Mem.Del(ref _uiToggle);
			Mem.Del(ref _colBox2D);
			if (_disPaperAnimCancel != null)
			{
				_disPaperAnimCancel.Dispose();
			}
			Mem.Del(ref _disPaperAnimCancel);
		}

		private void PlayPaperAnimation()
		{
			if (_disPaperAnimCancel != null)
			{
				_disPaperAnimCancel.Dispose();
			}
			paperIndex = 0;
			_disPaperAnimCancel = Observable.IntervalFrame(0).Select(delegate(long x)
			{
				long num = x;
				x = num + 1;
				return num;
			}).TakeWhile((long x) => x <= 21)
				.Subscribe(delegate(long x)
				{
					paperIndex = (int)x;
					if (paperIndex == 15)
					{
						ShowForegroundObjects();
					}
				})
				.AddTo(base.gameObject);
		}

		private void ShowForegroundObjects()
		{
			_uiForegroundPaper.transform.LTCancel();
			_uiForegroundPaper.transform.LTValue(_uiForegroundPaper.alpha, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiForegroundPaper.alpha = x;
				_uiDifficultyLabel.alpha = x;
			});
		}

		private void HideForegroundObjests()
		{
			_uiForegroundPaper.transform.LTCancel();
			_uiForegroundPaper.transform.LTValue(_uiForegroundPaper.alpha, 0f, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiForegroundPaper.alpha = x;
				_uiDifficultyLabel.alpha = x;
			});
		}

		public void ShowDifficultyRedLabel()
		{
			_uiDifficultyRedLabel.transform.LTCancel();
			_uiDifficultyRedLabel.transform.LTValue(_uiDifficultyRedLabel.alpha, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiDifficultyRedLabel.alpha = x;
			});
		}
	}
}
