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
			return $"[ユ\u30fcザ\u30fc行動フェ\u30fcズ]: ";
		}
	}
}
