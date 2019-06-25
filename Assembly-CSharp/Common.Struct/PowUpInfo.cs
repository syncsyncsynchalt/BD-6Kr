using System;

namespace Common.Struct
{
	public struct PowUpInfo
	{
		public int Taikyu;

		public int Karyoku;

		public int Raisou;

		public int Taiku;

		public int Soukou;

		public int Taisen;

		public int Lucky;

		public int Kaihi;

		public bool IsAllZero()
		{
			return Taikyu == 0 && Karyoku == 0 && Raisou == 0 && Taiku == 0 && Soukou == 0 && Taisen == 0 && Lucky == 0 && Kaihi == 0;
		}

		public bool HasPositive()
		{
			return Taikyu > 0 || Karyoku > 0 || Raisou > 0 || Taiku > 0 || Soukou > 0 || Taisen > 0 || Lucky > 0 || Kaihi > 0;
		}

		public void RemoveNegative()
		{
			Taikyu = Math.Max(0, Taikyu);
			Karyoku = Math.Max(0, Karyoku);
			Raisou = Math.Max(0, Raisou);
			Taiku = Math.Max(0, Taiku);
			Soukou = Math.Max(0, Soukou);
			Taisen = Math.Max(0, Taisen);
			Lucky = Math.Max(0, Lucky);
			Kaihi = Math.Max(0, Kaihi);
		}
	}
}
