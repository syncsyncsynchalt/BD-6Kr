using local.models;
using UnityEngine;

namespace KCV.SortieBattle
{
	[RequireComponent(typeof(UIPanel))]
	public class BaseUISortieBattleShip<ShipModelType> : BaseShipTexture where ShipModelType : IMemShip
	{
		protected UIPanel _uiPanel;

		[SerializeField]
		[Header("Lov Parameter")]
		protected Vector3 originPos = Vector3.zero;

		[SerializeField]
		protected Vector3 lovMaxPos = Vector3.zero;

		public virtual ShipModelType shipModel => (ShipModelType)_clsIShipModel;

		public virtual UIPanel panel => this.GetComponentThis(ref _uiPanel);

		protected virtual void SetLovOffset(ShipModelType model)
		{
		}

		protected override void OnUnInit()
		{
			Mem.Del(ref _uiPanel);
		}
	}
}
