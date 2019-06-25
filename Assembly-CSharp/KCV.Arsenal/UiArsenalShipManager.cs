using Common.Enum;
using local.models;
using System;
using UnityEngine;

namespace KCV.Arsenal
{
	public class UiArsenalShipManager : MonoBehaviour
	{
		private const int NORMAL_SHIP_COUNT = 5;

		private const int LARGE_SHIP_COUNT = 5;

		private const int TANKER_SHIP_COUNT = 5;

		[SerializeField]
		private GameObject _mini1Obj;

		[SerializeField]
		private GameObject _mini2Obj;

		[SerializeField]
		private GameObject _mini3Obj;

		[SerializeField]
		private UISprite[] _ship1;

		[SerializeField]
		private UISprite[] _ship2;

		[SerializeField]
		private UISprite[] _ship3;

		private int _nowBuildCount;

		private int _fullCount;

		private ShipModelMst _ship;

		private BuildDockModel _dock;

		public void init(int num)
		{
			_mini1Obj = base.transform.FindChild("ShipType1").gameObject;
			_mini2Obj = base.transform.FindChild("ShipType2").gameObject;
			_mini3Obj = base.transform.FindChild("ShipType3").gameObject;
			_ship1 = new UISprite[5];
			_ship2 = new UISprite[5];
			_ship3 = new UISprite[5];
			for (int i = 0; i < 5; i++)
			{
				_ship1[i] = ((Component)_mini1Obj.transform.FindChild("Ship" + (i + 1))).GetComponent<UISprite>();
				_ship1[i].alpha = 1f;
				_ship1[i].transform.SetActive(isActive: false);
			}
			for (int j = 0; j < 5; j++)
			{
				_ship2[j] = ((Component)_mini2Obj.transform.FindChild("Ship" + (j + 1))).GetComponent<UISprite>();
				_ship2[j].transform.SetActive(isActive: false);
			}
			for (int k = 0; k < 5; k++)
			{
				_ship3[k] = ((Component)_mini3Obj.transform.FindChild("Ship" + (k + 1))).GetComponent<UISprite>();
				_ship3[k].transform.SetActive(isActive: false);
			}
		}

		public void set(ShipModelMst ship, BuildDockModel dock, bool isHight)
		{
			_ship = ship;
			_dock = dock;
			_fullCount = _dock.CompleteTurn - _dock.StartTurn;
			_nowBuildCount = _fullCount - _dock.GetTurn();
			if (_dock.State == KdockStates.COMPLETE)
			{
				if (isHight)
				{
					SetFirstShip();
				}
				else
				{
					SetShipCmp();
				}
			}
			else
			{
				SetShip();
			}
		}

		private void SetFirstShip()
		{
			int num = 0;
			int[] array = new int[5];
			int num2;
			int num3;
			UISprite[] array2;
			if (_dock.IsTunker())
			{
				num2 = (int)Math.Ceiling((double)_fullCount / 5.0);
				num3 = 5;
				array2 = _ship3;
			}
			else
			{
				num2 = (int)Math.Ceiling((double)_fullCount / (double)_ship.BuildStep);
				num3 = _ship.BuildStep;
				array2 = _ship1;
			}
			if (num2 == 0)
			{
				num = _nowBuildCount;
				if (num <= num3)
				{
				}
			}
			else
			{
				for (int i = 0; i < 5; i++)
				{
					array[i] = i * num2;
				}
				for (int j = 0; j < 5; j++)
				{
					if (j != 0 && array[j] <= _nowBuildCount)
					{
						num++;
					}
					if (num > num3)
					{
						break;
					}
				}
			}
			array2[0].transform.SetActive(isActive: true);
		}

		private void SetShip()
		{
			int num = 0;
			int[] array = new int[5];
			int num2;
			int num3;
			UISprite[] array2;
			if (_dock.IsTunker())
			{
				num2 = 5;
				num3 = (int)Math.Ceiling((double)_fullCount / (double)num2);
				array2 = _ship3;
			}
			else
			{
				num3 = (int)Math.Ceiling((double)_fullCount / (double)_ship.BuildStep);
				num2 = _ship.BuildStep;
				array2 = _ship1;
			}
			if (num3 == 0)
			{
				num = _nowBuildCount;
				if (num > num2)
				{
					num = num2;
				}
			}
			else
			{
				for (int i = 0; i < 5; i++)
				{
					array[i] = i * num3;
				}
				for (int j = 0; j < 5; j++)
				{
					if (j != 0 && array[j] <= _nowBuildCount)
					{
						num++;
					}
					if (num > num2)
					{
						num = num2;
						break;
					}
				}
			}
			for (int k = 0; k < 5; k++)
			{
				if (num >= k)
				{
					array2[k].transform.SetActive(isActive: true);
				}
			}
		}

		private void SetShipCmp()
		{
			UISprite[] array;
			int num;
			if (_dock.IsTunker())
			{
				array = _ship3;
				num = 4;
			}
			else
			{
				array = _ship1;
				num = _ship.BuildStep;
			}
			for (int i = 0; i < 5; i++)
			{
				if (num >= i)
				{
					array[i].transform.SetActive(isActive: true);
				}
			}
		}

		private void OnDestroy()
		{
			_mini1Obj = null;
			_mini2Obj = null;
			_mini3Obj = null;
			_ship1 = null;
			_ship2 = null;
			_ship3 = null;
			_ship = null;
			_dock = null;
		}
	}
}
