using Common.Enum;
using KCV.SortieBattle;
using local.models;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.SortieMap
{
	[RequireComponent(typeof(UIPanel))]
	public class CtrlSortieResult : InstantiateObject<CtrlSortieResult>
	{
		[Serializable]
		private class GetItemInfo : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UITexture _uiBackground;

			[SerializeField]
			private UILabel _uiLabel;

			[SerializeField]
			private UIGrid _uiItemAnchor;

			[SerializeField]
			private UIAtlas _uiAtlas;

			private List<UISprite> _listItemIcon;

			public Transform transform => _tra;

			public bool Init(List<MapEventItemModel> items)
			{
				transform.localScaleY(0f);
				CreateGetItemIcon(items);
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _uiBackground);
				Mem.Del(ref _uiLabel);
				Mem.Del(ref _uiAtlas);
			}

			private void CreateGetItemIcon(List<MapEventItemModel> items)
			{
				_listItemIcon = new List<UISprite>();
				if (items != null && items.Count > 0)
				{
					int cnt = 0;
					items.ForEach(delegate(MapEventItemModel x)
					{
						GameObject gameObject = new GameObject($"GetItem{cnt}");
						gameObject.transform.parent = _uiItemAnchor.transform;
						gameObject.transform.localPositionZero();
						_listItemIcon.Add(gameObject.AddComponent<UISprite>());
						_listItemIcon[cnt].atlas = _uiAtlas;
						_listItemIcon[cnt].spriteName = $"item_{GetItemNum(x)}";
						_listItemIcon[cnt].MakePixelPerfect();
						_listItemIcon[cnt].depth = 1;
						_listItemIcon[cnt].alpha = 0f;
						_listItemIcon[cnt].transform.localScale = Vector3.one * 0.8f;
						cnt++;
					});
					_uiItemAnchor.Reposition();
				}
			}

			public IEnumerator Show(UniRx.IObserver<bool> observer)
			{
				_tra.LTScale(Vector3.one, 0.3f).setEase(LeanTweenType.easeOutCubic);
				yield return new WaitForSeconds(0.75f);
				_listItemIcon.ForEach(delegate(UISprite x)
				{
                    throw new NotImplementedException("‚È‚É‚±‚ê");
                    // _003CShow_003Ec__Iterator117 _003CShow_003Ec__Iterator = this;
					//x.transform.LTValue(0f, 1f, 0.3f).setDelay((float)base._003Ccnt_003E__0 * 0.15f).setEase(LeanTweenType.linear)
					//	.setOnUpdate(delegate(float a)
					//	{
					//		x.alpha = a;
					//	});
					//base._003Ccnt_003E__0++;
				});
				yield return new WaitForSeconds(1.5f);
				observer.OnNext(value: true);
				observer.OnCompleted();
			}

			public IEnumerator Hide(UniRx.IObserver<bool> observer)
			{
				_tra.LTScaleY(0f, 0.3f).setEase(LeanTweenType.easeOutCubic);
				yield return new WaitForSeconds(0.3f);
				observer.OnNext(value: true);
				observer.OnCompleted();
			}

			private string GetItemNum(MapEventItemModel model)
			{
				string result = string.Empty;
				if (model.IsMaterial())
				{
					switch (model.MaterialCategory)
					{
					case enumMaterialCategory.Fuel:
						result = "31";
						break;
					case enumMaterialCategory.Bull:
						result = "32";
						break;
					case enumMaterialCategory.Steel:
						result = "33";
						break;
					case enumMaterialCategory.Bauxite:
						result = "34";
						break;
					case enumMaterialCategory.Build_Kit:
						result = "2";
						break;
					case enumMaterialCategory.Dev_Kit:
						result = "3";
						break;
					case enumMaterialCategory.Repair_Kit:
						result = "1";
						break;
					case enumMaterialCategory.Revamp_Kit:
						result = "4";
						break;
					}
				}
				else if (model.IsUseItem())
				{
					result = model.ItemID.ToString();
				}
				return result;
			}
		}

		[SerializeField]
		private UIButton _uiGearButton;

		[SerializeField]
		private GetItemInfo _clsGetItemInfo;

		private UIPanel _uiPanel;

		private Action _actOnDecide;

		private bool _isInputPossible;

		private UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static CtrlSortieResult Instantiate(CtrlSortieResult prefab, Transform parent, List<MapEventItemModel> items, Action onDecide)
		{
			CtrlSortieResult ctrlSortieResult = InstantiateObject<CtrlSortieResult>.Instantiate(prefab, parent);
			ctrlSortieResult.Init(items, onDecide);
			return ctrlSortieResult;
		}

		private void OnDestroy()
		{
		}

		private bool Init(List<MapEventItemModel> items, Action onDecide)
		{
			_actOnDecide = onDecide;
			_isInputPossible = false;
			_uiGearButton.GetComponent<BoxCollider2D>().enabled = false;
			_uiGearButton.onClick = Util.CreateEventDelegateList(this, "OnDecide", null);
			_clsGetItemInfo.Init(items);
			Observable.FromCoroutine((UniRx.IObserver<bool> observer) => _clsGetItemInfo.Show(observer)).Subscribe(delegate
			{
				_isInputPossible = true;
				_uiGearButton.GetComponent<BoxCollider2D>().enabled = true;
			});
			return true;
		}

		public bool Run()
		{
			KeyControl keyControl = SortieBattleTaskManager.GetKeyControl();
			if (!_isInputPossible)
			{
				return false;
			}
			if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				OnDecide();
				return true;
			}
			return false;
		}

		private void OnDecide()
		{
			_isInputPossible = false;
			_uiGearButton.GetComponent<BoxCollider2D>().enabled = false;
			_uiGearButton.transform.LTValue(1f, 0f, 0.3f).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				_uiGearButton.GetComponent<UISprite>().alpha = x;
			});
			Observable.FromCoroutine((UniRx.IObserver<bool> observer) => _clsGetItemInfo.Hide(observer)).Subscribe(delegate
			{
				Dlg.Call(ref _actOnDecide);
			});
		}
	}
}
