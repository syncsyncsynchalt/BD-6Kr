using KCV.Battle.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdShellingSlot : BaseProdLine
	{
		[Serializable]
		private class Slot : IDisposable
		{
			[SerializeField]
			private Transform _tra;

			[SerializeField]
			private UITexture _uiSlotIcon;

			[SerializeField]
			private UILabel _uiSlotName;

			private bool _isPlane;

			public UIWidget.Pivot slotNamePivot
			{
				get
				{
					return _uiSlotName.pivot;
				}
				set
				{
					_uiSlotName.pivot = value;
				}
			}

			public UILabel slotName => _uiSlotName;

			public bool isSlotIconActive
			{
				get
				{
					return _uiSlotIcon.gameObject.activeSelf;
				}
				set
				{
					_uiSlotIcon.SetActive(value);
				}
			}

			public Vector3 slotIconScale
			{
				get
				{
					return _uiSlotIcon.transform.localScale;
				}
				set
				{
					_uiSlotIcon.transform.localScale = value;
				}
			}

			public Transform transform
			{
				get
				{
					return _tra;
				}
				set
				{
					if (_tra != value)
					{
						_tra = value;
					}
				}
			}

			public UITexture slotIcon => _uiSlotIcon;

			public int depth
			{
				get
				{
					return _uiSlotIcon.depth;
				}
				set
				{
					_uiSlotIcon.depth = value;
					_uiSlotName.depth = value + 1;
				}
			}

			public Slot(Transform parent, string objName)
			{
				Util.FindParentToChild(ref _tra, parent, objName);
				Util.FindParentToChild(ref _uiSlotIcon, _tra, "SlotIcon");
				Util.FindParentToChild(ref _uiSlotName, _tra, "SlotName");
				_uiSlotIcon.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE[4];
				_uiSlotIcon.transform.localPosition = Vector3.up * 120f;
			}

			public bool Init()
			{
				_uiSlotIcon.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE[4];
				_uiSlotIcon.transform.localPosition = Vector3.up * 120f;
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _tra);
				Mem.Del(ref _uiSlotIcon);
				Mem.Del(ref _uiSlotName);
				Mem.Del(ref _isPlane);
			}

			public bool SetSlotItem(SlotitemModel_Battle model)
			{
				return SetSlotItem(model, isSlotIconActive: true);
			}

			public bool SetSlotItem(SlotitemModel_Battle model, bool isSlotIconActive)
			{
				bool flag = model?.IsPlane() ?? false;
				_isPlane = flag;
				_uiSlotIcon.mainTexture = ((model == null) ? null : ((!flag) ? SlotItemUtils.LoadTexture(model) : SlotItemUtils.LoadUniDirTexture(model)));
				_uiSlotIcon.localSize = ((!flag) ? ResourceManager.SLOTITEM_TEXTURE_SIZE[4] : ResourceManager.SLOTITEM_TEXTURE_SIZE[7]);
				_uiSlotName.text = ((model != null && !flag) ? model.Name : string.Empty);
				this.isSlotIconActive = isSlotIconActive;
				return true;
			}

			public void SetFlipHorizontal(bool isNotFlipHorizontal)
			{
				_uiSlotName.transform.localRotation = ((!isNotFlipHorizontal) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity);
				_uiSlotIcon.transform.localRotation = ((!_isPlane) ? Quaternion.identity : ((!isNotFlipHorizontal) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity));
			}
		}

		[Serializable]
		private class GlowAircraftIcon : IDisposable
		{
			[SerializeField]
			private Transform _transform;

			private UITexture _uiTexture;

			private MoveWith _clsMoveWith;

			public Transform transform => _transform;

			public UITexture uiTexture
			{
				get
				{
					if (_uiTexture == null)
					{
						_uiTexture = ((Component)transform).GetComponent<UITexture>();
					}
					return _uiTexture;
				}
			}

			public MoveWith moveWith
			{
				get
				{
					if (_clsMoveWith == null)
					{
						_clsMoveWith = ((Component)transform).GetComponent<MoveWith>();
					}
					return _clsMoveWith;
				}
			}

			public bool isActive
			{
				get
				{
					return transform.gameObject.activeInHierarchy;
				}
				set
				{
					transform.SetActive(value);
				}
			}

			public bool Init(SlotitemModel_Battle model)
			{
				if (model == null || !model.IsPlane())
				{
					return false;
				}
				uiTexture.mainTexture = SlotItemUtils.LoadUniDirGlowTexture(model);
				uiTexture.localSize = ResourceManager.SLOTITEM_TEXTURE_SIZE[7];
				return true;
			}

			public bool Init(SlotitemModel_Battle model, bool isFlip)
			{
				if (!Init(model))
				{
					return false;
				}
				uiTexture.flip = (isFlip ? UIBasicSprite.Flip.Horizontally : UIBasicSprite.Flip.Nothing);
				return true;
			}

			public void Dispose()
			{
				Mem.Del(ref _transform);
				Mem.Del(ref _uiTexture);
				Mem.Del(ref _clsMoveWith);
			}
		}

		[SerializeField]
		private Transform _slotIconGrow;

		[SerializeField]
		private NoiseMove _clsNoiseMove;

		[SerializeField]
		private GlowAircraftIcon _clsGlowIcon;

		[SerializeField]
		private List<UITexture> _listOverlay;

		[SerializeField]
		private UILabel _uiFlashText;

		[SerializeField]
		private List<Slot> _listSlots;

		private UIPanel _uiPanel;

		private Dictionary<AnimationName, Vector3> _dicSlotSize;

		private bool _isFriend;

		public UIPanel panel
		{
			get
			{
				if (_uiPanel == null)
				{
					_uiPanel = GetComponent<UIPanel>();
				}
				return _uiPanel;
			}
		}

		public bool isFinished => _isFinished;

		public bool isNotFlipHorizontal
		{
			set
			{
				base.transform.rotation = ((!value) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity);
				_listSlots.ForEach(delegate(Slot x)
				{
					x.SetFlipHorizontal(value);
				});
			}
		}

		public AnimationName slotNamePivot
		{
			set
			{
				switch (value)
				{
				case AnimationName.ProdLine:
					_listSlots[0].slotNamePivot = UIWidget.Pivot.Left;
					_listSlots[0].slotName.transform.localPositionZero();
					break;
				case AnimationName.ProdNormalAttackLine:
					_listSlots[0].slotNamePivot = ((!_isFriend) ? UIWidget.Pivot.Left : UIWidget.Pivot.Right);
					_listSlots[0].slotName.transform.localPosition = Vector3.right * 375f;
					break;
				case AnimationName.ProdTripleLine:
				case AnimationName.ProdSuccessiveLine:
					_listSlots.ForEach(delegate(Slot x)
					{
						x.slotNamePivot = UIWidget.Pivot.Center;
					});
					_listSlots[0].slotName.transform.localPositionZero();
					break;
				}
			}
		}

		public AnimationName slotIconActive
		{
			set
			{
				_listSlots.ForEach(delegate(Slot x)
				{
					if (value == AnimationName.ProdLine)
					{
						x.isSlotIconActive = true;
					}
					else if (value == AnimationName.ProdNormalAttackLine || value == AnimationName.ProdAircraftAttackLine)
					{
						x.isSlotIconActive = true;
					}
					else
					{
						x.isSlotIconActive = false;
					}
				});
			}
		}

		private AnimationName depth
		{
			set
			{
				switch (value)
				{
				case AnimationName.ProdSuccessiveLine:
					break;
				case AnimationName.ProdLine:
				case AnimationName.ProdTripleLine:
				{
					int j;
					int i = j = 0;
					_listOverlay.ForEach(delegate(UITexture x)
					{
						x.depth = i * 10;
						i++;
					});
					_listSlots.ForEach(delegate(Slot x)
					{
						x.depth = j * 10 + 1;
						j++;
					});
					break;
				}
				case AnimationName.ProdNormalAttackLine:
				{
					int l;
					int k = l = 0;
					_listOverlay.ForEach(delegate(UITexture x)
					{
						x.depth = k * 10;
						k++;
					});
					_listSlots.ForEach(delegate(Slot x)
					{
						x.depth = l * 10 + 100;
						l++;
					});
					break;
				}
				}
			}
		}

		public static ProdShellingSlot Instantiate(ProdShellingSlot prefab, Transform parent)
		{
			ProdShellingSlot prodShellingSlot = UnityEngine.Object.Instantiate(prefab);
			prodShellingSlot.transform.parent = parent;
			prodShellingSlot.transform.localScale = Vector3.one;
			prodShellingSlot.transform.localPosition = Vector3.zero;
			return prodShellingSlot;
		}

		private void Awake()
		{
			_isFinished = false;
			_listSlots.ForEach(delegate(Slot x)
			{
				x.Init();
				x.SetSlotItem(null, isSlotIconActive: false);
			});
			_listOverlay.ForEach(delegate(UITexture x)
			{
				x.transform.localScaleZero();
			});
			_dicSlotSize = new Dictionary<AnimationName, Vector3>();
			_dicSlotSize.Add(AnimationName.ProdLine, Vector3.one * 0.65f);
			_clsGlowIcon.isActive = false;
			panel.widgetsAreStatic = true;
			base.transform.localScaleZero();
		}

		private new void OnDestroy()
		{
			Mem.Del(ref _slotIconGrow);
			Mem.Del(ref _clsNoiseMove);
			Mem.DelIDisposableSafe(ref _clsGlowIcon);
			Mem.DelListSafe(ref _listOverlay);
			Mem.Del(ref _uiFlashText);
			if (_listSlots != null)
			{
				_listSlots.ForEach(delegate(Slot x)
				{
					x.Dispose();
				});
			}
			Mem.DelListSafe(ref _listSlots);
			Mem.Del(ref _uiPanel);
			Mem.DelDictionarySafe(ref _dicSlotSize);
			Mem.Del(ref _isFriend);
		}

		public void SetSlotData(SlotitemModel_Battle model)
		{
			_listSlots[0].SetSlotItem(model);
			_clsGlowIcon.Init(model);
		}

		public void SetSlotData(SlotitemModel_Battle[] models)
		{
			int num = 0;
			foreach (SlotitemModel_Battle slotItem in models)
			{
				if (_listSlots[num] != null)
				{
					_listSlots[num].SetSlotItem(slotItem);
					num++;
				}
			}
		}

		public void SetSlotData(SlotitemModel_Battle[] models, ProdTranscendenceCutIn.AnimationList iList)
		{
			switch (iList)
			{
			case ProdTranscendenceCutIn.AnimationList.ProdTATorpedox2:
			{
				int num2 = 0;
				foreach (Slot listSlot in _listSlots)
				{
					if (num2 == 0)
					{
						listSlot.SetSlotItem(models[0]);
					}
					else if (num2 > 0)
					{
						listSlot.SetSlotItem(models[1]);
					}
					num2++;
				}
				break;
			}
			case ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryNTorpedo:
			{
				int num3 = 0;
				foreach (SlotitemModel_Battle slotItem2 in models)
				{
					if (_listSlots[num3] != null)
					{
						_listSlots[num3].SetSlotItem(slotItem2);
						num3++;
					}
				}
				break;
			}
			case ProdTranscendenceCutIn.AnimationList.ProdTAMainBatteryx3:
			{
				int num = 0;
				foreach (SlotitemModel_Battle slotItem in models)
				{
					if (_listSlots[num] != null)
					{
						_listSlots[num].SetSlotItem(slotItem);
						num++;
					}
				}
				break;
			}
			}
		}

		public void Play(AnimationName iName, bool isNotFlipHorizontal, Action callback)
		{
			_isFriend = isNotFlipHorizontal;
			panel.widgetsAreStatic = false;
			_listOverlay.ForEach(delegate(UITexture x)
			{
				Color color = (!_isFriend) ? new Color(1f, 0f, 0f, x.alpha) : new Color(0f, 51f / 160f, 1f, x.alpha);
				x.transform.localScaleOne();
				x.color = color;
			});
			this.isNotFlipHorizontal = isNotFlipHorizontal;
			slotIconActive = iName;
			depth = iName;
			ResetRotation();
			SetFlashText(iName);
			_clsGlowIcon.isActive = (iName == AnimationName.ProdAircraftAttackLine);
			_clsNoiseMove.enabled = (iName == AnimationName.ProdAircraftAttackLine);
			SetSlotIconPos(iName);
			slotNamePivot = iName;
			base.transform.localScaleOne();
			base.Play(iName, callback);
		}

		private void SetFlashText(AnimationName iName)
		{
			if (iName == AnimationName.ProdNormalAttackLine)
			{
				_uiFlashText.SetActive(isActive: true);
				_uiFlashText.pivot = UIWidget.Pivot.Center;
				Vector2 localSize = _listSlots[0].slotName.localSize;
				float num = localSize.x / 2f;
				_uiFlashText.transform.localPosition = new Vector3(num * (float)((!_isFriend) ? 1 : (-1)), 0f, 0f);
				_uiFlashText.text = _listSlots[0].slotName.text;
			}
			else
			{
				_uiFlashText.SetActive(isActive: false);
			}
		}

		private void SetSlotIconPos(AnimationName iName)
		{
			Vector3 localPosition = Vector3.zero;
			switch (iName)
			{
			case AnimationName.ProdNormalAttackLine:
				localPosition = new Vector3(138f * (float)((!_isFriend) ? 1 : (-1)), 194f, 0f);
				break;
			case AnimationName.ProdAircraftAttackLine:
				localPosition = Vector3.zero;
				break;
			default:
				localPosition = Vector3.up * 30f;
				break;
			}
			_listSlots[0].slotIcon.transform.localPosition = localPosition;
		}

		private void ResetRotation()
		{
			foreach (Slot listSlot in _listSlots)
			{
				listSlot.transform.localRotation = Quaternion.identity;
			}
			foreach (UITexture item in _listOverlay)
			{
				item.transform.localRotation = Quaternion.identity;
			}
		}

		protected override void onFinished()
		{
			base.onFinished();
			base.transform.localScaleZero();
			foreach (UITexture item in _listOverlay)
			{
				item.transform.localScale = Vector3.zero;
			}
			_listSlots.ForEach(delegate(Slot x)
			{
				x.slotName.text = string.Empty;
			});
			panel.widgetsAreStatic = true;
		}
	}
}
