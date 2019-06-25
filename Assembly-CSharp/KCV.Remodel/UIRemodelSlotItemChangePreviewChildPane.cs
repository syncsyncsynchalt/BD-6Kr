using KCV.Scene.Port;
using local.models;
using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodelSlotItemChangePreviewChildPane : MonoBehaviour
	{
		private Vector3 showPos = new Vector3(36f, 193.5f);

		private Vector3 hidePos = new Vector3(550f, 193.5f);

		[SerializeField]
		private UIRemodelSlotItemChangePreviewInParameter mPrefab_UIRemodelSlotItemChangePreviewInParameter;

		[SerializeField]
		private UITable paramTable;

		[SerializeField]
		private UILabel weaponName;

		[SerializeField]
		private UISprite weaponTypeIcon;

		[SerializeField]
		private UITexture weaponImage;

		[SerializeField]
		private UISprite mLock_Icon;

		public UITexture BackGround;

		private string[] paramLavels = new string[10]
		{
			"装甲",
			"火力",
			"雷装",
			"爆撃",
			"対空",
			"対潜",
			"命中",
			"回避",
			"索敵",
			"射程"
		};

		public void Init4Upper(SlotitemModel dstSlotItem)
		{
			InitViews(dstSlotItem);
			if (dstSlotItem != null)
			{
				processWithoutComparison(dstSlotItem, 0);
			}
		}

		public void Init4Lower(SlotitemModel dstSlotItem, SlotitemModel srcSlotItem)
		{
			InitViews(dstSlotItem);
			if (srcSlotItem == null)
			{
				processWithoutComparison(dstSlotItem, 0);
			}
			else
			{
				processWithComparison(dstSlotItem, srcSlotItem);
			}
		}

		public Texture GetSlotItemTexture()
		{
			if (weaponImage != null && weaponImage.mainTexture != null)
			{
				return weaponImage.mainTexture;
			}
			return null;
		}

		public void InitViews(SlotitemModel dstSlotItem)
		{
			if (dstSlotItem == null)
			{
				SwitchActive(active: false);
				weaponName.text = "\u3000-";
				mLock_Icon.transform.localScale = Vector3.zero;
				return;
			}
			SwitchActive(active: true);
			weaponName.text = dstSlotItem.Name;
			weaponTypeIcon.spriteName = $"icon_slot{dstSlotItem.Type4}";
			UnloadSlotItemTexture();
			weaponImage.mainTexture = BgTextureResourceLoad(dstSlotItem.MstId);
			if (dstSlotItem.IsLocked())
			{
				mLock_Icon.transform.localScale = Vector3.one;
			}
			else
			{
				mLock_Icon.transform.localScale = Vector3.zero;
			}
			paramTable.GetChildList().ForEach(delegate(Transform e)
			{
				NGUITools.Destroy(e);
			});
		}

		private void SwitchActive(bool active)
		{
			weaponTypeIcon.SetActive(active);
			weaponImage.SetActive(active);
			paramTable.SetActive(active);
		}

		private UIRemodelSlotItemChangePreviewInParameter CreateParamObj()
		{
			return Util.Instantiate(mPrefab_UIRemodelSlotItemChangePreviewInParameter.gameObject, paramTable.gameObject).GetComponent<UIRemodelSlotItemChangePreviewInParameter>();
		}

		private void processWithoutComparison(SlotitemModel dstSlotItem, int fixedSabun)
		{
			int num = 0;
			int[] array = createSlotItemValues(dstSlotItem);
			for (int i = 0; i < paramLavels.Length; i++)
			{
				if (array[i] != 0)
				{
					CreateParamObj().Init(paramLavels[i], (array[i] <= 0) ? (-array[i]) : array[i], array[i]);
					num++;
				}
			}
			if (num == 0)
			{
				CreateParamObj().Init(string.Empty, 0, 0);
			}
			paramTable.Reposition();
		}

		private void processWithComparison(SlotitemModel dstSlotItem, SlotitemModel srcSlotItem)
		{
			int num = 0;
			int[] array = createSlotItemValues(dstSlotItem);
			int[] array2 = createSlotItemValues(srcSlotItem);
			for (int i = 0; i < paramLavels.Length; i++)
			{
				if (array[i] != array2[i])
				{
					CreateParamObj().Init(paramLavels[i], (array[i] <= array2[i]) ? (array2[i] - array[i]) : (array[i] - array2[i]), array[i] - array2[i]);
					num++;
				}
			}
			if (num == 0)
			{
				CreateParamObj().Init(string.Empty, 0, 0);
			}
			paramTable.Reposition();
		}

		private int[] createSlotItemValues(SlotitemModel slotItem)
		{
			return new int[10]
			{
				slotItem.Soukou,
				slotItem.Hougeki,
				slotItem.Raigeki,
				slotItem.Bakugeki,
				slotItem.Taikuu,
				slotItem.Taisen,
				slotItem.HouMeityu,
				slotItem.Kaihi,
				slotItem.Sakuteki,
				slotItem.Syatei
			};
		}

		private Texture2D BgTextureResourceLoad(int masterId)
		{
			return Resources.Load($"Textures/SlotItems/{masterId}/2") as Texture2D;
		}

		internal void Release()
		{
			mPrefab_UIRemodelSlotItemChangePreviewInParameter = null;
			NGUITools.Destroy(paramTable);
			paramTable = null;
			NGUITools.Destroy(weaponName);
			weaponName = null;
			if (weaponTypeIcon != null)
			{
				weaponTypeIcon.Clear();
				NGUITools.Destroy(weaponTypeIcon);
			}
			weaponTypeIcon = null;
			if (weaponImage != null)
			{
				weaponImage.mainTexture = null;
				NGUITools.Destroy(weaponImage);
			}
			weaponImage = null;
			if (mLock_Icon != null)
			{
				mLock_Icon.Clear();
			}
			mLock_Icon = null;
			paramLavels = null;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref BackGround);
			UserInterfacePortManager.ReleaseUtils.Release(ref weaponName);
			UserInterfacePortManager.ReleaseUtils.Release(ref weaponTypeIcon);
			UserInterfacePortManager.ReleaseUtils.Release(ref weaponImage);
			UserInterfacePortManager.ReleaseUtils.Release(ref mLock_Icon);
			mPrefab_UIRemodelSlotItemChangePreviewInParameter = null;
			paramTable = null;
		}

		internal void UnloadSlotItemTexture(bool unloadTexture = false)
		{
			if (weaponImage != null)
			{
				if (weaponImage.mainTexture != null && unloadTexture)
				{
					Resources.UnloadAsset(weaponImage.mainTexture);
				}
				weaponImage.mainTexture = null;
			}
		}
	}
}
