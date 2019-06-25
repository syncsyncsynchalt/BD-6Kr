using local.models;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Resource
{
	public class ShipBannerManager : MonoBehaviour
	{
		private ShipResource mShipResource_Banner_Normal;

		private ShipResource mShipResource_Banner_Damaged;

		private ShipResource mShipResource_Card_Normal;

		private ShipResource mShipResource_Card_Damaged;

		private ShipResource mShipResource_Full_Normal;

		private ShipResource mShipResource_Full_Damaged;

		private IEnumerator mInitializeCoroutine;

		private void Awake()
		{
			mShipResource_Banner_Normal = new ShipResource(TextureType.Banner, StateType.Normal);
			mShipResource_Banner_Damaged = new ShipResource(TextureType.Banner, StateType.Damaged);
			mShipResource_Card_Normal = new ShipResource(TextureType.Card, StateType.Normal);
			mShipResource_Card_Damaged = new ShipResource(TextureType.Card, StateType.Damaged);
			mShipResource_Full_Normal = new ShipResource(TextureType.Full, StateType.Normal);
			mShipResource_Full_Damaged = new ShipResource(TextureType.Full, StateType.Damaged);
		}

		public void Initialize(ShipModel[] shipModels)
		{
			if (mInitializeCoroutine != null)
			{
				StopCoroutine(mInitializeCoroutine);
				mInitializeCoroutine = null;
			}
			mInitializeCoroutine = InitializeCoroutine(shipModels);
			StartCoroutine(mInitializeCoroutine);
		}

		private IEnumerator InitializeCoroutine(ShipModel[] shipModels)
		{
			Stopwatch sw = new Stopwatch();
			sw.Reset();
			sw.Start();
			yield return StartCoroutine(mShipResource_Banner_Normal.GenerateLoadAsync(shipModels));
			sw.Stop();
			UnityEngine.Debug.Log("LoadedShipBanners_Banner:" + sw.ElapsedMilliseconds + " ms");
			sw.Reset();
			sw.Start();
			yield return StartCoroutine(mShipResource_Banner_Damaged.GenerateLoadAsync(shipModels));
			sw.Stop();
			UnityEngine.Debug.Log("LoadedShipBanners_Banner_D:" + sw.ElapsedMilliseconds + " ms");
			sw.Reset();
		}

		public Texture GetShipBanner(ShipModel shipModel)
		{
			if (shipModel.IsDamaged())
			{
				return GetDamagedBanner(shipModel.GetGraphicsMstId());
			}
			return GetNormalBanner(shipModel.GetGraphicsMstId());
		}

		public Texture GetNormalBanner(int masterId)
		{
			return mShipResource_Banner_Normal.GetResource(masterId);
		}

		public Texture GetDamagedBanner(int masterId)
		{
			return mShipResource_Banner_Damaged.GetResource(masterId);
		}
	}
}
