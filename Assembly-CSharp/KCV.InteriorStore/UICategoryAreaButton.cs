using Common.Enum;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UICategoryAreaButton : MonoBehaviour
	{
		private class Fairy
		{
			private UISprite _uiSprite;

			private bool _isEyeOpen;

			private float _fEyeInterval;

			private Dictionary<bool, Vector3> _dicFairyPos;

			public UISprite uiSprite => _uiSprite;

			public bool isEyeOpen => _isEyeOpen;

			public Fairy(Transform parent, string objName)
			{
				_isEyeOpen = true;
				_fEyeInterval = XorRandom.GetFLim(1f, 3f);
				Util.FindParentToChild(ref _uiSprite, parent, objName);
				_uiSprite.type = UIBasicSprite.Type.Filled;
				_uiSprite.flip = UIBasicSprite.Flip.Nothing;
				_uiSprite.fillDirection = UIBasicSprite.FillDirection.Vertical;
				_uiSprite.fillAmount = 0.55f;
				_uiSprite.invert = true;
				_uiSprite.spriteName = "mini_08_a_01";
				_dicFairyPos = new Dictionary<bool, Vector3>();
				_dicFairyPos.Add(key: false, Vector3.down * 20f);
				_dicFairyPos.Add(key: true, Vector3.up * 40f);
			}

			public bool Init()
			{
				return true;
			}

			public bool Reset()
			{
				_isEyeOpen = true;
				_fEyeInterval = XorRandom.GetFLim(1f, 3f);
				_uiSprite.spriteName = "mini_08_a_01";
				return true;
			}

			public bool UnInit()
			{
				_dicFairyPos.Clear();
				_dicFairyPos = null;
				return true;
			}

			public void ShowYousei()
			{
				Vector3 endValue = _dicFairyPos[true];
				float to = 1f;
				_uiSprite.transform.DOLocalMove(endValue, 0.25f).SetEase(Ease.OutExpo);
				DOVirtual.Float(_uiSprite.fillAmount, to, 0.1f, delegate(float amount)
				{
					_uiSprite.fillAmount = amount;
				}).SetEase(Ease.OutExpo);
			}

			public void HideYousei()
			{
				Vector3 endValue = _dicFairyPos[false];
				float to = 0.55f;
				_uiSprite.transform.DOLocalMove(endValue, 0.25f).SetEase(Ease.OutExpo);
				DOVirtual.Float(_uiSprite.fillAmount, to, 0.1f, delegate(float amount)
				{
					_uiSprite.fillAmount = amount;
				}).SetEase(Ease.OutExpo);
			}

			public void Update(bool isDecide)
			{
				if (isDecide && _fEyeInterval >= 0f && (_uiSprite.spriteName == "mini_08_a_01" || _uiSprite.spriteName == "mini_08_a_02"))
				{
					_uiSprite.spriteName = ((!_isEyeOpen) ? "mini_08_a_04" : "mini_08_a_03");
				}
				_fEyeInterval -= Time.deltaTime;
				if (_fEyeInterval <= 0f)
				{
					_isEyeOpen = !_isEyeOpen;
					if (isDecide)
					{
						_uiSprite.spriteName = ((!_isEyeOpen) ? "mini_08_a_04" : "mini_08_a_03");
					}
					else
					{
						_uiSprite.spriteName = ((!_isEyeOpen) ? "mini_08_a_02" : "mini_08_a_01");
					}
					_fEyeInterval = ((!_isEyeOpen) ? XorRandom.GetFLim(0.1f, 0.5f) : XorRandom.GetFLim(0.5f, 1.5f));
				}
			}
		}

		private DelDecideCategoryAreaBtn _delDecideCategoryAreaBtn;

		[SerializeField]
		private Generics.NextIndexInfos _clsNextInfos;

		private int _nIndex;

		private bool _isFocus;

		private bool _isDecide;

		private Fairy _clsFairy;

		private List<UIButton> _listBtns;

		private FurnitureKinds _iKind;

		public Generics.NextIndexInfos nextIndexInfos => _clsNextInfos;

		public int index => _nIndex;

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
					if (!_isFocus)
					{
						setBtnState(isFocus: true);
						_clsFairy.ShowYousei();
						_isFocus = true;
					}
				}
				else if (_isFocus)
				{
					setBtnState(isFocus: false);
					_clsFairy.HideYousei();
					_isFocus = false;
				}
			}
		}

		public bool isDecide
		{
			get
			{
				return _isDecide;
			}
			set
			{
				_isDecide = value;
			}
		}

		public FurnitureKinds kind => _iKind;

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

		private void Awake()
		{
			if (_clsNextInfos == null)
			{
				_clsNextInfos = new Generics.NextIndexInfos();
			}
			_isFocus = false;
			_isDecide = false;
			_listBtns = new List<UIButton>();
			_listBtns.AddRange(GetComponents<UIButton>());
			_listBtns[0].onClick = Util.CreateEventDelegateList(this, "decideCategoryAreaBtn", null);
			_clsFairy = new Fairy(base.transform, "Fairy");
		}

		private void OnDestroy()
		{
			_delDecideCategoryAreaBtn = null;
			_listBtns.Clear();
			_clsFairy.UnInit();
		}

		private bool Update()
		{
			if (_isFocus)
			{
				if (_listBtns[0].state == UIButtonColor.State.Normal)
				{
					setBtnState(isFocus: true);
				}
				if (_clsFairy != null)
				{
					_clsFairy.Update(_isDecide);
				}
			}
			return true;
		}

		public bool Init(int nIndex, FurnitureKinds iKind, DelDecideCategoryAreaBtn decideEvent)
		{
			_nIndex = nIndex;
			_iKind = iKind;
			_delDecideCategoryAreaBtn = decideEvent;
			return true;
		}

		public bool Reset()
		{
			_isFocus = false;
			_isDecide = false;
			_clsFairy.Reset();
			return true;
		}

		public int GetNextIndex(KeyControl.KeyName iName, int defVal)
		{
			return _clsNextInfos.GetIndex(iName, defVal);
		}

		private void setBtnState(bool isFocus)
		{
			UIButtonColor.State state = isFocus ? UIButtonColor.State.Pressed : UIButtonColor.State.Normal;
			foreach (UIButton listBtn in _listBtns)
			{
				listBtn.state = state;
			}
		}

		private void decideCategoryAreaBtn()
		{
			if (_delDecideCategoryAreaBtn != null)
			{
				_delDecideCategoryAreaBtn(this);
			}
		}
	}
}
