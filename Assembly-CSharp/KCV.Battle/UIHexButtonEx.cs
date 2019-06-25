using KCV.Battle.Utils;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(Animation))]
	[RequireComponent(typeof(UIToggle))]
	public class UIHexButtonEx : MonoBehaviour
	{
		public enum AnimationList
		{
			HexButtonShow,
			HexButtonSelect
		}

		[SerializeField]
		protected UISprite _uiHexSprite;

		[SerializeField]
		protected UISprite _uiHexSelector;

		[SerializeField]
		protected Transform _traForeground;

		[SerializeField]
		protected UIWidget _uiWidgetLabel;

		protected int _nIndex;

		protected int _nSpriteIndex;

		protected bool _isFocus;

		protected UIToggle _uiToggle;

		protected BoxCollider2D _colBoxCollider2D;

		protected Animation _anim;

		protected AnimationList _iList;

		protected Action _actCallback;

		protected DelDecideHexButtonEx _delDecideAdvancingWithdrawalButtonEx;

		public virtual int index
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

		public virtual int spriteIndex
		{
			get
			{
				return _nSpriteIndex;
			}
			set
			{
				_nSpriteIndex = Mathe.MinMax2(value, 0, 16);
				_uiHexSprite.spriteName = $"hex_{_nSpriteIndex + 4:D5}";
			}
		}

		public virtual bool isFocus
		{
			get
			{
				return _isFocus;
			}
			set
			{
				if (value)
				{
					if (!_isFocus)
					{
						_isFocus = true;
						toggle.Set(state: true);
						SetForeground();
						PlayFocusAnimation();
					}
				}
				else if (_isFocus)
				{
					_isFocus = false;
					toggle.Set(state: false);
					SetForeground();
					StopFocusAnimatiom();
				}
			}
		}

		public virtual UIToggle toggle
		{
			get
			{
				if (_uiToggle == null)
				{
					_uiToggle = GetComponent<UIToggle>();
				}
				return _uiToggle;
			}
		}

		public new virtual BoxCollider2D collider2D
		{
			get
			{
				if (_colBoxCollider2D == null)
				{
					_colBoxCollider2D = GetComponent<BoxCollider2D>();
				}
				return _colBoxCollider2D;
			}
		}

		public virtual bool isColliderEnabled
		{
			get
			{
				return collider2D.enabled;
			}
			set
			{
				collider2D.enabled = value;
			}
		}

		public virtual int toggleGroup
		{
			get
			{
				return toggle.group;
			}
			private set
			{
				toggle.group = value;
			}
		}

		public new virtual Animation animation
		{
			get
			{
				if ((UnityEngine.Object)_anim == null)
				{
					_anim = GetComponent<Animation>();
				}
				return _anim;
			}
		}

		private void Awake()
		{
			_nIndex = 0;
			_nSpriteIndex = 0;
			isFocus = false;
			_actCallback = null;
			_delDecideAdvancingWithdrawalButtonEx = null;
			toggle.startsActive = false;
			toggle.validator = OnValidator;
			toggle.onDecide = OnDecide;
			_uiHexSelector.alpha = 0.01f;
			spriteIndex = 0;
			toggle.Set(state: false);
			SetForeground();
			StopFocusAnimatiom();
			base.transform.localScaleZero();
			toggle.onDecide = OnDecide;
		}

		private void OnDestroy()
		{
			OnUnInit();
			Mem.Del(ref _uiHexSprite);
			Mem.Del(ref _uiHexSelector);
			Mem.Del(ref _traForeground);
			Mem.Del(ref _nIndex);
			Mem.Del(ref _nSpriteIndex);
			Mem.Del(ref _isFocus);
			Mem.Del(ref _uiToggle);
			Mem.Del(ref _colBoxCollider2D);
			Mem.Del(ref _anim);
			Mem.Del(ref _actCallback);
			Mem.Del(ref _delDecideAdvancingWithdrawalButtonEx);
		}

		public bool Init(int nIndex, bool isColliderEnabled, int nGroup, DelDecideHexButtonEx decideDelegate)
		{
			index = nIndex;
			this.isColliderEnabled = isColliderEnabled;
			toggleGroup = nGroup;
			_delDecideAdvancingWithdrawalButtonEx = decideDelegate;
			OnInit();
			return true;
		}

		public bool Init(int nIndex, bool isColliderEnabled, int nGroup, Action onDecide)
		{
			index = nIndex;
			this.isColliderEnabled = isColliderEnabled;
			toggleGroup = nGroup;
			toggle.onDecide = onDecide;
			OnInit();
			return true;
		}

		protected virtual void OnInit()
		{
		}

		protected virtual void OnUnInit()
		{
		}

		public void Play(Action callback)
		{
			base.transform.localScaleOne();
			isFocus = false;
			_actCallback = callback;
			_uiHexSprite.color = Color.white;
			animation.Play(AnimationList.HexButtonShow.ToString());
		}

		protected virtual void SetForeground()
		{
			UILabel component = ((Component)_traForeground).GetComponent<UILabel>();
			if (component != null)
			{
				component.color = ((!toggle.value) ? Color.gray : Color.white);
			}
			if (_uiWidgetLabel != null)
			{
				_uiWidgetLabel.color = ((!toggle.value) ? Color.gray : Color.white);
			}
		}

		private void PlayFocusAnimation()
		{
			animation.wrapMode = WrapMode.Loop;
			_uiHexSelector.SetActive(isActive: true);
			animation.Play(AnimationList.HexButtonSelect.ToString());
		}

		private void StopFocusAnimatiom()
		{
			_uiHexSelector.SetActive(isActive: false);
			animation.Stop();
			animation.wrapMode = WrapMode.Default;
		}

		private void ChangeSprite(int nIndex)
		{
			spriteIndex = nIndex;
		}

		private void onAnimationFinished()
		{
			spriteIndex = 16;
			_uiWidgetLabel.alpha = 1f;
			_uiHexSprite.color = Color.gray;
			_isFocus = false;
			SetForeground();
			if (_actCallback != null)
			{
				_actCallback();
			}
		}

		private bool OnValidator(bool isChoice)
		{
			isFocus = isChoice;
			return true;
		}

		public void OnDecide()
		{
			Dlg.Call(ref toggle.onDecide);
			if (_delDecideAdvancingWithdrawalButtonEx != null)
			{
				_delDecideAdvancingWithdrawalButtonEx(this);
			}
		}
	}
}
