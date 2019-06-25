using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV
{
	public class AppInformation : SingletonMonoBehaviour<AppInformation>
	{
		public enum CurveType
		{
			Live2DEnter,
			_NUM
		}

		public enum LoadType
		{
			Ship,
			Yousei,
			White
		}

		[Serializable]
		public struct CommonCurve
		{
			public AnimationCurve curve;

			public CurveType type;
		}

		public Generics.Scene NextLoadScene;

		public LoadType NextLoadType;

		[SerializeField]
		private int _nCurrentAreaID;

		[SerializeField]
		private int _nCurrentPlayingBgmID = -1;

		[SerializeField]
		private DeckModel _clsCurrentDeck;

		[SerializeField]
		private MapAreaModel _areaModel;

		public bool SlogDraw;

		public int BattleCount;

		public CommonCurve[] curves;

		public static Dictionary<CurveType, AnimationCurve> curveDic;

		public int ReleaseSetNo;

		public int OpenAreaNum;

		public DeckModel[] prevStrategyDecks;

		public int CurrentAreaID
		{
			get
			{
				if (_clsCurrentDeck == null)
				{
					return 1;
				}
				return _clsCurrentDeck.AreaId;
			}
		}

		public MapAreaModel CurrentDeckAreaModel
		{
			get
			{
				return _areaModel;
			}
			set
			{
				_areaModel = value;
			}
		}

		public int FlagShipID => FlagShipModel.MstId;

		public ShipModel FlagShipModel
		{
			get
			{
				if (_clsCurrentDeck == null)
				{
					return null;
				}
				return _clsCurrentDeck.GetFlagShip();
			}
		}

		public DeckModel CurrentDeck
		{
			get
			{
				return _clsCurrentDeck;
			}
			set
			{
				_clsCurrentDeck = value;
			}
		}

		public int CurrentDeckID
		{
			get
			{
				if (_clsCurrentDeck == null)
				{
					return 1;
				}
				return _clsCurrentDeck.Id;
			}
		}

		public int currentPlayingBgmID
		{
			get
			{
				return _nCurrentPlayingBgmID;
			}
			set
			{
				_nCurrentPlayingBgmID = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			if (curveDic == null)
			{
				curveDic = new Dictionary<CurveType, AnimationCurve>();
			}
			if (curves != null)
			{
				for (int i = 0; i < curves.Length; i++)
				{
					if (!curveDic.ContainsKey(curves[i].type))
					{
						curveDic.Add(curves[i].type, curves[i].curve);
					}
				}
			}
			NextLoadType = LoadType.Ship;
			NextLoadScene = Generics.Scene.Scene_BEF;
		}

		public bool IsValidMoveToScene(Generics.Scene sceneType)
		{
			switch (sceneType)
			{
			case Generics.Scene.Repair:
			{
				int nDockMax = _areaModel.NDockMax;
				return IsValidMoveToRepairScene(nDockMax);
			}
			case Generics.Scene.Arsenal:
			{
				int currentAreaID = CurrentAreaID;
				return IsValidMoveToArsenalScene(currentAreaID);
			}
			default:
				return true;
			}
		}

		private bool IsValidMoveToRepairScene(int areaInDockCount)
		{
			return 0 < areaInDockCount;
		}

		private bool IsValidMoveToArsenalScene(int areaId)
		{
			int num = 1;
			return areaId == num;
		}

		public void FirstUpdateEnd()
		{
			StartCoroutine(FirstUpdateEndCoroutine());
		}

		private IEnumerator FirstUpdateEndCoroutine()
		{
			yield return new WaitForEndOfFrame();
			App.isFirstUpdate = false;
		}
	}
}
