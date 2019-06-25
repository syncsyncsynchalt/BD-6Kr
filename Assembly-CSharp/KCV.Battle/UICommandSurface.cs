using Common.Enum;
using KCV.Battle.Utils;
using KCV.Generic;
using KCV.Utils;
using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(UIWidget))]
	public class UICommandSurface : MonoBehaviour, IUICommandSurface
	{
		[SerializeField]
		private UISprite _uiBackground;

		[SerializeField]
		private UISprite _uiForeground;

		[SerializeField]
		private UISprite _uiGrow;

		[SerializeField]
		private UISprite _uiIcon;

		[SerializeField]
		private UISprite _uiIconGrow;

		[SerializeField]
		private UILabel _uiNum;

		[SerializeField]
		private Vector3 _vReductionSize = Vector3.one * 0.7f;

		[SerializeField]
		private Vector3 _vMagnifySize = Vector3.one;

		[SerializeField]
		private float _fScalingTime = 0.2f;

		private BoxCollider2D _colBox2D;

		private BattleCommand _iCommandType;

		private int _nDefaultDepth;

		private int _nIndex;

		private bool _isAbsorded;

		private bool _isMagnify;

		private bool _isReduction;

		private bool _isGrowHex;

		private Action _actOnSetCommandUnit;

		private UIWidget _uiWidget;

		public int depth
		{
			get
			{
				return _uiBackground.depth;
			}
			set
			{
				if (value != _uiBackground.depth)
				{
					_uiBackground.depth = value;
					_uiGrow.depth = value + 1;
					_uiForeground.depth = value + 2;
					_uiNum.depth = value + 3;
					_uiIconGrow.depth = value + 4;
					_uiIcon.depth = value + 5;
				}
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
				_uiNum.textInt = value + 1;
			}
		}

		public BoxCollider2D boxCollider2D => this.GetComponentThis(ref _colBox2D);

		public BattleCommand commandType
		{
			get
			{
				return _iCommandType;
			}
			set
			{
				_iCommandType = value;
				if (_iCommandType == BattleCommand.None)
				{
					_uiIcon.spriteName = string.Empty;
					return;
				}
				_uiIcon.spriteName = $"command_{(int)value}_on";
				_uiIcon.MakePixelPerfect();
			}
		}

		public bool isSetUnit => commandType != BattleCommand.None;

		private bool isGrowHex
		{
			set
			{
				if (value)
				{
					if (!_isGrowHex)
					{
						_isGrowHex = true;
						_uiGrow.color = KCVColor.BattleCommandSurfaceBlue;
						_uiGrow.alpha = 0f;
						_uiGrow.transform.LTValue(0f, 1f, 1f).setLoopPingPong(int.MaxValue).setOnUpdate(delegate(float x)
						{
							_uiGrow.alpha = x;
						});
					}
				}
				else if (_isGrowHex)
				{
					_isGrowHex = false;
					_uiGrow.transform.LTCancel();
					_uiGrow.transform.LTValue(_uiGrow.alpha, 0f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
					{
						_uiGrow.alpha = x;
					});
				}
			}
		}

		public bool isMagnify
		{
			get
			{
				return _isMagnify;
			}
			private set
			{
				if (value)
				{
					if (!_isMagnify)
					{
						_isMagnify = true;
					}
				}
				else if (_isMagnify)
				{
					_isMagnify = false;
				}
			}
		}

		public bool isReduction
		{
			get
			{
				return _isReduction;
			}
			private set
			{
				if (value)
				{
					if (!_isReduction)
					{
						_isReduction = true;
					}
				}
				else if (_isReduction)
				{
					_isReduction = false;
				}
			}
		}

		public UIWidget widget => this.GetComponentThis(ref _uiWidget);

		public bool isAbsorded
		{
			get
			{
				return _isAbsorded;
			}
			set
			{
				if (value)
				{
					if (!_isAbsorded)
					{
						KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_999);
						_uiIconGrow.transform.LTValue(1f, 0f, 0.35f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
						{
							_uiIconGrow.alpha = x;
						});
						_isAbsorded = true;
					}
				}
				else if (_isAbsorded)
				{
					_isAbsorded = false;
				}
			}
		}

		public static UICommandSurface Instantiate(UICommandSurface prefab, Transform parent, Vector3 pos, int nIndex, BattleCommand iCommand, Action onSetCommandUnit)
		{
			UICommandSurface uICommandSurface = UnityEngine.Object.Instantiate(prefab);
			uICommandSurface.transform.parent = parent;
			uICommandSurface.transform.localScale = uICommandSurface._vReductionSize;
			uICommandSurface.transform.localPosition = pos;
			uICommandSurface.Init(nIndex, iCommand, onSetCommandUnit);
			return uICommandSurface;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiForeground);
			Mem.Del(ref _uiGrow);
			Mem.Del(ref _uiIcon);
			Mem.Del(ref _uiIconGrow);
			Mem.Del(ref _uiNum);
			Mem.Del(ref _vReductionSize);
			Mem.Del(ref _vMagnifySize);
			Mem.Del(ref _fScalingTime);
			Mem.Del(ref _colBox2D);
			Mem.Del(ref _iCommandType);
			Mem.Del(ref _nDefaultDepth);
			Mem.Del(ref _nIndex);
			Mem.Del(ref _isAbsorded);
			Mem.Del(ref _isMagnify);
			Mem.Del(ref _isReduction);
			Mem.Del(ref _isGrowHex);
			Mem.Del(ref _actOnSetCommandUnit);
			Mem.Del(ref _uiWidget);
		}

		private bool Init(int nIndex, BattleCommand iCommand, Action onSetCommandUnit)
		{
			index = nIndex;
			_actOnSetCommandUnit = onSetCommandUnit;
			_isAbsorded = false;
			_isMagnify = false;
			_isReduction = false;
			_isGrowHex = false;
			commandType = iCommand;
			_nDefaultDepth = 1;
			depth = _nDefaultDepth;
			_uiGrow.alpha = 0f;
			_uiIconGrow.alpha = 0f;
			isGrowHex = isSetUnit;
			return true;
		}

		public void SetCommandUnit(UICommandUnitIcon unit)
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_003);
			commandType = unit.commandType;
			isGrowHex = true;
			Dlg.Call(ref _actOnSetCommandUnit);
			ReductionUnitSet();
		}

		public void RemoveCommandUnit()
		{
			if (commandType != BattleCommand.None)
			{
				KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
				commandType = BattleCommand.None;
				isAbsorded = false;
				isGrowHex = false;
				Dlg.Call(ref _actOnSetCommandUnit);
			}
		}

		public void ChkSurfaceMagnifyDistance(UICommandUnitIcon unit)
		{
			if (Vector3.Distance(base.transform.position, unit.transform.position) < 0.09f)
			{
				Absorded(unit);
			}
			else if (Vector3.Distance(base.transform.position, unit.transform.position) < 0.5f)
			{
				isAbsorded = false;
				Magnify();
			}
			else
			{
				isAbsorded = false;
				Reduction();
			}
		}

		public void Absorded(UICommandUnitIcon unit)
		{
			isAbsorded = true;
			Magnify();
			unit.transform.position = _uiIcon.transform.position;
			unit.isActiveIcon = true;
		}

		public LTDescr Magnify()
		{
			if (base.transform.localScale != _vMagnifySize)
			{
				if (!isMagnify)
				{
					depth = 10;
					isMagnify = true;
					return base.transform.LTScale(_vMagnifySize, _fScalingTime).setEase(LeanTweenType.easeOutSine).setOnComplete((Action)delegate
					{
						isMagnify = false;
						isGrowHex = false;
					});
				}
				return null;
			}
			return null;
		}

		public LTDescr Reduction()
		{
			if (base.transform.localScale != _vReductionSize)
			{
				if (!isReduction)
				{
					isReduction = true;
					depth = _nDefaultDepth;
					return base.transform.LTScale(_vReductionSize, _fScalingTime).setEase(LeanTweenType.easeOutSine).setOnComplete((Action)delegate
					{
						if (isSetUnit)
						{
							isGrowHex = true;
						}
						isReduction = false;
					});
				}
				return null;
			}
			return null;
		}

		private LTDescr ReductionUnitSet()
		{
			if (base.transform.localScale != Vector3.one * 1.1f)
			{
				base.transform.localScale = Vector3.one * 1.1f;
				return base.transform.LTScale(_vReductionSize, 0.3f).setEase(LeanTweenType.easeOutBounce).setOnComplete((Action)delegate
				{
					depth = _nDefaultDepth;
				});
			}
			return null;
		}

		public LTDescr Show(float time)
		{
			return widget.transform.LTValue(widget.alpha, 1f, time).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				widget.alpha = x;
			});
		}

		public LTDescr Hide(float time)
		{
			return widget.transform.LTValue(widget.alpha, 0f, time).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				widget.alpha = x;
			});
		}

		private void OnClick()
		{
			RemoveCommandUnit();
		}
	}
}
