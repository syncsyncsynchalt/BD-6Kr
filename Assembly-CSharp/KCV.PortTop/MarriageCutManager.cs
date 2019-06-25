using KCV.Utils;
using local.models;
using Server_Models;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.PortTop
{
	public class MarriageCutManager : MonoBehaviour
	{
		private int _debugIndex;

		private bool isControl;

		private Action _callback;

		private ShipModelMst _shipModelMst;

		private KeyControl _keyController;

		[SerializeField]
		private GameObject _objScene2;

		[SerializeField]
		private GameObject _objScene3;

		[SerializeField]
		private GameObject _objScene5;

		[SerializeField]
		private UITexture _uiShip;

		[SerializeField]
		private UITexture _uiBg;

		[SerializeField]
		private ParticleSystem _uiFeatherPar;

		[SerializeField]
		private ParticleSystem _uiLightPar;

		[SerializeField]
		private ParticleSystem _uiPetalPar1;

		[SerializeField]
		private ParticleSystem _uiPetalPar2;

		[SerializeField]
		private Animation _btnAnime;

		[SerializeField]
		private Animation _ringAnime;

		[SerializeField]
		private Animation _anime;

		[SerializeField]
		private Camera _blurCamera;

		public float Alpha
		{
			get
			{
				return GetComponent<UIPanel>().alpha;
			}
			set
			{
				GetComponent<UIPanel>().alpha = Alpha;
			}
		}

		private void init()
		{
			isControl = false;
			GetComponent<UIPanel>().alpha = 0f;
			if (_objScene2 == null)
			{
				_objScene2 = base.transform.FindChild("Camera/GameObject").gameObject;
			}
			if (_objScene3 == null)
			{
				_objScene3 = base.transform.FindChild("Camera/Scene3").gameObject;
			}
			if (_objScene5 == null)
			{
				_objScene5 = base.transform.FindChild("Camera/Scene5").gameObject;
			}
			Util.FindParentToChild(ref _uiShip, base.transform, "Camera/ShipObject/Character");
			Util.FindParentToChild(ref _uiBg, base.transform, "Camera/GameObject/Bg");
			Util.FindParentToChild<ParticleSystem>(ref _uiFeatherPar, base.transform, "CameraBlur/FeatherPar");
			Util.FindParentToChild<ParticleSystem>(ref _uiLightPar, base.transform, "Camera/LightPar");
			Util.FindParentToChild<ParticleSystem>(ref _uiPetalPar1, base.transform, "Camera/GameObject/MarriagePetal");
			Util.FindParentToChild<ParticleSystem>(ref _uiPetalPar2, base.transform, "Camera/MarriagePetal2");
			Util.FindParentToChild<Animation>(ref _btnAnime, base.transform, "Camera/Button");
			Util.FindParentToChild<Animation>(ref _ringAnime, base.transform, "Camera/Scene5/RingObj");
			Util.FindParentToChild(ref _blurCamera, base.transform, "CameraBlur");
			if ((UnityEngine.Object)_anime == null)
			{
				_anime = GetComponent<Animation>();
			}
			UIButtonMessage component = ((Component)base.transform.FindChild("Camera/Button")).GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "ButtonClick";
			component.trigger = UIButtonMessage.Trigger.OnClick;
			_objScene3.SetActive(false);
			_objScene5.SetActive(false);
		}

		private void OnDestroy()
		{
			Mem.Del(ref _callback);
			Mem.Del(ref _shipModelMst);
			Mem.Del(ref _objScene2);
			Mem.Del(ref _objScene3);
			Mem.Del(ref _uiShip);
			Mem.Del(ref _uiFeatherPar);
			Mem.Del(ref _uiLightPar);
			Mem.Del(ref _uiPetalPar1);
			Mem.Del(ref _btnAnime);
			Mem.Del(ref _ringAnime);
			Mem.Del(ref _anime);
			Mem.Del(ref _blurCamera);
		}

		private void Update()
		{
			if (isControl && _keyController.keyState[1].down)
			{
				isControl = false;
				ButtonClick();
			}
		}

		public void Initialize(ShipModelMst model, KeyControl kCtrl, Action callback)
		{
			init();
			_callback = callback;
			_shipModelMst = model;
			_keyController = kCtrl;
			setShipTexture();
		}

		public void Initialize(int graphicShipId, KeyControl kCtrl, Action callback)
		{
			init();
			_callback = callback;
			_shipModelMst = new ShipModelMst(graphicShipId);
			_keyController = kCtrl;
			setShipTexture();
		}

		public IEnumerator Play()
		{
			GetComponent<UIPanel>().alpha = 1f;
			_anime.Stop();
			_anime.Play("Marriage1");
			SoundUtils.PlayBGM(BGMFileInfos.Kekkon, isLoop: false);
			yield return new WaitForEndOfFrame();
		}

		private void setShipTexture()
		{
			int num = (_shipModelMst == null) ? 1 : _shipModelMst.GetGraphicsMstId();
			_uiShip.mainTexture = ShipUtils.LoadTexture(num, 9);
			_uiShip.MakePixelPerfect();
			_uiShip.transform.localPosition = Util.Poi2Vec(new ShipOffset(num).GetFace(damaged: false));
		}

		private void _setShipPosition(int index)
		{
			switch (index)
			{
			case 0:
				_uiShip.transform.localPosition = Util.Poi2Vec(_shipModelMst.Offsets.GetFoot_InBattle(damaged: false));
				break;
			case 1:
				_uiShip.transform.localPosition = Util.Poi2Vec(new ShipOffset(_shipModelMst.GetGraphicsMstId()).GetFace(damaged: false));
				break;
			case 2:
				_uiShip.transform.localPosition = Util.Poi2Vec(new ShipOffset(_shipModelMst.GetGraphicsMstId()).GetFace(damaged: false));
				break;
			}
		}

		private void playFeatherParticle()
		{
			_uiFeatherPar.Stop();
			_uiFeatherPar.Play();
		}

		private void playPetalParticle1()
		{
			_uiPetalPar1.Stop();
			_uiPetalPar1.Play();
		}

		private void playPetalParticle2()
		{
			_uiPetalPar2.Stop();
			_uiPetalPar2.Play();
		}

		private void playShipVoice()
		{
			ShipUtils.PlayShipVoice(_shipModelMst, 24);
		}

		private void playUpDownLoopAnime()
		{
			_ringAnime.Stop();
			_ringAnime.Play("MarriageUpDown");
		}

		private void _debugShipPos()
		{
			Debug.Log("ShipID:" + _debugIndex);
			if (Mst_DataManager.Instance.Mst_shipgraph.ContainsKey(_debugIndex))
			{
				IReward_Ship reward_Ship = new Reward_Ship(_debugIndex);
				_uiShip.mainTexture = SingletonMonoBehaviour<ResourceManager>.Instance.ShipTexture.Load(reward_Ship.Ship.GetGraphicsMstId(), 9);
				_uiShip.MakePixelPerfect();
				_uiShip.transform.localPosition = Util.Poi2Vec(new ShipOffset(reward_Ship.Ship.GetGraphicsMstId()).GetShipDisplayCenter(damaged: false));
				_uiShip.alpha = 1f;
			}
		}

		private void sceneFinished(int index)
		{
			_anime.Stop();
			switch (index)
			{
			case 1:
				_uiFeatherPar.Stop();
				_blurCamera.SetActive(isActive: false);
				_uiLightPar.Stop();
				_uiLightPar.Play();
				_anime.Play("Marriage2");
				break;
			case 2:
				_objScene3.SetActive(true);
				_uiLightPar.Stop();
				_uiLightPar.Play();
				_anime.Play("Marriage3");
				break;
			case 3:
				_objScene2.SetActive(false);
				_anime.Play("Marriage4");
				break;
			case 4:
			{
				UITexture component = ((Component)base.transform.FindChild("Camera/Vignette")).GetComponent<UITexture>();
				component.alpha = 0f;
				_objScene5.SetActive(true);
				_objScene2.SetActive(true);
				_anime.Play("Marriage5");
				break;
			}
			case 5:
				_objScene5.SetActive(false);
				((Component)_ringAnime).gameObject.SetActive(false);
				_uiLightPar.Stop();
				_uiBg.mainTexture = (Resources.Load("Textures/Marriage/bg_seane6") as Texture2D);
				_anime.Play("Marriage6");
				break;
			case 6:
				isControl = true;
				((Component)_btnAnime).transform.localPosition = new Vector3(410f, -200f, 0f);
				_btnAnime.Stop();
				_btnAnime.Play("NextIcon");
				break;
			}
		}

		public void ButtonClick()
		{
			GetComponent<UIPanel>().gameObject.SafeGetTweenAlpha(1f, 0f, 4f, 1f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
			if (_callback != null)
			{
				_callback();
			}
			isControl = false;
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		private void _finishedMarriage()
		{
		}
	}
}
