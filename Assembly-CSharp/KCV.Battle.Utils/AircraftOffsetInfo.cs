using System;
using UnityEngine;

namespace KCV.Battle.Utils
{
	[Serializable]
	public struct AircraftOffsetInfo
	{
		[SerializeField]
		private int _nMstId;

		[SerializeField]
		private bool _isFlipHorizontal;

		[SerializeField]
		private float _fRot;

		[SerializeField]
		private Vector3 _vPos;

		public int mstID => _nMstId;

		public bool isFlipHorizontal => _isFlipHorizontal;

		public float rot => _fRot;

		public Vector3 pos => _vPos;

		public AircraftOffsetInfo(int mstID, bool flipHorizontal, float rot, Vector3 pos)
		{
			_nMstId = mstID;
			_isFlipHorizontal = flipHorizontal;
			_fRot = rot;
			_vPos = pos;
		}

		public override string ToString()
		{
			string empty = string.Empty;
			return empty + $"MstID:{mstID}|反転:{isFlipHorizontal}|回転:{rot}|位置:{pos}";
		}
	}
}
