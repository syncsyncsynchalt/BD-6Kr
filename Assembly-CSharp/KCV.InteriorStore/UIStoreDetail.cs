using local.models;
using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIStoreDetail : BaseUIFurnitureDetail
	{
		[Serializable]
		protected new class Preview : BaseUIFurnitureDetail.Preview
		{
			public Preview(Transform parent, string objName)
				: base(parent, objName)
			{
				Util.FindParentToChild(ref _traObj, parent, objName);
				Util.FindParentToChild(ref _uiMaskPanel, _traObj, "Mask");
				Util.FindParentToChild(ref _uiFurnitureTex, _uiMaskPanel.transform, "Offs/FurnitureTex");
				Util.FindParentToChild(ref _uiWorker, _uiMaskPanel.transform, "Worker");
				_uiWorker.SetActive(isActive: false);
				_uiStars = new UISprite[BaseUIFurnitureDetail.RARE_STAR_MAX];
				for (int i = 0; i < _uiStars.Length; i++)
				{
					Util.FindParentToChild(ref _uiStars[i], _uiMaskPanel.transform.FindChild("RareStars").transform, $"Star{i + 1}");
					_uiStars[i].spriteName = "icon_star_bg";
				}
			}

			public new void SetFurniture(FurnitureModel model)
			{
				_setFurnitureTex(model);
				bool isActive = model.IsNeedWorker() ? true : false;
				_uiWorker.SetActive(isActive);
				_setRare(model);
			}

			private void _setRare(FurnitureModel model)
			{
				for (int i = 0; i < _uiStars.Length; i++)
				{
					if (i < model.Rarity)
					{
						_uiStars[i].spriteName = "icon_star";
					}
					else
					{
						_uiStars[i].spriteName = "icon_star_bg";
					}
				}
			}
		}

		[Serializable]
		private class FCoinInfo
		{
			private Transform _traObj;

			private UISprite _uiBg;

			private UISprite _uiLabel;

			private UILabel _uiVal;

			private UILabel _uiSoldOut;

			public FCoinInfo(Transform parent, string objName)
			{
				Util.FindParentToChild(ref _traObj, parent, objName);
				Util.FindParentToChild(ref _uiBg, _traObj, "BG");
				Util.FindParentToChild(ref _uiLabel, _traObj, "Label");
				Util.FindParentToChild(ref _uiVal, _traObj, "Val");
				Util.FindParentToChild(ref _uiSoldOut, _traObj, "SoldOut");
			}

			public void SetFCoinInfo(FurnitureModel model)
			{
				_uiVal.textInt = model.Price;
				_uiVal.SetActive(!model.IsPossession());
				_uiSoldOut.SetActive(model.IsPossession());
			}
		}

		private Preview _uiPreviwe;

		private FCoinInfo _uiFCoinInfo;

		private FurnitureModel _clsFStoreItemModel;

		public FurnitureModel DetailItem => _clsFStoreItemModel;

		protected override void Awake()
		{
			base.Awake();
			_uiPreviwe = new Preview(base.transform, "Preview");
			_uiFCoinInfo = new FCoinInfo(base.transform, "FCoinInfo");
		}

		public new void SetDetail(FurnitureModel model)
		{
			base.SetDetail(model);
			_uiPreviwe.SetFurniture(model);
			_uiFCoinInfo.SetFCoinInfo(model);
			_clsFStoreItemModel = model;
		}

		public void Clear()
		{
			_clsFStoreItemModel = null;
		}
	}
}
