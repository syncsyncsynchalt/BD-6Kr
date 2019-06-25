using KCV.Utils;
using local.models;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	[RequireComponent(typeof(UIWidget))]
	public class UIRebellionSelectorShip : MonoBehaviour
	{
		[SerializeField]
		private UIWidget _uiWiget;

		[SerializeField]
		private UITexture _uiShipTexture;

		private ShipModel _clsShipModel;

		public ShipModel shipModel => _clsShipModel;

		public float textureAlpha
		{
			get
			{
				return _uiShipTexture.alpha;
			}
			set
			{
				_uiShipTexture.alpha = value;
			}
		}

		public static UIRebellionSelectorShip Instantiate(UIRebellionSelectorShip prefab, Transform parent, Vector3 pos, ShipModel model)
		{
			UIRebellionSelectorShip uIRebellionSelectorShip = Object.Instantiate(prefab);
			uIRebellionSelectorShip.transform.parent = parent;
			uIRebellionSelectorShip.transform.localScaleOne();
			uIRebellionSelectorShip.transform.localPosition = pos;
			uIRebellionSelectorShip.SetShipTexture(model);
			return uIRebellionSelectorShip;
		}

		private void Awake()
		{
			if (_uiWiget == null)
			{
				_uiWiget = GetComponent<UIWidget>();
			}
			if (_uiShipTexture == null)
			{
				Util.FindParentToChild(ref _uiShipTexture, base.transform, "ShipTexture");
			}
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiWiget);
			Mem.Del(ref _uiShipTexture);
			Mem.Del(ref _clsShipModel);
		}

		public bool Init(ShipModel model)
		{
			SetShipTexture(model);
			return true;
		}

		private void SetShipTexture(ShipModel model)
		{
			_clsShipModel = model;
			if (model == null)
			{
				_uiShipTexture.mainTexture = null;
				_uiShipTexture.transform.localPositionZero();
			}
			else
			{
				_uiShipTexture.mainTexture = ShipUtils.LoadTexture(model);
				_uiShipTexture.MakePixelPerfect();
				_uiShipTexture.transform.localPosition = Util.Poi2Vec(model.Offsets.GetCutinSp1_InBattle(model.IsDamaged()));
			}
		}
	}
}
