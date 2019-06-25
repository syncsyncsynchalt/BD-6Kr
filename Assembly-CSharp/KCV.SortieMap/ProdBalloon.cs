using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UISprite))]
	public class ProdBalloon : AbsBalloon
	{
		[Serializable]
		private class Item
		{
			[SerializeField]
			private UIWidget _uiWidget;

			[SerializeField]
			private UISprite _uiItemIcon;

			[SerializeField]
			private UILabel _uiLabel;

			public Transform transform => _uiWidget.transform;

			public int depth
			{
				set
				{
					_uiItemIcon.depth = value;
					_uiLabel.depth = value + 1;
				}
			}

			public UISprite itemIcon => _uiItemIcon;

			public UIWidget widget => _uiWidget;

			public bool Init(MapEventItemModel itemModel)
			{
				string spriteName = string.Empty;
				if (itemModel.IsMaterial())
				{
					switch (itemModel.MaterialCategory)
					{
					case enumMaterialCategory.Fuel:
						spriteName = "icon_item1";
						break;
					case enumMaterialCategory.Bull:
						spriteName = "icon_item2";
						break;
					case enumMaterialCategory.Steel:
						spriteName = "icon_item3";
						break;
					case enumMaterialCategory.Bauxite:
						spriteName = "icon_item4";
						break;
					case enumMaterialCategory.Repair_Kit:
						spriteName = "icon_item6";
						break;
					case enumMaterialCategory.Dev_Kit:
						spriteName = "icon_item7";
						break;
					case enumMaterialCategory.Build_Kit:
						spriteName = "icon_item8";
						break;
					}
					_uiItemIcon.spriteName = spriteName;
					_uiItemIcon.MakePixelPerfect();
					_uiLabel.text = $"×{itemModel.Count}";
					return true;
				}
				_uiItemIcon.spriteName = spriteName;
				return false;
			}

			public bool UnInit()
			{
				Mem.Del(ref _uiWidget);
				Mem.Del(ref _uiItemIcon);
				Mem.Del(ref _uiLabel);
				return true;
			}
		}

		[Serializable]
		private class AirRecResult
		{
			[SerializeField]
			private UIWidget _uiWidget;

			[SerializeField]
			private Item _clsItem;

			[SerializeField]
			private UISprite _uiGetLabel;

			[SerializeField]
			private UISprite _uiResultLabel;

			public Transform transform => _uiWidget.transform;

			public int depth
			{
				set
				{
					_uiResultLabel.depth = value;
					_uiGetLabel.depth = value;
					_clsItem.depth = value;
				}
			}

			public bool Init(MissionResultKinds iKind, MapEventItemModel itemModel)
			{
				SetSprite(iKind);
				SetItem(iKind, itemModel);
				return true;
			}

			public bool UnInit()
			{
				Mem.Del(ref _uiWidget);
				_clsItem.UnInit();
				Mem.Del(ref _clsItem);
				Mem.Del(ref _uiGetLabel);
				Mem.Del(ref _uiResultLabel);
				return true;
			}

			private void SetItem(MissionResultKinds iKind, MapEventItemModel itemModel)
			{
				if (iKind == MissionResultKinds.FAILE)
				{
					_clsItem.widget.alpha = 0f;
					return;
				}
				_clsItem.Init(itemModel);
				_clsItem.itemIcon.transform.localScale = Vector3.one * 0.6f;
				_clsItem.widget.alpha = 1f;
			}

			private void SetSprite(MissionResultKinds iKind)
			{
				switch (iKind)
				{
				case MissionResultKinds.GREAT:
					_uiResultLabel.spriteName = "txt1";
					_uiResultLabel.MakePixelPerfect();
					break;
				case MissionResultKinds.SUCCESS:
					_uiResultLabel.spriteName = "txt2";
					_uiResultLabel.MakePixelPerfect();
					break;
				case MissionResultKinds.FAILE:
					_uiResultLabel.spriteName = "txt4";
					_uiResultLabel.MakePixelPerfect();
					break;
				}
				Vector3 localPosition = (iKind != 0) ? (Vector3.up * 26f) : Vector3.zero;
				_uiResultLabel.transform.localPosition = localPosition;
				_uiGetLabel.alpha = ((iKind != 0) ? 1f : 0f);
			}
		}

		[SerializeField]
		private UILabel _uiText;

		[SerializeField]
		private Item _uiItem;

		[SerializeField]
		private AirRecResult _clsAirRecResult;

		public int depth
		{
			set
			{
				base.sprite.depth = value;
				_uiText.depth = value + 1;
				_uiItem.depth = value + 1;
				_clsAirRecResult.depth = value + 1;
			}
		}

		public static ProdBalloon Instantiate(ProdBalloon prefab, Transform parent, UISortieShip.Direction iDirection, enumMapEventType iEventType, enumMapWarType iWarType)
		{
			ProdBalloon prodBalloon = UnityEngine.Object.Instantiate(prefab);
			prodBalloon.transform.parent = parent;
			prodBalloon.transform.localPositionZero();
			prodBalloon.transform.localScaleZero();
			prodBalloon.InitText(iDirection, iEventType, iWarType);
			return prodBalloon;
		}

		public static ProdBalloon Instantiate(ProdBalloon prefab, Transform parent, UISortieShip.Direction iDirection, MapEventItemModel itemModel)
		{
			ProdBalloon prodBalloon = UnityEngine.Object.Instantiate(prefab);
			prodBalloon.transform.parent = parent;
			prodBalloon.transform.localPositionZero();
			prodBalloon.transform.localScaleZero();
			prodBalloon.InitPortBackEo(iDirection, itemModel);
			return prodBalloon;
		}

		public static ProdBalloon Instantiate(ProdBalloon prefab, Transform parent, UISortieShip.Direction iDirection, MapEventAirReconnaissanceModel eventAirRecModel, MapEventItemModel eventItemModel)
		{
			ProdBalloon prodBalloon = UnityEngine.Object.Instantiate(prefab);
			prodBalloon.transform.parent = parent;
			prodBalloon.transform.localPositionZero();
			prodBalloon.transform.localScaleZero();
			prodBalloon.InitAirRec(iDirection, eventAirRecModel, eventItemModel);
			return prodBalloon;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiText);
			_uiItem.UnInit();
			Mem.Del(ref _uiItem);
			_clsAirRecResult.UnInit();
			Mem.Del(ref _clsAirRecResult);
		}

		private bool InitText(UISortieShip.Direction iDirection, enumMapEventType iEventType, enumMapWarType iWarType)
		{
			_uiItem.transform.localScaleZero();
			_clsAirRecResult.transform.localScaleZero();
			_uiText.transform.localScaleOne();
			SetBalloonPos(iDirection);
			SetText(iEventType, iWarType);
			return true;
		}

		private void SetText(enumMapEventType iEventType, enumMapWarType iWarType)
		{
			string text = string.Empty;
			if (iEventType == enumMapEventType.Stupid && iWarType == enumMapWarType.Midnight)
			{
				text = "艦隊針路\n選択可能!";
			}
			_uiText.text = text;
		}

		private bool InitPortBackEo(UISortieShip.Direction iDirection, MapEventItemModel itemModel)
		{
			_uiItem.transform.localScaleOne();
			_uiItem.widget.alpha = 1f;
			_clsAirRecResult.transform.localScaleZero();
			_uiText.transform.localScaleZero();
			SetBalloonPos(iDirection);
			SetGetItem(itemModel);
			return true;
		}

		private void SetGetItem(MapEventItemModel itemModel)
		{
			_uiItem.Init(itemModel);
		}

		private bool InitAirRec(UISortieShip.Direction iDirection, MapEventAirReconnaissanceModel eventAirRecModel, MapEventItemModel eventItemModel)
		{
			_uiItem.transform.localScaleZero();
			_uiText.transform.localScaleZero();
			SetBalloonPos(iDirection);
			_clsAirRecResult.Init(eventAirRecModel.SearchResult, eventItemModel);
			_clsAirRecResult.transform.localScaleOne();
			return true;
		}

		protected override void SetBalloonPos(UISortieShip.Direction iDirection)
		{
			switch (iDirection)
			{
			case UISortieShip.Direction.Left:
				base.transform.localPosition = new Vector3(71f, 17f, 0f);
				base.sprite.flip = UIBasicSprite.Flip.Horizontally;
				break;
			case UISortieShip.Direction.Right:
				base.transform.localPosition = new Vector3(-71f, 17f, 0f);
				base.sprite.flip = UIBasicSprite.Flip.Horizontally;
				break;
			}
		}
	}
}
