using KCV.Utils;
using local.models;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Startup
{
	public class TaskStartupFirstShipSelect : SceneTaskMono
	{
		private CtrlStarterSelect _ctrlStarterSelect;

		private CtrlPartnerSelect _ctrlPartnerSelect;

		private StatementMachine _clsState;

		private bool _shipCancelled;

		private bool isCached;

		public CtrlPartnerSelect ctrlPartnerSelect => _ctrlPartnerSelect;

		protected override bool Init()
		{
			if (_ctrlPartnerSelect == null)
			{
				_ctrlPartnerSelect = GameObject.Find("PartnerShip").GetComponent<CtrlPartnerSelect>();
			}
			if (_ctrlStarterSelect == null)
			{
				_ctrlStarterSelect = GameObject.Find("CtrlStarterSelect").GetComponent<CtrlStarterSelect>();
			}
			_shipCancelled = false;
			_clsState = new StatementMachine();
			_clsState.AddState(InitStarterSelect, UpdateStarterSelect);
			return true;
		}

		protected override bool UnInit()
		{
			if (_clsState != null)
			{
				_clsState.Clear();
			}
			return true;
		}

		protected override bool Run()
		{
			if (_clsState != null)
			{
				_clsState.OnUpdate(Time.deltaTime);
			}
			if (StartupTaskManager.GetMode() != StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_BEF)
			{
				return (StartupTaskManager.GetMode() == StartupTaskManager.StartupTaskManagerMode.FirstShipSelect) ? true : false;
			}
			return true;
		}

		private bool InitStarterSelect(object data)
		{
			UIStartupHeader startupHeader = StartupTaskManager.GetStartupHeader();
			startupHeader.SetMessage("ゲーム開始");
			_ctrlStarterSelect.Init(OnStarterSelected, OnStarterSelectCancel);
			_ctrlPartnerSelect.SetActive(isActive: false);
			StartCache(null);
			return false;
		}

		private bool UpdateStarterSelect(object data)
		{
			KeyControl keyControl = StartupTaskManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.MARU))
			{
				_ctrlStarterSelect.OnClickStarter(_ctrlStarterSelect.selectType);
				return true;
			}
			if (keyControl.GetDown(KeyControl.KeyName.BATU))
			{
				_ctrlStarterSelect.OnCancel();
				return true;
			}
			if (keyControl.GetDown(KeyControl.KeyName.LEFT))
			{
				_ctrlStarterSelect.PreparaNext(isFoward: false);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
			{
				_ctrlStarterSelect.PreparaNext(isFoward: true);
			}
			return false;
		}

		private void OnStarterSelected(CtrlStarterSelect.StarterType iType)
		{
			_ctrlPartnerSelect.SetStarter(iType);
			_clsState.Clear();
			_clsState.AddState(InitPartnerSelect, UpdatePartnerSelect);
		}

		private void OnStarterSelectCancel()
		{
			StartupTaskManager.ReqMode(StartupTaskManager.StartupTaskManagerMode.StartupTaskManagerMode_ST);
		}

		private bool InitPartnerSelect(object data)
		{
			UIStartupHeader startupHeader = StartupTaskManager.GetStartupHeader();
			startupHeader.SetMessage("初期艦選択");
			_ctrlPartnerSelect.SetActive(isActive: true);
			_ctrlPartnerSelect.Init(OnPartnerShipSelectFinished, OnPartnerShipCancel);
			return false;
		}

		private bool UpdatePartnerSelect(object data)
		{
			KeyControl keyControl = StartupTaskManager.GetKeyControl();
			if (keyControl.GetDown(KeyControl.KeyName.RIGHT))
			{
				_ctrlPartnerSelect.press_Button(CtrlPartnerSelect.ButtonIndex.R);
			}
			else if (keyControl.GetDown(KeyControl.KeyName.LEFT))
			{
				_ctrlPartnerSelect.press_Button(CtrlPartnerSelect.ButtonIndex.L);
			}
			else
			{
				if (keyControl.GetDown(KeyControl.KeyName.MARU))
				{
					return _ctrlPartnerSelect.OnDecidePartnerShip();
				}
				if (keyControl.GetDown(KeyControl.KeyName.BATU))
				{
					return _ctrlPartnerSelect.OnCancel();
				}
			}
			return false;
		}

		private void OnPartnerShipSelectFinished(ShipModelMst partnerShip)
		{
			XorRandom.Init(0u);
			StartupTaskManager.GetData().PartnerShipID = partnerShip.MstId;
			Observable.TimerFrame(10, FrameCountType.EndOfFrame).Subscribe(delegate
			{
				_ctrlPartnerSelect.Hide();
				StartupTaskManager.ReqMode(StartupTaskManager.StartupTaskManagerMode.PictureStoryShow);
			});
		}

		private void OnPartnerShipCancel()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			_ctrlPartnerSelect.SetActive(isActive: false);
			_shipCancelled = false;
			_clsState.Clear();
			_clsState.AddState(InitStarterSelect, UpdateStarterSelect);
		}

		public void StartCache(Action Onfinished)
		{
			isCached = false;
			_ctrlPartnerSelect.cachePreLoad();
		}
	}
}
