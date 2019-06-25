using Common.Enum;
using KCV.Utils;
using local.models;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIWidget))]
	[RequireComponent(typeof(iTweenEvent))]
	public class ProdItem : MonoBehaviour
	{
		[SerializeField]
		private UISprite _uiItemIcon;

		[SerializeField]
		private UILabel _uiCount;

		[SerializeField]
		private Vector3 _vEndPos = new Vector3(-15f, 40f, 0f);

		private UIWidget _uiWidget;

		private iTweenEvent _iteLostItemAnim;

		private UIWidget widget => this.GetComponentThis(ref _uiWidget);

		private iTweenEvent lostItemAnim => this.GetComponentThis(ref _iteLostItemAnim);

		private void OnDestroy()
		{
			Mem.Del(ref _uiItemIcon);
			Mem.Del(ref _uiCount);
			Mem.Del(ref _vEndPos);
			Mem.Del(ref _iteLostItemAnim);
			Mem.Del(ref _uiWidget);
		}

		public static ProdItem Instantiate(ProdItem prefab, Transform parent, MapEventItemModel model)
		{
			ProdItem prodItem = UnityEngine.Object.Instantiate(prefab);
			prodItem.transform.parent = parent;
			prodItem.transform.localScale = Vector3.one * 1.3f;
			prodItem.transform.localPositionZero();
			prodItem.Init(model);
			return prodItem;
		}

		private bool Init(MapEventItemModel model)
		{
			widget.alpha = 0f;
			_uiItemIcon.spriteName = string.Empty;
			_uiCount.text = string.Empty;
			if (model.IsMaterial())
			{
				switch (model.ItemID)
				{
				case 1:
					_uiItemIcon.spriteName = "icon-m1";
					break;
				case 2:
					_uiItemIcon.spriteName = "icon-m2";
					break;
				case 3:
					_uiItemIcon.spriteName = "icon-m3";
					break;
				case 4:
					_uiItemIcon.spriteName = "icon-m4";
					break;
				case 6:
					_uiItemIcon.spriteName = "icon-m5";
					break;
				case 5:
					_uiItemIcon.spriteName = "icon-m10";
					break;
				case 7:
					_uiItemIcon.spriteName = "icon-m8";
					break;
				case 8:
					_uiItemIcon.spriteName = "icon-m16";
					break;
				default:
					_uiItemIcon.spriteName = "icon_find";
					break;
				}
			}
			else if (model.IsUseItem())
			{
				switch (model.ItemID)
				{
				case 10:
					_uiItemIcon.spriteName = "icon-m12";
					break;
				case 11:
					_uiItemIcon.spriteName = "icon-m13";
					break;
				case 12:
					_uiItemIcon.spriteName = "icon-m14";
					break;
				default:
					_uiItemIcon.spriteName = "icon_find";
					break;
				}
			}
			_uiCount.text = model.Count.ToString();
			return true;
		}

		public void PlayGetAnim(Action onFinished)
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_032);
			Observable.Timer(TimeSpan.FromSeconds(1.5)).Subscribe(delegate
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_032);
			});
			float num = 1f;
			widget.alpha = 1f;
			_uiWidget.transform.LTValue(1f, 0f, num * 0.1f).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float x)
			{
				widget.alpha = x;
			})
				.setDelay(num * 0.9f);
			base.transform.LTMoveLocal(_vEndPos, num).setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinished);
				UnityEngine.Object.Destroy(base.transform.gameObject);
			});
		}

		public static ProdItem Instantiate(ProdItem prefab, Transform parent, MapEventHappeningModel model)
		{
			ProdItem prodItem = UnityEngine.Object.Instantiate(prefab);
			prodItem.transform.parent = parent;
			prodItem.transform.localScale = Vector3.one * 1.3f;
			prodItem.transform.localPositionZero();
			prodItem.Init(model);
			return prodItem;
		}

		private bool Init(MapEventHappeningModel model)
		{
			widget.alpha = 0f;
			_uiItemIcon.spriteName = string.Empty;
			_uiCount.text = string.Empty;
			switch (model.Material)
			{
			case enumMaterialCategory.Fuel:
				_uiItemIcon.spriteName = "icon-m1";
				break;
			case enumMaterialCategory.Bull:
				_uiItemIcon.spriteName = "icon-m2";
				break;
			case enumMaterialCategory.Steel:
				_uiItemIcon.spriteName = "icon-m3";
				break;
			case enumMaterialCategory.Bauxite:
				_uiItemIcon.spriteName = "icon-m4";
				break;
			case enumMaterialCategory.Repair_Kit:
				_uiItemIcon.spriteName = "icon-m5";
				break;
			case enumMaterialCategory.Build_Kit:
				_uiItemIcon.spriteName = "icon-m10";
				break;
			case enumMaterialCategory.Dev_Kit:
				_uiItemIcon.spriteName = "icon-m8";
				break;
			case enumMaterialCategory.Revamp_Kit:
				_uiItemIcon.spriteName = "icon-m16";
				break;
			default:
				_uiItemIcon.spriteName = "icon_find";
				break;
			}
			_uiCount.text = model.Count.ToString();
			return true;
		}

		public void PlayLostAnim(Action onFinished)
		{
			widget.transform.LTValue(0f, 1f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				widget.alpha = x;
			});
			Bezier bezier = new Bezier(Bezier.BezierType.Quadratic, Vector3.zero, new Vector3(-75f, -60f, 0f), new Vector3(-45f, 100f, 0f), Vector3.zero);
			base.transform.LTValue(0f, 1f, 2f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				base.transform.localPosition = bezier.Interpolate(x);
			});
			Observable.Timer(TimeSpan.FromSeconds(1.2000000476837158)).Subscribe(delegate
			{
				widget.transform.LTValue(1f, 0f, 0.5f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
				{
					widget.alpha = x;
				})
					.setOnComplete((Action)delegate
					{
						UnityEngine.Object.Destroy(base.gameObject);
					});
				Dlg.Call(ref onFinished);
			});
		}
	}
}
