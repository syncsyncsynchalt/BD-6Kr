using Common.Enum;
using KCV.Battle.Utils;
using local.managers;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleShips : IDisposable
	{
		private bool _isInitialize;

		private bool _isMakeRadar;

		private bool _isRadarDeployed;

		private Transform _traFriendFleetAnchor;

		private Transform _traEnemyFleetAnchor;

		private UIBattleShip _uiOriginalShip;

		private Dictionary<int, UIBattleShip> _dicFriendBattleShips;

		private Dictionary<int, UIBattleShip> _dicEnemyBattleShips;

		private List<UIBufferFleetCircle> _listBufferFleetCircle;

		private Dictionary<int, List<UIBufferCircle>> _dicBufferShipCircle;

		public bool isInitialize => _isInitialize;

		public bool isRadarDeployed => _isRadarDeployed;

		public Dictionary<int, UIBattleShip> dicFriendBattleShips
		{
			get
			{
				return _dicFriendBattleShips;
			}
			set
			{
				_dicFriendBattleShips = value;
			}
		}

		public Dictionary<int, UIBattleShip> dicEnemyBattleShips
		{
			get
			{
				return _dicEnemyBattleShips;
			}
			set
			{
				_dicEnemyBattleShips = value;
			}
		}

		public UIBattleShip flagShipFriend => _dicFriendBattleShips[0];

		public UIBattleShip flagShipEnemy => _dicEnemyBattleShips[0];

		public UIBattleShip lastIndexShipFriend => (from order in _dicFriendBattleShips
			orderby order.Value.shipModel.Index
			select order).Last().Value;

		public UIBattleShip lastIndexShipEnemy => (from order in dicEnemyBattleShips
			orderby order.Value.shipModel.Index
			select order).Last().Value;

		public List<UIBufferFleetCircle> bufferFleetCircle => _listBufferFleetCircle;

		public Dictionary<int, List<UIBufferCircle>> bufferShipCirlce => _dicBufferShipCircle;

		public BattleShips()
		{
			_uiOriginalShip = ((Component)BattleTaskManager.GetPrefabFile().prefabBattleShip).GetComponent<UIBattleShip>();
			_isInitialize = false;
			_isMakeRadar = false;
		}

		public void Dispose()
		{
			Mem.Del(ref _isInitialize);
			Mem.Del(ref _isMakeRadar);
			Mem.Del(ref _isRadarDeployed);
			Mem.Del(ref _traFriendFleetAnchor);
			Mem.Del(ref _traEnemyFleetAnchor);
			Mem.Del(ref _uiOriginalShip);
			if (_listBufferFleetCircle != null)
			{
				_listBufferFleetCircle.ForEach(delegate(UIBufferFleetCircle x)
				{
					if (x != null)
					{
						UnityEngine.Object.Destroy(x.gameObject);
					}
				});
			}
			if (_dicBufferShipCircle != null)
			{
				_dicBufferShipCircle[0].ForEach(delegate(UIBufferCircle x)
				{
					if (x != null)
					{
						UnityEngine.Object.Destroy(x.gameObject);
					}
				});
				_dicBufferShipCircle[1].ForEach(delegate(UIBufferCircle x)
				{
					if (x != null)
					{
						UnityEngine.Object.Destroy(x.gameObject);
					}
				});
			}
			Mem.DelListSafe(ref _listBufferFleetCircle);
			Mem.DelDictionarySafe(ref _dicBufferShipCircle);
			if (_dicFriendBattleShips != null)
			{
				_dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
				{
					if (x.Value != null)
					{
						x.Value.gameObject.Discard();
					}
				});
			}
			Mem.DelDictionary(ref _dicFriendBattleShips);
			if (_dicEnemyBattleShips != null)
			{
				_dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
				{
					if (x.Value != null)
					{
						x.Value.gameObject.Discard();
					}
				});
			}
			Mem.DelDictionary(ref _dicEnemyBattleShips);
		}

		public bool Init(BattleManager manager)
		{
			if (manager == null)
			{
				return false;
			}
			_traFriendFleetAnchor = GameObject.Find("FriendFleetAnchor").transform;
			_traEnemyFleetAnchor = GameObject.Find("EnemyFleetAnchor").transform;
			_dicFriendBattleShips = new Dictionary<int, UIBattleShip>();
			_dicEnemyBattleShips = new Dictionary<int, UIBattleShip>();
            UniRx.IObservable<Unit> source = Observable.FromCoroutine(() => CreateBattleShips(manager.Ships_f, manager.FormationId_f, _traFriendFleetAnchor.gameObject, _dicFriendBattleShips, FleetType.Friend, manager.ShipCount_f, 0));
            UniRx.IObservable<Unit> other = Observable.FromCoroutine(() => CreateBattleShips(manager.Ships_e, manager.FormationId_e, _traEnemyFleetAnchor.gameObject, _dicEnemyBattleShips, FleetType.Enemy, manager.ShipCount_e, 0));
			source.SelectMany(other).Subscribe(delegate
			{
				_uiOriginalShip = null;
				_isInitialize = true;
			});
			return true;
		}

		public bool Update()
		{
			if (isRadarDeployed)
			{
				bufferShipCirlce[0].ForEach(delegate(UIBufferCircle x)
				{
					x.Run();
				});
			}
			return true;
		}

		private IEnumerator CreateBattleShips(ShipModel_BattleAll[] ships, BattleFormationKinds1 iKind, GameObject parent, Dictionary<int, UIBattleShip> dic, FleetType iType, int fleetNum, int combineNum)
		{
			BattleFieldCamera cam = BattleTaskManager.GetBattleCameras().fieldCameras[0];
			Vector3[] formationPos = BattleDefines.FORMATION_POSITION[iKind][fleetNum];
			Vector3[] oneRowPos = BattleDefines.FORMATION_POSITION[BattleFormationKinds1.TanOu][fleetNum];
			int index = 0;
			foreach (ShipModel_BattleAll model in ships)
			{
				if (model == null)
				{
					continue;
				}
				UIBattleShip ship;
				if (_traFriendFleetAnchor.transform.FindChild(string.Format("{1}FleetAnchor/BattleShip{0}", index + 1, iType)) != null)
				{
					ship = ((Component)_traFriendFleetAnchor.transform.FindChild(string.Format("{1}FleetAnchor/BattleShip{0}", index + 1, iType))).GetComponent<UIBattleShip>();
				}
				else
				{
					ship = UIBattleShip.Instantiate(_uiOriginalShip.GetComponent<UIBattleShip>(), parent.transform);
					ship.name = $"BattleShip{model.Index}";
					Vector3 pos;
					Vector3 lPos;
					Vector3 commandBufferPos;
					Vector3 advancePos;
					if (index < formationPos.Length)
					{
						pos = ((iType != 0) ? (-formationPos[index] * 1f) : (formationPos[index] * 1f));
						lPos = oneRowPos[index] * 1f;
						commandBufferPos = ((iType != 0) ? (-formationPos[index] * 4f) : (formationPos[index] * 4f));
						advancePos = ((iType != 0) ? (lPos + Vector3.forward * 5f) : (lPos + Vector3.back * 5f));
					}
					else
					{
						pos = Vector3.zero;
						lPos = Vector3.zero;
						commandBufferPos = Vector3.zero;
						advancePos = Vector3.zero;
					}
					ship.transform.localPosition = lPos;
					ship.dicStandingPos[StandingPositionType.Formation] = pos;
					ship.dicStandingPos[StandingPositionType.OneRow] = lPos;
					ship.dicStandingPos[StandingPositionType.CommandBuffer] = commandBufferPos;
					ship.dicStandingPos[StandingPositionType.Advance] = advancePos;
					ship.billboard.billboardTarget = cam.transform;
					ship.billboard.isBillboard = true;
					ship.billboard.isEnableVerticalRotation = false;
					ship.drawType = ShipDrawType.Normal;
				}
				dic.Add(index, ship);
				dic[index].SetShipInfos(model, isStart: true);
				if (model.IsEscape())
				{
					dic[index].SetActive(isActive: false);
				}
				index++;
				yield return null;
			}
			yield return null;
		}

		public void UpdateDamageAll(IBattlePhase iPhase, bool isFriend)
		{
			List<ShipModel_Defender> defenders = iPhase.GetDefenders(isFriend);
			defenders.ForEach(delegate(ShipModel_Defender x)
			{
				if (isFriend)
				{
					dicFriendBattleShips[x.Index].UpdateDamage(x);
				}
				else
				{
					dicEnemyBattleShips[x.Index].UpdateDamage(x);
				}
			});
		}

		public void UpdateDamageAll(IBattlePhase iPhase)
		{
			UpdateDamageAll(iPhase, isFriend: true);
			UpdateDamageAll(iPhase, isFriend: false);
		}

		public void Restored(ShipModel_Defender defender)
		{
			UIBattleShip uIBattleShip = (!defender.IsFriend()) ? _dicEnemyBattleShips[defender.Index] : _dicFriendBattleShips[defender.Index];
			uIBattleShip.SetActive(isActive: true);
			uIBattleShip.Restored(defender);
		}

		public void SetStandingPosition(StandingPositionType iType)
		{
			_dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.Value != null)
				{
					x.Value.SetStandingPosition(iType);
				}
			});
			_dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.Value != null)
				{
					x.Value.SetStandingPosition(iType);
				}
			});
		}

		public void SetShipDrawType(ShipDrawType iType)
		{
			SetShipDrawType(FleetType.Friend, iType);
			SetShipDrawType(FleetType.Enemy, iType);
		}

		public void SetShipDrawType(FleetType iFleet, ShipDrawType iDrawType)
		{
			switch (iFleet)
			{
			case FleetType.Friend:
				_dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
				{
					if (x.Value != null)
					{
						x.Value.drawType = iDrawType;
					}
				});
				break;
			case FleetType.Enemy:
				_dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
				{
					if (x.Value != null)
					{
						x.Value.drawType = iDrawType;
					}
				});
				break;
			}
		}

		public void SetLayer(Generics.Layers iLayer)
		{
			_dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.Value != null && x.Value.layer != iLayer)
				{
					x.Value.layer = iLayer;
				}
			});
			_dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.Value != null && x.Value.layer != iLayer)
				{
					x.Value.layer = iLayer;
				}
			});
		}

		public void SetBollboardTarget(Transform target)
		{
			SetBollboardTarget(isFriend: true, target);
			SetBollboardTarget(isFriend: false, target);
		}

		public void SetBollboardTarget(bool isFriend, Transform target)
		{
			if (isFriend)
			{
				_dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
				{
					if (x.Value != null)
					{
						x.Value.billboard.billboardTarget = target;
					}
				});
			}
			else
			{
				_dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
				{
					if (x.Value != null)
					{
						x.Value.billboard.billboardTarget = target;
					}
				});
			}
		}

		public void RadarDeployment(bool isDeploy)
		{
			if (!_isMakeRadar)
			{
				MakeRadar();
			}
			_listBufferFleetCircle.ForEach(delegate(UIBufferFleetCircle x)
			{
				if (isDeploy)
				{
					x.SetDefault();
				}
				x.SetActive(isDeploy);
			});
			_dicBufferShipCircle[0].ForEach(delegate(UIBufferCircle x)
			{
				if (isDeploy)
				{
					x.SetDefault();
				}
				x.SetActive(isDeploy);
			});
			_dicBufferShipCircle[1].ForEach(delegate(UIBufferCircle x)
			{
				if (isDeploy)
				{
					x.SetDefault();
				}
				x.SetActive(isDeploy);
			});
			if (isDeploy)
			{
				_dicBufferShipCircle[0].ForEach(delegate(UIBufferCircle x)
				{
					x.PlayGearAnimation();
				});
				_dicBufferShipCircle[1].ForEach(delegate(UIBufferCircle x)
				{
					x.PlayGearAnimation();
				});
			}
			else
			{
				_dicBufferShipCircle[0].ForEach(delegate(UIBufferCircle x)
				{
					x.StopGearAnimation();
				});
				_dicBufferShipCircle[1].ForEach(delegate(UIBufferCircle x)
				{
					x.StopGearAnimation();
				});
			}
			_isRadarDeployed = isDeploy;
		}

		private void MakeRadar()
		{
			BattleField field = BattleTaskManager.GetBattleField();
			BattlePefabFile prefabFile = BattleTaskManager.GetPrefabFile();
			Transform p = prefabFile.prefabUIBufferFleetCircle;
			_listBufferFleetCircle = new List<UIBufferFleetCircle>();
			foreach (int value in Enum.GetValues(typeof(FleetType)))
			{
				if (value != 2)
				{
					_listBufferFleetCircle.Add(UIBufferFleetCircle.Instantiate(((Component)p).GetComponent<UIBufferFleetCircle>(), field.dicFleetAnchor[(FleetType)value], (FleetType)value));
					_listBufferFleetCircle[value].transform.positionY(0.001f);
				}
			}
			Mem.Del(ref p);
			Transform prefab = prefabFile.prefabUIBufferShipCircle;
			_dicBufferShipCircle = new Dictionary<int, List<UIBufferCircle>>();
			int cnt = 0;
			List<UIBufferCircle> friendBufferCircle = new List<UIBufferCircle>();
			dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				friendBufferCircle.Add(UIBufferCircle.Instantiate(((Component)prefab).GetComponent<UIBufferCircle>(), x.Value.transform, FleetType.Friend, field.dicFleetAnchor[FleetType.Enemy]));
				cnt++;
			});
			_dicBufferShipCircle.Add(0, friendBufferCircle);
			cnt = 0;
			List<UIBufferCircle> enemyBufferCircle = new List<UIBufferCircle>();
			_dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				enemyBufferCircle.Add(UIBufferCircle.Instantiate(((Component)prefab).GetComponent<UIBufferCircle>(), x.Value.transform, FleetType.Enemy, field.dicFleetAnchor[FleetType.Friend]));
				cnt++;
			});
			_dicBufferShipCircle.Add(1, enemyBufferCircle);
			Mem.Del(ref prefab);
			_isMakeRadar = true;
		}

		public void SetSprayColor()
		{
			_dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.Value != null)
				{
					x.Value.SetSprayColor();
				}
			});
			_dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.Value != null)
				{
					x.Value.SetSprayColor();
				}
			});
		}

		public void SetTorpedoSalvoWakeAngle(bool isSet)
		{
			_dicEnemyBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.Value != null)
				{
					x.Value.TorpedoSalvoWakeAngle(isSet);
				}
			});
			_dicFriendBattleShips.ForEach(delegate(KeyValuePair<int, UIBattleShip> x)
			{
				if (x.Value != null)
				{
					x.Value.TorpedoSalvoWakeAngle(isSet);
				}
			});
		}
	}
}
