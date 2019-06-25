using System;
using UnityEngine;

namespace KCV.Generic
{
	[Serializable]
	public class UIStatus
	{
		protected Transform _tra;

		protected UILabel _uiVal;

		protected UISprite _uiLabel;

		public virtual Transform transform
		{
			get
			{
				return _tra;
			}
			set
			{
				_tra = value;
			}
		}

		public virtual UILabel valueLabel
		{
			get
			{
				return _uiVal;
			}
			set
			{
				_uiVal = value;
			}
		}

		public virtual UISprite labelSprite
		{
			get
			{
				return _uiLabel;
			}
			set
			{
				_uiLabel = value;
			}
		}

		public UIStatus(Transform parent, string objName)
		{
			Util.FindParentToChild(ref _tra, parent, objName);
			Util.FindParentToChild(ref _uiVal, _tra, "Val");
			Util.FindParentToChild(ref _uiLabel, _tra, "Label");
		}

		public virtual bool UnInit()
		{
			_tra = null;
			_uiLabel = null;
			_uiVal = null;
			return true;
		}
	}
}
