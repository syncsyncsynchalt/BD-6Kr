using System;
using System.Runtime.InteropServices;

namespace Sony.NP
{
	public class Commerce
	{
		public struct CommerceCategoryInfo
		{
			private IntPtr _categoryId;

			private IntPtr _categoryName;

			private IntPtr _categoryDescription;

			private IntPtr _imageUrl;

			public int countOfProducts;

			public int countOfSubCategories;

			public string categoryId => Marshal.PtrToStringAnsi(_categoryId);

			public string categoryName => Marshal.PtrToStringAnsi(_categoryName);

			public string categoryDescription => Marshal.PtrToStringAnsi(_categoryDescription);

			public string imageUrl => Marshal.PtrToStringAnsi(_imageUrl);
		}

		public struct CommerceProductInfo
		{
			public int purchasabilityFlag;

			private IntPtr _productId;

			private IntPtr _productName;

			private IntPtr _shortDescription;

			private IntPtr _spName;

			private IntPtr _imageUrl;

			private IntPtr _price;

			private ulong releaseDate;

			public string productId => Marshal.PtrToStringAnsi(_productId);

			public string productName => Marshal.PtrToStringAnsi(_productName);

			public string shortDescription => Marshal.PtrToStringAnsi(_shortDescription);

			public string spName => Marshal.PtrToStringAnsi(_spName);

			public string imageUrl => Marshal.PtrToStringAnsi(_imageUrl);

			public string price => Marshal.PtrToStringAnsi(_price);
		}

		public struct CommerceProductInfoDetailed
		{
			public int purchasabilityFlag;

			private IntPtr _skuId;

			private IntPtr _productId;

			private IntPtr _productName;

			private IntPtr _shortDescription;

			private IntPtr _longDescription;

			private IntPtr _legalDescription;

			private IntPtr _spName;

			private IntPtr _imageUrl;

			private IntPtr _price;

			private IntPtr _ratingSystemId;

			private IntPtr _ratingImageUrl;

			private ulong releaseDate;

			public int numRatingDescriptors;

			public string skuId => Marshal.PtrToStringAnsi(_skuId);

			public string productId => Marshal.PtrToStringAnsi(_productId);

			public string productName => Marshal.PtrToStringAnsi(_productName);

			public string shortDescription => Marshal.PtrToStringAnsi(_shortDescription);

			public string longDescription => Marshal.PtrToStringAnsi(_longDescription);

			public string legalDescription => Marshal.PtrToStringAnsi(_legalDescription);

			public string spName => Marshal.PtrToStringAnsi(_spName);

			public string imageUrl => Marshal.PtrToStringAnsi(_imageUrl);

			public string price => Marshal.PtrToStringAnsi(_price);

			public string ratingSystemId => Marshal.PtrToStringAnsi(_ratingSystemId);

			public string ratingImageUrl => Marshal.PtrToStringAnsi(_ratingImageUrl);
		}

		public struct CommerceEntitlement
		{
			private IntPtr _id;

			public int type;

			public int remainingCount;

			public int consumedCount;

			public ulong createdDate;

			public ulong expireDate;

			public string id => Marshal.PtrToStringAnsi(_id);
		}

		public enum StoreIconPosition
		{
			Left,
			Center,
			Right
		}

		public static event Messages.EventHandler OnSessionCreated;

		public static event Messages.EventHandler OnSessionAborted;

		public static event Messages.EventHandler OnGotCategoryInfo;

		public static event Messages.EventHandler OnGotProductList;

		public static event Messages.EventHandler OnGotProductInfo;

		public static event Messages.EventHandler OnGotEntitlementList;

		public static event Messages.EventHandler OnConsumedEntitlement;

		public static event Messages.EventHandler OnError;

		public static event Messages.EventHandler OnCheckoutStarted;

		public static event Messages.EventHandler OnCheckoutFinished;

		public static event Messages.EventHandler OnProductBrowseStarted;

		public static event Messages.EventHandler OnProductBrowseSuccess;

		public static event Messages.EventHandler OnProductBrowseAborted;

		public static event Messages.EventHandler OnProductBrowseFinished;

		public static event Messages.EventHandler OnProductCategoryBrowseStarted;

		public static event Messages.EventHandler OnProductCategoryBrowseFinished;

