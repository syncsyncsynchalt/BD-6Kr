using Common.Enum;

namespace KCV.Startup
{
	public class StartupData
	{
		private string _strAdmiralName;

		private int _nPartnerShipID;

		private DifficultKind _iDifficultKind;

		private bool _isInherit;

		public string AdmiralName
		{
			get
			{
				return _strAdmiralName;
			}
			set
			{
				_strAdmiralName = value;
			}
		}

		public int PartnerShipID
		{
			get
			{
				return _nPartnerShipID;
			}
			set
			{
				_nPartnerShipID = value;
			}
		}

		public DifficultKind Difficlty
		{
			get
			{
				return _iDifficultKind;
			}
			set
			{
				_iDifficultKind = value;
			}
		}

		public bool isInherit
		{
			get
			{
				return _isInherit;
			}
			set
			{
				_isInherit = value;
			}
		}

		public StartupData()
		{
			_strAdmiralName = string.Empty;
			_nPartnerShipID = -1;
			_iDifficultKind = DifficultKind.OTU;
			_isInherit = false;
		}

		public bool UnInit()
		{
			Mem.Del(ref _strAdmiralName);
			Mem.Del(ref _nPartnerShipID);
			Mem.Del(ref _iDifficultKind);
			Mem.Del(ref _isInherit);
			return true;
		}
	}
}
