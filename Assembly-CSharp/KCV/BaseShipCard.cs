using Common.Enum;
using local.models;
using UnityEngine;

namespace KCV
{
	public class BaseShipCard : MonoBehaviour
	{
		protected ShipModel _clsShipModel;

		public virtual bool Init(ShipModel ship, UITexture _texture)
		{
			if (ship == null)
			{
				return false;
			}
			_clsShipModel = ship;
			int texNum = (!ship.IsDamaged()) ? 3 : 4;
			_Load(ship.MstId, texNum, _texture);
			return true;
		}

		protected virtual void _Load(int shipID, int texNum, UITexture _texture)
		{
			_texture.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(shipID, texNum);
			Vector2 vector = ResourceManager.SHIP_TEXTURE_SIZE[texNum];
			_texture.height = (int)vector.y;
			Vector2 vector2 = ResourceManager.SHIP_TEXTURE_SIZE[texNum];
			_texture.width = (int)vector2.x;
		}

		public virtual void UpdateStateIcon(UISprite _stateIcon)
		{
			_stateIcon.transform.localPositionX(67f);
			if (_clsShipModel.IsInRepair())
			{
				_stateIcon.alpha = 1f;
				_stateIcon.spriteName = "icon-s_syufuku";
			}
			else if (_clsShipModel.IsBling())
			{
				_stateIcon.alpha = 1f;
				_stateIcon.transform.localPositionX(88f);
				_stateIcon.spriteName = "icon-s_kaiko";
			}
			else if (_clsShipModel.IsInMission())
			{
				_stateIcon.alpha = 1f;
				_stateIcon.spriteName = "icon-s_ensei";
			}
			else if (_clsShipModel.DamageStatus == DamageState.Taiha)
			{
				_stateIcon.alpha = 1f;
				_stateIcon.spriteName = "icon-s_taiha";
			}
			else if (_clsShipModel.DamageStatus == DamageState.Tyuuha)
			{
				_stateIcon.alpha = 1f;
				_stateIcon.spriteName = "icon-s_chuha";
			}
			else if (_clsShipModel.DamageStatus == DamageState.Shouha)
			{
				_stateIcon.alpha = 1f;
				_stateIcon.spriteName = "icon-s_shoha";
			}
			else
			{
				_stateIcon.alpha = 0f;
			}
		}

		public virtual void UpdateFatigue(FatigueState state, UISprite _fatigueIcon, UISprite _fatigueMask)
		{
			switch (state)
			{
			case FatigueState.Normal:
				_fatigueMask.alpha = 0f;
				_fatigueIcon.alpha = 0f;
				break;
			case FatigueState.Light:
				_fatigueMask.spriteName = "card-s_fatigue_01";
				_fatigueIcon.spriteName = "icon-s_fatigue1";
				_fatigueMask.alpha = 1f;
				_fatigueIcon.alpha = 1f;
				break;
			case FatigueState.Distress:
				_fatigueMask.spriteName = "card-s_fatigue_02";
				_fatigueIcon.spriteName = "icon-s_fatigue2";
				_fatigueMask.alpha = 1f;
				_fatigueIcon.alpha = 1f;
				break;
			case FatigueState.Exaltation:
				_fatigueMask.alpha = 0f;
				_fatigueIcon.alpha = 0f;
				break;
			}
		}
	}
}