		public static event Messages.EventHandler OnProductVoucherInputStarted;

		public static event Messages.EventHandler OnProductVoucherInputFinished;

		public static event Messages.EventHandler OnDownloadListStarted;

		public static event Messages.EventHandler OnDownloadListFinished;

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxCommerceIsBusy();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxCommerceCreateSession();

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxCommerceBrowseCategory(string categoryId);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxCommerceBrowseProduct(string productId);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxCommerceDisplayDownloadList();

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxCommerceCheckout(string[] skuIDs, int count);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxCommerceVoucherInput();

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxCommerceRequestCategoryInfo(string categoryId);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxCommerceGetCategoryInfo(out CommerceCategoryInfo categoryInfo);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxCommerceGetSubCategoryInfo(int index, out CommerceCategoryInfo categoryInfo);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxCommerceRequestProductList(string categoryId);

		[DllImport("UnityNpToolkit")]
		private static extern int PrxCommerceGetProductListInfoCount();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxCommerceGetProductListInfoItem(int index, out CommerceProductInfo productInfo);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxCommerceRequestDetailedProductInfo(string productId);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxCommerceGetProductInfoDetailed(out CommerceProductInfoDetailed productInfo);

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxCommerceRequestEntitlementList();

		[DllImport("UnityNpToolkit")]
		private static extern int PrxCommerceGetEntitlementListCount();

		[DllImport("UnityNpToolkit")]
		private static extern ErrorCode PrxCommerceGetEntitlementListItem(int index, out CommerceEntitlement entitlement);

		[DllImport("UnityNpToolkit", CharSet = CharSet.Ansi)]
		private static extern ErrorCode PrxCommerceConsumeEntitlement(string entitlementId, int consumeCount);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxCommerceShowStoreIcon(bool show, StoreIconPosition position);

		[DllImport("UnityNpToolkit")]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool PrxCommerceGetLastError(out ResultCode result);

		public static bool GetLastError(out ResultCode result)
		{
			PrxCommerceGetLastError(out result);
			return result.lastError == ErrorCode.NP_OK;
		}

		public static bool IsBusy()
		{
			return PrxCommerceIsBusy();
		}

		public static ErrorCode CreateSession()
		{
			return PrxCommerceCreateSession();
		}

		public static ErrorCode BrowseCategory(string categoryId)
		{
			return PrxCommerceBrowseCategory(categoryId);
		}

		public static ErrorCode BrowseProduct(string productId)
		{
			return PrxCommerceBrowseProduct(productId);
		}

		public static ErrorCode DisplayDownloadList()
		{
			return PrxCommerceDisplayDownloadList();
		}

		public static ErrorCode Checkout(string[] skuIDs)
		{
			return PrxCommerceCheckout(skuIDs, skuIDs.Length);
		}

		public static ErrorCode VoucherInput()
		{
			return PrxCommerceVoucherInput();
		}

		public static ErrorCode RequestCategoryInfo(string categoryId)
		{
			return PrxCommerceRequestCategoryInfo(categoryId);
		}

		public static CommerceCategoryInfo GetCategoryInfo()
		{
			CommerceCategoryInfo categoryInfo = default(CommerceCategoryInfo);
			PrxCommerceGetCategoryInfo(out categoryInfo);
			return categoryInfo;
		}

		public static CommerceCategoryInfo GetSubCategoryInfo(int index)
		{
			CommerceCategoryInfo categoryInfo = default(CommerceCategoryInfo);
			PrxCommerceGetSubCategoryInfo(index, out categoryInfo);
			return categoryInfo;
		}

		public static ErrorCode RequestProductList(string categoryId)
		{
			return PrxCommerceRequestProductList(categoryId);
		}

		public static CommerceProductInfo[] GetProductList()
		{
			int num = PrxCommerceGetProductListInfoCount();
			CommerceProductInfo[] array = new CommerceProductInfo[num];
			for (int i = 0; i < num; i++)
			{
				PrxCommerceGetProductListInfoItem(i, out array[i]);
			}
			return array;
		}

		public static ErrorCode RequestDetailedProductInfo(string productId)
		{
			return PrxCommerceRequestDetailedProductInfo(productId);
		}

