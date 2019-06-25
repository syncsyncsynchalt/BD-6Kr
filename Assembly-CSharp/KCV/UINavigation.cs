using local.models;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	[RequireComponent(typeof(UIPanel))]
	public class UINavigation<T> : MonoBehaviour where T : UINavigation<T>
	{
		public enum Anchor
		{
			TopLeft,
			TopRight,
			BottomLeft,
			BottomRight
		}

		[SerializeField]
		protected UISprite _uiBackground;

		[SerializeField]
		protected UIHowTo _uiHowTo;

		private Anchor _iAnchor;

		private UIPanel _uiPanel;

		private SettingModel _clsSettingModel;

		private List<UIHowToDetail> _listDetails;

		public virtual Anchor anchor => _iAnchor;

		public virtual UIPanel panel => this.GetComponentThis(ref _uiPanel);

		protected virtual SettingModel settingModel
		{
			get
			{
				if (_clsSettingModel == null)
				{
					_clsSettingModel = new SettingModel();
				}
				return _clsSettingModel;
			}
		}

		protected virtual List<UIHowToDetail> details
		{
			get
			{
				if (_listDetails == null)
				{
					_listDetails = new List<UIHowToDetail>();
				}
				return _listDetails;
			}
		}

		protected void OnDestroy()
		{
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiHowTo);
			Mem.Del(ref _uiPanel);
			Mem.Del(ref _clsSettingModel);
			Mem.DelListSafe(ref _listDetails);
			OnUnInit();
		}

		protected virtual T VirtualCtor(SettingModel model)
		{
			_clsSettingModel = model;
			_listDetails = new List<UIHowToDetail>();
			return (T)this;
		}

		protected virtual void OnUnInit()
		{
		}

		public virtual T SetNavigation(List<UIHowToDetail> details)
		{
			_listDetails = details;
			DrawRefresh();
			return (T)this;
		}

		public virtual T SetAnchor(Anchor iAnchor)
		{
			if (_iAnchor == iAnchor)
			{
				return (T)this;
			}
			switch (iAnchor)
			{
			case Anchor.TopLeft:
				_uiHowTo.SetHorizontalAlign(UIHowToHorizontalAlign.left);
				_uiBackground.flip = UIBasicSprite.Flip.Horizontally;
				_uiBackground.pivot = UIWidget.Pivot.Left;
				_uiBackground.transform.localPositionZero();
				base.transform.localPosition = new Vector3(-480f, 256f, 0f);
				break;
			case Anchor.TopRight:
				_uiHowTo.SetHorizontalAlign(UIHowToHorizontalAlign.right);
				_uiBackground.flip = UIBasicSprite.Flip.Nothing;
				_uiBackground.pivot = UIWidget.Pivot.Right;
				_uiBackground.transform.localPositionZero();
				base.transform.localPosition = new Vector3(480f, 256f, 0f);
				break;
			case Anchor.BottomLeft:
				_uiHowTo.SetHorizontalAlign(UIHowToHorizontalAlign.left);
				_uiBackground.flip = UIBasicSprite.Flip.Horizontally;
				_uiBackground.pivot = UIWidget.Pivot.Left;
				_uiBackground.transform.localPositionZero();
				base.transform.localPosition = new Vector3(-480f, -256f, 0f);
				break;
			case Anchor.BottomRight:
				_uiHowTo.SetHorizontalAlign(UIHowToHorizontalAlign.right);
				_uiBackground.flip = UIBasicSprite.Flip.Nothing;
				_uiBackground.pivot = UIWidget.Pivot.Right;
				_uiBackground.transform.localPositionZero();
				base.transform.localPosition = new Vector3(480f, -256f, 0f);
				break;
			}
			_iAnchor = iAnchor;
			return (T)this;
		}

		protected virtual void AddDetail(HowToKey iKey, string strDescription)
		{
			_listDetails.Add(new UIHowToDetail(iKey, strDescription));
		}

		protected virtual T DrawRefresh()
		{
			_uiHowTo.Refresh(_listDetails.ToArray());
			_listDetails.Clear();
			return (T)this;
		}
	}
}
