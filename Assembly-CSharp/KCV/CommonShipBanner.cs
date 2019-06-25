using Common.Enum;
using local.models;
using UnityEngine;

namespace KCV
{
	public class CommonShipBanner : BaseShipBanner
	{
		public UISprite UiConditionIcon;

		public UISprite UiConditionMask;

		public GameObject Kira;

		public GameObject Smoke;

		public GameObject Ring;

		public int sizeX;

		private Vector2 defaultLocalPos;

		private Vector2 blingIconLocalPos;

		public bool isUseKira;

		public bool isUseSmoke;

		private BannerSmokes bannerSmokes;

		private Transform ParticlePanel;

		private bool isAwake;

		protected override void Awake()
		{
			if (isAwake)
			{
				return;
			}
			base.Awake();
			UiConditionMask.alpha = 0f;
			UiConditionIcon.alpha = 0f;
			_uiDamageIcon.keepAspectRatio = UIWidget.AspectRatioSource.Free;
			isUseKira = true;
			isUseSmoke = true;
			ParticlePanel = base.transform.FindChild("ParticlePanel");
			if (ParticlePanel == null)
			{
				ParticlePanel = _uiShipTex.transform.FindChild("ParticlePanel");
				if (ParticlePanel == null)
				{
					_uiShipTex.transform.AddChild("ParticlePanel");
					ParticlePanel = _uiShipTex.transform.FindChild("ParticlePanel");
				}
			}
			if (ParticlePanel != null)
			{
				ParticlePanel.SetParent(_uiShipTex.transform);
				ParticlePanel.transform.localScale = Vector3.one;
				ParticlePanel.transform.localPosition = new Vector3(-128f, 32f, 0f);
			}
			if (_uiDamageIcon != null)
			{
				_uiShipTex.pivot = UIWidget.Pivot.Center;
				_uiDamageIcon.pivot = UIWidget.Pivot.Center;
				_uiDamageIcon.transform.SetParent(_uiShipTex.transform);
			}
			isAwake = true;
		}

		public UITexture GetUITexture()
		{
			return _uiShipTex;
		}

		public new virtual void SetShipData(ShipModel model)
		{
			if (model == null)
			{
				_clsShipModel = null;
				return;
			}
			if (!isAwake)
			{
				Awake();
			}
			if (UiConditionIcon == null)
			{
				UiConditionIcon = ((Component)base.transform.FindChild("ConditionIcon")).GetComponent<UISprite>();
			}
			if (UiConditionMask == null)
			{
				UiConditionMask = ((Component)base.transform.FindChild("ConditionMask")).GetComponent<UISprite>();
			}
			_clsShipModel = model;
			int texNum = (!model.IsDamaged()) ? 1 : 2;
			_uiShipTex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, texNum);
			SetSmoke(model);
			SetStateIcon(model);
			InitMarriageRing(model);
			UpdateCondition(model.ConditionState);
		}

