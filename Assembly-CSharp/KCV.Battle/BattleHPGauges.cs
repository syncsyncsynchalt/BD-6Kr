using Common.Enum;
using KCV.Battle.Production;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleHPGauges : IDisposable
	{
		private List<UICircleHPGauge> _listHpGauge;

		private UICircleHPGauge _hpGauge;

		private int _count;

		public BattleHPGauges()
		{
			_listHpGauge = new List<UICircleHPGauge>();
			_hpGauge = null;
		}

		public string DRF()
		{
			if (_listHpGauge == null)
			{
				return "null";
			}
			return " " + _listHpGauge.Count;
		}

		public bool Init()
		{
			foreach (UICircleHPGauge item in _listHpGauge)
			{
				if (item != null)
				{
					item.transform.localScale = Vector3.zero;
				}
			}
			return true;
		}

		public void Dispose()
		{
			if (_listHpGauge != null)
			{
				foreach (UICircleHPGauge item in _listHpGauge)
				{
					if (item != null)
					{
						item.gameObject.Discard();
					}
				}
				_listHpGauge.Clear();
			}
			_listHpGauge = null;
		}

		public void AddInstantiates(GameObject obj, bool isFriend, bool isLight, bool isT, bool isNumber)
		{
			_listHpGauge.Add(_instantiate(obj, isFriend, isLight, isT, isNumber));
		}

		public void AddInstantiatesSafe(GameObject obj, bool isFriend, bool isLight, bool isT, bool isNumber, int index)
		{
			if (index + 1 > _listHpGauge.Count)
			{
				_listHpGauge.Add(_instantiate(obj, isFriend, isLight, isT, isNumber));
			}
		}

		public bool SetGauge(int index, bool isFriend, bool isLight, bool isT, bool isNumber)
		{
			if (_listHpGauge[index] == null)
			{
				return false;
			}
			_listHpGauge[index].SetTextureType(isLight);
			if (isNumber)
			{
				_listHpGauge[index].SetShipNumber(index + 1, isFriend, isT);
			}
			return true;
		}

		public void SetHp(int num, int maxHp, int nowHp, int toHp, int damage, BattleHitStatus status, bool isFriend)
		{
			if (!(_listHpGauge[num] == null))
			{
				_listHpGauge[num].SetHPGauge(maxHp, nowHp, toHp, damage, status, isFriend);
			}
		}

		public void SetDamagePosition(int num, Vector3 vec)
		{
			if (_listHpGauge[num] != null)
			{
				_listHpGauge[num].SetDamagePosition(vec);
			}
		}

		public Vector3 GetDamagePosition(int num)
		{
			if (_listHpGauge[num] != null)
			{
				return _listHpGauge[num].GetDamagePosition();
			}
			return Vector3.zero;
		}

		public void ShowAll(Vector3 scale, bool isScale)
		{
			foreach (UICircleHPGauge item in _listHpGauge)
			{
				if (item != null)
				{
					item.SetTextureScale(scale, isScale);
				}
			}
		}

		public void Show(int num, Vector3 pos, Vector3 scale, bool isScale)
		{
			if (_listHpGauge[num] != null)
			{
				_listHpGauge[num].transform.localPosition = pos;
				_listHpGauge[num].SetTextureScale(scale, isScale);
			}
		}

		public void PlayHpAll(Action callBack)
		{
			foreach (UICircleHPGauge item in _listHpGauge)
			{
				if (item != null)
				{
					item.Plays(callBack);
				}
			}
		}

		public void PlayHp(int num, Action callBack)
		{
			if (_listHpGauge[num] != null)
			{
				_listHpGauge[num].Plays(callBack);
			}
		}

		public void PlayMiss(int num)
		{
			if (_listHpGauge[num] != null)
			{
				_listHpGauge[num].PlayMiss();
			}
		}

		public void PlayCritical(int num)
		{
			if (_listHpGauge[num] != null)
			{
				_listHpGauge[num].PlayCriticall();
			}
		}

		private UICircleHPGauge _instantiate(GameObject obj, bool isFriend, bool isLight, bool isT, bool isNumber)
		{
			if (_hpGauge == null)
			{
				_hpGauge = Resources.Load<UICircleHPGauge>("Prefabs/Battle/UI/UICircleHPGaugeS");
			}
			UICircleHPGauge component = Util.Instantiate(_hpGauge.gameObject, obj).GetComponent<UICircleHPGauge>();
			component.name = ((!isFriend) ? ("HpGaugeE" + _listHpGauge.Count) : ("HpGaugeF" + _listHpGauge.Count));
			component.SetTextureType(isLight);
			if (isNumber)
			{
				component.SetShipNumber(_listHpGauge.Count + 1, isFriend, isT);
			}
			return component;
		}
	}
}
