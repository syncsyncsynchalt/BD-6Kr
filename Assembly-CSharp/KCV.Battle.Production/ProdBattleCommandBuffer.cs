using Common.Enum;
using KCV.Battle.Utils;
using KCV.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	public class ProdBattleCommandBuffer : InstantiateObject<ProdBattleCommandBuffer>
	{
		[SerializeField]
		private Transform _prefabUIBufferFleetCircle;

		[SerializeField]
		private Transform _prefabUIBufferShipCircle;

		[SerializeField]
		private BattleCommand _iCommand = BattleCommand.None;

		[SerializeField]
		[Range(0f, 4f)]
		private int _nBufferCnt;

		[Button("LoadPrefab", "コマンドプレハブ読み込み", new object[]
		{

		})]
		[SerializeField]
		private int _nLoadPrefab;

		private bool _isBufferObjectDeployment;

		private EffectModel _clsEffectModel;

		private Action _actOnFinished;

		public static ProdBattleCommandBuffer Instantiate(ProdBattleCommandBuffer prefab, Transform parent, EffectModel buffer, int nBufferCnt)
		{
			ProdBattleCommandBuffer prodBattleCommandBuffer = InstantiateObject<ProdBattleCommandBuffer>.Instantiate(prefab, parent);
			prodBattleCommandBuffer.Init(buffer, nBufferCnt);
			return prodBattleCommandBuffer;
		}

		private void OnDestroy()
		{
			Mem.Del(ref _prefabUIBufferFleetCircle);
			Mem.Del(ref _prefabUIBufferShipCircle);
			Mem.Del(ref _iCommand);
			Mem.Del(ref _nLoadPrefab);
			Mem.Del(ref _isBufferObjectDeployment);
			Mem.Del(ref _clsEffectModel);
			Mem.Del(ref _actOnFinished);
		}

		private bool Init(EffectModel model, int nBufferCnt)
		{
			_nBufferCnt = nBufferCnt;
			ProdBufferEffect prodBufferEffect = BattleTaskManager.GetPrefabFile().prodBufferEffect;
			prodBufferEffect.Init();
			prodBufferEffect.SetEffectData(model);
			_clsEffectModel = model;
			BufferObjectDeployment();
			return true;
		}

		public void Play(Action onFinished)
		{
			_actOnFinished = onFinished;
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.SetStandingPosition(StandingPositionType.CommandBuffer);
			PlayCommand(_clsEffectModel.Command);
		}

		private void PlayCommand(BattleCommand iCommand)
		{
			switch (iCommand)
			{
			case BattleCommand.None:
				OnFinished(null);
				break;
			case BattleCommand.Sekkin:
			case BattleCommand.Raigeki:
			{
				ProdBufferClose pbc = (!base.transform.GetComponentInChildren<ProdBufferClose>()) ? Util.Instantiate(LoadBufferPrefab(BattleCommand.Sekkin), base.gameObject).GetComponent<ProdBufferClose>() : base.transform.GetComponentInChildren<ProdBufferClose>();
				pbc.Play(PlayBufferEffect, CalcInitLineRotation, PlayLineAnimation, PlayNextFocusShipCircleAnimation, _nBufferCnt).Subscribe(delegate
				{
					OnFinished(pbc.gameObject);
				});
				break;
			}
			case BattleCommand.Hougeki:
			{
				ProdBufferShelling pbh = (!base.transform.GetComponentInChildren<ProdBufferShelling>()) ? Util.Instantiate(LoadBufferPrefab(BattleCommand.Hougeki), base.gameObject).GetComponent<ProdBufferShelling>() : base.transform.GetComponentInChildren<ProdBufferShelling>();
				pbh.Play(PlayBufferEffect, CalcInitLineRotation, PlayLineAnimation, PlayNextFocusShipCircleAnimation, _nBufferCnt).Subscribe(delegate
				{
					OnFinished(pbh.gameObject);
				});
				break;
			}
			case BattleCommand.Taisen:
			{
				ProdBufferAntiSubmarine pbas = (!base.transform.GetComponentInChildren<ProdBufferAntiSubmarine>()) ? Util.Instantiate(LoadBufferPrefab(BattleCommand.Taisen), base.gameObject).GetComponent<ProdBufferAntiSubmarine>() : base.transform.GetComponentInChildren<ProdBufferAntiSubmarine>();
				pbas.Play(PlayBufferEffect, CalcInitLineRotation, PlayLineAnimation, PlayNextFocusShipCircleAnimation, _nBufferCnt).Subscribe(delegate
				{
					OnFinished(pbas.gameObject);
				});
				break;
			}
			case BattleCommand.Ridatu:
			case BattleCommand.Kaihi:
			{
				ProdBufferAvoidance pbad = (!base.transform.GetComponentInChildren<ProdBufferAvoidance>()) ? Util.Instantiate(LoadBufferPrefab(BattleCommand.Kaihi), base.gameObject).GetComponent<ProdBufferAvoidance>() : base.transform.GetComponentInChildren<ProdBufferAvoidance>();
				pbad.Play(PlayBufferEffect, CalcInitLineRotation, PlayLineAnimation, PlayNextFocusShipCircleAnimation, _nBufferCnt).Subscribe(delegate
				{
					OnFinished(pbad.gameObject);
				});
				break;
			}
			case BattleCommand.Kouku:
			{
				ProdBufferAviation pba = (!base.transform.GetComponentInChildren<ProdBufferAviation>()) ? Util.Instantiate(LoadBufferPrefab(BattleCommand.Kouku), base.gameObject).GetComponent<ProdBufferAviation>() : base.transform.GetComponentInChildren<ProdBufferAviation>();
				pba.Play(PlayBufferEffect, CalcInitLineRotation, PlayLineAnimation, PlayNextFocusShipCircleAnimation, _nBufferCnt).Subscribe(delegate
				{
					OnFinished(pba.gameObject);
				});
				break;
			}
			case BattleCommand.Totugeki:
			{
				ProdBufferAssault pba2 = (!base.transform.GetComponentInChildren<ProdBufferAssault>()) ? Util.Instantiate(LoadBufferPrefab(BattleCommand.Totugeki), base.gameObject).GetComponent<ProdBufferAssault>() : base.transform.GetComponentInChildren<ProdBufferAssault>();
				pba2.Init(PlayLookAtLineAssult);
				pba2.Play(PlayBufferEffect, CalcInitLineRotation, PlayLineAnimation, PlayNextFocusShipCircleAnimation, _nBufferCnt).Subscribe(delegate
				{
					OnFinished(pba2.gameObject);
				});
				break;
			}
			case BattleCommand.Tousha:
			{
				ProdBufferUnifiedFire pbuf = (!base.transform.GetComponentInChildren<ProdBufferUnifiedFire>()) ? Util.Instantiate(LoadBufferPrefab(BattleCommand.Tousha), base.gameObject).GetComponent<ProdBufferUnifiedFire>() : base.transform.GetComponentInChildren<ProdBufferUnifiedFire>();
				pbuf.Init(PlayLookAtLine);
				pbuf.Play(PlayBufferEffect, CalcInitLineRotation, PlayLineAnimation, PlayNextFocusShipCircleAnimation, _nBufferCnt).Subscribe(delegate
				{
					OnFinished(pbuf.gameObject);
				});
				break;
			}
			}
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Executions();
			BattleTaskManager.GetTorpedoHpGauges().Hide();
		}

		private void CalcInitLineRotation()
		{
			BattleField field = BattleTaskManager.GetBattleField();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.bufferShipCirlce[0].ForEach(delegate(UIBufferCircle x)
			{
				x.CalcInitLineRotation(field.dicFleetAnchor[FleetType.Enemy]);
			});
			battleShips.bufferShipCirlce[1].ForEach(delegate(UIBufferCircle x)
			{
				x.CalcInitLineRotation(field.dicFleetAnchor[FleetType.Friend]);
			});
		}

		private void PlayLineAnimation()
		{
			BattleTaskManager.GetBattleField();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.bufferShipCirlce[0].ForEach(delegate(UIBufferCircle x)
			{
				x.PlayLineAnimation();
			});
			battleShips.bufferShipCirlce[1].ForEach(delegate(UIBufferCircle x)
			{
				x.PlayLineAnimation();
			});
		}

		private void PlayLookAtLine()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_056);
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.bufferFleetCircle[0].PlayRipple();
			battleShips.bufferShipCirlce[0].ForEach(delegate(UIBufferCircle x)
			{
				x.PlayLookAtLine();
			});
		}

		private void PlayLookAtLineAssult()
		{
			KCV.Utils.SoundUtils.PlaySE(SEFIleInfos.SE_056);
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.bufferShipCirlce[0].ForEach(delegate(UIBufferCircle x)
			{
				x.PlayLookAtLine2Assult();
			});
		}

		private void PlayBufferEffect()
		{
			ProdBufferEffect prodBufferEffect = BattleTaskManager.GetPrefabFile().prodBufferEffect;
			prodBufferEffect.Play(null);
		}

		private void PlayNextFocusShipCircleAnimation()
		{
			if (_clsEffectModel.NextActionShip != null)
			{
				ShipModel_Battle focusShip = _clsEffectModel.NextActionShip;
				BattleShips battleShips = BattleTaskManager.GetBattleShips();
				int cnt = 0;
				battleShips.bufferShipCirlce[0].ForEach(delegate(UIBufferCircle x)
				{
					x.PlayFocusCircleAnimation(focusShip.IsFriend() && focusShip.Index == cnt);
					cnt++;
				});
				cnt = 0;
				battleShips.bufferShipCirlce[1].ForEach(delegate(UIBufferCircle x)
				{
					x.PlayFocusCircleAnimation(!focusShip.IsFriend() && focusShip.Index == cnt);
					cnt++;
				});
				List<UIBufferFleetCircle> bufferFleetCircle = battleShips.bufferFleetCircle;
				bufferFleetCircle.ForEach(delegate(UIBufferFleetCircle x)
				{
					x.PlayFocusCircleAnimation();
				});
			}
		}

		private GameObject LoadBufferPrefab(BattleCommand Command)
		{
			string arg = string.Empty;
			switch (Command)
			{
			case BattleCommand.None:
				return null;
			case BattleCommand.Sekkin:
				arg = "Close";
				break;
			case BattleCommand.Hougeki:
				arg = "Shelling";
				break;
			case BattleCommand.Raigeki:
				arg = "TorpedoSalvo";
				break;
			case BattleCommand.Ridatu:
				arg = "Withdrawal";
				break;
			case BattleCommand.Taisen:
				arg = "AntiSubmarine";
				break;
			case BattleCommand.Kaihi:
				arg = "Avoidance";
				break;
			case BattleCommand.Kouku:
				arg = "Aviation";
				break;
			case BattleCommand.Totugeki:
				arg = "Assault";
				break;
			case BattleCommand.Tousha:
				arg = "UnifiedFire";
				break;
			}
			return Resources.Load($"Prefabs/Battle/Production/Command/ProdBuffer{arg}") as GameObject;
		}

		private void BufferObjectDeployment()
		{
			if (!_isBufferObjectDeployment)
			{
				_isBufferObjectDeployment = true;
				BattleShips battleShips = BattleTaskManager.GetBattleShips();
				battleShips.RadarDeployment(isDeploy: true);
			}
		}

		public void BufferObjectConvergence()
		{
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.RadarDeployment(isDeploy: false);
		}

		private void OnFinished(GameObject destroyObject)
		{
			if (destroyObject != null)
			{
				UnityEngine.Object.Destroy(destroyObject);
			}
			_isBufferObjectDeployment = false;
			Dlg.Call(ref _actOnFinished);
		}
	}
}