		public void SetShipDataWithDisableParticle(ShipModel model)
		{
			StopParticle();
			if (model == null)
			{
				_clsShipModel = null;
				return;
			}
			if (!isAwake)
			{
				Awake();
			}
			if (UiConditionIcon == null)
			{
				UiConditionIcon = ((Component)base.transform.FindChild("ConditionIcon")).GetComponent<UISprite>();
			}
			if (UiConditionMask == null)
			{
				UiConditionMask = ((Component)base.transform.FindChild("ConditionMask")).GetComponent<UISprite>();
			}
			_clsShipModel = model;
			int texNum = (!model.IsDamaged()) ? 1 : 2;
			_uiShipTex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, texNum);
			UpdateCondition(model.ConditionState);
		}

		public virtual void SetShipData(ShipModel_Practice model)
		{
			if (model != null)
			{
				int texNum = (!model.IsDamaged()) ? 1 : 2;
				_uiShipTex.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(model.MstId, texNum);
				_uiDamageMask.alpha = 0f;
				_uiDamageIcon.alpha = 0f;
				UiConditionMask.alpha = 0f;
				UiConditionIcon.alpha = 0f;
			}
		}

		public static void stopBannerParticle()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("CommonShipBanner");
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				gameObject.SendMessage("StopParticle");
			}
		}

		public static void startBannerParticle()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("CommonShipBanner");
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				gameObject.SendMessage("StartParticle");
			}
		}

		private void UpdateCondition(FatigueState state)
		{
			switch (state)
			{
			case FatigueState.Normal:
				UiConditionMask.alpha = 0f;
				UiConditionIcon.alpha = 0f;
				if (Kira != null)
				{
					Kira.SetActive(false);
				}
				break;
			case FatigueState.Light:
				UiConditionMask.alpha = 1f;
				UiConditionIcon.alpha = 1f;
				UiConditionMask.spriteName = "card-ss_fatigue_1";
				UiConditionIcon.spriteName = "icon_fatigue_1";
				if (Kira != null)
				{
					Kira.SetActive(false);
				}
				break;
			case FatigueState.Distress:
				UiConditionMask.alpha = 1f;
				UiConditionIcon.alpha = 1f;
				UiConditionMask.spriteName = "card-ss_fatigue_2";
				UiConditionIcon.spriteName = "icon_fatigue_2";
				if (Kira != null)
				{
					Kira.SetActive(false);
				}
				break;
			case FatigueState.Exaltation:
				UiConditionMask.alpha = 0f;
				UiConditionIcon.alpha = 0f;
				SetKiraPar();
				break;
			}
		}

		public void StartParticle()
		{
			if (ParticlePanel != null)
			{
				ParticlePanel.SetActive(isActive: true);
				UIPanel component = ((Component)ParticlePanel).GetComponent<UIPanel>();
				if (component != null)
				{
					component.alpha = 0f;
					TweenAlpha.Begin(component.gameObject, 1f, 1f);
				}
			}
			if (Smoke != null && isNeedSmoke())
			{
				Smoke.SetActive(true);
			}
			if (Kira != null && isNeedKira())
			{
				Kira.SetActive(true);
			}
		}

		public void StopParticle()
		{
			if (ParticlePanel != null)
			{
				ParticlePanel.SetActive(isActive: false);
			}
			if (Smoke != null)
			{
				Smoke.SetActive(false);
			}
			if (Kira != null)
			{
				Kira.SetActive(false);
			}
		}

		private void SetStateIcon(ShipModel model)
		{
			if (model.IsInRepair())
			{
				_uiDamageIcon.alpha = 1f;
				_uiDamageIcon.spriteName = "icon-ss_syufuku";
				setStateIconSize(isBling: false);
			}
			else if (model.IsInMission())
			{
				_uiDamageIcon.alpha = 1f;
				_uiDamageIcon.spriteName = "icon-ss_ensei";
				setStateIconSize(isBling: false);
			}
			else if (model.IsTettaiBling() && model.IsInDeck() != -1)
			{
				_uiDamageIcon.alpha = 1f;
				_uiDamageIcon.spriteName = "shipicon_withdraw";
				setStateIconSize(isBling: true);
			}
			else if (model.IsBling())
			{
				_uiDamageIcon.alpha = 1f;
				_uiDamageIcon.spriteName = "icon_kaikou";
				setStateIconSize(isBling: true);
			}
			else
			{
				UpdateDamage(model.DamageStatus);
				setStateIconSize(isBling: false);
			}
		}

		private void setStateIconSize(bool isBling)
		{
			if (isBling)
			{
				_uiDamageIcon.transform.localPositionX(100f);
				_uiDamageIcon.transform.localPositionY(0f);
				_uiDamageIcon.width = 48;
				_uiDamageIcon.height = 64;
				_uiDamageIcon.transform.localScale = Vector3.one;
			}
			else
			{
				_uiDamageIcon.transform.localPositionX(0f);
				_uiDamageIcon.transform.localPositionY(0f);
				_uiDamageIcon.width = 256;
				_uiDamageIcon.height = 64;
				_uiDamageIcon.transform.localScale = Vector3.one;
			}
		}

		private void SetSmoke(ShipModel model)
		{
			if (!isUseSmoke)
			{
				return;
			}
			if (model.DamageStatus != 0 && !model.IsInRepair())
			{
				if (Smoke == null)
				{
					CreateSmoke(model.DamageStatus);
					return;
				}
				ParticlePanel.SetActive(isActive: true);
				Smoke.SetActive(true);
			}
			else if (Smoke != null)
			{
				Smoke.SetActive(false);
			}
		}

		private void CreateSmoke(DamageState state)
		{
			if (ParticlePanel == null)
			{
				ParticlePanel = base.transform.FindChild("ParticlePanel");
			}
			Smoke = Util.Instantiate(Resources.Load("Prefabs/Common/BannerSmokes") as GameObject, ParticlePanel.gameObject);
			float magnification = getMagnification();
			Vector3 localScale = base.transform.localScale;
			float num = magnification / localScale.x;
			Smoke.transform.localScaleX(num);
			Smoke.transform.localScaleY(num);
			bannerSmokes = Smoke.GetComponent<BannerSmokes>();
			if (!ParticlePanel.gameObject.activeSelf)
			{
				ParticlePanel.gameObject.SetActive(true);
			}
		}

		private void SetKiraPar()
		{
			if (isUseKira)
			{
				ParticlePanel.SetActive(isActive: true);
				if (Kira == null)
				{
					CreateKiraPar();
					return;
				}
				ParticlePanel.SetActive(isActive: true);
				Kira.SetActive(true);
			}
		}

		private void CreateKiraPar()
		{
			if (ParticlePanel == null)
			{
				ParticlePanel = base.transform.FindChild("ParticlePanel");
			}
			Kira = Util.Instantiate(Resources.Load("Prefabs/Common/KiraPar") as GameObject, ParticlePanel.gameObject);
			float magnification = getMagnification();
			Vector3 localScale = base.transform.localScale;
			float num = magnification / localScale.x;
			Kira.transform.localScaleX(num);
			Kira.transform.localScaleY(num);
			if (!ParticlePanel.gameObject.activeSelf)
			{
				ParticlePanel.gameObject.SetActive(true);
			}
		}

		private void InitMarriageRing(ShipModel shipModel)
		{
			if (shipModel != null && shipModel.IsMarriage())
			{
				if (Ring == null)
				{
					Ring = Util.Instantiate(Resources.Load("Prefabs/Common/MarriagedRing") as GameObject, _uiShipTex.gameObject);
				}
			}
			else if (Ring != null)
			{
				Object.Destroy(Ring);
				Ring = null;
			}
		}

		private bool isNeedKira()
		{
			return _clsShipModel.ConditionState == FatigueState.Exaltation;
		}

		private bool isNeedSmoke()
		{
			return _clsShipModel.IsDamaged();
		}

		private void OnValidate()
		{
			if (_uiShipTex != null && _uiShipTex.mainTexture != null)
			{
				float magnification = getMagnification();
				base.transform.localScaleX(magnification);
				base.transform.localScaleY(magnification);
			}
		}

		private float getMagnification()
		{
			float num = _uiShipTex.mainTexture.width;
			float num2 = (float)_uiShipTex.mainTexture.height;
			Vector2 texelSize = _uiShipTex.mainTexture.texelSize;
			return (float)sizeX / num;
		}

		public void ReleaseShipBannerTexture(bool unloadAsset = false)
		{
			if (_uiShipTex != null)
			{
				if (_uiShipTex.mainTexture != null && unloadAsset)
				{
					Resources.UnloadAsset(_uiShipTex.mainTexture);
				}
				_uiShipTex.mainTexture = null;
			}
		}
	}
}
