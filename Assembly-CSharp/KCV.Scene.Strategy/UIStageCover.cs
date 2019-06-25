using local.models;
using UnityEngine;

namespace KCV.Scene.Strategy
{
	public class UIStageCover : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_Cover;

		[SerializeField]
		private UITexture mTexture_Lock;

		[SerializeField]
		private UITexture mTexture_ClearEmblem;

		[SerializeField]
		private Transform mTransform_BossGaugeArea;

		public MapModel Model
		{
			get;
			private set;
		}

		public void Initialize(MapModel mapModel)
		{
			ReleaseUITexture(ref mTexture_Cover, unloadUnUsedAsset: true);
			Model = mapModel;
			if (Model != null)
			{
				if (Model.MapHP != null)
				{
					InitializeBossGauge(Model.MapHP);
				}
				mTexture_Cover.mainTexture = ResourceManager.LoadStageCover(Model.AreaId, Model.No);
				if (Model.Cleared)
				{
					mTexture_ClearEmblem.SetActive(isActive: true);
				}
				else
				{
					mTexture_ClearEmblem.SetActive(isActive: false);
				}
				if (!Model.Map_Possible)
				{
					mTexture_Lock.SetActive(isActive: true);
				}
				else
				{
					mTexture_Lock.SetActive(isActive: false);
				}
			}
		}

		private void InitializeBossGauge(MapHPModel mapHPModel)
		{
			GameObject original = Resources.Load("Prefabs/Common/MapHP/UIMapHP_3") as GameObject;
			UIMapHP component = Util.Instantiate(original, mTransform_BossGaugeArea.gameObject).GetComponent<UIMapHP>();
			component.Initialize(mapHPModel);
			component.Play();
		}

		private void ReleaseUITexture(ref UITexture uiTexture, bool unloadUnUsedAsset = false)
		{
			if (uiTexture != null)
			{
				if (uiTexture.mainTexture != null && unloadUnUsedAsset)
				{
					Resources.UnloadAsset(uiTexture.mainTexture);
				}
				uiTexture.mainTexture = null;
			}
		}

		private void OnDestroy()
		{
			ReleaseUITexture(ref mTexture_Cover, unloadUnUsedAsset: true);
			mTexture_Cover = null;
			ReleaseUITexture(ref mTexture_Lock);
			mTexture_Lock = null;
			ReleaseUITexture(ref mTexture_ClearEmblem);
			mTexture_ClearEmblem = null;
		}

		internal void SelfRelease()
		{
			ReleaseUITexture(ref mTexture_Cover, unloadUnUsedAsset: true);
		}
	}
}
