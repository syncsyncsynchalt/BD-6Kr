using Common.Enum;
using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdTorpedoResucueCutIn : MonoBehaviour
	{
		private enum ResucueAnimationList
		{
			None,
			RescueCutIn1,
			RescueCutInFriend,
			RescueCutInEnemy
		}

		private Dictionary<FleetType, bool> _dicIsCutIn;

		private Dictionary<FleetType, UITexture> _listFlagShipTex;

		[SerializeField]
		private List<UITexture> _listShipFriend;

		[SerializeField]
		private List<UITexture> _listShipEnemy;

		[SerializeField]
		private List<ShipModel_Defender> _listShipDefenderF;

		[SerializeField]
		private List<ShipModel_Defender> _listShipDefenderE;

		private Dictionary<FleetType, UITexture> _dicOverlay;

		[SerializeField]
		private Animation _anime;

		private Action _actCallback;

		private ResucueAnimationList _iList;

		private int _count;

		private int _countF;

		private int _countE;

		private bool _isShake;

		private float _fPower = 0.1f;

		private Vector3 _vBasePosition;

		private Quaternion _qBaseRotation;

		private float _fShakeDecay = 0.002f;

		private void Awake()
		{
			_init();
		}

		public void _init()
		{
			_iList = ResucueAnimationList.None;
			_dicIsCutIn = new Dictionary<FleetType, bool>();
			_dicIsCutIn.Add(FleetType.Friend, value: false);
			_dicIsCutIn.Add(FleetType.Enemy, value: false);
			int num = 5;
			_listShipFriend = new List<UITexture>();
			_listShipFriend.Capacity = num;
			Transform transform = base.transform.FindChild("FriendShip");
			for (int i = 0; i < num; i++)
			{
				_listShipFriend.Add(((Component)transform.FindChild("Anchor" + (i + 1) + "/Ship")).GetComponent<UITexture>());
			}
			_listShipEnemy = new List<UITexture>();
			Transform transform2 = base.transform.FindChild("EnemyShip");
			for (int j = 0; j < num; j++)
			{
				_listShipEnemy.Add(((Component)transform2.FindChild("Anchor" + (j + 1) + "/Ship")).GetComponent<UITexture>());
			}
			_listShipDefenderF = new List<ShipModel_Defender>();
			_listShipDefenderE = new List<ShipModel_Defender>();
			if (_listFlagShipTex == null)
			{
				_listFlagShipTex = new Dictionary<FleetType, UITexture>();
				_listFlagShipTex.Add(FleetType.Friend, ((Component)base.transform.FindChild("FriendShip/AnchorFlag/Ship")).GetComponent<UITexture>());
				_listFlagShipTex.Add(FleetType.Enemy, ((Component)base.transform.FindChild("EnemyShip/AnchorFlag/Ship")).GetComponent<UITexture>());
			}
			if (_dicOverlay == null)
			{
				_dicOverlay = new Dictionary<FleetType, UITexture>();
				_dicOverlay.Add(FleetType.Friend, ((Component)base.transform.FindChild("FriendShip/Overlay")).GetComponent<UITexture>());
				_dicOverlay.Add(FleetType.Enemy, ((Component)base.transform.FindChild("EnemyShip/Overlay")).GetComponent<UITexture>());
			}
			if ((UnityEngine.Object)_anime == null)
			{
				_anime = ((Component)base.transform).GetComponent<Animation>();
			}
		}

		private void OnDestroy()
		{
			if (_listFlagShipTex != null)
			{
				_listFlagShipTex.Clear();
			}
			_listFlagShipTex = null;
			_actCallback = null;
		}

		private void Update()
		{
			if (!_isShake)
			{
			}
		}

		public void SetFlagShipInfo(FleetType type, ShipModel_BattleAll flagShip)
		{
			bool flag = false;
			if (flagShip.DmgStateEnd == DamageState_Battle.Tyuuha || flagShip.DmgStateEnd == DamageState_Battle.Taiha || flagShip.DmgStateEnd == DamageState_Battle.Gekichin)
			{
				flag = true;
			}
			_listFlagShipTex[type].mainTexture = ShipUtils.LoadTexture(flagShip, flag);
			_listFlagShipTex[type].MakePixelPerfect();
			_listFlagShipTex[type].transform.localPosition = ShipUtils.GetShipOffsPos(flagShip, flag, MstShipGraphColumn.CutIn);
			_dicIsCutIn[type] = true;
		}

		public void AddShipInfo(FleetType type, ShipModel_Defender ship)
		{
			bool isAfter = false;
			if (ship.DmgStateBefore == DamageState_Battle.Tyuuha || ship.DmgStateBefore == DamageState_Battle.Taiha || ship.DmgStateBefore == DamageState_Battle.Gekichin)
			{
				isAfter = true;
			}
			if (type == FleetType.Friend)
			{
				_listShipDefenderF.Add(ship);
				_countF = _listShipDefenderF.Count;
				_listShipFriend[_countF - 1].mainTexture = ShipUtils.LoadTexture(ship, isAfter);
				_listShipFriend[_countF - 1].MakePixelPerfect();
				_listShipFriend[_countF - 1].transform.localPosition = ShipUtils.GetShipOffsPos(ship, ship.DmgStateBefore, MstShipGraphColumn.CutIn);
			}
			else
			{
				_listShipDefenderE.Add(ship);
				_countE = _listShipDefenderE.Count;
				_listShipEnemy[_countE - 1].mainTexture = ShipUtils.LoadTexture(ship, isAfter);
				_listShipEnemy[_countE - 1].MakePixelPerfect();
				_listShipEnemy[_countE - 1].transform.localPosition = ShipUtils.GetShipOffsPos(ship, ship.DmgStateBefore, MstShipGraphColumn.CutIn);
			}
			_dicIsCutIn[type] = true;
		}

		public void Play(Action callback)
		{
			base.transform.localScale = Vector3.one;
			_actCallback = callback;
			if (_countF >= _countE)
			{
				_count = _countF;
			}
			else
			{
				_count = _countE;
			}
			if (_count == 0)
			{
				_cutinFinished();
				return;
			}
			if (_countF > 0)
			{
				_dicOverlay[FleetType.Friend].SafeGetTweenAlpha(0f, 1f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
				_listFlagShipTex[FleetType.Friend].SafeGetTweenAlpha(0f, 1f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
			}
			else
			{
				_listFlagShipTex[FleetType.Friend].mainTexture = null;
			}
			if (_countE > 0)
			{
				_dicOverlay[FleetType.Enemy].SafeGetTweenAlpha(0f, 1f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
				_listFlagShipTex[FleetType.Enemy].SafeGetTweenAlpha(0f, 1f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
			}
			else
			{
				_listFlagShipTex[FleetType.Enemy].mainTexture = null;
			}
			_anime.Play(ResucueAnimationList.RescueCutIn1.ToString());
		}

		private void _onAnimetionFinished(int index)
		{
			if (_count == index)
			{
				if (_countF > 0)
				{
					_dicOverlay[FleetType.Friend].SafeGetTweenAlpha(1f, 0f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
					_listFlagShipTex[FleetType.Friend].SafeGetTweenAlpha(1f, 0f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
				}
				if (_countE > 0)
				{
					_dicOverlay[FleetType.Enemy].SafeGetTweenAlpha(1f, 0f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
					_listFlagShipTex[FleetType.Enemy].SafeGetTweenAlpha(1f, 0f, 0.4f, 0f, UITweener.Method.Linear, UITweener.Style.Once, base.gameObject, string.Empty);
				}
				_cutinFinished();
			}
		}

		private void onPlaySeAnime(int seNo)
		{
			switch (seNo)
			{
			}
		}

		private void startSplashParticle()
		{
			_isShake = true;
			_vBasePosition = base.transform.localPosition;
			_qBaseRotation = base.transform.localRotation;
		}

		private void _cutinFinished()
		{
			_actCallback();
		}

		public static ProdTorpedoResucueCutIn Instantiate(ProdTorpedoResucueCutIn prefab, RaigekiModel model, Transform parent)
		{
			ProdTorpedoResucueCutIn prodTorpedoResucueCutIn = UnityEngine.Object.Instantiate(prefab);
			prodTorpedoResucueCutIn.transform.parent = parent;
			prodTorpedoResucueCutIn.transform.localPosition = Vector3.zero;
			prodTorpedoResucueCutIn.transform.localScale = Vector3.one;
			return prodTorpedoResucueCutIn;
		}
	}
}
