using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdAntiAerialCutIn : BaseBattleAnimation
	{
		public enum AnimationList
		{
			None,
			AntiAerialCutIn2
		}

		[SerializeField]
		private GameObject _uiShipObj;

		[SerializeField]
		private UITexture _uiShip;

		[SerializeField]
		private UITexture _uiShipShadow;

		[SerializeField]
		private UITexture[] _uiSlotBg;

		[SerializeField]
		private UILabel[] _uiSlotLabel;

		private KoukuuModel _clsAerial;

		private ShipModel_Attacker _ship;

		private AnimationList _iList;

		private UIPanel _uiPanel;

		public UIPanel panel => this.GetComponentThis(ref _uiPanel);

		public static ProdAntiAerialCutIn Instantiate(ProdAntiAerialCutIn prefab, KoukuuModel model, Transform parent)
		{
			ProdAntiAerialCutIn prodAntiAerialCutIn = UnityEngine.Object.Instantiate(prefab);
			prodAntiAerialCutIn._clsAerial = model;
			prodAntiAerialCutIn.transform.parent = parent;
			prodAntiAerialCutIn.transform.localPosition = Vector3.zero;
			prodAntiAerialCutIn.transform.localScale = Vector3.one;
			prodAntiAerialCutIn._iList = AnimationList.None;
			prodAntiAerialCutIn.panel.widgetsAreStatic = true;
			return prodAntiAerialCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
			_iList = AnimationList.None;
			if (_uiShipObj == null)
			{
				_uiShipObj = base.transform.FindChild("ShipObj").gameObject;
			}
			Util.FindParentToChild(ref _uiShip, base.transform, "ShipObj/Anchor/Object2D");
			Util.FindParentToChild(ref _uiShipShadow, base.transform, "ShipObj/Anchor/ObjectShadow");
			_uiSlotLabel = new UILabel[3];
			_uiSlotBg = new UITexture[3];
			for (int i = 0; i < 3; i++)
			{
				Util.FindParentToChild(ref _uiSlotLabel[i], base.transform, "SlotLabel" + (i + 1));
				Util.FindParentToChild(ref _uiSlotBg[i], base.transform, "SlotBg" + (i + 1));
				_uiSlotLabel[i].SetActive(isActive: false);
				_uiSlotBg[i].SetActive(isActive: false);
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _uiShipObj);
			Mem.Del(ref _uiShip);
			Mem.Del(ref _uiShipShadow);
			Mem.DelArySafe(ref _uiSlotBg);
			Mem.DelArySafe(ref _uiSlotLabel);
			Mem.Del(ref _clsAerial);
			Mem.Del(ref _ship);
			Mem.Del(ref _iList);
			Mem.Del(ref _uiPanel);
		}

		private void _setShipInfo(bool isFriend)
		{
			_ship = _clsAerial.GetTaikuShip(isFriend);
			_uiShip.mainTexture = ShipUtils.LoadTexture(_ship);
			_uiShip.MakePixelPerfect();
			_uiShip.transform.localPosition = Util.Poi2Vec(new ShipOffset(_ship.GetGraphicsMstId()).GetShipDisplayCenter(_ship.DamagedFlg));
			_uiShip.flip = ((!isFriend) ? UIBasicSprite.Flip.Horizontally : UIBasicSprite.Flip.Nothing);
			_uiShipShadow.mainTexture = _uiShip.mainTexture;
			_uiShipShadow.MakePixelPerfect();
			_uiShipShadow.flip = ((!isFriend) ? UIBasicSprite.Flip.Horizontally : UIBasicSprite.Flip.Nothing);
			_uiShipObj.transform.localRotation = ((!isFriend) ? Quaternion.EulerAngles(new Vector3(0f, 180f, 0f)) : Quaternion.EulerAngles(Vector3.zero));
		}

		private void _setSlotLabel(bool isFriend)
		{
			List<SlotitemModel_Battle> taikuSlotitems = _clsAerial.GetTaikuSlotitems(isFriend);
			if (taikuSlotitems != null)
			{
				for (int i = 0; i < taikuSlotitems.Count && i < 3; i++)
				{
					_uiSlotBg[i].SetActive(isActive: true);
					_uiSlotLabel[i].SetActive(isActive: true);
					_uiSlotLabel[i].text = taikuSlotitems[i].Name;
				}
			}
		}

		public override void Play(Action callback)
		{
			base.Play(_iList, callback);
		}

		public void Play(Action callback, bool isFriend)
		{
			panel.widgetsAreStatic = false;
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			cutInCamera.depth = 5f;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			cutInEffectCamera.depth = 6f;
			base.transform.localScale = Vector3.one;
			_actCallback = callback;
			_iList = getAnimationList();
			if (_iList == AnimationList.None)
			{
				onAnimationFinishedAfterDiscard();
				return;
			}
			_setShipInfo(isFriend);
			_setSlotLabel(isFriend);
			setGlowEffects();
			Play(callback);
		}

		private AnimationList getAnimationList()
		{
			return AnimationList.AntiAerialCutIn2;
		}

		private void startMotionBlur()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.motionBlur.enabled = true;
		}

		private void endMotionBlur()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.motionBlur.enabled = false;
		}

		private void _playExplosionParticle(int num)
		{
			if (num == 0)
			{
				ShipUtils.PlayAircraftCutInVoice(_ship);
			}
		}

		private void onPlaySeAnime(int seNo)
		{
			switch (seNo)
			{
			case 0:
			{
				SEFIleInfos info = SEFIleInfos.BattleAdmission;
				base._playSE(info);
				break;
			}
			case 1:
			{
				SEFIleInfos info = SEFIleInfos.BattleNightMessage;
				base._playSE(info);
				break;
			}
			}
		}

		private void _onFinishedAnimation()
		{
			panel.widgetsAreStatic = true;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.motionBlur.enabled = false;
			if (_actCallback != null)
			{
				_actCallback();
			}
		}
	}
}
