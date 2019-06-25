using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using UnityEngine;

namespace KCV
{
	public class BaseShipTexture : MonoBehaviour
	{
		[SerializeField]
		protected UITexture _uiShipTex;

		protected IShipModel _clsIShipModel;

		protected virtual void Load(int shipID, int texNum)
		{
			_uiShipTex.mainTexture = KCV.Utils.ShipUtils.LoadTexture(shipID, texNum);
			_uiShipTex.MakePixelPerfect();
		}

		protected virtual void SetShipTexture(ShipModel model)
		{
			_clsIShipModel = model;
			if (model == null)
			{
				_uiShipTex.mainTexture = null;
				_uiShipTex.transform.localPosition.Zero();
			}
			else
			{
				_uiShipTex.mainTexture = KCV.Utils.ShipUtils.LoadTexture(model);
				_uiShipTex.MakePixelPerfect();
			}
		}

		protected virtual void SetShipTexture(ShipModel_BattleResult model)
		{
			_clsIShipModel = model;
			if (model == null)
			{
				_uiShipTex.mainTexture = null;
				_uiShipTex.transform.localPosition.Zero();
			}
			else
			{
				_uiShipTex.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(model);
				_uiShipTex.MakePixelPerfect();
			}
		}

		protected virtual void OnDestroy()
		{
			_uiShipTex = null;
			_clsIShipModel = null;
			OnUnInit();
		}

		public virtual void Discard()
		{
			Object.Destroy(base.gameObject);
		}

		protected virtual void OnUnInit()
		{
		}
	}
}
