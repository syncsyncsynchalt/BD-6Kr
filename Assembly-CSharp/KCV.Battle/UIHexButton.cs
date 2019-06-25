using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(Animation))]
	[RequireComponent(typeof(UIButton))]
	[RequireComponent(typeof(BoxCollider2D))]
	public class UIHexButton : BaseAnimation
	{
		public enum AnimationList
		{
			HexButtonShow,
			ProdTranscendenceAttackHex,
			HexButtonSelect
		}

		protected UISprite _uiHexBtn;

		protected List<UIButton> _listBtns;

		protected UIToggle _toggle;

		protected int _nIndex;

		protected int _nSpriteIndex;

		protected bool _isFocus;

		protected string[] _strSpriteNames;

		protected AnimationList _iList;

		public virtual int index => _nIndex;

		public virtual int spriteIndex
		{
			get
			{
				return _nSpriteIndex;
			}
			set
			{
				_nSpriteIndex = value;
				_uiHexBtn.spriteName = _strSpriteNames[_nSpriteIndex];
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
						SetButtonState(UIButtonColor.State.Pressed);
					}
				}
				else if (_isFocus)
				{
					_isFocus = false;
					SetButtonState(UIButtonColor.State.Normal);
				}
			}
		}

		public bool isColliderEnabled
		{
			get
			{
				return GetComponent<Collider2D>().enabled;
			}
			set
			{
				GetComponent<Collider2D>().enabled = value;
			}
		}

		public virtual AnimationList list => _iList;

		public virtual UIButton uiButton => _listBtns[0];

		public virtual List<UIButton> buttonList => _listBtns;

		protected new virtual void Awake()
		{
			base.Awake();
			Init();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiHexBtn);
			Mem.DelListSafe(ref _listBtns);
			Mem.Del(ref _toggle);
			Mem.Del(ref _nIndex);
			Mem.Del(ref _nSpriteIndex);
			Mem.Del(ref _isFocus);
			Mem.DelArySafe(ref _strSpriteNames);
			Mem.Del(ref _iList);
		}

		public new virtual bool Init()
		{
			_uiHexBtn = ((Component)base.transform.FindChild("HexBtn")).GetComponent<UISprite>();
			_uiHexBtn.spriteName = string.Empty;
			_toggle = ((Component)base.transform).GetComponent<UIToggle>();
			_listBtns = new List<UIButton>();
			_listBtns.AddRange(GetComponents<UIButton>());
			_animAnimation = GetComponent<Animation>();
			_animAnimation.Stop();
			_nSpriteIndex = 0;
			_strSpriteNames = new string[16];
			for (int i = 0; i < 16; i++)
			{
				string str = $"{i + 4:D2}";
				_strSpriteNames[i] = "hex_000" + str;
			}
			this.SetActive(isActive: false);
			isFocus = true;
			return true;
		}

		public virtual bool Run()
		{
			if (!isFocus && uiButton.state != 0)
			{
				foreach (UIButton listBtn in _listBtns)
				{
					if (listBtn.state != 0)
					{
						listBtn.state = UIButtonColor.State.Normal;
					}
				}
			}
			return true;
		}

		public virtual void Play(AnimationList iList, Action callback)
		{
			_iList = iList;
			_isFinished = false;
			_actCallback = callback;
			_nSpriteIndex = 0;
			_uiHexBtn.spriteName = _strSpriteNames[_nSpriteIndex];
			this.SetActive(isActive: true);
			_animAnimation.Play(iList.ToString());
		}

		public void SetFocusAnimation()
		{
			_animAnimation.Stop();
			_animAnimation.wrapMode = WrapMode.Default;
			UISprite component = ((Component)base.transform.FindChild("HexSelect")).GetComponent<UISprite>();
			component.alpha = 0.03f;
			_toggle.startsActive = false;
			if (isFocus)
			{
				_toggle.startsActive = true;
				_animAnimation.wrapMode = WrapMode.Loop;
				_iList = AnimationList.HexButtonSelect;
				_animAnimation.Play(_iList.ToString());
			}
		}

		public virtual void SetIndex(int nIndex)
		{
			_nIndex = nIndex;
		}

		public virtual void SetDepth(int nDepth)
		{
		}

		protected virtual void SetButtonState(UIButtonColor.State iState)
		{
			foreach (UIButton listBtn in _listBtns)
			{
				if (listBtn.state != iState)
				{
					listBtn.state = iState;
				}
			}
		}

		public void ChangeSprite(int nIndex)
		{
			if (_strSpriteNames[nIndex] != string.Empty)
			{
				_nSpriteIndex = nIndex;
				_uiHexBtn.spriteName = _strSpriteNames[_nSpriteIndex];
			}
		}

		protected override void onAnimationFinished()
		{
			base.onAnimationFinished();
			isColliderEnabled = true;
		}
	}
}
