using Server_Common.Formats;

namespace local.models
{
	public class __DutyModel__ : DutyModel
	{
		public User_QuestFmt Fmt => _fmt;

		public __DutyModel__(User_QuestFmt fmt)
			: base(fmt)
		{
		}
	}
}
