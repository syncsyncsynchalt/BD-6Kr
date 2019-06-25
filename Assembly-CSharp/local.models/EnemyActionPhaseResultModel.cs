using Server_Common.Formats;

namespace local.models
{
	public class EnemyActionPhaseResultModel : PhaseResultModel
	{
		public EnemyActionPhaseResultModel(TurnWorkResult data)
			: base(data)
		{
		}

		public override string ToString()
		{
			return $"[敵行動フェ\u30fcズ]: ";
		}
	}
}
