using Common.Enum;
using KCV.Battle.Production;
using KCV.Utils;
using LT.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class UICommandUnitIcon : UIDragDropItem
	{
		[SerializeField]
		private UISprite _uiIcon;

		private int _nDefaultIconDepth;

		private bool _isBorderOrver;

		private bool _isValid;

		private Vector3 _vStartPos;

		private BoxCollider2D _colBox2D;

		private BattleCommand _iCommandType;

		private Action<BattleCommand> _actOnDragStart;

		private Action _actOnDragAndDropRelease;

		public BattleCommand commandType
		{
			get
			{
				return _iCommandType;
			}
			set
			{
				_iCommandType = value;
				_uiIcon.spriteName = $"command_{(int)value}";
				_uiIcon.MakePixelPerfect();
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
				_uiIcon.color = ((!_isValid) ? Color.gray : Color.white);
				colliderBox2D.enabled = (_isValid ? true : false);
			}
		}

		public bool isActiveIcon
		{
			get
			{
				return _uiIcon.depth == 100;
			}
			set
			{
				if (value)
				{
					_uiIcon.depth = 100;
					_uiIcon.spriteName = $"command_{(int)_iCommandType}_on";
				}
				else
				{
					_uiIcon.spriteName = $"command_{(int)_iCommandType}";
					_uiIcon.depth = _nDefaultIconDepth;
				}
			}
		}

		public bool isFocus
		{
			get
			{
				return isActiveIcon;
			}
			set
			{
				if (value)
				{
					isActiveIcon = true;
					base.transform.LTCancel();
					base.transform.LTScale(Vector3.one * 1.05f, 0.2f).setEase(LeanTweenType.easeOutSine);
				}
				else
				{
					isActiveIcon = false;
					base.transform.LTCancel();
					base.transform.LTScale(Vector3.one * 0.95f, 0.2f).setEase(LeanTweenType.easeOutSine);
				}
			}
		}

		public BoxCollider2D colliderBox2D => this.GetComponentThis(ref _colBox2D);

		public static UICommandUnitIcon Instantiate(UICommandUnitIcon prefab, Transform parent, Vector3 pos, int nType)
		{
			UICommandUnitIcon uICommandUnitIcon = UnityEngine.Object.Instantiate(prefab);
			uICommandUnitIcon.transform.parent = parent;
			uICommandUnitIcon.transform.localPosition = pos;
			uICommandUnitIcon.transform.localScaleOne();
			return uICommandUnitIcon;
		}

		private void Awake()
		{
			_nDefaultIconDepth = _uiIcon.depth;
			_isBorderOrver = false;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiIcon);
			Mem.Del(ref _vStartPos);
			Mem.Del(ref _colBox2D);
			Mem.Del(ref _nDefaultIconDepth);
			Mem.Del(ref _iCommandType);
			Mem.Del(ref _isBorderOrver);
			Mem.Del(ref _actOnDragStart);
			Mem.Del(ref _actOnDragAndDropRelease);
		}

		public bool Init(BattleCommand nType, bool isValid, Action<BattleCommand> onDragStart, Action onDragDropRelease)
		{
			_vStartPos = base.transform.localPosition;
			commandType = nType;
			this.isValid = isValid;
			_actOnDragStart = onDragStart;
			_actOnDragAndDropRelease = onDragDropRelease;
			return true;
		}

		protected override void OnDragStart()
		{
			isFocus = true;
			Dlg.Call(ref _actOnDragStart, _iCommandType);
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			base.OnDragStart();
		}

		protected override void OnDragDropMove(Vector2 delta)
		{
			ProdBattleCommandSelect prodBattleCommandSelect = BattleTaskManager.GetTaskCommand().prodBattleCommandSelect;
			List<UICommandSurface> listCommandSurfaces = prodBattleCommandSelect.commandBox.listCommandSurfaces;
			UICommandSurface mainTarget = (from x in listCommandSurfaces
				orderby (x.transform.position - base.transform.position).magnitude
				select x).First();
			ChkBorderLine();
			listCommandSurfaces.ForEach(delegate(UICommandSurface x)
			{
				if (x == mainTarget)
				{
					x.ChkSurfaceMagnifyDistance(this);
				}
				else
				{
					x.Reduction();
				}
			});
			if (!mainTarget.isAbsorded)
			{
				base.OnDragDropMove(delta);
			}
		}

		protected override void OnDragDropRelease(GameObject surface)
		{
			if (surface != null)
			{
				List<UICommandSurface> list = BattleTaskManager.GetTaskCommand().prodBattleCommandSelect.commandBox.listCommandSurfaces.FindAll((UICommandSurface x) => x.isAbsorded);
				if (list != null && list.Count != 0)
				{
					list[0].SetCommandUnit(this);
				}
			}
			base.OnDragDropRelease(surface);
			Reset();
		}

		private void ChkBorderLine()
		{
			ProdBattleCommandSelect prodBattleCommandSelect = BattleTaskManager.GetTaskCommand().prodBattleCommandSelect;
			float unitIconLabelDrawBorderLineLocalPosX = prodBattleCommandSelect.commandUnitList.unitIconLabelDrawBorderLineLocalPosX;
			Vector3 localPosition = base.transform.localPosition;
			if (localPosition.x < unitIconLabelDrawBorderLineLocalPosX && !_isBorderOrver)
			{
				_isBorderOrver = true;
				isActiveIcon = false;
				return;
			}
			Vector3 localPosition2 = base.transform.localPosition;
			if (localPosition2.x >= unitIconLabelDrawBorderLineLocalPosX && _isBorderOrver)
			{
				_isBorderOrver = false;
				isActiveIcon = true;
			}
		}

		public bool Reset()
		{
			_isBorderOrver = false;
			isActiveIcon = false;
			base.transform.localPosition = _vStartPos;
			base.transform.localScale = Vector3.one * 0.95f;
			Dlg.Call(ref _actOnDragAndDropRelease);
			return true;
		}

		public void ResetPosition()
		{
			if (base.transform.localPosition != _vStartPos)
			{
				base.transform.localPosition = _vStartPos;
				isActiveIcon = false;
				isFocus = false;
			}
		}
	}
}
