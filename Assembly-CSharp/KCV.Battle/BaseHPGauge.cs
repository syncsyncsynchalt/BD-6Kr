using System;
using UnityEngine;

namespace KCV.Battle
{
	public class BaseHPGauge : BaseAnimation
	{
		protected int _nFromHP;

		protected int _nToHP;

		protected int _nMaxHP;

		protected int _nDamage;

		protected override void OnDestroy()
		{
			Mem.Del(ref _nFromHP);
			Mem.Del(ref _nToHP);
			Mem.Del(ref _nMaxHP);
			Mem.Del(ref _nDamage);
			base.OnDestroy();
		}

		public virtual void SetHPGauge(int maxHP, int beforeHP, int afterHP, Vector3 pos, Vector3 size)
		{
			_nMaxHP = maxHP;
			_nFromHP = beforeHP;
			_nToHP = afterHP;
			base.transform.localPosition = pos;
			base.transform.localScale = size;
		}

		public override void Play(Action callback)
		{
			base.Play(callback);
		}
	}
}
