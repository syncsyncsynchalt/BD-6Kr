using Common.Enum;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using Server_Controllers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Port.Repair
{
	public class board2 : MonoBehaviour
	{
		private UILabel ele_l;

		private UITexture ele_t;

		private UIScrollBar sb;

		private UIGrid uig;

		private ShipModel _clsShipModel;

		private repair rep;

		private RepairManager _clsRepair;

		private board1_top_mask bd1m;

		private ShipModel[] ships;

		private GameObject line;

		private board bd;

		private board3 bd3;

		private UIScrollBar scb;

		private KeyControl dockSelectController;

		private int cy;

		private ResourceManager _clsResource;

		private KeyScrollControl _clsScroll;

		private UIScrollListRepair rep_p;

		private SoundManager sm;

		[SerializeField]
		private UIShipSortButton SortButton;

		private bool _startup;

		private UISprite sprite;

		private bool _board2_anime;

		private int damage_flag;

		private bool _debug_shipping;

		public void CompleteHandler()
		{
			set_board2_anime(value: false);
		}

		public void CompleteHandlerStaticOn()
		{
			base.gameObject.isStatic = true;
			base.gameObject.GetComponent<UIPanel>().enabled = false;
			set_board2_anime(value: false);
		}

		public bool get_board2_anime()
		{
			return _board2_anime;
		}

		public void set_board2_anime(bool value)
		{
			_board2_anime = value;
		}

		private void Start()
		{
			set_board2_anime(value: false);
		}

		private void OnDestroy()
		{
			Mem.Del(ref ele_l);
			Mem.Del(ref ele_t);
			Mem.Del(ref sb);
			Mem.Del(ref uig);
			Mem.Del(ref _clsShipModel);
			Mem.Del(ref rep);
			Mem.Del(ref _clsRepair);
			Mem.Del(ref bd1m);
			Mem.Del(ref ships);
			Mem.Del(ref line);
			Mem.Del(ref bd);
			Mem.Del(ref bd3);
			Mem.Del(ref scb);
			Mem.Del(ref dockSelectController);
			Mem.Del(ref _clsScroll);
			Mem.Del(ref rep_p);
			Mem.Del(ref SortButton);
			Mem.Del(ref sprite);
		}

		public void _init_repair()
		{
			_startup = false;
			StartUp();
		}

		public void StartUp()
		{
			if (!_startup)
			{
				_startup = true;
				bd = GameObject.Find("board1_top/board").GetComponent<board>();
				bd3 = GameObject.Find("board3_top/board3").GetComponent<board3>();
				rep = ((Component)base.gameObject.transform.parent.parent).GetComponent<repair>();
				rep_p = ((Component)rep.transform.FindChild("board2_top/board2/UIScrollListRepair")).GetComponent<UIScrollListRepair>();
				Camera component = ((Component)rep.transform.FindChild("Camera")).GetComponent<Camera>();
				_clsScroll = new KeyScrollControl(6, 6, scb);
				_clsRepair = rep.now_clsRepair();
				damage_flag = 0;
				_debug_shipping = false;
				TweenPosition.Begin(base.gameObject, 0.01f, new Vector3(840f, 123f, -1f));
				ships = _clsRepair.GetShipList();
				dockSelectController = new KeyControl();
				dockSelectController.setChangeValue(0f, 0f, 0f, 0f);
				rep_p.SetCamera(component);
				rep_p.SetKeyController(dockSelectController);
				rep_p.ResumeControl();
				rep_p.SetOnSelectedListener(delegate(UIScrollListRepairChild child)
				{
					rep_p.keyController.ClearKeyAll();
					rep_p.LockControl();
					rep.set_mode(-2);
					if (child.GetModel() != null)
					{
						bd3.UpdateInfo(child.GetModel());
						bd3.board3_appear(bstat: true);
						rep.setmask(2, value: true);
						rep.set_mode(3);
					}
				});
				redraw();
			}
		}

		public void redraw()
		{
			redraw(val: false);
		}

		public void redraw(bool val)
		{
			rep.set_mode(2);
			UpdateList();
		}

		public void UpdateList()
		{
			rep_p.Initialize(ships);
		}

		private void Update()
		{
			if (!(rep == null) && rep.now_mode() == 2 && !get_board2_anime() && !bd3.get_board3_anime() && !rep.first_change())
			{
				dockSelectController.Update();
				if (dockSelectController.keyState[0].down)
				{
					dockSelectController.ClearKeyAll();
					dockSelectController.firstUpdate = true;
					Cancelled();
				}
			}
		}

		public void set_touch_mode(bool value)
		{
			if (value)
			{
				rep_p.StartControl();
			}
			else
			{
				rep_p.LockControl();
			}
		}

		public void Cancelled()
		{
			Cancelled(NotChangeMode: false, isSirent: false);
		}

		public void Cancelled(bool NotChangeMode)
		{
			Cancelled(NotChangeMode, isSirent: false);
		}

		public void Cancelled(bool NotChangeMode, bool isSirent)
		{
			if (rep.now_mode() == 2 || NotChangeMode)
			{
				rep.set_mode(-10);
				board2_appear(boardStart: false, isSirent);
			}
			if (!NotChangeMode)
			{
				rep.set_mode(1);
			}
		}

		public void board2_appear(bool boardStart)
		{
			board2_appear(boardStart, isSirent: false);
		}

		public void board2_appear(bool boardStart, bool isSirent)
		{
			if (boardStart)
			{
				base.gameObject.isStatic = false;
				base.gameObject.GetComponent<UIPanel>().enabled = true;
				SortButton.SetActive(isActive: true);
				rep_p.Initialize(ships);
				for (int i = 0; i < 4; i++)
				{
					if (bd.get_HS_anime(i))
					{
						rep.now_clsRepair().ChangeRepairSpeed(i);
						bd.set_anime(i, stat: false);
						bd.set_HS_anime(i, stat: false);
					}
				}
				set_board2_anime(value: true);
				TweenPosition tweenPosition = TweenPosition.Begin(base.gameObject, 0.35f, new Vector3(162f, 123f, -1f));
				tweenPosition.animationCurve = UtilCurves.TweenEaseOutExpo;
				tweenPosition.SetOnFinished(CompleteHandler);
				rep.set_mode(-2);
				rep.setmask(1, value: true);
				rep.set_mode(2);
			}
			else
			{
				set_board2_anime(value: true);
				TweenPosition tweenPosition2 = TweenPosition.Begin(base.gameObject, 0.3f, new Vector3(840f, 123f, -1f));
				tweenPosition2.animationCurve = UtilCurves.TweenEaseOutExpo;
				tweenPosition2.SetOnFinished(CompleteHandlerStaticOn);
				rep.setmask(1, value: false);
				rep.set_mode(1);
			}
			if (!isSirent)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			}
		}

		public void DBG_damage()
		{
			if (!_debug_shipping)
			{
				_debug_shipping = true;
				UserInfoModel userInfo = rep.now_clsRepair().UserInfo;
				List<ShipModel> p = userInfo.__GetShipList__();
				List<ShipModel> areaShips = rep.now_clsRepair().GetAreaShips(rep.NowArea(), p);
				if (rep.NowArea() == 1)
				{
					areaShips.AddRange(rep.now_clsRepair().GetDepotShips(p));
				}
				areaShips = DeckUtil.GetSortedList(areaShips, SortKey.DAMAGE);
				foreach (ShipModel item in areaShips)
				{
					int subvalue;
					switch (damage_flag)
					{
					case 0:
						subvalue = (int)Math.Ceiling((float)item.MaxHp * 0.25f);
						break;
					case 1:
						subvalue = (int)Math.Ceiling((float)item.MaxHp * 0.25f);
						break;
					case 2:
						subvalue = item.NowHp - 1;
						break;
					default:
						subvalue = item.NowHp - item.MaxHp;
						damage_flag = -1;
						break;
					}
					Debug_Mod.SubHp(item.MemId, subvalue);
				}
				damage_flag++;
				Mem.DelList(ref p);
				Mem.DelList(ref areaShips);
				CommonPopupDialog.Instance.StartPopup("DBG: 母港へ行きます。");
				SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
				_debug_shipping = false;
			}
		}

		internal void Resume()
		{
			rep_p.keyController.ClearKeyAll();
			rep_p.keyController.firstUpdate = true;
			rep_p.ResumeControl();
		}
	}
}
