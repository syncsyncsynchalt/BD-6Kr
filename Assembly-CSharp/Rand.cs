using System;

public static class Rand
{
	private struct STR_RAND_WRK
	{
		public uint iSeed;

		public ulong iXorShiftX;

		public ulong iXorShiftY;

		public ulong iXorShiftZ;

		public ulong iXorShiftW;

		public override string ToString()
		{
			return $"STR_RAND_WRK->x(x.{iXorShiftX},y.{iXorShiftY},z.{iXorShiftZ},w.{iXorShiftW})";
		}
	}

	private static int _nWrk;

	private static STR_RAND_WRK[] _pWrk;

	public static bool Init(int nWrk = 1)
	{
		_nWrk = nWrk;
		Mem.NewAry(ref _pWrk, nWrk);
		ulong ticks = (ulong)DateTime.Now.Ticks;
		for (int i = 0; i < nWrk; i++)
		{
			SetSeed((uint)ticks, i);
		}
		return true;
	}

	public static bool UnInit()
	{
		Mem.DelAry(ref _pWrk);
		_nWrk = 0;
		return true;
	}

	public static void SetSeed(uint iSeed, int iWrk = 0)
	{
		_pWrk[iWrk].iSeed = iSeed;
		_pWrk[iWrk].iXorShiftW = _pWrk[iWrk].iSeed;
		_pWrk[iWrk].iXorShiftX = ((_pWrk[iWrk].iXorShiftW << 8) | (_pWrk[iWrk].iXorShiftW >> 8) | 0xBC614E);
		_pWrk[iWrk].iXorShiftY = (_pWrk[iWrk].iXorShiftX & _pWrk[iWrk].iXorShiftW);
		_pWrk[iWrk].iXorShiftZ = (_pWrk[iWrk].iXorShiftX ^ _pWrk[iWrk].iXorShiftY);
	}

	public static uint GetSeed(int iWrk = 0)
	{
		return _pWrk[iWrk].iSeed;
	}

	public static int Next()
	{
		return (int)GetIXorShift() & int.MaxValue;
	}

	public static int Next(int max)
	{
		return Next(0, max);
	}

	public static int Next(int min, int max)
	{
		return (Next(0) >> 1) % (max - min) + min;
	}

	public static byte GetB(int iWrk = 0)
	{
		return (byte)((byte)GetIXorShift(iWrk) & 0xFF);
	}

	public static sbyte GetSB(int iWrk = 0)
	{
		return (sbyte)((sbyte)GetIXorShift(iWrk) & 0x7F);
	}

	public static ulong GetC(int iWrk = 0)
	{
		return GetIXorShift(iWrk) & 0xFFFF;
	}

	public static short GetS(int iWrk = 0)
	{
		return (short)((short)GetIXorShift(iWrk) & 0x7FFF);
	}

	public static ushort GetUS(int iWrk = 0)
	{
		return (ushort)(GetIXorShift(iWrk) & 0xFFFF);
	}

	public static uint GetUI(int iWrk = 0)
	{
		return (uint)(GetIXorShift(iWrk) & uint.MaxValue);
	}

	public static int GetI(int iWrk = 0)
	{
		return (int)GetIXorShift(iWrk) & int.MaxValue;
	}

	public static int GetILim(int iMin, int iMax, int iWrk = 0)
	{
		float f = GetF01(iWrk);
		int a = (int)Mathe.Lerp(iMin, iMax, f);
		return Mathe.MinMax2(a, iMin, iMax);
	}

	public static bool GetIs(int iWrk = 0)
	{
		return (GetSB(iWrk) % 2 == 1) ? true : false;
	}

	public static int GetBI11(int iWrk = 0)
	{
		return GetIs(iWrk) ? 1 : (-1);
	}

	public static float GetBF11(int iWrk = 0)
	{
		return (!GetIs(iWrk)) ? (-1f) : 1f;
	}

	public static float GetF01(int iWrk = 0)
	{
		sbyte sB = GetSB(iWrk);
		float a = (float)sB / 127f;
		return Mathe.MinMax2F01(a);
	}

	public static float GetF11(int iWrk = 0)
	{
		float f = GetF01(iWrk);
		f *= 2f;
		f -= 1f;
		return Mathe.MinMax2F11(f);
	}

	public static float GetFLim(float fMin, float fMax, int iWrk = 0)
	{
		float f = GetF01(iWrk);
		f = Mathe.Lerp(fMin, fMax, f);
		return Mathe.MinMax2(f, fMin, fMax);
	}

	public static ulong GetIXorShift(int iWrk = 0)
	{
		ulong num = _pWrk[iWrk].iXorShiftX ^ (_pWrk[iWrk].iXorShiftX << 11);
		_pWrk[iWrk].iXorShiftX = _pWrk[iWrk].iXorShiftY;
		_pWrk[iWrk].iXorShiftZ = _pWrk[iWrk].iXorShiftW;
		_pWrk[iWrk].iXorShiftW = ((_pWrk[iWrk].iXorShiftW - (_pWrk[iWrk].iXorShiftW >> 19)) ^ (num ^ (num >> 8)));
		return _pWrk[iWrk].iXorShiftW;
	}

	private static STR_RAND_WRK GetWrk(int iWrk)
	{
		return _pWrk[iWrk];
	}
}
