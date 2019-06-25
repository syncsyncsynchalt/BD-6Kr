using local.models;
using local.utils;
using LT.Tweening;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KCV.Strategy
{
	public class MapTransitionCutManager : SingletonMonoBehaviour<MapTransitionCutManager>
	{
		private const int TRANSITION_CLOUD_MAX = 40;

		[SerializeField]
		private UITexture _uiSortieMapBackground;

		[SerializeField]
		private UITexture _uiBackground;

		[SerializeField]
		private UITexture _uiOverlay;

		[SerializeField]
		private UITexture _uiSortieLabel;

		[SerializeField]
		private ParticleSystem _psCloud;

		private AsyncOperation _asyncOperation;

		private GameObject cam;

		private Transform _prefabAreaMap;

		private bool _isWait;

		private Animation _anim;

		private static readonly int[] NeedPlaneCellAreaNo = new int[14]
		{
			44,
			93,
			122,
			123,
			124,
			142,
			152,
			153,
			154,
			161,
			162,
			163,
			164,
			172
		};

		private new Animation animation => this.GetComponentThis(ref _anim);

		protected override void Awake()
		{
			((Component)_psCloud).SetActive(isActive: false);
		}

		private void OnDestroy()
		{
			Mem.Del(ref _uiSortieMapBackground);
			Mem.Del(ref _uiBackground);
			Mem.Del(ref _uiOverlay);
			Mem.Del(ref _uiSortieLabel);
			Mem.Del(ref _psCloud);
			Mem.Del(ref _asyncOperation);
			Mem.Del(ref cam);
			Mem.Del(ref _isWait);
			Mem.Del(ref _anim);
			Mem.Del(ref _prefabAreaMap);
		}

		private void Update()
		{
			if (_isWait && _asyncOperation.progress >= 0.9f)
			{
				_isWait = false;
				UnityEngine.Object.DontDestroyOnLoad(cam);
				_asyncOperation.allowSceneActivation = true;
			}
		}

		public void Discard(Action onFinished)
		{
			base.transform.LTValue(1f, 0f, 0.5f).setOnUpdate(delegate(float x)
			{
				_uiBackground.alpha = x;
				_uiSortieMapBackground.alpha = x;
			}).setOnComplete((Action)delegate
			{
				Dlg.Call(ref onFinished);
				UnityEngine.Object.Destroy(cam);
			});
		}

		public void Initialize(MapModel mapModel, AsyncOperation async)
		{
			cam = UnityEngine.Object.Instantiate(base.transform.parent.Find("TopCamera").gameObject);
			cam.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
			cam.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
			cam.GetComponent<Camera>().depth = 99f;
			base.transform.parent = cam.transform;
			base.transform.localPosition = new Vector3(-26.4f, -43f, 496.4f);
			base.transform.localScale = Vector3.one;
			cam.transform.localPosition = 1000f * Vector3.right;
			_asyncOperation = async;
			Observable.FromCoroutine(() => PlayAnimation(mapModel, async)).Subscribe();
		}

		private IEnumerator PlayAnimation(MapModel mapModel, AsyncOperation async)
		{
			_uiSortieMapBackground.alpha = 0f;
			_uiBackground.alpha = 0f;
			_uiOverlay.alpha = 0f;
			_uiSortieLabel.alpha = 0f;
			_isWait = false;
			_uiSortieMapBackground.mainTexture = (Resources.Load(string.Format("Textures/SortieMap/MapBG/{0}/{0}{1}", mapModel.AreaId, mapModel.No)) as Texture);
			CreatePlaneCellPrefab(mapModel);
			_prefabAreaMap = (ResourceManager.LoadResourceOrAssetBundle($"Prefabs/SortieMap/AreaMap/Map{mapModel.MstId}") as Transform);
			yield return new WaitForFixedUpdate();
			Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate
			{
				((Component)this._psCloud).SetActive(isActive: true);
				this._psCloud.Play();
			});
			animation.Play("MapTransitionCut");
			yield return new WaitForSeconds(animation["MapTransitionCut"].length);
			_isWait = true;
		}

		public Transform GetPrefabAreaMap()
		{
			return _prefabAreaMap;
		}

		private void CreatePlaneCellPrefab(MapModel mapModel)
		{
			bool flag = false;
			for (int i = 0; i < NeedPlaneCellAreaNo.Length; i++)
			{
				if (NeedPlaneCellAreaNo[i] == mapModel.MstId)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				Util.InstantiatePrefab("SortieMap/AreaMap/Map" + mapModel.MstId + "_PlaneCell", _uiSortieMapBackground.gameObject);
			}
		}

		private void CheckTrophy()
		{
			TrophyUtil.Unlock_At_MapStart();
		}
	}
}
