using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdSupportCutIn : BaseBattleAnimation
	{
		public enum AnimationList
		{
			None,
			SupportCutIn
		}

		[SerializeField]
		private UITexture _shipTex;

		[SerializeField]
		private UITexture _shipShadow;

		[SerializeField]
		private ParticleSystem _firePar;

		private ShipModel_Attacker _flagShip;

		private IShienModel _clsSupport;

		public static ProdSupportCutIn Instantiate(ProdSupportCutIn prefab, IShienModel model, Transform parent)
		{
			ProdSupportCutIn prodSupportCutIn = UnityEngine.Object.Instantiate(prefab);
			prodSupportCutIn._clsSupport = model;
			prodSupportCutIn.transform.parent = parent;
			prodSupportCutIn.transform.localPosition = Vector3.zero;
			prodSupportCutIn.transform.localScale = Vector3.one;
			return prodSupportCutIn;
		}

		protected override void Awake()
		{
			base.Awake();
			Util.FindParentToChild(ref _shipTex, base.transform, "ShipObj/Anchor/Object2D");
			Util.FindParentToChild(ref _shipShadow, base.transform, "ShipObj/Anchor/ObjectShadow");
			Util.FindParentToChild<ParticleSystem>(ref _firePar, base.transform, "Fire");
			((Component)_firePar).SetActive(isActive: false);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Mem.Del(ref _shipTex);
			Mem.Del(ref _shipShadow);
			Mem.Del(ref _firePar);
			Mem.Del(ref _flagShip);
			Mem.Del(ref _clsSupport);
		}

		private void _setShipInfo()
		{
			_flagShip = _clsSupport.ShienShips[0];
			_shipTex.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(_flagShip);
			_shipTex.MakePixelPerfect();
			_shipTex.transform.localPosition = Util.Poi2Vec(new ShipOffset(_flagShip.GetGraphicsMstId()).GetFace(_flagShip.DamagedFlg));
			_shipShadow.mainTexture = _shipTex.mainTexture;
			_shipShadow.MakePixelPerfect();
			_shipShadow.transform.localPosition = _shipTex.transform.localPosition;
		}

		private void _debugShipInfo()
		{
			_shipTex.mainTexture = KCV.Battle.Utils.ShipUtils.LoadTexture(82, isDamaged: false);
			_shipTex.MakePixelPerfect();
			_shipTex.transform.localPosition = Util.Poi2Vec(new ShipOffset(82).GetFace(damaged: false));
			_shipShadow.mainTexture = _shipTex.mainTexture;
			_shipShadow.MakePixelPerfect();
			_shipShadow.transform.localPosition = _shipTex.transform.localPosition;
		}

		private void _setCameraRotation()
		{
			BattleFieldCamera friendFieldCamera = BattleTaskManager.GetBattleCameras().friendFieldCamera;
			friendFieldCamera.ReqViewMode(CameraActor.ViewMode.RotateAroundObject);
			friendFieldCamera.SetRotateAroundObjectCamera(Vector3.zero, new Vector3(0f, 19f, 220f), 1f);
		}

		public override void Play(Action callback)
		{
			base.transform.localScale = Vector3.one;
			_actCallback = callback;
			_setCameraRotation();
			_setShipInfo();
			setGlowEffects();
			((Component)_firePar).SetActive(isActive: true);
			((Component)_firePar).transform.localScale = new Vector3(100f, 1f, 1f);
			_firePar.Stop();
			_firePar.Play();
			base.Play(AnimationList.SupportCutIn, callback);
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_057);
		}

		private AnimationList getAnimationList()
		{
			return AnimationList.SupportCutIn;
		}

		private void startMotionBlur()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.motionBlur.enabled = true;
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
				SEFIleInfos info = SEFIleInfos.SE_057;
				base._playSE(info);
				break;
			}
			}
		}

		private void _playShipVoice()
		{
			KCV.Battle.Utils.ShipUtils.PlaySupportingFireVoice(_flagShip);
		}

		private void stopParticle()
		{
			_firePar.Stop();
		}

		private void _onFinishedAnimation()
		{
			if (_actCallback != null)
			{
				_actCallback();
			}
		}
	}
}
