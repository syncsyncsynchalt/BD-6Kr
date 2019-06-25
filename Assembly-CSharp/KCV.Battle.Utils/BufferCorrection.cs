using System;

namespace KCV.Battle.Utils
{
	public struct BufferCorrection : IDisposable
	{
		private BufferCorrectionType _iType;

		private int _nCorrectionFactor;

		public BufferCorrectionType type => _iType;

		public int collectionFactor => _nCorrectionFactor;

		public BufferCorrection(BufferCorrectionType iType, int nCorrectionFactor)
		{
			_iType = iType;
			_nCorrectionFactor = nCorrectionFactor;
		}

		public void Dispose()
		{
			Mem.Del(ref _iType);
			Mem.Del(ref _nCorrectionFactor);
		}
	}
}
