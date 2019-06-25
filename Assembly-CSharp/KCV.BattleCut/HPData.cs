namespace KCV.BattleCut
{
	public class HPData
	{
		private int _nMaxHP;

		private int _nStartHP;

		private int _nNowHP;

		private int _nNextHP;

		private int _nEndHP;

		private int _nAttackCnt = 4;

		private int[] _nOneAttackDamage;

		private int _nHPDifFmNow2End;

		public int maxHP
		{
			get
			{
				return _nMaxHP;
			}
			set
			{
				_nMaxHP = value;
			}
		}

		public int startHP
		{
			get
			{
				return _nStartHP;
			}
			set
			{
				_nStartHP = value;
			}
		}

		public int nowHP
		{
			get
			{
				return _nNowHP;
			}
			set
			{
				_nNowHP = value;
			}
		}

		public int nextHP
		{
			get
			{
				return _nNextHP;
			}
			set
			{
				_nNextHP = value;
			}
		}

		public int endHP
		{
			get
			{
				return _nEndHP;
			}
			set
			{
				_nEndHP = value;
			}
		}

		public int attackCnt
		{
			get
			{
				return _nAttackCnt;
			}
			set
			{
				_nAttackCnt = value;
			}
		}

		public int[] oneAttackDamage
		{
			get
			{
				return _nOneAttackDamage;
			}
			set
			{
				_nOneAttackDamage = value;
			}
		}

		public HPData(int maxHp, int nowHp)
		{
			maxHP = maxHp;
			startHP = nowHp;
			nowHP = nowHp;
			nextHP = nowHp;
			oneAttackDamage = new int[attackCnt];
		}

		public void SetEndHP(int endHP)
		{
			this.endHP = endHP;
			_nHPDifFmNow2End = nowHP - endHP;
			CalcOneAttackDamage();
		}

		public void ClearOneAttackDamage()
		{
			_nOneAttackDamage = new int[attackCnt];
		}

		private void CalcOneAttackDamage()
		{
			for (int i = 0; i < oneAttackDamage.Length - 1; i++)
			{
				oneAttackDamage[i] = XorRandom.GetILim(0, (int)((float)_nHPDifFmNow2End * 0.5f));
				_nHPDifFmNow2End -= oneAttackDamage[i];
			}
			oneAttackDamage[oneAttackDamage.Length - 1] = _nHPDifFmNow2End;
		}
	}
}