		public static CommerceProductInfoDetailed GetDetailedProductInfo()
		{
			CommerceProductInfoDetailed productInfo = default(CommerceProductInfoDetailed);
			PrxCommerceGetProductInfoDetailed(out productInfo);
			return productInfo;
		}

		public static ErrorCode RequestEntitlementList()
		{
			return PrxCommerceRequestEntitlementList();
		}

		public static CommerceEntitlement[] GetEntitlementList()
		{
			int num = PrxCommerceGetEntitlementListCount();
			CommerceEntitlement[] array = new CommerceEntitlement[num];
			for (int i = 0; i < num; i++)
			{
				PrxCommerceGetEntitlementListItem(i, out array[i]);
			}
			return array;
		}

		public static ErrorCode ConsumeEntitlement(string entitlementId, int consumeCount)
		{
			return PrxCommerceConsumeEntitlement(entitlementId, consumeCount);
		}

		public static bool ShowStoreIcon(StoreIconPosition position)
		{
			return PrxCommerceShowStoreIcon(show: true, position);
		}

		public static bool HideStoreIcon()
		{
			return PrxCommerceShowStoreIcon(show: false, StoreIconPosition.Center);
		}

		public static bool ProcessMessage(Messages.PluginMessage msg)
		{
			switch (msg.type)
			{
			case Messages.MessageType.kNPToolKit_CommerceSessionCreated:
				if (Commerce.OnSessionCreated != null)
				{
					Commerce.OnSessionCreated(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceSessionAborted:
				if (Commerce.OnSessionAborted != null)
				{
					Commerce.OnSessionAborted(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceGotCategoryInfo:
				if (Commerce.OnGotCategoryInfo != null)
				{
					Commerce.OnGotCategoryInfo(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceGotProductList:
				if (Commerce.OnGotProductList != null)
				{
					Commerce.OnGotProductList(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceGotProductInfo:
				if (Commerce.OnGotProductInfo != null)
				{
					Commerce.OnGotProductInfo(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceGotEntitlementList:
				if (Commerce.OnGotEntitlementList != null)
				{
					Commerce.OnGotEntitlementList(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceConsumedEntitlement:
				if (Commerce.OnConsumedEntitlement != null)
				{
					Commerce.OnConsumedEntitlement(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceError:
				if (Commerce.OnError != null)
				{
					Commerce.OnError(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceCheckoutStarted:
				if (Commerce.OnCheckoutStarted != null)
				{
					Commerce.OnCheckoutStarted(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceCheckoutFinished:
				if (Commerce.OnCheckoutFinished != null)
				{
					Commerce.OnCheckoutFinished(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceProductBrowseStarted:
				if (Commerce.OnProductBrowseStarted != null)
				{
					Commerce.OnProductBrowseStarted(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceProductBrowseSuccess:
				if (Commerce.OnProductBrowseSuccess != null)
				{
					Commerce.OnProductBrowseSuccess(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceProductBrowseAborted:
				if (Commerce.OnProductBrowseAborted != null)
				{
					Commerce.OnProductBrowseAborted(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceProductBrowseFinished:
				if (Commerce.OnProductBrowseFinished != null)
				{
					Commerce.OnProductBrowseFinished(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceCategoryBrowseStarted:
				if (Commerce.OnProductCategoryBrowseStarted != null)
				{
					Commerce.OnProductCategoryBrowseStarted(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceCategoryBrowseFinished:
				if (Commerce.OnProductCategoryBrowseFinished != null)
				{
					Commerce.OnProductCategoryBrowseFinished(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceVoucherInputStarted:
				if (Commerce.OnProductVoucherInputStarted != null)
				{
					Commerce.OnProductVoucherInputStarted(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceVoucherInputFinished:
				if (Commerce.OnProductVoucherInputFinished != null)
				{
					Commerce.OnProductVoucherInputFinished(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceDownloadListStarted:
				if (Commerce.OnDownloadListStarted != null)
				{
					Commerce.OnDownloadListStarted(msg);
				}
				return true;
			case Messages.MessageType.kNPToolKit_CommerceDownloadListFinished:
				if (Commerce.OnDownloadListFinished != null)
				{
					Commerce.OnDownloadListFinished(msg);
				}
				return true;
			default:
				return false;
			}
		}
	}
}
