using KCV.Utils;
using local.managers;
using local.models.battle;
using System;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdMapOpen : BaseAnimation
	{
		[SerializeField]
		private UITexture _uiMapImage;

		[SerializeField]
		private UITexture _uiMapOld;

		[SerializeField]
		private UISprite _uiMapIcon;

		[SerializeField]
		private UITexture _uiText;

		[SerializeField]
		private UISprite _uiNextIcon;

		[SerializeField]
		private ParticleSystem _uiLightPar;

		[SerializeField]
		private ParticleSystem _uiBackLightPar;

		[SerializeField]
		private Animation _anime;

		[SerializeField]
		private Animation _gearAnime;

		private int _nMapId;

		private int _nAreaId;

		private int[] _openMapIDs;

		private bool _isControl;

		private bool[] _isOpenMap;

		private KeyControl _keyControl;

		private BattleResultModel _resultModel;

		private void _init()
		{
			_isControl = false;
			_isFinished = false;
			_nAreaId = 1;
			_nMapId = 1;
			_openMapIDs = null;
			Util.FindParentToChild(ref _uiMapImage, base.transform, "MapImage");
			Util.FindParentToChild(ref _uiMapOld, base.transform, "MapImageOld");
			Util.FindParentToChild(ref _uiMapIcon, base.transform, "MapIcon");
			Util.FindParentToChild(ref _uiText, base.transform, "Text");
			Util.FindParentToChild(ref _uiNextIcon, base.transform, "NextIcon");
			Util.FindParentToChild<ParticleSystem>(ref _uiLightPar, base.transform, "Light1");
			Util.FindParentToChild<ParticleSystem>(ref _uiBackLightPar, base.transform, "BackLight");
			if ((UnityEngine.Object)_anime == null)
			{
				_anime = GetComponent<Animation>();
			}
			if ((UnityEngine.Object)_gearAnime == null)
			{
				_gearAnime = _uiNextIcon.GetComponent<Animation>();
			}
			((Component)_uiBackLightPar).SetActive(isActive: false);
			_uiNextIcon.alpha = 0f;
			_anime.Stop();
			_gearAnime.Stop();
			UIButtonMessage component = _uiNextIcon.GetComponent<UIButtonMessage>();
			component.target = base.gameObject;
			component.functionName = "_nextIconEL";
			component.trigger = UIButtonMessage.Trigger.OnClick;
		}

		private new void OnDestroy()
		{
			Mem.Del(ref _uiMapImage);
			Mem.Del(ref _uiMapOld);
			Mem.Del(ref _uiMapIcon);
			Mem.Del(ref _uiText);
			Mem.Del(ref _uiLightPar);
			Mem.Del(ref _uiBackLightPar);
			Mem.Del(ref _anime);
			Mem.Del(ref _gearAnime);
			Mem.Del(ref _openMapIDs);
			_keyControl = null;
			_resultModel = null;
		}

		public bool Run()
		{
			if (_isControl && _keyControl.keyState[1].down)
			{
				_nextIconEL(null);
			}
			if (_isFinished)
			{
				return true;
			}
			return false;
		}

		private void SetMapTexture()
		{
			string path = "Textures/Strategy/MapSelectGraph/stage" + _nAreaId + "-" + _nMapId;
			_uiMapImage.mainTexture = (Resources.Load(path) as Texture2D);
		}

		private void SetNextMapTexture()
		{
			_uiMapOld.mainTexture = _uiMapImage.mainTexture;
			string path = "Textures/Strategy/MapSelectGraph/stage" + _nAreaId + "-" + _nMapId;
			_uiMapImage.mainTexture = (Resources.Load(path) as Texture2D);
		}

		private void _setOpen(bool isOpen)
		{
			if (isOpen)
			{
				_uiMapIcon.transform.gameObject.SetActive(false);
			}
			else
			{
				_uiMapIcon.transform.gameObject.SetActive(true);
			}
		}

		private void SetNextOpenMap()
		{
			bool flag = false;
			for (int i = 0; i < _openMapIDs.Length; i++)
			{
				if (_isOpenMap[i])
				{
					_isOpenMap[i] = false;
					flag = true;
					GetOpenIds(_openMapIDs[i]);
					_setOpen(isOpen: false);
					break;
				}
			}
			_animAnimation.Stop();
			if (!flag)
			{
				_animAnimation.Play("MapOpenEnd");
				return;
			}
			SetNextMapTexture();
			_animAnimation.Play("MapOpenNext");
		}

		private void GetOpenIds(int id)
		{
			int num = 0;
			if (id >= 100)
			{
				int num2 = 0;
				for (int i = 0; i < 10; i++)
				{
					if (id >= 100 + 100 * i)
					{
						num2++;
					}
				}
				int num3 = num2 * 100 + 11;
				for (int j = 0; j < 10; j++)
				{
					if (id >= num3 + 10 * j)
					{
						num++;
					}
				}
				_nMapId = id - num2 * 100 - num * 10;
			}
			else
			{
				for (int k = 0; k < 10; k++)
				{
					if (id >= 11 + 10 * k)
					{
						num++;
					}
				}
				_nMapId = id - num * 10;
			}
			_nAreaId = (id - _nMapId) / 10;
		}

		public override void Play(Action callback)
		{
			this.SetActive(isActive: true);
			_actCallback = callback;
			_animAnimation.Stop();
			_animAnimation.Play("MapOpen");
		}

		private void _nextIconEL(GameObject obj)
		{
			if (_isControl)
			{
				SetNextOpenMap();
				_uiLightPar.Stop();
				((Component)_uiLightPar).SetActive(isActive: false);
				_uiBackLightPar.Stop();
				((Component)_uiBackLightPar).SetActive(isActive: false);
				_uiNextIcon.alpha = 0f;
				_gearAnime.Stop();
				_isControl = false;
			}
		}

		private void _startParticle()
		{
			_uiLightPar.Play();
		}

		private void _startControl()
		{
			_isControl = true;
		}

		private void _onStartAnimationEnd()
		{
			_isControl = true;
			SoundUtils.PlaySE(SEFIleInfos.SE_925);
			_animAnimation.Stop();
			((Component)_uiLightPar).SetActive(isActive: true);
			_uiLightPar.Play();
			((Component)_uiBackLightPar).SetActive(isActive: true);
			_uiBackLightPar.Play();
			_uiNextIcon.alpha = 1f;
			_gearAnime.Play("NextIcon");
		}

		private void _onCompEndAnimation()
		{
			_onFinishedMapOpen();
		}

		private void _onFinishedMapOpen()
		{
			if (_actCallback != null)
			{
				_actCallback();
			}
			_isFinished = true;
		}

		public static ProdMapOpen Instantiate(ProdMapOpen prefab, BattleResultModel resultModel, Transform parent, KeyControl keyControl, MapManager mapManager, int nPanelDepth)
		{
			ProdMapOpen prodMapOpen = UnityEngine.Object.Instantiate(prefab);
			prodMapOpen.transform.parent = parent;
			prodMapOpen.transform.localScale = Vector3.one;
			prodMapOpen.transform.localPosition = Vector3.zero;
			prodMapOpen._init();
			prodMapOpen._keyControl = keyControl;
			prodMapOpen._resultModel = resultModel;
			prodMapOpen._openMapIDs = prodMapOpen._resultModel.NewOpenMapIDs;
			prodMapOpen._isOpenMap = new bool[prodMapOpen._resultModel.NewOpenMapIDs.Length];
			for (int i = 0; i < prodMapOpen._resultModel.NewOpenMapIDs.Length; i++)
			{
				prodMapOpen._isOpenMap[i] = true;
			}
			for (int j = 0; j < prodMapOpen._resultModel.NewOpenMapIDs.Length; j++)
			{
				if (prodMapOpen._isOpenMap[j])
				{
					prodMapOpen._isOpenMap[j] = false;
					prodMapOpen.GetOpenIds(prodMapOpen._resultModel.NewOpenMapIDs[j]);
					break;
				}
			}
			prodMapOpen.SetMapTexture();
			return prodMapOpen;
		}

		public static ProdMapOpen Instantiate(ProdMapOpen prefab, int[] NewOpenAreaIDs, int[] NewOpenMapIDs, Transform parent, KeyControl keyControl, int nPanelDepth)
		{
			ProdMapOpen prodMapOpen = UnityEngine.Object.Instantiate(prefab);
			prodMapOpen.transform.parent = parent;
			prodMapOpen.transform.localScale = Vector3.one;
			prodMapOpen.transform.localPosition = Vector3.zero;
			prodMapOpen._init();
			prodMapOpen._keyControl = keyControl;
			prodMapOpen._openMapIDs = NewOpenMapIDs;
			prodMapOpen._isOpenMap = new bool[NewOpenMapIDs.Length];
			for (int i = 0; i < prodMapOpen._openMapIDs.Length; i++)
			{
				prodMapOpen._isOpenMap[i] = true;
			}
			for (int j = 0; j < prodMapOpen._openMapIDs.Length; j++)
			{
				if (prodMapOpen._isOpenMap[j])
				{
					prodMapOpen._isOpenMap[j] = false;
					prodMapOpen.GetOpenIds(prodMapOpen._openMapIDs[j]);
					break;
				}
			}
			prodMapOpen.SetMapTexture();
			prodMapOpen.SetActive(isActive: false);
			return prodMapOpen;
		}
	}
}
