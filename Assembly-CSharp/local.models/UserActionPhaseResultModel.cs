using Server_Common.Formats;

namespace local.models
{
	public class UserActionPhaseResultModel : PhaseResultModel
	{
		public UserActionPhaseResultModel(TurnWorkResult data)
			: base(data)
		{
		}

		public override string ToString()
		{
			return $"[ユーザー行動フェーズ]: ";
		}
	}
}
