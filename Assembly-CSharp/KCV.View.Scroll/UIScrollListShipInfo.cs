using DG.Tweening;
using local.models;
using UnityEngine;

namespace KCV.View.Scroll
{
	public class UIScrollListShipInfo : MonoBehaviour
	{
		[SerializeField]
		private UITexture mTexture_ShipCard;

		private Vector3 mVector3ShipCardDefaultPosition;

		private void Start()
		{
			mVector3ShipCardDefaultPosition = mTexture_ShipCard.transform.localPosition;
		}

		public void Initialize(ShipModel model)
		{
			if (model != null)
			{
				mTexture_ShipCard.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, 3);
				mTexture_ShipCard.transform.DOShakePosition(0.3f, 5f).OnComplete(delegate
				{
					mTexture_ShipCard.transform.DOLocalMove(mVector3ShipCardDefaultPosition, 0.1f);
				});
			}
		}
	}
}
