using local.models;
using Server_Controllers;
using System;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyDebugMenu2 : MonoBehaviour
	{
		public StrategyDebugButton[] buttons;

		public KeyControl key;

		public UIButtonManager BtnManager;

		public UIButtonManager MagBtnManager;

		private ShipModel ship;

		private Debug_Mod debugMod;

		private int mag;

		private void Update()
		{
			key.Update();
			buttons[BtnManager.nowForcusIndex].Act();
		}

		private void Start()
		{
			BtnManager.setFocus(0);
			buttons[0].Act = ShipSelect;
			buttons[1].Act = ShipLevel;
			buttons[2].Act = ShipStrength;
			key = new KeyControl();
			mag = 1;
			ship = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(1);
			ShipInit();
			debugMod = new Debug_Mod();
		}

		public void SetMag()
		{
			switch (MagBtnManager.nowForcusIndex)
			{
			case 0:
				mag = 1;
				break;
			case 1:
				mag = 10;
				break;
			case 2:
				mag = 100;
				break;
			case 3:
				mag = 1000;
				break;
			}
		}

		private void ShipSelect()
		{
			if (key.IsRightDown() || key.IsLeftDown())
			{
				int num = Convert.ToInt32(buttons[0].labels[0].text);
				num = ((!key.IsRightDown()) ? ((int)Util.RangeValue(num - 1 * mag, 0f, 500f)) : ((int)Util.RangeValue(num + 1 * mag, 0f, 500f)));
				buttons[0].labels[0].textInt = num;
				ship = StrategyTopTaskManager.GetLogicManager().UserInfo.GetShip(num);
				ShipInit();
			}
		}

		private void ShipInit()
		{
			buttons[0].labels[1].text = ((ship == null) ? "NONE" : ship.Name);
			buttons[1].labels[0].text = ((ship == null) ? "NONE" : ("LV : " + ship.Level.ToString()));
			buttons[2].labels[0].text = ((ship == null) ? "NONE" : string.Empty);
			buttons[2].labels[1].text = ((ship == null) ? "NONE" : ("耐久\n" + ship.Taikyu.ToString()));
			buttons[2].labels[2].text = ((ship == null) ? "NONE" : ("火力\n" + ship.Karyoku.ToString()));
			buttons[2].labels[3].text = ((ship == null) ? "NONE" : ("対空\n" + ship.Taiku.ToString()));
			buttons[2].labels[4].text = ((ship == null) ? "NONE" : ("対潜\n" + ship.Taisen.ToString()));
			buttons[2].labels[5].text = ((ship == null) ? "NONE" : ("運\n" + ship.Lucky.ToString()));
		}

		private void ShipLevel()
		{
			if (key.IsRightDown())
			{
				int level = ship.Level;
				level = (int)Util.RangeValue(level + 1 * mag, 1f, 150f);
				for (int i = ship.Level; i <= level; i++)
				{
					ship.AddExp(ship.Exp_Next);
				}
				ShipInit();
			}
		}

		private void ShipStrength()
		{
		}
	}
}
