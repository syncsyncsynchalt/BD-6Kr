using Common.Enum;
using Common.Struct;
using Server_Common.Formats;

namespace local.models
{
	public class HistoryModelBase
	{
		protected User_HistoryFmt _fmt;

		public HistoryType Type => _fmt.Type;

		public TurnString DateStruct => _fmt.DateString;

		public HistoryModelBase(User_HistoryFmt fmt)
		{
			_fmt = fmt;
		}
	}
}
